using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputController))]
public class ShooterController : TankParent
{
    private InputController _inputController;
    private Direction direction = new Direction();
    // Start is called before the first frame update
    void Start()
    {
        _inputController = GetComponent<InputController>();
        _inputController.onButton += OnButtonObserver;
    }

    private void OnButtonObserver(Button button)
    {
        if (button.name == "Drive")
        {
            SetState(State.IsDriving, button.isDown);
            direction.drive = button.value;
        }
        if (button.name == "Rotate")
        {
            SetState(State.IsRotating, button.isDown);
            direction.rotate = button.value; 
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (CheckState(State.IsRotating))
        {
            Rotate(direction.rotate);
        }
        if (CheckState(State.IsDriving))
        {
            Drive(direction.drive);
        }   
    }
}
