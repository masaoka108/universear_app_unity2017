/*===============================================================================
Copyright (c) 2015-2017 PTC Inc. All Rights Reserved.
 
Copyright (c) 2012-2015 Qualcomm Connected Experiences, Inc. All Rights Reserved.
 
Vuforia is a trademark of PTC Inc., registered in the United States and other 
countries.
===============================================================================*/
using UnityEngine;
using Vuforia;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;
using System;

/// <summary>
/// This MonoBehaviour implements the Cloud Reco Event handling for this sample.
/// It registers itself at the CloudRecoBehaviour and is notified of new search results as well as error messages
/// The current state is visualized and new results are enabled using the TargetFinder API.
/// </summary>

public class CloudRecoEventHandler : MonoBehaviour, ICloudRecoEventHandler
{
    #region PRIVATE_MEMBERS
    bool m_MustRestartApp;
    ObjectTracker m_ObjectTracker;
    TrackableSettings m_TrackableSettings;
    CloudRecoContentManager m_CloudRecoContentManager;
	string currentURL;
    #endregion // PRIVATE_MEMBERS


    #region PUBLIC_MEMBERS
    /// <summary>
    /// Can be set in the Unity inspector to reference a ImageTargetBehaviour that is used for augmentations of new cloud reco results.
    /// </summary>
    public ImageTargetBehaviour m_ImageTargetTemplate;
    /// <summary>
    /// The scan-line rendered in overlay when Cloud Reco is in scanning mode.
    /// </summary>
    public ScanLine m_ScanLine;
    /// <summary>
    /// Cloud Reco error UI elements.
    /// </summary>
    public Canvas m_CloudErrorCanvas;
    public UnityEngine.UI.Text m_CloudErrorTitle;
    public UnityEngine.UI.Text m_CloudErrorText;

	public string targetMenuURL;
	public string targetMovieURL;
    #endregion //PUBLIC_MEMBERS

	private GameObject InfoCanvas;

	[Serializable]
	class CloudMetaData
	{
		public string  title;
		public string  url;
		public string  linkUrl;
		public string  targetImageUrl;
	}

    #region MONOBEHAVIOUR_METHODS
    /// <summary>
    /// register for events at the CloudRecoBehaviour
    /// </summary>
    void Start()
    {
        m_TrackableSettings = FindObjectOfType<TrackableSettings>();
        m_ScanLine = FindObjectOfType<ScanLine>();
        m_CloudRecoContentManager = FindObjectOfType<CloudRecoContentManager>();

		InfoCanvas = GameObject.Find("InfoCanvas");

        // register this event handler at the cloud reco behaviour
        CloudRecoBehaviour cloudRecoBehaviour = GetComponent<CloudRecoBehaviour>();
        if (cloudRecoBehaviour)
        {
            cloudRecoBehaviour.RegisterEventHandler(this);
        }
    }
    #endregion //MONOBEHAVIOUR_METHODS


    #region ICloudRecoEventHandler_implementation
    /// <summary>
    /// Called when TargetFinder has been initialized successfully
    /// </summary>
    public void OnInitialized()
    {
        Debug.Log("Cloud Reco initialized successfully.");

        // get a reference to the Object Tracker, remember it
        m_ObjectTracker = TrackerManager.Instance.GetTracker<ObjectTracker>();
    }

    /// <summary>
    /// Called if Cloud Reco initialization fails
    /// </summary>
    public void OnInitError(TargetFinder.InitState initError)
    {
        Debug.Log("Cloud Reco initialization error: " + initError.ToString());
        switch (initError)
        {
            case TargetFinder.InitState.INIT_ERROR_NO_NETWORK_CONNECTION:
                {
                    m_MustRestartApp = true;
                    ShowError("Network Unavailable", "Please check your internet connection and try again.");
                    break;
                }
            case TargetFinder.InitState.INIT_ERROR_SERVICE_NOT_AVAILABLE:
                ShowError("Service Unavailable", "Failed to initialize app because the service is not available.");
                break;
        }
    }

