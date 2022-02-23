using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; 

public class Pulse : MonoBehaviour {

    public Image s1;
    public Image s2;
    private bool first = false; 
    // Use this for initialization
    void Awake() {

        s2.DOFade(0,0);
    }
	void Start () {

        Invoke("pulseEffect", 0.1f);
    }


    void pulseEffect() {

        first = !first;

        if (first)
        {

            s1.transform.DOScale(new Vector2(1.3f, 1.3f), 2).SetEase(Ease.OutQuad);
            s1.DOFade(0, 2).SetEase(Ease.OutQuad);
            // reset
            s2.transform.localScale = new Vector2(1, 1);
            s2.transform.DOScale(new Vector2(0.8f, 0.8f), 2).From().SetEase(Ease.InQuad);
            s2.DOFade(1, 2).SetEase(Ease.InQuad);

            
        }
        else {


            s2.transform.DOScale(new Vector2(1.3f, 1.3f), 2).SetEase(Ease.OutQuad);
            s2.DOFade(0, 2).SetEase(Ease.OutQuad);
            // reset
            s1.transform.localScale = new Vector2(1, 1);
            s1.transform.DOScale(new Vector2(0.8f, 0.8f),2).From().SetEase(Ease.InQuad);
            s1.DOFade(1, 2).SetEase(Ease.InQuad);

        }

        Invoke("pulseEffect", 2);

    }

	
	// Update is called once per frame
	void Update () {
		
	}
}
