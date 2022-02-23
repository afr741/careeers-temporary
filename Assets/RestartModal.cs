using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartModal : MonoBehaviour
{
    public NGXAnimate aniOut;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Restart()
    {
        UINav.instance.loadScreen("Attract");
    }

    public void KeepGoing()
    {
        UINav.instance.loadScreen("Details");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
