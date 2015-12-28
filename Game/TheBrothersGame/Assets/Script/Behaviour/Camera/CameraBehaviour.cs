﻿using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour 
{
    public LevelData m_LevelData;
    private Vector3 m_InitialPosition;

    private float m_WaitTimeSetup = 1.5f;
    private float m_WaitTimeReverse = 4.0f;
    private float m_WaitTimeCouter = 0f;
    private float m_CameraSpeed = 1.5f;
    private const float MINIMUM_DISTANCE_TO_RETURN = 32.5f;
    private const float OFFSET_INTRO_CAMERA = 12f;
    private bool m_ReverseToBase = false;
    private bool m_CameraIntroTraveling = true;

    public void Start()
    {
        m_InitialPosition = m_LevelData.m_EnemyGoal.transform.position;
        m_InitialPosition.z += OFFSET_INTRO_CAMERA;
        CoroutineManager.StartCoroutine(UpdateCameraIntroTravelling());
    }

    public IEnumerator UpdateCameraIntroTravelling()
    {
        // Time stay focus on ally base
        m_WaitTimeCouter = m_WaitTimeSetup;
        while (m_WaitTimeCouter > 0)
        {
            m_WaitTimeCouter -= Time.deltaTime;
            yield return null;
        }
        m_WaitTimeCouter = m_WaitTimeReverse;

        //Camera traveling sequence (ally base -> enemyBase -> ally base)
        while (m_CameraIntroTraveling)
        {
            yield return null;

            Vector3 direction = m_ReverseToBase ?
                 m_InitialPosition - transform.position :                                  //Enemy -> Ally
                 m_LevelData.m_EnemySpawnPoint.transform.position - transform.position;    //Ally -> Enemy

            float distance = direction.magnitude;
            direction.Normalize();

            //Set Z position of the camera
            Vector3 newPosition = transform.position;
            newPosition.z += direction.z * m_CameraSpeed;
            transform.position = newPosition;
            
            if (distance < MINIMUM_DISTANCE_TO_RETURN)
            {
                if (m_WaitTimeCouter > 0) // I dont apply while logic because i want camera continue to process 
                {
                    m_WaitTimeCouter -= Time.deltaTime;
                }
                else
                {
                    m_ReverseToBase = true;
                }
            }
        }
    }
}