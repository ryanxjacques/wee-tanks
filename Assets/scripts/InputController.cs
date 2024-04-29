using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public Action<InputAction.CallbackContext> onWASD;
    public Action<InputAction.CallbackContext> onSpacebar;

    public void WASD(InputAction.CallbackContext ctx)
    {
        onWASD?.Invoke(ctx);
    }

    public void Spacebar(InputAction.CallbackContext ctx)
    {
        onSpacebar?.Invoke(ctx);
    }
}
