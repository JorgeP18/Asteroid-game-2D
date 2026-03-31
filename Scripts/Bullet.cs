using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float forwardSpeed;
    [SerializeField]
    private float m_maximumLifetime;
   // [SerializeField]
    private float m_currentLifetime;
    // Update is called once per frame
    void Update()
    {
        transform.position += (transform.up * forwardSpeed) * Time.deltaTime;
        m_currentLifetime = Time.deltaTime;
        if (m_currentLifetime > m_maximumLifetime)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}
