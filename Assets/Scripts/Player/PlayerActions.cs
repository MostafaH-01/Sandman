using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    #region Variables
    [Header("Movement")]
    [SerializeField]
    private float speed = 17;
    [SerializeField]
    private InputScript inputScript;

    private Vector3 _movement;
    private Rigidbody _rb;
    private ShowMinigame _showMinigame; // Script of enemy near player
    private EnemyScript _enemyScript;

    public static event Action PlayerSucceeded;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _rb = transform.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (inputScript.Movement != Vector2.zero)
        {
            Move();
        }
    }

    private void Move()
    {
        // Get the forward direction of the camera
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0f; // Zero out the y component to ensure movement is in the horizontal plane

        // Get the right direction of the camera
        Vector3 cameraRight = Camera.main.transform.right;
        cameraRight.y = 0f;

        // Combine the forward and right directions based on input
        Vector3 movementDirection = (cameraForward * inputScript.Movement.y + cameraRight * inputScript.Movement.x).normalized;

        // Set the movement vector
        _movement = new Vector3(movementDirection.x, 0, movementDirection.z);

        _rb.velocity = _movement * speed * Time.deltaTime;

        // Look away from the camera (same direction as it's pointing)
        this.transform.LookAt(transform.position - (Camera.main.transform.position - transform.position));
    }

    public void StartConvertNightmare()
    {
        if (_showMinigame != null)
        {
            _showMinigame.StartConvertNightmare();
        }
    }
    public void CheckNightmareSuccess()
    {
        bool success = false;
        if (_showMinigame != null)
        {
            success = _showMinigame.CheckPlayerSuccess();

            if (success)
            {
                _showMinigame.ConvertGhost();
                _enemyScript.OnPlayerSuccess();
                //PlayerSucceeded?.Invoke();
            }

            Debug.Log(success ? "You did it!" : "You didn't do it...");

            _showMinigame.StopConvertNightmare();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Nightmare")
        {
            _enemyScript = other.GetComponent<EnemyScript>();
            _showMinigame = other.GetComponent<ShowMinigame>();
            _showMinigame.ShowOrHidePopup();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Nightmare")
        {
            _showMinigame.StopConvertNightmare();
            _showMinigame.ShowOrHidePopup();
            _enemyScript = null;
            _showMinigame = null; // Empty out enemy script reference
        }
    }
}
