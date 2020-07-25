using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
namespace Game
{
    /// <summary>
    /// 所有离开场景的金币数
    /// </summary>
    public class LeaveSceneCoinData
    {
        /// <summary>
        /// 分场景累计离开金币
        /// </summary>
        public Dictionary<int, BigInteger> LeaveSceneCoinDict = new Dictionary<int, BigInteger>();

        public void AddCoin(int sceneID, BigInteger addCoin)
        {
            BigInteger currCoin;
            if (!LeaveSceneCoinDict.TryGetValue(sceneID, out currCoin))
            {
                LeaveSceneCoinDict.Add(sceneID, addCoin);
                return;
            }

            LeaveSceneCoinDict[sceneID] += addCoin;
        }
    }

}
