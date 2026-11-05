using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Lanes")]
    public float laneDistance = 2.5f;
    public float laneChangeSpeed = 20f;

    [Header("Jump")]
    public float jumpVelocity = 9.5f;
    public float gravity = 35f;
    public float groundCheckDistance = 0.25f;
    public float jumpBufferTime = 0.12f;

    [Header("Visuals")]
    public Transform model;
    public Animator animator;          // optional: assign when you import a real princess
    public float bobHeight = 0.08f;
    public float bobSpeed = 12f;

    Rigidbody rb;
    int currentLane = 1;               // 0 = left, 1 = middle, 2 = right
    bool isGrounded;
    float verticalVelocity;
    float jumpBuffer;
    bool hasRunParam, hasGroundedParam, hasJumpParam;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;                       // we apply our own gravity
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        if (animator == null && model != null) animator = model.GetComponentInChildren<Animator>();
        hasRunParam = HasParam("IsRunning");
        hasGroundedParam = HasParam("IsGrounded");
        hasJumpParam = HasParam("Jump");
    }

    bool HasParam(string paramName)
    {
        if (animator == null || animator.runtimeAnimatorController == null) return false;
        foreach (AnimatorControllerParameter p in animator.parameters)
            if (p.name == paramName) return true;
        return false;
    }

    void Update()
    {
        GameManager gm = GameManager.Instance;
        bool playing = gm == null || gm.State == GameState.Playing;

        if (playing)
        {
            if (InputHelper.LeftPressed()) currentLane = Mathf.Max(0, currentLane - 1);
            if (InputHelper.RightPressed()) currentLane = Mathf.Min(2, currentLane + 1);
            if (InputHelper.JumpPressed()) jumpBuffer = jumpBufferTime;
        }
        jumpBuffer -= Time.deltaTime;

        UpdateVisuals(gm, playing);
    }

    void FixedUpdate()
    {
        GameManager gm = GameManager.Instance;
        if (gm != null && gm.State != GameState.Playing)
        {
            // stop on pause / game over (if the princess is in mid-air at Game Over she falls down)
            if (gm.State == GameState.GameOver && !CheckGround())
            {
                verticalVelocity -= gravity * Time.fixedDeltaTime;
                rb.linearVelocity = new Vector3(0f, verticalVelocity, 0f);
            }
            else
            {
                verticalVelocity = 0f;
                rb.linearVelocity = Vector3.zero;
            }
            return;
        }

        float speed = gm != null ? gm.CurrentSpeed : 12f;
        float dt = Time.fixedDeltaTime;

        isGrounded = verticalVelocity <= 0.01f && CheckGround();

        if (isGrounded && jumpBuffer > 0f)
        {
            verticalVelocity = jumpVelocity;
            jumpBuffer = 0f;
            isGrounded = false;
            AudioManager.Jump();
            if (hasJumpParam) animator.SetTrigger("Jump");
        }
        else if (isGrounded)
        {
            verticalVelocity = -1f;
        }
        else
        {
            verticalVelocity -= gravity * dt;
        }

        float targetX = (currentLane - 1) * laneDistance;
        float newX = Mathf.MoveTowards(rb.position.x, targetX, laneChangeSpeed * dt);
        float vx = (newX - rb.position.x) / dt;

        rb.linearVelocity = new Vector3(vx, verticalVelocity, speed);
    }

    bool CheckGround()
    {
        Vector3 origin = rb.position + Vector3.up * 0.2f;
        return Physics.Raycast(origin, Vector3.down, 0.2f + groundCheckDistance,
                               Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
    }

    void UpdateVisuals(GameManager gm, bool playing)
    {
        if (model == null) return;

        if (gm != null && gm.State == GameState.GameOver)
        {
            model.localRotation = Quaternion.Slerp(model.localRotation, Quaternion.Euler(0f, 0f, 85f), 6f * Time.deltaTime);
            model.localPosition = Vector3.Lerp(model.localPosition, new Vector3(0f, 0.35f, 0f), 6f * Time.deltaTime);
            if (hasRunParam) animator.SetBool("IsRunning", false);
            return;
        }

        if (!playing)
        {
            AudioManager.Running(false);
            if (hasRunParam) animator.SetBool("IsRunning", false);
            return;
        }

        if (animator == null)
        {
            // simple "run" bounce for the placeholder princess
            float y = isGrounded ? Mathf.Abs(Mathf.Sin(Time.time * bobSpeed)) * bobHeight : 0f;
            model.localPosition = new Vector3(0f, y, 0f);
        }
        else
        {
            if (hasRunParam) animator.SetBool("IsRunning", true);
            if (hasGroundedParam) animator.SetBool("IsGrounded", isGrounded);
        }

        AudioManager.Running(isGrounded);
    }
}
