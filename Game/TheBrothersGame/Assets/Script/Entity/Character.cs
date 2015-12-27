using UnityEngine;

public class Character : MonoBehaviour
{
    public float m_Speed;
    public Rigidbody m_RigidBody;
    public bool IsDead = false;

    public void Update()
    {
        GotoTarget();
        if (transform.position.z < -40)
        {
            IsDead = true;
        }
    }

    private void GotoTarget()
    {
        Vector3 direction = Vector3.back;
        m_RigidBody.AddForce(direction * m_Speed);
    }
}

