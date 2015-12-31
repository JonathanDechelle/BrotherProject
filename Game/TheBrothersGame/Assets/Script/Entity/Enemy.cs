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
    public EState m_CurrentState = EState.Move;
    private float m_AttackRange = 10f;
    private float m_SearchWaitingTime = 2f;
    private float m_SearchCounter = 0f;
    private float m_AttackCounterTime = 0f;
    private EnemyGoal m_LastGoal;
    private NavMeshAgent m_NavMeshAgent;
    private float FIELD_OF_VIEW = 45f;

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
                AttackGoal();
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

    private void OnEnterSearch()
    {
        StopAllMovement();
        m_CurrentState = EState.Search;
    }

    public void AttackGoal()
    {
        m_AttackCounterTime += Time.deltaTime;
        if(m_AttackCounterTime >= m_AttackFrequency)
        {
            m_AttackCounterTime = 0;
            m_CurrentGoal.m_Life -= m_AttackDamage;

            if (m_CurrentGoal.IsDestroy() || m_CurrentGoal == null)
            {
                OnEnterSearch();
            }
        }
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

    private void GotoTarget()
    {
        m_CurrentGoal = GetClosestGoal();

        float distance = GetDistanceWith(m_CurrentGoal.transform.position);
        if (distance < m_AttackRange)
        {
            m_CurrentState = EState.Attack;
        }
        else
        {
            m_NavMeshAgent.destination = m_CurrentGoal.transform.position;
        }

        if (m_LastGoal != null && m_LastGoal != m_CurrentGoal && distance > FIELD_OF_VIEW)
        {
            OnEnterSearch();
        }

        m_LastGoal = m_CurrentGoal;
    }

    private float GetDistanceWith(Vector3 aPosition)
    {
        Vector3 direction = aPosition - transform.position;
        direction.y = 0f;
        return direction.magnitude;
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
            if (m_Goals[i] == null || m_Goals[i].IsDestroy() || m_Goals[i].IsTooWeak())
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

    #region Get/Set 
    public bool HasAttackMode
    {
        get
        {
            return m_CurrentState == EState.Attack;
        }
    }
    #endregion
}