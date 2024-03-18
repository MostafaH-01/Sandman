using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private AudioClip success;
    [SerializeField]
    private AudioClip fail;

    private AudioSource audioSource;
    #endregion

    #region Start
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    #endregion

    #region Audio Methods
    private void playerSuccess(bool playerAttempt)
    {
        if(playerAttempt)
        {
            audioSource.PlayOneShot(success);
        }
        else
        {
            audioSource.PlayOneShot(fail);
        }
    }
    #endregion

    #region OnEnable and Disable
    private void OnEnable()
    {
        EnemyScript.GhostArrived += playerSuccess;
    }

    private void OnDisable()
    {
        EnemyScript.GhostArrived -= playerSuccess;
    }
    #endregion
}
