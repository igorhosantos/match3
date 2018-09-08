using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startup : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Awake()
    {
        MatchController.ME.StartSession();
        Debug.Log("Session has created: " + MatchController.ME.match.ToString());
    }
}
