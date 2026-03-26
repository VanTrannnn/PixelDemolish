using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    
    [SerializeField] private LevelConfig[] _levelConfigs;  // Array chứa config cho các level
    private LevelConfig _currentLevelConfig;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        LoadLevelConfig(currentSceneIndex);
    }

    public void LoadLevelConfig(int sceneIndex)
    {
        // Index 0 = Menu, không cần load
        if (sceneIndex == 0)
            return;

        // Index 1+ = Game levels
        int levelIndex = sceneIndex - 1;
        
        if (levelIndex < _levelConfigs.Length && _levelConfigs[levelIndex] != null)
        {
            _currentLevelConfig = _levelConfigs[levelIndex];
            Debug.Log($"Loaded level config: {_currentLevelConfig.levelName}");
        }
        else
        {
            Debug.LogError($"Level config not found for level index {levelIndex}!");
        }
    }

    public int GetInitialSawCountToPlace()
    {
        return _currentLevelConfig != null ? _currentLevelConfig.initialSawCountToPlace : 1;
    }

    public LevelConfig GetCurrentLevelConfig()
    {
        return _currentLevelConfig;
    }
}
