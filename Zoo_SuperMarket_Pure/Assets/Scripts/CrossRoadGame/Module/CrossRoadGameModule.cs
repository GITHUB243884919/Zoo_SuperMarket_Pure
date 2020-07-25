//using DG.Tweening;
//using Game;
//using Game.MessageCenter;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UFrame;
//using UFrame.Logger;
//using UFrame.MessageCenter;
//using UnityEngine;

//namespace CrossRoadGame
//{
//    public partial class CrossRoadGameModule : GameModule
//    {
//        public CrossRoadGameModule(int orderID) : base(orderID) { }

//        /// <summary>
//        /// 每条马路的第一个位置列表
//        /// </summary>
//        List<Vector3> crossRoadList {
//            get {
//                return CrossRoadModelManager.GetInstance().roadModel.animalRoadSegment;
//            }
//        }

//        /// <summary>
//        /// 获取当前配置的过马路文件 crossroadstageCell
//        /// </summary>
//        Config.crossroadstageCell cellStage {
//            get {
//                int stageID = CrossRoadModelManager.GetInstance().stageID;
//                return Config.crossroadstageConfig.getInstace().getCell(stageID);
//            }
//        }

//        // Start is called before the first frame update
//        public override void Init()
//        {
           
//        }
//        public override void Release()
//        {
//            this.Stop();
            
//        }
//        public override void Tick(int deltaTimeMS)
//        {
//            if (!this.CouldRun())
//            {
//                return;
//            }

//        }

//        int currentRoad = 0;

       


//        /// <summary>
//        /// 收到最后一个动物完成某个过马路的消息
//        /// </summary>
//        /// cellStage.roadnum > idx    过完单条马路
//        /// cellStage.roadnum == idx   需要过最后一条马路，到达终点
//        ///                            全部过完   结算界面
//        /// <param name="obj"></param>
//        private void OnGetCrossRoadGameSingleRoadSucceed(Message obj)
//        {

//            int idx = currentRoad + 1;
//            if (cellStage.roadnum > idx)
//            {
//                //LogWarp.LogErrorFormat("测试：  当前马路条数   {0}", currentRoad);
//                CrossRoadCameraController.GetInstance().MoveCamera(crossRoadList[currentRoad]);
//                var ve = GetMovetArgetVector3s(idx);
//                for (int i = 0; i < cellStage.animalnum; i++)
//                {
//                    AnimalDataModule.entityCrossRoadAnimalList[i].SetEntityInit(ve[i]);
//                    LogWarp.LogErrorFormat("测试：     测试 动物{0}   现在位置{1}  目标位置{2}",i, AnimalDataModule.entityCrossRoadAnimalList[i].startPos, AnimalDataModule.entityCrossRoadAnimalList[i].endPos);
//                }
//            }
//            else if (cellStage.roadnum == idx)
//            {
//                //LogWarp.LogErrorFormat("测试：  需要到达终点");
//                var endAnimalPos = CrossRoadModelManager.GetInstance().endAnimalPos;
//                var endAnimalTransferPos = CrossRoadModelManager.GetInstance().endAnimalTransferPos;
//                CrossRoadCameraController.GetInstance().MoveCamera(crossRoadList[currentRoad]);

//                for (int i = 0; i < cellStage.animalnum; i++)
//                {
//                    AnimalDataModule.entityCrossRoadAnimalList[i].SetEntityInit( endAnimalTransferPos, endAnimalPos[i]);
//                }
//            }
//            else
//            {
//                LogWarp.LogErrorFormat("测试：  关卡完成 ");
//                var endAnimalPos = CrossRoadModelManager.GetInstance().endAnimalPos;
//                CrossRoadCameraController.GetInstance().MoveCamera(endAnimalPos[0]);
//                float timeCount = 0.1f;
//                DOTween.To(() => timeCount, a => timeCount = a, 0.1f, 1f).OnComplete(new TweenCallback(delegate
//                {
//                    PageMgr.ShowPage<UIGameVictoryPage>();
//                }));
//            }

//            currentRoad = idx;
//        }
//    }
//}
