using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubAdjust : MonoBehaviour {

    // Use this for initialization
    public Text txt;
    public string txt_str = "xxx";
    public CanvasGroup cg; 
	void Start () {

        txt_str = txt.text;
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    void change() {

        if (txt.text == "")
        {
            cg.alpha = 0;
        }
        else {
            LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
        }
    }
	
	// Update is called once per frame
	void Update () {

        if (txt_str != txt.text) {
            change();
            txt_str = txt.text;
        }
		
	}
}
