using UFrame.Logger;
using System;
using System.Collections;
using System.Collections.Generic;
using UFrame;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class EntryGateData_New
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
        public List<EntryGateData> entryGateList = new List<EntryGateData>();
    }
}
