using System;
using UnityEngine;

public class GameEvents
{
    public static GameEvents m_instance;

    public event Action onPlayerDie;

    public event Action onGameOver;

    public event Action onRetry;

    public static GameEvents Instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = new GameEvents();
            }
            return m_instance;
        }
    }

    public event Action<int> onAddToScore;
    public void AddToScore(int amount = 1)
    {
        onAddToScore?.Invoke(amount);
    }

    public void OnPlayerDie()
    {
        onPlayerDie?.Invoke();
    }
    public void OnGameOver()
    {
        onGameOver?.Invoke();
    }
    public void OnRetry()
    {
        onRetry?.Invoke(); 
    }
    

}
