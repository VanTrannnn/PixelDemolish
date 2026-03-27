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
    [SerializeField] private TextMeshProUGUI _levelText;

    private int _totalPixelsInLevel;
    private int _destroyPixelsCount = 0;
    private bool _levelCompleted = false;
    private void Awake() {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start() {
        _levelText.text = $"Level {SceneManager.GetActiveScene().buildIndex + 1}";
        _winPanel.SetActive(false);
        
        if (_levelProgressImage == null)
        {
            return;
        }
        
        GameObject[] pixels = GameObject.FindGameObjectsWithTag("Cube");
        _totalPixelsInLevel = pixels.Length;
        
        _destroyPixelsCount = 0;
        _levelProgressImage.fillAmount = 0;
        
    }
    public void NotifyPixelDestroyed(int amount = 1) {
        if (_levelCompleted)
            return;

        _destroyPixelsCount += amount;
        
        if (_totalPixelsInLevel == 0)
        {
            return;
        }
        
        float fillAmount = _destroyPixelsCount / (float)_totalPixelsInLevel;
        _levelProgressImage.fillAmount = fillAmount;
        
        if (_destroyPixelsCount >= _totalPixelsInLevel) {
            LevelCompleted();
        }
    }
    private void LevelCompleted() {
        _levelCompleted = true;
        
        if (_winPanel == null)
        {
            return;
        }
        
        
        _winPanel.SetActive(true);
        Time.timeScale = 0;
    }
    
    public void NextLevel()
    {
        Time.timeScale = 1f;  
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        
        // Check if next level exists
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        
    }
    
    public void SaveProgress()
    {
        PlayerPrefs.SetInt("CurrentLevel", SceneManager.GetActiveScene().buildIndex);
    }
}