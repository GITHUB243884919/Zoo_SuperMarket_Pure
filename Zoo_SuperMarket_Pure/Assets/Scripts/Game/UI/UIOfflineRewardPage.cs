//using DG.Tweening;
//using Game;
//using Game.GlobalData;
//using Game.MessageCenter;
//using UFrame.Logger;
//using System.Collections;
//using System.Collections.Generic;
//using System.Numerics;
//using UFrame;
//using UFrame.MessageCenter;
//using UFrame.MiniGame;
//using UnityEngine;
//using UnityEngine.UI;

//public class UIOfflineRewardPage : UIPage
//{
//    public UIOfflineRewardPage() : base(UIType.Fixed, UIMode.DoNothing, UICollider.None)
//    {
//        uiPath = "uiprefab/UIOfflineReward";
//    }
//    Text rewardText;
//    Button rmbButton;
//    Button advertButton;
//    Button hideUIButton;
//    bool lockAdsBool = false;
//    Text advertButton_Text;
//    Image rewardIcon;
//    //视频广告翻倍倍数
//    int waitingADProfit;
//    /// <summary>
//    /// 离线时间的所有收益（不翻倍）
//    /// </summary>
//    //BigInteger offlineReward;

//    PlayerData playerData { get { return GlobalDataManager.GetInstance().playerData; } }

//    //PlayerCoin offlineRewardCoin = new PlayerCoin();
//    BigInteger offlineRewardCoin = 0;

//    public override void Awake(GameObject go)
//    {
//        base.Awake(go);
//        this.RegistAllCompent();
//        //this.ShowUICallBack();
//        GetTransPrefabAllTextShow(this.transform);
//    }


//    /// <summary>
//    /// 内部组件的查找赋值
//    /// </summary>
//    private void RegistAllCompent()
//    {
//        waitingADProfit = Config.globalConfig.getInstace().WaitingADProfit;

//        rewardText = RegistCompent<Text>("UI/TextGroup/RewardText");

//        rmbButton = AddCompentInChildren(rmbButton, "UI/ButtonGroup/RmbButton");
//        rmbButton = RegistBtnAndClick("UI/ButtonGroup/RmbButton", OnClickDiamond);
//        advertButton = AddCompentInChildren(advertButton, "UI/ButtonGroup/AdvertButton");
//        advertButton = RegistBtnAndClick("UI/ButtonGroup/AdvertButton", OnClickAds);
//        advertButton_Text = RegistCompent<Text>("UI/ButtonGroup/AdvertButton/ButtonText");
//        hideUIButton = AddCompentInChildren(hideUIButton, "UI/TextGroup/CloseButton");
//        hideUIButton = RegistBtnAndClick("UI/TextGroup/CloseButton", OnClickHideUI);

//        rewardIcon = RegistCompent<Image>("UI/TextGroup/RewardIcon");
//    }

//    private void InitCompent()
//    {
//        CalcOfflineRewardCoinMMSC();
//        rewardText.text = MinerBigInt.ToDisplay(offlineRewardCoin);
//        advertButton_Text.text = string.Format(GetL10NString("Ui_Text_21"), waitingADProfit);

//    }

//    /// <summary>
//    /// 离线金币计算
//    /// 所有离线获得金币/2
//    /// 只计算当前场景和当前场景同类型金币场景
//    /// </summary>
//    /// <returns></returns>
//    protected void CalcOfflineRewardCoinMMSC()
//    {
//        //离线时间
//        long offlineSeconds = (long)GlobalDataManager.GetInstance().offlineSeconds;
//        if (offlineSeconds <= 0)
//        {
//            PageMgr.ClosePage(this);
//        }

//        //离线时间不能超出最大值
//        if (offlineSeconds >= Config.globalConfig.getInstace().WaitingADTimeMax)
//        {
//            offlineSeconds = Config.globalConfig.getInstace().WaitingADTimeMax;
//        }

       
//        offlineRewardCoin = PlayerDataModule.OnGetCalculateOfflineSecondCoin(offlineSeconds);
//        //离线只算最新解锁的场景/M收益
//        BigInteger perMinCoin = 0;
//        if (playerData.playerZoo.currSceneID == playerData.playerZoo.lastUnLockSceneID)
//        {
//            perMinCoin = PlayerDataModule.CurrScenePerMinCoin(true);
//        }
//        else
//        {
//            perMinCoin = PlayerDataModule.LeaveScenePerMinCoin(playerData.playerZoo.lastUnLockSceneID, true);
//        }
//        offlineRewardCoin = offlineSeconds * perMinCoin / 60;

//        //offlineRewardCoin /= 2;
//    }

