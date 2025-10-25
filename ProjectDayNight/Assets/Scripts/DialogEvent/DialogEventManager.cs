using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Call SetDialogEvent and pass a DialogEvent to start a dialogue
/// </summary>
public class DialogEventManager : MonoBehaviour
{
    public static DialogEventManager Instance;

    [Header("UI References")]
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshProUGUI nameTMP;
    [SerializeField] private TextMeshProUGUI dialogTMP;
    [SerializeField] private Button choice1Button;
    [SerializeField] private Button choice2Button;

    private TextMeshProUGUI choice1TMP;
    private TextMeshProUGUI choice2TMP;

    private DialogEvent currentDialogEvent;
    private int currentLineIndex = 0;
    Action onGoodEnd;
    Action onBadEnd;
    int good = 2;


    private void Awake()
    {
        if (Instance == null) Instance = this;

        choice1TMP = choice1Button.GetComponentInChildren<TextMeshProUGUI>();
        choice2TMP = choice2Button.GetComponentInChildren<TextMeshProUGUI>();
    }

    /// <summary>
    /// Start a dialogue event (called when interacting with an NPC)
    /// </summary>
    public void SetDialogEvent(DialogEvent d, Action goodEnd, Action badEnd)
    {
        onGoodEnd = goodEnd;
        onBadEnd = badEnd;
        good = 2;
        currentDialogEvent = d;
        currentLineIndex = 0;
        //Time.timeScale = 0.1f; // pause game
        DisplayUI();
    }

    private void DisplayUI()
    {
        if (currentDialogEvent == null) return;

        // End dialogue if no more lines
        if (currentLineIndex >= currentDialogEvent.lines.Length)
        {
            EndDialog();
            return;
        }

        // Restart animation
        animator.gameObject.SetActive(false);
        animator.gameObject.SetActive(true);

        // Enable buttons
        choice1Button.interactable = true;
        choice2Button.interactable = true;

        // Update UI with current line and choices
        var line = currentDialogEvent.lines[currentLineIndex];

        nameTMP.text = currentDialogEvent.npcName;
        dialogTMP.text = line.npcLine;

        // Button 1
        choice1TMP.text = line.choices[0].choiceText;
        choice1Button.onClick.RemoveAllListeners();
        choice1Button.onClick.AddListener(() => OnChoiceSelect(line.choices[0]));

        // Button 2
        choice2TMP.text = line.choices[1].choiceText;
        choice2Button.onClick.RemoveAllListeners();
        choice2Button.onClick.AddListener(() => OnChoiceSelect(line.choices[1]));
    }

    private void OnChoiceSelect(DialogChoice choice)
    {
        choice1Button.interactable = false;
        choice2Button.interactable = false;

        if (choice.isCorrect)
        {
            // Debug.Log("Correct choice: " + choice.choiceText);
        }
        else
        {
            good--;
            Debug.Log("Wrong choice: " + choice.choiceText);
            GameStateManager.Instance.IncrementMistakes();

        }

        // Move to next line
        currentLineIndex++;
        animator.SetTrigger("slideOut");
        Invoke(nameof(DisplayUI), 0.25f); // wait for slide out animation
    }

    private void EndDialog()
    {
        //Time.timeScale = 1f; // resume game
        if (good > 0)
        {
            onGoodEnd?.Invoke();
        }
        else
        {
            onBadEnd?.Invoke();
        }
        animator.SetTrigger("slideOut");

    }
}
