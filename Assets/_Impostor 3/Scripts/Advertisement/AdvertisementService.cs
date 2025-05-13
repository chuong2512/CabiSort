// using System.Collections;
// using UnityEngine;
// using System;
//
// public class AdvertisementService : AdvertisementManager
// {
//     private string IronSourceAppKey
//     {
//         get
//         {
//             //if (Application.platform == RuntimePlatform.IPhonePlayer)
//             //{
//             //    return iosIronSourceAppKey;
//             //}
//             //else if (GameManager.Instance.isBuyVersion)
//             //{
//             //    return paidIronSourceAppKey;
//             //}
//             //else
//             //{
//                 return freeIronSourceAppKey;
//             //}
//         }
//     }
//
//     int RewardedRequestsTry = 0;
//     bool FirstTimeRewardedListeners = true;
//     bool FirstTimeInterstitialListeners = true;
//     [System.NonSerialized] public bool HasWatchedRewardedAds = false;
//
//     private Action finishVideo;
//     private Action skipVideo;
//     private Action failVideo;
//
//     private bool isInit = false;
//     public override bool IsLoadedInterstitial
//     {
//         get
//         {
//             if (IronSource.Agent.isInterstitialReady() == false)
//             {
//                 RequestInterstitial();
//             }
//             return IronSource.Agent.isInterstitialReady();
//         }
//     }
//
//     public override bool IsReadyVideo
//     {
//         get
//         {
//             return IronSource.Agent.isRewardedVideoAvailable();
//         }
//     }
//
//     public override void LoadAds()
//     {
//         if (!isInit)
//         {
//             isInit = true;
//             // TODO: Remove debug Unity Ads when game release
//             //IronSource.Agent.setAdaptersDebug(true);
//             IronSource.Agent.init(IronSourceAppKey, IronSourceAdUnits.REWARDED_VIDEO);
//             IronSourceConfig.Instance.setClientSideCallbacks(true);
//             IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
//             IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
//             IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
//             IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
//             IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
//             IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
//             IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
//             IronSourceEvents.onRewardedVideoAdClickedEvent += RewardedVideoAdClickEvent;
//
//
//             IronSource.Agent.init(IronSourceAppKey, IronSourceAdUnits.INTERSTITIAL);
//             IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
//             IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
//             IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
//             IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
//             IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
//             IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
//             IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;
//             
//             IronSource.Agent.init(IronSourceAppKey, IronSourceAdUnits.BANNER);
//             IronSourceEvents.onBannerAdLoadedEvent += BannerAdLoadedEvent;
//             IronSourceEvents.onBannerAdLoadFailedEvent += BannerAdLoadFailedEvent;        
//             IronSourceEvents.onBannerAdClickedEvent += BannerAdClickedEvent; 
//             IronSourceEvents.onBannerAdScreenPresentedEvent += BannerAdScreenPresentedEvent; 
//             IronSourceEvents.onBannerAdScreenDismissedEvent += BannerAdScreenDismissedEvent;
//             IronSourceEvents.onBannerAdLeftApplicationEvent += BannerAdLeftApplicationEvent;
//
//             IronSource.Agent.validateIntegration();
//             //if (isDebugMode)
//             //{
//             //    IronSource.Agent.validateIntegration();
//             //}
//             
//             RequestInterstitial();
//         }
//     }
//
//     #region Reward Ads
//  //Invoked when the RewardedVideo ad view has opened.
//     //Your Activity will lose focus. Please avoid performing heavy 
//     //tasks till the video ad will be closed.
//     void RewardedVideoAdOpenedEvent()
//     {
//         _isAdsRewarded = false;
//         //if (isDebugMode)
//         //{
//         //    Debug.LogError("[unity-script] RewardedVideoAdOpenedEvent");
//         //}
//     }
//     //Invoked when the RewardedVideo ad view is about to be closed.
//     //Your activity will now regain its focus.
//     void RewardedVideoAdClosedEvent()
//     {
//         if (!_isAdsRewarded)
//         {
//             if (skipVideo != null)
//             {
//                 skipVideo.Invoke();
//                 skipVideo = null;
//             }
//         }
//         //if (isDebugMode)
//         //{
//         //    Debug.LogError("[unity-script] RewardedVideoAdClosedEvent");
//         //}
//     }
//     //Invoked when there is a change in the ad availability status.
//     //@param - available - value will change to true when rewarded videos are available. 
//     //You can then show the video by calling showRewardedVideo().
//     //Value will change to false when no videos are available.
//     void RewardedVideoAvailabilityChangedEvent(bool available)
//     {
//         //Change the in-app 'Traffic Driver' state according to availability.
//         //IsReadyVideo = available;
//     }
//
//     private bool _isAdsRewarded;
//     //  Note: the events below are not available for all supported rewarded video 
//     //   ad networks. Check which events are available per ad network you choose 
//     //   to include in your build.
//     //   We recommend only using events which register to ALL ad networks you 
//     //   include in your build.
//     //Invoked when the video ad starts playing.
//     void RewardedVideoAdStartedEvent()
//     {
//         //if (isDebugMode)
//         //{
//         //    Debug.LogError("[unity-script] RewardedVideoAdStartedEvent");
//         //}
//     }
//
//     void RewardedVideoAdClickEvent(IronSourcePlacement placement)
//     {
//         //if (isDebugMode)
//         //{
//         //    Debug.LogError("[unity-script] RewardedVideoAdStartedEvent");
//         //}
//         //StickmanTracking.Instance.Firebase.EventAds(ItemSources.Click, AdsType.Rewarded.ToString(), AdsManager.Instance.lastSource, true);
//     }
//     //Invoked when the video ad finishes playing.
//     void RewardedVideoAdEndedEvent()
//     {
//         //if (isDebugMode)
//         //{
//         //    Debug.LogError("[unity-script] RewardedVideoAdEndedEvent");
//         //}
//     }
//     //Invoked when the user completed the video and should be rewarded. 
//     //If using server-to-server callbacks you may ignore this events and wait for the callback from the  ironSource server.
//     //
//     //@param - placement - placement object which contains the reward data
//     //
//     void RewardedVideoAdRewardedEvent(IronSourcePlacement placement)
//     {
//         _isAdsRewarded = true;
//         if (finishVideo != null)
//         {
//             this.finishVideo.Invoke();
//             this.finishVideo = null;
//         }
//     }
//     //Invoked when the Rewarded Video failed to show
//     //@param description - string - contains information about the failure.
//     void RewardedVideoAdShowFailedEvent(IronSourceError error)
//     {
//         //if (isDebugMode)
//         //{
//         //    Debug.LogError("[unity-script] RewardedVideoAdShowFailedEvent");
//         //}
//         if (failVideo != null)
//         {
//             this.failVideo.Invoke();
//             this.failVideo = null;
//         }
//     }
//
//
//     public override void ShowVideo(AdsPlacement adsPlacement, Action finished = null, Action failed = null, Action skipped = null)
//     {
//         this.finishVideo = finished;
//         this.skipVideo = skipped;
//         this.failVideo = failed;
//         try
//         {
//             IronSource.Agent.showRewardedVideo(ADS_PLACE[(int)adsPlacement]);
//         }
//         catch
//         {
//             IronSource.Agent.showRewardedVideo();
//         }
//     }
//     
//
//     #endregion
//     
//     #region Interstitial
//
//     /************* Interstitial Delegates *************/
//     void InterstitialAdReadyEvent()
//     {
//         //if (isDebugMode)
//         //{
//         //    Debug.LogError("unity-script: I got InterstitialAdReadyEvent");
//         //}
//     }
//
//     void InterstitialAdLoadFailedEvent(IronSourceError error)
//     {
//         //if (isDebugMode)
//         //{
//         //    Debug.LogError("unity-script: I got InterstitialAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
//         //}
//     }
//
//     void InterstitialAdShowSucceededEvent()
//     {
//         //if (isDebugMode)
//         //{
//         //    Debug.LogError("unity-script: I got InterstitialAdShowSucceededEvent");
//         //}
//     }
//
//     void InterstitialAdShowFailedEvent(IronSourceError error)
//     {
//         //if (isDebugMode)
//         //{
//         //    Debug.LogError("unity-script: I got InterstitialAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
//         //}
//     }
//
//     void InterstitialAdClickedEvent()
//     {
//         //if (isDebugMode)
//         //{
//         //    Debug.LogError("unity-script: I got InterstitialAdClickedEvent");
//         //}
//
//         //StickmanTracking.Instance.Firebase.EventAds(ItemSources.Click, AdsType.Intersititial.ToString(), ModeManager.Instance.CurrentModePlay.ToString(), true);
//     }
//
//     void InterstitialAdOpenedEvent()
//     {
//         //if (isDebugMode)
//         //{
//         //    Debug.LogError("unity-script: I got InterstitialAdOpenedEvent");
//         //}
//     }
//
//     void InterstitialAdClosedEvent()
//     {
//         RequestInterstitial();
//         //if (isDebugMode)
//         //{
//         //    Debug.LogError("unity-script: I got InterstitialAdClosedEvent");
//         //}
//     }
//
//     void InterstitialAdRewardedEvent()
//     {
//         //if (isDebugMode)
//         //{
//         //    Debug.LogError("unity-script: I got InterstitialAdRewardedEvent");
//         //}
//     }
//
//     public override void ShowInterstitial()
//     {
//         if (IsLoadedInterstitial)
//         {
//             this.StartDelayMethod(0, () =>
//             {
//                 if (IronSource.Agent.isInterstitialReady())
//                     IronSource.Agent.showInterstitial();
//             });
//         }
//     }
//
//     void OnApplicationPause(bool isPaused)
//     {
//         IronSource.Agent.onApplicationPause(isPaused);
//     }
//
//     public override void RequestInterstitial()
//     {
//         base.RequestInterstitial();
//         IronSource.Agent.loadInterstitial();
//     }
//     #endregion
//
//     #region Banner
//
//     public override void LoadBanner()
//     {
//         IronSource.Agent.loadBanner(new IronSourceBannerSize(728, 50), IronSourceBannerPosition.BOTTOM);
//     }
//
//     //Invoked once the banner has loaded
//     void BannerAdLoadedEvent() {
//     }
//     //Invoked when the banner loading process has failed.
//     //@param description - string - contains information about the failure.
//     void BannerAdLoadFailedEvent (IronSourceError error) {
//     }
//     // Invoked when end user clicks on the banner ad
//     void BannerAdClickedEvent () {
//     }
//     //Notifies the presentation of a full screen content following user click
//     void BannerAdScreenPresentedEvent () {
//     }
//     //Notifies the presented screen has been dismissed
//     void BannerAdScreenDismissedEvent() {
//     }
//     //Invoked when the user leaves the app
//     void BannerAdLeftApplicationEvent() {
//     }
//
//     #endregion
// }