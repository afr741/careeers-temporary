using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using RenderHeads.Media.AVProVideo;
public class Video : MonoBehaviour
{

    // Use this for initialization


    private JSONObject data;




    // VIDEO

    public SubtitlesUGUI subs;
    public DisplayUGUI display;
    public DisplayBackground display_bg;
    private MediaPlayer mp;
    public bool isplaying = true;
    private bool isvisible = false;

    public CanvasGroup play_btn;
    public CanvasGroup pause_btn;
    public CanvasGroup controls;

    public Slider _videoSeekSlider;
    private float _setVideoSeekSliderValue;
    private bool _wasPlayingOnScrub;
    private bool started = false;
    public bool has_cc = true;
    private float max = 1;
    public TextMeshProUGUI duration_txt;







    public void toggle()
    {

        playPause();
    }





    public void loadVideo(string _video_path, string _cc = "") {

        mp = Main.instance.mp;

        subs.ChangeMediaPlayer(mp);



        if (_cc != null) {

            mp.EnableSubtitles(MediaPlayer.FileLocation.RelativeToProjectFolder, Main.parseRelativePath(_cc));

        }


        if (GetComponent<SubtitlesUGUI>() != null)
        {
            GetComponent<SubtitlesUGUI>().ChangeMediaPlayer(mp);
        }



        // display_bg = Main.instance.db;

        //display._mediaPlayer = mp;


         mp.Events.AddListener(OnVideoEvent);

        Main.instance.video_path = Main.parsePath(_video_path);


        if (!started)
        {



            /*

             if (NI_Main.instance.current_media["srt"].IsString) {

                 Debug.Log("HAS SRT!!");
                 Debug.Log(NI_Main.parseRelativePath(NI_Main.instance.current_media["srt"].str));

                 //mp.EnableSubtitles(MediaPlayer.FileLocation.RelativeToProjectFolder, "uploads/2019/09/NIWeek_Opener_1_V3.srt");
                 mp.EnableSubtitles(MediaPlayer.FileLocation.RelativeToProjectFolder, NI_Main.parseRelativePath(NI_Main.instance.current_media["srt"].str));
             }


         */

            Attract.instance.can_attract = false;






            // mp.OpenVideoFromFile(MediaPlayer.FileLocation.RelativeToStreamingAssetsFolder, "video.mp4");

            started = true;
        }

        return;

        //CancelInvoke("playPause");
        //Invoke("playPause", 0.5f);

    }


    public void pauseVideo()
    {
        if (mp.Control.IsPlaying())
        {

            //display.DOFade(0, 0);
            playPause();
        }
    }




    public void stopVideo()
    {
        //  Debug.Log("STOP VIDEO!!");
        if (Attract.instance != null) {
            Attract.instance.can_attract = true;
        }


        started = false;

        //display.GetComponent<CanvasGroup>().alpha = 0;



        if (mp != null)
        {

            isplaying = false;
            if (mp.Control != null)
            {
                mp.Control.Rewind();
                mp.Control.Stop();
            }
            pause_btn.alpha = 0;
            play_btn.alpha = 1;
            _videoSeekSlider.value = 0;



        }


    }



    public void playVideo()
    {
        if (mp.Control.IsPaused())
        {
            playPause();
        }
    }

    public void playPause()
    {

        if (mp == null) { return; }
        if (mp.Control == null)
        {
            return;
        }

        if (mp.Control.IsPlaying())
        {

            Attract.instance.can_attract = true;
            isplaying = false;
            mp.Control.Pause();
            pause_btn.alpha = 0;
            play_btn.alpha = 1;



        }
        else
        {


            if (!started)
            {



                /*

                 if (NI_Main.instance.current_media["srt"].IsString) {

                     Debug.Log("HAS SRT!!");
                     Debug.Log(NI_Main.parseRelativePath(NI_Main.instance.current_media["srt"].str));

                     //mp.EnableSubtitles(MediaPlayer.FileLocation.RelativeToProjectFolder, "uploads/2019/09/NIWeek_Opener_1_V3.srt");
                     mp.EnableSubtitles(MediaPlayer.FileLocation.RelativeToProjectFolder, NI_Main.parseRelativePath(NI_Main.instance.current_media["srt"].str));
                 }

             */

                Attract.instance.can_attract = false;
                // mp.OpenVideoFromFile(MediaPlayer.FileLocation.AbsolutePathOrURL, Main.instance.video_path, false);
                started = true;
            }





            mp.Control.SetVolume(Main.instance.max);


            if (Attract.instance != null)
            {
                Attract.instance.can_attract = false;
            }





            isplaying = true;
            mp.Control.Play();

            pause_btn.alpha = 1;
            play_btn.alpha = 0;
        }
    }

    public void OnVideoEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode)
    {
        switch (et)
        {

            case MediaPlayerEvent.EventType.ReadyToPlay:


                break;
            case MediaPlayerEvent.EventType.Started:



                break;
            case MediaPlayerEvent.EventType.FirstFrameReady:
                //SwapPlayers();
                break;
            case MediaPlayerEvent.EventType.FinishedPlaying:
                Attract.instance.can_attract = true;
               // stopVideo();

                break;
        }

        //Debug.Log("Event: " + et.ToString());
    }



    public void OnVideoSeekSlider()
    {

        if (mp.Control != null) {
            if (mp && _videoSeekSlider && _videoSeekSlider.value != _setVideoSeekSliderValue)
            {
                //resetDim (); 
                mp.Control.Seek(_videoSeekSlider.value * mp.Info.GetDurationMs());
            }
        }
    }
    public void OnVideoSliderDown()
    {
        if (mp)
        {
            _wasPlayingOnScrub = mp.Control.IsPlaying();
            if (_wasPlayingOnScrub)
            {
                pauseVideo();
                //SetButtonEnabled( "PauseButton", false );
                //SetButtonEnabled( "PlayButton", true );
            }
            OnVideoSeekSlider();
        }
    }
    public void OnVideoSliderUp()
    {
        if (mp && _wasPlayingOnScrub)
        {
            playVideo();
            _wasPlayingOnScrub = false;

            //SetButtonEnabled( "PlayButton", false );
            //SetButtonEnabled( "PauseButton", true );
        }
    }







    void OnEnable()
    {
        // display.GetComponent<CanvasGroup>().alpha = 0;
        // playPause();


        pause_btn.alpha = 1;
        play_btn.alpha = 0;
    }





    public void draw(JSONObject _data)
    {

        data = _data;


    }

    void OnDisable()
    {

        // stopVideo();

    }

    string formatTime(float _time) {

        TimeSpan t = TimeSpan.FromMilliseconds(_time);
        string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
                                t.Hours,
                                t.Minutes,
                                t.Seconds,
                                t.Milliseconds);

        return t.Minutes + ":" + t.Seconds;
    }


    void Update()
    {

        
        if (mp && mp.Info != null && mp.Info.GetDurationMs() > 0f)
        {
            float time = mp.Control.GetCurrentTimeMs();
            float duration = mp.Info.GetDurationMs();
            float d = Mathf.Clamp(time / duration, 0.0f, 1.0f);

            duration_txt.text = "<b>" + formatTime(time) + "</b> / " + formatTime(duration); 
            _setVideoSeekSliderValue = d;
            _videoSeekSlider.value = d;
        }
        



    }



}
