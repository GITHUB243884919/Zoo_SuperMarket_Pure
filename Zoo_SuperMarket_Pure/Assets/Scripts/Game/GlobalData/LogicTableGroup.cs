/*******************************************************************
* FileName:     LogicTableGroup.cs
* Author:       Fan Zheng Yong
* Date:         2019-8-16
* Description:  
* other:    
********************************************************************/



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UFrame.Logger;
using UFrame;
using System;

namespace Game.GlobalData
{
    /// <summary>
    /// 根据游戏逻辑，对group表的数据结构进行二次加工，方便使用
    /// </summary>
    public class LogicTableGroup
    {
        /// <summary>
        /// 场景 排序后的组ID ，key为场景ID，value = sortedGroupID
        /// </summary>
        protected Dictionary<int, List<int>> sceneSortedGroupIDs;

        /// <summary>
        /// 场景 排序后动物栏id，key为场景id,value = sortedLittleZooIDs
        /// </summary>
        protected Dictionary<int, Dictionary<int, List<int>>> sceneSortedLittleZooIDs;

        public LogicTableGroup()
        {
            Init();
        }

        protected void Init()
        {
            sceneSortedGroupIDs = new Dictionary<int, List<int>>();
            sceneSortedLittleZooIDs = new Dictionary<int, Dictionary<int, List<int>>>();
        }

        public List<int> GetSortedGroupIDs(int sceneID)
        {
            List<int> result = null;

            this.sceneSortedGroupIDs.TryGetValue(sceneID, out result);
            if (result != null)
            {
                return result;
            }

            result = new List<int>();
            foreach (var kv in Config.groupConfig.getInstace().AllData)
            {
                int groupID;
                if (!int.TryParse(kv.Key, out groupID))
                {
                    string e = string.Format("group 表错误，groupid 不是数字型 {0}", kv.Key);
                    throw new System.Exception(e);
                }
                if (kv.Value.scene == sceneID)
                {
                    result.Add(groupID);
                }
            }

            result.Sort();

            if (result.Count <= 0)
            {
                string e = string.Format("group 没有对应的scene {0}", sceneID);
                throw new System.Exception(e);
            }
            sceneSortedGroupIDs.Add(sceneID, result);

            return result;
        }

        public List<int> GetSortedLittleZooIDs(int sceneID, int groupID)
        {
            List<int> result = null;
            Dictionary<int, List<int>> sortedLittleZooIDs = null;
            this.sceneSortedLittleZooIDs.TryGetValue(sceneID, out sortedLittleZooIDs);
            if (sortedLittleZooIDs != null)
            {
                sortedLittleZooIDs.TryGetValue(groupID, out result);
                if (result == null)
                {
                    string e = string.Format("group 没有对应的scene={0} 或 group={1}", sceneID, groupID);
                    throw new System.Exception(e);
                }
                return result;
            }

            sortedLittleZooIDs = new Dictionary<int, List<int>>();
            foreach (var kv in Config.groupConfig.getInstace().AllData)
            {
                int _groupID;
                if (!int.TryParse(kv.Key, out _groupID))
                {
                    string e = string.Format("group 表错误，groupid 不是数字型 {0}", kv.Key);
                    throw new System.Exception(e);
                }
                if (kv.Value.scene == sceneID)
                {
                    var _littleZooIDs = new List<int>();
                    _littleZooIDs.AddRange(kv.Value.startid);
                    sortedLittleZooIDs.Add(_groupID, _littleZooIDs);
                }
            }

            sortedLittleZooIDs.TryGetValue(groupID, out result);
            if (result == null)
            {
                string e = string.Format("group 没有对应的scene={0} 或 group={1}", sceneID, groupID);
                throw new System.Exception(e);
            }

            sceneSortedLittleZooIDs.Add(sceneID, sortedLittleZooIDs);
            return result;
        }

        public bool GetNextGroupID(int sceneID, int groupID, ref int nextGroupID)
        {
            var sortedGroupID = GetSortedGroupIDs(sceneID);
            int idx = sortedGroupID.IndexOf(groupID);

            //没找到 或者 当前就是最后一个
            if (idx < 0 || idx == sortedGroupID.Count - 1)
            {
                return false;
            }

            nextGroupID = sortedGroupID[idx + 1];

            return true;
        }



        public int FindGroupID(int littleZooID)
        {
            int groupID = Const.Invalid_Int;
            foreach(var kv in Config.groupConfig.getInstace().AllData)
            {
                var cell = kv.Value;
                for (int i = 0; i < cell.startid.Length; i++)
                {
                    if (cell.startid[i] == littleZooID)
                    {
                        int.TryParse(kv.Key, out groupID);
                        return groupID;
                    }
                }
            }

            return groupID;
        }

        /// <summary>
        /// 是不是组中最后一个动物栏（配表顺序）
        /// </summary>
        /// <param name="littleZooID"></param>
        /// <param name="groupID"></param>
        /// <returns></returns>
        public bool IsLastLittleZooID(int littleZooID, ref int groupID)
        {
            foreach (var kv in Config.groupConfig.getInstace().AllData)
            {
                var cell = kv.Value;
                for (int i = 0; i < cell.startid.Length; i++)
                {
                    if (cell.startid[i] == littleZooID)
                    {
                        int.TryParse(kv.Key, out groupID);
                        return i == cell.startid.Length - 1;
                    }
                }
            }

            return false;
        }

        public bool IsTrigerLoadNextGroupID(int sceneID, int littleZooID, ref int nextGroupID)
        {
            int groupID = Const.Invalid_Int;

            if (!IsLastLittleZooID(littleZooID, ref groupID))
            {
                return false;
            }

            if (!GetNextGroupID(sceneID, groupID, ref nextGroupID))
            {
                return false;
            }

            return true;
        }

        public int GetSingleBuildingID(int sceneID, BuildingType buildingType)
        {
            int result = Const.Invalid_Int;
            foreach (var kv in Config.buildupConfig.getInstace().AllData)
            {
                if (kv.Value.buildtype == (int)buildingType && kv.Value.scene == sceneID)
                {
                    int.TryParse(kv.Key, out result);

                    return result;
                }
            }

            return Const.Invalid_Int;
        }

		public int GetFirstGroupID(int sceneID)
		{
			int firstGroupID = Const.Invalid_Int;
			List<int> sceneSortedGroupIDs = null;
			if (!this.sceneSortedGroupIDs.TryGetValue(sceneID, out sceneSortedGroupIDs)) 
			{
				return firstGroupID;
			}

			firstGroupID = sceneSortedGroupIDs[0];

			return firstGroupID;
		}

		public int GetFirstLittleZooID(int sceneID)
		{
			int firstLittleZooID = Const.Invalid_Int;
			int firstGroupID = Const.Invalid_Int;
			firstGroupID = GetFirstGroupID(sceneID);
			if (firstGroupID == Const.Invalid_Int) 
			{
				return firstLittleZooID;
			}

			Dictionary<int, List<int>> groupLittleZoo = null;
			if (!sceneSortedLittleZooIDs.TryGetValue(sceneID, out groupLittleZoo)) 
			{
				return firstLittleZooID;
			}

			List<int> littleZoo = null;
			if (!groupLittleZoo.TryGetValue(firstGroupID, out littleZoo)) 
			{
				return firstLittleZooID;
			}

			firstLittleZooID = littleZoo[0];
			return firstLittleZooID;
		}
    }
}
