using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UFrame.Logger;

namespace Game.GlobalData
{
    [Serializable]
    public class PlayerZoo
    {
        /// <summary>
        /// 弃用-玩家的动物数据
        /// </summary>
        public PlayerAnimal playerAnimal = new PlayerAnimal();

        /// <summary>
        /// 玩家动物数据-已弃用
        /// </summary>
        public List<PlayerAnimal_MS> playerAnimalDatas = new List<PlayerAnimal_MS>();

        /// <summary>
        /// 玩家动物数据 多场景 
        /// </summary>
        public List<PlayerAnimal_MSS> playerAnimalDatasMSS = new List<PlayerAnimal_MSS>();


        /// <summary>
        /// 动物栏ID对应的数据 -已弃用
        /// </summary>
        public List<LittleZooModuleData> littleZooModuleDatas = new List<LittleZooModuleData>();

        /// <summary>
        /// 动物栏ID对应的数据 多场景，多布局
        /// </summary>
        public List<LittleZooModuleDataMSS> littleZooModuleDatasMSS = new List<LittleZooModuleDataMSS>();

        /// <summary>
        /// 弃用-售票口门票等级
        /// </summary>
        public int entryTicketsLevel = 1;

        /// <summary>
        /// 弃用-开启的入口，List形式存储每个入口的数据
        /// </summary>
        public List<EntryGateData> entryGateList = new List<EntryGateData>();

        /// <summary>
        /// 售票口多场景数据
        /// </summary>
        public List<EntryGateData_MS> entryGateList_MS = new List<EntryGateData_MS>();

        /// <summary>
        /// 弃用-停车场数据源
        /// </summary>
        public ParkingCenterData parkingCenterData = new ParkingCenterData();

        /// <summary>
        /// 停车场数据源(带场景ID)
        /// </summary>
        public List<ParkingCenterData_MS> parkingCenterDataList = new List<ParkingCenterData_MS>();

        /// <summary>
        /// 引导任务数据 --以作废
        /// </summary>
        public GuideMissionPlayerData guideMissionPlayerData = new GuideMissionPlayerData();

        /// <summary>
        /// 引导任务数据 多场景多布局
        /// </summary>
        public GuideMissionPlayerDataMSS guideMissionPlayerDataMSS = new GuideMissionPlayerDataMSS();

        /// <summary>
        /// 玩家场景数据 --已作废
        /// </summary>
        public ScenePlayerData scenePlayerData = new ScenePlayerData();

        /// <summary>
        /// 玩家场景数据 多场景多布局
        /// </summary>
        public ScenePlayerDataMSS scenePlayerDataMSS = new ScenePlayerDataMSS();

        /// <summary>
        /// 金币 -- 已经弃用
        /// </summary>
        public string coin = "0";

        /// <summary>
        /// 金币大数据==coin -- 已经弃用
        /// </summary>
        public System.Numerics.BigInteger coinBigInt = 0;

        /// <summary>
        /// 多场景玩家金币
        /// </summary>
        public PlayerCoin playerCoin = new PlayerCoin();

        /// <summary>
        /// 钻石
        /// </summary>
        public int diamond = 0;

        /// <summary>
        /// 星星
        /// </summary>
        public int star = 0;

        /// <summary>
        /// 拥有但还没使用的道具
        /// </summary>
        //public List<Item> itemList = new List<Item>();
        public List<int> itemList = new List<int>();

        /// <summary>
        /// 上次登出时间
        /// </summary>
        public long lastLogoutTime = 0;

        /// <summary>
        /// 已弃用出口等级
        /// </summary>
        public int exitGateLevel = UFrame.Const.Invalid_Int;

        /// <summary>
        /// 小游戏等级(过马路)
        /// </summary>
        public int littleGameLevel = 1;

        /// <summary>
        /// 全部动物数量
        /// </summary>
        public int allZooNumber;

        /// <summary>
        /// 建筑升级的Transform
        /// </summary>
        public Transform BuildShowTransform;


        /// <summary>
        /// 是否开启声音
        /// </summary>
        public bool isSound = true;
        /// <summary>
        /// 是否开启新手引导
        /// </summary>
        public bool isGuide = false;

        public bool isGuide_CrossRoad=false;

        /// <summary>
        /// 出口cd buff计算值
        /// </summary>
        public float buffExitEntryCDVal = UFrame.Const.Invalid_Float;

