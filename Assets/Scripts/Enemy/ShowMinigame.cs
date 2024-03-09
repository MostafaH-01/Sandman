using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowMinigame : MonoBehaviour
{
    #region Variables
    [Header("Components")]
    [SerializeField]
    private GameObject canvasParent;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Image sliderCenterFillImage;
    [SerializeField]
    private RectTransform purpleImageTransform;
    [Header("Minigame Settings")]
    [SerializeField]
    private float sliderIncrement;
    [SerializeField]
    private float minWidth = 0.08f;
    [SerializeField]
    private float maxWidth = 0.17f;
    [SerializeField]
    private float maxPurplePos = 0.57f;
    [SerializeField]
    private float redFlashDuration = 0.2f;

    private float lowerBoundSliderWin;
    private float upperBoundSliderWin;
    private float purpleImageWidth;
    private float purpleImagePos;
    private bool activated = false; // Minigame Started
    private bool rightLeftFlag = false; // Left to right if true, right to left if false
    private bool converted = false;
    private Color _colorRed = Color.red;
    #endregion

    private void Awake()
    {
        GetSettingsFromManager();
    }
    void Update()
    {
        if (activated)
        {
            // Increase if going to right, decrease if going to left
            slider.value += rightLeftFlag ? sliderIncrement : -sliderIncrement;

            // Clamping between 0 and 1 and keeping it rounded 2 digits after decimal
            slider.value = Mathf.Clamp((float)Math.Round(slider.value, 2, MidpointRounding.AwayFromZero), -maxPurplePos, maxPurplePos);

            // When slider reaches max, go opposite direction
            if (slider.value == maxPurplePos)
                rightLeftFlag = false;
            else if (slider.value == -maxPurplePos)
                rightLeftFlag = true;
        }
    }
    // When player holds mouse
    public void StartConvertNightmare ()
    {
        if (!converted)
        {
            ResetMiniGame();
            activated = true;
        }
    }
    public void StopConvertNightmare()
    {
        ResetMiniGame();
        activated = false;
    }

    // Check if player won
    public bool CheckPlayerSuccess()
    {
        if (slider.value >= lowerBoundSliderWin && slider.value <= upperBoundSliderWin)
            return true;
        StartCoroutine(FlashRoutine());
        return false;
    }

    // When player enters or exits trigger area
    public void ShowOrHidePopup()
    {
        if (!converted)
        {
            ResetMiniGame();
            canvasParent.SetActive(!canvasParent.activeSelf);
        }
    }
    // Make a ghost good
    public void ConvertGhost()
    {
        converted = true;
        canvasParent.SetActive(false);
    }

    private void ResetMiniGame()
    {
        slider.value = slider.maxValue;
        rightLeftFlag = false;
    }
    // Randomize purple center's width and position and assign winning parameters
    private void RandomizePurpleCenter()
    {
        purpleImageWidth = UnityEngine.Random.Range(minWidth, maxWidth);
        purpleImagePos = UnityEngine.Random.Range(-maxPurplePos, maxPurplePos);

        purpleImageTransform.anchoredPosition = new Vector2(purpleImagePos, purpleImageTransform.anchoredPosition.y);
        purpleImageTransform.sizeDelta = new Vector2 (purpleImageWidth, purpleImageTransform.sizeDelta.y);

        lowerBoundSliderWin = purpleImagePos - purpleImageWidth / 2;
        upperBoundSliderWin = purpleImagePos + purpleImageWidth / 2;
    }
    // Get enemy settings from Game Manager
    private void GetSettingsFromManager()
    {
        List<float> settings = ManagingGame.Instance.GetEnemySettings();

        sliderIncrement = settings[0];
        minWidth = settings[1];
        maxWidth = settings[2];
        maxPurplePos = settings[3];
        redFlashDuration = settings[4];
        
    }
    // Flashes red if player fails
    private IEnumerator FlashRoutine()
    {
        sliderCenterFillImage.color = _colorRed;
        yield return new WaitForSeconds(redFlashDuration);
        sliderCenterFillImage.color = Color.white;
    }
    private void OnEnable()
    {
        RandomizePurpleCenter();
    }
    private void OnDisable()
    {
        RandomizePurpleCenter();
        converted = false;
    }
}
