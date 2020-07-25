using UFrame.Logger;
using System;
using System.Collections;
using System.Collections.Generic;
using UFrame;
using UnityEngine;

namespace Game.GlobalData
{
    [Serializable]
    public class LittleZooModuleData_MS 
    {
        /// <summary>
        /// 所属场景
        /// </summary>
        public int sceneID = GameConst.First_SceneID;

        /// <summary>
        /// 动物栏id
        /// </summary>
        public int littleZooID = 0;

        /// <summary>
        /// 门票等级
        /// </summary>
        public int littleZooTicketsLevel = 1;

        /// <summary>
        /// 观光位数量等级
        /// </summary>
        public int littleZooVisitorSeatLevel = 1;

        /// <summary>
        /// 观光游客的流量等级
        /// </summary>
        public int littleZooEnterVisitorSpawnLevel = 1;

    }
}
