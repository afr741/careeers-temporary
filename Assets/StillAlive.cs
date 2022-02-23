using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class StillAlive : MonoBehaviour
{

    string filepath;
    float timer;
    public float timertarget;

    void Start()
    {
        filepath = "health.txt";
        StartCoroutine(HeartBeat());
    }

    IEnumerator HeartBeat()
    {
        timer = 0;
        while (true)
        {

            //			if (Input.GetKeyDown (KeyCode.Space)) {
            //				ForceCrash ();
            //			}
            //
            timer += Time.deltaTime;
            if (timer >= timertarget)
            {
                timer = 0;
                //				if (File.Exists (filepath))
                //					File.Delete (filepath);
                StreamWriter write = new StreamWriter(filepath, false);
                write.WriteLine("Still Alive");
                write.Close();
                yield return null;
            }
        }
    }

    //	void ForceCrash()
    //	{
    //		while (true) {
    //			//RIGGITY WRECKED
    //		}
    //	}
}