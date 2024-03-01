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
    [Header("Minigame Settings")]
    [SerializeField]
    private float sliderIncrement;
    [SerializeField]
    private float lowerBoundSliderWin;
    [SerializeField]
    private float upperBoundSliderWin;

    private bool activated = false; // Minigame Started
    private bool rightLeftFlag = false; // Left to right if true, right to left if false
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
        ResetMiniGame();
        activated = true;
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
        return false;
    }

    // When player enters or exits trigger area
    public void ShowOrHidePopup()
    {
        ResetMiniGame();
        canvasParent.SetActive(!canvasParent.activeSelf);
    }

    private void ResetMiniGame()
    {
        slider.value = 1;
        rightLeftFlag = false;
    }
}
