﻿/*******************************************************************
* FileName:     FSMGameLoad.cs
* Author:       Fan Zheng Yong
* Date:         2019-12-18
* Description:  
* other:    
********************************************************************/


using System.Collections;
using System.Collections.Generic;
using UFrame;
using UnityEngine;

namespace Game
{
    public class FSMGameLoad : FSMMachine
    {
        //public EntityShip ownerEntity;
        //public List<int> loadGroup;

        //public FSMGameLoad(List<int> loadGroup)

        public int sceneID = 0;
        public FSMGameLoad()
        {
            //loadGroup = new List<int>();
            //this.loadGroup = loadGroup;
        }

        public override void Release()
        {
            //ownerEntity = null;
            base.Release();
        }
    }
}
