
using Game.MessageCenter;
using UFrame;
using UFrame.EntityFloat;
using UFrame.Logger;
using UFrame.MessageCenter;
using UnityEngine;

namespace CrossRoadGame
{
    public class EntityCrossRoadAnimal : EntityMovable
    {
        public static ObjectPool<EntityCrossRoadAnimal> pool = new ObjectPool<EntityCrossRoadAnimal>();

        public FSMMachine fsm;
        /// <summary>
        /// 速度备份
        /// </summary>
        public float moveSpeedBak = Const.Invalid_Float;

        /// <summary>
        /// 动物移动加速度(单位长度/毫秒)
        /// </summary>
        public float animalAccelerationMS = 0.5f;

        /// <summary>
        /// 动物旋转速度(欧拉角/毫秒)
        /// </summary>
        public float rotateSpeedMS = 10;

        /// <summary>
        /// 动画
        /// </summary>
        public SimpleAnimation simpleAnimation = new SimpleAnimation();

        /// <summary>
        /// 行走动物播放时间  
        /// </summary>
        public float animalAnimationSpeed;

        /// <summary>
        /// 实体在队伍中的位置编号
        /// </summary>
        public int idxInTeam = 0;

        /// <summary>
        /// 是否在移动
        /// </summary>
        public bool isMoving = false;

        /// <summary>
        /// 目标位置
        /// </summary>
        public Vector3 targetPos;

        /// <summary>
        /// 过了马路（过马路南边第一个点）
        /// </summary>
        public bool isPassedRoad = false;

        /// <summary>
        /// 到达下一条街的位置
        /// </summary>
        public bool isArrivedNextRoadPos = false;

        /// <summary>
        /// 过了最后一个点(lastRoadPos)
        /// </summary>
        public bool arrivedLastPos = false;

        /// <summary>
        /// 往后转
        /// </summary>
        public bool isRotateback = false;

        /// <summary>
        /// 往前转
        /// </summary>
        public bool isRotateForward = false;

        /// <summary>
        /// 旋转角度
        /// </summary>
        public float rotateAngle = 0;

        public int waitFrameCount = 0;

        public SimpleParticle runEffSp = null;

        /// <summary>
        /// 每个动物的标准尺寸（z值）
        /// </summary>
        Vector3 standardAnimalBoxSize {
            get {
                return CrossRoadModelManager.GetInstance().standardAnimalBoxSize;
            }
        }

        public CrossRoadAnimalTeamModel animalTeamModel {
            get {
                return CrossRoadModelManager.GetInstance().animalTeamModel;
            }
        }

        public RoadModel roadModel {
            get {
                return CrossRoadModelManager.GetInstance().roadModel;
            }
        }

        /// <summary>
        /// 当前路的第一个点
        /// </summary>
        public Vector3 firstPos {
            get {
                return roadModel.animalRoadSegment[animalTeamModel.currentRoad]
                    + Vector3.back * standardAnimalBoxSize.z * 0.8f;
            }
        }

        /// <summary>
        /// 当前路下一条路的第一个点
        /// </summary>
        public Vector3 nextFirstPos {
            get {
                return roadModel.animalRoadSegment[animalTeamModel.currentRoad + 1]
                    + Vector3.back * standardAnimalBoxSize.z * 0.8f;
            }
        }

        /// <summary>
        /// 在下一条路上对应的队伍位置的点
        /// </summary>
        public Vector3 nextRoadPos {
            get {
                return nextFirstPos + Vector3.back * standardAnimalBoxSize.z * idxInTeam;
            }
        }

        /// <summary>
        /// 最后一条路过马路后点
        /// </summary>
        public Vector3 lastRoadPos {
            get {
                //return roadModel.animalRoadSegment[roadModel.animalRoadSegment.Count - 1]
                //    + Vector3.forward * standardAnimalBoxSize.z * 0.8f;
                var a = roadModel.animalRoadSegment[roadModel.animalRoadSegment.Count - 1];
                var b = roadModel.crossRoadRectArea[roadModel.animalRoadSegment.Count - 1].height;
                return (a + b * Vector3.forward + Vector3.forward * standardAnimalBoxSize.z);
            }
        }

