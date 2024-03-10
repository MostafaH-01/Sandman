using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManagingGame : MonoBehaviour
{
    #region Variables
    [Header("Objects To Turn On After Starting Game")]
    [SerializeField]
    private GameObject Spawning;
    [SerializeField]
    private GameObject InputManager;
    [SerializeField]
    private GameObject HUD;

    [Header("Objects To Turn Off After Starting Game")]
    [SerializeField]
    private GameObject mainMenuCamera;

    [Header("UI Elements")]
    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    private Slider pointSlider;
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject winMenu;
    [SerializeField]
    private GameObject loseMenu;
    [SerializeField]
    private GameObject endScreen;
    [SerializeField]
    private GameObject mobileControls;

    [Header("Game Settings")]
    [SerializeField]
    private float gameDuration;
    [SerializeField]
    private int pointsToEndGame;

    [Header("Enemy Settings")]
    [SerializeField]
    private float enemySpeed = 2f;
    [SerializeField]
    private float sliderIncrement = 0.02f;
    [SerializeField]
    private float minWidth = 0.08f;
    [SerializeField]
    private float maxWidth = 0.17f;
    [SerializeField]
    private float maxPurplePos = 0.57f;
    [SerializeField]
    private float redFlashDuration = 0.2f;

    private int _points = 0;
    private float _currentTime;
    private bool _gameStarted = false;
    #endregion

    #region Singleton Setup
    // To setup Singleton
    private static ManagingGame instance;
    public static ManagingGame Instance
    {
        get
        {
            if (instance == null)
            {
                SetupInstance();
            }

            return instance;
        }
    }
    #endregion
    #region Method to Create Singleton Instance
    private static void SetupInstance()
    {
        instance = FindObjectOfType<ManagingGame>();

        if (instance == null)
        {
            GameObject gameObj = new GameObject();
            gameObj.name = "GameManager";
            instance = gameObj.AddComponent<ManagingGame>();
        }
    }
    #endregion
    private void Start()
    {
        ResetGame();

        if (Application.platform == RuntimePlatform.Android)
        {
            mobileControls.SetActive(true);
        }
    }
    private void Update()
    {
        if (_gameStarted)
        {
            if (_currentTime <= 0)
                CheckIfPlayerWon(!_gameStarted);
            else
                _currentTime -= Time.deltaTime;

            UpdateTimerDisplay();
        }
    }
    #region Utility Methods

    private void PointManagement(bool goodOrBad)
    {
        _points += goodOrBad ? 1 : -1; // Adds or subtracts points depending on whether ghost/nightmare entered a house

        UpdatePointsSlider();

        CheckIfPlayerWon(!_gameStarted);
    }
    private void CheckIfPlayerWon(bool gameEnded)
    {
        if ((!gameEnded && _points >= pointsToEndGame) || (gameEnded && _points >= 0))
        {
            // Won game
            EndGame();
            endScreen.SetActive(true);
            winMenu.SetActive(true);
            Debug.Log("You won!");
        }
        else if ((!gameEnded && _points <= -pointsToEndGame) || (gameEnded && _points < 0))
        {
            // Lost game
            EndGame();
            endScreen.SetActive(true);
            loseMenu.SetActive(true);
            Debug.Log("You lost womp womp");
        }
    }
    private void EndGame()
    {
        Cursor.visible = true;
        Time.timeScale = 0;
        _gameStarted = false;
        Spawning.SetActive(false);
        InputManager.SetActive(false);
    }

    public void ResetGame()
    {
        Cursor.visible = true;
        Time.timeScale = 1;
        _currentTime = gameDuration;
        _points = 0;

        // Set slider min and max value to points max
        pointSlider.minValue = -pointsToEndGame;
        pointSlider.maxValue = pointsToEndGame;

        UpdatePointsSlider();
        UpdateTimerDisplay();
    }

    public void BackToGame()
    {
        Cursor.visible = true;
        SceneManager.LoadScene("GameScene");
    }
    public List<float> GetEnemySettings()
    {
        List<float> settings = new List<float>();
        settings.Add(sliderIncrement);
        settings.Add(minWidth);
        settings.Add(maxWidth);
        settings.Add(maxPurplePos);
        settings.Add(redFlashDuration);
        settings.Add(enemySpeed);
        return settings;
    }
    #endregion
    #region UI Methods

    public void GameStart()
    {
        Cursor.visible = false;
        _gameStarted = true;

        Spawning.SetActive(true);
        InputManager.SetActive(true);
        mainMenuCamera.SetActive(false);
        HUD.SetActive(true);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    private void UpdateTimerDisplay()
    {
        var ts = TimeSpan.FromSeconds(_currentTime);
        timerText.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
    }
    private void UpdatePointsSlider()
    {
        pointSlider.value = _points;
    }
    public void PauseMenu()
    {
        HUD.SetActive(!HUD.activeSelf);
        pauseMenu.SetActive(!pauseMenu.activeSelf);
        Cursor.visible = pauseMenu.activeSelf;
        Time.timeScale = Time.timeScale == 1 ? 0 : 1;
    }
    #endregion

    #region Subscribing To Events
    private void OnEnable()
    {
        EnemyScript.GhostArrived += PointManagement;
        InputScript.pauseMenuTriggered += PauseMenu;
    }
    private void OnDisable()
    {
        EnemyScript.GhostArrived -= PointManagement;
        InputScript.pauseMenuTriggered -= PauseMenu;
    }
    #endregion
}
