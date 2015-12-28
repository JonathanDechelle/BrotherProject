using UnityEngine;
using System.Collections.Generic;

public class Enemy : Character
{
    public EnemyGoal m_CurrentGoal;
    public List<EnemyGoal> m_Goals;

    public override void Update()
    {
        GotoTarget();
        base.Update();
    }

    private void GotoTarget()
    {
        Vector3 direction = GetClosestGoal().transform.position - transform.position;
        direction.y = 0f;
        m_RigidBody.AddForce(direction * m_Speed);
    }

    private EnemyGoal GetClosestGoal()
    {
        if (m_Goals.Count == 0)
        {
            return null;
        }

        float distanceMinimum = float.MaxValue;
        int indexClosest = 0;
        for (int i = 0; i < m_Goals.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, m_Goals[i].transform.position);
            if (distance < distanceMinimum)
            {
                distanceMinimum = distance;
                indexClosest = i;
            }
        }

        return m_Goals[indexClosest];
    }
}