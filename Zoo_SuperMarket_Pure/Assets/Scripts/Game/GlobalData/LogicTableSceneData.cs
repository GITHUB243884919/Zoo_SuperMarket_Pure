using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.GlobalData
{
    public class SceneData
    {
        public int sceneID;
        public int openStar;
    }

    public class LogicTableSceneData
    {
        public List<SceneData> sceneDataList = new List<SceneData>();
        public LogicTableSceneData()
        {
            Init();
        }

        protected void Init()
        {
            foreach (var kv in Config.sceneConfig.getInstace().AllData)
            {
                var sceneData = new SceneData();
                sceneData.sceneID = int.Parse(kv.Key);
                sceneData.openStar = kv.Value.openstar;
                sceneDataList.Add(sceneData);
            }
            //按openStar升序
            sceneDataList.Sort((a, b) => a.openStar.CompareTo(b.openStar));
        }
    }
}

