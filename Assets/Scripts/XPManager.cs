using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class XPManager : MonoBehaviour
{
    public static XPManager Instance;
    [Header("XP Settings")]
    [SerializeField] private float _baseXPRequired = 100f;
    [SerializeField] private float _xpIncreasePerLevel = 50f;

    private float _currentXP = 0f;
    private int _upgradeCount = 0;
    [Header("UI Elements")]
    [SerializeField] private Slider _xpSlider;
    [SerializeField] private GameObject _upgradePanel;  // Panel to show when an upgrade is available

    public float RequireXP => _baseXPRequired + (_upgradeCount * _xpIncreasePerLevel);
    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start() {
        _upgradePanel.SetActive(false);
    }
    public void AddXP(float amount)
    {
        _currentXP += amount;
        while (_currentXP >= RequireXP)
        {
            _currentXP -= RequireXP; 
            _upgradeCount++;
            
            TriggerUpgrade();
        }
        
        UpdateUI();
        Debug.Log($"XP: {_currentXP:F1}/{RequireXP} | Upgrade Count: {_upgradeCount}");
    }
    private void TriggerUpgrade()
    {
        Time.timeScale = 0f; // Pause the game
        _upgradePanel.SetActive(true);
        _upgradePanel.GetComponent<UpgradeUIHandler>().ShowRandomUpgrade();
    } 
    public void HideUpgradePanel() // Hide panel for select saw socket
    {
        Time.timeScale = 0f; // Keep game paused during socket selection
        _upgradePanel.SetActive(false);
    }
    public void FinishUpgrade()
    {
        _upgradePanel.SetActive(false);
        Time.timeScale = 1f; // Resume the game
    }
    private void UpdateUI()
    {
        if (_xpSlider != null)
        {
            _xpSlider.value = _currentXP / RequireXP;
        }
    }
}