﻿/*******************************************************************
* FileName:     StateLoadOrgSceneForGame.cs
* Author:       Fan Zheng Yong
* Date:         2019-12-18
* Description:  
* other:    
********************************************************************/


using Game.Path.StraightLine;
using UFrame;
using UFrame.MessageCenter;
using UFrame.Logger;
using Game.MessageCenter;
using System;
using UnityEngine;
using System.Collections.Generic;
using UFrame.MiniGame;
using Game.GlobalData;

namespace Game
{
    /// <summary>
    /// 加载原始场景
    /// </summary>
    public class StateLoadOrgSceneForGame : FSMState
    {
        bool isToStateLoadPart = false;

        public StateLoadOrgSceneForGame(int stateName, FSMMachine fsmCtr) :
            base(stateName, fsmCtr)
        {
        }

        public override void Enter(int preStateName)
        {
            base.Enter(preStateName);

            isToStateLoadPart = false;


            //SceneMgr.Inst.LoadSceneAsync(Config.globalConfig.getInstace().ZooSceneName,
            //    FinishedCallback, ProcessCallback);

            int sceneID = (this.fsmCtr as FSMGameLoad).sceneID;
            string sceneName = Config.sceneConfig.getInstace().getCell(sceneID).resourceid;
            SceneMgr.Inst.LoadSceneAsync(sceneName, FinishedCallback, ProcessCallback);
        }

        protected void FinishedCallback()
        {
            (PageMgr.allPages["UILoading"] as UILoading).SliderValueLoading(1f / 6f);

            isToStateLoadPart = true;
        }

        protected void ProcessCallback(float process)
        {
            (PageMgr.allPages["UILoading"] as UILoading).SliderValueLoading(process / 6f);
        }

        public override void Tick(int deltaTimeMS)
        {
        }

        public override void Leave()
        {
            base.Leave();
        }

        public override void AddAllConvertCond()
        {
            AddConvertCond((int)GameLoaderState.LoadPartScenes, ToStateLoadPart);
        }

        protected bool ToStateLoadPart()
        {
            return isToStateLoadPart;
        }
    }
}