        Vector3 preFirstPos {
            get {
                return roadModel.animalRoadSegment[animalTeamModel.currentRoad - 1]
                    + Vector3.back * standardAnimalBoxSize.z * 0.8f;
            }
        }
        
        /// <summary>
        /// 获取当前配置的过马路文件 crossroadstageCell
        /// </summary>
        public Config.crossroadstageCell cellStage {
            get {
                int stageID = CrossRoadModelManager.GetInstance().stageID;
                return Config.crossroadstageConfig.getInstace().getCell(stageID);
            }
        }

        /// <summary>
        /// 动物在队伍最大索引
        /// </summary>
        public int maxIdxInTeam { get {
                return animalTeamModel.entityCrossRoadAnimalList.Count - 1; } }

        public void Init(float animalAnimationSpeed, float animalMoveSpeed, int idxInTeam,
            float animalAcceleration, float rotateSpeed)
        {
            this.animalAnimationSpeed = animalAnimationSpeed;
            this.moveSpeed = animalMoveSpeed * 0.001f;
            this.moveSpeedBak = this.moveSpeed;
            this.animalAccelerationMS = animalAcceleration * 0.001f;
            this.rotateSpeedMS = rotateSpeed * 0.001f;
            this.idxInTeam = idxInTeam;
            isMoving = false;
            isPassedRoad = false;
            isArrivedNextRoadPos = false;
            arrivedLastPos = false;
            isRotateback = false;
            isRotateForward = false;
            rotateAngle = 0f;
            waitFrameCount = 0;

            //var a = roadModel.animalRoadSegment[roadModel.animalRoadSegment.Count - 1];
            //var b = roadModel.crossRoadRectArea[roadModel.animalRoadSegment.Count - 1].height;
            //Debug.LogError(a + b * Vector3.forward + Vector3.forward * standardAnimalBoxSize.z);
            if (fsm == null)
            {
                fsm = new FSMCrossRoadAnimal(this);
                fsm.AddState(new StateCrossRoadAnimalIdle((int)FSMCrossRoadAnimalState.Idle, fsm));
                fsm.AddState(new StateCrossRoadAnimalCrossRoad((int)FSMCrossRoadAnimalState.CrossRoad, fsm));
                fsm.AddState(new StateCrossRoadRunToEndPoint((int)FSMCrossRoadAnimalState.RunToEndPoint, fsm));

            }
            fsm.GotoState((int)FSMCrossRoadAnimalState.Idle);
            fsm.Run();

            DebugFile.GetInstance().MarkGameObject(mainGameObject, "Animal-{0}", idxInTeam);

        }

        public override void Active()
        {
            base.Active();

            SetAnimation();
            PlayIdle();

            MessageManager.GetInstance().Regist((int)GameMessageDefine.CrossRoadAnimalTeamMove,
                OnCrossRoadAnimalTeamMove);
            MessageManager.GetInstance().Regist((int)GameMessageDefine.CrossRoadAnimalTeamStopMove,
                OnCrossRoadAnimalTeamStopMove);

        }

        public override void Deactive()
        {
            this.position = Const.Invisible_Postion;

            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.CrossRoadAnimalTeamMove,
                OnCrossRoadAnimalTeamMove);
            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.CrossRoadAnimalTeamStopMove,
                OnCrossRoadAnimalTeamStopMove);

