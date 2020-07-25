using System.Collections.Generic;

namespace CrossRoadGame
{
    public class CrossRoadAnimalTeamModel
    {
        /// <summary>
        /// 所有动物实体列表(按照顺序排队)
        /// </summary>
        public List<EntityCrossRoadAnimal> entityCrossRoadAnimalList = new List<EntityCrossRoadAnimal>();

        /// <summary>
        /// 已经跨过当前路的
        /// </summary>
        public HashSet<int> passedCurrRoadSet = new HashSet<int>();

        /// <summary>
        /// 队伍正在通过的路
        /// </summary>
        public int currentRoad = 0;

        ///// <summary>
        ///// 队伍是否处于旋转
        ///// </summary>
        //public bool isRotate = false;

        /// <summary>
        /// 最大往前转等待时间
        /// </summary>
        public int maxWaitRotateForward = 0;

        /// <summary>
        /// 到达终点完成转身的动物数量
        /// </summary>
        public int numArrivedEndPoint = 0;

        public CrossRoadAnimalTeamModel()
        {
        }

        void Init()
        {

        }

        public void Release()
        {
            entityCrossRoadAnimalList.Clear();
            passedCurrRoadSet.Clear();
            currentRoad = 0;
        }

    }
}
