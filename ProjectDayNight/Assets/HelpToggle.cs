using UnityEngine;

public class OptionsToggle : MonoBehaviour
{
    public GameObject optionsPanel;

    public void ToggleOptionsPanel()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(!optionsPanel.activeSelf);
        }
    }
}
