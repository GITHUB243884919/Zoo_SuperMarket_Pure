//using System.Collections;
//using System.Collections.Generic;
//using UFrame.Logger;
//using UnityEngine;

//namespace CrossRoadGame
//{

//    public class AnimalData
//    {
//        /// <summary>
//        /// 起点
//        /// </summary>
//        public Vector3 startPos;

//        /// <summary>
//        /// 终点
//        /// </summary>
//        public Vector3 endPos;

//        /// <summary>
//        /// 区域
//        /// </summary>
//        public Rect crossRoadRectArea;

//        /// <summary>
//        /// 动物的速度
//        /// </summary>
//        public float carSpeed;
//    }


//    public class AnimalModel
//    {
//        public List<Vector3> animalDataList = new List<Vector3>();

//        public AnimalModel()
//        {
//            Init();
//        }

//        void Init()
//        {
//            //var animalRoadSegment = CrossRoadModelManager.GetInstance().roadModel.animalRoadSegment;

//            //LogWarp.LogErrorFormat("测试：    AnimalModel   init ,  animalRoadSegment.Count={0} ", animalRoadSegment.Count);

//            //animalDataList = animalRoadSegment;
//        }

//        //public void AddCrossRoad(Rect rc, Vector3 startPos, Vector3 endPos, int cdVal, RoadDir roadDir, float carSpeed)
//        //{
//        //    var animalData = new AnimalData
//        //    {
//        //        crossRoadRectArea = rc,
//        //        startPos = startPos,
//        //        endPos = endPos,
//        //        carSpeed = carSpeed,
//        //    };
//        //    //animalDataList.Add(animalData);
//        //}

//        public void Release()
//        {
//            animalDataList.Clear();
//        }

//    }
//}
