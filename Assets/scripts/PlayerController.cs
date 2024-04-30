using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/* Comment's Date: 30th April 2024
 * The PlayerController class defines the player game object. It is a
 * derivative of the TankParent component. The PlayerController
 * communicates with the InputController and JumpFeature.
 */
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
    private AudioSource _audioSource;

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
        _audioSource = GetComponent<AudioSource>();
    }

    // Called by InputController invoking the action 'onWASD'.
    // KeyData looks like a 2D tuple (x, y).
    // For x = 1  : D is pressed
    // For x = -1 : A is pressed
    // For y = 1  : W is pressed
    // For y = -1 : S is pressed.
    private void HandleWASD(InputAction.CallbackContext ctx)
    {
        keyData = ctx.ReadValue<Vector2>();
        Debug.Log(keyData);
        isRotating = (keyData.x != 0) ? true : false;
        isDriving = (keyData.y != 0) ? true : false;
    }

    // Called by JumpFeature invoking the action 'onJump'.
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

    // Called by InputController invoking the action 'onSpacebar'.
    private void Jump(InputAction.CallbackContext ctx)
    {
        InputActionPhase spacebarPhase = ctx.phase;
        if (spacebarPhase == InputActionPhase.Started) // Spacebar initiated
        {
            moveEnabled = false;
            _jumpFeature.DrawProjection(transform.position, forward, speed);
        }
        if (spacebarPhase == InputActionPhase.Canceled) // Spacebar released
        {
            _audioSource.Play();
            moveEnabled = true;
            _jumpFeature.Jump(_rigidbody, forward, speed);
        }

        Debug.Log($"Spacebar! {spacebarPhase}");
    }

    // Renable Movemnet after hitting the ground.
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

