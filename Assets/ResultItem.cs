using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ResultItem : MonoBehaviour
{

    // Use this for initialization
    JSONObject data;

    public TextMeshProUGUI description_txt;
    public TextMeshProUGUI title_txt;
    public RawImage icon;
    public int activeQuestionIndex;


    private void OnEnable()
    {

    }
    void Start()
    {
        activeQuestionIndex = Main.instance.ActiveQuestionIndex;
        

    }




    public void draw(JSONObject _data)
    {
        data = _data;
        /*        StartCoroutine(Main.instance.loadRawImage(icon, data["iconType"].str));
        */

        title_txt.text = Main.parseText(data["title"].str);
        description_txt.text = Main.instance.json_config["jobDescriptions"][title_txt.text].str;

    }


    // Update is called once per frame
    void Update()
    {

    }
}
