using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesUI : MonoBehaviour
{
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] List<Button> buttons = new List<Button>();

    [SerializeField] GameObject panel;

    public void CreateUpgradeButton(Upgrade upgrade)
    {
        var b = Instantiate(buttonPrefab, panel.transform);

        // Update visuals
        UpgradeButtonUI buttonUI = b.GetComponent<UpgradeButtonUI>();
        buttonUI.SetButtonUI(upgrade);

        Button button = b.GetComponent<Button>();
        buttons.Add(button);
        button?.onClick.AddListener(InteractOff);
    }

    public void InteractOff()
    {
        while (buttons.Count > 0)
        {
            var b = buttons[0];
            b.interactable = false;
            b.onClick.RemoveAllListeners();
            buttons.Remove(b);
            Destroy(b.gameObject);
        }
        buttons.RemoveAll(item => item == null);

        gameObject.SetActive(false);
    }
}
