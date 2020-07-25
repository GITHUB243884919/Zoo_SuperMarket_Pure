using UFrame.Logger;
using System;
using System.Collections;
using System.Collections.Generic;
using UFrame;
using UnityEngine;

namespace Game.GlobalData
{
    [Serializable]
    public class GateData
    {
        /// <summary>
        /// 入口ID
        /// </summary>
        public int entryID;

        /// <summary>
        /// 入口等级
        /// </summary>
        public int level;
    }
    [Serializable]
    public class EntryGateData_MS
    {
        /// <summary>
        /// 所属场景
        /// </summary>
        public int sceneID = GameConst.First_SceneID;

        /// <summary>
        /// 售票口门票等级
        /// </summary>
        public int entryTicketsLevel = 1;

        /// <summary>
        /// 开启的入口，List形式存储每个入口的数据
        /// </summary>
        public List<GateData> entryGateList = new List<GateData>();
    }
}
