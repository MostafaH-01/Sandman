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
    [Header("Minigame Settings")]
    [SerializeField]
    private float sliderIncrement;
    [SerializeField]
    private float lowerBoundSliderWin;
    [SerializeField]
    private float upperBoundSliderWin;
    [SerializeField]
    private float redFlashDuration = 0.2f;

    private bool activated = false; // Minigame Started
    private bool rightLeftFlag = false; // Left to right if true, right to left if false
    private bool converted = false;
    private Color _colorRed = Color.red;
    #endregion

    void Update()
    {
        if (activated)
        {
            // Increase if going to right, decrease if going to left
            slider.value += rightLeftFlag ? sliderIncrement : -sliderIncrement;

            // Clamping between 0 and 1 and keeping it rounded 2 digits after decimal
            slider.value = Mathf.Clamp01((float)Math.Round(slider.value, 2, MidpointRounding.AwayFromZero));

            // When slider reaches max, go opposite direction
            if (slider.value == 1)
                rightLeftFlag = false;
            else if (slider.value == 0)
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

    public void ConvertGhost()
    {
        converted = true;
        canvasParent.SetActive(false);
    }

    private void ResetMiniGame()
    {
        slider.value = 1;
        rightLeftFlag = false;
    }
    private IEnumerator FlashRoutine()
    {
        sliderCenterFillImage.color = _colorRed;
        yield return new WaitForSeconds(redFlashDuration);
        sliderCenterFillImage.color = Color.white;
    }
    private void OnDisable()
    {
        converted = false;
    }
}
