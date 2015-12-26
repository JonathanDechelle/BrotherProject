using UnityEngine;
using System.Collections;

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
    private float m_WaitTime;
    private EWaveState m_WaveState;
    private EWaveState m_DebugWaveState = EWaveState.Completed;
    private const string WAVE_FORMAT = "Wave State = {0}";
    private Enemy m_Enemy;

    public Wave(WaveInfo aWaveInfo)
    {
        m_WaitTime = aWaveInfo.m_WaitTime;
        m_WaveState = EWaveState.Created;
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
        GameObject enemyGo = Instantiate(Resources.Load("Prefab/Enemy")) as GameObject;
        m_Enemy = enemyGo.GetComponent<Enemy>();
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
            float timer = m_WaitTime;
            while(timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }

            StartCombat();
        }

        while(m_WaveState == EWaveState.InProgress)
        {
            yield return null;
            if (!m_Enemy.Alive) //Victory Condition
            {
                Destroy(m_Enemy.gameObject);
                m_WaveState = EWaveState.Completed;
                CoroutineManager.StopCoroutine(UpdateDebugWave());
            }
        }
    }
}
