using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;
using UnityEngine.UI;
using Zitga.CsvTools.Tutorials;
using CodeStage.AntiCheat.Storage;

public class Movement
{
    public Box currentBox;
    public Box targetBox;
    public Imposter imposter;

    public Movement(Box currentBox, Box targetBox, Imposter imposter)
    {
        this.currentBox = currentBox;
        this.targetBox = targetBox;
        this.imposter = imposter;
    }
}

public class GamePlayController : MonoBehaviour
{
    
    public DataController DataController;
    public GraphicController GraphicController;
    
    public int numberRows;
    public int maxBoxPerRow;
    public int maxValueCols;
    public int numberCols;
    public int maxCols;
    
    public int numberColors=4;
    
    public Stack<Movement> movementSaveStack = new Stack<Movement>();

    private bool isPeeking;

    public bool IsPeeking
    {
        get => isPeeking;
        set => isPeeking = value;
    }

    // x for value, y for colValue
    public Box currentBox;
    public Box selectedBox;
    public Imposter selectedImposter;

    private void SetDefault()
    {
        isPeeking = false;
        GraphicController.SetDefaultAnimation();
        movementSaveStack.Clear();
    }
    
    public bool isCancelHandle()
    {
        if (selectedBox.Imposters.Count == 0) return false;
        if (selectedBox.Imposters.Count >= maxValueCols) return true;
        return (currentBox == selectedBox) || (currentBox.Imposters.Peek().id != selectedBox.Imposters.Peek().id);
    }

    public void SaveLastMove()
    {
        selectedImposter.transform.position = Vector3.zero;
        
        Movement saveLastMoving = new Movement(currentBox,selectedBox,selectedImposter);
        movementSaveStack.Push(saveLastMoving);
    }
    
    public void ResetPeeking()
    {
        IsPeeking = false;
        selectedImposter = null;
        currentBox = null;
        selectedBox = null;
    }

    public void RewindPlay()
    {
        if (movementSaveStack.Count != 0)
        {
            if (currentBox != null)
            {
                currentBox.effect.SetActive(false);
                selectedImposter._animation.AnimationState.SetAnimation(0, "Fall", false);
            }
            
            Movement lastMoving = movementSaveStack.Peek();
            movementSaveStack.Pop();
            selectedImposter = lastMoving.targetBox.Imposters.Peek();
            currentBox = lastMoving.targetBox;
            selectedBox = lastMoving.currentBox;
            IsPeeking = true;
            lastMoving.imposter.MoveToTop(0f);
            currentBox.moveItem(currentBox,selectedBox,selectedImposter);
            ResetPeeking();

            
            GameManager.Instance.numberRewinds -= 1;
            GUIManager.Instance.SetDefaultGUI();
        }
    }

    public bool isWinning()
    {
        int cnt = 0;
        foreach (var box in GraphicController.Boxes)
        {
            if (box.isDoneBox())
            {
                cnt++;
            }
        }
        return cnt==maxCols;
    }

    private void ResetAndUpdateGameManager()
    {
        GameManager.Instance.currentLevel++;
//        GameManager.Instance.numberRewinds = 5;
        GameManager.Instance.isAddBox = false;
        GUIManager.Instance.PlayWinningSound();
        GUIManager.Instance.videoGO.SetActive(false);
        
        ObscuredPrefs.SetInt("Level",GameManager.Instance.currentLevel);
        
        movementSaveStack.Clear();
    }

    private bool isGetToDestinationLevel()
    {
        return GameManager.Instance.currentLevel == 5 || GameManager.Instance.currentLevel % 15 == 0;
    }
    
    public void Winning()
    {
        if (isWinning())
        {
            ResetAndUpdateGameManager();
            this.StartDelayMethod(2,()=> LeanPool.DespawnAll());
            if (isGetToDestinationLevel())
            {
                this.StartDelayMethod(2,()=>RatingReviewManager.Instance.EnableRatingReviewPanel());
            }
            else
            {
                this.StartDelayMethod(2,()=>GUIManager.Instance.ActivatingWinningCanvas());
            }
            
        }
    }
    

    public void NextLevel()
    {
        if (GameManager.Instance.currentLevel != 1)
        {
            GUIManager.Instance.WatchVideo1();
        }
        DataController.LoadMap();
    }

    public void Replay()
    {
        SetDefault();
        GameManager.Instance.isAddBox = false;
//        GameManager.Instance.numberRewinds = 5;
        GUIManager.Instance.SetDefaultGUI();
        this.StartDelayMethod(0,()=> LeanPool.DespawnAll());
        this.StartDelayMethod(0,()=>DataController.LoadMap());
    }

    public void AddOneBox()
    {
        if (GameManager.Instance.currentLevel == 0) return;
        
        if (GameManager.Instance.isAddBox)
        {
            
        }
        else
        {
            GameManager.Instance.isAddBox = true;
            DataController.AddOneBox();
        }
    }
    
}
