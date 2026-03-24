using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro; // Thêm namespace của TextMesh Pro

// Cấu trúc dữ liệu để lưu thông tin hiển thị của mỗi nâng cấp
[System.Serializable]
public class UpgradeUIData
{
    public UpgradeType type;
    public string title;
    [TextArea(2, 4)]
    public string detail;
    public Sprite icon;
}

public class UpgradeUIHandler : MonoBehaviour
{
    [Header("Cấu hình dữ liệu nâng cấp")]
    [SerializeField] private List<UpgradeUIData> _upgradesDataList;

    [Header("UI Option 1")]
    [SerializeField] private TextMeshProUGUI _txtTitle1;  
    [SerializeField] private TextMeshProUGUI _txtDetail1; 
    [SerializeField] private Image _imgIcon1;
    [SerializeField] private Button _btnOption1;

    [Header("UI Option 2")]
    [SerializeField] private TextMeshProUGUI _txtTitle2;  
    [SerializeField] private TextMeshProUGUI _txtDetail2; 
    [SerializeField] private Image _imgIcon2;
    [SerializeField] private Button _btnOption2;

    public void ShowRandomUpgrade()
    {
        List<UpgradeUIData> availableData = new List<UpgradeUIData>();
        
        foreach (var data in _upgradesDataList)
        {
            if (data.type == UpgradeType.NewSaw)
            {
                if (UpgradeManager.Instance != null && UpgradeManager.Instance.HasEmptySocket())
                {
                    availableData.Add(data);
                }
            }
            else
            {
                availableData.Add(data);
            }
        }

        if (availableData.Count < 2) return; 

        int r1 = Random.Range(0, availableData.Count);
        UpgradeUIData data1 = availableData[r1];
        availableData.RemoveAt(r1);

        int r2 = Random.Range(0, availableData.Count);
        UpgradeUIData data2 = availableData[r2];

        SetOptionUI(data1, _txtTitle1, _txtDetail1, _imgIcon1, _btnOption1);

        SetOptionUI(data2, _txtTitle2, _txtDetail2, _imgIcon2, _btnOption2);
    }

    private void SetOptionUI(UpgradeUIData data, TextMeshProUGUI titleTxt, TextMeshProUGUI detailTxt, Image iconImg, Button btn)
    {
        if (titleTxt != null) titleTxt.text = data.title;
        if (detailTxt != null) detailTxt.text = data.detail;
        if (iconImg != null) iconImg.sprite = data.icon;

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => OnSelectUpgrade(data.type));
    }

    private void OnSelectUpgrade(UpgradeType type)
    {
        if (UpgradeManager.Instance != null)
        {
            UpgradeManager.Instance.ApplyUpgrade(type);  // Gọi ApplyUpgrade
        }

        if(type!= UpgradeType.NewSaw && XPManager.Instance != null) // Nếu không phải NewSaw thì kết thúc nâng cấp ngay
        {   
            XPManager.Instance.FinishUpgrade();  
        }
    }
}