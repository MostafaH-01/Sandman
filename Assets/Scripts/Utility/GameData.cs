using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
    [SerializeField] public string language = "ar";
    [SerializeField] public float camSensitivity = 110f;
    [SerializeField] public float musicVolume = 0.5f;
    [SerializeField] public float sfxVolume = 0.5f;
    [SerializeField] public int renderAssetPipelineIndex = 0;
}
