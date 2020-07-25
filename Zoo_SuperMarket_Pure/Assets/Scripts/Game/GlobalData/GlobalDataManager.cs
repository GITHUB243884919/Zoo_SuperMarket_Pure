/*******************************************************************
* FileName:     GlobalDataManager.cs
* Author:       Fan Zheng Yong
* Date:         2019-8-16
* Description:  
* other:    
********************************************************************/


using UFrame.Logger;
using System;
using System.Collections;
using System.Collections.Generic;
using UFrame;
using UFrame.Common;
using UnityEngine;

namespace Game.GlobalData
{
    public partial class GlobalDataManager : Singleton<GlobalDataManager>, ISingleton
    {
        bool isInit = false;

        public I18N i18n { get; protected set; }

        public LogicTableGroup logicTableGroup { get; protected set; }

        public LogicTableResource logicTableResource { get; protected set; }

        public LogicTableExitGate logicTableExitGate { get; protected set; }

        public LogicTableEntryGate logicTableEntryGate { get; protected set; }

        public LogicTableParkingData logicTableParkingData { get; protected set; }

        public LogicTableSceneData logicTableSceneData { get; protected set; }

        public LogicTableVisitorAction logicTableVisitorAction { get; protected set; }

        public LogicAnimalAtlasData logicAnimalAtlasData { get; protected set; }

        public AnimalAnimation animalAnimation { get; protected set; }

        public ZooGameSceneData zooGameSceneData { get; protected set; }

        public PlayerData playerData = null;
        /// <summary>
        /// 玩家动物栏场景UI类
        /// </summary>
        public LittleSceneUI littleSceneUI;

        private Vector3 sceneForward = Vector3.zero;

        /// <summary>
        /// 场景正方向
        /// </summary>
        public Vector3 SceneForward {
            get
            {
                float[] v = Config.globalConfig.getInstace().SceneForward;
                sceneForward.x = v[0];
                sceneForward.y = v[1];
                sceneForward.z = v[2];
                return sceneForward;
            }
        }

        /// <summary>
        /// 登录游戏时，加载场景
        /// </summary>
        public bool isLoginLoadScene = true;

        /// <summary>
        /// 是否在加载场景
        /// </summary>
        public bool isLoadingScene = true;

        /// <summary>
        /// 离开场景的金币，按场景累计
        /// </summary>
        public LeaveSceneCoinData leaveSceneCoinData = new LeaveSceneCoinData();

        /// <summary>
        /// 当前的buff列表
        /// </summary>
        public List<Buff> currBuffs = new List<Buff>();

        /// <summary>
        /// 离线秒数
        /// </summary>
        public double offlineSeconds = 0;

        public void Init()
        {
            if (isInit)
            {
                return;
            }
            isInit = true;
            littleSceneUI = new LittleSceneUI();
            InitLogicTable();
            InitLogicRes();
            playerData = PlayerData.Load();
            //图鉴信息获取
            logicAnimalAtlasData.GetAnimalAtlasData();
        }


        /// <summary>
        /// 不是所有都Release
        /// </summary>
        public void Release()
        {
            zooGameSceneData.Release();
            littleSceneUI.Release();
        }

        protected void InitLogicTable()
        {
            if (i18n == null)
            {
                i18n = new I18N();
            }

            if (logicTableGroup == null)
            {
                logicTableGroup = new LogicTableGroup();
            }

            if (logicTableResource == null)
            {
                logicTableResource = new LogicTableResource();
            }

            if (animalAnimation == null)
            {
                animalAnimation = new AnimalAnimation();
            }

            if (logicTableExitGate == null)
            {
                logicTableExitGate = new LogicTableExitGate();
            }

            if (logicTableEntryGate == null)
            {
                logicTableEntryGate = new LogicTableEntryGate();
            }

            if (logicTableParkingData == null)
            {
                logicTableParkingData = new LogicTableParkingData();
            }

            if (logicTableSceneData == null)
            {
                logicTableSceneData = new LogicTableSceneData();
            }

            if (logicTableVisitorAction == null)
            {
                logicTableVisitorAction = new LogicTableVisitorAction();
            }

            if (logicAnimalAtlasData == null)
            {
                logicAnimalAtlasData = new LogicAnimalAtlasData();
            }
        }

        protected void InitLogicRes()
        {
            if (zooGameSceneData == null)
            {
                zooGameSceneData = new ZooGameSceneData();
            }
        }

    }

}