        /// <summary>
        /// 浏览cd buff计算值
        /// </summary>
        public float buffVisitCDVal = UFrame.Const.Invalid_Float;

        /// <summary>
        /// 入口cd buff计算值
        /// </summary>
        public float buffEntryGateCDVal = UFrame.Const.Invalid_Float;

        /// <summary>
        /// 金币收入倍数相加 buff影响值
        /// </summary>
        public float buffRatioCoinInComeAdd = 1;

        /// <summary>
        /// 金币收入倍数相乘法 buff影响值
        /// </summary>
        public float buffRatioCoinInComeMul = 1;

        /// <summary>
        /// 是否开启动物培养功能（默认为否）
        /// </summary>
        public bool isShowAnimalCultivate = true;

        /// <summary>
        /// Buff列表
        /// </summary>
        public List<Buff> buffList = new List<Buff>();

        /// <summary>
        /// 离线buff列表
        /// </summary>
        public List<Buff> offlineBuffList = new List<Buff>();

        /// <summary>
        /// 开启的入口数量
        /// </summary>
        public int numEntryGate = 1;




        /// <summary>
        /// 玩家观看广告数据
        /// </summary>
        public PlayerNumberOfVideosWatched playerNumberOfVideosWatched = new PlayerNumberOfVideosWatched();

        /// <summary>
        /// 记录上次登录日期（天，不可大于31）
        /// </summary>
        public int LastLogingDate_Day = 0;

        /// <summary>
        /// 场景加载判定是否需要离线，用于在主界面用于场景加载后
        /// 离线界面是否显示逻辑
        /// </summary>
        public bool isLoadingShowOffline = false;

        /// <summary>
        /// 当前场景，默认都是第一个
        /// </summary>
        public int currSceneID = GameConst.First_SceneID;

        /// <summary>
        /// 最近解锁的场景
        /// </summary>
        public int lastUnLockSceneID = GameConst.First_SceneID;

        /// <summary>
        /// 15版动物数据
        /// </summary>
        public PlayerAnimal_MSS_15 animalMSS15 = new PlayerAnimal_MSS_15();


        public void SetDefault()
        {
            parkingCenterDataList.Clear();
            /*初始化停车场数据结构*/
            SetDefaultParkingCenterData(GameConst.First_SceneID);

            exitGateLevel = 1;
            entryTicketsLevel = 1;
            diamond = Config.globalConfig.getInstace().InitialrmbNumber;
            isSound = true;
            isGuide = false;
            isGuide_CrossRoad = true;
            lastLogoutTime = DateTime.Now.Ticks;
            LastLogingDate_Day = DateTime.Now.Day;
            //coin = Config.globalConfig.getInstace().InitialGoldNumber;
            star = Config.globalConfig.getInstace().InitialStarNumber;
            allZooNumber = 1;
            isShowAnimalCultivate = false;
#if NOVICEGUIDE
            isGuide = true;

#endif
            this.entryGateList_MS.Clear();
            SetDefaultEntryGateData(GameConst.First_SceneID);

            littleZooModuleDatasMSS.Clear();
            //coin = "1000";
            /* 动物栏的数据初始化 */
            SetDefaultlittleZooData(GameConst.First_SceneID);
            playerAnimalDatasMSS.Clear();
            SetDefaultPlayerAnimalData(GameConst.First_SceneID);

            //多货币初始化
            this.playerCoin.SetDefault();
        }

