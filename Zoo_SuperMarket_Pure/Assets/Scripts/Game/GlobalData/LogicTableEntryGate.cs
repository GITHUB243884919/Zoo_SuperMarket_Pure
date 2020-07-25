using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.GlobalData
{
    public class LogicTableEntryGate
    {
        //public List<int> sortGateIDs;

        public Dictionary<int, List<int>> sortSceneGateID = new Dictionary<int, List<int>>();

        public LogicTableEntryGate()
        {
            Init();
        }

        protected void Init()
        {
            foreach(var k in Config.sceneConfig.getInstace().AllData.Keys)
            {
                int sceneID = UFrame.Const.Invalid_Int;
                if (!int.TryParse(k, out sceneID))
                {
                    string e = string.Format("scene 表 主键不是数字 {0}", k);
                    throw new System.Exception(e);
                }
                AddScene(sceneID);
            }
        }

        public void AddScene(int sceneID)
        {
            List<int> sortGateIDs = null;
            if (!sortSceneGateID.TryGetValue(sceneID, out sortGateIDs))
            {
                sortGateIDs = new List<int>();
                foreach (var tick in Config.ticketConfig.getInstace().AllData)
                {
                    int tickSceneID = tick.Value.scene;
                    if (tickSceneID == sceneID)
                    {
                        int entryID = UFrame.Const.Invalid_Int;
                        if (!int.TryParse(tick.Key, out entryID))
                        {
                            string e = string.Format("ticket.Key 表 主键不是数字 {0}", tick.Key);
                            throw new System.Exception(e);
                        }
                        sortGateIDs.Add(entryID);
                    }

                }
                sortGateIDs.Sort();
                sortSceneGateID.Add(sceneID, sortGateIDs);
            }
        }

        public List<int> GetSortGateIDs(int sceneID)
        {
            List<int> sortGateIDs = null;
            this.sortSceneGateID.TryGetValue(sceneID, out sortGateIDs);
            return sortGateIDs;
        }

    }

}

