using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationManager : MonoBehaviour
{
    [Header("Arabic UI Elements")]
    [SerializeField]
    private List<GameObject> arabicElements;

    [Header("English UI Elements")]
    [SerializeField]
    private List<GameObject> englishElements;

    [Header("Language Buttons")]
    [SerializeField]
    private Button arabicButton;
    [SerializeField]
    private Button englishButton;
    private void Awake()
    {
        switch (ManagingGame.Instance.GameData.language)
        {
            case "ar":
                arabicButton.onClick.Invoke();
                break;
            case "en":
                englishButton.onClick.Invoke();
                break;
        }
    }

    public void ToggleLanguage(string language)
    {
        ManagingGame.Instance.GameData.language = language;
        if (language == "ar")
        {
            foreach (GameObject element in arabicElements)
            {
                element.SetActive(true);
            }
            foreach (GameObject element in englishElements)
            {
                element.SetActive(false);
            }
        }
        else if (language == "en")
        {
            foreach (GameObject element in arabicElements)
            {
                element.SetActive(false);
            }
            foreach (GameObject element in englishElements)
            {
                element.SetActive(true);
            }
        }
    }
}
