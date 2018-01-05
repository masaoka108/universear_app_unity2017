using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdMob : MonoBehaviour {

	public BannerView bannerView;

	// Use this for initialization
	void Start () {

	}

	void Awake(){
		//		DontDestroyOnLoad (this);
		// バナー広告を表示
		RequestBanner ();

		bannerView.Show ();

	}

//	// Update is called once per frame
//	void Update () {
//
//	}

	private void RequestBanner()
	{
		#if UNITY_EDITOR
		string adUnitId = "ca-app-pub-7513980837257698/1850316452";
		#elif UNITY_ANDROID
		string adUnitId = "INSERT_ANDROID_BANNER_AD_UNIT_ID_HERE";
		#elif UNITY_IPHONE
		string adUnitId = "ca-app-pub-7513980837257698/1850316452";
		#else
		string adUnitId = "unexpected_platform";
		#endif

		// Create a 320x50 banner at the top of the screen.
		bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
		// Create an empty ad request.
		//		AdRequest request = new AdRequest.Builder().Build();

		AdRequest request = new AdRequest.Builder()
		.AddTestDevice(AdRequest.TestDeviceSimulator)       // Simulator.
		.AddTestDevice("92df1d22f04535c98f2fddb147c02e8c")  // My test device.
		.Build();


		// Load the banner with the request.
		bannerView.LoadAd(request);
		}

}
