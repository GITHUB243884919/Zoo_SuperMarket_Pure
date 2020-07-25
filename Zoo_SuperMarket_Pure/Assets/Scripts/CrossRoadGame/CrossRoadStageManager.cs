using Game;
using Game.MessageCenter;
using Game.MiniGame;
using UFrame;
using UFrame.Logger;
using UFrame.Common;
using UFrame.MessageCenter;
using UnityEngine;
using Game.GlobalData;
using DG.Tweening;
using System;

namespace CrossRoadGame
{
    public class CrossRoadStageManager : SingletonMono<CrossRoadStageManager>
    {
        [Header("测试关卡ID")]
        /// <summary>
        /// 测试关卡id
        /// </summary>
        public int TeststageID = Const.Invalid_Int;

        [Header("动物动画播放速度")]
        /// <summary>
        /// 动物动画播放速度
        /// </summary>
        public float animalAnimSpeed = 1f;

        [Header("动物移动速度(单位长度/秒)")]
        /// <summary>
        /// 动物移动速度(单位长度/秒)
        /// </summary>
        public float animalMoveSpeed = 5f;

        [Header("动物移动加速度(单位长度/秒^2)")]
        /// <summary>
        /// 动物移动加速度(单位长度/秒)
        /// </summary>
        public float animalAcceleration = 0.5f;

        [Header("动物旋转速度(欧拉角/秒)")]
        /// <summary>
        /// 动物旋转速度(欧拉角/秒)
        /// </summary>
        public float rotateSpeed = 10;

        [Header("动物前转等待系数")]
        /// <summary>
        /// 动物前转等待系数
        /// </summary>
        public double waitRotateRatio = 0.01d;

        [Header("按钮长按时间响应时间")]
        /// <summary>
        /// 按钮长按时间
        /// </summary>
        public float buttonClickLongPressTime = 0.3f;

        [Header("是否开启碰撞")]
        /// <summary>
        /// 是否开启碰撞
        /// </summary>
        public bool isCollision = true;

        [Header("汽车起点距离中心点的X方向的偏移量")]
        /// <summary>
        /// 汽车起点距离中心点的X方向的偏移量
        /// </summary>
        public float carStartOffsetX = 30;

        [Header("碰撞后失败UI延时出现时间")]
        /// <summary>
        /// 碰撞后失败UI延时出现时间
        /// </summary>
        public float timeLapseFailPage = 0.5f;

        PlayerData playerData { get { return GlobalDataManager.GetInstance().playerData; } }

        FSMCrossRoadGame fsmLoad = null;
        GameModules gameModules = null;
        int moduleOrderID = 0;
        public override void Awake()
        {
            base.Awake();
        }

        public void Start()
        {
            Init();
        }

        public void Update()
        {
            int deltaMS = Math_F.FloatToInt1000(Time.deltaTime);
            fsmLoad.Tick(deltaMS);
            gameModules.Tick(deltaMS);
        }

        public void Init()
        {
            //初始化加载状态机
            fsmLoad = new FSMCrossRoadGame();
            fsmLoad.AddState(new StateLoadOrgScene((int)CrossRoadGameState.LoadOrgScene, fsmLoad));
            fsmLoad.AddState(new StateLoadSceneObject((int)CrossRoadGameState.LoadSceneObject, fsmLoad));
            fsmLoad.AddState(new StateLoadAnimal((int)CrossRoadGameState.LoadAnimal, fsmLoad));
            fsmLoad.Run();

            //module容器
            gameModules = new GameModules();
            //注册消息
            MessageManager.GetInstance().Regist((int)GameMessageDefine.LoadCrossRoadLevelFinished, OnLoadCrossRoadLevelFinished);
            MessageManager.GetInstance().Regist((int)GameMessageDefine.CrossRoadGameFailure, OnGetCrossRoadGameFailure);
        }

        public void Load(int stageID)
        {
            if (stageID < 0 )
            {
                string e = string.Format("过马路关卡数不正确 {0}", stageID);
                throw new System.Exception(e);
            }
            else if(stageID >= Config.crossroadstageConfig.getInstace().RowNum)  
            {
                int number = Config.crossroadstageConfig.getInstace().RowNum;
                stageID =  Config.crossroadstageConfig.getInstace().getCell(number).level;
            }
            GameSoundManager.GetInstance().PlayBGMusicAsync(Config.globalConfig.getInstace().ZooSceneBGM);

            CrossRoadModelManager.GetInstance().stageID = stageID;
            PageMgr.CloseAllPage(true, "");
            PageMgr.ShowPage<UICrossRoadLoading>();
            SetFSM("UICrossRoadLoading");
            LoadModule();
        }
        /// <summary>
        /// 再来一次
        /// </summary>
        /// <param name="stageID"></param>
        public void Load_Restart()
        {
            ////清除小动物
            //foreach (var item in CrossRoadModelManager.GetInstance().animalTeamModel.entityCrossRoadAnimalList)
            //{
            //    CrossRoadModelManager.GetInstance().entityModel.RemoveFromEntityMovables(item);
            //}
            //CrossRoadModelManager.GetInstance().entityModel.Release_Load_Restart();
            //CrossRoadModelManager.GetInstance().Release_NoRoad();
            //gameModules.AddMoudle(new CrossRoadCarModule(moduleOrderID++));

            ////生成小动物
            //MessageManager.GetInstance().Send((int)GameMessageDefine.SetCrossRoadAnimalObjectData);
            //gameModules.Run();
            ////移动相机
            //CrossRoadCameraController.GetInstance().MoveCamera( CrossRoadModelManager.GetInstance().startPoint );
        }
        
        void SetFSM(string loadingPageName)
        {
            fsmLoad.loadingPageName = loadingPageName;
            fsmLoad.GotoState((int)CrossRoadGameState.LoadOrgScene);
        }

        void LoadModule()
        {
            gameModules.AddMoudle(
                new PlayerDataModule(moduleOrderID++));
            gameModules.AddMoudle(
                new CrossRoadCarModule(moduleOrderID++));
            gameModules.AddMoudle(
                new CrossRoadStrengthModule(moduleOrderID++));

            gameModules.AddMoudle(
                new CrossRoadLeaveZooCoinModule(moduleOrderID++));

            ///移动模块最好保持是最后一个module，新增加module在此之前添加
            gameModules.AddMoudle(
                new CrossRoadMoveMovableEntityModule(moduleOrderID++));

        }

        public void UnLoad()
        {
            gameModules.Release();
            CrossRoadModelManager.GetInstance().Release();
            PoolManager.GetInstance().Release();

            Resources.UnloadUnusedAssets();
            System.GC.Collect();
        }

        public void Release()
        {
            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.LoadCrossRoadLevelFinished, OnLoadCrossRoadLevelFinished);
            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.CrossRoadGameFailure, OnGetCrossRoadGameFailure);

        }

        /// <summary>
        /// 小游戏关卡加载成功
        /// </summary>
        /// <param name="msg"></param>
        protected void OnLoadCrossRoadLevelFinished(Message msg)
        {
            gameModules.Run();
        }

        /// <summary>
        /// 小游戏失败
        /// </summary>
        /// <param name="obj"></param>
        private void OnGetCrossRoadGameFailure(Message obj)
        {
            gameModules.Stop();

            float timeCount = 0.1f;
            DOTween.To(() => timeCount, a => timeCount = a, 0.1f, timeLapseFailPage).OnComplete(new TweenCallback(delegate
            {
                PageMgr.ShowPage<UIGameFailPage>();

            }));
            
            //弹出UI  失败
            LogWarp.LogError("游戏失败");
        }
    }
}

