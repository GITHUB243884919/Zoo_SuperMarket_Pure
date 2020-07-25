using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// 设置按钮呼吸特效，挂载到按钮上
/// </summary>
public class BtnEffect : MonoBehaviour{
    void Start()
    {
        //在自身的大小上加上0.2倍   
        Vector3 effectScale = transform.localScale + new Vector3(0.07f, 0.07f, 0.07f);
        //设置动画      
        Tweener tweener = transform.DOScale(effectScale, 0.8f);
        //设置动画loop属性    
        tweener.SetLoops(-1, LoopType.Yoyo);
        tweener.Play();
    }
}
