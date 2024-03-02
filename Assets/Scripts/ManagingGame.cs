using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagingGame : MonoBehaviour
{
    [Header("Objects To Turn On After Starting Game")]
    [SerializeField]
    private GameObject Spawning;
    [SerializeField]
    private GameObject InputManager;

    [Header("Objects To Turn Off After Starting Game")]
    [SerializeField]
    private GameObject mainMenuCamera;

    [Header("Game Settings")]
    [SerializeField]
    private float gameDuration;
    [SerializeField]
    private int pointsToEndGame;

    private int points = 0;

    public void GameStart()
    {

        StartCoroutine(GameTimer(gameDuration));
        Spawning.SetActive(true);
        InputManager.SetActive(true);
        mainMenuCamera.SetActive(false);
    }

    private void PointManagement(bool goodOrBad)
    {
        points += goodOrBad ? 1 : -1; // Adds or subtracts points depending on whether ghost/nightmare entered a house

        if (points >= pointsToEndGame)
        {
            // Won game
            GameEnd();
            Debug.Log("You won!");
        }
        else if (points <= -pointsToEndGame)
        {
            // Lost game
            GameEnd();
            Debug.Log("You lost womp womp");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void GameEnd()
    {
        Spawning.SetActive(false);
        InputManager.SetActive(false);
    }

    IEnumerator GameTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        GameEnd();
    }
    private void OnEnable()
    {
        EnemyScript.GhostArrived += PointManagement;
    }
    private void OnDisable()
    {
        EnemyScript.GhostArrived -= PointManagement;
    }
}
