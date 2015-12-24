using UnityEngine;

public class Character : MonoBehaviour
{
    public float m_Speed;
    public Rigidbody m_RigidBody;
    public bool Alive = true;

    public void Update()
    {
        GotoTarget();
        if (transform.position.z < -40)
        {
            Alive = false;
        }
    }

    private void GotoTarget()
    {
        Vector3 direction = Vector3.back;
        m_RigidBody.AddForce(direction * m_Speed);
    }
}

