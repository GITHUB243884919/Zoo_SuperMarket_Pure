using Game.GlobalData;
using Game.MessageCenter;
using System.Collections;
using System.Collections.Generic;
using UFrame;
using UFrame.MessageCenter;
using UnityEngine;
using System.Numerics;
using UFrame.Logger;
using System;

namespace Game
{
    public partial class PlayerDataModule : GameModule
    {
        /// <summary>
        /// 停车场的等级升级（利润）
        /// </summary>
        /// <param name="msg"></param>
        protected void OnSetParkingProfitLevelOfPlayerData(Message msg)
        {
            var _msg = msg as SetDetailValueOfPlayerData;
            parkingCenterData = playerData.GetParkingCenterDataIDIndexOfDataIdx(currSceneID);
            int parkingProfitLevel = parkingCenterData.parkingProfitLevel;
            if ((parkingProfitLevel+ _msg.detailVal) >parkingCell.lvmax)
            {
                return;
            }
            BigInteger bigDelta = ParkingCenter.GetUpGradeParkingProfitConsumption(parkingProfitLevel,_msg.detailVal);

            if (!playerData.playerZoo.playerCoin.WarpAddCoin(playerData, -bigDelta))
            {
                return;
            }

            int needLevel = parkingProfitLevel + _msg.detailVal;
            this.playerData.GetParkingCenterDataIDIndexOfDataIdx(currSceneID).parkingProfitLevel = needLevel;
            BroadcastValueOfPlayerData.Send((int)GameMessageDefine.BroadcastParkingProfitLevelOfPlayerData,
                this.playerData.GetParkingCenterDataIDIndexOfDataIdx(currSceneID).parkingProfitLevel, _msg.detailVal, 0, 0);
            BuildingLevelup.Send((int)BuildingTypeFM.Parking, 999, (int)ParkingProperty.Revenue, this.playerData.GetParkingCenterDataIDIndexOfDataIdx(currSceneID).parkingProfitLevel);

            //收集星星：
            var lvshage = parkingCell.lvshage;
            int idx = FindLevelRangIndex01(lvshage, this.playerData.GetParkingCenterDataIDIndexOfDataIdx(currSceneID).parkingProfitLevel);
            int stageLevel = parkingCell.lvshage[idx];
            if (this.playerData.GetParkingCenterDataIDIndexOfDataIdx(currSceneID).parkingProfitLevel == stageLevel)
            {
                int awardType = parkingCell.lvrewardtype[idx];
                int awardID = parkingCell.lvreward[idx];
                var cell = Config.itemConfig.getInstace().getCell(awardID);
                if (awardType == 1)
                {
                    //发放奖励道具
                    MessageInt.Send((int)GameMessageDefine.GetItem, awardID);
                    if (cell.itemtype == 2)
                    {
                        PageMgr.GetPage<UIMainPage>().OnMoneyEffect();
                    }
                }
                //发放星星
                MessageInt.Send((int)GameMessageDefine.GetItem, 4);

            }

        }

        /// <summary>
        /// 停车场的停车位数量升级
        /// </summary>
        /// <param name="obj"></param>
        private void OnSetParkingSpaceLevelOfPlayerData(Message obj)
        {
            var _msg = obj as SetValueOfPlayerData;
            parkingCenterData = playerData.GetParkingCenterDataIDIndexOfDataIdx();
            int parkingSpaceLevel = parkingCenterData.parkingSpaceLevel;
            if (parkingSpaceLevel >= parkingCell.touristmaxlv)
            {
                return;
            }
            BigInteger bigDelta = (ParkingCenter.GetUpGradeNumberConsumption(parkingSpaceLevel));
            //bool retCode = VaryDataCoin(bigDelta);
            //if (!retCode)
            //{
            //    //string e = string.Format("停车场停车位数量升级失败");
            //    //throw new System.Exception(e);
            //    return;
            //}

            //BroadcastValueOfPlayerData.Send((int)GameMessageDefine.BroadcastCoinOfPlayerData,
            //0, 0, BigInteger.Parse(GlobalDataManager.GetInstance().playerData.playerZoo.coin), bigDelta);

            if (!playerData.playerZoo.playerCoin.WarpAddCoin(playerData, -bigDelta))
            {
                return;
            }

            this.playerData.GetParkingCenterDataIDIndexOfDataIdx(currSceneID).parkingSpaceLevel += 1;
            BroadcastValueOfPlayerData.Send((int)GameMessageDefine.BroadcastParkingSpaceLevelOfPlayerData,
                this.playerData.GetParkingCenterDataIDIndexOfDataIdx(currSceneID).parkingSpaceLevel, _msg.deltaVal, 0, 0);
            BuildingLevelup.Send((int)BuildingTypeFM.Parking, 999, (int)ParkingProperty.Capacity, this.playerData.GetParkingCenterDataIDIndexOfDataIdx(currSceneID).parkingSpaceLevel);
        }

        /// <summary>
        /// 停车场的来客流量升级
        /// </summary>
        /// <param name="obj"></param>
        private void OnSetParkingEnterCarSpawnLevelOfPlayerData(Message obj)
        {
            var _msg = obj as SetValueOfPlayerData;
            parkingCenterData = playerData.GetParkingCenterDataIDIndexOfDataIdx();
            
            int parkingEnterCarSpawnLevel = parkingCenterData.parkingEnterCarSpawnLevel;
            BigInteger bigDelta = ParkingCenter.GetUpGradeEnterCarSpawnConsumption(parkingEnterCarSpawnLevel);
            if (parkingEnterCarSpawnLevel >= parkingCell.touristmaxlv)
            {
                return;
            }
            //bool retCode = VaryDataCoin(bigDelta);
            //if (!retCode)
            //{
            //    //string e = string.Format("停车场的来客流量升级");
            //    //throw new System.Exception(e);
            //    return;
            //}

            //BroadcastValueOfPlayerData.Send((int)GameMessageDefine.BroadcastCoinOfPlayerData,
            //0, 0, BigInteger.Parse(GlobalDataManager.GetInstance().playerData.playerZoo.coin), bigDelta);

            if (!playerData.playerZoo.playerCoin.WarpAddCoin(playerData, -bigDelta))
            {
                return;
            }

            this.playerData.GetParkingCenterDataIDIndexOfDataIdx(currSceneID).parkingEnterCarSpawnLevel += 1;
            BroadcastValueOfPlayerData.Send((int)GameMessageDefine.BroadcastParkingEnterCarSpawnLevelOfPlayerData,
                this.playerData.GetParkingCenterDataIDIndexOfDataIdx(currSceneID).parkingEnterCarSpawnLevel, _msg.deltaVal, 0, 0);
            BuildingLevelup.Send((int)BuildingTypeFM.Parking, 999, (int)ParkingProperty.VisitorFlowSpeed, this.playerData.GetParkingCenterDataIDIndexOfDataIdx(currSceneID).parkingEnterCarSpawnLevel);
        }

        
    }
}
