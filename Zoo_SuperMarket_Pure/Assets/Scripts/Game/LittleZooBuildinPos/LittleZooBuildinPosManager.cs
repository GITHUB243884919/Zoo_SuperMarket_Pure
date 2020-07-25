/*******************************************************************
* FileName:     LittleZooBuildinPosManager.cs
* Author:       Fan Zheng Yong
* Date:         2019-8-17
* Description:  
* other:    
********************************************************************/


using UFrame.Logger;
using System.Collections;
using System.Collections.Generic;
using UFrame;
using UFrame.Common;
using UnityEngine;

namespace Game
{
    public partial class LittleZooBuildinPosManager : Singleton<LittleZooBuildinPosManager>, ISingleton
    {
        public Dictionary<int, LittleZooBuildinPos> littleZooBuildinPosMap = new Dictionary<int, LittleZooBuildinPos>();

        public void Init()
        {
            AddAll();
        }

        public void AddLittleZooBuildinPos(LittleZooBuildinPos littleZooBuildinPos)
        {
            if (littleZooBuildinPosMap.ContainsKey(littleZooBuildinPos.LittleZooID))
            {
                LogWarp.LogErrorFormat("动物栏内置点重复 {0}", littleZooBuildinPos.LittleZooID);
                return;
            }

            littleZooBuildinPosMap.Add(littleZooBuildinPos.LittleZooID, littleZooBuildinPos);
        }

        public LittleZooBuildinPos GetLittleZooBuildinPos(int littleZooID)
        {
            int maplittleZooID = LittleZooModule.MapToFirstSceneLittleZooID(littleZooID);
            LittleZooBuildinPos littleZooBuildinPos = null;

            littleZooBuildinPosMap.TryGetValue(maplittleZooID, out littleZooBuildinPos);

            return littleZooBuildinPos;
        }
    }

}

