using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DayNightTransitionUI : MonoBehaviour
{
    [SerializeField] float transitionTime = 2f;   // Duration of rotation

    [Header("Gradients")]
    [SerializeField] Gradient backgroundGradient;
    [SerializeField] Gradient splashGradient;

    [Header("References")]
    [SerializeField] GameObject root;
    [SerializeField] RectTransform skybox;
    [SerializeField] Image splashArt;
    [SerializeField] Image background;

    private Coroutine transitionCoroutine;

    public void TransitionToDay(float transitionTime)
    {
        root.SetActive(true);
        skybox.localRotation = Quaternion.Euler(0f, 0f, 180f);
        StartRotation(Quaternion.Euler(0f, 0f, 0f), false, transitionTime);
    }

    public void TransitionToNight(float transitionTime)
    {
        root.SetActive(true);
        skybox.localRotation = Quaternion.identity;
        StartRotation(Quaternion.Euler(0f, 0f, -180f), true, transitionTime);
    }

    private void StartRotation(Quaternion targetRotation, bool dayToNight, float transitionTime)
    {
        if (transitionCoroutine != null)
            StopCoroutine(transitionCoroutine);

        transitionCoroutine = StartCoroutine(RotateSkybox(targetRotation, dayToNight, transitionTime));
    }

    private IEnumerator RotateSkybox(Quaternion targetRotation, bool dayToNight, float transitionTime)
    {
        Quaternion startRotation = skybox.localRotation;
        float timer = 0f;

        while (timer < transitionTime)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / transitionTime);

            // Rotate
            skybox.localRotation = Quaternion.Slerp(startRotation, targetRotation, t);

            // Color transition (flip gradient depending on direction)
            float colorT = dayToNight ? t : 1f - t;

            splashArt.color = splashGradient.Evaluate(colorT);
            background.color = backgroundGradient.Evaluate(colorT);

            yield return null;
        }

        // Snap to final rotation & colors
        skybox.localRotation = targetRotation;
        float finalT = dayToNight ? 1f : 0f;

        splashArt.color = splashGradient.Evaluate(finalT);
        background.color = backgroundGradient.Evaluate(finalT);

        transitionCoroutine = null;
    }

    public void HideUI()
    {
        root.SetActive(false);
    }
}
