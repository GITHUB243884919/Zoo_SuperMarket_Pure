using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UFrame;
using System.Numerics;

namespace Game.GlobalData
{
    public partial class PlayerData
    {
        private static bool IsSingleSceneDataToMultiSceneData_Parking(PlayerData playerData)
        {
            if (playerData.playerZoo.parkingCenterData.parkingSpaceLevel > 1 ||
                playerData.playerZoo.parkingCenterData.parkingSpaceLevel > 1 ||
                playerData.playerZoo.parkingCenterData.parkingSpaceLevel > 1)
            {
                return true;
            }
            return false;
        }
        private static bool IsSingleSceneDataToMultiSceneData_EntryGate(PlayerData playerData)
        {

            if (playerData.playerZoo.entryTicketsLevel > 1)
            {
                return true;
            }
            var entryGateList = playerData.playerZoo.entryGateList;
            for (int i = 0; i < entryGateList.Count; i++)
            {
                if (entryGateList[i].level>1)
                {
                    return true;
                }
            }
            return false;
        }
        private static bool IsSingleSceneDataToMultiSceneData_Animal(PlayerData playerData)
        {
            var playerAnimal = playerData.playerZoo.playerAnimal;
            for (int i = 0; i < playerAnimal.littleAnimalList.Count; i++)
            {
                if (playerAnimal.littleAnimalList[i].animalLevel>0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 单场景数据迁移到多场景数据
        /// </summary>
        /// <param name="playerData"></param>
        private static void SingleSceneDataToMultiSceneData(PlayerData playerData)
        {
            /*老版本数据迁移*/
            //停车场;
            SingleSceneDataToMultiSceneData_Parking(playerData);
            //售票口
            SingleSceneDataToMultiSceneData_EntryGate(playerData);
            //动物；
            SingleSceneDataToMultiSceneData_Animal(playerData);
        }

        /// <summary>
        /// 多场景数据迁移到多场景多布局
        /// </summary>
        /// <param name="playerData"></param>
        private static void MultiSceneDataToMultiStyleSceneData(PlayerData playerData)
        {
            //动物栏数据迁移
            MultiSceneDataToMultiStyleSceneData_LittleZoo(playerData);

            //动物数据迁移
            MultiSceneDataToMultiStyleSceneData_PlayerAnimal(playerData);

            //任务数据迁移
            MultiSceneDataToMultiStyleSceneData_GuideMission(playerData);

            //场景数据迁移
            MultiSceneDataToMultiStyleSceneData_Scene(playerData);

        }

        /// <summary>
        /// 多场景多布局转多类型金币
        /// </summary>
        /// <param name="playerData"></param>
        private static void MultiStyleSceneDataToMultiCoinSceneData(PlayerData playerData)
        {
            BigInteger oldCoin;
            if (BigInteger.TryParse(playerData.playerZoo.coin, out oldCoin))
            {
                if (oldCoin > 0)
                {
                    playerData.playerZoo.playerCoin.SingleCoinToMultiCoinData(playerData);
                    playerData.playerZoo.coin = "0";
                }
            }
        }

        /// <summary>
        /// 多场景到多布局动物栏数据迁移，
        /// 分析：上版本只开一个岛，新版本也只开一个岛，且新版本中全是10动物栏布局
        /// 迁移目标：仅保留最新group表中第一个岛存在的动物栏id（10）包含的所有动物信息，其他设为默认
        /// </summary>
        /// <param name="playerData"></param>
        private static void MultiSceneDataToMultiStyleSceneData_PlayerAnimal(PlayerData playerData)
        {
            if (!IsMultiSceneDataToMultiStyleSceneData_PlayerAnimal(playerData))
            {
                return;
            }
            var playerAnimalDatasMSS = playerData.playerZoo.playerAnimalDatasMSS;
            playerAnimalDatasMSS.Clear();
            playerData.playerZoo.SetDefaultPlayerAnimalData(GameConst.First_SceneID);

            playerData.playerZoo.playerAnimalDatas.Clear();

        }

        /// <summary>
        /// 多场景到多布局动物栏数据迁移，
        /// 迁移结果：动物栏数据数据等于新账号
        /// </summary>
        /// <param name="playerData"></param>
        private static void MultiSceneDataToMultiStyleSceneData_LittleZoo(PlayerData playerData)
        {
            if (!IsMultiSceneDataToMultiStyleSceneData_LittleZoo(playerData))
            {
                return;
            }

            var littleZooModuleDatasMSS = playerData.playerZoo.littleZooModuleDatasMSS;
            littleZooModuleDatasMSS.Clear();
            playerData.playerZoo.SetDefaultlittleZooData(GameConst.First_SceneID);
            playerData.playerZoo.littleZooModuleDatas.Clear();
        }

        /// <summary>
        /// 多场景任务数据迁移到多布局
        /// 迁移结果：任务数据清空
        /// </summary>
        /// <param name="playerData"></param>
        private static void MultiSceneDataToMultiStyleSceneData_GuideMission(PlayerData playerData)
        {
            if (playerData.playerZoo.guideMissionPlayerData.taskStates.Count != 0 ||
                playerData.playerZoo.guideMissionPlayerData.missionProgress.Count != 0)
            {
                playerData.playerZoo.guideMissionPlayerData.taskStates.Clear();
                playerData.playerZoo.guideMissionPlayerData.missionProgress.Clear();
            }
        }

        /// <summary>
        /// 多场景场景数据迁移到多布局
        /// 迁移结果：场景数据清空
        /// </summary>
        /// <param name="playerData"></param>
        private static void MultiSceneDataToMultiStyleSceneData_Scene(PlayerData playerData)
        {
            if (playerData.playerZoo.scenePlayerData.sceneStates.Count != 0)
            {
                playerData.playerZoo.scenePlayerData.sceneStates.Clear();
            }
        }

        private static bool IsMultiSceneDataToMultiStyleSceneData_PlayerAnimal(PlayerData playerData)
        {
            if (playerData.playerZoo.playerAnimalDatas.Count > 0)
            {
                return true;
            }
            return false;
        }

        private static bool IsMultiSceneDataToMultiStyleSceneData_LittleZoo(PlayerData playerData)
        {
            if (playerData.playerZoo.littleZooModuleDatas.Count > 0)
            {
                return true;
            }
            return false;
        }

        private static void SingleSceneDataToMultiSceneData_Parking(PlayerData playerData)
        {
            if (!IsSingleSceneDataToMultiSceneData_Parking(playerData))
                return;
            ParkingCenterData_MS parkingCenterData_MS = new ParkingCenterData_MS
            {
                parkingEnterCarSpawnLevel = playerData.playerZoo.parkingCenterData.parkingEnterCarSpawnLevel,
                parkingProfitLevel = playerData.playerZoo.parkingCenterData.parkingProfitLevel,
                parkingSpaceLevel = playerData.playerZoo.parkingCenterData.parkingSpaceLevel,
                sceneID = GameConst.First_SceneID,
            };
            playerData.playerZoo.parkingCenterDataList.Clear();
            playerData.playerZoo.parkingCenterDataList.Add(parkingCenterData_MS);
            playerData.playerZoo.parkingCenterData.ClearData();
        }
        private static void SingleSceneDataToMultiSceneData_EntryGate(PlayerData playerData)
        {
            if (!IsSingleSceneDataToMultiSceneData_EntryGate(playerData))
                return;

            List<GateData> gateList = new List<GateData>();
            for (int i = 0; i < playerData.playerZoo.entryGateList.Count; i++)
            {
                GateData gateData = new GateData {
                    entryID = playerData.playerZoo.entryGateList[i].entryID,
                    level = playerData.playerZoo.entryGateList[i].level,
                };
                gateList.Add(gateData);

            }

            EntryGateData_MS entryGateData_MS = new EntryGateData_MS
            {
                entryGateList = gateList,
                entryTicketsLevel = playerData.playerZoo.entryTicketsLevel,
                sceneID = GameConst.First_SceneID
            };
            playerData.playerZoo.entryGateList_MS.Clear();
            playerData.playerZoo.entryGateList_MS.Add(entryGateData_MS);
            playerData.playerZoo.entryTicketsLevel = UFrame.Const.Invalid_Int;
            playerData.playerZoo.entryGateList.Clear();
        }
        private static void SingleSceneDataToMultiSceneData_Animal(PlayerData playerData)
        {
            if (!IsSingleSceneDataToMultiSceneData_Animal(playerData))
                return;
            List<LittleAnimal_MSS> littleAnimalLists = new List<LittleAnimal_MSS>();
            for (int i = 0; i < playerData.playerZoo.playerAnimal.littleAnimalList.Count; i++)
            {
                LittleAnimal_MSS littleAnimal_MSS = new LittleAnimal_MSS();
                littleAnimal_MSS.animalEntityID = playerData.playerZoo.playerAnimal.littleAnimalList[i].animalEntityID;    
                littleAnimal_MSS.animalLevel = playerData.playerZoo.playerAnimal.littleAnimalList[i].animalLevel;
                littleAnimal_MSS.animalState = playerData.playerZoo.playerAnimal.littleAnimalList[i].animalState;
                littleAnimalLists.Add(littleAnimal_MSS);
            }

            PlayerAnimal_MSS playerAnimal = new PlayerAnimal_MSS
            {
                sceneID = GameConst.First_SceneID,
                animalID = playerData.playerZoo.playerAnimal.animalID,
                littleAnimalList = littleAnimalLists,
                playerAnimalsNumber = playerData.playerZoo.playerAnimal.playerAnimalsNumber,
                playerAllAnimalsLevel = playerData.playerZoo.playerAnimal.playerAllAnimalsLevel,
            };
            playerData.playerZoo.playerAnimalDatasMSS.Clear();
            playerData.playerZoo.playerAnimalDatasMSS.Add(playerAnimal);
            playerData.playerZoo.playerAnimal.littleAnimalList.Clear();
        }

        /// <summary>
        /// 把进入次数>0 ,id 大于lastUnLockSceneID的设置成lastUnLockSceneID
        /// </summary>
        /// <param name="playerData"></param>
        public static void SetLastOpenScene(PlayerData playerData)
        {
            var sceneStates = playerData.playerZoo.scenePlayerDataMSS.sceneStates;
            for (int i = 0; i < sceneStates.Count; i++)
            {
                var sceneState = sceneStates[i];
                if (sceneState.enterCount > 0 && sceneState.sceneId > playerData.playerZoo.lastUnLockSceneID)
                {
                    playerData.playerZoo.lastUnLockSceneID = sceneState.sceneId;
                }
            }
        }
    }
}