        public void SetDefaultlittleZooData(int sceneID)
        {
            int openLittleZoo = 0;
            int defaultOpenGroup = Config.globalConfig.getInstace().DefaultOpenGroup;
            int defaultLoadGroup = defaultOpenGroup + Config.globalConfig.getInstace().ExtendLoadGroup;
            int defaultOpenLittleZoo = Config.globalConfig.getInstace().DefaultOpenLittleZoo;
#if NO_BIGINT
            defaultLoadGroup = 8;
            defaultOpenLittleZoo = 19;
#endif
            var sortedGroupIDs = GlobalDataManager.GetInstance().logicTableGroup.GetSortedGroupIDs(sceneID);
            for (int i = 0; i < defaultLoadGroup; i++)
            {
                int groupID = sortedGroupIDs[i];
                var cfgLittleZooIDs = GlobalDataManager.GetInstance().logicTableGroup.GetSortedLittleZooIDs(sceneID, groupID);
                for (int j = 0; j < cfgLittleZooIDs.Count; j++)
                {
                    LittleZooModuleDataMSS littleZooModuleData = new LittleZooModuleDataMSS();
                    littleZooModuleDatasMSS.Add(littleZooModuleData);
                    littleZooModuleData.littleZooID = cfgLittleZooIDs[j];

                    littleZooModuleData.sceneID = sceneID;

                    if (openLittleZoo < defaultOpenLittleZoo)
                    {
                        littleZooModuleData.littleZooTicketsLevel = 1;
                        littleZooModuleData.littleZooVisitorSeatLevel = 1;
#if NO_BIGINT
                        littleZooModuleData.littleZooVisitorSeatLevel = 10;
#endif
                        littleZooModuleData.littleZooEnterVisitorSpawnLevel = 1;
                        ++openLittleZoo;
                    }
                    else
                    {
                        littleZooModuleData.littleZooTicketsLevel = 0;
                        littleZooModuleData.littleZooVisitorSeatLevel = 0;
                        littleZooModuleData.littleZooEnterVisitorSpawnLevel = 0;
                    }
                }
            }
        }

        public bool IsExistlittleZooModuleDatas(int sceneID)
        {
            for (int i = 0; i < littleZooModuleDatasMSS.Count; i++)
            {
                var littleZooModuleData = littleZooModuleDatasMSS[i];
                if (littleZooModuleData.sceneID == sceneID)
                {
                    return true;
                }
            }

            return false;
        }
        public bool IsExistPackingModuleDatas(int sceneID)
        {
            for (int i = 0; i < parkingCenterDataList.Count; i++)
            {
                var parkingCenterData = parkingCenterDataList[i];
                if (parkingCenterData.sceneID == sceneID)
                {
                    return true;
                }
            }

            return false;
        }
        public bool IsExistEntryModuleDatas(int sceneID)
        {
            for (int i = 0; i < entryGateList_MS.Count; i++)
            {
                if (entryGateList_MS[i].sceneID == sceneID)
                {
                    return true;
                }
            }

            return false;
        }
        public bool IsExistPlayerAnimalModuleDatas(int sceneID)
        {
            for (int i = 0; i < playerAnimalDatasMSS.Count; i++)
            {
                var playerAnimalData = playerAnimalDatasMSS[i];
                if (playerAnimalData.sceneID == sceneID)
                {
                    return true;
                }
            }

            return false;
        }
        public void SetDefaultParkingCenterData(int sceneID)
        {
            //LogWarp.LogError("   SetDefaultParkingCenterData  " + this.currSceneID);
            ParkingCenterData_MS parkingCenterData = new ParkingCenterData_MS
            {
                parkingSpaceLevel = 1,
                parkingProfitLevel = 1,
                parkingEnterCarSpawnLevel = 1,
                sceneID = sceneID,
            };
            parkingCenterDataList.Add(parkingCenterData);
        }

        public void SetDefaultEntryGateData(int sceneID)
        {
            GlobalDataManager.GetInstance().logicTableEntryGate.AddScene(sceneID);
            var sortGateIDs = GlobalDataManager.GetInstance().logicTableEntryGate.GetSortGateIDs(sceneID);

            numEntryGate = 1;
            GateData entryGateData = new GateData
            {
                entryID = sortGateIDs[0],
                level = 1
            };

            EntryGateData_MS entryGateData_MS = new EntryGateData_MS();
            entryGateData_MS.sceneID = sceneID;
            entryGateData_MS.entryTicketsLevel = 1;
            entryGateData_MS.entryGateList.Add(entryGateData);
            this.entryGateList_MS.Add(entryGateData_MS);

        }

        public void SetDefaultPlayerAnimalData(int sceneID)
        {
            //PlayerAnimal_MSS playerAnimal = new PlayerAnimal_MSS();
            //playerAnimal.InitPlayerAnimalDic(sceneID);
            //playerAnimal.playerAnimalsNumber = 0;
            //playerAnimal.sceneID = sceneID;
            //playerAnimalDatasMSS.Add(playerAnimal);
            animalMSS15 = new PlayerAnimal_MSS_15();
        }

    }
}

