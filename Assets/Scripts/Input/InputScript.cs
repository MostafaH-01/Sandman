using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputScript : MonoBehaviour
{
    private InputAction triggerAction;
    private void Awake()
    {
        var playerInput = GetComponent<PlayerInput>();

        triggerAction = playerInput.actions["Interact"];
    }
    private void OnMovement(InputValue value)
    {
        this.Move(value.Get<Vector2>());
    }
    private void OnPause(InputValue value)
    {
        this.Pause();
    }
    private void StartTrigger(InputAction.CallbackContext context)
    {
        this.StartTrigger();
    }
    private void TriggerPerformed(InputAction.CallbackContext context)
    {
        this.TriggerPerformed();
    }
    private void OnEnable()
    {
        triggerAction.started += StartTrigger;
        triggerAction.performed += TriggerPerformed;
        triggerAction.canceled += TriggerPerformed;
    }
    private void OnDisable()
    {
        triggerAction.started -= StartTrigger;
        triggerAction.performed -= TriggerPerformed;
        triggerAction.canceled -= TriggerPerformed;
    }
}
