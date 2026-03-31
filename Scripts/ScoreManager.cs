using System.Runtime.CompilerServices;
using NUnit.Framework.Internal;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private int m_score;
    [SerializeField]
    private int m_topScore;
    [SerializeField]
    private int m_currentLives;
    [SerializeField]
    private int m_maxLives;

    private void Awake()
    {
        ClearScore();
    }
    private void OnEnable()
    {
        GameEvents.Instance.onAddToScore += AddScore;
        GameEvents.Instance.onPlayerDie += OnPlayerDie;
        GameEvents.Instance.onRetry += OnRetry;
        
    }
    

    private void OnDisable()
    {
        GameEvents.Instance.onAddToScore -= AddScore;
        GameEvents.Instance.onPlayerDie -= OnPlayerDie;
        GameEvents.Instance.onRetry -= OnRetry;
    }

    public void AddScore( int amount = 1)
    {
        m_score += amount;

        if (m_topScore < m_score)
        {
            m_topScore = m_score;
        }
    }

  

    public void ClearScore()
    {
        m_score = 0;
        m_currentLives = m_maxLives;
    }
    private void OnPlayerDie()
    {
        m_currentLives--;
        if (m_currentLives <= 0)
        {
            GameEvents.Instance.OnGameOver();
            Time.timeScale = 0;
        }
    }
    private void OnRetry()
    {
        Time.timeScale = 1;
        ClearScore();
    }
}
