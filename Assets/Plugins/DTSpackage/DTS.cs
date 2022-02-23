using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

public class DTS : MonoBehaviour
{
	private bool allowForIntermittantInternet = true;
	private int maxRequests = 100;
	private bool dev_enabled = true;
	private bool islive = true;

	private string tracking_id;
	private string dev_tracking_id;
	private string guid;
	private string application_language;
	private string screen_resolution;
	private string application_id;
	private string version;
	private string url;
	private WWWForm urlVars;
	private Hashtable urlvars;

	public string session_id = ""; 

	public static DTS instance { get; private set; }

	string leadingZero (int num)
	{
		if (num < 10) {
			return "0" + num;
		}
		return num.ToString ();
	}


	void OnApplicationQuit() {

		sendQuit (); 
	}
	//only for single session application
	public void startSession(){
		session_id = System.Guid.NewGuid ().ToString ().ToUpper ();
	}
	//used to get a new session ID
	public string fetchToken(){
	
		return System.Guid.NewGuid ().ToString ().ToUpper ();
	
	}

	public void clearSession(){

		session_id = "";
	}

	void Start ()
	{
		urlvars = new Hashtable ();
		/*
        foreach (DictionaryEntry item in urlvars)
        {
            Debug.Log(item.Key + ":" + item.Value);
        }
        */
		instance = this;
		guid = System.Guid.NewGuid ().ToString ().ToUpper ();
		url = "http://www.google-analytics.com/collect";

		tracking_id = "INSERT TRACKING ID HERE";
		dev_tracking_id = "INSERT DEV TRACKING ID HERE";
		application_language = "en-us";
		screen_resolution = "INSERT RESOLUTION HERE";
		application_id = "INSERT APPLICATION ID HERE";
		version = "1";

		if (dev_enabled)
			tracking_id = dev_tracking_id;

		urlvars ["v"] = version;
		urlvars ["tid"] = tracking_id;
		urlvars ["cid"] = guid;
		urlvars ["ul"] = application_language;
		urlvars ["vp"] = screen_resolution;
		urlvars ["dr"] = application_id;

		//PlayerPrefs.DeleteKey("dts");
		if (Time.realtimeSinceStartup < 10f)
			sendLoaded ();

		DTS.instance.sendScreen ("/home/", "Start");
	}

	private void clearEvent ()
	{
		urlvars.Remove ("ea");
		urlvars.Remove ("ni");
		urlvars.Remove ("cd1");
		urlvars.Remove ("cd2");
		urlvars.Remove ("cd3");
		urlvars.Remove ("ec");
		urlvars.Remove ("ev");
	}


	public void sendLoaded ()
	{
		clearEvent ();
		urlvars ["ea"] = "Application Loaded";
		urlvars ["ec"] = "Application Event";
		urlvars ["ev"] = "1";
		sendData ("event", session_id);
	}

	public void sendQuit ()
	{
		clearEvent ();

		urlvars ["ea"] = "Application Quit";
		urlvars ["ec"] = "Application Event";
		urlvars ["ev"] = "1";
		sendData ("event", session_id);

		//Debug.Log ("QUIT SENT"); 
	}

	//key = category
	//value = action
	//sid = session ID

	public void sendEvent(string key, string value, string category = "1", string sid = ""){
	
		clearEvent ();

		if (sid != "") {
			session_id = sid; 
		}


		urlvars ["cid"] = System.Guid.NewGuid ().ToString ().ToUpper ();
		urlvars ["ea"] = key;
		urlvars ["ec"] = value;
		urlvars ["ev"] = category;

		sendData ("event", session_id);
	
	}

	public void sendScreen (string screen_slug, string screen_label, string sid = "")
	{

		if (sid != "") {
			session_id = sid; 
		}


		urlvars ["cid"] = System.Guid.NewGuid ().ToString ().ToUpper ();
		urlvars ["dp"] = screen_slug;
		urlvars ["dt"] = screen_label;
		sendData ("pageview", session_id);
	}

	public void sendAttractBreak ()
	{
		clearEvent ();
		urlvars ["ea"] = "Attract Screen Break";
		urlvars ["ec"] = "User Interaction";
		urlvars ["ev"] = "1";
		sendData ("event", session_id);
	}

