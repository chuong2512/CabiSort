using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lean.Pool;
using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] private int id;

    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private List<GameObject> targets;
    [SerializeField] private Imposter _imposter;
    [SerializeField] private Stack<Imposter> imposterStack;

    [SerializeField] private Transform topBox;

    [SerializeField] private ParticleSystem ps;
    [SerializeField] public GameObject effect;
    [SerializeField] public Animation wrongEffect;


    private GamePlayController gpc;

    void Start()
    {
        gpc = GameController.Ins.gamePlayController;
    }
    
    public int Id
    {
        get => id;
        set => id = value;
    }
    
    
    public Transform TopBox
    {
        get => topBox;
        set => topBox = value;
    }

    public List<GameObject> Targets
    {
        get => targets;
        set => targets = value;
    }

    public Stack<Imposter> Imposters
    {
        get => imposterStack;
        set => imposterStack = value;
    }
    
    public void SetUp(Stack<int> stack)
    {
        imposterStack = new Stack<Imposter>();
        
        for (int i = 0; i < stack.Count; i++)
        {
            var imposter = LeanPool.Spawn(_imposter, targets[i].transform.position, Quaternion.identity);
            imposter.transform.position = Vector3.zero;
            
            imposter.gameObject.transform.SetParent(targets[i].transform,false);
            
            imposter.SetUp(stack.ElementAt(i),i==0 ? true : false);
            imposter.Toptarget = topBox.position;
            imposterStack.Push(imposter);
        }
    }
    
    

    private bool canPeeking()
    {
        return !gpc.IsPeeking;
    }

    private bool isEmptyBox()
    {
        return imposterStack.Count == 0;
    }

    public void TurnOnEffectWrong()
    {
        wrongEffect.Play();
    }

    public void TurnOnEffectRight()
    {
        effect.SetActive(true);
    }

    public void TurnOffEffectRight()
    {
        effect.SetActive(false);
    }
    
    private void chooseItem()
    {
        gpc.selectedImposter = imposterStack.Peek();
        gpc.currentBox = this;
        gpc.IsPeeking = true;

        gpc.selectedImposter._animation.AnimationState.SetAnimation(0, "Pick Acion_test", false);
        TurnOnEffectRight();

        GUIManager.Instance.PlayClickBoxSound();
    }

    public void moveItem(Box currentBox,Box targetBox, Imposter imposter)
    {
        gpc.selectedBox.Imposters.Push(gpc.selectedImposter);
        gpc.selectedImposter.MoveToTarget(0.25f,
            targetBox.topBox.transform.position,
            targetBox.targets[gpc.selectedBox.Imposters.Count-1].transform.position);
        gpc.selectedImposter.transform.SetParent(gpc.selectedBox.targets[gpc.selectedBox.Imposters.Count-1].transform);
        
        gpc.currentBox.Imposters.Pop();
        gpc.selectedImposter.Toptarget = gpc.selectedBox.topBox.position;
        
        GUIManager.Instance.PlayFallingSound();
    }
    
    
    public void OnMouseDown()
    {
        
        if (isDoneBox() || GUIManager.Instance.menuPanel.activeSelf || GUIManager.Instance.winningCanvasPanel.activeSelf)
        {
            return;
        }
        
        //tutorial level
        if (GameManager.Instance.currentLevel == 0)
        {
            if (Imposters.Count == 3 && GameController.Ins.gamePlayController.currentBox==null)
            {
                return;
            }
            else
            {
                if (GameController.Ins.gamePlayController.currentBox == null)
                {
                    GameController.Ins.SetHandToSelectedBox();
                }
                else
                {
                    GameController.Ins.SetHandToCurrentBox();
                }
            }
        }
        
        if (canPeeking())
        {
            if (!isEmptyBox())
            {
                chooseItem();
            }
            else
            {
                GUIManager.Instance.PlayWrongPick();
                TurnOnEffectWrong();
            }
        }
        else
        {
            gpc.currentBox.TurnOffEffectRight();
            gpc.selectedBox = this;
            if (gpc.isCancelHandle())
            {
                gpc.selectedImposter.BackIntoPosition(0f);
                GUIManager.Instance.PlayWrongPick();
                TurnOnEffectWrong();
                gpc.ResetPeeking();
            }
            else
            {
                moveItem(gpc.currentBox,
                    gpc.selectedBox,
                    gpc.selectedImposter);
                
                gpc.SaveLastMove();
                gpc.ResetPeeking();

            }
        }

        if (isDoneBox())
        {
            if (GameManager.Instance.currentLevel == 0)
            {
                Destroy(GameController.Ins.tutorialHand.transform.parent.gameObject);
                GameController.Ins.isTutorialLevel = false;
            }
            GUIManager.Instance.Vibrating();
            ps.Play();
            GUIManager.Instance.PlaySolveSound();
            gpc.Winning();
        }
    }

    public bool isDoneBox()
    {
        
        if (imposterStack.Count == gpc.maxValueCols)
        {
            int firstID = imposterStack.Peek().id;
            foreach (var imposter in imposterStack)
            {
                if (imposter.id != firstID) return false;
            }

            return true;
        }

        return false;
    }
    
}
