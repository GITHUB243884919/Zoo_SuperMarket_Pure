using Game.MessageCenter;
using UFrame.Logger;
using System;
using System.Collections;
using System.Collections.Generic;
using UFrame;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 动物栏动物数量类
    /// <动物ID,动物数量>
    /// </summary>
    [Serializable]
    public class LittleAnimal_MSS
    {
        /// <summary>
        /// AnimalState状态
        /// </summary>
        public AnimalState animalState = AnimalState.NoneOwn;
        /// <summary>
        /// 动物等级
        /// </summary>
        public int animalLevel = 0;
        /// <summary>
        /// 动物的模型ID
        /// </summary>
        public int animalEntityID = 0;

        public LittleAnimal_MSS()
        {
        }
        public LittleAnimal_MSS(AnimalState state, int level = 0)
        {
            this.animalState = state;
            this.animalLevel = level;
        }
    }
    [Serializable]
    public class PlayerAnimal_MSS
    {
        /// <summary>
        /// 所属场景
        /// </summary>
        public int sceneID = GameConst.First_SceneID;

        /// <summary>
        /// 动物对应的ID
        /// </summary>
        public List<string> animalID = new List<string>();
        /// <summary>
        /// 动物ID对应的的动物数据类
        /// </summary>
        public List<LittleAnimal_MSS> littleAnimalList = new List<LittleAnimal_MSS>();

        /// <summary>
        /// 当前场景玩家拥有的动物数量
        /// </summary>
        public int playerAnimalsNumber;

        /// <summary>
        /// 玩家拥有的所有动物等级和
        /// </summary>
        public int playerAllAnimalsLevel;


        public PlayerAnimal_MSS()
        {
            playerAnimalsNumber = 0;
            sceneID = GameConst.First_SceneID;
            //InitPlayerAnimalDic(sceneID);

        }

        /// <summary>
        /// 初始化玩家的playerAnimalDic
        /// </summary>
        public void InitPlayerAnimalDic(int sceneID)
        {
            //LogWarp.LogError("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            List<int> LittleZooIDs = new List<int>();
            var groupData = Config.groupConfig.getInstace().AllData;
            foreach (var item in groupData)
            {
                if (item.Value.scene == sceneID)
                {
                    for (int i = 0; i < item.Value.startid.Length; i++)
                    {
                        LittleZooIDs.Add(item.Value.startid[i]);
                    }
                }
            }

            foreach (var item in LittleZooIDs)
            {
                var cellBuildupData = Config.buildupConfig.getInstace().getCell(item);

                for (int i = 0; i < cellBuildupData.animalid.Length; i++)
                {
                    string id = cellBuildupData.animalid[i].ToString();
                    LittleAnimal_MSS littleAnimal = new LittleAnimal_MSS
                    {
                        animalLevel = 0,
                        animalState = AnimalState.NoneOwn,
                        animalEntityID = 0,
                    };

                    animalID.Add(id);
                    littleAnimalList.Add(littleAnimal);
                }


            }
        }

        /// <summary>
        /// 修改玩家动物为解锁状态
        /// </summary>
        /// <param name="id">动物ID</param>
        public void SetPlayerAnimalDataStayOpen(int id)
        {
            LittleAnimal_MSS littleAnimal = getPlayerAnimalCell(id);
            littleAnimal.animalState = AnimalState.StayOpen;
        }

        /// <summary>
        /// 修改玩家动物为可升级状态
        /// </summary>
        /// <param name="id"></param>
        public void SetPlayerAnimalDataAlreadyOpen(int id, int entityID=0)
        {
            var LittleAnimal = getPlayerAnimalCell(id);
            if (LittleAnimal.animalLevel > 0)
            {
                return;
            }
            else
            {   //新开启的动物显示在动物栏
                LittleAnimal.animalLevel = 1;
                LittleAnimal.animalState = AnimalState.AlreadyOpen;
                LittleAnimal.animalEntityID = entityID;
                playerAnimalsNumber += 1;
                playerAllAnimalsLevel += 1;
            }
            MessageInt.Send((int)GameMessageDefine.SetAnimalAtlasDataMessage, id);

        }

        /// <summary>
        /// 修改玩家动物的等级
        /// </summary>
        /// <param name="id"></param>
        public void SetPlayerAnimalLevelData(int id)
        {
            LittleAnimal_MSS littleAnimal = getPlayerAnimalCell(id);
            littleAnimal.animalLevel += 1;
            playerAllAnimalsLevel += 1;
        }

        public int GetAnimalEntityID(int animalID)
        {
            var littleAnimal = getPlayerAnimalCell(animalID);
            return littleAnimal.animalEntityID;
        }

        /// <summary>
        /// 获取ID对应的动物数据
        /// </summary>
        /// <param name="key">动物ID</param>
        /// <returns></returns>
        public LittleAnimal_MSS getPlayerAnimalCell(string key)
        {
            LittleAnimal_MSS t = null;
            int idx = animalID.IndexOf(key);
            t = this.littleAnimalList[idx];
            return t;
        }

        /// <summary>
        /// 获取ID对应的动物数据
        /// </summary>
        /// <param name="key">动物ID</param>
        /// <returns></returns>
        public LittleAnimal_MSS getPlayerAnimalCell(int key)
        {
            LittleAnimal_MSS t = null;
            int idx = animalID.IndexOf(key.ToString());

            t = this.littleAnimalList[idx];
            return t;
        }
        public void ClearData()
        {
            sceneID = GameConst.First_SceneID;
            animalID.Clear();
            littleAnimalList.Clear();
            playerAnimalsNumber = -1;
            playerAllAnimalsLevel = -1;
        }
    }
}
