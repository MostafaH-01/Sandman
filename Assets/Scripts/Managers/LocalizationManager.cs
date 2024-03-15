using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    [Header("Arabic UI Elements")]
    [SerializeField]
    private List<GameObject> arabicElements;

    [Header("English UI Elements")]
    [SerializeField]
    private List<GameObject> englishElements;

    public void ToggleLanguage(string language)
    {
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
