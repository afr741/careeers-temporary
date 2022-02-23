using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.IO;
using RenderHeads.Media.AVProVideo; 

public class Main : MonoBehaviour
{



    // JSON
    private string config_path = "config.json";
    public JSONObject json_config;
    public string path;

    // Settings
    public int attract_timeout = 120;
    private int stall = 0;
    private int timeout = 60;
    private int running = 0;
    public bool isready = false;
    
    // App
    public static Main instance { get; private set; }
    public Text status;
    public GameObject screens;
    public string video_path = "";
    public string cc_path = "";
    public float max = 1;
    public MediaPlayer mp;
    public RippleEffect re; 
    public SpriteRenderer sr;
    public SpriteRenderer overlay;
    public DisplayBackground db; 
    public bool is_back = false;
    private int q_activeQuestionIndex = 0;
    private int c_comboIndex;
    private string a_ActiveTag;
    public List<string> tagList = new List<string>();







    [SerializeField]
    public int ActiveQuestionIndex
    {
        get { return q_activeQuestionIndex; }
        set { q_activeQuestionIndex = value;
            EventManager.InvokeOnQuestionChanged();
        }
    }
    public string ActiveTag
    {
        get { return a_ActiveTag; }
        set { a_ActiveTag = value;
            EventManager.InvokeOnTagChanged();
        }
    }
    public int comboIndex
    {
        get { return c_comboIndex; }
        set
        {
            c_comboIndex = value;
        }
    }



    public void showDetails() {

        sr.DOFade(1, 2);
        overlay.DOFade(0.8f, 2);
        re.canAnimate = false;

    }


  
    public void playSingle() {

        sr.DOFade(0, 2);
    }


    public IEnumerator loadRawImage(RawImage target, string url, bool animate = false, bool resize = false, bool scale = false)
    {


        target.DOFade(0, 0);
        System.GC.Collect();
        //Debug.Log (url); 
       // string[] thumb = url.Split(new string[] { "wp-content" }, System.StringSplitOptions.None);
       // string thumb_fn = thumb[thumb.Length - 1];

       // url = thumb_fn.Replace("\\", "");

        url = "file:///" + path.Substring(0, path.Length - 1) + url;

        Debug.Log(url);

        var www = new WWW(url);
        yield return www;

        target.transform.localScale = new Vector2(1, 1);
        target.texture = www.texture;

        //target.texture.filterMode = FilterMode.Bilinear; 
        //target.SetNativeSize (); 
        target.transform.DOKill(true);

        if (resize)
        {
            target.SetNativeSize();

            if (scale)
            {

                float w = target.rectTransform.sizeDelta.x * 0.5f;
                float h = target.rectTransform.sizeDelta.y * 0.5f;
                target.rectTransform.sizeDelta = new Vector2(w, h);
            }
        }


        if (animate)
        {
            target.DOFade(0, 0);
        }
        else
        {
            target.DOFade(1, 2);
        }

        www.Dispose();
        www = null;
    }


    void Awake()
    {

        instance = this;
        Application.targetFrameRate = 300;
        Input.multiTouchEnabled = false; 

        path = Application.dataPath;
        if (Application.platform == RuntimePlatform.OSXPlayer)
        {
            path += "/../../";
        }
        else if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            path += "/../";
        }
        else
        {
            path += "/../";
        }

        // Load Configuration File
        // If the configuration file doesn't exist, create one from resources 

        if (File.Exists(config_path))
        {
            // File Exists - Load
            StreamReader sr = new StreamReader(config_path);
            string fileContents = sr.ReadToEnd();
            sr.Close();
            json_config = new JSONObject(fileContents);

        }
        else
        {
            // File Does not exist - Load from Resources
            TextAsset config = Resources.Load<TextAsset>("config");
            json_config = new JSONObject(config.text);

            // Create File
            var sr = File.CreateText(config_path);
            // Write File
            sr.WriteLine(config.text);
            sr.Close();
        }


        // Settings

        if (json_config.HasField("show_mouse"))
        {

            if (json_config["show_mouse"].b)
            {

              //  Debug.Log("CURSOR VISIBLE");
                Cursor.visible = true;

            }
            else
            {
              //  Debug.Log("CURSOR HIDDEN");
                Cursor.visible = false;
            }
        }

        attract_timeout = (int)json_config["attract_screen_seconds"].n;
        status.text = "Loading data...";
        Invoke("startGame", 1);



    }


    


    public void startAttract()
    {
       
        UINav.instance.loadScreen("Attract");
        sr.DOFade(0, 2);
        overlay.DOFade(0.8f, 2);
    }

 
    public void breakAttract()
    {
        
      
    }


    public static string parsePath(string url)
    {

        string[] thumb = url.Split(new string[] { "wp-content" }, System.StringSplitOptions.None);
        string thumb_fn = thumb[thumb.Length - 1];

        url = thumb_fn.Replace("\\", "");

        url = "file:///" + Main.instance.path.Substring(0, Main.instance.path.Length - 1) + url;

        return url;


    }


    public static void clear(Transform transform)
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public static string Truncate(string value, int maxChars)
    {
        return value.Length <= maxChars ? value : value.Substring(0, maxChars) + "...";
    }

    void startGame()
    {
       
        isready = true;

        startAttract();

       


    }


    public static Color HexToColor(string hex)
    {
        if (hex.Length > 6)
        {
            hex = hex.Substring(1, hex.Length - 1);
        }
        byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        return new Color32(r, g, b, 255);
    }




    public static string parseRelativePath(string url)
    {

        string[] thumb = url.Split(new string[] { "wp-content" }, System.StringSplitOptions.None);
        string thumb_fn = thumb[thumb.Length - 1];

        url = thumb_fn.Replace("\\", "");



        return url.Substring(1, url.Length - 1);


    }

    public static string parseText(string text)
    {

        text = text.Replace("\\r", "");
        text = text.Replace("\\n", "");
        text = text.Replace("<br \\/>", "\n");
        text = text.Replace("<br/>", "\n");
        text = text.Replace("<br />", "\n");
        text = text.Replace("<p>", "");
        text = text.Replace("<\\/p>", "\n\n");
        text = text.Replace("<ul>", "");
        text = text.Replace("<\\/ul>", "");
        text = text.Replace("<li>", "");
        text = text.Replace("<\\/li>", "\n");
        text = text.Replace("<\\/b>", "</b>");
        text = text.Replace("<em>", "<i>");
        text = text.Replace("<\\/em>", "</i>");
        text = text.Replace("\\u201c", "“");
        text = text.Replace("\\u201d", "”");
        text = text.Replace("\\u2014", "—");
        text = text.Replace("\\u2019", "’");
        text = text.Replace("\\u2026", "…");
        text = text.Replace("\\u00A0", "\u00A0");
        
        text = text.Replace("\\/", "/");
        text = text.Replace("\\\"", "\"");
       // text = text.Replace("u00A0", "\u00A0");
        
        return text;

    }


}
