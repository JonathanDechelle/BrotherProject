using UnityEngine;
using System.Collections;

class Starter : MonoBehaviour
{
    public LevelData m_LevelData = null;

    private EnemyGenerator m_EnemyGenerator;
    public void Start()
    {
        m_EnemyGenerator = new EnemyGenerator();
        WaveManager.InitManager(m_LevelData);
    }

    public void Update()
    {
        for (int i = 0; i < CoroutineManager.Coroutines.Count; i++)
        {
            CoroutineManager.Coroutines[i].MoveNext();
        }
    }
}

