using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] zombies;
    public GameObject[] humans;

    private bool isChaseMode = false;

    private void Start()
    {
        zombies = GameObject.FindGameObjectsWithTag("Zombie");
        humans = GameObject.FindGameObjectsWithTag("Human");
    }

    private void Update()
    {
        // Press Enter to toggle chase mode
        if (Input.GetKeyDown(KeyCode.Return))
        {
            isChaseMode = !isChaseMode;

            if (isChaseMode)
            {
                StartChase();
            }
            else
            {
                StopChase();
            }
        }
    }

    private void StartChase()
    {
        foreach (var zombie in zombies)
        {
            var zombieAgent = zombie.GetComponent<WanderingAgent>();
            zombieAgent.SetChaseTarget(humans);
        }

        foreach (var human in humans)
        {
            var humanAgent = human.GetComponent<WanderingAgent>();
            humanAgent.SetRunAwayFrom(zombies);
        }
    }

    private void StopChase()
    {
        foreach (var zombie in zombies)
        {
            var zombieAgent = zombie.GetComponent<WanderingAgent>();
            zombieAgent.StopChasing();
        }

        foreach (var human in humans)
        {
            var humanAgent = human.GetComponent<WanderingAgent>();
            humanAgent.StopRunningAway();
        }
    }
}