using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class IntroScreen : MonoBehaviour
{

    // Use this for initialization


    public TextMeshProUGUI title_txt;
    public TextMeshProUGUI subtitle_txt;
    public TextMeshProUGUI instructions_txt;
    void Start()
    {

       /* title_txt.text = Main.parseText(Main.instance.json_config["title"].str);
        subtitle_txt.text = Main.parseText(Main.instance.json_config["subtitle"].str);
        instructions_txt.text = Main.parseText(Main.instance.json_config["instructions"].str);*/
    }

    public void playSingle()
    {

        UINav.instance.loadScreen("Details");

    }

    // Update is called once per frame
    void Update()
    {

    }
}
