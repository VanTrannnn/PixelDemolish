using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelProgressManager : MonoBehaviour
{
    public static LevelProgressManager Instance;
    [Header("UI References")]
    [SerializeField] private Image _levelProgressImage;
    [SerializeField] private GameObject _winPanel;

    private int _totalPixelsInLevel;
    private int _destroyPixelsCount = 0;
    private bool _levelCompleted = false;
    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    private void Start() {
        _winPanel.SetActive(false);
        
        if (_levelProgressImage == null)
        {
            Debug.LogError("ERROR: Level Progress Image is not assigned in Inspector!");
            return;
        }
        
        GameObject[] pixels = GameObject.FindGameObjectsWithTag("Cube");
        _totalPixelsInLevel = pixels.Length;
        
        _destroyPixelsCount = 0;
        _levelProgressImage.fillAmount = 0;
        
        Debug.Log($"LevelProgressManager Start: Total pixels = {_totalPixelsInLevel}");
    }
    public void NotifyPixelDestroyed(int amount = 1) {
        if (_levelCompleted)
            return;

        _destroyPixelsCount += amount;
        
        if (_totalPixelsInLevel == 0)
        {
            Debug.LogError("ERROR: _totalPixelsInLevel is 0!");
            return;
        }
        
        float fillAmount = _destroyPixelsCount / (float)_totalPixelsInLevel;
        _levelProgressImage.fillAmount = fillAmount;
        Debug.Log($"Level Progress: {_destroyPixelsCount}/{_totalPixelsInLevel} ({fillAmount * 100:F1}%)");        
        
        if (_destroyPixelsCount >= _totalPixelsInLevel) {
            LevelCompleted();
        }
    }
    private void LevelCompleted() {
        _levelCompleted = true;
        
        if (_winPanel == null)
        {
            Debug.LogError("ERROR: Win Panel is not assigned in Inspector!");
            return;
        }
        
        
        _winPanel.SetActive(true);
        Time.timeScale = 0;
    }
    
    public void NextLevel()
    {
        Time.timeScale = 1f;  // Resume game before loading next level
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        
        // Check if next level exists
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No more levels available! Staying on current level.");
        }
    }
    
    //save level scene index to player prefs
    public void SaveProgress()
    {
        PlayerPrefs.SetInt("CurrentLevel", SceneManager.GetActiveScene().buildIndex);
    }
}