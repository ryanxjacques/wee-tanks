using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(JumpFeature))]
[RequireComponent(typeof(InputController))]
public class Player_Controller : TankParent
{
    private JumpFeature _jumpFeature;
    private InputController _inputController;
    private Rigidbody _rigidbody;
    private Vector3 velocity;
    private Vector2 keyData;
    private bool isRotating, isDriving, moveEnabled;
    public AudioSource aud;
    //private AudioSource aud;

    private void Awake()
    {
        keyData = new Vector2 (0, 0);
        isRotating = false;
        isDriving = false;
        moveEnabled = true;
    }

    private void Start()
    {
        _inputController = GetComponent<InputController>();
        _inputController.onWASD += HandleWASD;
        _inputController.onSpacebar += Jump;
        _jumpFeature = GetComponent<JumpFeature>();
        _jumpFeature.onJump += HandleJump;
        _rigidbody = GetComponent<Rigidbody>();
        aud = GetComponent<AudioSource>();
    }

    private void HandleWASD(InputAction.CallbackContext ctx)
    {
        keyData = ctx.ReadValue<Vector2>();
        isRotating = (keyData.x != 0) ? true : false;
        isDriving = (keyData.y != 0) ? true : false;
    }

    private void HandleJump(string msg)
    {
        if (msg == "Started")
        {
            moveEnabled = false;
        }
        if (msg == "Ended")
        {
            moveEnabled = true;
        }
    }

    private void Jump(InputAction.CallbackContext ctx)
    {
        InputActionPhase spacebarPhase = ctx.phase;
        if (spacebarPhase == InputActionPhase.Started)
        {
            moveEnabled = false;
            _jumpFeature.DrawProjection(transform.position, forward, speed);
        }
        if (spacebarPhase == InputActionPhase.Canceled)
        {
            aud.Play();
            moveEnabled = true;
            _jumpFeature.Jump(_rigidbody, forward, speed);
        }

        Debug.Log($"Spacebar! {spacebarPhase}");
    }

    // Enable Moving after hitting the ground.
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Solid"))
        {
            moveEnabled = true;
        }
    }

    private void FixedUpdate()
    {
        if (!moveEnabled)
            return; 

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

