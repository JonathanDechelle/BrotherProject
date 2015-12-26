using UnityEngine;
using System.Collections.Generic;

public enum ELevelDifficulty
{
    Easy = 0,
    Medium = 1,
    Difficult = 2,
    Extreme = 3
}

public class LevelData : MonoBehaviour
{
    public string m_LevelName = "Mission 1 : The test";
    public ELevelDifficulty m_DifficultyLevel = ELevelDifficulty.Easy;
    public List<WaveInfo> m_WavesInfo;

    private const string LEVEL_FORMAT = "Mission Name = {0} Difficulty {1}";
    public void Start()
    {
        DebugLevel();
    }

    private void DebugLevel()
    {
        ShowNameAndDifficulty();
    }

    private void ShowNameAndDifficulty()
    {
        Debug.Log(string.Format(LEVEL_FORMAT, m_LevelName, m_DifficultyLevel));
    }
}
