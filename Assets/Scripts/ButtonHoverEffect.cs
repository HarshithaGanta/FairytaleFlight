using UnityEngine;
using UnityEngine.EventSystems;

// Grows a button slightly on hover and plays the click sound.
public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public float hoverScale = 1.08f;
    public float speed = 12f;
    float target = 1f;

    void Update()
    {
        float s = Mathf.Lerp(transform.localScale.x, target, speed * Time.unscaledDeltaTime);
        transform.localScale = new Vector3(s, s, 1f);
    }

    public void OnPointerEnter(PointerEventData e) { target = hoverScale; }
    public void OnPointerExit(PointerEventData e) { target = 1f; }
    public void OnPointerClick(PointerEventData e) { AudioManager.Click(); }
}