    /// <summary>
    /// Called if a Cloud Reco update error occurs
    /// </summary>
    public void OnUpdateError(TargetFinder.UpdateState updateError)
    {
        Debug.Log("Cloud Reco update error: " + updateError.ToString());
        switch (updateError)
        {
            case TargetFinder.UpdateState.UPDATE_ERROR_AUTHORIZATION_FAILED:
                ShowError("Authorization Error", "The cloud recognition service access keys are incorrect or have expired.");
                break;
            case TargetFinder.UpdateState.UPDATE_ERROR_NO_NETWORK_CONNECTION:
                ShowError("Network Unavailable", "Please check your internet connection and try again.");
                break;
            case TargetFinder.UpdateState.UPDATE_ERROR_PROJECT_SUSPENDED:
                ShowError("Authorization Error", "The cloud recognition service has been suspended.");
                break;
            case TargetFinder.UpdateState.UPDATE_ERROR_REQUEST_TIMEOUT:
                ShowError("Request Timeout", "The network request has timed out, please check your internet connection and try again.");
                break;
            case TargetFinder.UpdateState.UPDATE_ERROR_SERVICE_NOT_AVAILABLE:
                ShowError("Service Unavailable", "The service is unavailable, please try again later.");
                break;
            case TargetFinder.UpdateState.UPDATE_ERROR_TIMESTAMP_OUT_OF_RANGE:
                ShowError("Clock Sync Error", "Please update the date and time and try again.");
                break;
            case TargetFinder.UpdateState.UPDATE_ERROR_UPDATE_SDK:
                ShowError("Unsupported Version", "The application is using an unsupported version of Vuforia.");
                break;
        }
    }

    /// <summary>
    /// when we start scanning, unregister Trackable from the ImageTargetTemplate, then delete all trackables
    /// </summary>
    public void OnStateChanged(bool scanning)
    {

        Debug.Log("<color=blue>OnStateChanged(): </color>" + scanning);

        if (scanning)
        {
            // clear all known trackables
            m_ObjectTracker.TargetFinder.ClearTrackables(false);

        }

		if (m_ScanLine != null) {
			m_ScanLine.ShowScanLine(scanning);
		} 
    }

    /// <summary>
    /// Handles new search results
    /// </summary>
    /// <param name="targetSearchResult"></param>
    public void OnNewSearchResult(TargetFinder.TargetSearchResult targetSearchResult)
    {
        Debug.Log("<color=blue>OnNewSearchResult(): </color>" + targetSearchResult.TargetName);

        // This code demonstrates how to reuse an ImageTargetBehaviour for new search results and modifying it according to the metadata
        // Depending on your application, it can make more sense to duplicate the ImageTargetBehaviour using Instantiate(), 
        // or to create a new ImageTargetBehaviour for each new result

        // Vuforia will return a new object with the right script automatically if you use
        // TargetFinder.EnableTracking(TargetSearchResult result, string gameObjectName)

		//okamura comment out
        //m_CloudRecoContentManager.HandleTargetFinderResult(targetSearchResult);

		//okamura add
		//******** 再生する動画のURLをセット
		var data2 = JsonUtility.FromJson<CloudMetaData> (targetSearchResult.MetaData);

		GameObject video = GameObject.Find("Video");
		VideoPlayer vp = video.GetComponent<VideoPlayer>();

//		if (vp.url != "https://s3.amazonaws.com/hlar-test/2dzOmnzKe_test.mp4") {
//			vp.url = "https://s3.amazonaws.com/hlar-test/2dzOmnzKe_test.mp4";
//
//			//ビデオが設定されたならtexture も設定
//			StartCoroutine(SetTextureFromURL());
//		}

		Debug.Log ("data2.url:" + data2.url);
		Debug.Log ("vp.url:" + vp.url);

		if (vp.url != data2.url) {
			//前回Reco時のtextureが表示されるのを防ぐ為、真っ黒のtextureを設定
			SetTextureToBlack();


			vp.aspectRatio = VideoAspectRatio.FitInside;
			vp.url = data2.url;

			Debug.Log ("data2.url-2-:" + data2.url);

			//ビデオが設定されたならtexture も設定
			StartCoroutine(SetTextureFromURL(data2.targetImageUrl, vp));
			Debug.Log ("data2.url-3-:" + data2.url);
		}

		Debug.Log ("data2.url-4-:" + data2.url);


//		Debug.Log("We got a target metadata title: " + data2.title);
//		Debug.Log("We got a target metadata url: " + data2.url);
//		Debug.Log("We got a target metadata linkUrl: " + data2.linkUrl);


		//******** HLARのdbカウントアップ APIへアクセス(制限回数を超えていたらターゲットをinactiveに更新)
		Debug.Log("targetSearchResult.UniqueTargetId: " + targetSearchResult.UniqueTargetId);
		CountUp (targetSearchResult.UniqueTargetId);

		//******** HLARのaccess logにinsert
		InsAccessLog (targetSearchResult.UniqueTargetId);

		//******** ContentsMenu を非表示とする
		GameObject refObj = GameObject.Find("ContentsMenuCanvas");

		if (refObj != null) {
			refObj.SetActive (false);
		}

		//******** Info ボタンを表示
		InfoCanvas.SetActive(true);

		//******** 誘導リンク をセット
		targetMenuURL = data2.linkUrl; 

		if (targetMenuURL == "" || targetMenuURL == null) {
			targetMenuURL = "https://universear.hiliberate.biz/";
		} 
			
		//******** 動画URL をセット
		targetMovieURL = data2.url;
			
		//******** video表示エフェクト particle systemを表示
		GameObject FX_Explosion_Rubble = GameObject.Find("FX_Explosion_Rubble");
		Debug.Log ("Particle_video:" + FX_Explosion_Rubble);
		ParticleSystem PS = FX_Explosion_Rubble.GetComponent<ParticleSystem> ();
		PS.Play ();



	
        //Check if the metadata isn't null
        if (targetSearchResult.MetaData == null)
        {
            Debug.Log("Target metadata not available.");
            //return;
        }
        else
        {
            Debug.Log("MetaData: " + targetSearchResult.MetaData);
            Debug.Log("TargetName: " + targetSearchResult.TargetName);
            Debug.Log("Pointer: " + targetSearchResult.TargetSearchResultPtr);
            Debug.Log("TargetSize: " + targetSearchResult.TargetSize);
            Debug.Log("TrackingRating: " + targetSearchResult.TrackingRating);
            Debug.Log("UniqueTargetId: " + targetSearchResult.UniqueTargetId);
        }

        // First clear all trackables
        m_ObjectTracker.TargetFinder.ClearTrackables(false);

        // enable the new result with the same ImageTargetBehaviour:
        ImageTargetBehaviour imageTargetBehaviour =
            m_ObjectTracker.TargetFinder.EnableTracking(targetSearchResult, m_ImageTargetTemplate.gameObject) as ImageTargetBehaviour;

        //if extended tracking was enabled from the menu, we need to start the extendedtracking on the newly found trackble.
        if (m_TrackableSettings && m_TrackableSettings.IsExtendedTrackingEnabled())
        {
            imageTargetBehaviour.ImageTarget.StartExtendedTracking();
        }

		//vp.Play ();
    }
    #endregion //ICloudRecoEventHandler_implementation


