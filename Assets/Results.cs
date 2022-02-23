using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Results : MonoBehaviour {

    // Use this for initialization
    public NGXAnimate aniOut;
    public GameObject container;
    public TextMeshProUGUI questionText;
    public int activeAnswerIndex;
    public bool flag = false;
    void Start () {
        CompareResults();
        updateDetailsInfo();
    }


    //Compares tagList from Main instance to config.json resultCombos
    public void CompareResults()
    {
        int count = 0;

        foreach (JSONObject d in Main.instance.json_config["resultCombo"].list)
        {
            for (int i = 0; i <= Main.instance.tagList.Count-1; i++)
            {
              
                if (!Main.instance.tagList[i].Equals(d["combo"].list[i].str))
                {
                    flag = true;
                    count++;
                    break;

                }
                if(i== Main.instance.tagList.Count - 1 && Main.instance.tagList[i].Equals(d["combo"].list[i].str))
                {
                    activeAnswerIndex = count;
                    Debug.Log(activeAnswerIndex);
                    return;
                }
                
      /*         
                Debug.Log("1combo item " + (d["combo"].list[i].str));
                Debug.Log("1taglist item " + (Main.instance.tagList[i]));*/



            }

        }
      
    }


    //istantiates the result cards and text
    public void updateDetailsInfo()
    {
        questionText.text = "the results are in, great careers for you are:";
        Main.instance.showDetails();
        showDetails();
    }

    private void showDetails() {

        Attract.instance.can_attract = true; 
        foreach (JSONObject d in Main.instance.json_config["resultCombo"][activeAnswerIndex]["jobs"].list) {
            GameObject go = Instantiate(Resources.Load("Prefabs/result_item")) as GameObject;
            go.transform.SetParent(container.transform);
            go.transform.DOLocalRotate(new Vector3(0, 0, 0), 0);
            go.GetComponent<ResultItem>().draw(d);
        }

    }


    //resets everything and retruns to attract
    public void ReturnToAttract()
    {

        aniOut.animateIn();
        UINav.instance.loadScreen("Attract");
        Main.instance.ActiveQuestionIndex = 0;
        activeAnswerIndex = 0;
        Main.instance.tagList.Clear();

    }
}
