/*******************************************************************
* FileName:     StateLoadAnimalInLittleZooForGame.cs
* Author:       Fan Zheng Yong
* Date:         2019-12-18
* Description:  
* other:    
********************************************************************/


using Game.Path.StraightLine;
using UFrame;
using UFrame.MessageCenter;
using UFrame.Logger;
using Game.MessageCenter;
using System;
using UnityEngine;
using System.Collections.Generic;
using UFrame.MiniGame;
using Game.GlobalData;

namespace Game
{
    /// <summary>
    /// 加载动物栏的动物
    /// </summary>
    public class StateLoadAnimalInLittleZooForGame : FSMState
    {
        PlayerData playerData { get { return GlobalDataManager.GetInstance().playerData; } }

        bool isSendFinished = false;
        bool isClosed = false;

        IntCD waitCD;

        int sceneID;

        public StateLoadAnimalInLittleZooForGame(int stateName, FSMMachine fsmCtr) :
            base(stateName, fsmCtr)
        {
        }

        public override void Enter(int preStateName)
        {
            base.Enter(preStateName);

            isSendFinished = false;
            isClosed = false;

            //playerData = GlobalDataManager.GetInstance().playerData;

            sceneID = (fsmCtr as FSMGameLoad).sceneID;

            waitCD = new IntCD(100);
            waitCD.Stop();

            LoadAnimalInLittleZoo();
            ////SendLoadFinised();
            //if (playerData.playerLittleGame.stageID > 0)
            //{
            //    SendLoadFinised();
            //    return;
            //}
            EnterCrossRoad();
        }

        /// <summary>
        /// 加载动物栏的小动物
        /// MIN(1+INT(lv/100),10) 然后取buildup表中的animalid数组中的元素
        /// </summary>
        protected void LoadAnimalInLittleZoo()
        {
            int firstLittleZooID = GlobalDataManager.GetInstance().logicTableGroup.GetFirstLittleZooID(sceneID);//默认，代码准确赋值
            if (firstLittleZooID == Const.Invalid_Int)
            {
                firstLittleZooID = 1001;
            }
            Config.buildupCell cellBuildUp;
            Config.animalupCell cellAnimalUp;
            int animalID;
            //var coin = playerData.playerZoo.playerCoin.GetCoinByScene(playerData.playerZoo.currSceneID);
            LittleZooModule.playerAnimalGoToResourceID.Clear();
            var animalMSS15 = playerData.playerZoo.animalMSS15;

            for (int i = 0; i < this.playerData.playerZoo.littleZooModuleDatasMSS.Count; i++)
            {
                //若是非本场景跳出
                if (playerData.playerZoo.littleZooModuleDatasMSS[i].sceneID != sceneID)
                    continue;
                //若是本场景第一个动物栏，
                int littleZooID = playerData.playerZoo.littleZooModuleDatasMSS[i].littleZooID;
                if (littleZooID==1001)
                {   //AppsFlyer打点：首次加载游戏
                }
                //看第一个动物数据是否有类型的数据，若有继续，若无添加
                if (littleZooID == firstLittleZooID)
                {
                    cellBuildUp = Config.buildupConfig.getInstace().getCell(littleZooID);
                    animalID = cellBuildUp.animalid[0];

                    bool isShow = LittleZooModule.GetExamineAnimalShowToLittleZooScene(animalID);
                    if (isShow == false)
                    {
                        animalMSS15.AddAnimal(animalID, true);
                    }
                }
                //动物栏的门票等级是否大于0
                if (playerData.playerZoo.littleZooModuleDatasMSS[i].littleZooTicketsLevel > 0)
                {
                    //利用animalID去查询是否有小类型等级
                    cellBuildUp = Config.buildupConfig.getInstace().getCell(littleZooID);
                    for (int j = 0; j < cellBuildUp.animalid.Length; j++)
                    {
                        animalID = cellBuildUp.animalid[j];
                        bool isShow = LittleZooModule.GetExamineAnimalShowToLittleZooScene(animalID);
                        if (isShow == true)
                        {
                            cellAnimalUp = Config.animalupConfig.getInstace().getCell(animalID);
                            LittleZooModule.LoadAnimal(littleZooID, animalID,
                            cellAnimalUp.moveradius, cellBuildUp.animalwanderoffset, sceneID);
                        }
                    }
                }
            }

            (PageMgr.allPages["UILoading"] as UILoading).SliderValueLoading(1f);
            waitCD.Run();
        }

