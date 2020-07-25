
using UFrame;
using UnityEngine;
using UFrame.Logger;

namespace CrossRoadGame
{
    public class StateCrossRoadAnimalCrossRoad : FSMState
    {
        bool isToStateRunToEndPoint = false;

        EntityCrossRoadAnimal owner { get { return (this.fsmCtr as FSMCrossRoadAnimal).owner; } }
        
        public StateCrossRoadAnimalCrossRoad(int stateName, FSMMachine fsmCtr) :
            base(stateName, fsmCtr)
        {
        }

        public override void Enter(int preStateName)
        {
            base.Enter(preStateName);

            owner.MarkGameObject();

            isToStateRunToEndPoint = false;
        }

        public override void AddAllConvertCond()
        {
            AddConvertCond((int)FSMCrossRoadAnimalState.RunToEndPoint,
                IsToStateRunToEndPoint);
        }

        public override void Tick(int deltaTimeMS)
        {
            Tick_Move(deltaTimeMS);
            Tick_RotateBack(deltaTimeMS);
            Tick_RotateForward(deltaTimeMS);
        }

        void Tick_Move(int deltaTimeMS)
        {
            if (!owner.isMoving)
            {
                return;
            }

            if (owner.arrivedLastPos)
            {
                return;
            }

            if (owner.isRotateForward || owner.isRotateback)
            {
                owner.PlayIdle();
                return;
            }

            //第一条到倒数第二条路
            if (owner.animalTeamModel.currentRoad < owner.cellStage.roadnum - 1)
            {
                //过马路记录
                if (owner.IsPassed(owner.firstPos) && !owner.isPassedRoad)
                {
                    owner.isPassedRoad = true;
                    owner.animalTeamModel.passedCurrRoadSet.Add(owner.idxInTeam);
                    DebugFile.GetInstance().WriteKeyFile(string.Format("Animal-{0}", owner.idxInTeam),
                        "PassedFirstPos, currentRoad={0}", owner.animalTeamModel.currentRoad);
                }

                //到达终点记录
                if (owner.IsPassed(owner.nextRoadPos) && !owner.isArrivedNextRoadPos)
                {
                    owner.isArrivedNextRoadPos = true;
                    owner.position = owner.targetPos;
                    owner.isMoving = false;
                    owner.PlayIdle();
                    //自身开启往后转
                    if (owner.idxInTeam < owner.maxIdxInTeam)
                    {
                        owner.isRotateback = true;
                    }
                        
                    //最后一个人到达,所有人到达
                    if (owner.idxInTeam == owner.maxIdxInTeam)
                    {
                        CrossRoadCameraController.GetInstance().MoveCamera(owner.nextFirstPos);
                        //owner.animalTeamModel.passedCurrRoadSet.Clear();

                        //++owner.animalTeamModel.currentRoad;

                        //设置最大前转等待时间
                        //owner.animalTeamModel.maxWaitRotateForward =
                        //    (int)owner.animalTeamModel.entityCrossRoadAnimalList[0].rotateAngle;


                        owner.animalTeamModel.maxWaitRotateForward = (int)(double)(
                            owner.animalTeamModel.entityCrossRoadAnimalList[0].rotateAngle / owner.rotateSpeedMS);

                        DebugFile.GetInstance().WriteKeyFile("CrossRoad", "currentRoad = {0}", owner.animalTeamModel.currentRoad);
                        
                        for (int i = 0; i <= owner.maxIdxInTeam; i++)
                        {
                            var animal = owner.animalTeamModel.entityCrossRoadAnimalList[i];
                            animal.isMoving = false;
                            //animal.isPassedRoad = false;
                            animal.moveSpeed = animal.moveSpeedBak;

                            DebugFile.GetInstance().WriteKeyFile(string.Format("Animal-{0}", owner.idxInTeam),
                                "isMoving = false {0}, currentRoad={1}", 1, owner.animalTeamModel.currentRoad);

                            //除了最后一个全切到向前转
                            if (i < owner.maxIdxInTeam && !animal.isRotateForward)
                            //if (i < owner.maxIdxInTeam)
                            {
                                animal.isRotateForward = true;
                                animal.waitFrameCount = 0;
                            }
                        }

                        //Debug.LogErrorFormat("LastTwo rotateForward {0}",
                        //    owner.animalTeamModel.currentRoad);
                    }
                    return;
                }
            }
            //最后一条路
            else if (owner.animalTeamModel.currentRoad == owner.cellStage.roadnum - 1)
            {
                if (owner.IsPassed(owner.firstPos))
                {
                    owner.isPassedRoad = true;
                    owner.animalTeamModel.passedCurrRoadSet.Add(owner.idxInTeam);
                }

                if (owner.IsPassed(owner.lastRoadPos))
                {
                    owner.PlayWalk();
                    owner.isMoving = false;
                    owner.position = owner.lastRoadPos;
                    owner.arrivedLastPos = true;

                    //切到跑终点状态
                    isToStateRunToEndPoint = true;
                    return;
                }
            }
            else
            {
                return;
            }

            //到达目标点停止
            if (owner.IsPassed(owner.targetPos))
            {
                owner.position = owner.targetPos;
                owner.isMoving = false;
                DebugFile.GetInstance().WriteKeyFile(string.Format("Animal-{0}", owner.idxInTeam),
                    "isMoving = false {0}, currentRoad={1}", 3, owner.animalTeamModel.currentRoad);
                owner.PlayIdle();
                return;
            }

            //position += Vector3.forward * moveSpeed * deltaTimeMS;

            //return;
            //跨马路的时候加速
            if (owner.IsPassed(owner.firstPos))
            {
                //float animalAcceleration = CrossRoadStageManager.GetInstance().animalAcceleration;
                //float delta = owner.moveSpeed + animalAcceleration * 0.001f;
                //owner.position += Vector3.forward * delta * deltaTimeMS;
                //owner.moveSpeed = delta;

                float delta = owner.moveSpeed + owner.animalAccelerationMS;
                owner.position += Vector3.forward * delta * deltaTimeMS;
                owner.moveSpeed = delta;
                owner.PlayWalk();
            }
            else
            {
                owner.position += Vector3.forward * owner.moveSpeed * deltaTimeMS;
                owner.PlayWalk();
            }
        }

