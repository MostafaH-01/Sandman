using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class InputInvoker
{
    #region Events

    public static event Action<Vector2> OnMovement;
    public static event Action OnStartInteract;
    public static event Action OnInteractPerformed;
    public static event Action OnPause;

    #endregion

    #region Invoke Methods

    public static void Move(this InputScript inputScript, Vector2 direction) => OnMovement?.Invoke(direction);
    public static void StartTrigger(this InputScript inputScript) => OnStartInteract?.Invoke();
    public static void TriggerPerformed(this InputScript inputScript) => OnInteractPerformed?.Invoke();
    public static void Pause(this InputScript inputScript) => OnPause?.Invoke();

    #endregion
}
