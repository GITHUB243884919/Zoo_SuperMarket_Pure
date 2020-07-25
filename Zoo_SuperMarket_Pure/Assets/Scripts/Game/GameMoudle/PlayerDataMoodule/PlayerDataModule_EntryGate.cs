using Game.GlobalData;
using Game.MessageCenter;
using System.Collections;
using System.Collections.Generic;
using UFrame;
using UFrame.MessageCenter;
using UnityEngine;
using System.Numerics;
using UFrame.Logger;

namespace Game
{
    public partial class PlayerDataModule : GameModule
    {
        /// <summary>
        /// 收到设置售票口的门票等级升级的消息
        /// </summary>
        /// <param name="msg"></param>
        protected void OnSetEntryGateLevelOfPlayerData(Message msg)
        {
            var _msg = msg as SetDetailValueOfPlayerData;
            int entryTicketsLevel = GlobalDataManager.GetInstance().playerData.GetEntryDateDataIDIndexOfDataIdx().entryTicketsLevel;
            if ((entryTicketsLevel+_msg.detailVal) > ticketCell.lvmax)
            {
                return;
            }
            // 涉及金币减扣
            BigInteger bigDelta = EntryGateModule.GetUpGradeConsumption(entryTicketsLevel, _msg.detailVal);
            //bool retCode = VaryDataCoin(bigDelta);
            //if (!retCode)
            //{
            //    //string e = string.Format("售票口门票升级扣钱失败");
            //    //throw new System.Exception(e);
            //    return;
            //}

            //BroadcastValueOfPlayerData.Send((int)GameMessageDefine.BroadcastCoinOfPlayerData,
            //   0, 0, BigInteger.Parse(GlobalDataManager.GetInstance().playerData.playerZoo.coin), bigDelta);

            if (!playerData.playerZoo.playerCoin.WarpAddCoin(playerData, -bigDelta))
            {
                return;
            }

            this.playerData.GetEntryDateDataIDIndexOfDataIdx().entryTicketsLevel += _msg.detailVal;
            BroadcastValueOfPlayerData.Send((int)GameMessageDefine.BroadcastEntryGateLevelOfPlayerData, this.playerData.GetEntryDateDataIDIndexOfDataIdx().entryTicketsLevel, _msg.deltaVal, 0, 0);
            BuildingLevelup.Send((int)BuildingTypeFM.EntryGate, -1, (int)EntryGateProperty.TicketPrice, this.playerData.GetEntryDateDataIDIndexOfDataIdx().entryTicketsLevel);

            //收集星星：
            var sortEntryGateIDs = GlobalDataManager.GetInstance().logicTableEntryGate.GetSortGateIDs(currSceneID);
            var lvshage = Config.ticketConfig.getInstace().getCell(sortEntryGateIDs[0]).lvshage;
            int idx = FindLevelRangIndex01(lvshage, this.playerData.GetEntryDateDataIDIndexOfDataIdx().entryTicketsLevel);
            int stageLevel = Config.ticketConfig.getInstace().getCell(sortEntryGateIDs[0]).lvshage[idx];
            if (this.playerData.GetEntryDateDataIDIndexOfDataIdx().entryTicketsLevel == stageLevel)
            {
                int awardType = Config.ticketConfig.getInstace().getCell(sortEntryGateIDs[0]).lvrewardtype[idx];
                int awardID = Config.ticketConfig.getInstace().getCell(sortEntryGateIDs[0]).lvreward[idx];
                var cell = Config.itemConfig.getInstace().getCell(awardID);
                if (awardType == 1 )
                {
                    //发放奖励道具
                    MessageInt.Send((int)GameMessageDefine.GetItem, awardID);
                    if (cell.itemtype == 2)
                    {
                        PageMgr.GetPage<UIMainPage>().OnMoneyEffect();
                    }
                    //LogWarp.LogErrorFormat("售票口 当前等级为{0}，可以发放奖励道具{1}", stageLevel, awardID);
                }
                //发放星星
                MessageInt.Send((int)GameMessageDefine.GetItem, 4);
                //LogWarp.LogErrorFormat("售票口 当前等级为{0}，可以发放星星", stageLevel);

            }
        }

