using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using System;
using Game.GlobalData;
using UFrame;
using Game.MessageCenter;

namespace Game
{
    [Serializable]
    public class PlayerCoin
    {
        public List<MultiCoin> multiCoinList = new List<MultiCoin>();

        public void SetDefault()
        {
            multiCoinList.Clear();
            multiCoinList.Add(new MultiCoin());
            var bigDefault = BigInteger.Parse(Config.sceneConfig.getInstace().getCell(GameConst.First_SceneID).scenelnitialgoldnum);
            AddCoinByScene(GameConst.First_SceneID, bigDefault);
        }

        public void StrToBigint()
        {
            for(int i = 0; i < multiCoinList.Count; i++)
            {
                multiCoinList[i].StrToBigint();
            }
        }

        public void Scale(int n)
        {
            if (n < 0 || n == 1)
            {
                return;
            }

            for(int i = 0; i < multiCoinList.Count; i++)
            {
                var multiCoin = multiCoinList[i];
                multiCoin.coinBigInt *= n;
                multiCoin.coinStr = multiCoin.coinBigInt.ToString();
                multiCoin.coinShow = MinerBigInt.ToDisplay(multiCoin.coinStr);
            }
        }

        public void SingleCoinToMultiCoinData(PlayerData playerData)
        {
            if (!string.IsNullOrEmpty(playerData.playerZoo.coin) && playerData.playerZoo.coin != "0")
            {
                multiCoinList.Clear();
                var multiCoin = new MultiCoin();
                multiCoin.coinStr = playerData.playerZoo.coin;
                multiCoin.coinShow = MinerBigInt.ToDisplay(playerData.playerZoo.coin);
                multiCoin.coinBigInt = BigInteger.Parse(playerData.playerZoo.coin);
                multiCoinList.Add(multiCoin);
                playerData.playerZoo.coin = "0";
            }
        }

        public MultiCoin GetCoinByType(int coinType)
        {
            MultiCoin result = null;

            for(int i = 0; i < multiCoinList.Count; i++)
            {
                var ret = multiCoinList[i];
                if (ret.coinType == coinType)
                {
                    return ret;
                }
            }

            return result;
        }

        public void AddCoinByType(int coinType, BigInteger coin)
        {
            MultiCoin ret = GetCoinByType(coinType);

            if (ret == null)
            {
                ret = new MultiCoin();
                multiCoinList.Add(ret);
            }

            ret.coinType = coinType;
            ret.coinBigInt += coin;
            ret.coinStr = ret.coinBigInt.ToString();
            ret.coinShow = MinerBigInt.ToDisplay(ret.coinStr);
        }

        public MultiCoin GetCoinByScene(int sceneID)
        {
            var cell = Config.sceneConfig.getInstace().getCell(sceneID);
            int coinType = cell.moneyid;
            return GetCoinByType(coinType);
        }

        public void AddCoinByScene(int sceneID, BigInteger coin)
        {
            var cell = Config.sceneConfig.getInstace().getCell(sceneID);
            int coinType = cell.moneyid;
            AddCoinByType(coinType, coin);
        }

        public void AddCoinBySceneOnce(int sceneID, BigInteger coin)
        {
            var cell = Config.sceneConfig.getInstace().getCell(sceneID);
            int coinType = cell.moneyid;
            if (null == GetCoinByType(coinType))
            {
                AddCoinByType(coinType, coin);
            }
        }

        public bool CheckCoin(int sceneID, BigInteger deltaCoin)
        {
            var cell = Config.sceneConfig.getInstace().getCell(sceneID);
            var multiCoin = GetCoinByType(cell.moneyid);
            return (multiCoin.coinBigInt + deltaCoin) >= 0;
        }

        public bool WarpAddCoin(PlayerData playerData, BigInteger bigDelta, bool shouldCheck = true)
        {
            //检查钱
            if (shouldCheck)
            {
                if (!playerData.playerZoo.playerCoin.CheckCoin(playerData.playerZoo.currSceneID, bigDelta))
                {
                    return false;
                }
            }

            //扣钱
            playerData.playerZoo.playerCoin.AddCoinByScene(playerData.playerZoo.currSceneID, bigDelta);
            //发送钱修改通知
            MessageManager.GetInstance().Send((int)GameMessageDefine.BroadcastCoinOfPlayerDataMSSC);
            return true;
        }
    }

    [Serializable]
    public class MultiCoin
    {
        /// <summary>
        /// 金币类型
        /// </summary>
        public int coinType = GameConst.First_CoinTypeID;

        /// <summary>
        /// 用于显示的值
        /// </summary>
        public string coinShow = "0";

        /// <summary>
        ///  字符串存储的值
        /// </summary>
        public string coinStr = "0";

        /// <summary>
        /// BigInteger存储的值
        /// </summary>
        public BigInteger coinBigInt = 0;


        public void StrToBigint()
        {
            coinBigInt = BigInteger.Parse(coinStr);
        }
    }
}
