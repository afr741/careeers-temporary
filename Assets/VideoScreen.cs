using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; 

public class VideoScreen : MonoBehaviour {

    // Use this for initialization
    public Video video;
    public NGXAnimate aniOut;

    private int timeout = 300;
    private int current_time = 0;

    public CanvasGroup controls; 

    public void close() {

        aniOut.animateIn();
        //CancelInvoke("back");
        UINav.instance.loadScreen("Details");
    }

    private void back() {
        UINav.instance.loadScreen("Details");
    }

	void Start () {

        video.loadVideo(Main.instance.video_path, Main.instance.cc_path);

	}
	
	// Update is called once per frame
	void Update () {



        if (Input.anyKey) {

            current_time = 0;
            if (controls.alpha != 1) {
        
                controls.DOKill(); 
                controls.alpha = 1;
            }
           

        }

        if (current_time < timeout)
        {
            current_time++;
        }
        else {

            if (controls.alpha > 0) {
                controls.DOFade(0, 2);
            }
        }
	}
}
