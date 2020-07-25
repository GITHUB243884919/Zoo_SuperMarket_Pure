using System.Collections;
using System.Collections.Generic;
using UFrame;
using UnityEngine;

public class EntryGateSenceData 
{
    /// <summary>
    /// 售票口特效挂点
    /// </summary>
    public List<Vector3> entryGatesVector = new List<Vector3>();
    /// <summary>
    /// 售票口金币粒子特效列表
    /// </summary>
    public List<SimpleParticle> entryCoinSpList = new List<SimpleParticle>();
    /// <summary>
    /// 获取售票口下标的对应对象
    /// </summary>
    public List<Transform> EntrySubscriptGB = new List<Transform>();

    public void Release()
    {
        entryGatesVector.Clear();

        for(int i = 0; i < entryCoinSpList.Count; i++)
        {
            entryCoinSpList[i].Release();
        }
        entryCoinSpList.Clear();

        for(int i = 0; i < EntrySubscriptGB.Count; i++)
        {
            EntrySubscriptGB[i] = null;
        }
        EntrySubscriptGB.Clear();

    }
}
