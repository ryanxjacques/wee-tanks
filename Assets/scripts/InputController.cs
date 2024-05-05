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

public struct Button
{
    public string name;
    public bool isDown;
    public float value;
} 


public class InputController : MonoBehaviour
{
    public Action<Button> onButton;

    private Button CreateButtonStruct(string name, InputActionPhase phase, float value=0)
    {
        Button button = new Button();
        button.name = name;
        button.value = value;
        if (phase == InputActionPhase.Started || phase == InputActionPhase.Performed)
            button.isDown = true;
        else if (phase == InputActionPhase.Canceled)
            button.isDown = false;
        return button;
    }

    public void DriveCompositeButton(InputAction.CallbackContext ctx)
    {
        Button driveBtn = CreateButtonStruct("Drive", ctx.phase, ctx.ReadValue<float>());
        onButton?.Invoke(driveBtn);
    }

    public void RotateCompositeButton(InputAction.CallbackContext ctx)
    {
        Button rotateBtn = CreateButtonStruct("Rotate", ctx.phase, ctx.ReadValue<float>());
        onButton?.Invoke(rotateBtn);
    }

    public void JumpButton(InputAction.CallbackContext ctx)
    {
        Button jumpButton = CreateButtonStruct("Jump", ctx.phase);
        onButton?.Invoke(jumpButton);
    }
}
