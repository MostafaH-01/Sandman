using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
    [Header("Game Settings")]
    [SerializeField] public float gameDuration = 180f;
    [SerializeField] public int pointsToEndGame = 8;
    [SerializeField] public string language = "ar";
    [Header("Volume Settings")]
    [SerializeField] public float camSensitivity = 110f;
    [SerializeField] public float musicVolume = 0.5f;
    [SerializeField] public float sfxVolume = 0.5f;
    [Header("Quality Settings")]
    [SerializeField] public int renderAssetPipelineIndex = 0;
}
