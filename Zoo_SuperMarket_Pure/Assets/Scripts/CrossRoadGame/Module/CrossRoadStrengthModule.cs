using Game;
using Game.GlobalData;
using Game.MessageCenter;
using System;
using System.Collections;
using System.Collections.Generic;
using UFrame;
using UFrame.Logger;
using UFrame.MessageCenter;
using UnityEngine;

namespace CrossRoadGame
{
    /// <summary>
    /// 体力模块，每小时恢复1点，最多10点
    /// </summary>
    public class CrossRoadStrengthModule : GameModule
    {
        public CrossRoadStrengthModule(int orderID) : base(orderID) { }

        //VoidParamIntCDs multiCD = null;
        MultiIntCD multiCD = null;
        /// <summary>
        /// 最大点数
        /// </summary>
        int maxStrength { get { return Config.globalConfig.getInstace().MaxStrength; } }

        /// <summary>
        /// 恢复1点的时间(秒)
        /// </summary>
        int increaseOnePointSec {
            get {
                return (int)(double)Config.globalConfig.getInstace().IncreaseOnePointSec;
            }
        }

        PlayerData playerdata {
            get {
                return GlobalDataManager.GetInstance().playerData;
            }
        }

        public override void Init()
        {
            MessageManager.GetInstance().Regist(
                (int)GameMessageDefine.AddStrength,
                OnAddStrength);
        }

        public override void Release()
        {
            MessageManager.GetInstance().UnRegist(
                (int)GameMessageDefine.AddStrength,
                OnAddStrength);

            //multiCD.Release();
        }

        public override void Tick(int deltaTimeMS)
        {
            if (!CouldRun())
            {
                return;
            }

            if (multiCD != null)
            {
                multiCD.Tick(deltaTimeMS);
            }
        }

        public override void Run()
        {
            base.Run();
            if (multiCD == null)
            {
                //multiCD = new VoidParamIntCDs();
                multiCD = new MultiIntCD();
                AddStrengthCD();
                multiCD.Run();
            }
        }

        public override void Stop()
        {
            base.Stop();
            if (multiCD != null)
            {
                multiCD.Stop();
            }
        }

        public override void Pause()
        {
            base.Pause();
            if (multiCD != null)
            {
                multiCD.Pause();
            }
        }
        /// <summary>
        /// 增加体力恢复时间CD
        /// </summary>
        void AddStrengthCD()
        {
            var tsIncreased = new TimeSpan(playerdata.playerLittleGame.increaseStrengthTicks);
            var tsNow = new TimeSpan(DateTime.Now.Ticks);
            double maxDays = (double)(maxStrength * increaseOnePointSec) / 86400d;
            double totalDays = (tsNow - tsIncreased).TotalDays;
            if (totalDays < 0)
            {
                throw new System.Exception("体力时间异常");
            }

            playerdata.playerLittleGame.strength = Mathf.Clamp(
                playerdata.playerLittleGame.strength += (int)((totalDays / maxDays) * maxStrength), 0, maxStrength);

            int idx = maxStrength - playerdata.playerLittleGame.strength;
            if (idx > 0)
            {
                var increase = new TimeSpan(DateTime.Now.Ticks - playerdata.playerLittleGame.increaseStrengthTicks);
                double increaseStrengthTicks = increase.TotalSeconds;
                playerdata.playerLittleGame.increaseStrengthTicks = DateTime.Now.Ticks;
                playerdata.playerLittleGame.intCD = multiCD.AddCD((increaseOnePointSec - (int)increaseStrengthTicks) * 1000, CallbackStrengthCD);
            }

        }

        void CallbackStrengthCD(int arg1, IntCD intCD)
        {
            if (playerdata.playerLittleGame.strength < maxStrength)
            {
                ++playerdata.playerLittleGame.strength;
                MessageManager.GetInstance().Send((int)GameMessageDefine.BroadcastChangedStrength);
                OnAddMultiCD();
            }
        }

        void OnAddMultiCD()
        {
            if (playerdata.playerLittleGame.strength < maxStrength)
            {
                playerdata.playerLittleGame.intCD = multiCD.AddCD((increaseOnePointSec) * 1000, CallbackStrengthCD);
                playerdata.playerLittleGame.increaseStrengthTicks = DateTime.Now.Ticks;

            }
        }

        /// <summary>
        /// 监听体力需要修改的消息
        /// </summary>
        /// <param name="msg"></param>
        protected void OnAddStrength(Message msg)
        {
            var _msg = msg as UFrame.MessageInt;

            if (playerdata.playerLittleGame.strength + _msg.val < 0)
            {
                string e = string.Format("体力小于0  之前{0},  增加{1}",
                    playerdata.playerLittleGame.strength, _msg.val);
                throw new System.Exception(e);
            }
            playerdata.playerLittleGame.strength += _msg.val;
            MessageManager.GetInstance().Send((int)GameMessageDefine.BroadcastChangedStrength);

            if (_msg.val < 0)
            {//变化值小于0    消耗
                IsOnAddMultiCD();
            }
        }

        /// <summary>
        /// 体力被消耗，判断时候是10变9还是其他
        /// </summary>
        private void IsOnAddMultiCD()
        {
            if (maxStrength - playerdata.playerLittleGame.strength == 1)
            {
                OnAddMultiCD();
            }
        }
    }
}

