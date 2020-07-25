using Game.MessageCenter;
using System.Collections.Generic;
using UFrame;
using UFrame.MiniGame;
using UnityEngine;
using Game;
using UFrame.Logger;
using System;
using Game.GlobalData;
using UFrame.MessageCenter;

namespace CrossRoadGame
{
    /// <summary>
    /// 加载动物
    /// </summary>
    public class StateLoadAnimal : FSMState
    {
        ///// <summary>
        ///// 所有动物实体列表(按照顺序排队)
        ///// </summary>
        //public static List<EntityCrossRoadAnimal> entityCrossRoadAnimalList;

        public StateLoadAnimal(int stateName, FSMMachine fsmCtr) :
            base(stateName, fsmCtr)
        {
        }

        /// <summary>
        /// 获取各个马路的第一个位置列表 
        /// </summary>
        List<Vector3> animalRoadSegment {
            get {
                return CrossRoadModelManager.GetInstance().roadModel.animalRoadSegment;
            }
        }

        Vector3 standardAnimalBoxSize {
            get {
                return CrossRoadModelManager.GetInstance().standardAnimalBoxSize;
            }
        }

        /// <summary>
        /// 行走动物播放时间  
        /// </summary>
        float animalAnimationSpeed {
            get {
                return CrossRoadStageManager.GetInstance().animalAnimSpeed;
            }
        }

        /// <summary>
        /// 行走动物移动速度
        /// </summary>
        float animalMoveSpeed {
            get {
                return CrossRoadStageManager.GetInstance().animalMoveSpeed;
            }
        }

        float animalAcceleration {
            get {
                return CrossRoadStageManager.GetInstance().animalAcceleration;
            }
        }

        float rotateSpeed {
            get {
                return CrossRoadStageManager.GetInstance().rotateSpeed;
            }
        }

        CrossRoadAnimalTeamModel animalTeamModel {
            get {
                return CrossRoadModelManager.GetInstance().animalTeamModel;
            }
        }

        /// <summary>
        /// 获取当前配置的过马路文件 crossroadstageCell
        /// </summary>
        Config.crossroadstageCell cellStage {
            get {
                int stageID = CrossRoadModelManager.GetInstance().stageID;
                return Config.crossroadstageConfig.getInstace().getCell(stageID);
            }
        }

        public override void Enter(int preStateName)
        {
            base.Enter(preStateName);
            MessageManager.GetInstance().Regist((int)GameMessageDefine.SetCrossRoadAnimalObjectData, this.OnLoadAnimal);
            OnLoadAnimal(null);

            WhenLoadFinish();
        }

