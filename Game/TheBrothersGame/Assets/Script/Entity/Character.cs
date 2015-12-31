using UnityEngine;

public class Character : MonoBehaviour
{
    public float m_Speed;
    public Rigidbody m_RigidBody;
    protected float m_AttackFrequency = 1f;
    protected float m_AttackDamage = 0.25f;

    [HideInInspector]
    public bool IsDead = false;

    public virtual void Update()
    {

    }
}

