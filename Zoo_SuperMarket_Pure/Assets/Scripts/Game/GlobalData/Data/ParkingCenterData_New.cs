using UFrame.Logger;
using System;
using System.Collections;
using System.Collections.Generic;
using UFrame;
using UnityEngine;

namespace Game.GlobalData
{
    [Serializable]
    public class ParkingCenterData_MS
    {
        /// <summary>
        /// 所属场景
        /// </summary>
        public int sceneID = GameConst.First_SceneID;

        /// <summary>
        /// 利润等级
        /// </summary>
        public int parkingProfitLevel;

        /// <summary>
        /// 停车位数量等级
        /// </summary>
        public int parkingSpaceLevel;

        /// <summary>
        /// 流量等级
        /// </summary>
        public int parkingEnterCarSpawnLevel;

        public ParkingCenterData_MS()
        {
            parkingSpaceLevel = 1;
            parkingProfitLevel = 1;
            parkingEnterCarSpawnLevel = 1;
            sceneID = GameConst.First_SceneID;
        }
        public void ClearData()
        {
            parkingSpaceLevel = -1;
            parkingProfitLevel = -1;
            parkingEnterCarSpawnLevel = -1;
            sceneID = GameConst.First_SceneID;
        }
    }
}