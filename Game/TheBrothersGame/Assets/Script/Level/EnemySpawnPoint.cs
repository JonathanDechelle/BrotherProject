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
    private const int NUMBERS_QUARTERS_TO_SPAWN = 8;
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
            enemyGo.transform.position = GetSpawnPosition();

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

    private Vector3 GetSpawnPosition()
    {
        float gap = RADIANS_IN_CIRCLES / NUMBERS_QUARTERS_TO_SPAWN;
        int randomQuarter = Random.Range(0, NUMBERS_QUARTERS_TO_SPAWN);
        Vector3 newPosition =
                              new Vector3(
                                            Mathf.Cos(gap * randomQuarter),
                                            0,
                                            Mathf.Sin(gap * randomQuarter));

        return (newPosition * m_SpawnRange) + transform.position;
    }
}
