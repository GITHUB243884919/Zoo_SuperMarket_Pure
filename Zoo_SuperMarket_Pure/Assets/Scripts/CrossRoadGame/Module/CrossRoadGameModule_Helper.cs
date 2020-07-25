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
//        /// <summary>
//        /// 获取要移动的目标位置
//        /// </summary>
//        /// <param name="idx"></param>
//        /// <returns></returns>
//        public static List<Vector3> GetMovetArgetVector3s(int idx)
//        {
//            Vector3 vector = new Vector3();
//            List<Vector3> targetLocationList = new List<Vector3>();
//            int stageID = CrossRoadModelManager.GetInstance().stageID;
//            var cellStage = Config.crossroadstageConfig.getInstace().getCell(stageID);
//            var animalRoadSegment = CrossRoadModelManager.GetInstance().roadModel.animalRoadSegment;
//            var standardAnimalBoxSize = CrossRoadModelManager.GetInstance().standardAnimalBoxSize;
//            //float animalPosOffsetZ = CrossRoadStageManager.GetInstance().animalPosOffsetZ;
//            vector = animalRoadSegment[idx + 1];

//            for (int i = 0; i < cellStage.animalnum; i++)
//            {
//                var targetLocation = new Vector3(vector.x, vector.y, vector.z  - standardAnimalBoxSize.z * (i+0.8f));
//                targetLocationList.Add(targetLocation);
//                //LogWarp.LogErrorFormat("测试：GetMovetArgetVector3s      i={0}    pos={1} ", i, targetLocation);

//            }

//            return targetLocationList;
//        }


//        /// <summary>
//        /// 获取对应的马路前小动物的位置
//        /// </summary>
//        /// <param name="idx"></param>
//        /// <returns></returns>
//        public static List<Vector3> GetCurrentPosition(int idx)
//        {
//            var animalRoadSegment = CrossRoadModelManager.GetInstance().roadModel.animalRoadSegment;
//            var standardAnimalBoxSize = CrossRoadModelManager.GetInstance().standardAnimalBoxSize;
//            LogWarp.LogErrorFormat("测试：      idx={0}    count={1} ", idx, animalRoadSegment.Count);

//            var ve = animalRoadSegment[idx];
//            List<Vector3> vector3s = new List<Vector3>();
//            int stageID = CrossRoadModelManager.GetInstance().stageID;
//            var cellStage = Config.crossroadstageConfig.getInstace().getCell(stageID);
//            for (int i = 0; i < cellStage.animalnum; i++)
//            {
//                var vector = new Vector3(ve.x, ve.y, ve.z - standardAnimalBoxSize.z * (i+0.8f));
//                vector3s.Add(vector);
//            }

//            return vector3s;
//        }

//    }
//}
