using System.Collections;
using System.Collections.Generic;

namespace Game.GlobalData
{
    /// <summary>
    /// 玩家场景数据 --已作废
    /// </summary>
    [System.Serializable]
    public class ScenePlayerData
    {
        [System.Serializable]
        public class SceneState
        {
            /// <summary>
            /// 场景Id
            /// </summary>
            public int sceneId;
            /// <summary>
            /// 是否达到解锁条件，1表示达到解锁条件
            /// </summary>
            public int unlocked = 0;
            /// <summary>
            /// 是否被玩家手动解锁，1表示已解锁
            /// </summary>
            public int browsed = 0;
            /// <summary>
            /// 进入该场景的次数
            /// </summary>
            public int enterCount = 0;
        }

        public int isFirst = 1;
        public List<SceneState> sceneStates = new List<SceneState>();
    }

    [System.Serializable]
    public class ScenePlayerDataMSS
    {
        [System.Serializable]
        public class SceneStateMSS
        {
            /// <summary>
            /// 场景Id
            /// </summary>
            public int sceneId;
            /// <summary>
            /// 是否达到解锁条件，1表示达到解锁条件
            /// </summary>
            public int unlocked = 0;
            /// <summary>
            /// 是否被玩家手动解锁，1表示已解锁
            /// </summary>
            public int browsed = 0;
            /// <summary>
            /// 进入该场景的次数
            /// </summary>
            public int enterCount = 0;
        }

        public int isFirst = 1;
        public List<SceneStateMSS> sceneStates = new List<SceneStateMSS>();
    }
}