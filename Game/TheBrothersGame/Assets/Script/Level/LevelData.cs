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
    public List<EnemyGoal> m_EnemyGoals;
    public EnemySpawnPoint m_EnemySpawnPoint;
    public List<WaveInfo> m_WavesInfo;
}
