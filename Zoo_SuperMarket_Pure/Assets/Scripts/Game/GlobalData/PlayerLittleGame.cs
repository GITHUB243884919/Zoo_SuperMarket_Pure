using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UFrame;

namespace Game.GlobalData
{
    /// <summary>
    /// 过马路用户数据
    /// </summary>
    [Serializable]
    public class PlayerLittleGame
    {
        /// <summary>
        /// 是否首次进入
        /// </summary>
        public bool isFirst =true ;

        /// <summary>
        /// 通过关卡id
        /// </summary>
        public int stageID;

        /// <summary>
        /// 体力值
        /// </summary>
        public int strength = Config.globalConfig.getInstace().MaxStrength;

        /// <summary>
        /// 恢复体力的ticks
        /// </summary>
        public long increaseStrengthTicks;

        /// <summary>
        /// 倒计时间
        /// </summary>
        public long countDownTicks;

        /// <summary>
        /// 正在恢复的体力时间
        /// </summary>
        public IntCD intCD ;
    }
}
