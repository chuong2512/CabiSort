using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdManager : MonoBehaviour
{
	[Header("Admob Ad Units :")] string idBanner      ="ca-app-pub";
	string                              idInterstitial="ca-app-pub";
	string                              idReward      ="ca-app-pub";


	AndroidJavaObject currentActivity;
	AndroidJavaClass  UnityPlayer;
	AndroidJavaObject context;
	AndroidJavaObject toast;

	[Header("Toggle Admob Ads :")] private bool bannerAdEnabled      =true;
	private                                bool interstitialAdEnabled=true;
	private                                bool rewardedAdEnabled    =true;

	public GameObject GDPR;

	public static AdManager Instance;
	public        bool      _firstInit=true;

	protected void Awake()
	{
		if(Instance==null)
		{
			DontDestroyOnLoad(this);

#if UNITY_ANDROID && !UNITY_EDITOR
        UnityPlayer=
            new AndroidJavaClass("com.unity3d.player.UnityPlayer");

        currentActivity=UnityPlayer
            .GetStatic<AndroidJavaObject>("currentActivity");


        context=currentActivity
            .Call<AndroidJavaObject>("getApplicationContext");
#endif

			Instance=this;

			// show banner every scene loaded
			SceneManager.sceneLoaded+=(Scene s,LoadSceneMode lsm) =>
			{
				if(PlayerPrefs.GetInt("npa",-1)==-1)
				{
					GDPR.SetActive(true);
					Time.timeScale=0;
				}
				else
				{
					if(_firstInit) this.InitAd();
				}
			};

		}
		else
		{
			Destroy(this.gameObject);
		}


	}

	public void ShowToast(string message)
	{
#if UNITY_EDITOR
		Debug.Log(message);
#elif UNITY_ANDROID
            currentActivity.Call
                (
                    "runOnUiThread",
                    new AndroidJavaRunnable(() =>
                    {
                        AndroidJavaClass Toast
                        =new AndroidJavaClass("android.widget.Toast");
            
                        AndroidJavaObject javaString
                        =new AndroidJavaObject("java.lang.String", message);
            
                        toast=Toast.CallStatic<AndroidJavaObject>
                        (
                            "makeText",
                            context,
                            javaString,
                            Toast.GetStatic<int>("LENGTH_SHORT")
                        );
            
                        toast.Call("show");
                    })
                 );
#endif
	}

	public void OnUserClickAccept()
	{
		PlayerPrefs.SetInt("npa",0);
		GDPR.SetActive(false);
		Time.timeScale=1;
		if(_firstInit) this.InitAd();
		Destroy(GDPR);
	}


	public void OnUserClickCancel()
	{
		PlayerPrefs.SetInt("npa",1);
		GDPR.SetActive(false);
		Time.timeScale=1;
		if(_firstInit) this.InitAd();
		Destroy(GDPR);
	}

	public void OnUserClickPrivacyPolicy() { Application.OpenURL("http://polarisgamestudio.epizy.com/policy.html"); }

	public void ClickAD()
	{
		PlayerPrefs.SetInt("npa",-1);
		if(PlayerPrefs.GetInt("npa",-1)==-1)
		{
			if(GDPR==null)
			{
				GameObject original=Resources.Load<GameObject>("CanvasGDPR");
				GDPR=UnityEngine.Object.Instantiate<GameObject>(original);
			}
			GDPR.SetActive(true);
			Time.timeScale=0;
		}

		_firstInit=true;
	}

	public void InitAd() { }

	private void OnDestroy() { }

	public void Destroy() => Destroy(gameObject);

	public bool IsRewardAdLoaded() { return false; }



}
