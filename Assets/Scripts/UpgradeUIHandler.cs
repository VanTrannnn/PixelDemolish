using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class UpgradeUIHandler : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button _btnOption1;
    [SerializeField] private Button _btnOption2;
    [SerializeField] private TextMeshProUGUI _txtOption1;
    [SerializeField] private TextMeshProUGUI _txtOption2;

    public void ShowRandomUpgrade()
    {
        Debug.Log("Generating upgrade options...");
        List<UpgradeType> upgrades = new List<UpgradeType> { UpgradeType.BiggerSaw, UpgradeType.FasterSaw, UpgradeType.MoreDamage };
        if(UpgradeManager.Instance.HasEmptySocket())
        {
            upgrades.Add(UpgradeType.NewSaw);
        }
        int r1 = Random.Range(0, upgrades.Count);
        UpgradeType option1 = upgrades[r1];
        upgrades.RemoveAt(r1);
        int r2 = Random.Range(0, upgrades.Count);
        UpgradeType option2 = upgrades[r2];

        _txtOption1.text = GetUpgradeDescription(option1);
        _txtOption2.text = GetUpgradeDescription(option2);
            Debug.Log($"Upgrade Options: 1) {option1} - {GetUpgradeDescription(option1)}, 2) {option2} - {GetUpgradeDescription(option2)}");
        _btnOption1.onClick.RemoveAllListeners();
        _btnOption2.onClick.RemoveAllListeners();
        _btnOption1.onClick.AddListener(() => OnUpgradeSelected(option1));
        _btnOption2.onClick.AddListener(() => OnUpgradeSelected(option2));
    }
    private void OnUpgradeSelected(UpgradeType type)
    {
        UpgradeManager.Instance.ApplyUpgrade(type);
        XPManager.Instance.FinishUpgrade();
    }
    private string GetUpgradeDescription(UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.BiggerSaw:
                return "Increase saw size by 20%";
            case UpgradeType.FasterSaw:
                return "Increase saw rotation speed by 20";
            case UpgradeType.MoreDamage:
                return "Increase damage by 10";
            case UpgradeType.NewSaw:
                return "Add a new saw to an empty socket";
            default:
                return "";
        }
    }

}