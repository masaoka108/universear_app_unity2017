using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour {

	GameObject ContentsMenuCanvas;
	GameObject InfoCanvas;
	// Use this for initialization
	void Start () {
		ContentsMenuCanvas = GameObject.Find("ContentsMenuCanvas");
		InfoCanvas = GameObject.Find("InfoCanvas");
	}

	public void OnClickInfo() {
		Debug.Log("OnClickInfo click!");

		if (ContentsMenuCanvas != null) {
			ContentsMenuCanvas.SetActive (true);

			if (InfoCanvas != null) {
				InfoCanvas.SetActive (false);
			}
		}
	}

	public void OnClickLink() {
		Debug.Log("OnClickLink click!");

		GameObject CloudRecognition = GameObject.Find("CloudRecognition");
		Debug.Log ("CloudRecognition:" + CloudRecognition);
		CloudRecoEventHandler CloudRecoEventHandler = CloudRecognition.GetComponent<CloudRecoEventHandler>();
		Debug.Log ("CloudRecoEventHandler:" + CloudRecoEventHandler);
		Debug.Log ("CloudRecoEventHandler.targetMenuURL:" + CloudRecoEventHandler.targetMenuURL);

		Application.OpenURL (CloudRecoEventHandler.targetMenuURL);
	}

	public void OnClickFullScreen() {
		Debug.Log("OnClickFullScreen click!");

		StartCoroutine ("StartUnityVideo");
//
//		VideoPlaybackBehaviour video = PickVideo ();
//
//		Debug.Log ("fullscreen アイコン");
//		Debug.Log (video);
//
//		if (video != null) {
//			video.VideoPlayer.Pause ();
//
//			VideoPlayerHelper.MediaState state = VideoPlayerHelper.MediaState.NOT_READY;
//			while(state != VideoPlayerHelper.MediaState.PAUSED) {
//				state = video.VideoPlayer.UpdateVideoData();
//
//				Debug.Log("OnClickFullScreen:WhileStateCheck:-1- : " + state);
//
//				if (state == VideoPlayerHelper.MediaState.PAUSED) {
//					break;
//				} 
//			}
//
//
//			//			Screen.orientation = ScreenOrientation.LandscapeLeft;
//
//			//video.VideoPlayer.PlayFullScreen();
//
//			StartCoroutine ("StartUnityVideo");
//
//			//			GameObject refObj = GameObject.Find("TargetMenuPlane");
//			//			TapEvent tapEvent = refObj.GetComponent<TapEvent>();
//			//			Handheld.PlayFullScreenMovie (tapEvent.fullScreenURL, Color.black, FullScreenMovieControlMode.Full, FullScreenMovieScalingMode.AspectFit);
//		}
	}

	public void OnClickFacebook() {
		Debug.Log("OnClickFullFacebook click!");

		Debug.Log ("fb_icon --1--");

		string fbURL = WWW.EscapeURL ("https://universear.hiliberate.biz/hlar/");
		Debug.Log ("fb_icon --2--");
		Debug.Log (fbURL);

		Application.OpenURL ("https://www.facebook.com/share.php?u=" + fbURL);

		Debug.Log ("fb_icon --3--");

	}

	public void OnClickTwitter() {
		Debug.Log("OnClickFullTwitter click!");

		//@ToDo ここでターゲット画像のURLを取得して添付する
		//					string tweetMsg = WWW.EscapeURL ("ARアプリUNIVERSE https://universear.hiliberate.biz/static/images/IMG_1272.JPG");
		string tweetMsg = WWW.EscapeURL ("ARアプリ【UNIVERSE AR】画像と動画を登録するだけで誰でも簡単にオリジナルARコンテンツを作成可能！");
		string tweetURL = WWW.EscapeURL ("https://universear.hiliberate.biz/hlar/");
		//string tweetMsg = "ARアプリUNIVERSE";
		Application.OpenURL ("https://twitter.com/share?text=" + tweetMsg + "&url=" + tweetURL);
	}


	public void OnClickClose() {
		Debug.Log("OnClickClose click!");

		GameObject ContentsMenuCanvas = GameObject.Find ("ContentsMenuCanvas");
		if (ContentsMenuCanvas != null) {
			Debug.Log ("TapEvent updat:ContentsMenuCanvas:false");
			ContentsMenuCanvas.SetActive (false);

			if (InfoCanvas != null) {
				InfoCanvas.SetActive (true);
			}
		}


	}


//	//@ToDo 共通化したい
//	private VideoPlaybackBehaviour PickVideo()
//	{
//		VideoPlaybackBehaviour[] behaviours = GameObject.FindObjectsOfType<VideoPlaybackBehaviour>();
//		foreach (VideoPlaybackBehaviour vb in behaviours)
//		{
//			return vb;
//		}
//		return null;
//	}
//
	IEnumerator StartUnityVideo()
	{
		yield return new WaitForSeconds(2);
		Debug.Log ("StartUnityVideo");

		GameObject CloudRecognition = GameObject.Find("CloudRecognition");
		Debug.Log ("CloudRecognition:" + CloudRecognition);
		CloudRecoEventHandler CloudRecoEventHandler = CloudRecognition.GetComponent<CloudRecoEventHandler>();
		Debug.Log ("CloudRecoEventHandler:" + CloudRecoEventHandler);
		Debug.Log ("CloudRecoEventHandler.targetMovieURL:" + CloudRecoEventHandler.targetMovieURL);


		Handheld.PlayFullScreenMovie (CloudRecoEventHandler.targetMovieURL, Color.black, FullScreenMovieControlMode.Full, FullScreenMovieScalingMode.AspectFit);

		Debug.Log ("FIN Iniciando video");
		yield break;
	}
}
