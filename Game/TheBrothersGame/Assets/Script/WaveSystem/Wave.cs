using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EWaveState
{
    Created = 0,
    WaitingToLaunch = 1,
    InProgress = 2,
    Completed = 3
}

[System.Serializable]
public class WaveInfo
{
    public float m_WaitTime = 2f;
    public EEnemy m_EnemyType;
    public int m_NumberOfEnemy;
}

public class Wave : MonoBehaviour
{
    private WaveInfo m_WaveInfo;
    private EWaveState m_WaveState;
    private EWaveState m_DebugWaveState = EWaveState.Completed;
    private const string WAVE_FORMAT = "Wave State = {0}";
    private List<Enemy> m_Enemies;

    private EnemySpawnPoint m_SpawnPoint;
    private float m_SpawnRange = 10f;
    private EnemyGoal m_Goal;

    public Wave(WaveInfo aWaveInfo, EnemyGoal aGoal, EnemySpawnPoint aSpawnPoint)
    {
        m_WaveInfo = aWaveInfo;
        m_WaveState = EWaveState.Created;
        m_Goal = aGoal;
        m_SpawnPoint = aSpawnPoint;
        CoroutineManager.StartCoroutine(UpdateWave());
        CoroutineManager.StartCoroutine(UpdateDebugWave());
    }

    private void GenerateEnemies()
    {
        m_Enemies = new List<Enemy>();
        
        for (int i = 0; i < m_WaveInfo.m_NumberOfEnemy; i++)
        {
            GameObject enemyGo = Instantiate(EnemyGenerator.GetEnemyGameObject(m_WaveInfo.m_EnemyType)) as GameObject;
            enemyGo.transform.position = GetSpawnPosition(i);

            Enemy newEnemy = enemyGo.GetComponent<Enemy>();
            if (newEnemy != null)
            {
                newEnemy.m_Target = m_Goal.transform.position;
                m_Enemies.Add(newEnemy);
            }
        }
    }

    public Vector3 GetSpawnPosition(int aUnitNumber)
    {
        float gap = 6.28319f / m_WaveInfo.m_NumberOfEnemy;
        Vector3 newPosition = 
                              new Vector3(
                                            Mathf.Cos(gap * (aUnitNumber + 1)),
                                            0,
                                            Mathf.Sin(gap * (aUnitNumber + 1))) * m_SpawnRange;

        return newPosition + m_SpawnPoint.transform.position;
    }
    
    public void StartCoolDown()
    {
        m_WaveState = EWaveState.WaitingToLaunch;
    }

    public void StartCombat()
    {
        m_WaveState = EWaveState.InProgress;
        GenerateEnemies();
    }

    public bool LaunchAnotherWave()
    {
        return m_WaveState == EWaveState.InProgress || m_WaveState == EWaveState.Completed;
    }
    
    private void DebugWave()
    {
        if(m_DebugWaveState != m_WaveState)
        {
            m_DebugWaveState = m_WaveState;
            Debug.Log(string.Format(WAVE_FORMAT, m_WaveState));
        }
    }
    
    private IEnumerator UpdateDebugWave()
    {
        while (true)
        {
            DebugWave();
            yield return null;
        }
    }

    private IEnumerator UpdateWave()
    {
        while(m_WaveState == EWaveState.Created)
        {
            yield return null;
        }

        //Wait time before lunch
        while(m_WaveState == EWaveState.WaitingToLaunch)
        {
            float timer = m_WaveInfo.m_WaitTime;
            while(timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }

            StartCombat();
        }

        //Update wave during timeLife
        while (m_WaveState == EWaveState.InProgress)
        {
            yield return null;

            UpdateDeadEnnemies();

            if (m_Enemies.Count == 0)
            {
                m_WaveState = EWaveState.Completed;
                CoroutineManager.StopCoroutine(UpdateDebugWave());
            }
        }
    }

    private void UpdateDeadEnnemies()
    {
        for (int i = 0; i < m_Enemies.Count; i++)
        {
            if (m_Enemies[i].IsDead)
            {
                Destroy(m_Enemies[i].gameObject);
                m_Enemies.RemoveAt(i);
            }
        }
    }
}
