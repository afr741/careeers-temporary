using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro; 

public class BtnEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	// Use this for initialization

	public float scale = 1.1f;
    public TextMeshProUGUI txt; 
    public Color32 colour;
    private Color txt_color;
 
  
      
	void Start () {

        if (txt != null) {
            
            txt_color = txt.color; 
        }
	}


	public void OnPointerDown(PointerEventData p){

        if (txt != null) {
            txt.DOColor(Color.white, 0);
        }
		transform.DOKill (true); 
		transform.DOScale (new Vector3(scale, scale, 1), 0.2f); 
	}

	public void OnPointerUp(PointerEventData p){

        if (txt != null)
        {
            txt.DOColor(txt_color, 0);
        }
       // CancelInvoke ("deactivate");
		//Invoke ("deactivate", 0.1f);
		transform.DOKill (true);
        transform.DOScale(new Vector3(1f, 1f, 1), 1.2f);
    }

	private void deactivate(){
		GetComponent<Button> ().interactable = false;
		CancelInvoke ("reactivate");
		Invoke ("reactivate", 0.2f);
	}

	private void reactivate(){
		GetComponent<Button> ().interactable = true;
	}

}