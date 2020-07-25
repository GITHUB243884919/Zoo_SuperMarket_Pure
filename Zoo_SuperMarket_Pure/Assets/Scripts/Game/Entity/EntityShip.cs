﻿/*******************************************************************
* FileName:     EntityShip.cs
* Author:       Fan Zheng Yong
* Date:         2019-10-28
* Description:  
* other:    
********************************************************************/


using UFrame;
using UFrame.BehaviourFloat;
using UFrame.EntityFloat;
using UFrame.Logger;
using System.Collections.Generic;
using UnityEngine;
using UFrame.MessageCenter;
using Game.MessageCenter;
using Game.Path.StraightLine;

namespace Game
{
    public partial class EntityShip : EntityMovable
    {
        public static ObjectPool<EntityShip> pool = new ObjectPool<EntityShip>();

        public FollowPath followPath;
        public FSMMachine fsmMachine;

        public int maxSpawnVisitorNum;
        public int visitorGetOffInterval;
        public override void Active()
        {
            base.Active();
            Reset();
            followPath.Run();
            fsmMachine.Run();
        }

        public override void Deactive()
        {
            //移动到看不见的地方
            this.position = Const.Invisible_Postion;
            this.followPath.Stop();
            fsmMachine.Stop();
            this.entityID = Const.Invalid_Int;
            Reset();

            base.Deactive();
        }

        public void Reset()
        {
        }

        public override void Tick(int deltaTimeMS)
        {
            if (!this.CouldActive())
            {
                return;
            }

            this.followPath.Tick(deltaTimeMS);
            fsmMachine.Tick(deltaTimeMS);

        }

        public override void OnDeathToPool()
        {
            this.Deactive();
            base.OnDeathToPool();
        }

        public override void OnRecovery()
        {
            base.OnRecovery();
            this.Deactive();
            this.followPath.Release();
            followPath = null;

            fsmMachine.Release();
            this.fsmMachine = null;
        }
    }

}
