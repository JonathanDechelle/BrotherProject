using UnityEngine;
using System.Collections.Generic;

public class Enemy : Character
{
    public enum EState
    {
        Move = 0,
        Attack = 1,
        Search = 2,
    }

    public EnemyGoal m_CurrentGoal;
    public List<EnemyGoal> m_Goals;
    private EState m_CurrentState = EState.Move;
    private float m_AttackRange = 20f;

    public override void Update()
    {
        switch (m_CurrentState)
        {
            case EState.Move:
                GotoTarget();
                break;
            case EState.Attack:
                m_RigidBody.velocity = Vector3.zero;
                m_RigidBody.angularVelocity = Vector3.zero;
                break;
            case EState.Search:
                break;
        }

        base.Update();
    }

    public bool HasAttackMode()
    {
        return m_CurrentState == EState.Attack;
    }

    private void GotoTarget()
    {
        m_CurrentGoal = GetClosestGoal();
        Vector3 direction = m_CurrentGoal.transform.position - transform.position;
        direction.y = 0f;

        float distance = direction.magnitude;
        if (distance < m_AttackRange)
        {
            m_CurrentState = EState.Attack;
        }
        else
        {
            m_RigidBody.AddForce(direction * m_Speed, ForceMode.Acceleration);
        }
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
            if (m_Goals[i].IsTooWeak())
            {
                continue;
            }

            float distance = Vector3.Distance(transform.position, m_Goals[i].transform.position);
            if (distance < distanceMinimum)
            {
                distanceMinimum = distance;
                indexClosest = i;
            }
        }

        return m_Goals[indexClosest];
    }

    public void BeginDeadSequence()
    {
        Destroy(gameObject);
    }
}