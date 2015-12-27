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
        foreach (IEnumerator routine in CoroutineManager.Coroutines)
        {
            routine.MoveNext();
        }
    }
}

