using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/Level Config")]
public class LevelConfig : ScriptableObject
{
    [Header("Level Settings")]
    public int levelIndex;  // Tương ứng với SceneManager buildIndex
    public int initialSawCountToPlace = 1;  // Số saw mà player phải đặt ở đầu level
    public string levelName = "Level 1";
    [TextArea(2, 4)]
    public string description = "Destroy all cubes to complete the level";
}
