using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderingAgent : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 destination;

    public float wanderRadius = 10f;
    public float wanderTimer = 10f;
    private float timer;

    // Chase and Runaway parameters
    private bool isChasing = false;
    private Transform chaseTarget;
    private List<Transform> runAwayTargets = new List<Transform>();

    // Add a tag for each AI type: "Zombie" and "Human"
    public string aiTypeTag;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
        SetNewRandomDestination();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (isChasing)
        {
            // Chase the target
            agent.SetDestination(chaseTarget.position);
        }
        else
        {
            if (timer >= wanderTimer)
            {
                SetNewRandomDestination();
            }

            if (agent.remainingDistance < 1f)
            {
                SetNewRandomDestination();
            }
        }
    }

    public void SetChaseTarget(GameObject[] targets)
    {
        isChasing = true;
        chaseTarget = GetClosestTarget(targets);
    }

    public void StopChasing()
    {
        isChasing = false;
    }

    public void SetRunAwayFrom(GameObject[] targets)
    {
        foreach (var target in targets)
        {
            if (Vector3.Distance(transform.position, target.transform.position) < wanderRadius)
            {
                runAwayTargets.Add(target.transform);
            }
        }
    }

    public void StopRunningAway()
    {
        runAwayTargets.Clear();
    }

    private void SetNewRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randomDirection, out navHit, wanderRadius, -1);

        destination = navHit.position;
        agent.SetDestination(destination);
        timer = 0;
    }

    private Transform GetClosestTarget(GameObject[] targets)
    {
        Transform closestTarget = null;
        float closestDistance = float.MaxValue;

        foreach (var target in targets)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target.transform;
            }
        }

        return closestTarget;
    }
}