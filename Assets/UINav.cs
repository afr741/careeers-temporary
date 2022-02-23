using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class UINav : MonoBehaviour {

	public static UINav instance { get; private set; }
	// Use this for initialization

    
	public NGXScreen screens; 
	public NGXScreen persistant;
    public NGXScreen overlay;

	public string currentScreen = "Attract";
	public string currentPersistant = "Empty";
    public string currentOverlay = "Empty";

    public Canvas c;
    private string current_mode = "c";
    void Awake(){
		instance = this; 
	}

    public void loadScreen(string screen){
       
        currentScreen = screen; 
		screens.Transition (screen);
       
	}

    public void loadOverlay(string screen) {

        currentOverlay = screen;
        overlay.Transition(screen);
    }

    public void loadPersistant(string screen)
    {
        currentPersistant = screen;
        persistant.Transition(screen);
    }


    void Update() {

        if (Input.GetKeyDown("s")) {

            loadScreen("Secret");
        }

    }


}
