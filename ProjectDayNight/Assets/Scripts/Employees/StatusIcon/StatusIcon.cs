using UnityEngine;
using System.Collections;

public class StatusIcon : MonoBehaviour
{
    [SerializeField] private StatusIconSprites statusIconSprites;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private float durationInOut = 0.3f;
    [SerializeField] private float easeFadeInOut;
    [Range(1f, 2f)]
    [SerializeField] private float maxBounceInOut = 1.5f;
    [SerializeField] private float continuousBounce = 1.2f;
    [SerializeField] private float bounceFrequency = 2f;

    // Sprite References
    public Sprite HappySprite => statusIconSprites.HappySprite;
    public Sprite NeutralSprite => statusIconSprites.NeutralSprite;
    public Sprite UpsetSprite => statusIconSprites.UpsetSprite;
    public Sprite SlackingOffSprite => statusIconSprites.SlackingOffSprite;
    public bool Active => gameObject.activeSelf;

    void Start()
    {
        transform.localScale = Vector3.zero;
        SetAlpha(0f);
        gameObject.SetActive(false);
    }

    public void SetSprite(Sprite sprite) => sr.sprite = sprite;

    private void SetAlpha(float alpha)
    {
        Color c = sr.color;
        c.a = alpha;
        sr.color = c;
    }

    public void Show()
    {
        if (Active) return;
        gameObject.SetActive(true);
        StartCoroutine(AnimatePopFadeIn(easeFadeInOut));
        StartCoroutine(AnimateBounce());
    }

    public void Hide()
    {
        if (!Active) return;
        StopAllCoroutines();
        StartCoroutine(AnimatePopFadeOut(easeFadeInOut));
    }

    public void ForceHide()
    {
        if (!Active) return;
        StopAllCoroutines();
        transform.localScale = Vector3.zero;
        SetAlpha(0f);
        gameObject.SetActive(false);
    }

    private IEnumerator AnimatePopFadeIn(float easeFadeOut = 1f)
    {
        yield return AnimatePopFade(true, maxBounceInOut, easeFadeOut);
    }

    private IEnumerator AnimatePopFadeOut(float easeFadeIn = 1f)
    {
        yield return AnimatePopFade(false, maxBounceInOut, easeFadeIn);
        gameObject.SetActive(false);
    }

    private IEnumerator AnimatePopFade(bool direction, float maxBounceScale, float easeFade = 1f)
    {
        // direction: true = in, false = out
        Vector3 startScale = direction ? Vector3.zero : Vector3.one;
        Vector3 endScale = direction ? Vector3.one : Vector3.zero;
        float startAlpha = direction ? 1f : 1f;
        float endAlpha = direction ? 1f : 1f;
        float unitScaleQ2Radians = Mathf.PI - Mathf.Asin(1 / maxBounceScale); // Quadrant 2, where localScale = 1
        if (!direction) easeFade = 1 / easeFade;

        transform.localScale = startScale;

        float t = 0f;
        while (t < durationInOut)
        {
            float progress = t / durationInOut;

            float scaleProgress = (Mathf.Sin(direction ? progress*unitScaleQ2Radians : unitScaleQ2Radians*(1-progress)) + 1) / 2;
            transform.localScale = Vector3.LerpUnclamped(Vector3.zero, Vector3.one * maxBounceScale, scaleProgress);

            // Alpha
            SetAlpha(Mathf.Lerp(startAlpha, endAlpha, Mathf.Pow(progress, 1/easeFade)));

            t += Time.deltaTime;
            yield return null;
        }

        transform.localScale = endScale;
        SetAlpha(endAlpha);
    }

    private IEnumerator AnimateBounce()
    {
        // Bounce indefinitely using continuousBounce
        float t = -continuousBounce / 2;
        while (true)
        {
            t += Time.deltaTime;
            float progress = (Mathf.Sin(t * Mathf.PI * 2 * bounceFrequency) + 1) / 2; // oscillates between 0 and 1
            transform.localScale = Vector3.LerpUnclamped(Vector3.one, Vector3.one * continuousBounce, progress);
            yield return null;
        }
    }
}