using UnityEngine;

public class Character : MonoBehaviour
{
    public float m_Speed;
    public Rigidbody m_RigidBody;

    [HideInInspector]
    public bool IsDead = false;

    public virtual void Update()
    {

    }
}

