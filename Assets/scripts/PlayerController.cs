using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(JumpFeature))]
[RequireComponent(typeof(InputController))]
public class Player_Controller : TankParent
{
    private InputController _inputController;
    private Vector3 velocity;
    private Vector2 keyData;
    private bool isRotating, isDriving;

    private void Awake()
    {
        keyData = new Vector2 (0, 0);
        isRotating = false;
        isDriving = false;
    }

    private void Start()
    {
        _inputController = GetComponent<InputController>();
        _inputController.onWASD += HandleWASD;
        _inputController.onSpacebar += Jump;
    }

    private void HandleWASD(InputAction.CallbackContext ctx)
    {
        keyData = ctx.ReadValue<Vector2>();
        isRotating = (keyData.x != 0) ? true : false;
        isDriving = (keyData.y != 0) ? true : false;
    }

    private void Jump(InputAction.CallbackContext ctx)
    {
        Debug.Log($"Spacebar! {ctx}");
    }

    private void FixedUpdate()
    {
        if (isRotating)
        {
            Rotate(keyData.x);
        }
        if (isDriving)
        {
            Drive(keyData.y);
        }
    }
}

