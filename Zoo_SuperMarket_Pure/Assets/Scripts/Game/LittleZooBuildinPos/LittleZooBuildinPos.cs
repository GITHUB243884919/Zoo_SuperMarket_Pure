/*******************************************************************
* FileName:     LittleZooBuildinPos.cs
* Author:       Fan Zheng Yong
* Date:         2019-8-17
* Description:  
* other:    
********************************************************************/


using System.Collections;
using System.Collections.Generic;
using UFrame;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 动物栏内置点，广告位，等待位
    /// </summary>
    public class LittleZooBuildinPos
    {
        /// <summary>
        /// 动物栏名称，跟表一致
        /// </summary>
        public int LittleZooID;

        /// <summary>
        /// 观光位
        /// </summary>
        public List<Vector3> visitPosList = new List<Vector3>();

        /// <summary>
        /// 等待位
        /// </summary>
        public List<Vector3> waitPosList = new List<Vector3>();

        /// <summary>
        /// 动物初始位置
        /// </summary>
        public List<Vector3> animalPosList = new List<Vector3>();

    }
}
