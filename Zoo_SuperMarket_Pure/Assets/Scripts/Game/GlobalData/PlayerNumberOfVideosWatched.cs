using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.GlobalData;
using Game;
using Game.MessageCenter;
using DG.Tweening;
using UFrame.Logger;

[Serializable]
public class PlayerNumberOfVideosWatched 
{
    /// <summary>
    /// (旧)观看离线奖励视频次数
    /// </summary>
    public int playerLockOfflineAdsVideoCount ;
    /// <summary>
    /// 观看增益加倍奖励视频次数
    /// </summary>
    public int playerLockGainDoubleAdsVideoCount ;
    /// <summary>
    ///(旧) 观看增加游客奖励视频次数
    /// </summary>
    public int playerLockVisitorNumberAdsVideoCount;
    /// <summary>
    /// (旧)观看观光加速奖励视频次数
    /// </summary>
    public int playerLockVisitorExpediteAdsVideoCount ;
    /// <summary>
    /// (旧)观看售票加速奖励视频次数
    /// </summary>
    public int playerLockEntryExpediteAdsVideoCount ;
    /// <summary>
    /// (旧)观看免费货币奖励视频次数
    /// </summary>
    public int playerLockFreeMoneyAdsVideoCount;
    /// <summary>
    ///(旧)贵宾定时道具奖励视频次数（热气球）
    /// </summary>
    public int playerFreeItemAdsVideoCount;



    /// <summary>
    /// 玩家观看视频的次数（6次一轮回）
    /// </summary>
    public int playerWatchAdsVideoCount;




    public PlayerNumberOfVideosWatched()
    {
        playerLockOfflineAdsVideoCount = 0;
        playerLockGainDoubleAdsVideoCount = 0;
        playerLockVisitorExpediteAdsVideoCount = 0;
        playerLockEntryExpediteAdsVideoCount = 0;
        playerLockFreeMoneyAdsVideoCount = 0;
        playerFreeItemAdsVideoCount = 0;
        playerWatchAdsVideoCount = 0;
    }
    public void SetResetVideosWatchedData()
    {
        playerLockOfflineAdsVideoCount = 0;
        playerLockGainDoubleAdsVideoCount = 0;
        playerLockVisitorExpediteAdsVideoCount = 0;
        playerLockEntryExpediteAdsVideoCount = 0;
        playerLockFreeMoneyAdsVideoCount = 0;
        playerFreeItemAdsVideoCount = 0;
        GlobalDataManager.GetInstance().playerData.playerZoo.LastLogingDate_Day = System.DateTime.Now.Day;
    }

    public void SetNumberOfVideoViews(AdTagFM adTagFM)
    {
        switch (adTagFM)
        {
            case AdTagFM.Add_Double_Advert:
                if (playerLockGainDoubleAdsVideoCount < Config.globalConfig.getInstace().GoldRewardMaxNumber)
                {
                    playerLockGainDoubleAdsVideoCount += 1;
                    if (playerLockGainDoubleAdsVideoCount == Config.globalConfig.getInstace().GoldRewardMaxNumber)
                    {
                        var rewardNumCoin = PlayerDataModule.AllScenePerMinCoin(true) * Config.globalConfig.getInstace().AdvertGoldReward;
                        //加钱
                        SetValueOfPlayerData.Send((int)GameMessageDefine.AddCoinOfPlayerDataMSSC, 0, rewardNumCoin, 0);
                        float timeCount = 0.1f;
                        DOTween.To(() => timeCount, a => timeCount = a, 0.1f, 0.1f).OnComplete(new TweenCallback(delegate
                        {
                            UIMainPage uIMainPage = PageMgr.GetPage<UIMainPage>();
                            uIMainPage.OnMoneyEffect();//飘钱特效
                        }));
                    }
                }
               
                break;
            case AdTagFM.Add_Tourist_Advert:
                playerLockVisitorNumberAdsVideoCount += 1;
                break;
            case AdTagFM.Add_Ticket_Advert:
                playerLockEntryExpediteAdsVideoCount += 1;
                break;
            case AdTagFM.Add_Visit_Advert:
                playerLockVisitorExpediteAdsVideoCount += 1;
                break;
            case AdTagFM.Add_Offline_Advert:
                playerLockOfflineAdsVideoCount += 1;
                break;
            case AdTagFM.Add_Viptiming_Advert:
                playerFreeItemAdsVideoCount += 1;
                break;
            default:
                break;
        }

        playerWatchAdsVideoCount += 1;



        if (adTagFM == AdTagFM.Add_Double_Advert)
        {

            

        }
        if (playerWatchAdsVideoCount == Config.globalConfig.getInstace().AdvertRewardNumber)
        {
            var rewardNum = Config.globalConfig.getInstace().AdvertRewardRmbNumber;
            //加钻石
            SetValueOfPlayerData.Send((int)GameMessageDefine.SetDiamondOfPlayerData, rewardNum, 0, 0);
            LogWarp.LogErrorFormat("看广告6次    增加了钻石");
            playerWatchAdsVideoCount = 0;
        }
       
    }

}
