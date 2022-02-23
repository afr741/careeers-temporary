using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ProgressBar : MonoBehaviour
{
    public float movementSpeed = 5.0f;
    public float screenWidth = 1920;
    // Start is called before the first frame update
    void Start()
    {
        UpdatePosition();
        EventManager.OnQuestionChanged += UpdatePosition;

    }

    public void UpdatePosition()
    {

        transform.DOMoveX(gameObject.transform.position.x + Main.instance.ActiveQuestionIndex+1 * (460), 1);
    }
    // Update is called once per frame
    private void OnDestroy()
    {
        EventManager.OnQuestionChanged -= UpdatePosition;
    }
}