        void Tick_RotateBack(int deltaTimeMS)
        {
            //不处理后转:没有收到后转或者是最后一个人
            if (!owner.isRotateback || owner.idxInTeam == owner.maxIdxInTeam)
            {
                return;
            }

            //停止后转:收到前转，并且不是最后一个人,
            if (owner.isRotateForward && owner.idxInTeam != owner.maxIdxInTeam)
            {
                owner.isRotateback = false;
                return;
            }

            float rotateAngle = owner.rotateAngle;
            //float delta = CrossRoadStageManager.GetInstance().rotateSpeed * 0.001f * deltaTimeMS;
            float delta = owner.rotateSpeedMS * deltaTimeMS;
            rotateAngle += delta;
            if (rotateAngle - 180 >= 0)
            {
                owner.Rotate(new Vector3(0, 180 - owner.rotateAngle, 0));
                owner.rotateAngle = 180;
                owner.isRotateback = false;
            }
            else
            {
                owner.Rotate(new Vector3(0, delta, 0));
                owner.rotateAngle = rotateAngle;
            }
        }

        void Tick_RotateForward(int deltaTimeMS)
        {
            //不处理前转:没有收到前转或者是最后一个人
            if (!owner.isRotateForward || owner.idxInTeam == owner.maxIdxInTeam)
            {
                return;
            }

            //停止前转：后转导致的角度为0,并且不是最后一个人
            if (owner.rotateAngle == 0 && owner.idxInTeam != owner.maxIdxInTeam)
            {
                owner.isRotateForward = false;
                //第一个人完成前转
                if (owner.idxInTeam == 0)
                {
                    owner.animalTeamModel.maxWaitRotateForward = 0;
                    owner.animalTeamModel.passedCurrRoadSet.Clear();
                    ++owner.animalTeamModel.currentRoad;
                    for (int i = 0; i <= owner.maxIdxInTeam; i++)
                    {
                        var animal = owner.animalTeamModel.entityCrossRoadAnimalList[i];
                        animal.isPassedRoad = false;
                        animal.isArrivedNextRoadPos = false;
                    }
                }
                return;
            }

            if (owner.idxInTeam != owner.maxIdxInTeam 
                && owner.waitFrameCount < GetWaitRotateForward())
            {
                ++owner.waitFrameCount;
                return;
            }

            float rotateAngle = owner.rotateAngle;
            //float delta = CrossRoadStageManager.GetInstance().rotateSpeed * 0.001f * deltaTimeMS;
            float delta = owner.rotateSpeedMS * deltaTimeMS;
            rotateAngle -= delta;
            if (rotateAngle <= 0)
            {
                owner.Rotate(new Vector3(0, -owner.rotateAngle, 0));
                owner.rotateAngle = 0;
                owner.isRotateForward = false;
                //第一个人完成前转
                if (owner.idxInTeam == 0)
                {
                    owner.animalTeamModel.maxWaitRotateForward = 0;
                    owner.animalTeamModel.passedCurrRoadSet.Clear();
                    ++owner.animalTeamModel.currentRoad;
                    for (int i = 0; i <= owner.maxIdxInTeam; i++)
                    {
                        var animal = owner.animalTeamModel.entityCrossRoadAnimalList[i];
                        animal.isPassedRoad = false;
                        animal.isArrivedNextRoadPos = false;
                    }
                }
            }
            else
            {
                owner.Rotate(new Vector3(0, -delta, 0));
                owner.rotateAngle = rotateAngle;
            }
        }

        int GetWaitRotateForward()
        {
            //int wait = owner.animalTeamModel.maxWaitRotateForward
            //    * (owner.maxIdxInTeam - owner.idxInTeam) / 100;
            int wait = (int)(double)(owner.animalTeamModel.maxWaitRotateForward
     * (owner.maxIdxInTeam - owner.idxInTeam) * CrossRoadStageManager.GetInstance().waitRotateRatio);
            //int wait = 33 * 5;
            //Debug.LogErrorFormat("{0} , wait = {1}", owner.idxInTeam, wait);
            return wait;
        }

        protected bool IsToStateRunToEndPoint()
        {
            return isToStateRunToEndPoint;
        }
    }
}
