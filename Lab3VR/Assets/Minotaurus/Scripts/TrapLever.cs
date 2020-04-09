using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TrapLever : MonoBehaviour {
    public bool isExit;
    private PlayerInLabirint player;
    Animator anim;
    private float exit = 0;
    [SerializeField]
    GameObject pressEText;

    // Use this for initialization
    void Start () {
        GameObject playerObject = GameObject.FindWithTag("Player");
        player = playerObject.GetComponent<PlayerInLabirint>();
        anim = GetComponentInParent<Animator>();
        }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster")
        {
            exit = 1;
            anim.SetFloat("exit", exit);
        }
        else
        {
            pressEText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Monster")
        {
            exit = -1;
            anim.SetFloat("exit", exit);
        }

        else
        {
            pressEText.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (isExit == true)
            {
                pressEText.SetActive(false);
                exit = 1;
                anim.SetFloat("exit", exit);
            }

            else
            {
                exit = -1;
                anim.SetFloat("exit", exit);
                player.GetDamage(100);
            }
        }
    }
}
