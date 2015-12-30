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

    [HideInInspector]
    public List<EnemyGoal> m_Goals;
    private EState m_CurrentState = EState.Move;
    private float m_AttackRange = 20f;
    private float m_SearchWaitingTime = 2f;
    private float m_SearchCounter = 0f;
    private EnemyGoal m_LastGoal;
    private NavMeshAgent m_NavMeshAgent;

    private void Start()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    public override void Update()
    {
        switch (m_CurrentState)
        {
            case EState.Move:
                GotoTarget();
                break;
            case EState.Attack:
                StopAllMovement();
                break;
            case EState.Search:
                transform.Rotate(Vector3.up * 10); //Just for debug
                SearchAnimation();
                break;
        }

        base.Update();
    }

    private void StopAllMovement()
    {
        m_NavMeshAgent.destination = transform.position;
    }

    private void SearchAnimation()
    {
        m_SearchCounter += Time.deltaTime;
        if (m_SearchCounter > m_SearchWaitingTime)
        {
            m_SearchCounter = 0;
            m_CurrentState = EState.Move;
        }
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
            m_NavMeshAgent.destination = m_CurrentGoal.transform.position;
        }

        if (m_LastGoal != null && m_LastGoal != m_CurrentGoal)
        {
            StopAllMovement();
            m_CurrentState = EState.Search;
        }

        m_LastGoal = m_CurrentGoal;
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