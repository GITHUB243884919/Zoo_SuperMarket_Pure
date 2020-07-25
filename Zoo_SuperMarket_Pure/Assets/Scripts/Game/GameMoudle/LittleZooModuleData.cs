using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{
    /// <summary>
    /// 动物栏数据 -已弃用
    /// </summary>
    [Serializable]
    public class LittleZooModuleData
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

    /// <summary>
    /// 多场景多布局动物栏
    /// </summary>
    [Serializable]
    public class LittleZooModuleDataMSS
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
