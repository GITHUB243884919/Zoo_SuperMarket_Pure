using System;
using System.Collections;
using System.Collections.Generic;
using Game.GlobalData;
using Game.MessageCenter;
using UFrame;
using UFrame.Common;
using UFrame.MessageCenter;
using UnityEngine;
using UFrame.Logger;

namespace Game
{
    public class WorldMapModel : Singleton<WorldMapModel>, ISingleton
    {
        private List<int> allSceneType;

        public void Init()
        {
            if (scenePlayerData.isFirst > 0)
            {
                scenePlayerData.isFirst = 0;
                int defaultSceneId = 0; // 解锁默认场景
                UnlockScene(defaultSceneId);
                BrowseScene(defaultSceneId);
            }
            HandlePlayerStarCountChange();

        }

        private PlayerData playerData { get { return GlobalDataManager.GetInstance().playerData; } }
        private Config.sceneConfig configInst { get { return Config.sceneConfig.getInstace(); } }
        private ScenePlayerDataMSS scenePlayerData { get { return playerData.playerZoo.scenePlayerDataMSS; } }

        /// <summary>
        /// 玩家星星数量
        /// </summary>
        public int playerStarQuantity { get { return playerData.playerZoo.star; } }

        /// <summary>
        /// 玩家金币数量
        /// </summary>
        public string playerCoinQuantity { get { return playerData.playerZoo.playerCoin.GetCoinByScene(playerData.playerZoo.currSceneID).coinShow; } }

        public string playerEarningsQuantity { get { return MinerBigInt.ToDisplay(PlayerDataModule.CurrScenePerMinCoin(true)); } }
        public int playerDiamondQuantity { get { return playerData.playerZoo.diamond; } }


        /// <summary>
        /// 获取场景离开收益
        /// </summary>
        /// <param name="sceneId"></param>
        /// <returns></returns>
        public System.Numerics.BigInteger GetLeaveSceneCoins(int sceneId)
        {
            if (!GlobalDataManager.GetInstance().leaveSceneCoinData.LeaveSceneCoinDict.ContainsKey(sceneId))
                return 0;
            return GlobalDataManager.GetInstance().leaveSceneCoinData.LeaveSceneCoinDict[sceneId];
        }

        /// <summary>
        /// 获取场景每分钟收益
        /// </summary>
        /// <param name="sceneId"></param>
        /// <returns></returns>
        public System.Numerics.BigInteger GetSceneEarningsPerMinute(int sceneId)
        {
            if (sceneId == currSceneId)
                return PlayerDataModule.CurrScenePerMinCoin(true);
            return PlayerDataModule.LeaveScenePerMinCoin(sceneId,true);
        }

        /// <summary>
        /// 解锁场景 
        /// </summary>
        /// <param name="sceneId"></param>
        public void UnlockScene(int sceneId)
        {
            Config.sceneCell sceneCell = configInst.getCell(sceneId);
            if (sceneCell.israwopen < 1)
                return;

            ScenePlayerDataMSS.SceneStateMSS sceneState = GetSceneState(sceneId);
            if (sceneState == null)
            {
                sceneState = new ScenePlayerDataMSS.SceneStateMSS() { sceneId = sceneId };
                scenePlayerData.sceneStates.Add(sceneState);
            }
            if (sceneState.unlocked < 1)
                sceneState.unlocked = 1;
        }

        /// <summary>
        /// 浏览场景
        /// </summary>
        /// <param name="sceneId"></param>
        public void BrowseScene(int sceneId)
        {
            Config.sceneCell sceneCell = configInst.getCell(sceneId);
            if (sceneCell.israwopen < 1)
                return;

            ScenePlayerDataMSS.SceneStateMSS sceneState = GetSceneState(sceneId);
            if (sceneState == null)
            {
                sceneState = new ScenePlayerDataMSS.SceneStateMSS();
                scenePlayerData.sceneStates.Add(sceneState);
            }
            if (sceneState.browsed < 1)
                sceneState.browsed = 1;
        }

        /// <summary>
        /// 获取所有场景类型
        /// </summary>
        /// <returns></returns>
        public List<int> GetAllSceneType()
        {
            if (allSceneType == null)
            {
                allSceneType = new List<int>();
                foreach (var cell in GetAllSceneCell().Values)
                {
                    if (!allSceneType.Contains(cell.scenetype))
                        allSceneType.Add(cell.scenetype);
                }
            }
            return allSceneType;
        }

