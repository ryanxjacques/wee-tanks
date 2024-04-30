using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/* Comment's Date: 30th April 2024
 * The InputController component collects user input using Unity's InputSystem.
 * The configuration for user input is found in:
 *      Assets/InputSystem/PlayerInputSystem.inputactions.
 * The configurations are attached to the player's game object.
 */
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
