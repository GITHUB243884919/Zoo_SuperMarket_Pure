using Game.GlobalData;
using Game.MessageCenter;
using System.Collections;
using System.Collections.Generic;
using UFrame;
using UFrame.MessageCenter;
using UnityEngine;
using System.Numerics;
using UFrame.Logger;
using System;

namespace Game
{
    public partial class PlayerDataModule : GameModule
    {
        protected void Tick_LeaveSceneCD(int deltaTimeMs)
        {
            if (leaveSceneCD == null)
            {
                return;
            }
            int realCDVal = leaveSceneCD.org;
            leaveSceneCD.Tick(deltaTimeMs);
            if (leaveSceneCD.IsRunning() && leaveSceneCD.IsFinish())
            {
                if (leaveSceneCD.cd < 0)
                {
                    realCDVal += (-leaveSceneCD.cd);
                }
                leaveSceneCD.Reset();
                Tick_LeaveSceneCoin(realCDVal);
            }
        }

        protected void Tick_LeaveSceneCoin(int deltaTimeMs)
        {
            var sceneStates = playerData.playerZoo.scenePlayerDataMSS.sceneStates;
            var msg = BroadcastLeaveSceneCoin.PreSend();
            var leaveSceneCoinData = GlobalDataManager.GetInstance().leaveSceneCoinData;
            bool isAddedCoin = false;
            for (int i = 0; i < sceneStates.Count; i++)
            {
                var sceneState = sceneStates[i];
                if ((sceneState.enterCount > 0 || sceneState.sceneId == GameConst.First_SceneID) && 
                    sceneState.sceneId != playerData.playerZoo.currSceneID)
                {
                    var perMinCoin = PlayerDataModule.LeaveScenePerMinCoin(sceneState.sceneId,true);
                    perMinCoin = perMinCoin * deltaTimeMs / 60000;
                    if (perMinCoin > 0)
                    {
                        isAddedCoin = true;
                        playerData.playerZoo.playerCoin.AddCoinByScene(sceneState.sceneId, perMinCoin);
                        msg.AddSceneCoin(sceneState.sceneId, perMinCoin);
                        leaveSceneCoinData.AddCoin(sceneState.sceneId, perMinCoin);
                    }
                }
            }

            if (isAddedCoin)
            {
                //发送离开金币收入消息
                BroadcastLeaveSceneCoin.Send();

                //广播金币收入变化
                MessageManager.GetInstance().Send((int)GameMessageDefine.BroadcastCoinOfPlayerDataMSSC);
            }
        }
    }
}
