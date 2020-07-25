using DG.Tweening;
using Game;
using Game.GlobalData;
using Game.MessageCenter;
using UFrame.Logger;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UFrame;
using UFrame.MessageCenter;
using UnityEngine;
using UnityEngine.UI;
using UFrame.MiniGame;

public partial class UINewCurrencyAdvertPage : UIPage
{

    /// <summary>
    /// 获取免费气球的收益加倍系数
    /// </summary>
    /// <returns></returns>
    private int GetSceneAdcoefficient()
    {
        var sceneStates = playerData.playerZoo.scenePlayerDataMSS.sceneStates;
        int adcoefficient = 0;
        int scenetype = Config.sceneConfig.getInstace().getCell(playerData.playerZoo.currSceneID).scenetype;
        for (int i = 0; i < sceneStates.Count; i++)
        {
            int _scenetype = Config.sceneConfig.getInstace().getCell(sceneStates[i].sceneId).scenetype;
            if (_scenetype == scenetype)
            {
                int _adcoefficient = Config.sceneConfig.getInstace().getCell(sceneStates[i].sceneId).adcoefficient;
                if (adcoefficient < _adcoefficient)
                {
                    adcoefficient = _adcoefficient;
                }
            }
        }
        return adcoefficient;
    }
    #region 看视频成功事件
    /// <summary>
    /// 增加游客
    /// </summary>
    private void LockAdsSucceed_Tourist()
    {
        EntityShip.GetoffVisitor(numberVisitor);   //轮船游客到来
        playerData.playerZoo.playerNumberOfVideosWatched.SetNumberOfVideoViews(AdTagFM.Add_Tourist_Advert);
        monitorCell = Config.monitorConfig.getInstace().getCell(18);
        AdWatchComplete.Send(AdWatchComplete.AdType_RewardedVideo, AdTagFM.Add_Tourist_Advert.ToString());
    }
    /// <summary>
    /// 快速浏览动物栏
    /// </summary>
    private void LockAdsSucceed_Visit()
    {
        BroadcastNum.Send((int)GameMessageDefine.AddBuff, 10, 0, 0);    //动物栏观光时间
        playerData.playerZoo.playerNumberOfVideosWatched.SetNumberOfVideoViews(AdTagFM.Add_Visit_Advert);
        MessageManager.GetInstance().Send((int)GameMessageDefine.ImmediateFinishVisitCD);
    }
    /// <summary>
    /// 快速售票售票口
    /// </summary>
    private void LockAdsSucceed_Ticket()
    {
        BroadcastNum.Send((int)GameMessageDefine.AddBuff, 12, 0, 0);    //售票口时间
        playerData.playerZoo.playerNumberOfVideosWatched.SetNumberOfVideoViews(AdTagFM.Add_Ticket_Advert);
        MessageManager.GetInstance().Send((int)GameMessageDefine.ImmediateFinishEntryGateCheckInCD);
    }

    /// <summary>
    /// 免费气球广告
    /// </summary>
    private void LockAdsSucceed_FreeItem()
    {
        SetValueOfPlayerData.Send((int)GameMessageDefine.AddCoinOfPlayerDataMSSC, 0, freeItemRwdCoinQuantity, 0); //贵宾定时广告
        GameManager.GetInstance().StartCoroutine(FinishMoneyEffect());
        playerData.playerZoo.playerNumberOfVideosWatched.SetNumberOfVideoViews(AdTagFM.Add_Viptiming_Advert);
    }
    /// <summary>
    /// 离线广告视频
    /// </summary>
    private void LockAdsSucceed_Offline()
    {
        offlineRewardCoin *= waitingADProfit;
        GlobalDataManager.GetInstance().playerData.playerZoo.playerNumberOfVideosWatched.SetNumberOfVideoViews(AdTagFM.Add_Offline_Advert);
        var cell = Config.monitorConfig.getInstace().getCell(26);
        AdWatchComplete.Send(AdWatchComplete.AdType_RewardedVideo, AdTagFM.Add_Offline_Advert.ToString());

    }
    /// <summary>
    /// 增益翻倍广告
    /// </summary>
    private void LockAdsSucceed_Double()
    {
        PlayerNumberOfVideosWatched playerNumberOfVideosWatched = GlobalDataManager.GetInstance().playerData.playerZoo.playerNumberOfVideosWatched;

        if (playerNumberOfVideosWatched.playerLockGainDoubleAdsVideoCount < 6)
        {
            AdWatchComplete.Send(AdWatchComplete.AdType_RewardedVideo, AdTagFM.Add_Double_Advert.ToString());
            BroadcastNum.Send((int)GameMessageDefine.AddBuff, 14, 0, 0);
        }
        playerData.playerZoo.playerNumberOfVideosWatched.SetNumberOfVideoViews(AdTagFM.Add_Double_Advert);

    }
    #endregion

    public IEnumerator FinishMoneyEffect()
    {
        yield return new WaitForSeconds(0.1f);
        UIMainPage uIMainPage = PageMgr.GetPage<UIMainPage>();
        uIMainPage.OnMoneyEffect();//飘钱特效
    }

    /// <summary>
    /// 离线金币计算
    /// 计算所第一个场景的离线收益
    /// </summary>
    /// <returns></returns>
    protected void CalcOfflineRewardCoinMMSC()
    {
        //离线时间
        long offlineSeconds = (long)GlobalDataManager.GetInstance().offlineSeconds;

        if (offlineSeconds <= 0)
        {
            PageMgr.ClosePage(this);
        }

        //离线时间不能超出最大值
        if (offlineSeconds >= Config.globalConfig.getInstace().WaitingADTimeMax)
        {
            offlineSeconds = Config.globalConfig.getInstace().WaitingADTimeMax;
        }

        var perMinCoin = PlayerDataModule.CurrScenePerMinCoin(true);
        offlineRewardCoin = offlineSeconds * perMinCoin / 60;
    }

    /// <summary>
    /// 发送消息离线的收益
    /// 奖励金币数可能是正常离线收益，也可能是广告离线收益
    /// 具体数目由参数rewardCoin提供
    /// </summary>
    /// <param name="rewardCoin"></param>
    private void ActualEarnings(BigInteger rewardCoin)
    {
        //离线时间清0
        GlobalDataManager.GetInstance().offlineSeconds = 0;
        SetValueOfPlayerData.Send((int)GameMessageDefine.AddCoinOfPlayerDataMSSC,
            0, rewardCoin, 0);
        this.HideUICallBack();

    }
}
