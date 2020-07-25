using System;
using System.Collections;
using System.Collections.Generic;
using Game.GlobalData;
using Game.MessageCenter;
using UFrame;
using UFrame.Common;
using UFrame.MessageCenter;
using UnityEngine;
using UFrame.Logger;

namespace Game
{
    // ad tag for mission
    public enum AdTagFM
    {
        Add_Double_Advert, // 双倍广告 23
        Add_Tourist_Advert, // 游轮广告 18
        Add_Ticket_Advert, // 售票速度 20
        Add_Visit_Advert, // 观光速度 22
        Add_Offline_Advert, // 离线广告 // 26
        Add_Viptiming_Advert,//气球广告
    }

    // buliding type for mission
    public enum BuildingTypeFM
    {
        Parking = 1,
        EntryGate = 2,
        LittleZoo = 3,
    }

    public enum TaskType
    {
        Unknown = 0, // 未知
        ParkingLevelup = 1, // 停车场升级，利润、车位容量、来客速度
        EntryGateLevelup = 2, // 售票口升级，门票价格、售票口
        LittleZooLevelup = 3, // 动物栏升级，门票价格、观光位容量、观光速度
        LittleZooVisit = 4, // 游客动物栏观光
        AdWatch = 5, // 广告观看
        OpenNewLittleZoo = 6,//开启新动物栏
    }

    public enum ParkingProperty
    {
        None = 0, // 空
        Revenue = 1, // 收益
        Capacity = 2, // 容量
        VisitorFlowSpeed = 3 // 来客速度
    }

    public enum EntryGateProperty
    {
        None = 0, // 空
        TicketPrice = 1, // 门票价格
        Entrance = 2 // 入口（售票口）
    }

    public enum LittleZooProperty
    {
        None = 0, // 空
        TicketPrice = 1, // 门票价格
        Capacity = 2, // 观光位容量
        VisitSpeed = 3 // 观光速度
    }

    public static class GuideMissionUtil
    {
        public static TaskType ParseTaskType(this Config.missionCell missionCell)
        {
            if (missionCell == null)
                return TaskType.Unknown;
            return (TaskType)missionCell.tasktype;
        }

        public static ParkingProperty ParseParkingProperty(this Config.missionCell missionCell)
        {
            if (missionCell == null || missionCell.ParseTaskType() != TaskType.ParkingLevelup)
                return ParkingProperty.None;
            return (ParkingProperty)missionCell.taskparam2;
        }

        public static EntryGateProperty ParseEntryGateProperty(this Config.missionCell missionCell)
        {
            if (missionCell == null || missionCell.ParseTaskType() != TaskType.EntryGateLevelup)
                return EntryGateProperty.None;
            return (EntryGateProperty)missionCell.taskparam2;
        }

        public static LittleZooProperty ParseLittleZooProperty(this Config.missionCell missionCell)
        {
            if (missionCell == null || missionCell.ParseTaskType() != TaskType.LittleZooLevelup)
                return LittleZooProperty.None;
            return (LittleZooProperty)missionCell.taskparam2;
        }

        public static int ParseBuildingProperty(this Config.missionCell missionCell)
        {
            if (missionCell == null)
                return 0;
            if (missionCell.ParseTaskType() == TaskType.ParkingLevelup)
                return (int)missionCell.ParseParkingProperty();
            if (missionCell.ParseTaskType() == TaskType.LittleZooLevelup)
                return (int)missionCell.ParseLittleZooProperty();
            if (missionCell.ParseTaskType() == TaskType.EntryGateLevelup)
                return (int)missionCell.ParseEntryGateProperty();
            return 0;
        }

        public static int GetLittleZooId(this Config.missionCell missionCell)
        {
            if (missionCell == null || (missionCell.ParseTaskType() != TaskType.LittleZooLevelup &&
                missionCell.ParseTaskType() != TaskType.LittleZooVisit))
                return -1;
            int ret;
            int.TryParse(missionCell.taskparam1, out ret);
            return ret;
        }

        public static string GetAdTag(this Config.missionCell missionCell)
        {
            if (missionCell == null || missionCell.ParseTaskType() != TaskType.AdWatch)
                return string.Empty;
            return missionCell.taskparam1;
        }

