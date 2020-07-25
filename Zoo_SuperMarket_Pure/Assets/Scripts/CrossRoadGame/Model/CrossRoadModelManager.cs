using System.Collections;
using System.Collections.Generic;
using UFrame;
using UFrame.Common;
using UnityEngine;

namespace CrossRoadGame
{
    public class CrossRoadModelManager : Singleton<CrossRoadModelManager>, ISingleton
    {
        /// <summary>
        /// 路的数据
        /// </summary>
        public RoadModel roadModel;

        /// <summary>
        /// enity数据
        /// </summary>
        public CrossRoadEntityModel entityModel;

        /// <summary>
        /// 关卡id
        /// </summary>
        public int stageID;

        /// <summary>
        /// 动物标准尺寸
        /// </summary>
        public Vector3 standardAnimalBoxSize;

        /// <summary>
        /// 终点动物杯挂点
        /// </summary>
        public List<Vector3> endAnimalPos;

        /// <summary>
        /// 终点动物中转挂点
        /// </summary>
        public Vector3 endAnimalTransferPos;

        /// <summary>
        /// 终点资源的位置（中心点，美术资源要确保终点资源的重心就是中心点）
        /// </summary>
        public Vector3 endPos;


        /// <summary>
        /// 第一条路资源的位置
        /// </summary>
        public Vector3 startPoint;


        /// <summary>
        /// 屏幕左下世界坐标
        /// </summary>
        public Vector3 spLBW;

        /// <summary>
        /// 屏幕左上世界坐标
        /// </summary>
        public Vector3 spLTW;

        /// <summary>
        /// 屏幕右下世界坐标
        /// </summary>
        public Vector3 spRBW;

        /// <summary>
        /// 屏幕右上世界坐标
        /// </summary>
        public Vector3 spRTW;

        /// <summary>
        /// 相机是否在移动
        /// </summary>
        public bool isCameraMoving = false;

        public SimpleParticle endPosEffectSp;

        public CrossRoadAnimalTeamModel animalTeamModel;

        public void Init()
        {
            roadModel = new RoadModel();
            entityModel = new CrossRoadEntityModel();
            endAnimalPos = new List<Vector3>();
            endAnimalTransferPos = new Vector3();
            animalTeamModel = new CrossRoadAnimalTeamModel();
        }

        public void Release()
        {
            if (roadModel != null)
            {
                roadModel.Release();
            }

            if (entityModel != null)
            {
                entityModel.Release();
            }

            if (animalTeamModel != null)
            {
                animalTeamModel.Release();
            }
        }

        /// <summary>
        /// 释放非路的实体
        /// </summary>
        public void Release_NoRoad()
        {
            if (entityModel != null)
            {
                entityModel.Release();
            }

            if (animalTeamModel != null)
            {
                animalTeamModel.Release();
            }
        }


    }

}
