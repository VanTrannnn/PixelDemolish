using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/Level Config")]
public class LevelConfig : ScriptableObject
{
    [Header("Level Settings")]
    public int levelIndex; 
    public int initSawStart = 1; 
    public string levelName = "Level 1";
}
