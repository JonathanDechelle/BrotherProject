using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour 
{
    public LevelData m_LevelData;
    private Vector3 m_InitialPosition;

    private float m_WaitTimeOnAllyBase = 1.5f;
    private float m_WaitTimeOnEnemyBase = 4.0f;
    private float m_WaitTimeCouter = 0f;
    private float m_CameraSpeed = 1.5f;
    private const float MINIMUM_DISTANCE_TO_RETURN = 43f;
    private bool m_ReturnToBase = false;
    private bool m_CameraIntroSequence = true;

    public void Start()
    {
        m_InitialPosition = m_LevelData.m_EnemyGoals[0].transform.position;
        CoroutineManager.StartCoroutine(UpdateCameraIntroTravelling());
    }

    public IEnumerator UpdateCameraIntroTravelling()
    {
        // Time stay focus on ally base
        m_WaitTimeCouter = m_WaitTimeOnAllyBase;
        while (m_WaitTimeCouter > 0)
        {
            m_WaitTimeCouter -= Time.deltaTime;
            yield return null;
        }
        m_WaitTimeCouter = m_WaitTimeOnEnemyBase;

        //Camera traveling sequence (ally base -> enemyBase -> ally base)
        while (m_CameraIntroSequence)
        {
            yield return null;

            Vector3 direction = m_ReturnToBase ?
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
                    if (!m_ReturnToBase)
                    {
                        m_ReturnToBase = true;
                    }
                    else
                    {
                        m_CameraIntroSequence = false;
                    }
                }
            }
        }

        CoroutineManager.StartCoroutine(UpdateCameraInGame());   
    }

    private IEnumerator UpdateCameraInGame()
    {
        while (true)
        {
            yield return null;

            Vector3 newPosition = transform.position;
            if (Input.GetKey(KeyCode.A))
            {
                newPosition += Vector3.back * m_CameraSpeed;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                newPosition += Vector3.forward * m_CameraSpeed;
            }

            transform.position = newPosition;
        }
    }
}
