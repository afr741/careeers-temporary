using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingUVs : MonoBehaviour
{

    private Material _material;
    private float speed = 0.01f;
    private float currentscroll = 0;
    void Start()
    {
        _material = GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        currentscroll += speed * Time.deltaTime;
        _material.mainTextureOffset = new Vector2(currentscroll, 0);
    }
}