            this.fsm.Stop();
            base.Deactive();
        }

        public override void Tick(int deltaTimeMS)
        {
            if (!this.CouldActive())
            {
                return;
            }

            fsm.Tick(deltaTimeMS);
        }

        public override void OnDeathToPool()
        {
            this.Deactive();
            base.OnDeathToPool();
        }

        public override void OnRecovery()
        {
            this.Deactive();
            base.OnRecovery();
        }

        public void PlayWalk()
        {
            if (simpleAnimation.lastAnimName != "walk"
                || !simpleAnimation.IsRunning())
            {
                simpleAnimation.Play("walk");
                runEffSp.Play();
                //Debug.LogErrorFormat("PlayWalk {0}", idxInTeam);
            }
        }

        public void PlayPose()
        {
            simpleAnimation.Play("pose");
        }

        public void PlayIdle()
        {
            if (simpleAnimation.lastAnimName != "idle"
                || !simpleAnimation.IsRunning())
            {
                simpleAnimation.Play("idle");
                runEffSp.Stop();
                //Debug.LogErrorFormat("PlayIdle {0}", idxInTeam);
            }
        }

        /// <summary>
        /// 设置动画循环播放
        /// </summary>
        private void SetAnimation()
        {
            simpleAnimation.SetAnimSpeed("walk", animalAnimationSpeed);
            simpleAnimation.SetAnimLoop("walk");
            simpleAnimation.SetAnimLoop("pose");
            simpleAnimation.SetAnimLoop("idle");
        }

        protected void OnCrossRoadAnimalTeamMove(Message msg)
        {
            Vector3 pos = Vector3.zero;
            if (animalTeamModel.currentRoad == cellStage.roadnum - 1)
            {
                pos = lastRoadPos;
            }
            else if (animalTeamModel.currentRoad < cellStage.roadnum - 1)
            {
                pos = nextRoadPos;
            }
            else
            {
                return;
            }

            if (arrivedLastPos)
            {
                return;
            }

            //DebugFile.GetInstance().WriteKeyFile(string.Format("Animal-{0}", idxInTeam),
            //    "OnCrossRoadAnimalTeamMove {0}", 1);

            if (isMoving)
            {
                return;
            }
            //if (isMoving || isRotateback || isRotateForward)
            //{
            //    return;
            //}
            //if (idxInTeam == 0)
            //{
            //    Debug.LogErrorFormat("GotoState {0}, {1}, {2}",
            //        isMoving, isRotateback, isRotateForward);
            //}
            isMoving = true;
            //DebugFile.GetInstance().WriteKeyFile(string.Format("Animal-{0}", idxInTeam),
            //    "OnCrossRoadAnimalTeamMove {0}, currentRoad={1}", 2, animalTeamModel.currentRoad);
            //PlayWalk();
            //下一段路对应的位置
            targetPos = pos;

            
            fsm.GotoState((int)FSMCrossRoadAnimalState.CrossRoad);
        }

        protected void OnCrossRoadAnimalTeamStopMove(Message msg)
        {
            //第一位的忽略
            if (idxInTeam == 0)
            {
                return;
            }

            //DebugFile.GetInstance().WriteKeyFile(string.Format("Animal-{0}", idxInTeam),
            //    "OnCrossRoadAnimalTeamStopMove {0}", 2);

            //已经跨过马路的忽略
            if (isPassedRoad)
            {
                return;
            }


            //if (isRotateback || isRotateForward)
            //{
            //    return;
            //}
            //DebugFile.GetInstance().WriteKeyFile(string.Format("Animal-{0}", idxInTeam),
            //    "OnCrossRoadAnimalTeamStopMove {0}, currentRoad={1}", 3, animalTeamModel.currentRoad);
            //没有过马路的，设置目标位置：当前路的位置
            int idx = idxInTeam - animalTeamModel.passedCurrRoadSet.Count;
            targetPos = firstPos + Vector3.back * standardAnimalBoxSize.z * idx;
        }

        public bool IsPassed(Vector3 pos)
        {
            return position.z >= pos.z || Math_F.ApproximateNumber(position.z, pos.z, 0.001f);
        }

        public void GotoState(int stateName)
        {
            fsm.GotoState(stateName);
        }

        public int GetCurrentStateName()
        {
            return fsm.GetCurrentStateName();
        }

        public int GetPreStateName()
        {
            return fsm.GetPreStateName();
        }

        public void MarkGameObject()
        {
            DebugFile.GetInstance().MarkGameObject(mainGameObject, 
                "animal_{0}_{1}_{2}",
                idxInTeam,
                (FSMCrossRoadAnimalState)fsm.GetPreStateName(),
                (FSMCrossRoadAnimalState)fsm.GetCurrentStateName());
        }

    }
}
