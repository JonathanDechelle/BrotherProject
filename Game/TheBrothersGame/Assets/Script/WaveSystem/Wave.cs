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
    public List<MiniWave> m_MiniWaves;
}

[System.Serializable]
public class MiniWave
{
    public EEnemy m_EnemyType;
    public int m_NumberOfEnemy;
}

public class Wave
{
    public EWaveState m_WaveState;
    private WaveInfo m_WaveInfo;
    private EWaveState m_DebugWaveState = EWaveState.Completed;
    private const string WAVE_FORMAT = "Wave State = {0}";
    private List<Enemy> m_Enemies;

    private EnemySpawnPoint m_SpawnPoint;
    private List<EnemyGoal> m_Goals;

    public Wave(WaveInfo aWaveInfo, List<EnemyGoal> aGoals, EnemySpawnPoint aSpawnPoint)
    {
        m_WaveInfo = aWaveInfo;
        m_WaveState = EWaveState.Created;
        m_Goals = aGoals;
        m_SpawnPoint = aSpawnPoint;
        CoroutineManager.StartCoroutine(UpdateWave());
        CoroutineManager.StartCoroutine(UpdateDebugWave());
    }
    
    public void StartCoolDown()
    {
        m_WaveState = EWaveState.WaitingToLaunch;
    }

    public void StartCombat()
    {
        m_WaveState = EWaveState.InProgress;
        m_Enemies = new List<Enemy>();
        m_SpawnPoint.GenerateEnemies(m_WaveInfo);
        m_SpawnPoint.m_EnemyGOReady += OnEnemyGoReady;
    }

    private void OnEnemyGoReady(GameObject aEnemyGo)
    {
        Enemy newEnemy = aEnemyGo.GetComponent<Enemy>();
        if (newEnemy != null)
        {
            newEnemy.m_Goals = m_Goals;
            m_Enemies.Add(newEnemy);
        }
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

            if (m_Enemies.Count == 0 && m_SpawnPoint.m_FinishToSpawn)
            {
                m_WaveState = EWaveState.Completed;
                CoroutineManager.StopCoroutine(UpdateDebugWave());
				m_SpawnPoint.m_EnemyGOReady -= OnEnemyGoReady;
            }
        }
    }
	
	private void OnDestroy()
	{
		m_SpawnPoint.m_EnemyGOReady -= OnEnemyGoReady;
	}

    private void UpdateDeadEnnemies()
    {
        for (int i = 0; i < m_Enemies.Count; i++)
        {
            if (m_Enemies[i].IsDead)
            {
                m_Enemies[i].BeginDeadSequence();
                m_Enemies.RemoveAt(i);
            }
        }
    }
}