        protected void OnLoadAnimal(Message msg)
        {
            animalTeamModel.numArrivedEndPoint = 0;

            List<int> animalResourceloadList = GetAnimaelList(cellStage.animalnum);
            for (int i = 0; i < cellStage.animalnum; i++)
            {
                var animal = EntityManager.GetInstance().GenEntityGameObject(
                    animalResourceloadList[i], EntityFuncType.Animal_LittleGame) as EntityCrossRoadAnimal;
                //var animal = EntityManager.GetInstance().GenEntityGameObject(
                //    2201, EntityFuncType.Animal_LittleGame) as EntityCrossRoadAnimal;
                //LogWarp.LogErrorFormat("     {0}",animal.mainGameObject.name);
                animal.Init(animalAnimationSpeed, animalMoveSpeed, i,
                    animalAcceleration, rotateSpeed);
                //LogWarp.LogErrorFormat("测试：动物赋值   {0}        {1}        {2}       {3}       {4}  ", animalAnimationSpeed, animalMoveSpeed, i, animalAcceleration, rotateSpeed);
                animal.position = animalRoadSegment[animalTeamModel.currentRoad]
                    + Vector3.back * (standardAnimalBoxSize.z * i + standardAnimalBoxSize.z * 0.8f);
                //animal.position = animalRoadSegment[animalTeamModel.currentRoad]
                //    + Vector3.back * (standardAnimalBoxSize.z * i);
                GameObject colliderGB = animal.GetTrans().Find("Collider").gameObject;
                colliderGB.SetActive(true);
                FSMCrossRoadGame.Scale_Z(animal.mainGameObject, standardAnimalBoxSize.z);
                animal.position = new Vector3(animal.position.x, 0, animal.position.z);
                if (animal.simpleAnimation != null)
                {
                    animal.simpleAnimation.Init(animal.mainGameObject);
                }

                if (colliderGB.GetComponent<Rigidbody>() == null)
                {
                    colliderGB.AddComponent<Rigidbody>();
                    SetRigidbody(colliderGB);
                }

                if (colliderGB.GetComponent<AnimalCollisionEnterHelp>() == null)
                {
                    colliderGB.AddComponent<AnimalCollisionEnterHelp>();
                }

                if (animal.runEffSp == null)
                {
                    animal.runEffSp = new SimpleParticle();
                    animal.runEffSp.Init(animal.mainGameObject.transform.Find("Effect/MoveEffect").gameObject);
                }
                
                animal.Active();
                animalTeamModel.entityCrossRoadAnimalList.Add(animal);
                CrossRoadModelManager.GetInstance().entityModel.AddToEntityMovables(animal);
            }
        }
        private List<int> GetAnimaelList(int number)
        {
            PlayerData playerData = GlobalDataManager.GetInstance().playerData;
            /*获取动物列表  随机存放*/
            List<int> animalResourceloadList = new List<int>();

            if (playerData.playerLittleGame.isFirst)
            {
                var gameranksCell = Config.gameranksConfig.getInstace().getCell(1);

                for (int i = 0; i < gameranksCell.ranksanimalid.Length; i++)
                {
                    int resourceload = Config.animalupConfig.getInstace().getCell(gameranksCell.ranksanimalid[i]).resourceload;
                    animalResourceloadList.Add(resourceload);
                }
            }
            else
            {
                int stageID = GlobalDataManager.GetInstance().playerData.playerLittleGame.stageID;
                var gameranksCell = Config.gameranksConfig.getInstace().getCell(stageID);

                for (int i = 0; i < gameranksCell.ranksanimalid.Length; i++)
                {
                    int resourceload = Config.animalupConfig.getInstace().getCell(gameranksCell.ranksanimalid[i]).resourceload;
                    animalResourceloadList.Add(resourceload);
                }
            }
            
            return animalResourceloadList;
        }



        /// <summary>
        /// 设置刚体属性
        /// </summary>
        private void SetRigidbody(GameObject gameObject)
        {
            Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
            rigidbody.useGravity = false;
            rigidbody.isKinematic = false;
            rigidbody.mass = 1;
            rigidbody.angularDrag = 0f;
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
        }

        public override void Tick(int deltaTimeMS)
        {
            //        Debug.DrawLine(CrossRoadModelManager.GetInstance().spLBW,
            //CrossRoadModelManager.GetInstance().spLTW, Color.red);
            //        Debug.DrawLine(CrossRoadModelManager.GetInstance().spLTW,
            //            CrossRoadModelManager.GetInstance().spRTW, Color.red);
            //        Debug.DrawLine(CrossRoadModelManager.GetInstance().spRTW,
            //            CrossRoadModelManager.GetInstance().spRBW, Color.red);
            //        Debug.DrawLine(CrossRoadModelManager.GetInstance().spRBW,
            //            CrossRoadModelManager.GetInstance().spLBW, Color.red);
        }

        public override void Leave()
        {

            base.Leave();
        }

        public override void AddAllConvertCond()
        {
        }

        void WhenLoadFinish()
        {
            (this.fsmCtr as FSMCrossRoadGame).SetLoadingPageSlider();
            PageMgr.ClosePage((this.fsmCtr as FSMCrossRoadGame).loadingPageName);
            MessageManager.GetInstance().Send((int)GameMessageDefine.LoadCrossRoadLevelFinished);
        }
    }
}