        /// <summary>
        /// 按类型过滤sceneCell
        /// </summary>
        /// <param name="sceneType"></param>
        /// <param name="coll"></param>
        public void FilterSceneCell(int sceneType, IDictionary<int, Config.sceneCell> coll)
        {
            IDictionary<string, Config.sceneCell> allSceneCell = GetAllSceneCell();
            foreach (var k in allSceneCell.Keys)
            {
                if (allSceneCell[k].scenetype == sceneType)
                    coll.Add(int.Parse(k), allSceneCell[k]);
            }
        }

        /// <summary>
        /// 是否有一个场景被解锁，按指定类型
        /// </summary>
        /// <param name="sceneType"></param>
        /// <returns></returns>
        public bool HasUnlockedSceneByType(int sceneType)
        {
            ScenePlayerDataMSS.SceneStateMSS sceneState = null;
            IDictionary<string, Config.sceneCell> allSceneCell = GetAllSceneCell();
            foreach (var k in allSceneCell.Keys)
            {
                if (allSceneCell[k].scenetype == sceneType)
                {
                    sceneState = GetSceneState(int.Parse(k));
                    if (sceneState != null && sceneState.unlocked > 0)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获取场景状态
        /// </summary>
        /// <param name="sceneId"></param>
        /// <returns></returns>
        public ScenePlayerDataMSS.SceneStateMSS GetSceneState(int sceneId)
        {
            return scenePlayerData.sceneStates.Find((s) => { return s.sceneId == sceneId; });
        }

        /// <summary>
        /// 所有解锁但为进入场景的数量
        /// </summary>
        /// <returns></returns>
        public int GetUnbrowsedSceneCount()
        {
            int ret = 0;
            int idx = PlayerData.GetcurrSceneIDByStar(GlobalDataManager.GetInstance().playerData);
            int a = 0;
            foreach (var sceneState in playerData.playerZoo.scenePlayerDataMSS.sceneStates)
            {
                if (sceneState.unlocked > 0 && sceneState.browsed < 1)
                    ret++;
                if (idx == sceneState.sceneId)
                {
                    a++;
                }
                //else if (sceneState.unlocked > 0 && sceneState.browsed == 1 && sceneState.enterCount < 1 && sceneState.sceneId != GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID)
                //    ret++;
            }
            if (a==0)
            {
                ret++;
            }


            return ret;
        }

        /// <summary>
        /// 当前场景Id
        /// </summary>
        public int currSceneId { get { return playerData.playerZoo.currSceneID; } }

        /// <summary>
        /// 处理用户星星数量改变
        /// </summary>
        public void HandlePlayerStarCountChange()
        {
            var cellDict = GetAllSceneCell();
            foreach (string key in cellDict.Keys)
            {
                if (playerStarQuantity >= cellDict[key].openstar)
                    UnlockScene(int.Parse(key));
            }
        }

        /// <summary>
        /// 获取所有场景数据
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, Config.sceneCell> GetAllSceneCell()
        {
            return configInst.AllData;
        }
    }

    public class WordlMapModule : GameModule
    {
        public WordlMapModule(int orderID) : base(orderID) { }
        public override void Tick(int deltaTimeMS) {  }

        public override void Init()
        {
            mapModel.Init();
            RegistMessages();

            LogWarp.Log("-->WordlMapModule init.");
        }

        private PlayerData playerData { get { return GlobalDataManager.GetInstance().playerData; } }

        public override void Release()
        {
            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.BroadcastStarOfPlayerData, OnPlayerStarCountChanged);
        }

        private void RegistMessages()
        {
            MessageManager.GetInstance().Regist((int)GameMessageDefine.BroadcastStarOfPlayerData, OnPlayerStarCountChanged);
        }

        private WorldMapModel mapModel { get { return WorldMapModel.GetInstance(); } }

        private void OnPlayerStarCountChanged(Message message)
        {
            mapModel.HandlePlayerStarCountChange();
            UIMapPage mapPage = PageMgr.GetPage<UIMapPage>();
            if (mapPage != null)
                mapPage.UpdatePieceStatesDisplay();
        }
    }
}
