/*******************************************************************
* FileName:     EntityShip_Helper.cs
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
using Game.GlobalData;

namespace Game
{
    public partial class EntityShip : EntityMovable
    {
        /// <summary>
        /// 轮船开过来，下指定数量的游客
        /// </summary>
        /// <param name="maxSpawnVisitorNum"></param>
        public static void GetoffVisitor(int maxSpawnVisitorNum)
        {
            LogWarp.LogError("GetoffVisitor");
            EntityShip entity;

            int sceneID = GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID;
            int visitorpathID = Config.sceneConfig.getInstace().getCell(sceneID).visitorpath;
            Config.sceneaddvisitorCell sceneaddvisitorCell = Config.sceneaddvisitorConfig.getInstace().getCell(visitorpathID);

            switch (visitorpathID)
            {
                case 0:
                    entity = EntityManager.GetInstance().GetRandomEntity(ResType.Ship, EntityFuncType.Ship) as EntityShip;
                    break;
                case 1:
                    entity = EntityManager.GetInstance().GetRandomEntity(ResType.Train, EntityFuncType.Train) as EntityShip;
                    break;
                default:
                    entity = EntityManager.GetInstance().GetRandomEntity(ResType.Ship, EntityFuncType.Ship) as EntityShip;
                    break;
            }

            EntityManager.GetInstance().AddToEntityMovables(entity);
            if (entity.followPath == null)
            {
                entity.followPath = new FollowPath();
            }
            entity.moveSpeed = sceneaddvisitorCell.movespeed;
            var path = PathManager.GetInstance().GetPath(sceneaddvisitorCell.intopath);
            entity.position = path[0];
            entity.followPath.Init(entity, path, path[0], 0, entity.moveSpeed, false);
            entity.maxSpawnVisitorNum = maxSpawnVisitorNum;
            entity.visitorGetOffInterval = Math_F.FloatToInt1000(Config.globalConfig.getInstace().ShipVisitorGetOffInterval);
            if (entity.fsmMachine == null)
            {
                entity.fsmMachine = new FSMMachineShip(entity);

                entity.fsmMachine.AddState(new StateShipGoto((int)ShipState.Goto,
                    entity.fsmMachine));
                entity.fsmMachine.AddState(new StateShipGoback((int)ShipState.Goback,
                    entity.fsmMachine));

                entity.fsmMachine.SetDefaultState((int)ShipState.Goto);
            }
            else
            {
                entity.fsmMachine.GotoState((int)ShipState.Goto);
            }
            entity.Active();
        }
    }

}