        public static void EnterCrossRoad()
        {
            ZooGameLoader.GetInstance().UnLoad();
            int testStageID = CrossRoadGame.CrossRoadStageManager.GetInstance().TeststageID;
            if (testStageID != Const.Invalid_Int)
            {
                CrossRoadGame.CrossRoadStageManager.GetInstance().Load(testStageID);
            }
            else
            {
                CrossRoadGame.CrossRoadStageManager.GetInstance().Load(GlobalDataManager.GetInstance().playerData.playerLittleGame.stageID + 1);
            }
        }

        protected void SendLoadFinised()
        {
#if UNITY_EDITOR
            GameObject.Find("LittlezooBuildinPos").transform.GetChild(0).gameObject.SetActive(true);
#endif

            var playerData = GlobalDataManager.GetInstance().playerData;

            //存储当前场景ID
            playerData.playerZoo.currSceneID = sceneID;
            //记录场景进入次数
            var sceneStates = playerData.playerZoo.scenePlayerDataMSS.sceneStates;
            for(int i = 0; i < sceneStates.Count; i++)
            {
                var sceneState = sceneStates[i];
                if (sceneState.sceneId == sceneID)
                {
                    sceneState.enterCount++;
                    if (sceneState.enterCount==1 && sceneState.sceneId!= GameConst.First_SceneID)
                    {
                    }
                    break;
                }
            }

            playerData.playerZoo.playerCoin.AddCoinByScene(sceneID, 0);

            //设置最后解锁的场景
            if (sceneID > playerData.playerZoo.lastUnLockSceneID)
            {
                playerData.playerZoo.lastUnLockSceneID = sceneID;
                //走到这一步，一定是解锁新场景触发
                //同金币类型的场景金币设置成默认
                var multiCoin = playerData.playerZoo.playerCoin.GetCoinByScene(sceneID);
                var bigDefault = System.Numerics.BigInteger.Parse(
                    Config.sceneConfig.getInstace().getCell(sceneID).scenelnitialgoldnum);
                //这里不能走消息，因为module还没加载
                playerData.playerZoo.playerCoin.AddCoinByScene(
                    playerData.playerZoo.currSceneID, -multiCoin.coinBigInt + bigDefault);
            }
            
            GlobalDataManager.GetInstance().isLoadingScene = false;

            MessageManager.GetInstance().Send((int)GameMessageDefine.LoadZooSceneFinished);

            isSendFinished = true;
            
        }

        public override void Tick(int deltaTimeMS)
        {
            if (!isSendFinished)
            {
                return;
            }

            if (isClosed)
            {
                return;
            }

            waitCD.Tick(deltaTimeMS);

            if (waitCD.IsRunning() && waitCD.IsFinish())
            {
                isClosed = true;
                PageMgr.ClosePage("UILoading");
                //this.fsmCtr.Stop();
                PageMgr.ShowPage<UIMainPage>();
                //var playerData = GlobalDataManager.GetInstance().playerData;
                ////存储当前场景ID
                //playerData.playerZoo.currSceneID = (fsmCtr as FSMGameLoad).sceneID;
            }
        }

        public override void Leave()
        {
            base.Leave();
            //PageMgr.ShowPage<UIMainPage>();
        }

        public override void AddAllConvertCond()
        {
        }
    }

}
