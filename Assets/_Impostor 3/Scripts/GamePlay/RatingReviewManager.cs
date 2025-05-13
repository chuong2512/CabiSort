using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatingReviewManager : SingletonMonoDontDestroy<RatingReviewManager>
{
    public GameObject ratingReviewPanel;
    public GameObject sorryReviewPanel;

    public void EnableRatingReviewPanel()
    {
        ratingReviewPanel.SetActive(true);
    }
    
    public void UnableRatingReviewPanel()
    {
        ratingReviewPanel.SetActive(false);
    }

    private void EnableSorryReviewPanel()
    {
        sorryReviewPanel.SetActive(true);
    }
    
    private void UnableSorryReviewPanel()
    {
        sorryReviewPanel.SetActive(false);
    }
    
    public void DontRate()
    {
        EnableSorryReviewPanel();
        UnableRatingReviewPanel();
    }

    public void MoveToWinningCanvas()
    {
        UnableRatingReviewPanel();
        UnableSorryReviewPanel();
        this.StartDelayMethod(0,()=>GUIManager.Instance.ActivatingWinningCanvas());
    }
    
    public void GoToRate()
    {
        
        #if UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.Lucy.ImpostorSortPuzzle");
        MoveToWinningCanvas();
        #endif
        
        
    }
}
