using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class GraphicController : MonoBehaviour
{
    public List<Box> Boxes;
    
    public SpriteRenderer background;
    public Sprite[] backgroundList;
    
    public Transform posRow1;
    public Transform posRow1_2;
    public Transform posRow2_2;

    private float timeResetAnimation = 5;

    void Update()
    {
        if (timeResetAnimation < 0)
        {
            Debug.Log("oke");
            RandomAnimation();
            timeResetAnimation = 5;
        }
        else
        {
            timeResetAnimation -= Time.deltaTime;
            
        }
    }

    public void Set1RowBackground()
    {
        background.sprite = backgroundList[0];
    }
    
    public void Set2RowBackground()
    {
        background.sprite = backgroundList[1];
    }
    
    public void AddBox(Box box)
    {
        Boxes.Add(box);
    }

    public void SetUp(List<Stack<int>> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Boxes[i].SetUp(list[i]);
        }
    }

    public void RandomAnimation()
    {
        foreach (Box box in Boxes)
        {
            foreach (Imposter imposter in box.Imposters)
            {
                if (UnityEngine.Random.Range(0, 100) < 10)
                    imposter._animation.AnimationState.SetAnimation(0, "Idle", false);
            }            
        }
    }
    
    public void SetDefaultAnimation()
    {
        foreach (Box box in Boxes)
        {
            box.effect.SetActive(false);
            box.wrongEffect.GetComponent<Animation>().Stop();
            foreach (Imposter imposter in box.Imposters)
            {
                imposter._animation.AnimationState.SetAnimation(0, "Fall", false);
            }            
        }
    }
}
