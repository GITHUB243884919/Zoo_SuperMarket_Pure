using Game;
using Game.GlobalData;
using UFrame.Logger;
using System.Collections;
using System.Collections.Generic;
using Game.MessageCenter;
using UFrame.MessageCenter;
using UnityEngine;
using UFrame;

namespace Game
{
    public partial class PlayerDataModule : GameModule
    {
        /// <summary>
        /// 给定一个等级范围数组，等级，返回等级所在范围数组的索引
        /// </summary>
        /// <param name="levels"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static int FindLevelRangIndex(int[] levels, int level)
        {
            int idx = Const.Invalid_Int;
            if (level == 0)
            {
                return levels[0];
            }

            for (int j = 1; j < levels.Length; j++)
            {
                if (level <= levels[j] - 1)
                {
                    idx = j;
                    break;
                }
            }
            if (idx == Const.Invalid_Int)    //若idx超过数组范围则等于最大返回结果
            {
                idx = levels.Length - 1;
            }
            return idx;
        }


        /// <summary>
        /// 仅限Ui等级显示和奖励发放获取下标的情况使用
        /// 给定一个等级范围数组，等级，返回等级所在数组的上限下标
        /// </summary>
        /// <param name="levels"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static int FindLevelRangIndex01(int[] levels, int level)
        {
            int idx = Const.Invalid_Int;
            if (level == 0)
            {
                return levels[0];
            }

            for (int j = 1; j < levels.Length; j++)
            {
                if (level < levels[j])
                {
                    idx = j-1;
                    break;
                }
            }
            if (idx == Const.Invalid_Int)    //若idx超过数组范围则等于最大返回结果
            {
                idx = levels.Length - 1;
            }
            return idx;
        }

    }
}
