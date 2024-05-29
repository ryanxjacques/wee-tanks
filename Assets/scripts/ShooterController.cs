using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Incredibly Dead-Simple AI using NavMeshAgent.
[RequireComponent(typeof(NavMeshAgent))]
public class ShooterController : MonoBehaviour
{
    private NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(new Vector3(-49, 0, 0));
    }
    void Update(){}
    void FixedUpdate(){}
}
