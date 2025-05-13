using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Spine.Unity;

public class Imposter : MonoBehaviour
{
    [SerializeField] public SkeletonAnimation _animation;
    public int id;

    [SerializeField] private readonly string[] skinList =
        {"Yellow", "Blue", "Green", "Orange", "Pink", "Purple", "Red", "White", "Black", "Brown"};  

    [SerializeField] private Vector3 toptarget;

    public Vector3 Toptarget
    {
        get => toptarget;
        set => toptarget = value;
    }

    

    public void SetUp(int id,bool isStand)
    {
        this.id = id;
        _animation.Skeleton.SetSkin(skinList[id]);
    }

    private Sequence _sequence;

    public void MoveToTop(float startTime, float fixTime = .25f)
    {
        
        _sequence = DOTween.Sequence();

        _sequence.Append(transform.DOMove(toptarget, fixTime));
        
        _sequence.Restart();
    }

    public void BackIntoPosition(float startTime, float fixTime = .25f)
    {
        _sequence = DOTween.Sequence();

        _sequence.Append(transform.DOMove(this.gameObject.transform.parent.transform.position, fixTime));
        
        _sequence.Restart();
        _animation.AnimationState.SetAnimation(0, "Fall", false);
    }
    
    
    public void MoveToTarget(float startTime, Vector3 target1, Vector3 target2, float fixTime = .25f)
    {
        _sequence = DOTween.Sequence();

        _sequence.Append(transform.DOMove(toptarget, startTime)).AppendCallback(() => transform.position = target1)
            .AppendCallback(() => _animation.AnimationState.SetAnimation(0, "Fall", false))
            .Append(transform.DOMove(target2, fixTime))
            .AppendCallback(() => _animation.AnimationState.SetAnimation(0, "Idle", false));

        _sequence.Restart();
        
        
    }
 
}
