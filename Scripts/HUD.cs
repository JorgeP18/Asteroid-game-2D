using System.ComponentModel;
using UnityEngine;
using UnityEngine.UIElements;
using System;


public class Hud : MonoBehaviour
{
    private UIDocument m_uiDocument;
    [SerializeField]
    private ScoreManager m_scoreManager;
    private VisualElement m_gameOverScreen;

    private void OnEnable()
    {
        m_uiDocument = gameObject.GetComponent<UIDocument>();
        VisualElement root = m_uiDocument.rootVisualElement;
        VisualElement topBar = root.Q<VisualElement>("TopBar");
        topBar.dataSource = m_scoreManager;

        m_gameOverScreen = root.Q<VisualElement>("GameOver");
        VisualElement retryButton = m_gameOverScreen.Q<VisualElement>("RetryButton");
        VisualElement quitButton = m_gameOverScreen.Q<VisualElement>("QuitButton");
        Clickable retryClickable = new Clickable(() => {
            HandleRetryEvent();
        });
        Clickable quitClickable = new Clickable(() =>
        {
            OnQuit();
        });

        quitButton.AddManipulator(quitClickable);
        retryButton.AddManipulator(retryClickable);

        GameEvents.Instance.onGameOver += OnGameOver;
    }

    private void HandleRetryEvent()
    {
        GameEvents.Instance.OnRetry();
        m_gameOverScreen.AddToClassList("hidden");
    }

    private  void OnQuit()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#endif
    }

    private void OnDisable()
    {
        GameEvents.Instance.onGameOver -= OnGameOver;
    }
    private void OnGameOver()
    {
        m_gameOverScreen.RemoveFromClassList("hidden");

    }
}
