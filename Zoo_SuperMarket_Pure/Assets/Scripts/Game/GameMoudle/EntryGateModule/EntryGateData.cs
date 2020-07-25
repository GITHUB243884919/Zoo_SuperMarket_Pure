﻿using UFrame.Logger;
using System;
using System.Collections;
using System.Collections.Generic;
using UFrame;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class EntryGateData
    {
        /// <summary>
        /// 入口ID
        /// </summary>
        public int entryID;

        /// <summary>
        /// 入口等级
        /// </summary>
        public int level;
    }
}