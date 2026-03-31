using System;
using UnityEngine;
using UnityEngine.Rendering;

public class AsteroidController : BoundedEntity
{
    [SerializeField]
    private int m_size;
    [SerializeField]
    private float m_forcePower;
    [SerializeField]
    private float m_constantPower;
    [SerializeField]
    private float m_angularPower;
    public int Size => m_size;
    public event Action<AsteroidController> onAsteroidDie;
    private void Start()
    {
        m_rigidbody.AddForce(UnityEngine.Random.insideUnitCircle * m_rigidbody.mass * m_forcePower, ForceMode2D.Impulse);
        m_rigidbody.angularVelocity = UnityEngine.Random.Range(-m_angularPower, m_angularPower);
    }
    private void FixedUpdate()
    {
        Vector3 direction = transform.up;
        m_rigidbody.AddForce((direction * m_rigidbody.mass * m_constantPower)* Time.deltaTime, ForceMode2D.Impulse);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        LoseHealth();
    }
    protected override void OnDie()
    {
        onAsteroidDie?.Invoke(this);
    }

}
