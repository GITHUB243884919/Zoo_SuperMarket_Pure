using System.Collections;
using System.Collections.Generic;
using UFrame;
using UnityEngine;

namespace Game.GlobalData
{
    public class ZooGameSceneData
    {
        /// <summary>
        /// 场景的相机父类
        /// </summary>
        public GameObject camera ;

        /// <summary>
        /// 动物栏的父节点
        /// </summary>
        public Transform littleZooParentNode;

        /// <summary>
        /// 停车场地块的父节点
        /// </summary>
        public GameObject ParckingSencePos;

        /// <summary>
        /// 售票口场景挂点数据
        /// </summary>
        public EntryGateSenceData entryGateSenceData;

        public Transform endNode;

        /// <summary>
        /// 额外加载的group
        /// group和动物栏的逻辑不一样
        /// group是有默认加载的情况，也就是场景制作时就有的
        /// </summary>
        Dictionary<int, GameObject> extendLoadGroup;

        Dictionary<int, GameObject> loadLittleZoo;

        public List<int> loadGroup;

        public Dictionary<int, Dictionary<int, SimpleParticle>> visitSeatSpDict;

        public ZooGameSceneData()
        {
            Init();
        }

        protected void Init()
        {
            extendLoadGroup = new Dictionary<int, GameObject>();
            loadLittleZoo = new Dictionary<int, GameObject>();
            entryGateSenceData = new EntryGateSenceData();
            loadGroup = new List<int>();
            visitSeatSpDict = new Dictionary<int, Dictionary<int, SimpleParticle>>();
        }

        public void Release()
        {
            camera = null;
            ParckingSencePos = null;
            foreach (var val in extendLoadGroup.Values)
            {
                GameObject.Destroy(val);
            }
            extendLoadGroup.Clear();

            foreach (var val in loadLittleZoo.Values)
            {
                GameObject.Destroy(val);
            }
            loadLittleZoo.Clear();

            littleZooParentNode = null;
            endNode = null;

            entryGateSenceData.Release();

            loadGroup.Clear();

            foreach(var val in visitSeatSpDict.Values)
            {
                foreach(var sps in val.Values)
                {
                    sps.Release();
                }
                val.Clear();
            }
            visitSeatSpDict.Clear();
        }

        public void AddExtendLoadGroup(int groupID, GameObject go)
        {
            extendLoadGroup.Add(groupID, go);
        }

        public bool IsExtendGroupContains(int groupID)
        {
            return extendLoadGroup.ContainsKey(groupID);
        }

        public int GetExtendLoadGroupCount()
        {
            return extendLoadGroup.Count;
        }

        public void AddLoadLittleZoo(int littleZooID, GameObject go)
        {
            loadLittleZoo.Add(littleZooID, go);
        }

        public void RemoveLoadLittleZoo(int littleZooID)
        {
            GameObject go = null;
            if (!loadLittleZoo.TryGetValue(littleZooID, out go))
            {
                return;
            }

            GameObject.Destroy(go);
            loadLittleZoo.Remove(littleZooID);
        }

        public Vector3 GetLittleZooFocusPoint(int littleZooId)
        {
            return LittleZooPosManager.GetInstance().GetPos(littleZooId);
        }

        public Vector3 GetParkingFocusPoint(int parkingId)
        {
            var dinst = GlobalDataManager.GetInstance();
            Config.parkingCell parkingCell = null;
            foreach (var cell in Config.parkingConfig.getInstace().AllData.Values)
            {
                if (cell.scene == dinst.playerData.playerZoo.currSceneID)
                {
                    parkingCell = cell;
                    break;
                }
            }
            Vector3 ret = Vector3.zero;
            Transform centerTrans = null;
            GameObject go = GameObject.Find("damen");
            if (go != null)
            {
                ret = go.transform.position;
                centerTrans = go.transform.Find(parkingCell.prefabsname + "/Center");
            }
            if (centerTrans != null)
                ret = centerTrans.position;
            return ret;
        }

        public Vector3 GetEntryGateGroupFocusPoint()
        {
            var dinst = GlobalDataManager.GetInstance();
            var sortGateIDs = dinst.logicTableEntryGate.GetSortGateIDs(dinst.playerData.playerZoo.currSceneID);
            Config.ticketCell ticketCell = Config.ticketConfig.getInstace().getCell(sortGateIDs[0]);
            Vector3 ret = Vector3.zero;
            Transform centerTrans = null;
            GameObject go = GameObject.Find("damen"); 
            if (go != null)
            {
                ret = go.transform.position;
                centerTrans = go.transform.Find(ticketCell.prefabsname + "/Center");
            }
            if (centerTrans != null)
                ret = centerTrans.position;
            return ret;
        }
    }
}

