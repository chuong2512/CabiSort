using System.Collections.Generic;
using CodeStage.AntiCheat.Storage;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : SingletonMonoDontDestroy<GUIManager>
{
    public GameObject gamePlayPanel;
    public GameObject menuPanel;
    public GameObject winningCanvasPanel;
    public GameObject videoGO;
    public GameObject priceGO;
    public Text levelText;
    public Text rewindText;
    public AudioSource sound;
    public AudioClip[] soundList;

    
    public List<AudioSource> allSound;

    public Image soundImg;
    public Sprite[] soundImgSprite;
    
    public Image vibrateImg;
    public Sprite[] vibrateImgSprite;

    public bool isSoundTurnOn = true;
    public bool canVibrate = true;

    
    
    public void SetDefaultGUI()
    {
        levelText.text = "Level " + (GameManager.Instance.currentLevel  + 1);
        rewindText.text = GameManager.Instance.numberRewinds.ToString();
        if (isSoundTurnOn)
        {
            soundImg.sprite = soundImgSprite[1];
            foreach (AudioSource _audio in allSound)
            {
                _audio.volume = 1;
            }
        }
        else
        {
            soundImg.sprite = soundImgSprite[0];
            foreach (AudioSource _audio in allSound)
            {
                _audio.volume = 0;
            }
        }
        if (canVibrate)
        {
            vibrateImg.sprite = vibrateImgSprite[1];
        }
        else
        {
            vibrateImg.sprite = vibrateImgSprite[0];
        }
        
        if (GameManager.Instance.numberRewinds==0)
        {
            EnableWatchingVideo();
        }
    }

    public void ActivatingMenu()
    {
        PlayClickBoxSound();
        // IAPManager.Instance().Init(null);
        // var localPrice = IAPManager.Instance().GetPriceStringById(ProductId.PackageNoAds);
        // if (localPrice.Length == 0 || localPrice == "0")
        // {
        //     priceGO.GetComponent<Text>().text = $"$2.99";
        // }
        // else
        // {
        //     priceGO.GetComponent<Text>().text = localPrice;
        // }
        menuPanel.SetActive(true);
    }
    
    public void HideMenu()
    {
        menuPanel.SetActive(false);
    }
    
    public void EnableWatchingVideo()
    {
        videoGO.SetActive(true);
    }
    
    public void WatchVideo1()
    {
        if (!GameManager.Instance.isRemoveAds)
        {
            // todo txy
//            AdsManager.Instance.ShowInterstitialAds();
            //AdManager.Instance.ShowInterstitialAd();
        }
    }
    
    public void WatchVideo2()
    {
        
        // todo txy
        /*AdManager.Instance.RewardAction = () =>
        {
            GameManager.Instance.numberRewinds = 5;
            SetDefaultGUI();
            videoGO.SetActive(false);
        };
        AdManager.Instance.ShowRewardAd();*/
        
//        AdsManager.Instance.ShowAds(AdsPlacement.ADS_MAIN, "rewind_bonus", () =>
//        {
//            GameManager.Instance.numberRewinds = 5;
//            SetDefaultGUI();
//            videoGO.SetActive(false);
//        });
    }
    
    public void WatchVideo3()
    {
        PlayClickBoxSound();
        if (!GameManager.Instance.isAddBox)
        {
            // todo txy
          //  AdManager.Instance.RewardAction = () =>
            {
                AddOneBox();
            };
           // AdManager.Instance.ShowRewardAd();
            
//            AdsManager.Instance.ShowAds(AdsPlacement.ADS_MAIN, "add_box", () =>
//            {
//                AddOneBox();
//            });
        }
    }

    public void Replay()
    {
        PlayClickBoxSound();
        videoGO.SetActive(false);
        GameController.Ins.gamePlayController.Replay();
    }
    
    private bool isEnbleToRewind()
    {
        return GameManager.Instance.numberRewinds > 0;
    }
    
    public void Rewind()
    {
        PlayClickBoxSound();
        if (isEnbleToRewind())
        {
            if (GameController.Ins.gamePlayController.movementSaveStack.Count > 0)
            {
                GameController.Ins.gamePlayController.RewindPlay();
            }
        }
        else
        {
            if (GameManager.Instance.numberRewinds==0)
            {
                EnableWatchingVideo();
                WatchVideo2();
            }
        }
       
    }

    public void AddOneBox()
    {
        GameController.Ins.gamePlayController.AddOneBox();
    }

    public void Shopping()
    {
        PlayClickBoxSound();
    }

    public void RemoveAds()
    {
        PlayClickBoxSound();
        // IAPManager.Instance().Buy(ProductId.PackageNoAds, (success, str) =>
        // {
        //     if (success)
        //     {
        //         GameManager.Instance.isRemoveAds = true;
        //         ObscuredPrefs.SetBool("RemoveAds",true);
        //     }
        // });
    }
    
    public void TurnOnOffSound()
    {
        PlayClickBoxSound();
        if (isSoundTurnOn)
        {
            soundImg.sprite = soundImgSprite[0];
            foreach (AudioSource _audio in allSound)
            {
                _audio.volume = 0;
            }
        }
        else
        {
            soundImg.sprite = soundImgSprite[1];
            foreach (AudioSource _audio in allSound)
            {
                _audio.volume = 1;
            }
        }
        isSoundTurnOn = !isSoundTurnOn;
        //Debug.Log(isSoundTurnOn);
        ObscuredPrefs.SetBool("isSoundOn",isSoundTurnOn);
    }

    public void Vibrating()
    {
        if (!canVibrate) return;
        this.StartDelayMethod( 0.5f,()=>Handheld.Vibrate());
    }
    
    public void TurnOnOffVibrate()
    {
        PlayClickBoxSound();
        if (canVibrate)
        {
            vibrateImg.sprite = vibrateImgSprite[0];
        }
        else
        {
            vibrateImg.sprite = vibrateImgSprite[1];
        }
        canVibrate = !canVibrate;
        ObscuredPrefs.SetBool("canVibrate",canVibrate);
    }

    public void GivenGift()
    {
        PlayClickBoxSound();
    }
    
    public void PlayClickBoxSound()
    {
        sound.PlayOneShot(soundList[1]);
    }
    
    public void PlayFallingSound()
    {
        sound.PlayOneShot(soundList[2]);
    }
    
    public void PlaySolveSound()
    {
        sound.PlayOneShot(soundList[3]);
    }

    public void PlayWinningSound()
    {
        sound.PlayOneShot(soundList[4]);
    }

    public void PlayWrongPick()
    {
        sound.PlayOneShot(soundList[5]);
    }

    public void ActivatingWinningCanvas()
    {
        winningCanvasPanel.SetActive(true);
    }
    
    public void EndWinningCanvas()
    {
        winningCanvasPanel.SetActive(false);
    }

    public void NextLevel()
    {
        PlayClickBoxSound();
        SetDefaultGUI();
        EndWinningCanvas();
        GameController.Ins.gamePlayController.NextLevel();
    }


}