    #region PUBLIC_METHODS
    public void CloseErrorDialog()
    {
        if (m_CloudErrorCanvas)
        {
            m_CloudErrorCanvas.transform.parent.position = Vector3.right * 2 * Screen.width;
            m_CloudErrorCanvas.gameObject.SetActive(false);
            m_CloudErrorCanvas.enabled = false;

            if (m_MustRestartApp)
            {
                m_MustRestartApp = false;
                RestartApplication();
            }
        }
    }
    #endregion //PUBLIC_METHODS

    #region PRIVATE_METHODS
    void ShowError(string title, string msg)
    {
        if (!m_CloudErrorCanvas) return;

        if (m_CloudErrorTitle)
            m_CloudErrorTitle.text = title;

        if (m_CloudErrorText)
            m_CloudErrorText.text = msg;

        // Show the error canvas
        m_CloudErrorCanvas.transform.parent.position = Vector3.zero;
        m_CloudErrorCanvas.gameObject.SetActive(true);
        m_CloudErrorCanvas.enabled = true;
    }

    // Callback for network-not-available error message
    void RestartApplication()
    {
        int startLevel = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex - 2;
        if (startLevel < 0) startLevel = 0;
        UnityEngine.SceneManagement.SceneManager.LoadScene(startLevel);
    }
    #endregion //PRIVATE_METHODS


	public void CountUp(string targetId)
	{
		WWWForm form = new WWWForm();
		form.AddField("targetId", targetId);
		string POST_URL = "https://universear.hiliberate.biz/api/targets/" + targetId + "/set_count_up_and_inactive/";
		WWW www = new WWW(POST_URL, form);
		StartCoroutine("WaitForRequest", www);
	}

	public void InsAccessLog(string targetId)
	{
		WWWForm form = new WWWForm();
		form.AddField("targetId", targetId);

		//operating_system
		string os = WWW.EscapeURL(SystemInfo.operatingSystem);

		//device_unique_identifier
		string ui = WWW.EscapeURL(SystemInfo.deviceUniqueIdentifier);

		// アクセス URL
		string POST_URL = "https://universear.hiliberate.biz/api/targets/" + targetId + "/ins_access_log/?os=" + os + "&ui" + ui;
		WWW www = new WWW(POST_URL, form);
		StartCoroutine("WaitForRequest", www);
	}

