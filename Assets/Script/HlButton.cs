using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HlButton : MonoBehaviour {

	/// ボタンをクリックした時の処理
	public void OnClick() {
		Debug.Log("Button click!");
		Application.OpenURL("https://universear.hiliberate.biz/");
	}

//	/// ボタンをクリックした時の処理
//	public void OnClickVolumeOn() {
//		Debug.Log("Button ON click!");
//		GameObject refObj = GameObject.Find("TargetMenuPlane");
//		TapEvent tapEvent = refObj.GetComponent<TapEvent>();
//
//		tapEvent.VolumeOnButton.SetActive (false);
//		tapEvent.VolumeOffButton.SetActive (true);
//
//		GameObject refObj2 = GameObject.Find("CloudRecoTarget");
//		TrackableEventHandler teh = refObj2.GetComponent<TrackableEventHandler>();
//		teh.video.VideoPlayer.VolumeOff ();
//
//		//		video.VideoPlayer.GetStatus();
//	}
//
//	/// ボタンをクリックした時の処理
//	public void OnClickVolumeOff() {
//		Debug.Log("Button OFF click!");
//		GameObject refObj = GameObject.Find("TargetMenuPlane");
//		TapEvent tapEvent = refObj.GetComponent<TapEvent>();
//
//		tapEvent.VolumeOnButton.SetActive (true);
//		tapEvent.VolumeOffButton.SetActive (false);
//
//		GameObject refObj2 = GameObject.Find("CloudRecoTarget");
//		TrackableEventHandler teh = refObj2.GetComponent<TrackableEventHandler>();
//		teh.video.VideoPlayer.VolumeOn ();
//	}


	// Use this for initialization
	void Start () {
		Image _This = this.GetComponent<Image>();
		_This.color = new Color(1,1,1,0.5f);

	}
		
	//	// Update is called once per frame
	//	void Update () {
	//		
	//	}
}
