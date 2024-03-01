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

    [Header("Player Script")]
    [SerializeField]
    private PlayerActions playerActions;

    private Vector2 _movement;

    private Action<InputAction.CallbackContext> convertTrigger;
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
    #endregion
    #region Event Subscribing
    private void OnEnable()
    {
        convert.action.started += convertBtnHeld;
        convert.action.performed += convertTrigger;
        convert.action.canceled += convertTrigger;
    }
    private void OnDisable()
    {
        convert.action.started -= convertBtnHeld;
        convert.action.performed -= convertTrigger;
        convert.action.canceled -= convertTrigger;
    }
    #endregion
}
