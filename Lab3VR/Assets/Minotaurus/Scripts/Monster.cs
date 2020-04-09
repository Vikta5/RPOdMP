using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour {
    NavMeshAgent agent;
    float timer = 5;
    AudioSource source;
    public AudioClip roar;
    public Transform player;
    private PlayerInLabirint playerIn;


    // Use this for initialization
    void Start () {
        source = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        GameObject playerObject = GameObject.FindWithTag("Player");
        playerIn = playerObject.GetComponent<PlayerInLabirint>();
        agent.SetDestination(player.position);
    }
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if(timer<0)
        {
            source.PlayOneShot(roar);
            timer = Random.Range(10, 25);
        }
        

    }


    private void FixedUpdate()
    {

        agent.SetDestination(player.position);
        Debug.Log(agent.remainingDistance);


        if (agent.remainingDistance != 0 && agent.remainingDistance < 2 && !float.IsInfinity(agent.remainingDistance))
        {
            playerIn.GetDamage(10);
        }
    }
}
