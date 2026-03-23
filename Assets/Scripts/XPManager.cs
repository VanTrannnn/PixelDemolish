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
    private bool _isPaused = false;
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
        UpdateUI();

        if (_currentXP >= RequireXP)
        {
            TriggerUpgrade();
        }
    }
    private void TriggerUpgrade()
    {
        if (_upgradePanel == null)
        {
            Debug.LogError("[XPManager] Upgrade panel not assigned in Inspector!");
            return;
        }

        Time.timeScale = 0f; // Pause the game
        _isPaused = true;
        _upgradePanel.SetActive(true);
        
        var handler = _upgradePanel.GetComponent<UpgradeUIHandler>();
        if (handler == null)
        {
            Debug.LogError("[XPManager] UpgradeUIHandler component not found on upgrade panel!");
            return;
        }
        
        handler.ShowRandomUpgrade();
    }
    public void FinishUpgrade()
    {
        _upgradePanel.SetActive(false);
        Time.timeScale = 1f; // Resume the game
        _isPaused = false;
    }
    private void UpdateUI()
    {
        if (_xpSlider != null)
        {
            _xpSlider.value = _currentXP / RequireXP;
        }
    }
}