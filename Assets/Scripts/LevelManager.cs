using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    
    [SerializeField] private LevelConfig[] _levelConfigs;  
    private LevelConfig _currentLevelConfig;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        LoadLevelConfig(currentSceneIndex);
    }

    public void LoadLevelConfig(int sceneIndex)
    {

        int levelIndex = sceneIndex;
        
        if (levelIndex >= 0 && levelIndex < _levelConfigs.Length && _levelConfigs[levelIndex] != null)
        {
            _currentLevelConfig = _levelConfigs[levelIndex];
        }
    }

    public int GetInitialSawStart()
    {
        return _currentLevelConfig != null ? _currentLevelConfig.initSawStart : 1;
    }

    public LevelConfig GetCurrentLevelConfig()
    {
        return _currentLevelConfig;
    }
}
