using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour
{
    #region Variables
    [Header("Movement")]
    [SerializeField]
    private float speed = 17;

    private Vector2 _tempMovement;

    [Header("Camera Settings")]
    [SerializeField]
    private Camera _camera;

    private Vector3 _movement;
    private Rigidbody _rb;
    private ShowMinigame _showMinigame; // Script of enemy near player
    private EnemyScript _enemyScript;

    private bool _inMiniGame = false;
    private int _ghostsAroundPlayerCount = 0;

    private Vector3 _cameraForward, _cameraRight, _movementDirection, _moveReturn;

    public static event Action PlayerSucceeded;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        _rb = transform.GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_tempMovement != Vector2.zero)
        {
            Move();
        }
    }

    private void Move()
    {
        // Get the forward direction of the camera
        _cameraForward = _camera.transform.forward;
        _cameraForward.y = 0f; // Zero out the y component to ensure movement is in the horizontal plane

        // Get the right direction of the camera
        _cameraRight = _camera.transform.right;
        _cameraRight.y = 0f;

        // Combine the forward and right directions based on input
        _movementDirection = (_cameraForward * _tempMovement.y + _cameraRight * _tempMovement.x).normalized;

        // Set the movement vector
        _movement = new Vector3(_movementDirection.x, 0, _movementDirection.z);

        _rb.velocity = _movement * speed * Time.deltaTime;

        // Look away from the camera (same direction as it's pointing)
        transform.LookAt(transform.position - (_camera.transform.position - transform.position));
    }

    private void OnMove(Vector2 moveDirection)
    {
        _tempMovement = moveDirection;
    }

    private void StartTrigger ()
    {
        if (_inMiniGame)
        {
            StartConvertNightmare();
        }
    }
    private void TriggerPerformed()
    {
        if (_inMiniGame)
            CheckNightmareSuccess();
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

                _inMiniGame = false;

                _enemyScript = null;
                _showMinigame = null; // Empty out enemy script reference
            }

            else
                _showMinigame.StopConvertNightmare();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Nightmare")
        {
            // Keeps track of how many ghosts the player is around. This helps open only one mini game at a time
            _ghostsAroundPlayerCount++;
            if (!_inMiniGame)
            {
                _inMiniGame = true;

                _enemyScript = other.transform.parent.GetComponent<EnemyScript>();
                _showMinigame = other.GetComponent<ShowMinigame>();
                _showMinigame.ShowOrHidePopup(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Nightmare")
        {
            if (_ghostsAroundPlayerCount == 1)
            {
                _inMiniGame = false;
                //inputScript.UnsubscribeFromConvert();

                _showMinigame.StopConvertNightmare();
                _showMinigame.ShowOrHidePopup(false);
                _enemyScript = null;
                _showMinigame = null; // Empty out enemy script reference
            }
            _ghostsAroundPlayerCount--;
        }
    }
    private void OnEnable()
    {
        InputInvoker.OnMovement += OnMove;
        InputInvoker.OnStartInteract += StartTrigger;
        InputInvoker.OnInteractPerformed += TriggerPerformed;
    }
    private void OnDisable()
    {
        InputInvoker.OnMovement -= OnMove;
        InputInvoker.OnStartInteract -= StartTrigger;
        InputInvoker.OnInteractPerformed -= TriggerPerformed;
    }
}