	public void sendAttractStart ()
	{		
		clearEvent ();
		urlvars ["ea"] = "Attract Screen Start";
		urlvars ["ec"] = "Application Event";
		urlvars ["ev"] = "1";
		sendData ("event", session_id);
	}

	private IEnumerator Dispatch (WWWForm dispURLVARS, Hashtable _tempVars)
	{
		// Debug.Log("DTS CALL");
		// Debug.Log(_tempVars);
		WWW www = new WWW (url, dispURLVARS);
		yield return www;

		if (www.error != null) {
			// this failed, so lets save it for later
			// Debug.Log("DTS FAIL");
			// Debug.Log(www.error);
			saveEvent (_tempVars);
		} else {

			//  Debug.Log("DTS SUCCESS");
			// This worked, so lets check if there is more to send.
			if (PlayerPrefs.HasKey ("dts")) {
				string dtsjson = PlayerPrefs.GetString ("dts");
				List<Hashtable> arr = JsonConvert.DeserializeObject<List<Hashtable>> (dtsjson);

				if (arr.Count > 0) {
					WWWForm prev = hashConvert (arr [0]);
					//  Debug.Log("SENDING ARCHIVED");
					//  Debug.Log(prev);
					StartCoroutine (Dispatch (prev, arr [0]));
					arr.RemoveAt (0);

					string dtsreencode = JsonConvert.SerializeObject (arr);
					PlayerPrefs.SetString ("dts", dtsreencode);
				}
			}
		}
	}

	private void saveEvent (Hashtable urlvars)
	{

		string dtsjson;
		// Check if exists
		if (PlayerPrefs.HasKey ("dts")) {
			// Exists

			//  Debug.Log("ADDING TO PLAYER PREFS");
			dtsjson = PlayerPrefs.GetString ("dts");

			List<Hashtable> arr = JsonConvert.DeserializeObject<List<Hashtable>> (dtsjson);

			//   Debug.Log(arr.Count);

			if (arr.Count < maxRequests) {
				arr.Add (urlvars);
				dtsjson = JsonConvert.SerializeObject (arr);
				PlayerPrefs.SetString ("dts", dtsjson);
			}

		} else {

			//  Debug.Log("CREATING KEY IN PLAYER PREFS");
			// Does not exist, create;
			List<Hashtable> arr = new List<Hashtable> ();
			arr.Add (urlvars);
			dtsjson = JsonConvert.SerializeObject (arr);

			PlayerPrefs.SetString ("dts", dtsjson);
		}
	}

	private WWWForm hashConvert (Hashtable _tempVars)
	{
		WWWForm returnVars = new WWWForm ();
		foreach (DictionaryEntry item in _tempVars) {
			// Debug.Log(item.Key.ToString() + " " + item.Value.ToString()); 
			returnVars.AddField (item.Key.ToString (), item.Value.ToString ());
		}
		return returnVars;
	}

	private void sendData (string hitType, string sid = "")
	{


//		if (Main.instance.exhibit != "rental") {
//			return; 
//		}
		//  Debug.Log("SEND DATA CALLED");
		//  Debug.Log(System.DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"));

		urlvars ["cd6"] = sid; 
		urlvars ["cd5"] = System.DateTime.Now.ToString ("yyyy/MM/dd hh:mm:ss");
		urlvars ["cd4"] = leadingZero (System.DateTime.Now.Second);
		urlvars ["t"] = hitType;


		Debug.Log (urlvars ["ea"] + " - " + urlvars ["ec"] + " - " + urlvars ["ev"] + " - " + sid);

	
		System.DateTime epochStart = new System.DateTime (1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
		System.Int64 cur_time = (System.Int64)(System.DateTime.UtcNow - epochStart).TotalMilliseconds;

		urlvars ["z"] = cur_time.ToString ();
		// Debug.Log (urlvars ["dp"] + " - " + urlvars ["dt"] + " - " + session_id);

		if (islive) {
			urlVars = hashConvert (urlvars);
			Hashtable tempVars = urlvars;
			StartCoroutine (Dispatch (urlVars, tempVars));
			///WWW www = new WWW(url, urlVars);
		}

		if (hitType == "event") {
			clearEvent ();
		}
	}
}
