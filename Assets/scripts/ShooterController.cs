using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ShooterController : TankParent
{
    private NavMeshAgent agent;
    private Direction direction = new Direction();
    // private Direction agentDirection = new Direction();
    private Vector3 agentForward;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        // Disable NavMeshAgent auto-movement
        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.SetDestination(new Vector3(-49, 0, 0));
    }

    void Update()
    {
        if (agent.pathPending || agent.remainingDistance > 0.1)
        {
            agentForward = agent.velocity.normalized;
            if (this.forward.x - 0.01f > agent.velocity.normalized.x)
            {
                SetState(State.IsRotating, true);
                direction.rotate = -1;
            } else if (this.forward.x + 0.01f < agent.velocity.normalized.x)
            {
                SetState(State.IsRotating, true);
                direction.rotate = 1;
            } else {
                SetState(State.IsRotating, false);
            }
            if (CheckState(State.IsRotating) == false)
            {
                SetState(State.IsDriving, true);
            }
        }
        else
        {
            // Stop the agent when it reaches the destination
            agent.isStopped = true;
            SetState(State.IsRotating, false);
            SetState(State.IsDriving, false);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (CheckState(State.IsRotating))
        {
            Rotate(direction.rotate, agentForward);
        }
        if (CheckState(State.IsDriving))
        {
            Drive(1);
        }
    }
}
