using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputScript : MonoBehaviour
{
    #region Variables
    [Header("Player Controls")]
    [SerializeField]
    private InputActionReference move;
    [SerializeField]
    private InputActionReference convert;
    [SerializeField]
    private InputActionReference pause;

    [Header("Player Script")]
    [SerializeField]
    private PlayerActions playerActions;

    private Vector2 _movement;

    private Action<InputAction.CallbackContext> convertTrigger;
    private Action<InputAction.CallbackContext> pauseTrigger;

    public static event Action pauseMenuTriggered;
    #endregion
    public Vector2 Movement
    {
        get
        {
            return _movement;
        }
    }
    private void Awake()
    {
        convertTrigger = (ctx) => convertBtnPressed();
        pauseTrigger = (ctx) => PauseButtonPressed();
    }

    // Update is called once per frame
    void Update()
    {
        MovementInput();
    }
    #region Event Handlers
    private void MovementInput()
    {
        _movement = move.action.ReadValue<Vector2>();
    }

    private void convertBtnPressed()
    {
        // Check if pointer is in right position or not
        playerActions.CheckNightmareSuccess();
    }

    private void convertBtnHeld(InputAction.CallbackContext context)
    {
        // Start minigame
        playerActions.StartConvertNightmare();
    }

    private void PauseButtonPressed()
    {
        pauseMenuTriggered?.Invoke();
    }
    #endregion
    #region Event Subscribing
    private void OnEnable()
    {
        pause.action.performed += pauseTrigger;
    }
    private void OnDisable()
    {
        pause.action.performed -= pauseTrigger;
    }
    public void SubscribeToConvert()
    {
        convert.action.started += convertBtnHeld;
        convert.action.performed += convertTrigger;
        convert.action.canceled += convertTrigger;
    }
    public void UnsubscribeFromConvert()
    {
        convert.action.started -= convertBtnHeld;
        convert.action.performed -= convertTrigger;
        convert.action.canceled -= convertTrigger;
    }
    #endregion
}
