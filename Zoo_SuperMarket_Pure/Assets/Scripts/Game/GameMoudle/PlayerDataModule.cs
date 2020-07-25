/*******************************************************************
* FileName:     PlayerDataModule.cs
* Author:       Fan Zheng Yong
* Date:         2019-9-10
* Description:  
* other:    
********************************************************************/


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
        public PlayerDataModule(int orderID) : base(orderID) { }

        PlayerData playerData { get { return GlobalDataManager.GetInstance().playerData; } }

        ParkingCenterData_MS parkingCenterData;
        int currSceneID;
        List<int> trigerLoadLittleZooIDs;
        Config.parkingCell parkingCell;
        Config.ticketCell ticketCell;
        Config.buildupCell buildupCell;
        IntCD leaveSceneCD;
        int levelSceneCDVal;
        List<int> sortGateIDs;
        public override void Init()
        {
            currSceneID = playerData.playerZoo.currSceneID;
            sortGateIDs = GlobalDataManager.GetInstance().logicTableEntryGate.GetSortGateIDs(currSceneID);
            trigerLoadLittleZooIDs = new List<int>();
            parkingCell = GlobalDataManager.GetInstance().logicTableParkingData.GetParkingCell(currSceneID);
            ticketCell = GetTicketCell();
            InitLeaveSceneCD();

            MessageManager.GetInstance().Regist((int)GameMessageDefine.SetParkingProfitLevelOfPlayerData, this.OnSetParkingProfitLevelOfPlayerData);
            MessageManager.GetInstance().Regist((int)GameMessageDefine.SetParkingSpaceLevelOfPlayerData, this.OnSetParkingSpaceLevelOfPlayerData);
            MessageManager.GetInstance().Regist((int)GameMessageDefine.SetParkingEnterCarSpawnLevelOfPlayerData, this.OnSetParkingEnterCarSpawnLevelOfPlayerData);

            //MessageManager.GetInstance().Regist((int)GameMessageDefine.SetCoinOfPlayerData, this.OnSetCoinOfPlayerData);
            MessageManager.GetInstance().Regist((int)GameMessageDefine.AddCoinOfPlayerDataMSSC, this.OnSetCoinOfPlayerDataMSSC);
            MessageManager.GetInstance().Regist((int)GameMessageDefine.SetDiamondOfPlayerData, this.OnSetDiamondOfPlayerData);
            MessageManager.GetInstance().Regist((int)GameMessageDefine.SetStarOfPlayerData, this.OnSetStarOfPlayerData);

            MessageManager.GetInstance().Regist((int)GameMessageDefine.SetLittleZooTicketsLevelPlayerData, this.OnSetLittleZooTicketsLevelPlayerData);
            MessageManager.GetInstance().Regist((int)GameMessageDefine.SetLittleZooVisitorLocationLevelOfPlayerData, this.OnSetLittleZooVisitorLocationLevelOfPlayerData);
            MessageManager.GetInstance().Regist((int)GameMessageDefine.SetLittleZooEnterVisitorSpawnLevelOfPlayerData, this.OnSetLittleZooEnterVisitorSpawnLevelOfPlayerData);


            MessageManager.GetInstance().Regist((int)GameMessageDefine.OpenNewLittleZoo, this.OnOpenNewLittleZoo);
            MessageManager.GetInstance().Regist((int)GameMessageDefine.VisitorVisitCDFinshedReply, OnVisitorVisitCDFinshedReply);
            MessageManager.GetInstance().Regist((int)GameMessageDefine.EntryGateCheckInCDFinshedReply, OnEntryGateCheckGoToZoo);
            MessageManager.GetInstance().Regist((int)GameMessageDefine.SetAnimalLevel, this.OnSetAnimalLevelData);

            MessageManager.GetInstance().Regist((int)GameMessageDefine.SetEntryGateLevelOfPlayerData, this.OnSetEntryGateLevelOfPlayerData);
            MessageManager.GetInstance().Regist((int)GameMessageDefine.SetEntryGatePureLevelOfPlayerData, this.OnSetEntryGatePureLevelOfPlayerData);
            MessageManager.GetInstance().Regist((int)GameMessageDefine.SetEntryGateNumOfPlayerData, this.OnSetEntryGateNumOfPlayerData);


            MessageManager.GetInstance().Regist((int)GameMessageDefine.IncreaseCrossRoadStageID, this.OnIncreaseCrossRoadStageID);
        }

        protected void InitLeaveSceneCD()
        {
            levelSceneCDVal = Config.globalConfig.getInstace().LeaveSceneCoinCD * 1000;
            leaveSceneCD = new IntCD(levelSceneCDVal);
            leaveSceneCD.Run();
        }

        public override void Release()
        {
            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.SetParkingProfitLevelOfPlayerData, this.OnSetParkingProfitLevelOfPlayerData);
            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.SetParkingSpaceLevelOfPlayerData, this.OnSetParkingSpaceLevelOfPlayerData);
            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.SetParkingEnterCarSpawnLevelOfPlayerData, this.OnSetParkingEnterCarSpawnLevelOfPlayerData);


            //MessageManager.GetInstance().UnRegist((int)GameMessageDefine.SetCoinOfPlayerData, this.OnSetCoinOfPlayerData);
            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.AddCoinOfPlayerDataMSSC, this.OnSetCoinOfPlayerDataMSSC);
            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.SetDiamondOfPlayerData, this.OnSetDiamondOfPlayerData);
            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.SetStarOfPlayerData, this.OnSetStarOfPlayerData);

            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.SetLittleZooTicketsLevelPlayerData, this.OnSetLittleZooTicketsLevelPlayerData);
            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.SetLittleZooVisitorLocationLevelOfPlayerData, this.OnSetLittleZooVisitorLocationLevelOfPlayerData);
            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.SetLittleZooEnterVisitorSpawnLevelOfPlayerData, this.OnSetLittleZooEnterVisitorSpawnLevelOfPlayerData);

            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.OpenNewLittleZoo, this.OnOpenNewLittleZoo);
            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.VisitorVisitCDFinshedReply, OnVisitorVisitCDFinshedReply);
            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.EntryGateCheckInCDFinshedReply, OnEntryGateCheckGoToZoo);
            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.SetAnimalLevel, this.OnSetAnimalLevelData);
            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.SetEntryGateLevelOfPlayerData, this.OnSetEntryGateLevelOfPlayerData);
            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.SetEntryGatePureLevelOfPlayerData, this.OnSetEntryGatePureLevelOfPlayerData);
            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.SetEntryGateNumOfPlayerData, this.OnSetEntryGateNumOfPlayerData);

            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.IncreaseCrossRoadStageID, this.OnIncreaseCrossRoadStageID);

            trigerLoadLittleZooIDs.Clear();
            if (leaveSceneCD != null)
            {
                leaveSceneCD.Stop();
                leaveSceneCD = null;
            }
            Stop();
        }

        public override void Tick(int deltaTimeMS)
        {
            if (!CouldRun())
            {
                return;
            }
            //离开场景产生金币屏蔽
            //Tick_LeaveSceneCD(deltaTimeMS);
        }

        //protected  bool VaryDataCoin( BigInteger big )
        //{
        //    var left = BigInteger.Parse(GlobalDataManager.GetInstance().playerData.playerZoo.coin) - big;
        //    if (left < 0)
        //    {
        //        return false;
        //    }
        //    GlobalDataManager.GetInstance().playerData.playerZoo.coin = left.ToString();
        //    return true;
        //}
 
        //protected void OnSetCoinOfPlayerData(Message msg)
        //{
        //    var _msg = msg as SetValueOfPlayerData;
           
        //    var deltaVal = _msg.bigIntDeltaVal;
        //    var currVal = BigInteger.Parse(this.playerData.playerZoo.coin);
        //    currVal += deltaVal;
        //    this.playerData.playerZoo.coin = currVal.ToString();
        //    BroadcastValueOfPlayerData.Send((int)GameMessageDefine.BroadcastCoinOfPlayerData, 0, 0, BigInteger.Parse(GlobalDataManager.GetInstance().playerData.playerZoo.coin), _msg.bigIntDeltaVal);
        //}

        protected void OnSetCoinOfPlayerDataMSSC(Message msg)
        {
            var _msg = msg as SetValueOfPlayerData;
            playerData.playerZoo.playerCoin.WarpAddCoin(playerData, _msg.bigIntDeltaVal);
        }

        protected void OnSetDiamondOfPlayerData(Message msg)
        {
            var _msg = msg as SetValueOfPlayerData;
            if (this.playerData.playerZoo.diamond<100000000)
            {
                this.playerData.playerZoo.diamond += _msg.deltaVal;
            }
            BroadcastValueOfPlayerData.Send((int)GameMessageDefine.BroadcastDiamondOfPlayerData, this.playerData.playerZoo.diamond, _msg.deltaVal, 0, 0);
        }

        protected void OnSetStarOfPlayerData(Message msg)
        {
            var _msg = msg as SetValueOfPlayerData;
            this.playerData.playerZoo.star += _msg.deltaVal;
            BroadcastValueOfPlayerData.Send((int)GameMessageDefine.BroadcastStarOfPlayerData, this.playerData.playerZoo.star, _msg.deltaVal, 0, 0);
            
            //if (PlayerData.GetcurrSceneIDByStar(playerData) != playerData.playerZoo.currSceneID)
            //{

            //}
        }

        //protected void AddCoin(BigInteger addNum)
        //{
        //    BigInteger currCoin = BigInteger.Parse(this.playerData.playerZoo.coin);
        //    currCoin += addNum;
        //    this.playerData.playerZoo.coin = currCoin.ToString();
        //    BroadcastValueOfPlayerData.Send((int)GameMessageDefine.BroadcastCoinOfPlayerData,
        //        0, 0, currCoin, addNum);
        //}

        /// <summary>
        /// 动物栏CD收益
        /// </summary>
        /// <param name="msg"></param>
        protected void OnVisitorVisitCDFinshedReply(Message msg)
        {
#if NO_BIGINT
            PlaySceneMoneyMusic();
#else

            //只有最新解锁的场景动物栏和大门产钱
            if (playerData.playerZoo.currSceneID != playerData.playerZoo.lastUnLockSceneID)
            {
                return;
            }

            var _msg = msg as VisitorVisitCDFinshedReply;
            int littleZooEnterVisitorSpawnLevel = GlobalDataManager.GetInstance().playerData.GetLittleZooModuleData(_msg.littleZooID).littleZooTicketsLevel;
            BigInteger price = LittleZooModule.GetLittleZooPrice(_msg.littleZooID,littleZooEnterVisitorSpawnLevel);
            PlaySceneMoneyMusic();

            playerData.playerZoo.playerCoin.WarpAddCoin(playerData, price, false);
#endif
        }

        /// <summary>
        /// 售票口CD
        /// </summary>
        /// <param name="msg"></param>
        protected void OnEntryGateCheckGoToZoo(Message msg)
        {
#if NO_BIGINT
            PlaySceneMoneyMusic();
#else
            //只有最新解锁的场景动物栏和大门产钱
            if (playerData.playerZoo.currSceneID != playerData.playerZoo.lastUnLockSceneID)
            {
                return;
            }
            var sceneID = playerData.playerZoo.currSceneID;
            var price = EntryGateModule.GetEntryPrice(GlobalDataManager.GetInstance().playerData.GetEntryDateDataIDIndexOfDataIdx().entryTicketsLevel,sceneID,true);
            PlaySceneMoneyMusic();


            playerData.playerZoo.playerCoin.WarpAddCoin(playerData, price, false);
#endif
        }
        
        /// <summary>
        /// 播放场景音乐
        /// </summary>
        protected void PlaySceneMoneyMusic()
        {
            string btnSoundPath = Config.globalConfig.getInstace().SceneMoneyMusic;
            UFrame.MiniGame.SoundManager.GetInstance().PlaySound(btnSoundPath);
        }

    }
}

