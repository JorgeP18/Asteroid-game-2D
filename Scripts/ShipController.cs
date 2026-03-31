using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;
public class ShipController : BoundedEntity
{

    private float m_turnInput;
    private float m_forwardInput;
    [SerializeField]
    private float m_maxBoostingSpeed;
    [SerializeField]
    private float m_maxSpeed;
    [SerializeField]
    private float m_turnSpeed;
    [SerializeField]
    private float m_moveSpeed;
    [SerializeField]
    private float m_boostSpeed;
    [SerializeField]
    private float m_stoppingPower;
    [SerializeField]
    private GameObject m_bulletPrefab;
    [SerializeField]
    private float m_fireDelay;
    [SerializeField]
    private float m_fireCount;
    [SerializeField]
    private bool m_isFiring;
    [SerializeField]
    private bool m_isBoosting;
    [SerializeField]
    private bool m_isDead;

    


    void  OnMove(InputValue value)
    {
        Vector2 moveInputDirection = value.Get<Vector2> ();
        m_turnInput = moveInputDirection.x;
        m_forwardInput = moveInputDirection.y;

    }
    void OnAttack(InputValue value)
    {
        if (value.Get<float>() > 0f)
        {
            SpawnBullet();
        }
    }

    void OnBoost(InputValue value)
    {
        m_isBoosting = value.Get<float>() > 0f;
    }
    void SpawnBullet()
    {
        GameObject bullet = Instantiate(m_bulletPrefab);
        bullet.transform.position = transform.position + (transform.up * 3f);
        bullet.transform.up = transform.up;
    }

    protected override void LateUpdate()
    {
        if (m_isDead) { return; }
        m_rigidbody.rotation -= (m_turnInput * (m_turnSpeed * 100f)) * Time.deltaTime;
        if (m_isBoosting)
        {
            m_rigidbody.AddRelativeForceY((m_forwardInput * (m_boostSpeed * 100f)) * Time.deltaTime);
        }
        if (m_forwardInput > 0)
        {
                m_rigidbody.AddRelativeForceY((m_forwardInput * (m_moveSpeed * 100f)) * Time.deltaTime);
        }
        else if (m_forwardInput < 0)
        {
            m_rigidbody.linearVelocity = Vector2.Lerp(m_rigidbody.linearVelocity, Vector2.zero, m_stoppingPower * Time.deltaTime);
        }
        if(m_isBoosting && m_rigidbody.linearVelocity.magnitude > m_maxBoostingSpeed)
        {
            m_rigidbody.linearVelocity = m_rigidbody.linearVelocity.normalized * m_maxBoostingSpeed;
        }
        else if (!m_isBoosting && m_rigidbody.linearVelocity.magnitude > m_maxSpeed)
        {
            m_rigidbody.linearVelocity = m_rigidbody.linearVelocity.normalized * m_maxSpeed;
        }
        base.LateUpdate();

        if (m_isFiring)
        {
            TrySpawnBullet();
        }
        else
        {
            m_fireCount = m_fireDelay;
        }
    }
    void TrySpawnBullet()
    {
        if(m_fireCount > m_fireDelay)
        {
            m_fireCount = 0;
            GameObject bullet = GameObject.Instantiate(m_bulletPrefab);
            bullet.transform.position = transform.position + (transform.up * 3f);
            bullet.transform.up = transform.up;
        }
        else
        {
            m_fireCount += Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent(out AsteroidController asteroid))
        {
            LoseHealth();
        }
    }
    protected override void OnEnable()
    {
        GameEvents.Instance.onRetry += OnRetry;
        base.OnEnable();
        
    }
    private void OnRetry()
    {
        SetPlayerAsDead();
        StartCoroutine(RespawnPlayer());
    }
    protected void SetPlayerAsDead()
    {
        m_isDead = true;
        m_spriteRenderer.enabled = false;
        m_collider.enabled = false;
        m_rigidbody.simulated = false;
    }

    protected override void OnDisable()
    {
        GameEvents.Instance.onRetry -= OnRetry;
        base.OnDisable();
    }
    protected override void OnDie()
    {
        GameEvents.Instance.OnPlayerDie();
        SetPlayerAsDead();

        StartCoroutine(RespawnPlayer());

    }
    private IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(0.5f);

        m_rigidbody.position = Vector3.zero;
        m_rigidbody.linearVelocity = Vector2.zero;
        m_rigidbody.transform.position = Vector3.zero;
        m_rigidbody.rotation = 0;
        m_spriteRenderer.enabled = true;
        ResetHealth();

        yield return new WaitForSeconds(0.5f);

        m_isDead = false;
        m_rigidbody.simulated = true;

        yield return new WaitForSeconds(0.5f);
        m_collider. enabled = true;

    }
}
