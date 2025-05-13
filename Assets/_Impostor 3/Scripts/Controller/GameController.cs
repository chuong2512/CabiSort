using CodeStage.AntiCheat.Storage;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Ins;
    public GamePlayController gamePlayController;

    public GameObject tutorialHand;
    private float resetTime =1;
    public Transform[] posList;
    private Vector3 currentPos;
    public bool isTutorialLevel = false;

    private void Awake()
    {
        Ins = this;
    }

    private void Start()
    {
        // Get Current Level
        GameManager.Instance.currentLevel = ObscuredPrefs.GetInt("Level");
        // Get remove ads
        GameManager.Instance.isRemoveAds = ObscuredPrefs.GetBool("RemoveAds");
        
        
        if (GameManager.Instance.currentLevel == 0)
        {
            isTutorialLevel = true;
            ObscuredPrefs.SetBool("isSoundOn",true);
            ObscuredPrefs.SetBool("canVibrate",true);
        }
        GUIManager.Instance.isSoundTurnOn = ObscuredPrefs.GetBool("isSoundOn");
        GUIManager.Instance.canVibrate = ObscuredPrefs.GetBool("canVibrate");
        
        Debug.Log(GUIManager.Instance.isSoundTurnOn);
        
        gamePlayController.DataController.LoadMap();
        GUIManager.Instance.SetDefaultGUI();
        
    }

    public void SetHandToCurrentBox()
    {
        currentPos = posList[0].transform.position;
    }
    
    public void SetHandToSelectedBox()
    {
        currentPos = posList[1].transform.position;
    }

    public void SetupTutorial()
    {
        currentPos = posList[0].transform.position;
        
    }
    
    private void MoveHand()
    {
        if (resetTime>0)
        {
            tutorialHand.transform.position += new Vector3(-1*0.5f*Time.deltaTime,0.5f*Time.deltaTime,0);
            resetTime -= Time.deltaTime;
        }
        else
        {
            tutorialHand.transform.position = currentPos;
            resetTime = 1;
        }
    }
    
    void Update()
    {
        if (isTutorialLevel)
        {
            if (tutorialHand == null) return;
            MoveHand();
        }
    }
}
