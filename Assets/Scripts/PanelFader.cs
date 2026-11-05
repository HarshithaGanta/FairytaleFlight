using System.Collections;
using UnityEngine;

// Fades a UI panel in whenever it is enabled (works while the game is paused).
[RequireComponent(typeof(CanvasGroup))]
public class PanelFader : MonoBehaviour
{
    public float duration = 0.35f;
    CanvasGroup group;

    void OnEnable()
    {
        group = GetComponent<CanvasGroup>();
        group.alpha = 0f;
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            group.alpha = Mathf.Clamp01(t / duration);
            yield return null;
        }
        group.alpha = 1f;
    }
}
