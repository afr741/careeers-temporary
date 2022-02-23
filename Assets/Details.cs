using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Details : MonoBehaviour {

    // Use this for initialization
    public NGXAnimate aniOut;
    public GameObject container;
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI questionNumber;
    public GameObject nextButton; 

    void Start () {
        nextButton = gameObject.transform.Find("NextButton").gameObject;
        updateQandA();
        EventManager.OnQuestionChanged +=updateInfo;
        EventManager.OnTagChanged += showAndHideNextButton;
    }

    public void showAndHideNextButton()
    {
        nextButton = gameObject.transform.Find("NextButton").gameObject;

        if (Main.instance.ActiveTag != "")
        {
            nextButton.gameObject.SetActive(true);
        }
        else
        {
            nextButton.gameObject.SetActive(false);

        }
    }


    //instantiates question and answers
    public void updateQandA()
    {
        questionText.text = Main.instance.json_config["quizData"][Main.instance.ActiveQuestionIndex]["question"].str;
        questionNumber.text = "0" + (Main.instance.ActiveQuestionIndex + 1).ToString();
        Main.instance.showDetails();
        showAnswers();
    }
    private void showAnswers() {

        Attract.instance.can_attract = true; 
        foreach (JSONObject d in Main.instance.json_config["quizData"][Main.instance.ActiveQuestionIndex]["answers"].list) {
            GameObject go = Instantiate(Resources.Load("Prefabs/video_item")) as GameObject;
            go.transform.SetParent(container.transform);
            go.transform.DOLocalRotate(new Vector3(0, 0, 0), 0);
            go.GetComponent<VideoItem>().draw(d);
        }

    }
    public void handleClickNext()
    {

        if (Main.instance.ActiveQuestionIndex==Main.instance.json_config["quizData"].Count-1)
        {
            Main.instance.tagList.Add(Main.instance.ActiveTag);
            UINav.instance.loadScreen("Results");


        }
        else
        {
            Main.instance.ActiveQuestionIndex = Main.instance.ActiveQuestionIndex + 1;
            Main.instance.tagList.Add(Main.instance.ActiveTag);
            Main.instance.ActiveTag = "";

          
        }
        foreach (string item in Main.instance.tagList)
        {
            Debug.Log("tag list item is " + item);
        }
    }

        public void updateInfo()
        {

          foreach (Transform child in container.transform)
          {
              GameObject.Destroy(child.gameObject);

        }
        updateQandA();




    }




    public void handleClickRestart()
    {
        Main.instance.ActiveTag = "";

        UINav.instance.loadScreen("RestartModal");
        EventManager.OnQuestionChanged -= updateInfo;
        EventManager.OnTagChanged -= showAndHideNextButton;

    }


	// Update is called once per frame
	void Update () {
		
	}

    private void OnDestroy()
    {
        EventManager.OnQuestionChanged -= updateInfo;
        EventManager.OnTagChanged -= showAndHideNextButton;
    }
}
