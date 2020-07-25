/*******************************************************************
* FileName:     StateLoadPartSceneForGame.cs
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
    /// 按需分块加载场景
    /// </summary>
    public class StateLoadPartSceneForGame : FSMState
    {
        PlayerData playerData { get { return GlobalDataManager.GetInstance().playerData; } }

        bool isToStateLoadAnimal = false;

        int sceneID;
        
        public StateLoadPartSceneForGame(int stateName, FSMMachine fsmCtr) :
            base(stateName, fsmCtr)
        {
        }

        public override void Enter(int preStateName)
        {
            base.Enter(preStateName);

            isToStateLoadAnimal = false;

            sceneID = (fsmCtr as FSMGameLoad).sceneID;
            LoadLittleZoo();
            LoadSceneButton();
            LoadParking();
            //GlobalDataManager.GetInstance().logicTableEntryGate.Init(sceneID);
            GlobalDataManager.GetInstance().logicTableEntryGate.AddScene(sceneID);

            FindEntryUISceneNode();
            SetEntrySceneObject();
            //LoadPlayerAnimal();
            //LogWarp.LogFormat("总地块={0}", loadGroup.Count);
            float value = 1 / 6f;
            (PageMgr.allPages["UILoading"] as UILoading).SliderValueLoading(value);

            isToStateLoadAnimal = true;
        }

        private void LoadPlayerAnimal()
        {
            bool retCode = this.playerData.playerZoo.IsExistPlayerAnimalModuleDatas(sceneID);
            if (!retCode)
            {
                this.playerData.playerZoo.SetDefaultPlayerAnimalData(sceneID);
            }
        }

        /// <summary>
        /// 加载动物栏
        /// </summary>
        protected void LoadLittleZoo()
        {
            var loadGroup = GlobalDataManager.GetInstance().zooGameSceneData.loadGroup;
            GameObject camera = GameObject.Find("Camera");
            GlobalDataManager.GetInstance().zooGameSceneData.camera = camera;
            var littleZooRoot = GameObject.Find("LittleZoo").transform;
            GlobalDataManager.GetInstance().zooGameSceneData.littleZooParentNode = littleZooRoot;
            int littleZooID = Const.Invalid_Int;
            int groupID = Const.Invalid_Int;

            bool retCode = this.playerData.playerZoo.IsExistlittleZooModuleDatas(sceneID);
            if (!retCode)
            {
                this.playerData.playerZoo.SetDefaultlittleZooData(sceneID);
            }

            //var littleZooModuleDatas = this.GetlittleZooModuleDatas(sceneID);
            var littleZooModuleDatas = this.playerData.playerZoo.littleZooModuleDatasMSS;
            //for (int i = 0; i < this.playerData.playerZoo.littleZooModuleDatas.Count; i++)
            for (int i = 0; i < littleZooModuleDatas.Count; i++)
            {
                var littleZooModuleData = this.playerData.playerZoo.littleZooModuleDatasMSS[i];
                if (littleZooModuleData.sceneID != sceneID)
                {
                    continue;
                }
                littleZooID = littleZooModuleData.littleZooID;
                groupID = GlobalDataManager.GetInstance().logicTableGroup.FindGroupID(littleZooID);
                if (!loadGroup.Contains(groupID))
                {
                    loadGroup.Add(groupID);
                    LogWarp.LogFormat("loadGroup {0}", groupID);
                }

                //加载动物栏
                int level = littleZooModuleData.littleZooTicketsLevel;
                var cellBuild = Config.buildupConfig.getInstace().getCell(littleZooID);
                int buildResIdx = LittleZooModule.FindLevelRangIndex(cellBuild.lvmodel, level);

                LittleZooModule.LoadLittleZoo(sceneID, littleZooID, buildResIdx, littleZooRoot, false);
            }
            loadGroup.Sort();


            Config.resourceCell cellRes;
            int idx = 0;
            float offset = Config.globalConfig.getInstace().ZooPartResLen;
            float extendOffset = 0;
            Config.groupCell preCell = null;
            Config.groupCell lastCell = null;
            for (int i = 0; i < loadGroup.Count; i++)
            {
                var cellGroup = Config.groupConfig.getInstace().getCell(loadGroup[i]);

                //加载Group
                if (cellGroup.zoopartresID > 0 && i >= (Config.globalConfig.getInstace().DefaultOpenGroup))
                {
                    cellRes = Config.resourceConfig.getInstace().getCell(cellGroup.zoopartresID);
                    var goPart = ResourceManager.GetInstance().LoadGameObject(cellRes.prefabpath);
                    if (preCell != null)
                    {
                        extendOffset += preCell.groundsize;
                    }
                    goPart.transform.position = new Vector3(goPart.transform.position.x - extendOffset, 0, 0);
                    goPart.name = string.Format("Group_{0}", cellGroup.zoopartresID);
                    ++idx;
                    preCell = cellGroup;
                    lastCell = cellGroup;
                    GlobalDataManager.GetInstance().zooGameSceneData.AddExtendLoadGroup(loadGroup[i], goPart);
                }
            }
            if (lastCell != null)
            {
                extendOffset += lastCell.groundsize;
            }

            LittleZooModule.LoadExitGate(sceneID, idx, extendOffset);
        }

        /// <summary>
        /// 加载场景按钮点击
        /// </summary>
        private void LoadSceneButton()
        {
            UIInteractive.GetInstance().Init(sceneID);
        }

        //protected List<LittleZooModuleData> GetlittleZooModuleDatas(int sceneID)
        //{
        //    var result = new List<LittleZooModuleData>();
        //    var littleZooModuleDatas = this.playerData.playerZoo.littleZooModuleDatas;
        //    for (int i = 0; i < littleZooModuleDatas.Count; i++)
        //    {
        //        var littleZooModuleData = littleZooModuleDatas[i];
        //        if (littleZooModuleData.sceneID == sceneID)
        //        {
        //            result.Add(littleZooModuleData);
        //        }
        //    }

        //    return result;
        //}



        /// <summary>
        /// 加载停车场地块
        /// </summary>
        protected void LoadParking()
        {
            GlobalDataManager.GetInstance().zooGameSceneData.ParckingSencePos = GameObject.Find("ParckingSencePos");
            var allData = Config.parkingConfig.getInstace().AllData;
            Config.parkingCell parkingCell = GlobalDataManager.GetInstance().logicTableParkingData.GetParkingCell(sceneID);

            //var cellBuildUp = Config.parkingConfig.getInstace().getCell(sceneID);
            bool retCode = this.playerData.playerZoo.IsExistPackingModuleDatas(sceneID);
            if (!retCode)
            {
                this.playerData.playerZoo.SetDefaultParkingCenterData(sceneID);
            }
            var parkingSpaceLevel = this.playerData.GetParkingCenterDataIDIndexOfDataIdx(sceneID).parkingSpaceLevel;
            int currResIdx = ParkingCenter.FindLevelRangIndex(parkingCell.openlv, parkingSpaceLevel);
            var cellRes = Config.resourceConfig.getInstace().getCell(parkingCell.openggroup[currResIdx]);
            var parkingModel = ResourceManager.GetInstance().LoadGameObject(cellRes.prefabpath);
            parkingModel.transform.position = new UnityEngine.Vector3(0, 0, 0);
            parkingModel.SetActive(true);
            parkingModel.transform.SetParent(GlobalDataManager.GetInstance().zooGameSceneData.ParckingSencePos.transform, false);
        }

        /// <summary>
        /// 查找售票口的下标
        /// </summary>
        protected void FindEntryUISceneNode()
        {
            if (GlobalDataManager.GetInstance().zooGameSceneData.entryGateSenceData.entryGatesVector.Count < 1)
            {
                var sortGateIDs = GlobalDataManager.GetInstance().logicTableEntryGate.GetSortGateIDs(sceneID);
                for (int i = 0; i < sortGateIDs.Count; i++)
                {
                    var cell = Config.ticketConfig.getInstace().getCell(sortGateIDs[i]);
                    Vector3 vector = GameObject.Find(cell.gameobjectpath).transform.position;
                    GlobalDataManager.GetInstance().zooGameSceneData.entryGateSenceData.entryGatesVector.Add(vector);
                    var simpleParticle = new SimpleParticle();
                    GlobalDataManager.GetInstance().zooGameSceneData.entryGateSenceData.entryCoinSpList.Add(simpleParticle);
                    string path = cell.prohibitroute;

                    Transform transform = GameObject.Find(path).transform;
                    GlobalDataManager.GetInstance().zooGameSceneData.entryGateSenceData.EntrySubscriptGB.Add(transform);
                }
            }
            //if (GlobalDataManager.GetInstance().zooGameSceneData.entryGateSenceData.entryGatesVector.Count < 1)
            //{
            //    ////循环查找所有的出口坐标  放在entryGatesVector
            //    for (int i = 0; i < Config.ticketConfig.getInstace().AllData.Count; i++)
            //    {
            //        var cell = Config.ticketConfig.getInstace().getCell(i);
            //        if (cell.scene == sceneID)
            //        {
            //            Vector3 vector = GameObject.Find(cell.gameobjectpath).transform.position;
            //            GlobalDataManager.GetInstance().zooGameSceneData.entryGateSenceData.entryGatesVector.Add(vector);
            //            var simpleParticle = new SimpleParticle();
            //            GlobalDataManager.GetInstance().zooGameSceneData.entryGateSenceData.entryCoinSpList.Add(simpleParticle);
            //            //LogWarp.LogErrorFormat("测试：AAAAAAAAAAAA   {0}     {1}", i, Config.ticketConfig.getInstace().AllData.Count);

            //            string path = Config.ticketConfig.getInstace().getCell(i).prohibitroute;

            //            Transform transform = GameObject.Find(path).transform;

            //            GlobalDataManager.GetInstance().zooGameSceneData.entryGateSenceData.EntrySubscriptGB.Add(transform);
            //        }

            //    }
            //}
        }

        /// <summary>
        /// 设置售票口部件的显示或隐藏
        /// </summary>
        protected void SetEntrySceneObject()
        {
            bool retCode = this.playerData.playerZoo.IsExistEntryModuleDatas(sceneID);
            if (!retCode)
            {
                this.playerData.playerZoo.SetDefaultEntryGateData(sceneID);
            }
            List<GateData> entryGateList = this.playerData.GetEntryDateDataIDIndexOfDataIdx(sceneID).entryGateList;
            
            int entryNum = entryGateList.Count; 
            for (int i = 0; i < entryNum; i++)
            {
                HideEntryGateForbidGameObject(i);
            }
        }

        /// <summary>
        /// 隐藏开启的售票口对应的禁止牌
        /// </summary>
        /// <param name="number">出口ID</param>
        void HideEntryGateForbidGameObject(int number)
        {
            if (number == 0)
            {
                return;
            }
            GameObject gameObject = GlobalDataManager.GetInstance().zooGameSceneData.entryGateSenceData.EntrySubscriptGB[number].Find("jinzhitongxing").gameObject;
            GameObject gameObject1 = GlobalDataManager.GetInstance().zooGameSceneData.entryGateSenceData.EntrySubscriptGB[number].Find("damen_shoufei").gameObject;

            if (gameObject != null)
            {
                gameObject.SetActive(false);
                gameObject1.SetActive(true);
            }
        }

        public override void Tick(int deltaTimeMS)
        {
        }

        public override void Leave()
        {
            base.Leave();
        }

        public override void AddAllConvertCond()
        {
            AddConvertCond((int)GameLoaderState.LoadAnimalInLittleZoo, ToStateLoadAnimal);
        }

        protected bool ToStateLoadAnimal()
        {
            return isToStateLoadAnimal;
        }
    }

}
