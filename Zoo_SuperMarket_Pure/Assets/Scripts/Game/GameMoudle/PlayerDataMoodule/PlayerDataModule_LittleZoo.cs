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
        /// 动物栏的门票升级消息修改
        /// </summary>
        /// <param name="msg"></param>
        protected void OnSetLittleZooTicketsLevelPlayerData(Message msg)
        {
            var _msg = msg as SetDetailValueOfPlayerData;
            // 涉及金币减扣
            LittleZooModuleDataMSS littleZooModuleData = GlobalDataManager.GetInstance().playerData.GetLittleZooModuleData(_msg.detailVal);
            buildupCell = GetBuildupCell(_msg.detailVal);
            if ((littleZooModuleData.littleZooTicketsLevel+ _msg.deltaVal) > buildupCell.lvmax)
            {
                return;
            }
            BigInteger bigDelta = (LittleZooModule.GetUpGradeConsumption(_msg.detailVal, littleZooModuleData.littleZooTicketsLevel + _msg.deltaVal));
            if (!playerData.playerZoo.playerCoin.WarpAddCoin(playerData, -bigDelta))
            {
                return;
            }

            //修改动物栏等级
            int currVal = littleZooModuleData.littleZooTicketsLevel + _msg.deltaVal;
            int idx = GlobalDataManager.GetInstance().playerData.GetLittleZooIDIndexOfDataIdx(_msg.detailVal); //获取动物栏ID  下标
            this.playerData.playerZoo.littleZooModuleDatasMSS[idx].littleZooTicketsLevel = currVal;

            BroadcastDetailValueOfPlayerData.Send((int)GameMessageDefine.BroadcastLittleZooTicketsLevelPlayerData,
                _msg.detailVal, currVal, _msg.deltaVal);
            BuildingLevelup.Send((int)BuildingTypeFM.LittleZoo, _msg.detailVal, (int)LittleZooProperty.TicketPrice, currVal);

            //收集星星：
            var lvshage = Config.buildupConfig.getInstace().getCell(_msg.detailVal).lvshage;

            int idx01 = FindLevelRangIndex01(lvshage, currVal);
            //LogWarp.LogErrorFormat("测试：  等级={0}  下标={1}  ",currVal,idx01);

            int stageLevel = Config.buildupConfig.getInstace().getCell(_msg.detailVal).lvshage[idx01];
            if (this.playerData.playerZoo.littleZooModuleDatasMSS[idx].littleZooTicketsLevel == stageLevel)
            {
                int awardType = Config.buildupConfig.getInstace().getCell(_msg.detailVal).lvrewardtype[idx01];
                int awardID = Config.buildupConfig.getInstace().getCell(_msg.detailVal).lvreward[idx01];
                var cell = Config.itemConfig.getInstace().getCell(awardID);
                if (awardType == 1)
                {
                    //发放奖励道具
                    MessageInt.Send((int)GameMessageDefine.GetItem, awardID);
                    if (cell.itemtype == 2)
                    {
                        PageMgr.GetPage<UIMainPage>().OnMoneyEffect();
                    }                    //LogWarp.LogErrorFormat("动物栏   当前等级为{0}，可以发放奖励道具{1}", stageLevel, awardID);
                }
                else if (awardType == 2)
                {
                    var buildUpCell = Config.buildupConfig.getInstace().getCell(_msg.detailVal);

                    int animalID = buildUpCell.lvreward[idx01];
                    //LogWarp.LogErrorFormat("测试：AAAAAAAAAAAAAAAAAAAAAAA 动物栏：{0}   animalID ={1}" , _msg.detailVal, animalID);
                    var animalUpCell = Config.animalupConfig.getInstace().getCell(animalID);
                    //判断是否需要存储动物
                    bool isExistAnimalID = playerData.playerZoo.animalMSS15.FindAnimalID(animalID);
                    if (!isExistAnimalID)
                    {
                        playerData.playerZoo.animalMSS15.AddAnimal(animalID,true);
                        LittleZooModule.LoadAnimal(_msg.detailVal, animalID,
                          animalUpCell.moveradius, buildUpCell.animalwanderoffset);
                        GetShowUIReceivePage(animalID);
                        MessageInt.Send((int)GameMessageDefine.GetAnimalAtlasDataMessage, animalID);
                        MessageManager.GetInstance().Send((int)GameMessageDefine.AnimalBuffAlterSucceed);
                    }
                }
                //发放星星
                MessageInt.Send((int)GameMessageDefine.GetItem, 4);

                //LogWarp.LogErrorFormat("动物栏  当前等级为{0}，可以发放星星", stageLevel);

            }


        }

        /// <summary>
        /// 动物升级对应的动物展示旋转
        /// </summary>
        /// <param name="animalID"></param>
        private static void GetShowUIReceivePage(int animalID)
        {
            //关于Ui等级打点（在旋转相机的时候）
            UIZooPage uIZooPage = PageMgr.GetPage<UIZooPage>();
            if (uIZooPage != null)
            {
                uIZooPage.OnGetBroadcastLittleZooTicketsLevelPlayerData(null);
                uIZooPage.Hide();
            }

            var resourceID = Config.animalupConfig.getInstace().getCell(animalID).resourceload;
            //旋转视角UI
            PageMgr.ShowPage<UIReceivePage>(resourceID);
            MessageString.Send((int)GameMessageDefine.UIMessage_ActiveButHidePart, "UIMainPage");
        }

        /// <summary>
        /// 动物栏的观光点数量消息修改
        /// </summary>
        /// <param name="obj"></param>
        private void OnSetLittleZooVisitorLocationLevelOfPlayerData(Message obj)
        {
            var _msg = obj as SetDetailValueOfPlayerData;
            // 涉及金币减扣
            LittleZooModuleDataMSS littleZooModuleData = GlobalDataManager.GetInstance().playerData.GetLittleZooModuleData(_msg.detailVal);
            buildupCell = GetBuildupCell(_msg.detailVal);
            if (littleZooModuleData.littleZooVisitorSeatLevel >= buildupCell.watchmaxlv)
            {
                return;
            }
            BigInteger bigDelta = (LittleZooModule.GetUpGradeVisitorLocationLevelConsumption(_msg.detailVal, littleZooModuleData.littleZooVisitorSeatLevel + _msg.deltaVal));

            //bool retCode = VaryDataCoin(bigDelta);
            //if (!retCode)
            //{
            //    //string e = string.Format("升级动物栏扣钱失败");
            //    //throw new System.Exception(e);
            //    return;
            //}
            if (!playerData.playerZoo.playerCoin.WarpAddCoin(playerData, -bigDelta))
            {
                return;
            }

            int currVal = littleZooModuleData.littleZooVisitorSeatLevel + _msg.deltaVal;
            //LogWarp.Log("测试：  等级原来是"+zooLevel+"   现在是  "+currVal);
            int idx = GlobalDataManager.GetInstance().playerData.GetLittleZooIDIndexOfDataIdx(_msg.detailVal);  //获取动物栏ID  下标
            this.playerData.playerZoo.littleZooModuleDatasMSS[idx].littleZooVisitorSeatLevel = currVal;

            //BroadcastValueOfPlayerData.Send((int)GameMessageDefine.BroadcastCoinOfPlayerData,
            //    0, 0, BigInteger.Parse(GlobalDataManager.GetInstance().playerData.playerZoo.coin), bigDelta);
            BroadcastDetailValueOfPlayerData.Send((int)GameMessageDefine.BroadcastLittleZooVisitorLocationLevelOfPlayerData,
                _msg.detailVal, currVal, _msg.deltaVal);
            BuildingLevelup.Send((int)BuildingTypeFM.LittleZoo, _msg.detailVal, (int)LittleZooProperty.Capacity, currVal);
        }

        /// <summary>
        /// 动物栏的观光游客流量消息修改
        /// </summary>
        /// <param name="obj"></param>  
        private void OnSetLittleZooEnterVisitorSpawnLevelOfPlayerData(Message obj)
        {
            var _msg = obj as SetDetailValueOfPlayerData; 
             // 涉及金币减扣
            LittleZooModuleDataMSS littleZooModuleData = GlobalDataManager.GetInstance().playerData.GetLittleZooModuleData(_msg.detailVal);
            buildupCell = GetBuildupCell(_msg.detailVal);
            if (littleZooModuleData.littleZooEnterVisitorSpawnLevel >= buildupCell.itemmaxlv)
            {
                return;
            }
            BigInteger bigDelta = (LittleZooModule.GetUpGradeEnterVisitorSpawnLevelConsumption(_msg.detailVal, littleZooModuleData.littleZooEnterVisitorSpawnLevel + _msg.deltaVal));

            //bool retCode = VaryDataCoin(bigDelta);
            //if (!retCode)
            //{
            //    //string e = string.Format("升级动物栏扣钱失败");
            //    //throw new System.Exception(e);
            //    return;
            //}
            if (!playerData.playerZoo.playerCoin.WarpAddCoin(playerData, -bigDelta))
            {
                return;
            }


            //修改动物栏等级
            int currVal = littleZooModuleData.littleZooEnterVisitorSpawnLevel + _msg.deltaVal;
            //LogWarp.Log("测试：  等级原来是"+zooLevel+"   现在是  "+currVal);
            int idx = GlobalDataManager.GetInstance().playerData.GetLittleZooIDIndexOfDataIdx(_msg.detailVal); //获取动物栏ID  下标
            this.playerData.playerZoo.littleZooModuleDatasMSS[idx].littleZooEnterVisitorSpawnLevel = currVal;
            BroadcastDetailValueOfPlayerData.Send((int)GameMessageDefine.BroadcastLittleZooEnterVisitorSpawnLevelOfPlayerData,
                _msg.detailVal, currVal, _msg.deltaVal);
            BuildingLevelup.Send((int)BuildingTypeFM.LittleZoo, _msg.detailVal, (int)LittleZooProperty.VisitSpeed, currVal);
        }

        /// <summary>
        /// 开启动物
        /// </summary>
        private void OnSetAnimalLevelData(Message obj)
        {
            var msg = obj as SetBuyAnimalObjectData;
            /* 扣钱   扣钱成功后修改商品数量     发送扣钱的通知和商品修改的通知  */
            var animalMSS15 = GlobalDataManager.GetInstance().playerData.playerZoo.animalMSS15;
            Config.animalupCell animalupCell = Config.animalupConfig.getInstace().getCell(msg.goodsID);
            if (animalMSS15.GetAnimalProp(msg.goodsID).lv >= Config.globalConfig.getInstace().AnimalLvUpLimit)
            {
                return;
            }
            var expendDelta = LittleZooModule.GetAnimalUpLevelPriceFormula(msg.goodsID);
            
            if (!WarpAddCoin(-expendDelta))
            {
                return;
            }

            //增加动物数据
            Config.buildupCell cellBuildUp = Config.buildupConfig.getInstace().getCell(msg.littleZooID);

            animalMSS15.AnimalLvUp(msg.goodsID,1);

            LittleZooModule.playerAnimalGoToResourceID.TryGetValue(msg.goodsID, out int playerAnimalGoToResourceID);
            MessageInt.Send((int)GameMessageDefine.AnimalPlayLevelUpEffect, playerAnimalGoToResourceID);
            GetAddNewAnimalData.Send((int)GameMessageDefine.GetAnimalLevel, msg.goodsID, msg.littleZooID);
            MessageManager.GetInstance().Send((int)GameMessageDefine.AnimalBuffAlterSucceed);

        }
        public bool WarpAddCoin(int expendDelta)
        {
            //检查钱
            if ((playerData.playerZoo.diamond + expendDelta)<0)
                return false;
            //扣钱
            playerData.playerZoo.diamond+= expendDelta;
            //发送钻石修改通知
            MessageManager.GetInstance().Send((int)GameMessageDefine.BroadcastDiamondOfPlayerData);
            return true;
        }

        protected void OnOpenNewLittleZoo(Message msg)
        {
            //判定是不是最后，是否需要加载新地块，以及新加载地块上的动物栏
            var _msg = msg as OpenNewLittleZoo;
            int littleZooID = _msg.littleZooID;
            string e;
            int idx = GlobalDataManager.GetInstance().playerData.GetLittleZooIDIndexOfDataIdx(littleZooID);
            if (idx < 0)
            {
                //e = string.Format("开启的动物栏 {0} 在用户数据中没有", littleZooID);
                //throw new System.Exception(e);
                return;
            }

#if UNITY_EDITOR
            if (playerData.playerZoo.littleZooModuleDatasMSS[idx].littleZooTicketsLevel != 0)
            {
                //e = string.Format("开启的动物栏 {0} 在用户数据中等级!=0", littleZooID);
                //throw new System.Exception(e);
                return;
            }
#endif
            playerData.playerZoo.littleZooModuleDatasMSS[idx].sceneID = playerData.playerZoo.currSceneID;
            playerData.playerZoo.littleZooModuleDatasMSS[idx].littleZooTicketsLevel = 1;
            playerData.playerZoo.littleZooModuleDatasMSS[idx].littleZooEnterVisitorSpawnLevel = 1;
            playerData.playerZoo.littleZooModuleDatasMSS[idx].littleZooVisitorSeatLevel = 1;

            int nextGroupID = Const.Invalid_Int;
            bool trigerExtend = false;
            trigerLoadLittleZooIDs.Clear();
            if (GlobalDataManager.GetInstance().logicTableGroup.IsTrigerLoadNextGroupID(
                playerData.playerZoo.currSceneID, littleZooID, ref nextGroupID))
            {
                //触发额外的地块，用户数据中加数据
                if (!GlobalDataManager.GetInstance().zooGameSceneData.IsExtendGroupContains(nextGroupID))
                {
                    trigerExtend = true;
                    var cell = Config.groupConfig.getInstace().getCell(nextGroupID);
                    trigerLoadLittleZooIDs.AddRange(cell.startid);

                    for (int i = 0; i < trigerLoadLittleZooIDs.Count; i++)
                    {
                        //playerData.playerZoo.littleZooLevels.Add(0);
                        LittleZooModuleDataMSS littleZooModuleData = new LittleZooModuleDataMSS
                        {
                            sceneID = playerData.playerZoo.currSceneID,
                            littleZooID = trigerLoadLittleZooIDs[i],
                            littleZooTicketsLevel = 0,
                            littleZooVisitorSeatLevel = 0,
                            littleZooEnterVisitorSpawnLevel = 0,
                        };
                        playerData.playerZoo.littleZooModuleDatasMSS.Add(littleZooModuleData);
                    }
                }
            }
            //LogWarp.LogError("测试：开启了littleZooID" + littleZooID);
            BroadcastOpenNewLittleZoo.Send(littleZooID, trigerExtend, nextGroupID, trigerLoadLittleZooIDs);
        }
        /// <summary>
        /// 获取对应场景的本地动物栏数据
        /// </summary>
        /// <returns></returns>
        private Config.buildupCell GetBuildupCell(int littleZooID)
        {
            Config.buildupCell buildupCell = null;
            var allData = Config.buildupConfig.getInstace().AllData;
            foreach (var item in allData)
            {
                if (item.Value.scene == playerData.playerZoo.currSceneID && item.Key == littleZooID.ToString())
                {
                    buildupCell = item.Value;
                    return buildupCell;
                }
            }
            string e = string.Format("没有{0}对应的动物栏数据表", playerData.playerZoo.currSceneID);
            throw new System.Exception(e);
            return buildupCell;
        }

    }
}