using Game;
using Game.GlobalData;
using Game.MessageCenter;
using UFrame;

namespace CrossRoadGame
{
    public class CrossRoadLeaveZooCoinModule : GameModule
    {
        IntCD CD;
        int CDVal;
        PlayerData playerData { get { return GlobalDataManager.GetInstance().playerData; } }
        public CrossRoadLeaveZooCoinModule(int orderID) : base(orderID) { }

        public override void Init()
        {
            InitCD();
        }

        public override void Release()
        {
            CD.Stop();
        }

        public override void Tick(int deltaTimeMS)
        {
            Tick_CD(deltaTimeMS);
        }

        protected void InitCD()
        {
            CDVal = Config.globalConfig.getInstace().LeaveSceneCoinCD * 1000;
            CD = new IntCD(CDVal);
            CD.Run();
        }

        protected void Tick_CD(int deltaTimeMs)
        {
            if (CD == null)
            {
                return;
            }
            int realCDVal = CD.org;
            CD.Tick(deltaTimeMs);
            if (CD.IsRunning() && CD.IsFinish())
            {
                if (CD.cd < 0)
                {
                    realCDVal += (-CD.cd);
                }
                CD.Reset();
                ZooAddCoin(realCDVal);
            }
        }

        /// <summary>
        /// 只算第一个场景的
        /// </summary>
        /// <param name="deltaTimeMs"></param>
        protected void ZooAddCoin(int deltaTimeMs)
        {
            var perMinCoin = PlayerDataModule.LeaveScenePerMinCoin(
                GameConst.First_SceneID, true);
            var deltaCoin = perMinCoin * deltaTimeMs / 60000;
            
            SetValueOfPlayerData.Send((int)GameMessageDefine.AddCoinOfPlayerDataMSSC, 0, deltaCoin, 0);

            //var sceneStates = playerData.playerZoo.scenePlayerDataMSS.sceneStates;
            //bool isCalcFirst = false;
            //for (int i = 0; i < sceneStates.Count; i++)
            //{
            //    var sceneState = sceneStates[i];
            //    if (sceneState.enterCount > 0)
            //    {
            //        if (sceneState.sceneId == GameConst.First_SceneID)
            //        {
            //            isCalcFirst = true;
            //        }
            //        var perMinCoin = PlayerDataModule.LeaveScenePerMinCoin(sceneState.sceneId, true);
            //        var deltaCoin = perMinCoin * deltaTimeMs / 60000;
            //        if (perMinCoin > 0)
            //        {
            //            playerData.playerZoo.playerCoin.AddCoinByScene(sceneState.sceneId, deltaCoin);
            //        }
            //    }
            //}

            ////防止场景数据中没有第一个场景
            //if (!isCalcFirst)
            //{
            //    var perMinCoin = PlayerDataModule.LeaveScenePerMinCoin(GameConst.First_SceneID, true);
            //    var deltaCoin = perMinCoin * deltaTimeMs / 60000;
            //    if (perMinCoin > 0)
            //    {
            //        playerData.playerZoo.playerCoin.AddCoinByScene(GameConst.First_SceneID, deltaCoin);
            //    }
            //}
        }
    }
}
