
using Game.MessageCenter;
using SWS;
using UFrame;
using UnityEngine;
using UnityEngine.Events;

namespace CrossRoadGame
{
    public class StateCrossRoadRunToEndPoint : FSMState
    {
        EntityCrossRoadAnimal owner { get { return (this.fsmCtr as FSMCrossRoadAnimal).owner; } }

        public StateCrossRoadRunToEndPoint(int stateName, FSMMachine fsmCtr) :
            base(stateName, fsmCtr)
        {

        }

        public override void Enter(int preStateName)
        {
            base.Enter(preStateName);

            owner.MarkGameObject();

            SWS_Walk(owner.mainGameObject, owner.idxInTeam);
            if (owner.idxInTeam == owner.animalTeamModel.entityCrossRoadAnimalList.Count - 1)
            {
                ++owner.animalTeamModel.currentRoad;
                CrossRoadCameraController.GetInstance().MoveCamera(
                    CrossRoadModelManager.GetInstance().endPos);
            }
        }

        public override void AddAllConvertCond()
        {
        }

        public override void Tick(int deltaTimeMS)
        {
        }

        public void SWS_Walk(GameObject mainGameObject, int idx)
        {
            //return;
            var spm = mainGameObject.GetComponent<splineMove>();
            if (spm == null)
            {
                spm = mainGameObject.AddComponent<splineMove>();
            }
            //spm.animEaseType = (int)DG.Tweening.Ease.InOutSine;
            spm.speed = owner.moveSpeedBak * 1000f;
            //create path manager game object
            GameObject newPath = new GameObject(string.Format("Path{0} (Runtime Creation)", idx));
            PathManager path = newPath.AddComponent<PathManager>();

            //declare waypoint positions
            Vector3[] positions = new Vector3[] { owner.position,
                CrossRoadModelManager.GetInstance().endAnimalTransferPos,
                CrossRoadModelManager.GetInstance().endAnimalPos[idx] };
            Transform[] waypoints = new Transform[positions.Length];

            //instantiate waypoints
            for (int i = 0; i < positions.Length; i++)
            {
                GameObject newPoint = new GameObject("Waypoint " + i);
                waypoints[i] = newPoint.transform;
                waypoints[i].position = positions[i];
            }

            //assign waypoints to path
            path.Create(waypoints, true);

            spm.SetPath(path);
            spm.StartMove();

            UnityEvent SWS_ArrivedGate = spm.events[1];
            SWS_ArrivedGate.RemoveAllListeners();
            SWS_ArrivedGate.AddListener(delegate {
                CrossRoadModelManager.GetInstance().endPosEffectSp.Play();
            });
            
            UnityEvent SWS_ArrivedEndPoint = spm.events[2];
            SWS_ArrivedEndPoint.RemoveAllListeners();
            SWS_ArrivedEndPoint.AddListener(
                delegate {
                    float angle = Vector3.Angle(mainGameObject.transform.forward, Vector3.back);
                    Vector3 normal = Vector3.Cross(mainGameObject.transform.forward, Vector3.back);
                    angle *= Mathf.Sign(Vector3.Dot(normal, Vector3.up));
                    mainGameObject.transform.Rotate(new Vector3(0, angle, 0));
                    owner.PlayPose();
                    ++owner.animalTeamModel.numArrivedEndPoint;
                    if (owner.animalTeamModel.numArrivedEndPoint == (owner.maxIdxInTeam + 1))
                    {
                        MessageManager.GetInstance().Send(
                            (int) GameMessageDefine.CrossRoadAnimalTeamArrived);

                        foreach (var item in owner.animalTeamModel.entityCrossRoadAnimalList)
                        {
                            item.mainGameObject.transform.Find("Effect/MoveEffect").gameObject.SetActive(false);

                        }
                    }
                }
            );
        }
    }
}
