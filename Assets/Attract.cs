using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class Attract : MonoBehaviour {
	
	float timeout = 60.0f;
	[SerializeField]
	float countdown = 60.0f;
	private float delay = 5.0f;

	public bool idle = false;
	public static Attract instance { get; private set; }
	public bool can_attract = true;
    private bool debug = false;


    
    
	void Awake(){
		instance = this;
	}

	void Start () {
		idle = true; 
		//timeout = 2;
		countdown = timeout; 
		timeout = (int)Main.instance.json_config["attract_screen_seconds"].n;

      



    }

		
	public void reset(){
		if (idle) {
			idle = false; 
			Main.instance.breakAttract(); 
		}
		countdown = timeout; 
		idle = false; 
	}


	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit(); 
        }

        if (!can_attract) {
            return; 
        }
		if(!idle)
		{
			if(countdown <= 0.0f)
			{
				idle = true;
                Main.instance.startAttract (); 
				// RESET
			} else {
				countdown -= Time.deltaTime;
			}
		}


        if (Input.anyKey)
        {


            if (Main.instance.isready)
            {
                reset();
            }


        }



    }
}
