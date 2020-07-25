using Game.MessageCenter;
using UFrame.Logger;
using System;
using System.Collections;
using System.Collections.Generic;
using UFrame;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class AnimalProp : IComparable<AnimalProp>
    {
        /// <summary>
        /// 动物大类型
        /// </summary>
        /// </summary>
        public int animalType = 0;

        /// <summary>
        /// 动物小类型
        /// </summary>
        public int animalDetailType = 0;

        /// <summary>
        /// 等级
        /// </summary>
        public int lv = 0;

        int IComparable<AnimalProp>.CompareTo(AnimalProp other)
        {
            int ret = this.animalType.CompareTo(other.animalType);
            //大类型比较
            if (ret != 0)
            {
                return ret;
            }
            //大类型相同比较小类型
            return this.animalDetailType.CompareTo(other.animalDetailType);
        }

    }

    /// <summary>
    /// 15版动物栏动物数据类
    /// </summary>
    [Serializable]
    public class PlayerAnimal_MSS_15
    {
		/// <summary>
		/// 玩家拥有动物id
		/// </summary>
        public List<int> animalIDs = new List<int>();

		/// <summary>
		/// 动物属性（按大类型，小类型）
		/// </summary>
        public List<AnimalProp> animalProps = new List<AnimalProp>();

		/// <summary>
		/// 动物等级之和（按大类型，小类型）
		/// </summary>
		public int sumLv = 0;

        private int animalID = Const.Invalid_Int;
        private int animalType = Const.Invalid_Int;
        private int animalDetailType = Const.Invalid_Int;
        private AnimalProp animalProp = new AnimalProp();

        public PlayerAnimal_MSS_15()
        {

        }

        /// <summary>
        /// 新增加动物
        /// </summary>
        /// <param name="animalID"></param>
        public void AddAnimal(int animalID, bool isFindProp)
        {
			var cell = Config.animalupConfig.getInstace().getCell(animalID);
			if (cell == null) 
			{
				string e = string.Format("动物表animalup不存在 {0}", animalID);
				throw new System.Exception(e);
			}

			if (FindAnimalID(animalID))
            {
                string e = string.Format("动物id重复添加 {0}", animalID);
                throw new System.Exception(e);
            }

            animalIDs.Add(animalID);
            //animalIDs.Sort();

            //查询animalProps中是否存在，不存在增加1条等级为1的记录
            if (!isFindProp) 
			{
				return;
			}
            AnimalProp animalProp = null;
            if (!FindAnimalProp(cell.bigtype, cell.smalltype, out animalProp))
            {
                animalProp = new AnimalProp();
                animalProp.animalType = cell.bigtype;
                animalProp.animalDetailType = cell.smalltype;
                animalProp.lv = 1;
				++sumLv;
                animalProps.Add(animalProp);
                //animalProps.Sort();
            }
        }

        /// <summary>
        /// 动物升级
        /// </summary>
        /// <param name="animalID"></param>
        /// <param name="lv"></param>
        public void AnimalLvUp(int animalID, int lv)
        {
            AnimalProp animalProp = GetAnimalProp(animalID);
            if (animalProp == null)
            {
                string e = string.Format("AnimalLvUp 没有获取到动物属性 animalid = {0}", animalID);
                throw new System.Exception(e);
            }
            animalProp.lv += lv;
			sumLv += lv;
		}

        /// <summary>
        /// 获得动物属性
        /// </summary>
        /// <param name="animalID"></param>
        /// <returns></returns>
        public AnimalProp GetAnimalProp(int animalID)
        {
            if (!FindAnimalID(animalID))
            {
                //string e = string.Format("动物id在用户数据中不存在 {0}", animalID);
                //throw new System.Exception(e);
                return null;
            }

            var cell = Config.animalupConfig.getInstace().getCell(animalID);
            AnimalProp animalProp = null;
            if (!FindAnimalProp(cell.bigtype, cell.smalltype, out animalProp))
            {
                string e = string.Format("动物类型属性在用户数据中不存在 动物id{0}, " +
                    "大类型 {1} ，小类型{2}",
                    animalID, cell.bigtype, cell.smalltype);
                throw new System.Exception(e);
            }

            return animalProp;
        }

        public bool FindAnimalID(int animalID)
        {
            if (animalIDs.Count <= 0)
            {
                return false;
            }

            this.animalID = animalID;
            //if (animalIDs.Count <= 100)
            {
                int _animalID = Const.Invalid_Int;
                _animalID = animalIDs.Find(IsAnimalIDEquals);

                return _animalID == animalID;
            }

            int idx = Const.Invalid_Int;
            idx = animalIDs.BinarySearch(animalID);

            return idx >= 0;
        }

        public bool FindAnimalProp(int animalType, int animalDetailType, out AnimalProp animalProp)
        {
            this.animalType = animalType;
            this.animalDetailType = animalDetailType;

            animalProp = null;
            if (animalProps.Count <= 0)
            {
                return false;
            }

            //if (animalProps.Count < 100)
            {
                animalProp = animalProps.Find(IsAnimalPropEquals);
                return animalProp != null;
            }

            this.animalProp.animalType = this.animalType;
            this.animalProp.animalDetailType = this.animalDetailType;
            
            int idx = animalProps.BinarySearch(this.animalProp);
            if (idx >= 0)
            {
                animalProp = animalProps[idx];
                return true;
            }

            return false;
        }

        bool IsAnimalIDEquals(int animalID)
        {
            return this.animalID == animalID;
        }

        bool IsAnimalPropEquals(AnimalProp animalProp)
        {
            return (animalProp.animalType == this.animalType)
                && (animalProp.animalDetailType == this.animalDetailType);
        }

        /// <summary>
        /// 获取大类型小类型对应的某个动物ID
        /// </summary>
        /// <param name="animalType"></param>
        /// <param name="animalDetailType"></param>
        /// <returns></returns>
        public int GetAnimalID(int animalType, int animalDetailType)
        {
            this.animalType = animalType;
            this.animalDetailType = animalDetailType;

            if (FindAnimalProp(animalType, animalDetailType,out AnimalProp animalProp))
            {
                var allData = Config.animalupConfig.getInstace().AllData;

                foreach (var item in allData)
                {
                    if (item.Value.bigtype == animalType&& item.Value.smalltype == animalDetailType)
                    {
                        return int.Parse(item.Key);
                    }
                }
            }

            return Const.Invalid_Int;
        }

    }
}
