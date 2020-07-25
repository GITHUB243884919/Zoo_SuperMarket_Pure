//using Game;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UFrame.Logger;
//using UFrame;
//using Game.MessageCenter;
//using System;
//using UFrame.MessageCenter;
//using DG.Tweening;

//namespace CrossRoadGame
//{
//    public class AnimalDataModule : GameModule
//    {
//        /// <summary>
//        /// 当前第几条马路
//        /// </summary>
//        int currentRoad = 0;

//        /// <summary>
//        /// 当前小游戏未过马路的实体列表
//        /// </summary>
//        public List<EntityCrossRoadAnimal> needCrossRoadAnimalList = new List<EntityCrossRoadAnimal>();

//        /// <summary>
//        /// 所有动物实体列表(按照顺序排队)
//        /// </summary>
//        public static List<EntityCrossRoadAnimal> entityCrossRoadAnimalList = new List<EntityCrossRoadAnimal>();

//        /// <summary>
//        /// 获取当前配置的过马路文件 crossroadstageCell
//        /// </summary>
//        Config.crossroadstageCell cellStage {
//            get {
//                int stageID = CrossRoadModelManager.GetInstance().stageID;
//                return Config.crossroadstageConfig.getInstace().getCell(stageID);
//            }
//        }

//        /// <summary>
//        /// 每条马路的第一个位置列表
//        /// </summary>
//        List<Vector3> crossRoadList {
//            get {
//                return CrossRoadModelManager.GetInstance().roadModel.animalRoadSegment;
//            }
//        }

//        public AnimalDataModule(int orderID) : base(orderID) { }
     
//        public override void Init()
//        {
//            MessageManager.GetInstance().Regist((int)GameMessageDefine.BroadcastForwardOneStepIRoad, this.OnBroadcastForwardOneStepIRoad);

//            MessageManager.GetInstance().Regist((int)GameMessageDefine.CrossRoadGameSingleRoadSucceed, this.OnGetCrossRoadGameSingleRoadSucceed);
//        }

//        public override void Release()
//        {
//        }
//        public override void Tick(int deltaTimeMS)
//        {
//            if (!CouldRun())
//            {
//                return;
//            }
//        }
//        #region 方法消息

//        /// <summary>
//        /// 监听小游戏动物移动的消息
//        /// </summary>
//        /// <param name="obj"></param>
//        private void OnBroadcastForwardOneStepIRoad(Message obj)
//        {
//            if (needCrossRoadAnimalList.Count == 0)
//            {
//                AnimalDataModule.entityCrossRoadAnimalList.ForEach(i => needCrossRoadAnimalList.Add(i));
//            }
//            //若有一个不是准备状态   取消事件
//            foreach (var item in needCrossRoadAnimalList)
//            {
//                if (item.entityAnimalStare != EntityAnimalStare.ReadyCondition)
//                    return;
//            }
//            //将第一个设置为过马路状态、其他为前进一步状态
//            for (int i = 0; i < needCrossRoadAnimalList.Count; i++)
//            {
//                if (i == 0)
//                    needCrossRoadAnimalList[0].TestAnimalMove();
//                else
//                    needCrossRoadAnimalList[i].OnBroadcastForwardOneStepInExitGateQueue();
//            }
//            //设置完后剔除过完马路的实体
//            needCrossRoadAnimalList.Remove(needCrossRoadAnimalList[0]);
//        }

//        /// <summary>
//        /// 动物单条马路成功的方法
//        /// </summary>
//        /// <param name="obj"></param>
//        private void OnGetCrossRoadGameSingleRoadSucceed(Message obj)
//        {   /*过马路成功的可能性
//            需要过非最后一个马路
//            需要过最后一个马路
//            完成游戏，成功
//             */
//            currentRoad += 1;
//            if (cellStage.roadnum > currentRoad + 1)
//            {
//                CrossRoadCameraController.GetInstance().MoveCamera(crossRoadList[currentRoad]);
//                var ve = CrossRoadGameModule.GetMovetArgetVector3s(currentRoad);
//                for (int i = 0; i < cellStage.animalnum; i++)
//                {
//                    AnimalDataModule.entityCrossRoadAnimalList[i].SetEntityInit(ve[i]);
//                }
//            }
//            else if (cellStage.roadnum == currentRoad + 1)
//            {
//                var endAnimalPos = CrossRoadModelManager.GetInstance().endAnimalPos;
//                var endAnimalTransferPos = CrossRoadModelManager.GetInstance().endAnimalTransferPos;
//                CrossRoadCameraController.GetInstance().MoveCamera(crossRoadList[currentRoad]);

//                for (int i = 0; i < cellStage.animalnum; i++)
//                {
//                    AnimalDataModule.entityCrossRoadAnimalList[i].SetEntityInit(endAnimalTransferPos, endAnimalPos[i]);
//                }
//            }
//            else
//            {
//                var endAnimalPos = CrossRoadModelManager.GetInstance().endAnimalPos;
//                CrossRoadCameraController.GetInstance().MoveCamera(endAnimalPos[0]);
//                float timeCount = 0.1f;
//                currentRoad = 0;

//                DOTween.To(() => timeCount, a => timeCount = a, 0.1f, 1f).OnComplete(new TweenCallback(delegate
//                {
//                    PageMgr.ShowPage<UIGameVictoryPage>();
//                }));
//            }
//        }
//        #endregion

//    }
//}