        public static int GetEntryGateId(this Config.missionCell missionCell)
        {
            if (missionCell == null || missionCell.ParseTaskType() != TaskType.EntryGateLevelup)
                return -1;
            int ret;
            int.TryParse(missionCell.taskparam1, out ret);
            return ret;
        }

        public static int GetBuildingId(this Config.missionCell missionCell)
        {
            if (missionCell == null)
                return -1;
            if (missionCell.ParseTaskType() == TaskType.AdWatch)
                return -1;
            int ret;
            int.TryParse(missionCell.taskparam1, out ret);
            return ret;
        }
    }

    public class GuideMissionModel : Singleton<GuideMissionModel>, ISingleton
    {
        public void Init()
        {
            currSceneTaskCells.Clear();
            IDictionary<string, Config.missionCell> allTaskCell = configInst.AllData;
            foreach (var k in allTaskCell.Keys)
            {
                if (allTaskCell[k].scene == currSceneId)
                    currSceneTaskCells.Add(k, allTaskCell[k]);
            }
        }

        private PlayerData playerData { get { return GlobalDataManager.GetInstance().playerData; } }
        private Config.missionConfig configInst { get { return Config.missionConfig.getInstace(); } }
        private GuideMissionPlayerDataMSS missionPlayerData { get { return playerData.playerZoo.guideMissionPlayerDataMSS; } }
        private IDictionary<string, Config.missionCell> currSceneTaskCells = new Dictionary<string, Config.missionCell>();

        private GuideMissionPlayerDataMSS.TaskStateMSS GetTaskState(int taskId)
        {
            return missionPlayerData.taskStates.Find((task) => { return task.taskId == taskId; });
        }

        private GuideMissionPlayerDataMSS.MissionProgressMSS GetMissionProgress(int sceneId)
        {
            return missionPlayerData.missionProgress.Find((progress) => { return progress.sceneId == sceneId; });
        }

        /// <summary>
        /// 是否首次接到引导任务
        /// </summary>
        public bool isFirstGuide
        {
            get { return missionPlayerData.isFirst > 0; }
            set { missionPlayerData.isFirst = value ? 1 : 0; }
        }

        /// <summary>
        /// 当前场景Id
        /// </summary>
        public int currSceneId { get { return playerData.playerZoo.currSceneID; } }

        /// <summary>
        /// 当前任务Id
        /// </summary>
        public int currTaskId
        {
            get
            {
                var progress = GetMissionProgress(currSceneId);
                return progress == null ? -1 : progress.currTaskId;
            }
            set
            {
                var progress = GetMissionProgress(currSceneId);
                if (progress == null)
                {
                    progress = new GuideMissionPlayerDataMSS.MissionProgressMSS() { sceneId = currSceneId };
                    missionPlayerData.missionProgress.Add(progress);
                }
                progress.currTaskId = value;   
            }
        }

        /// <summary>
        /// 设置任务进程
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="progress"></param>
        public void SetTaskProgress(int taskId, int progress)
        {
            GuideMissionPlayerDataMSS.TaskStateMSS taskState = GetTaskState(taskId);
            if (taskState == null)
            {
                taskState = new GuideMissionPlayerDataMSS.TaskStateMSS() { taskId = taskId, progress = progress, isCleared = 0 };
                missionPlayerData.taskStates.Add(taskState);
            }
            taskState.progress = Mathf.Min(progress, GetTaskGoal(taskId));
        }

        /// <summary>
        /// 设置任务cleared（完成且领取了奖励）
        /// </summary>
        /// <param name="taskId"></param>
        public void SetTaskCleared(int taskId)
        {
            GuideMissionPlayerDataMSS.TaskStateMSS taskState = GetTaskState(taskId);
            if (taskState != null)
                taskState.isCleared = 1;
        }

        /// <summary>
        /// 第一个任务Id
        /// </summary>
        public int firstTaskId { get { return Config.sceneConfig.getInstace().getCell(currSceneId).missionstart; } }

        /// <summary>
        /// 任务教程是否完成
        /// </summary>
        public bool selfGuideComplete
        {
            get { return missionPlayerData.selfGuideComplete > 0; }
            set { missionPlayerData.selfGuideComplete = value ? 1 : 0; }
        }

