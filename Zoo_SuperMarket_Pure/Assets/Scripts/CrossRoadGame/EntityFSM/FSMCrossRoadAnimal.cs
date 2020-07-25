using System;
using UFrame;
using UnityEngine;

namespace CrossRoadGame
{
    public enum FSMCrossRoadAnimalState
    {
        /// <summary>
        /// 空闲
        /// </summary>
        Idle,

        /// <summary>
        /// 过马路
        /// </summary>
        CrossRoad,

        /// <summary>
        /// 到达后往后转
        /// </summary>
        RotateBack,

        /// <summary>
        /// 往前转
        /// </summary>
        RotateForward,

        /// <summary>
        /// 跑向终点
        /// </summary>
        RunToEndPoint,
    }

    public class FSMCrossRoadAnimal : FSMMachine
    {

        public EntityCrossRoadAnimal owner;

        public FSMCrossRoadAnimal(EntityCrossRoadAnimal owner)
        {
            this.owner = owner;
        }

        public override void Release()
        {
            base.Release();
        }
    }
}

