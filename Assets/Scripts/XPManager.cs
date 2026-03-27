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
    [SerializeField] private Image _xpImage;
    [SerializeField] private GameObject _upgradePanel;  // Panel to show when an upgrade is available
    [SerializeField] private GameObject _slot1;  // First upgrade option slot
    [SerializeField] private GameObject _slot2;  // Second upgrade option slot

    public float RequireXP => _baseXPRequired + (_upgradeCount * _xpIncreasePerLevel);
    public int UpgradeCount => _upgradeCount;
    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
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

    public void DisableSlot2()
    {
        if (_slot2 != null)
            _slot2.SetActive(false);
    }

    public void EnableSlot2()
    {
        if (_slot2 != null)
            _slot2.SetActive(true);
    }
    private void UpdateUI()
    {
        if (_xpImage != null)
        {
            _xpImage.fillAmount = _currentXP / RequireXP;
        }
    }
}