//    /// <summary>
//    /// 钻石加倍
//    /// </summary>
//    /// <param name="name"></param>
//    private void OnClickDiamond(string name)
//    {
//    }
//    /// <summary>
//    /// 广告加倍
//    /// </summary>
//    /// <param name="name"></param>
//    private void OnClickAds(string name)
//    {
//        Game.Event_SDK.AppsFlyerEvent.GetInstance().SendEvent(AppsFlyerEnum.video_start_offline);
//        var cell = Config.monitorConfig.getInstace().getCell(25);
//        TAEvent_VideoAds_Start(cell.keytype, cell.valuetype, GlobalDataManager.GetInstance().playerData.playerZoo.playerNumberOfVideosWatched.playerLockOfflineAdsVideoCount);
//        ThirdPartMgr.ShowRewardedAd(OnGetIsLockAdsSucceedBool);
//    }
//    private void OnGetIsLockAdsSucceedBool(bool isBool)
//    {
//#if TEST_NO_AD_SHOW
//        isBool = true;
//#endif
//        if (!isBool)
//        {
//            return;
//        }

//        lockAdsBool = true;
        
//        offlineRewardCoin *= waitingADProfit;
//        ActualEarnings(offlineRewardCoin);
//        GlobalDataManager.GetInstance().playerData.playerZoo.playerNumberOfVideosWatched.SetNumberOfVideoViews(AdTagFM.Add_Offline_Advert);
//        var cell = Config.monitorConfig.getInstace().getCell(26);
//        AdWatchComplete.Send(AdWatchComplete.AdType_RewardedVideo, AdTagFM.Add_Offline_Advert.ToString());
//        TAEvent_VideoAds_Finish(cell.keytype, cell.valuetype, GlobalDataManager.GetInstance().playerData.playerZoo.playerNumberOfVideosWatched.playerLockOfflineAdsVideoCount);
//        Game.Event_SDK.AppsFlyerEvent.GetInstance().SendEvent(AppsFlyerEnum.video_finish_offline);

//    }

//    private void OnClickHideUI(string name)
//    {
//        ActualEarnings(offlineRewardCoin);
//    }


//    /// <summary>
//    /// 发送消息离线的收益
//    /// 奖励金币数可能是正常离线收益，也可能是广告离线收益
//    /// 具体数目由参数rewardCoin提供
//    /// </summary>
//    /// <param name="rewardCoin"></param>
//    private void ActualEarnings(BigInteger rewardCoin)
//    {
//        //离线时间清0
//        GlobalDataManager.GetInstance().offlineSeconds = 0;
//        SetValueOfPlayerData.Send((int)GameMessageDefine.AddCoinOfPlayerDataMSSC,
//            0, rewardCoin, 0);
//        this.HideUICallBack();
        
//    }

//    private void HideUICallBack()
//    {
//        float timeCount = 0.1f;
//        DOTween.To(() => timeCount, a => timeCount = a, 0.1f, 0.1f).OnComplete(new TweenCallback(delegate
//        {
//            UIMainPage uIMainPage = PageMgr.GetPage<UIMainPage>();
//            uIMainPage.OnMoneyEffect();//飘钱特效
//            Hide();
//        }));
//    }
//    /// <summary>
//    /// 更新:动态修改图片大小
//    /// </summary>
//    public override void Refresh()
//    {
//        base.Refresh();
//    }
//    /// <summary>
//    /// 活跃
//    /// </summary>
//    public override void Active()
//    {
//        base.Active();
//        InitCompent();

//        if (lockAdsBool)
//        {
//            HideUICallBack();
//        }
//        int scenetype = Config.sceneConfig.getInstace().getCell(playerData.playerZoo.currSceneID).moneyid;
//        string iconPath = Config.moneyConfig.getInstace().getCell(scenetype).bigmoneyicon;
//        rewardIcon.sprite = ResourceManager.LoadSpriteFromPrefab(iconPath);
//    }
//    /// <summary>
//    /// 隐藏
//    /// </summary>
//    public override void Hide()
//    {
//        base.Hide();
//        lockAdsBool = false;
//        GlobalDataManager.GetInstance().playerData.isTestOfflineRewardPage = false;
//        GlobalDataManager.GetInstance().playerData.isTestOfflineRewardTime = UFrame.Const.Invalid_Int;
        
//    }
//    /// <summary>
//    /// 玩家打点事件
//    /// </summary>
//    public void TAEvent_VideoAds_Start(string isVideo_type, string isVideo_name, int isVideo_num)
//    {
//        Utils.DebugLogger.GetInstance().Log("测试: 广告类型=" + isVideo_type + "    广告名称=" + isVideo_name + "    广告次数=" + isVideo_num);
//        var gameEventDic = PlayerMonitorEnumUtility.GetInstance().GetAddDictionary_Video_Ads(isVideo_type, isVideo_name, isVideo_num);
//        string gameEventKey = TAEventsMonitorEnum.video_start;
//        ThirdPartTA.Track(gameEventKey, gameEventDic);
//    }

//    public void TAEvent_VideoAds_Finish(string isVideo_type, string isVideo_name, int isVideo_num)
//    {
//        Utils.DebugLogger.GetInstance().Log("测试: 广告类型=" + isVideo_type + "    广告名称=" + isVideo_name + "    广告次数=" + isVideo_num);
//        var gameEventDic = PlayerMonitorEnumUtility.GetInstance().GetAddDictionary_Video_Ads(isVideo_type, isVideo_name, isVideo_num);
//        string gameEventKey = TAEventsMonitorEnum.video_finish;
//        ThirdPartTA.Track(gameEventKey, gameEventDic);
//    }

//}
