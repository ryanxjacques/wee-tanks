using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/* Note: Each entity will HAVE to have this enum. They cannot define their
   own enum because enums cannot be abstract/derived/inherited. They are
   treated like values. So, the cleanist solution is to give all entities
   the same State space. */
[Flags]
public enum State  
{                        // Supports up to 32 states.
    OnGround   = 1 << 0, // 000001
    IsDriving  = 1 << 1, // 000100
    IsRotating = 1 << 2, // 001000
    IsJumping  = 1 << 3, // 010000
    IsPlanning = 1 << 4, // 100000
}


/* I wish I could move the Direction struct to the PlayerController file
   but Unity will complain that Direction is a class that is not derived
   from a Monobehavior class. Ugh */
struct Direction
{
    public float drive;
    public float rotate;
    public Direction(float drive = 0, float rotate = 0)
    {
        this.drive = drive;
        this.rotate = rotate;
    }
}


public interface IStateful
{
    bool CheckState(State state);
    bool CheckStates(params (State state, bool value)[] states);
    void SetState(State state, bool value);
}


/* The Entity Parent class handles the state changing.
*/
public class Entity : MonoBehaviour, IStateful
{
    protected State state;

    /* Check if this entity is in the given state. */
    public bool CheckState(State state)
    {
        return this.state.HasFlag(state);
    }

    /* For checking multiple states and if those states are true or false.   */
    /* Usuage: CheckStates((State.OnGround, true), (State.IsDriving, false)) */
    public bool CheckStates(params (State state, bool value)[] states)
    {
        bool result = true;
        foreach (var tuple in states)
        {
            if (tuple.value == false) {
                result = result && !CheckState(tuple.state);
            } else {
                result = result && CheckState(tuple.state);
            }
        }
        return result;
    }

    /* Move this entity into or out of a given state. */
    public void SetState(State state, bool value)
    {
        if (value) {
            this.state |= state;
        } else {
            this.state &= ~state;
        }
    }
}
