using System.Collections;
using System.Collections.Generic;

namespace Game.GlobalData
{
    /// <summary>
    /// 引导任务数据 --以作废
    /// </summary>
    [System.Serializable]
    public class GuideMissionPlayerData
    {
        [System.Serializable]
        public class TaskState
        {
            public int taskId;
            public int progress = 0;
            public int isCleared = 0;
        }

        [System.Serializable]
        public class MissionProgress
        {
            public int sceneId = -1;
            public int currTaskId = -1;
        }

        public int isFirst = 1;
        public int selfGuideComplete = 0;
        public List<MissionProgress> missionProgress = new List<MissionProgress>();
        public List<TaskState> taskStates = new List<TaskState>();
    }

    [System.Serializable]
    public class GuideMissionPlayerDataMSS
    {
        [System.Serializable]
        public class TaskStateMSS
        {
            public int taskId;
            public int progress = 0;
            public int isCleared = 0;
        }

        [System.Serializable]
        public class MissionProgressMSS
        {
            public int sceneId = -1;
            public int currTaskId = -1;
        }

        public int isFirst = 1;
        public int selfGuideComplete = 0;
        public List<MissionProgressMSS> missionProgress = new List<MissionProgressMSS>();
        public List<TaskStateMSS> taskStates = new List<TaskStateMSS>();
    }
}