	//通信の処理待ち
	private IEnumerator WaitForRequest(WWW www)
	{
		yield return www;
		connectionEnd(www);
	}

	//通信終了後の処理
	private void connectionEnd(WWW www)
	{
		//通信結果をLogで出す
		if (www.error != null)
		{			
			//エラー内容 -> www.error
			Debug.Log("www.error");
			Debug.Log(www.error);
		}
		else
		{
			//通信結果 -> www.text
			Debug.Log("www.text");
			Debug.Log(www.text);
		}
	}


	public IEnumerator SetTextureFromURL(string targetImageUrl, VideoPlayer vp){

		Debug.Log ("SetTextureFromURL -1-");

		//video のタテヨコを合わせる
		StartCoroutine(SetVideoAspect(vp));

		// wwwクラスのコンストラクタに画像URLを指定
//		string url = "https://s3.amazonaws.com/hlar-test/CkcI0eW16_test.jpg";
		string url = targetImageUrl;
		WWW www = new WWW(url);

		// 画像ダウンロード完了を待機
		yield return www;

		// webサーバから取得した画像をRaw Imagで表示する
//		RawImage rawImage = GetComponent<RawImage>();
//		rawImage.texture = www.textureNonReadable;

		Debug.Log ("www.textureNonReadable:" + www.textureNonReadable);

		GameObject video = GameObject.Find("Video");
//		Material videoMaterial = video.GetComponent<Material>();
		Material videoMaterial = video.GetComponent<Renderer>().material;

		Debug.Log ("videoMaterial:" + videoMaterial);

		//Textureをset
		videoMaterial.mainTexture = www.textureNonReadable;

		//video のタテヨコを合わせる
		float height = ((float)www.textureNonReadable.height * (float)0.1) / (float)www.textureNonReadable.width;
		vp.transform.localScale = new Vector3(0.1f, 100, height);

		//Play Buttonのタテヨコを合わせる(タテヨコを逆に指定している)
		GameObject PlayButton = GameObject.Find("PlayButton");
		float heightVideo = ((float)www.textureNonReadable.height * (float)50) / (float)www.textureNonReadable.width;
		PlayButton.GetComponent<RectTransform>().sizeDelta = new Vector2(heightVideo, 50);



		//ピクセルサイズ等倍に
//		rawImage.SetNativeSize();
	}

	public void SetTextureToBlack(){

//		//video のタテヨコを合わせる
//		StartCoroutine(SetVideoAspect(vp));
					

		GameObject video = GameObject.Find("Video");
		Material videoMaterial = video.GetComponent<Renderer>().material;

		//Textureをset
		videoMaterial.mainTexture = Resources.Load("UserInterface/black_background") as Texture;

//		videoMaterial.mainTexture = www.textureNonReadable;

	}


	public IEnumerator SetVideoAspect(VideoPlayer vp){

//		Debug.Log ("SetVideoAspect -1- ");

		bool err = true;
		while(err) {
//			Debug.Log ("SetVideoAspect -2- ");

//			GameObject video = GameObject.Find("Video");
//			VideoPlayer vp = video.GetComponent<VideoPlayer>();

			try  
			{  

				float height = ((float)vp.texture.height * (float)0.1) / (float)vp.texture.width;
				vp.transform.localScale = new Vector3(0.1f, 100, height);

//				Debug.Log ("SetVideoAspect -3- ");
//
//				Debug.Log ("vp.texture.width:" + vp.texture.width);
//				Debug.Log ("vp.texture.height:" + vp.texture.height);
//
//				Debug.Log ("SetVideoAspect -4- ");

				//Play Buttonのタテヨコを合わせる(タテヨコを逆に指定している)
				GameObject PlayButton = GameObject.Find("PlayButton");
				float heightVideo = ((float)vp.texture.height * (float)50) / (float)vp.texture.width;
				PlayButton.GetComponent<RectTransform>().sizeDelta = new Vector2(heightVideo, 50);


//				var tmp = vp.texture.width;
//				err = false;
				break;
			} catch (Exception e) {
				
//				Debug.Log ("SetVideoAspect -5- ");

			}  
			yield return null;
		}

		yield break;
	}



}
