using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;


public class PlayerInLabirint : MonoBehaviour {

    private AudioSource audChain;
    public int hp;
    public GameObject gameOverText;
    public Slider healthSlider;

    // Use this for initialization
    void Start () {
        Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update () {

	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "chain")
        {
            audChain = other.GetComponentInParent<AudioSource>();
            audChain.Play();
        }
    }

    public void GetDamage(int damage)
    {
        hp -= damage;
        healthSlider.value = hp;
        if (hp == 0)
        {
            Death();
        }
    }

    public void Death()
    {
            Time.timeScale = 0;
            GetComponent<CharacterController>().enabled = false;
           // GetComponent<FirstPersonController>().enabled = false;
            gameOverText.SetActive(true);
        
    }
}
