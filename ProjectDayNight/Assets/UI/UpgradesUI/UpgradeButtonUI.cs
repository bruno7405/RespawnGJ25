using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButtonUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] Image image;
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
        image.sprite = upgrade.image;
        description.text = upgrade.description;
        statText.text = "Increases " + upgrade.stat + " by " + upgrade.increasePercentage.ToString() + "%";

        int randomVal = (int)Random.Range(0, 2);
        if (randomVal == 0) 
        switch (randomVal)
        {
            case 0:
                button.image.color = new Color(243, 222, 138);
                break;
            case 1:
                button.image.color = new Color(163, 185, 201);
                break;
            case 2:
                button.image.color = new Color(163, 201, 198);
                break;

            }
    }

    private void OnUpgradeChosen()
    {
        UpgradeState.instance.SetNewUpgrades(upgrade);
    }
}
