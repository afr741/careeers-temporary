
using UnityEngine;
using System.Collections;
using DG.Tweening; 
using UnityEngine.UI;

public class Float : MonoBehaviour {

    public bool floating = false;
    public float variance = 10;
	public float speed = 50;
	private float angle = 0;
	private float toDegrees = Mathf.PI / 180;
	public float start = 0;
    public float adjust = 1;
	private bool visible = false;

  
	
	private float r; 
	

	public bool invert = false; 

	void Start () {

		r = 1;
        

		if (invert) {
			angle = 0;
		} else {
			angle = 180;
		}

        angle = Random.Range(0, 180);

	
	}

	


	void animateFloat(){
		angle += speed * Time.deltaTime;
		if (angle > 360) angle -= 360;
		float pos = (start + variance) * Mathf.Sin(angle * toDegrees);

        GetComponent<CanvasGroup>().alpha = (pos + adjust);
       

       



    }

	void LateUpdate () {
		if (floating == true) {

            if (Attract.instance.idle)
            {
                animateFloat();
            }
            else {
                if (GetComponent<CanvasGroup>().alpha > 0) {
                    GetComponent<CanvasGroup>().alpha = 0;
                }
            }
			
		}
	}
}