using UnityEngine;

public class Enemy : Character
{
    public Vector3 m_Target;

    public override void Update()
    {
        GotoTarget();
        base.Update();
    }

    private void GotoTarget()
    {
        Vector3 direction = m_Target - transform.position;
        direction.y = 0f;
        m_RigidBody.AddForce(direction * m_Speed);
    }
}