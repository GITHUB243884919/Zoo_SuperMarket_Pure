using System.Collections;
using System.Collections.Generic;
using Game;
using Game.MessageCenter;
using UFrame;
using UnityEngine;

#if UNITY_EDITOR

    public class MonoForDebug : MonoBehaviour
    {
        private static MonoForDebug _inst;
        public static MonoForDebug inst
        {
            get
            {
                if (_inst == null)
                {
                    _inst = FindObjectOfType<MonoForDebug>();
                    if (_inst == null)
                    {
                        GameObject go = new GameObject("[MonoForDebug]");
                        DontDestroyOnLoad(go);
                        _inst = go.AddComponent<MonoForDebug>();
                    }
                }
                return _inst;
            }
        }

        private GuideMissionModel missionModel { get { return GuideMissionModel.GetInstance(); } }
        private int currTaskIdForDbg = 0;
        private Config.missionCell _currMissionCellForDbg;
        private Config.missionCell currMissionCellForDbg
        {
            get
            {
                if (currTaskIdForDbg != missionModel.currTaskId)
                {
                    currTaskIdForDbg = missionModel.currTaskId;
                    _currMissionCellForDbg = missionModel.GetTaskCell(currTaskIdForDbg);
                }
                return _currMissionCellForDbg;
            }
        }

        //private void OnGUI()
        //{
        //    GUILayout.BeginVertical();
        //    if (GUILayout.Button("Task Test"))
        //    {
        //        if (currMissionCellForDbg != null)
        //        {
        //            TaskType taskType = currMissionCellForDbg.ParseTaskType();
        //            if (taskType == TaskType.ParkingLevelup)
        //            {
        //                BuildingLevelup.Send((int)BuildingTypeFM.Parking,
        //                    currMissionCellForDbg.GetBuildingId(),
        //                    currMissionCellForDbg.ParseBuildingProperty(),
        //                    currMissionCellForDbg.need);
        //            }
        //            else if (taskType == TaskType.EntryGateLevelup)
        //            {
        //                BuildingLevelup.Send((int)BuildingTypeFM.EntryGate,
        //                    currMissionCellForDbg.GetBuildingId(),
        //                    currMissionCellForDbg.ParseBuildingProperty(),
        //                    currMissionCellForDbg.need);
        //            }
        //            else if (taskType == TaskType.LittleZooLevelup)
        //            {
        //                BuildingLevelup.Send((int)BuildingTypeFM.LittleZoo,
        //                    currMissionCellForDbg.GetBuildingId(),
        //                    currMissionCellForDbg.ParseBuildingProperty(),
        //                    currMissionCellForDbg.need);
        //            }
        //            else if (taskType == TaskType.LittleZooVisit)
        //            {
        //                VisitorReceiveComplete.Send(currMissionCellForDbg.GetLittleZooId());
        //            }
        //            else if (taskType == TaskType.AdWatch)
        //            {
        //                AdWatchComplete.Send(AdWatchComplete.AdType_RewardedVideo, currMissionCellForDbg.GetAdTag());
        //            }
        //        }
        //    }

        //    if (GUILayout.Button("Star Increase"))
        //        MessageInt.Send((int)GameMessageDefine.GetItem, 4); // 发星星

        //    if (GUILayout.Button("Map Coin Effect"))
        //    {
        //        var msg = BroadcastLeaveSceneCoin.PreSend();
        //        msg.sceneCoinDict.Add(0, 6666);
        //        BroadcastLeaveSceneCoin.Send();
        //    }

        //    GUILayout.EndVertical();
        //}  
    }

#endif