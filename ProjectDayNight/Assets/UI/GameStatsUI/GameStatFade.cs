using UnityEngine;
using TMPro;
using System.Collections;
public class GameStatFade : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI label;
    //[SerializeField] float fadeInDuration = 0.1f;   // quick pop-in
    [SerializeField]CanvasGroup cg;

    float lifeTime;      // set by spawner: 2 * statInterval

    public void UpdateUI(string text, Color color, float lifeTimeSeconds)
    {
        this.lifeTime = lifeTimeSeconds;

        label.text = text;
        label.color = color;

        cg.alpha = 1f;
        //StartCoroutine(FadeRoutine());
    }

    IEnumerator FadeRoutine()
    {
        // 1) Fade in
        // float t = 0f;
        // while (t < fadeInDuration)
        // {
        //     t += Time.unscaledDeltaTime;
        //     cg.alpha = Mathf.Clamp01(t / fadeInDuration);
        //     yield return null;
        // }
        // cg.alpha = 1f;

        // 2) Fade out so that total time since spawn == lifeTime
        float outDuration = lifeTime; // continuous fade across remaining time
        float u = 0f;
        while (u < outDuration)
        {
            u += Time.unscaledDeltaTime;
            float a = 1f - Mathf.Clamp01(u / outDuration);
            cg.alpha = a;
            yield return null;
        }

        Destroy(gameObject);
    }
}
