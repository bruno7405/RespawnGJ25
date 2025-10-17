using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject creditsPanel;

    [Header("Other Objects")]
    public GameObject background;
    public GameObject startButtons;  // if your start buttons are separate

    // Called when clicking the Credits button
    public void ShowCredits()
    {
        // Hide main menu elements
        if (mainMenuPanel) mainMenuPanel.SetActive(false);
        if (background) background.SetActive(false);
        if (startButtons) startButtons.SetActive(false);

        // Show credits
        if (creditsPanel) creditsPanel.SetActive(true);
    }

    // Called when clicking the Return button
    public void ReturnToMenu()
    {
        // Hide credits
        if (creditsPanel) creditsPanel.SetActive(false);

        // Show main menu elements again
        if (mainMenuPanel) mainMenuPanel.SetActive(true);
        if (background) background.SetActive(true);
        if (startButtons) startButtons.SetActive(true);
    }
}


