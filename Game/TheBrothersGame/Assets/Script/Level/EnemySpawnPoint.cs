using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawnPoint : MonoBehaviour
{
    public float m_SpawnRange = 10f;
    public float m_SpawnFrequency = 0.5f;
    public System.Action<GameObject> m_EnemyGOReady;
    public bool m_FinishToSpawn = false;
 
    private const float RADIANS_IN_CIRCLES = 6.28319f;
    private WaveInfo m_WaveInfo;

    public void GenerateEnemies(WaveInfo aWaveInfo)
    {
        m_WaveInfo = aWaveInfo;
        CoroutineManager.StartCoroutine(SpawnEnemyWithFrequency());
    }

    private IEnumerator SpawnEnemyWithFrequency()
    {
        for (int i = 0; i < m_WaveInfo.m_NumberOfEnemy; i++)
        {
            GameObject enemyGo = Instantiate(EnemyGenerator.GetEnemyGameObject(m_WaveInfo.m_EnemyType)) as GameObject;
            enemyGo.transform.position = GetSpawnPosition(i);

            if (m_EnemyGOReady != null)
            {
                m_EnemyGOReady(enemyGo);
            }

            float timer = m_SpawnFrequency;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                yield return null;
            }
        }

        m_FinishToSpawn = true;
    }

    private Vector3 GetSpawnPosition(int aUnitNumber)
    {
        float gap = RADIANS_IN_CIRCLES / m_WaveInfo.m_NumberOfEnemy;
        Vector3 newPosition =
                              new Vector3(
                                            Mathf.Cos(gap * (aUnitNumber)),
                                            0,
                                            Mathf.Sin(gap * (aUnitNumber)));

        return (newPosition * m_SpawnRange) + transform.position;
    }
}
