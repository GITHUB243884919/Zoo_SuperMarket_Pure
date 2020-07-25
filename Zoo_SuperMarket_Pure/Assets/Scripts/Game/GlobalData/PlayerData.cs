using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UFrame;
using UFrame.Logger;

namespace Game.GlobalData
{
    [Serializable]
    public partial class PlayerData
    {
        public bool isFirst = false;
        public PlayerZoo playerZoo;
        public PlayerLittleGame playerLittleGame;
        /// <summary>
        /// 是否是测试离线UI
        /// </summary>
        public bool isTestOfflineRewardPage = false;
        public int isTestOfflineRewardTime = UFrame.Const.Invalid_Int;
        public ParkingCenterData_MS GetParkingCenterDataIDIndexOfDataIdx(int sceneID = -1)
        {
            if (sceneID == -1)
            {
                sceneID = GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID;
            }
            ParkingCenterData_MS parkingCenterData = null;
            var parkingCenterDataList = GlobalDataManager.GetInstance().playerData.playerZoo.parkingCenterDataList;
            for (int i = 0; i < parkingCenterDataList.Count; i++)
            {
                parkingCenterData = parkingCenterDataList[i];
                if (parkingCenterData.sceneID == sceneID)
                {
                    return parkingCenterData;
                }
            }
            string e = string.Format("场景为{0}的停车场数据为空", sceneID);
            throw new System.Exception(e);
            return null;
        }
        /// <summary>
        /// 获取当前场景在多数据列表里面的下标
        /// </summary>
        /// <returns></returns>
        public int GetSceneIDGoToEntryDataListSubscript()
        {
            var entryGateList_MS = GlobalDataManager.GetInstance().playerData.playerZoo.entryGateList_MS;
            int sceneID = GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID;

            for (int i = 0; i < entryGateList_MS.Count; i++)
            {
                if (entryGateList_MS[i].sceneID == sceneID)
                {
                    return i;
                }
            }
            return 0;
        }
        /// <summary>
        /// 获取多场景数据结构中的当前售票口数据Data
        /// </summary>
        /// <returns></returns>
        public EntryGateData_MS GetEntryDateDataIDIndexOfDataIdx(int sceneID = -1)
        {
            if (sceneID == -1)
            {
                sceneID = GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID;
            }
            EntryGateData_MS entryGateData_MS = null;
            var entryGateList_MS = GlobalDataManager.GetInstance().playerData.playerZoo.entryGateList_MS;

            for (int i = 0; i < entryGateList_MS.Count; i++)
            {
                entryGateData_MS = entryGateList_MS[i];
                if (entryGateData_MS.sceneID == sceneID)
                {
                    return entryGateData_MS;
                }
            }

            string e = string.Format("场景为{0}的售票口数据为空", sceneID);
            throw new System.Exception(e);
            return null;
        }


        /// <summary>
        /// 获取多场景的ID售票口对应的数据
        /// </summary>
        /// <param name="entryID"></param>
        /// <returns></returns>
        public GateData GetEntryGateIDIndexOfDataIdx(int entryID)
        {
            GateData entryGate = null;
            var entryGateList = GetEntryDateDataIDIndexOfDataIdx().entryGateList;
            for (int i = 0; i < entryGateList.Count; i++)
            {
                if (entryGateList[i].entryID == entryID)
                {
                    entryGate = entryGateList[i];
                    return entryGate;
                }
            }
            string e = string.Format("售票口ID在用户数据中没找到{0}id", entryID);
            throw new System.Exception(e);
            return null;
        }

        public int GetLittleZooIDIndexOfDataIdx(int littleZooID)
        {
            int number = Const.Invalid_Int;
            var littleZooModuleDatas = GlobalDataManager.GetInstance().playerData.playerZoo.littleZooModuleDatasMSS;
            for (int i = 0; i < littleZooModuleDatas.Count; i++)
            {
                if (littleZooModuleDatas[i].littleZooID == littleZooID)
                {
                    number = i;
                }
            }
            if (number < 0)
            {
                string e = string.Format("动物栏ID在用户数据中没找到{0}", littleZooID);
                throw new System.Exception(e);
            }
            return number;
        }

        public bool GetOpenedLittleZooIDIndexOfData(int littleZooID, ref int idx)
        {
            idx = Const.Invalid_Int;
            var littleZooModuleDatas = GlobalDataManager.GetInstance().playerData.playerZoo.littleZooModuleDatasMSS;
            for (int i = 0; i < littleZooModuleDatas.Count; i++)
            {
                var littleZooModuleData = littleZooModuleDatas[i];
                if (littleZooModuleData.littleZooID == littleZooID && littleZooModuleData.littleZooTicketsLevel > 0)
                {
                    idx = i;
                    return true;
                }
            }

            return false;
        }


