using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyGoal : MonoBehaviour
{
    public float m_Life = 100f;
    public float m_Defense;
    public float m_BaseHazardLevel = 2f; //decrease more ennemies are closed
    public float m_RangeCheckHazard = 10f;
    public float m_CurrentHazardLevel = 0f;

    public void Update()
    {
        List<Enemy> ennemies = CombatEnemyUtil.GetEnnemiesAround(transform.position, m_RangeCheckHazard);
        float newHasardLevel = ennemies.Count;
       
        foreach(Enemy e in ennemies)
        {
            if (!e.HasAttackMode)
            {
                --newHasardLevel;
            }
        }
        m_CurrentHazardLevel = -newHasardLevel + m_BaseHazardLevel;
    }

    public bool IsDestroy()
    {
        return m_Life < 0;
    }

    public bool IsTooWeak()
    {
        return m_CurrentHazardLevel < 1;
    }
}
