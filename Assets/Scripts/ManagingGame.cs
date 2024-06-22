using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
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
    [SerializeField]
    private GameObject DayNightCycle;
    [SerializeField]
    private Highscore highscoreManager;

    [Header("Objects To Turn Off After Starting Game")]
    [SerializeField]
    private GameObject mainMenuCamera;

    [Header("UI Elements")]
    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    private TextMeshProUGUI defeatedCount;
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

    [Header("Mobile UI")]
    [SerializeField]
    private GameObject mobileControls;
    [SerializeField]
    private GameObject arabicMobileInstructions;
    [SerializeField]
    private GameObject englishMobileInstructions;

    [Header ("Desktop UI")]
    [SerializeField]
    private GameObject arabicDesktopInstructions;
    [SerializeField]
    private GameObject englishDesktopInstructions;

    [Header("Controller UI")]
    [SerializeField]
    private GameObject arabicControllerInstructions;
    [SerializeField]
    private GameObject englishControllerInstructions;

    [Header("Highscore Menu")]
    [SerializeField]
    private GameObject highscoreTable;
    [SerializeField]
    private GameObject enterNamePanel;
    [SerializeField]
    private TMP_InputField inputtedName;

    [Header("Game Settings")]
    [SerializeField]
    private GameData gameData;
    [SerializeField]
    private GameData defaultData;
    private float gameDuration;
    private int pointsToEndGame;

    [Header("For Settings Menu")]
    [SerializeField]
    private Slider sensitivitySlider;
    [SerializeField]
    private CinemachineFreeLook playerCamera;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private AudioSource musicSource;
    [SerializeField]
    private Slider sfxSlider;
    [SerializeField]
    private AudioSource sfxSource;
    [SerializeField]
    private TMP_Dropdown renderQualityDropdown;
    [SerializeField]
    private List<RenderPipelineAsset> renderAssets;

    [Header("For Settings In Pause Menu")]
    [SerializeField]
    private Slider sensitivityPauseSlider;
    [SerializeField]
    private Slider musicPauseSlider;
    [SerializeField]
    private Slider sfxPauseSlider;

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
    private int enemiesDefeated = 0;
    private float _currentTime;
    private bool _gameStarted = false;
    private string _playerName;

    private bool _android = false;

    public GameData GameData
    {
        get
        {
            return gameData;
        }
    }
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
        SetSettingsOnStart();
        ResetGame();
        FillScoreTable();

        if (Application.platform == RuntimePlatform.Android)
        {
            mobileControls.SetActive(true);
            arabicMobileInstructions.SetActive(true);
            englishMobileInstructions.SetActive(true);

            arabicDesktopInstructions.SetActive(false);
            englishDesktopInstructions.SetActive(false);

            _android = true;
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
    private void Update()
    {
        if (_gameStarted)
        {
            if (_currentTime <= 0)
                CheckIfPlayerWon(true);
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
        }
        else if ((!gameEnded && _points <= -pointsToEndGame) || (gameEnded && _points < 0))
        {
            // Lost game
            EndGame();
            endScreen.SetActive(true);
            loseMenu.SetActive(true);
        }
    }
    private void EndGame()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        _gameStarted = false;
        HUD.SetActive(false);
        Spawning.SetActive(false);
        InputManager.SetActive(false);

        HandleHighscore();
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

    private void HandleHighscore()
    {
        if (highscoreManager.CheckNewHighScore(enemiesDefeated))
        {
            enterNamePanel.SetActive(true);
            highscoreTable.transform.parent.gameObject.SetActive(false);
        }    
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
        Cursor.lockState = CursorLockMode.Locked;

        _gameStarted = true;

        Spawning.SetActive(true);
        InputManager.SetActive(true);
        mainMenuCamera.SetActive(false);
        HUD.SetActive(true);
        DayNightCycle.SetActive(true);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void ChangeSelectedButton(GameObject button)
    {
        if (!_android)
        {
            if (button != null && button.name != "Dummy")
                EventSystem.current.SetSelectedGameObject(button);
            else
                EventSystem.current.SetSelectedGameObject(null);
        }
    }
    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is Gamepad gamepad)
        {
            arabicControllerInstructions.SetActive(true);
            englishControllerInstructions.SetActive(true);

            arabicDesktopInstructions.SetActive(false);
            englishDesktopInstructions.SetActive(false);
        }
        if (device is Keyboard keyboard)
        {
            arabicDesktopInstructions.SetActive(true);
            englishDesktopInstructions.SetActive(true);

            arabicControllerInstructions.SetActive(false);
            englishControllerInstructions.SetActive(false);
        }
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
    private void IncrementDefeated()
    {
        enemiesDefeated++;
        defeatedCount.text = enemiesDefeated.ToString();
    }
    public void PauseMenu()
    {
        SetSettingsSliderFromPrefs("Pause");

        HUD.SetActive(!HUD.activeSelf);
        pauseMenu.SetActive(!pauseMenu.activeSelf);

        if (pauseMenu.activeSelf)
        {
            ChangeSelectedButton(sensitivityPauseSlider.gameObject);
        }
        else
        {
            ChangeSelectedButton(null);
        }

        Cursor.visible = pauseMenu.activeSelf;
        Cursor.lockState = pauseMenu.activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
        Time.timeScale = Time.timeScale == 1 ? 0 : 1;
    }

    public void DoneInputtingName()
    {
        _playerName = inputtedName.text;
        enterNamePanel.SetActive(false);
        highscoreTable.transform.parent.gameObject.SetActive(true);

        highscoreManager.AddNewScore(_playerName, enemiesDefeated);

        FillScoreTable();
    }

    public void ChangeSensitivty(string menu)
    {
        if (menu == "Settings")
        {
            playerCamera.m_XAxis.m_MaxSpeed = sensitivitySlider.value;
            gameData.camSensitivity = sensitivitySlider.value;
        }
        else if (menu == "Pause")
        {
            playerCamera.m_XAxis.m_MaxSpeed = sensitivityPauseSlider.value;
            gameData.camSensitivity = sensitivityPauseSlider.value;
        }
    }

    public void ChangeMusicVolume(string menu)
    {
        if (menu == "Settings")
        {
            musicSource.volume = musicSlider.value;
            gameData.musicVolume = musicSlider.value;
        }
        else if (menu == "Pause")
        {
            musicSource.volume = musicPauseSlider.value;
            gameData.musicVolume = musicPauseSlider.value;
        }
    }

    public void ChangeSfxVolume(string menu)
    {
        if (menu == "Settings")
        {
            sfxSource.volume = sfxSlider.value;
            gameData.sfxVolume = sfxSlider.value;
        }
        else if (menu == "Pause")
        {
            sfxSource.volume = sfxPauseSlider.value;
            gameData.sfxVolume = sfxPauseSlider.value;
        }
    }

    public void ChangeQuality(Int32 dropDownValue)
    {
        GraphicsSettings.renderPipelineAsset = renderAssets[dropDownValue];

        gameData.renderAssetPipelineIndex = dropDownValue;
    }
    public void SetSettingsSliderFromPrefs(string menu)
    {
        if (menu == "Settings")
        {
            sensitivitySlider.value = gameData.camSensitivity;
            musicSlider.value = gameData.musicVolume;
            sfxSlider.value = gameData.sfxVolume;
        }
        else if (menu == "Pause")
        {
            sensitivityPauseSlider.value = gameData.camSensitivity;
            musicPauseSlider.value = gameData.musicVolume;
            sfxPauseSlider.value = gameData.sfxVolume;
        }

        renderQualityDropdown.value = gameData.renderAssetPipelineIndex;
    }

    private void SetSettingsOnStart()
    {
        SetSettingsSliderFromPrefs("Settings");
        ChangeSensitivty("Settings");
        ChangeMusicVolume("Settings");
        ChangeSfxVolume("Settings");

        gameDuration = gameData.gameDuration;
        pointsToEndGame = gameData.pointsToEndGame;
    }
    private void FillScoreTable()
    {
        List<Scorer> scoreList = highscoreManager.GetScoreList();
        GameObject row;
        for (int i = 0; i < scoreList.Count; i++)
        {
            row = highscoreTable.transform.GetChild(i + 1).gameObject;
            row.SetActive(true);

            row.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = scoreList[i].name;
            row.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = scoreList[i].score.ToString();
        }
    }
    #endregion

    #region Subscribing To Events
    private void OnEnable()
    {
        EnemyScript.GhostArrived += PointManagement;
        EnemyScript.EnemyDefeated += IncrementDefeated;
        InputInvoker.OnPause += PauseMenu;

        InputSystem.onDeviceChange += OnDeviceChange;
    }
    private void OnDisable()
    {
        EnemyScript.GhostArrived -= PointManagement;
        EnemyScript.EnemyDefeated -= IncrementDefeated;
        InputInvoker.OnPause -= PauseMenu;

        InputSystem.onDeviceChange -= OnDeviceChange;
    }
    #endregion
}
