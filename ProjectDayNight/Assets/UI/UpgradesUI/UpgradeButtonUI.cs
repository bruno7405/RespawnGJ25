using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] Image iconSprite;
    [SerializeField] Image backgroundImage;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI statText;

    private Button button;

    private Upgrade upgrade;

    private void Awake()
    {
        button = GetComponent<Button>();
        button?.onClick.AddListener(OnUpgradeChosen);
    }

    public void SetButtonUI(Upgrade u)
    {
        upgrade = u;
        title.text = upgrade.name;
        iconSprite.sprite = upgrade.image;
        backgroundImage.color = upgrade.backgroundColor;
        description.text = upgrade.description;

        // set stat text based on upgrade category
        switch (upgrade.category)
        {
            case Upgrade.Category.playerSpeed: // increase speed
                statText.text = "+" + upgrade.increasePercentage.ToString() + "% " + "walk speed";
                break;
            case Upgrade.Category.playerInteractRange: // increase range
                statText.text = "+" + upgrade.increasePercentage.ToString() + "% " + "range";
                break;
            case Upgrade.Category.playerLightRange: // increase view distance (night time)
                statText.text = "+" + upgrade.increasePercentage.ToString() + "% " + "light";
                break;
            case Upgrade.Category.morale: // increase morale
                statText.text = "+" + upgrade.increasePercentage.ToString() + " morale";
                break;
            case Upgrade.Category.productivity: // increase revenue
                statText.text = "+" + upgrade.increasePercentage.ToString() + "% " + "revenue";
                break;
        }
    }

    private void OnUpgradeChosen()
    {
        UpgradeState.instance.SetNewUpgrades(upgrade);
    }
}
