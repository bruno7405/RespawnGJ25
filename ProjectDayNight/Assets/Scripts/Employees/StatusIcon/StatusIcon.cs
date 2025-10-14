using UnityEngine;
using System.Collections;

public class StatusIcon : MonoBehaviour
{
    [SerializeField] private StatusIconSprites statusIconSprites;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private float durationInOut = 0.3f;
    [SerializeField] private float easeFadeInOut;
    [Range(1f, 2f)]
    [SerializeField] private float maxBounceScale = 1.2f;
    [SerializeField] private float bounceFrequency = 2f;

    // Sprite References
    public Sprite HappySprite => statusIconSprites.HappySprite;
    public Sprite NeutralSprite => statusIconSprites.NeutralSprite;
    public Sprite UpsetSprite => statusIconSprites.UpsetSprite;
    public Sprite SlackingOffSprite => statusIconSprites.SlackingOffSprite;
    public bool Active => gameObject.activeSelf;

    void Awake()
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
        StartCoroutine(AnimatePopFadeOut());
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
        yield return AnimatePopFade(Vector3.zero, Vector3.one, 1f, 1f, 1/easeFadeOut);
    }

    private IEnumerator AnimatePopFadeOut(float easeFadeIn = 1f)
    {
        yield return AnimatePopFade(Vector3.one, Vector3.zero, 1f, 1f, easeFadeIn);
        gameObject.SetActive(false);
    }

    private IEnumerator AnimatePopFade(Vector3 startScale, Vector3 endScale, float startAlpha, float endAlpha, float easeInOut = 1f)
    {
        float t = 0f;
        while (t < durationInOut)
        {
            float progress = t / durationInOut;

            float finalRadians = Mathf.PI - Mathf.Asin(1 / maxBounceScale); // Quadrant 2
            float scaleProgress = Mathf.Sin(progress * finalRadians);
            transform.localScale = Vector3.LerpUnclamped(startScale, endScale * maxBounceScale, scaleProgress);

            // Alpha
            SetAlpha(Mathf.Lerp(startAlpha, endAlpha, Mathf.Pow(progress, easeInOut)));

            t += Time.deltaTime;
            yield return null;
        }

        transform.localScale = endScale;
        SetAlpha(endAlpha);
    }

    private IEnumerator AnimateBounce()
    {
        // Bounce indefinitely using maxBounceScale
        float t = -bounceFrequency / 2;
        while (true)
        {
            t += Time.deltaTime;
            float progress = (Mathf.Sin(t * Mathf.PI * 2 * bounceFrequency) + 1) / 2; // oscillates between 0 and 1
            transform.localScale = Vector3.LerpUnclamped(Vector3.one, Vector3.one * maxBounceScale, progress);
            yield return null;
        }
    }
}