        /// <summary>
        /// 获取某个任务进度
        /// </summary>
        /// <returns></returns>
        public bool GetTaskProgress(int taskId, out int progress, out int goal)
        {
            progress = 0;
            goal = GetTaskGoal(taskId);
            Config.missionCell missionCell = GetTaskCell(taskId);
            if (missionCell != null)
                goal = missionCell.need;
            GuideMissionPlayerDataMSS.TaskStateMSS taskState = GetTaskState(taskId);
            if (taskState != null)
                progress = taskState.progress;
            return progress >= goal;
        }

        /// <summary>
        /// 根据用户数据获取任务进度
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public int ResolveTaskProgress(int taskId)
        {
            Config.missionCell missionCell = GetTaskCell(taskId);
            if (missionCell == null)
                return 0;
            TaskType taskType = missionCell.ParseTaskType();
            if (taskType == TaskType.AdWatch || taskType == TaskType.LittleZooVisit)
                return 0;

            if (taskType == TaskType.LittleZooLevelup)
            {
                int litzooId = missionCell.GetLittleZooId();
                LittleZooModuleDataMSS litzooData = null;
                try { litzooData = playerData.GetLittleZooModuleData(litzooId); }
                catch (Exception e) { string.Format("动物栏[{0}]还未开启", litzooId); }

                if (litzooData != null)
                {
                    LittleZooProperty zooProp = missionCell.ParseLittleZooProperty();
                    if (zooProp == LittleZooProperty.TicketPrice)
                        return litzooData.littleZooTicketsLevel;
                    else if (zooProp == LittleZooProperty.Capacity)
                        return litzooData.littleZooVisitorSeatLevel;
                    else if (zooProp == LittleZooProperty.VisitSpeed)
                        return litzooData.littleZooEnterVisitorSpawnLevel;
                }
                return 0;
            }
            else if (taskType == TaskType.ParkingLevelup)
            {
                ParkingProperty parkProp = missionCell.ParseParkingProperty();
                if (parkProp == ParkingProperty.Revenue)
                    return playerData.GetParkingCenterDataIDIndexOfDataIdx().parkingProfitLevel;
                else if (parkProp == ParkingProperty.Capacity)
                    return playerData.GetParkingCenterDataIDIndexOfDataIdx().parkingSpaceLevel;
                else if (parkProp == ParkingProperty.VisitorFlowSpeed)
                    return playerData.GetParkingCenterDataIDIndexOfDataIdx().parkingEnterCarSpawnLevel;
            }
            else if (taskType == TaskType.EntryGateLevelup)
            {
                EntryGateProperty entryProp = missionCell.ParseEntryGateProperty();
                if (entryProp == EntryGateProperty.TicketPrice)
                    return playerData.GetEntryDateDataIDIndexOfDataIdx().entryTicketsLevel;
                else if (entryProp == EntryGateProperty.Entrance)
                {
                    GateData entryData = null;
                    int entryId = missionCell.GetEntryGateId();
                    try { entryData = playerData.GetEntryGateIDIndexOfDataIdx(entryId); }
                    catch (Exception e) { string.Format("售票口[{0}]还未开启", entryId); }

                    return entryData != null ? entryData.level : 0;
                }   
            }
            return 0;
        }

        /// <summary>
        /// 任务是否Cleared
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public bool IsTaskCleared(int taskId)
        {
            GuideMissionPlayerDataMSS.TaskStateMSS taskState = GetTaskState(taskId);
            return taskState == null ? false : taskState.isCleared > 0;
        }

