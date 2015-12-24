using UnityEngine;
using System.Collections;

class Starter : MonoBehaviour
{
    public LevelData m_LevelData = null;

    public void Start()
    {
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

