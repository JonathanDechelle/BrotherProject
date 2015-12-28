using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EnemyMap
{
    public EEnemy m_EnemyType;
    public GameObject m_EnemyGameObject;
}

public class FakeEnemyMapper : MonoBehaviour
{
    public List<EnemyMap> EnemyMappers;
}