        public LittleZooModuleDataMSS GetLittleZooModuleData(int littleZooID)
        {
            int idx = GlobalDataManager.GetInstance().playerData.GetLittleZooIDIndexOfDataIdx(littleZooID);
            if (idx < 0)
            {
                string e = string.Format("动物栏ID在用户数据中没找到{0}", littleZooID);
                throw new System.Exception(e);
            }
            return playerZoo.littleZooModuleDatasMSS[idx];
        }

        /// <summary>
        /// 获取没有开启的动物栏ID
        /// </summary>
        /// <returns></returns>
        public int GetFirstUnopenLittleZooID()
        {
            for(int i = 0; i < playerZoo.littleZooModuleDatasMSS.Count; i++)
            {
                if (playerZoo.littleZooModuleDatasMSS[i].littleZooTicketsLevel == 0
                    && playerZoo.littleZooModuleDatasMSS[i].sceneID == playerZoo.currSceneID)
                {
                    return playerZoo.littleZooModuleDatasMSS[i].littleZooID;
                }
            }

            return UFrame.Const.Invalid_Int;
        }

        /// <summary>
        /// 获取当前场景的动物数据
        /// </summary>
        /// <returns></returns>
        public PlayerAnimal_MSS GetPlayerAnimalData()
        {
            var playerAnimalDatas = playerZoo.playerAnimalDatasMSS;
            PlayerAnimal_MSS playerAnimal = null;
            for (int i = 0; i < playerAnimalDatas.Count; i++)
            {
                if (playerAnimalDatas[i].sceneID == playerZoo.currSceneID)
                {
                    playerAnimal = playerAnimalDatas[i];
                }
            }
            return playerAnimal;
        }

        /// <summary>
        /// 获取senceID场景的动物数据
        /// </summary>
        /// <param name="senceID"></param>
        /// <returns></returns>
        public PlayerAnimal_MSS GetPlayerAnimalData(int senceID)
        {
            if (senceID ==-1)
            {
                senceID = playerZoo.currSceneID;
            }
            var playerAnimalDatas = playerZoo.playerAnimalDatasMSS;
            PlayerAnimal_MSS playerAnimal = null;
            for (int i = 0; i < playerAnimalDatas.Count; i++)
            {
                if (playerAnimalDatas[i].sceneID == senceID)
                {
                    playerAnimal = playerAnimalDatas[i];
                    break;
                }
            }
            return playerAnimal;
        }

        public void Logout()
        {
            playerZoo.lastLogoutTime = DateTime.Now.Ticks;
        }

        /// <summary>
        /// 获取离线时间
        /// .Ticks 得到的值是自公历 0001-01-01 00:00:00:000 至此的以 100 ns（即 1/10000 ms）为单位的时间数。
        /// </summary>
        /// <returns></returns>
        public double GetOfflineSecond()
        {
            long realOffline = (DateTime.Now.Ticks - playerZoo.lastLogoutTime) / 10000000;
            if (isTestOfflineRewardPage)
            {
                realOffline = isTestOfflineRewardTime;
            }
            //XXX秒内离线不算离线
            if (realOffline <= Config.globalConfig.getInstace().MinOfflineSecond)
            {
                return 0;
            }
            long realOfflineTime = realOffline - Config.globalConfig.getInstace().MinOfflineSecond;
            
            return realOfflineTime;
        }

        public double GetRealOfflineSecond()
        {
            long realOffline = (DateTime.Now.Ticks - playerZoo.lastLogoutTime) / 10000000;
            return realOffline;
        }

        public static PlayerData Load()
        {
            LogWarp.Log("PlayerData.Load");
            var playerData = GlobalDataManager.GetInstance().playerData;
            if (playerData == null)
            {
                LogWarp.Log("PlayerData.LoadFromPlayerPrefs");
                playerData = LoadFromPlayerPrefs();
                GlobalDataManager.GetInstance().playerData = playerData;
            }

            //playerData.playerZoo.currSceneID = GetcurrSceneIDByStar(playerData);

            //单场景数据迁移到多场景数据
            SingleSceneDataToMultiSceneData(playerData);

            //多场景数据迁移到多场景多布局
            MultiSceneDataToMultiStyleSceneData(playerData);

            //多场景多布局迁移到多种金币
            MultiStyleSceneDataToMultiCoinSceneData(playerData);

            //设置最后解锁的场景
            SetLastOpenScene(playerData);

            NewAnimalDataToPlayerAnimal_MSS_15(playerData);

            return playerData;
        }

