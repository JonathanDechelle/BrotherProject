using UnityEngine;
using System.Collections;

class Starter : MonoBehaviour
{
    public LevelData m_LevelData = null;

    public void Start()
    {
        gameObject.AddComponent<EnemyGenerator>();
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

