using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    public static FakeEnemyMapper m_FakeEnemyMapper;

    public EnemyGenerator()
    {
        InitializeEnemyList();
    }

    public void InitializeEnemyList()
    {
        GameObject fakeEnemyMapperGO = Instantiate(Resources.Load("Prefab/Mappers/EnemyMapper")) as GameObject;
        EnemyGenerator.m_FakeEnemyMapper = fakeEnemyMapperGO.GetComponent<FakeEnemyMapper>();
        Destroy(fakeEnemyMapperGO);
    }

    public static GameObject GetEnemyGameObject(EEnemy aEnemyType)
    {
        for (int i = 0; i < m_FakeEnemyMapper.EnemyMappers.Count; i++)
        {
            EnemyMap enemyMap = m_FakeEnemyMapper.EnemyMappers[i];
            if (enemyMap.m_EnemyType == aEnemyType)
            {
                return enemyMap.m_EnemyGameObject;
            }
        }

        return null;
    }
}