        private static void NewAnimalDataToPlayerAnimal_MSS_15(PlayerData playerData)
        {
            var playerAnimalDatasMSS = playerData.playerZoo.playerAnimalDatasMSS;
            var animalMSS15 = playerData.playerZoo.animalMSS15;
            if (playerAnimalDatasMSS.Count>0)
            {
                foreach (var item in playerAnimalDatasMSS)
                {
                    for (int i = 0; i < item.animalID.Count; i++)
                    {
                        if (item.littleAnimalList[i].animalLevel>0)
                        {
                            int animalID = int.Parse(item.animalID[i]);
                            var cellAnimalUp = Config.animalupConfig.getInstace().getCell(animalID);

                            bool isShow = animalMSS15.FindAnimalProp(cellAnimalUp.bigtype, cellAnimalUp.smalltype, out AnimalProp animalProp);
                            if (!isShow)
                            {
                                animalMSS15.AddAnimal(animalID,true);

                            }
                            else if (isShow && !animalMSS15.animalIDs.Contains(animalID))
                            {
                                animalMSS15.AddAnimal(animalID, false);
                            }
                        }
                    }
                }

                playerAnimalDatasMSS.Clear();
            }
        }

        public static int GetcurrSceneIDByStar(PlayerData playerData)
        {
            var sceneDataList = GlobalDataManager.GetInstance().logicTableSceneData.sceneDataList;

            for (int i = 0; i < sceneDataList.Count; i++)
            {
                if (playerData.playerZoo.star < sceneDataList[i].openStar)
                {

                    return sceneDataList[i - 1].sceneID;
                }
            }

            return playerData.playerZoo.currSceneID;
        }

        protected static PlayerData LoadFromPlayerPrefs()
        {
            PlayerData pd = null;
            string str = PlayerPrefs.GetString("PlayerData");
            bool isFirst = false;
            if (string.IsNullOrEmpty(str))
            {
                isFirst = true;
                var playerData = new PlayerData();
                playerData.playerZoo = new PlayerZoo();
                playerData.playerZoo.SetDefault();
                //Logger.LogWarp.LogErrorFormat("playerData.playerZoo.isGuide {0}",
                    //playerData.playerZoo.isGuide);
                Save(playerData);
                str = PlayerPrefs.GetString("PlayerData");
                if (string.IsNullOrEmpty(str))
                {
                    string e = string.Format("取本地数据PlayerData异常");
                    throw new System.Exception(e);
                }
            }

            pd = JsonUtility.FromJson<PlayerData>(str);
            if (pd == null)
            {
                pd = new PlayerData();
                pd.playerZoo.SetDefault();
                pd.isFirst = false;
                if (isFirst)
                {
                    pd.isFirst = true;
                }
                return pd;
            }

            pd.playerZoo.playerCoin.StrToBigint();

            pd.isFirst = false;
            if (isFirst)
            {
                pd.isFirst = true;
            }
            if (pd.playerZoo.entryGateList_MS == null)
            {
                pd.playerZoo.entryGateList_MS = new List<EntryGateData_MS>();
            }
            if (pd.playerZoo.entryGateList_MS.Count == 0)
            {
                pd.playerZoo.SetDefaultEntryGateData(GameConst.First_SceneID);
            }

            if (pd.playerZoo.littleZooModuleDatasMSS == null)
            {
                pd.playerZoo.littleZooModuleDatasMSS = new List<LittleZooModuleDataMSS>();
            }
            if (pd.playerZoo.littleZooModuleDatasMSS.Count == 0)
            {
                pd.playerZoo.SetDefaultlittleZooData(GameConst.First_SceneID);
            }


            if (pd.playerZoo.parkingCenterDataList == null)
            {
                pd.playerZoo.parkingCenterDataList = new List<ParkingCenterData_MS>();
            }
            if (pd.playerZoo.parkingCenterDataList.Count == 0)
            {
                pd.playerZoo.SetDefaultParkingCenterData(GameConst.First_SceneID);
            }

            if (pd.playerZoo.playerAnimalDatasMSS == null)
            {
                pd.playerZoo.playerAnimalDatasMSS = new List<PlayerAnimal_MSS>();
            }
            //if (pd.playerZoo.playerAnimalDatasMSS.Count == 0)
            //{
            //    pd.playerZoo.SetDefaultPlayerAnimalData(GameConst.First_SceneID);
            //}


            return pd;
        }

        public static void Save(PlayerData playerData)
        {
            string str = JsonUtility.ToJson(playerData);
            if (string.IsNullOrEmpty(str))
            {
                string e = string.Format("存本地数据PlayerData异常");
                throw new System.Exception(e);
            }
            PlayerPrefs.SetString("PlayerData", str);
        }
    }
}

