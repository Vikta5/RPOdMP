using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Patrol : MonoBehaviour {
    public GameObject[] wayPoints;

    public bool randomWay = false;

    NavMeshAgent agent;

    int current_point = 0;
    // Use this for initialization
    void Start () {
        wayPoints = GameObject.FindGameObjectsWithTag("POINT");
        agent = GetComponent<NavMeshAgent>();
    }
	
	// Update is called once per frame
	void Update () {
        agent.SetDestination(wayPoints[current_point].transform.position);

        if(agent.remainingDistance<4)
        {
            if (randomWay == false)
            {
                current_point++;
                if (current_point == wayPoints.Length)
                {
                    current_point = 0;
                }
            }
            else
            {
                current_point = Random.Range(0, wayPoints.Length);
            }
        }
	}
}
