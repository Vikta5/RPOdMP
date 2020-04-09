using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour {

   AudioSource aud;

	// Use this for initialization
	void Start () {
        aud = GetComponentInParent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        aud = other.GetComponent<AudioSource>();
        aud.Play();
    }
}