        /// <summary>
        /// 收到设置售票口的ID对应等级升级的消息
        /// </summary>
        /// <param name="msg"></param>
        protected void OnSetEntryGatePureLevelOfPlayerData(Message msg)
        {
            var _msg = msg as SetDetailValueOfPlayerData;
            //var sortGateIDs = GlobalDataManager.GetInstance().playerData.playerZoo.GetSortTicketConfigGateIDs();

            //int sceneID = GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID;

            int entryID = sortGateIDs[_msg.detailVal];
            var entryGate = GlobalDataManager.GetInstance().playerData.GetEntryGateIDIndexOfDataIdx(entryID);
            if (entryGate.level >= ticketCell.speedmaxlv)
            {
                return;
            }
            //升级扣钱
            BigInteger bigDelta = EntryGateModule.GetUpGradeCheckinSpeedConsumption(entryID, entryGate.level);

            //bool retCode = VaryDataCoin(bigDelta);
            //if (!retCode)
            //{
            //    //string e = string.Format("售票口扣钱失败");
            //    //throw new System.Exception(e);
            //    return;
            //}
            if (!playerData.playerZoo.playerCoin.WarpAddCoin(playerData, -bigDelta))
            {
                return;
            }

            int deltaLevel = _msg.deltaVal;
            entryGate.level += deltaLevel;

            ////广播金钱变化
            //BroadcastValueOfPlayerData.Send((int)GameMessageDefine.BroadcastCoinOfPlayerData,
            //0, 0, BigInteger.Parse(GlobalDataManager.GetInstance().playerData.playerZoo.coin), bigDelta);
            //LogWarp.LogError("测试：    升级单售票口   "+ entryID);
            //广播某入口升级
            SetDetailValueOfPlayerData.Send((int)GameMessageDefine.BroadcastEntryGatePureLevelOfPlayerData,
                entryID, deltaLevel, 0);

            BuildingLevelup.Send((int)BuildingTypeFM.EntryGate, entryID, (int)EntryGateProperty.Entrance, entryGate.level);
        }

        /// <summary>
        /// 设置入口数量
        /// </summary>
        /// <param name="msg"></param>
        protected void OnSetEntryGateNumOfPlayerData(Message msg)
        {
            var _msg = msg as SetValueOfPlayerData;

            //var sortGateIDs = GlobalDataManager.GetInstance().playerData.playerZoo.GetSortTicketConfigGateIDs();
            //int sceneID = GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID;
            var entryGateList = playerData.GetEntryDateDataIDIndexOfDataIdx().entryGateList;
            int number = entryGateList.Count - 1;
            int currLastEntryID = entryGateList[number].entryID;
            int idx = sortGateIDs.IndexOf(currLastEntryID);
#if UNITY_EDITOR
            if (idx < 0)
            {
                //string e = string.Format("设置入口开启异常! 原来{0} 增加 {1}", playerData.playerZoo.numEntryGate, _msg.deltaVal);
                //throw new System.Exception(e);
            }
#endif
            //已经开完了。
            if (idx == sortGateIDs.Count - 1)
            {
                return;
            }
            int entryID = 0;
            for (int i = idx + 1; i < idx + 1 + _msg.deltaVal; i++)
            {
                int subscript = this.playerData.GetSceneIDGoToEntryDataListSubscript();
                var entryGateData = new GateData
                {
                    entryID = sortGateIDs[i],
                    level = 1
                };
                entryID = sortGateIDs[i];

                this.playerData.playerZoo.entryGateList_MS[subscript].entryGateList.Add(entryGateData);
            }

            this.playerData.playerZoo.numEntryGate += _msg.deltaVal;


            //开启扣钱
            var parce = Config.ticketConfig.getInstace().getCell(idx).number;
            BigInteger bigDelta = BigInteger.Parse(parce);
            //bool retCode = VaryDataCoin(bigDelta);
            //if (!retCode)
            //{
            //    string e = string.Format("售票口开启扣钱失败");
            //    throw new System.Exception(e);
            //    return;
            //}
            ////广播金钱变化
            //BroadcastValueOfPlayerData.Send((int)GameMessageDefine.BroadcastCoinOfPlayerData,
            //0, 0, BigInteger.Parse(GlobalDataManager.GetInstance().playerData.playerZoo.coin), bigDelta);

            if (!playerData.playerZoo.playerCoin.WarpAddCoin(playerData, -bigDelta))
            {
                return;
            }

            //广播新开启了几个入口
            SetValueOfPlayerData.Send((int)GameMessageDefine.BroadcastEntryGateNumOfPlayerData, entryID, 0, _msg.channelID);
        }

        /// <summary>
        /// 获取对应场景的本地售票口数据
        /// </summary>
        /// <returns></returns>
        private Config.ticketCell GetTicketCell()
        {
            Config.ticketCell ticketCell = null;
            ticketCell = Config.ticketConfig.getInstace().getCell(sortGateIDs[0]);
            return ticketCell;
        }
    }
}