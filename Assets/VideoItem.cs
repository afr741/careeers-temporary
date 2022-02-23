using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro; 

public class VideoItem : MonoBehaviour {

    // Use this for initialization
    JSONObject data;

    public TextMeshProUGUI description_txt;
    public RawImage icon;
    public RawImage image;
    public Texture imageSelected;
    public Texture imageNotSelected;
    public int activeQuestionIndex;
    public bool isSelected; 


    private void OnEnable()
    {
        EventManager.OnTagChanged += changeTextColor;
    }
    void Start() {
        activeQuestionIndex = Main.instance.ActiveQuestionIndex;

    }

    public void draw(JSONObject _data) {
        data = _data;
        StartCoroutine(Main.instance.loadRawImage(icon, data["iconType"].str));

        /*        title_txt.text = Main.parseText(data["title"].str);
        */
        description_txt.text = Main.parseText(data["text"].str);
        /*        duration_txt.text = "Duration <color=#005aff> | </color>   <b>" + data["duration"].str + "</b>";
        */
    }
/*    public void play() {

        Main.instance.video_path = data["video_path"].str;
        Main.instance.cc_path = data["srt_path"].str; 
        transform.parent.GetComponent<Bridge>().playVideo(); 

    }*/

    public void selectTheAnswer()
    {
  /*      Debug.Log("tag before selection"+Main.instance.ActiveTag);
        Debug.Log(data["tag"].str +" tag selected");*/

        Main.instance.ActiveTag = data["tag"].str;
        




    }

    public void changeTextColor()
    {
        if (Main.instance.ActiveTag == data["tag"].str)
        {
            description_txt.faceColor = new Color32(231, 231, 231, 255);
            image.texture = imageSelected;

            Debug.Log(description_txt.faceColor);
        }
        else
        {
            description_txt.faceColor = new Color32(0, 0, 0, 255);
            image.texture = imageNotSelected;


        }

    }
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDestroy()
    {
        EventManager.OnTagChanged -= changeTextColor;
    }
}