        /// <summary>
        /// 所有任务是否完成
        /// </summary>
        /// <returns></returns>
        public bool IsAllTasksCleared()
        {
            foreach (var key in currSceneTaskCells.Keys)
            {
                if (!IsTaskCleared(int.Parse(key)))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 获取某个任务的目标
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public int GetTaskGoal(int taskId)
        {
            Config.missionCell missionCell = GetTaskCell(taskId);
            return missionCell == null ? int.MaxValue : missionCell.need;
        }

        /// <summary>
        /// 获取某个任务奖励道具
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public Config.itemCell GetTaskRewardItem(int taskId)
        {
            Config.missionCell missionCell = GetTaskCell(taskId);
            if (missionCell != null)
                return Config.itemConfig.getInstace().getCell(missionCell.reward);
            return null;
        }

        /// <summary>
        /// 获取某个任务的奖励
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="itemType"></param>
        /// <param name="itemQuantity"></param>
        public void GetTaskReward(int taskId, out ItemType itemType, out System.Numerics.BigInteger itemQuantity)
        {
            itemType = ItemType.Coin;
            var itemCell = GetTaskRewardItem(taskId);
            if (itemCell != null)
                itemQuantity = System.Numerics.BigInteger.Parse(itemCell.itemval); 
        }

        /// <summary>
        /// 获取任务数据
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        public Config.missionCell GetTaskCell(int taskId)
        {
            if (currSceneTaskCells.ContainsKey(taskId.ToString()))
                return currSceneTaskCells[taskId.ToString()];
            return null;
        }
    }

    public class GuideMissionModule : GameModule
    {
        private bool inited = false;
        private bool isAllTaskCleared = false;
        private bool isCurrTaskComplete = false;
        private float checkInterval = 1;
        private float checkDeltaTime = 0;

        public GuideMissionModule(int orderID) : base(orderID) { }
        public override void Tick(int deltaTimeMS) { CheckTaskForUI();  }

        public override void Init()
        {
            missionModel.Init();

            if (missionModel.isFirstGuide)
            {
                missionModel.isFirstGuide = false;
            }

            if (missionModel.currTaskId <= 0) // 从未领取当前场景的任务
                ReceiveTask(missionModel.firstTaskId);
            else
                ReceiveTask(missionModel.currTaskId);

            RegistMessages();
            inited = true;

            LogWarp.Log("-->GuideMissionModule init.");
        }

        public override void Release()
        {
            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.BuildingLevelup, OnBuildingLevelup);
            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.BroadcastOpenNewLittleZoo, OnBroadcastOpenNewLittleZoo);
            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.VisitorReceiveComplete, OnVisitorReceiveComplete);
            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.AdWatchComplete, OnAdWatchComplete);
            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.GuideTaskActionButtonClick, OnTaskActionButtonClick);
            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.BuildingUnlock, OnBuildingUnlock);
            GlobalEventHandler.inst.onApplicationPauseOnResume -= OnApplicationPauseOnResume;
        }

       

        private void RegistMessages()
        {
            MessageManager.GetInstance().Regist((int)GameMessageDefine.BuildingLevelup, OnBuildingLevelup);
            MessageManager.GetInstance().Regist((int)GameMessageDefine.BroadcastOpenNewLittleZoo, OnBroadcastOpenNewLittleZoo);
            MessageManager.GetInstance().Regist((int)GameMessageDefine.VisitorReceiveComplete, OnVisitorReceiveComplete);
            MessageManager.GetInstance().Regist((int)GameMessageDefine.AdWatchComplete, OnAdWatchComplete);
            MessageManager.GetInstance().Regist((int)GameMessageDefine.GuideTaskActionButtonClick, OnTaskActionButtonClick);
            MessageManager.GetInstance().Regist((int)GameMessageDefine.BuildingUnlock, OnBuildingUnlock);
            GlobalEventHandler.inst.onApplicationPauseOnResume += OnApplicationPauseOnResume;
        }

        private GuideMissionModel missionModel { get { return GuideMissionModel.GetInstance(); } }

        private void OnBroadcastOpenNewLittleZoo(Message obj)
        {
            BroadcastOpenNewLittleZoo aa = obj as BroadcastOpenNewLittleZoo;
            CheckTasksProgressForOpenNewLittleZoo(aa.littleZooID);
        }

        private bool IsOnlyMainPageShowing()
        {
            UIMainPage mainPage = PageMgr.GetPage<UIMainPage>();
            foreach (var p in PageMgr.allPages.Values)
            {
                if (p == mainPage)
                    continue;
                if (p.gameObject.activeSelf)
                    return false;
            }
            return mainPage.gameObject.activeSelf;
        }

        private void CheckTaskForUI()
        {
            if (!inited || isAllTaskCleared)
                return;

            checkDeltaTime += Time.deltaTime;
            if (checkDeltaTime >= checkInterval)
            {
                checkDeltaTime = 0;
                if (isCurrTaskComplete)
                {
                    UIMainPage mainPage = PageMgr.GetPage<UIMainPage>();
                    if (mainPage != null && IsOnlyMainPageShowing())
                    {
                        if (!mainPage.lastCloseWithManual)
                            mainPage.OpenGuideTaskPanel();
                    }
                }
            }
        }

        private void OnApplicationPauseOnResume(bool isPause)
        {
            if (!isPause)
            {
                if (PageMgr.allPages != null)
                {
                    UIMainPage mainPage = PageMgr.GetPage<UIMainPage>();
                    if (mainPage != null)
                        mainPage.OpenGuideTaskPanel();
                }
            }
        }

        private void SetTaskStateVars()
        {
            int progress, goal;
            isCurrTaskComplete = isCurrTaskComplete = missionModel.GetTaskProgress(missionModel.currTaskId, out progress, out goal);
            isAllTaskCleared = missionModel.IsAllTasksCleared();
        }

        private void ReceiveTask(int taskId)
        {
            missionModel.currTaskId = taskId;
            missionModel.SetTaskProgress(taskId, missionModel.ResolveTaskProgress(taskId));
            SetTaskStateVars();
            GuideMissionStateChanged.Send(missionModel.currTaskId, GuideMissionStateChanged.ChangeDetail_NewTask);
        }

        private void NextTask(int taskId)
        {
            Config.missionCell missionCell = missionModel.GetTaskCell(taskId);
            if (missionCell != null)
            {
                int nextTaskId = missionCell.nextid;
                ReceiveTask(nextTaskId);
            }
        }

        private void ReceiveRewardForTask(int taskId)
        {
            missionModel.SetTaskCleared(taskId);
            ItemType itemType;
            System.Numerics.BigInteger itemQuantity;
            missionModel.GetTaskReward(taskId, out itemType, out itemQuantity);
            SetTaskStateVars();
            MessageInt.Send((int)GameMessageDefine.GetItem, int.Parse(missionModel.GetTaskCell(taskId).reward));
        }

        private void CheckTasksProgressForBuildingLevelup(TaskType taskType, int buildingProperty, int buildingId, int achieveLevel)
        {
            Config.missionCell missionCell = missionModel.GetTaskCell(missionModel.currTaskId);
            if (missionCell != null)
            {
                TaskType tstype = missionCell.ParseTaskType();

                if (tstype == taskType && buildingProperty == missionCell.ParseBuildingProperty() && buildingId == missionCell.GetBuildingId())
                {
                    if (!missionModel.IsTaskCleared(missionModel.currTaskId))
                    {
                        missionModel.SetTaskProgress(missionModel.currTaskId, achieveLevel);
                        GuideMissionStateChanged.Send(missionModel.currTaskId, GuideMissionStateChanged.ChangeDetail_TaskProgress);
                    }
                    SetTaskStateVars();
                }
            }

            #region -- old
            /*var allCell = missionModel.GetAllTaskCell();
            Config.missionCell missionCell;
            TaskType tstype; 
            int progress, goal;
            int intKey = 0;
            foreach (string key in allCell.Keys)
            {
                missionCell = allCell[key];
                intKey = int.Parse(key);
                tstype = missionCell.ParseTaskType();
                if (taskType == TaskType.Unknown || taskType == TaskType.AdWatch || taskType == TaskType.LittleZooVisit)
                    continue;
                
                if (tstype == taskType && buildingProperty == missionCell.ParseBuildingProperty() && buildingId == missionCell.GetBuildingId())
                {
                    missionModel.GetTaskProgress(missionModel.currTaskId, out progress, out goal);
                    if (!missionModel.IsTaskCleared(intKey))
                    {
                        missionModel.SetTaskProgress(missionModel.currTaskId, achieveLevel);
                        if (intKey == missionModel.currTaskId) // 只有当前进行的任务发送消息
                            GuideMissionStateChanged.Send(missionModel.currTaskId, GuideMissionStateChanged.ChangeDetail_TaskProgress);
                    }
                }
            }*/
            #endregion
        }
        private void CheckTasksProgressForOpenNewLittleZoo(int littleZooId)
        {
            Config.missionCell missionCell = missionModel.GetTaskCell(missionModel.currTaskId);
            if (missionCell != null && missionCell.ParseTaskType() == TaskType.OpenNewLittleZoo)
            {

                int progress, goal;

                bool IsTaskCleared = missionModel.IsTaskCleared(missionModel.currTaskId);

                if (!missionModel.IsTaskCleared(missionModel.currTaskId))
                {
                    missionModel.GetTaskProgress(missionModel.currTaskId, out progress, out goal);
                    missionModel.SetTaskProgress(missionModel.currTaskId, progress + 1);
                    GuideMissionStateChanged.Send(missionModel.currTaskId, GuideMissionStateChanged.ChangeDetail_TaskProgress);
                }
                SetTaskStateVars();
            }
        }

        private void CheckTasksProgressForVisitorReceiveComplete(int littleZooId)
        {
            Config.missionCell missionCell = missionModel.GetTaskCell(missionModel.currTaskId);
            if (missionCell != null && missionCell.ParseTaskType() == TaskType.LittleZooVisit)
            {
                if (littleZooId == missionCell.GetLittleZooId())
                {
                    int progress, goal;

                    missionModel.GetTaskProgress(missionModel.currTaskId, out progress, out goal);
                    missionModel.SetTaskProgress(missionModel.currTaskId, progress + 1);
                    GuideMissionStateChanged.Send(missionModel.currTaskId, GuideMissionStateChanged.ChangeDetail_TaskProgress);
                    SetTaskStateVars();
                }
            }
        }

        private void CheckTasksProgressForAdWatchComplete(string adTag)
        {
            Config.missionCell missionCell = missionModel.GetTaskCell(missionModel.currTaskId);
            if (missionCell != null)
            {
                if (adTag == missionCell.GetAdTag())
                {
                    int progress, goal;
                    if (!missionModel.IsTaskCleared(missionModel.currTaskId))
                    {
                        missionModel.GetTaskProgress(missionModel.currTaskId, out progress, out goal);
                        missionModel.SetTaskProgress(missionModel.currTaskId, progress + 1);
                        GuideMissionStateChanged.Send(missionModel.currTaskId, GuideMissionStateChanged.ChangeDetail_TaskProgress);
                    }
                    SetTaskStateVars();
                }
            }
        }

        private void OnBuildingLevelup(Message message)
        {
            BuildingLevelup bdmessage = message as BuildingLevelup;

            TaskType taskType = (TaskType)bdmessage.buildingType; // 目前TaskType == BuildingType
            CheckTasksProgressForBuildingLevelup(taskType, bdmessage.buildingProperty, bdmessage.buildingId, bdmessage.currLevel);
        }

        private void OnVisitorReceiveComplete(Message message)
        {
            VisitorReceiveComplete vrmessage = message as VisitorReceiveComplete;
            CheckTasksProgressForVisitorReceiveComplete(vrmessage.littleZooId);
        }

        private void OnAdWatchComplete(Message message)
        {
            AdWatchComplete awmessage = message as AdWatchComplete;
            CheckTasksProgressForAdWatchComplete(awmessage.adTag);
        }

        private void OnBuildingUnlock(Message message)
        {
            int buildingId = (message as BuildingUnlock).buildingId;
            Config.missionCell missionCell = missionModel.GetTaskCell(missionModel.currTaskId);
            if (missionCell != null && missionCell.ParseTaskType() == TaskType.LittleZooLevelup && missionCell.GetBuildingId() == buildingId)
            {
                missionModel.SetTaskProgress(missionModel.currTaskId, missionModel.ResolveTaskProgress(missionModel.currTaskId));
                GuideMissionStateChanged.Send(missionModel.currTaskId, GuideMissionStateChanged.ChangeDetail_TaskProgress);
            }   
        }

        private void PostionSceneTo(Vector3 worldPos)
        {
            ZooCamera.GetInstance().PointAtScreenUpCenter(worldPos);
        }

        private void CloseOthersPageAndTaskPanel()
        {
            foreach (var page in PageMgr.allPages.Values)
            {
                if (page is UIMainPage)
                    continue;
                PageMgr.ClosePage(page);
            }

            UIMainPage mainPage = PageMgr.GetPage<UIMainPage>();
            if (mainPage != null)
                mainPage.CloseGuideTaskPanel();
        }

        private void PositionToTask(int taskId)
        {
            Config.missionCell missionCell = missionModel.GetTaskCell(taskId);
            if (missionCell != null && missionCell.skip > 0)
            {
                int progress, goal;
                missionModel.GetTaskProgress(taskId, out progress, out goal);
                if (progress < 1 && missionCell.ParseTaskType() == TaskType.LittleZooLevelup) // 此动物栏还未开启
                    return;

                CloseOthersPageAndTaskPanel();

                TaskType taskType = missionCell.ParseTaskType();
                int buildingId = missionCell.GetBuildingId();
                if (taskType == TaskType.ParkingLevelup)
                {
                    PageMgr.ShowPage<UIParkPage>();
                    UIInteractive.GetInstance().iPage = new UIParkPage();
                    PostionSceneTo(GlobalDataManager.GetInstance().zooGameSceneData.GetParkingFocusPoint(buildingId));
                }
                else if (taskType == TaskType.EntryGateLevelup)
                {
                    PageMgr.ShowPage<UIEntryPage>();
                    UIInteractive.GetInstance().iPage = new UIEntryPage();
                    PostionSceneTo(GlobalDataManager.GetInstance().zooGameSceneData.GetEntryGateGroupFocusPoint());
                }
                else if (taskType == TaskType.LittleZooLevelup)
                {
                    PageMgr.ShowPage<UIZooPage>(buildingId);
                    UIInteractive.GetInstance().iPage = new UIZooPage();
                    PostionSceneTo(GlobalDataManager.GetInstance().zooGameSceneData.GetLittleZooFocusPoint(buildingId));
                }
                else if (taskType == TaskType.LittleZooVisit)
                {
                    // 不跳转
                }
                else if (taskType == TaskType.AdWatch)
                {
                    //PageMgr.ShowPage<UIAdvertActivityPage>();

                    PageMgr.ShowPage<UINewCurrencyAdvertPage>(AdTagFM.Add_Double_Advert);


                }
                else if (taskType == TaskType.OpenNewLittleZoo)
                {
                    PageMgr.ShowPage<UIBuildOpenPage>(buildingId);  //开启新的动物园交互
                    UIInteractive.GetInstance().iPage = new UIZooPage();
                    PostionSceneTo(GlobalDataManager.GetInstance().zooGameSceneData.GetLittleZooFocusPoint(buildingId));
                }
            }
        }

        private IEnumerator AfterTaskRewardReceived()
        {
            UIMainPage mainPage = PageMgr.GetPage<UIMainPage>();
            if (mainPage != null)
                mainPage.OnMoneyEffect();
            yield return new UnityEngine.WaitForSeconds(0.2f);
            NextTask(missionModel.currTaskId);
        }

        private void OnTaskActionButtonClick(Message message)
        {
            int progress, goal;
            bool completed = missionModel.GetTaskProgress(missionModel.currTaskId, out progress, out goal);
            if (completed && !missionModel.IsTaskCleared(missionModel.currTaskId))
            {
                ReceiveRewardForTask(missionModel.currTaskId);
                GameManager.GetInstance().StartCoroutine(AfterTaskRewardReceived());
            }
            else
            {
                PositionToTask(missionModel.currTaskId);
            }
        }
    }

    public class GlobalEventHandler : MonoBehaviour
    {
        private static GlobalEventHandler _inst;

        public static GlobalEventHandler inst
        {
            get
            {
                if (_inst == null)
                {
                    _inst = FindObjectOfType<GlobalEventHandler>();
                    if (_inst == null)
                    {
                        GameObject go = new GameObject("[GlobalEventHandler]");
                        DontDestroyOnLoad(go);
                        _inst = go.AddComponent<GlobalEventHandler>();
                    }
                }
                return _inst;
            }
        }

        public delegate void ApplicationPauseOnResume(bool isPause);

        public ApplicationPauseOnResume onApplicationPauseOnResume;

        void OnApplicationFocus(bool hasFocus)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer ||
                Application.platform == RuntimePlatform.OSXEditor)
            {
                onApplicationPauseOnResume?.Invoke(!hasFocus);
            }
        }

        void OnApplicationPause(bool pauseStatus)
        {
            if (Application.platform == RuntimePlatform.Android ||
                Application.platform == RuntimePlatform.WindowsEditor)
            {
                onApplicationPauseOnResume?.Invoke(pauseStatus);
            }
        }

        void OnApplicationQuit()
        {
            
        }
    }
}
