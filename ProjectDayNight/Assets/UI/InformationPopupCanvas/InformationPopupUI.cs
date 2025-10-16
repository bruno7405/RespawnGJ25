using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationPopupUI : MonoBehaviour
{
    private static InformationPopupUI instance;
    public static InformationPopupUI Instance => instance;

    [Header("References")]
    [SerializeField] GameObject root;
    [SerializeField] Animator animator;
    [SerializeField] TextMeshProUGUI popupTMP;
    [SerializeField] Image backgroundImage;

    [Header("Params")]
    const float displayDuration = 3f;
    [SerializeField] Color dayTextColor;
    [SerializeField] Color dayBackgroundColor;
    [SerializeField] Color nightTextColor;
    [SerializeField] Color nightBackgroundColor;

    private Coroutine popupCoroutine;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        instance = this;

        root.SetActive(false);
    }

    public void DisplayText(string text, bool isDay)
    {
        if (popupCoroutine != null) StopCoroutine(popupCoroutine);
        StartCoroutine(DisplaySequence(text, isDay));
    }

    public void DisplayText(string text, bool isDay, float duration = displayDuration)
    {
        if (popupCoroutine != null)
        {
            StopCoroutine(popupCoroutine);
            popupCoroutine = null;
        }
        StartCoroutine(DisplaySequence(text, isDay, duration));
    }

    private IEnumerator DisplaySequence(string text, bool isDay, float duration = displayDuration)
    {
        root.SetActive(true);
        popupTMP.text = text;

        if (isDay)
        {
            backgroundImage.color = new Color(dayBackgroundColor.r, dayBackgroundColor.g, dayBackgroundColor.b, dayBackgroundColor.a);
            popupTMP.color = new Color(dayTextColor.r, dayTextColor.g, dayTextColor.b, dayTextColor.a); ;
        }
        else
        {
            backgroundImage.color = new Color(nightBackgroundColor.r, nightBackgroundColor.g, nightBackgroundColor.b, nightBackgroundColor.a);
            popupTMP.color = new Color(nightTextColor.r, nightTextColor.g, nightTextColor.b, nightTextColor.a);
        }

        yield return new WaitForSeconds(duration);

        animator.SetTrigger("slideOut");

        yield return new WaitForSeconds(0.5f);

        popupCoroutine = null;
        root.SetActive(false);
    }

    private IEnumerator DisplaySequence(string text, bool isDay, Color textColor)
    {
        root.SetActive(true);
        popupTMP.text = text;

        if (isDay)
        {
            backgroundImage.color = new Color(dayBackgroundColor.r, dayBackgroundColor.g, dayBackgroundColor.b, dayBackgroundColor.a);
            popupTMP.color = new Color(textColor.r, textColor.g, textColor.b, textColor.a);
        }
        else
        {
            backgroundImage.color = new Color(nightBackgroundColor.r, nightBackgroundColor.g, nightBackgroundColor.b, nightBackgroundColor.a);
            popupTMP.color = new Color(textColor.r, textColor.g, textColor.b, textColor.a);
        }

        yield return new WaitForSeconds(displayDuration);

        animator.SetTrigger("slideOut");

        yield return new WaitForSeconds(0.5f);

        popupCoroutine = null;
        root.SetActive(false);
    }

}
