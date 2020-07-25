//using DG.Tweening;
//using Game;
//using Game.GlobalData;
//using Game.MessageCenter;
//using UFrame.Logger;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Numerics;
//using UFrame;
//using UFrame.MessageCenter;
//using UnityEngine;
//using UnityEngine.UI;
//using UFrame.MiniGame;

//public class UIAdvertActivityPage : UIPage
//{

//    Text titleText;
//    Text tipsText_1;
//    Text tipsText_2;
//    Text goldText;
//    Slider slider;
//    Text timeSlider;
//    Text rewardNumText;
//    Text tipsText;
//    Text buttonText;
//    Button advertButton;
//    Button closeButton;
//    Text timeText;
//    BigInteger rewardNumCoin;

//    Image goldIconImage;
//    Image rewardIconImage;
//    public UIAdvertActivityPage() : base(UIType.Fixed, UIMode.DoNothing, UICollider.None, UITickedMode.Update)
//    {
//        uiPath = "uiprefab/UIAdvertActivity";
//    }
//    public override void Awake(GameObject go)
//    {
//        base.Awake(go);
//        //初始化控件
//        this.RegistAllCompent();
//        GetTransPrefabAllTextShow(this.transform);
//    }

//    private void RegistAllCompent()
//    {
//        titleText = RegistCompent<Text>("UIAdvertUpper/UiBg/TextGroup/TitleText");
//        tipsText_1 = RegistCompent<Text>("UIAdvertUpper/UiBg/TextGroup/TipsText_1");
//        tipsText_2 = RegistCompent<Text>("UIAdvertUpper/UiBg/TextGroup/TipsText_2");
//        goldText = RegistCompent<Text>("UIAdvertUpper/UiBg/TextGroup/GoldText");
//        slider = RegistCompent<Slider>("UIAdvertUpper/UiBg/Schedule/Slider");
//        timeSlider = RegistCompent<Text>("UIAdvertUpper/UiBg/Schedule/timeSlider");
//        rewardNumText = RegistCompent<Text>("UIAdvertLower/UiBg/TextGroup/RewardNumText");
//        tipsText = RegistCompent<Text>("UIAdvertLower/UiBg/TextGroup/TipsText");
//        buttonText = RegistCompent<Text>("UIAdvertLower/UiBg/TextGroup/AdvertButton/ButtonBg/ButtonText");
//        timeText = RegistCompent<Text>("UIAdvertUpper/UiBg/Schedule/TimeText");

//        advertButton = AddCompentInChildren<Button>(advertButton, "UIAdvertLower/UiBg/TextGroup/AdvertButton");
//        advertButton = RegistBtnAndClick("UIAdvertLower/UiBg/TextGroup/AdvertButton/ButtonBg", OnClickAdvertPalyButton);
//        closeButton = AddCompentInChildren<Button>(closeButton, "UIAdvertUpper/UiBg/CloseButton");
//        closeButton = RegistBtnAndClick("UIAdvertUpper/UiBg/CloseButton", OnClickCloseButton);

//        goldIconImage = RegistCompent<Image>("UIAdvertUpper/UiBg/TextGroup/GoldIcon");
//        rewardIconImage = RegistCompent<Image>("UIAdvertLower/UiBg/TextGroup/RewardIcon");

//    }

//    private void SetCorrectShowImage()
//    {
//        int scenetype = Config.sceneConfig.getInstace().getCell(GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID).moneyid;
//        string iconPath = Config.moneyConfig.getInstace().getCell(scenetype).moneyicon;
//        string bigMoneyiconPath = Config.moneyConfig.getInstace().getCell(scenetype).bigmoneyicon;
//        goldIconImage.sprite = ResourceManager.LoadSpriteFromPrefab(iconPath);
//        rewardIconImage.sprite = ResourceManager.LoadSpriteFromPrefab(bigMoneyiconPath);
//    }

//    private void OnClickCloseButton(string obj)
//    {
//        float timeCount = 0.1f;
//        DOTween.To(() => timeCount, a => timeCount = a, 0.1f, 0.1f).OnComplete(new TweenCallback(Hide));
//    }
//    private void OnGetIsLockAdsSucceedBool(bool isBool)
//    {
//#if TEST_NO_AD_SHOW
//        isBool = true;
//#endif
//        if (isBool)
//        {
//            PlayerNumberOfVideosWatched playerNumberOfVideosWatched = GlobalDataManager.GetInstance().playerData.playerZoo.playerNumberOfVideosWatched;

//            if (playerNumberOfVideosWatched.playerLockGainDoubleAdsVideoCount < 6)
//            {
//                var playerData = GlobalDataManager.GetInstance().playerData;
//                playerData.playerZoo.playerNumberOfVideosWatched.SetNumberOfVideoViews(AdTagFM.Add_Double_Advert);
//                if (playerData.playerZoo.playerNumberOfVideosWatched.playerLockGainDoubleAdsVideoCount == 6)
//                {
//                    SetValueOfPlayerData.Send((int)GameMessageDefine.AddCoinOfPlayerDataMSSC, 0, rewardNumCoin, 0);

//                    float timeCount = 0.1f;
//                    DOTween.To(() => timeCount, a => timeCount = a, 0.1f, 0.1f).OnComplete(new TweenCallback(delegate
//                    {
//                        UIMainPage uIMainPage = PageMgr.GetPage<UIMainPage>();
//                        uIMainPage.OnMoneyEffect();//飘钱特效
//                    }));
//                    OnClickCloseButton("0");
//                }

//                AdWatchComplete.Send(AdWatchComplete.AdType_RewardedVideo, AdTagFM.Add_Double_Advert.ToString());
//                BroadcastNum.Send((int)GameMessageDefine.AddBuff, 14, 0, 0);
//                var cell = Config.monitorConfig.getInstace().getCell(23);
//                TAEvent_VideoAds_Finish(cell.keytype, cell.valuetype, GlobalDataManager.GetInstance().playerData.playerZoo.playerNumberOfVideosWatched.playerLockGainDoubleAdsVideoCount);
//                Game.Event_SDK.AppsFlyerEvent.GetInstance().SendEvent(AppsFlyerEnum.video_finish_doublebonus);
//            }
//            InitCompent();
//        }
//        else
//        {
//            InitCompent();
//        }
//    }

//    private void OnClickAdvertPalyButton(string obj)
//    {
//        int count = GlobalDataManager.GetInstance().playerData.playerZoo.playerNumberOfVideosWatched.playerLockOfflineAdsVideoCount;
//        int AdvertBUFFTimes = Config.globalConfig.getInstace().AdvertBUFFTimes;
//        if ((AdvertBUFFTimes - count) > 0)
//        {
//            Game.Event_SDK.AppsFlyerEvent.GetInstance().SendEvent(AppsFlyerEnum.video_start_doublebonus);
//            var cell = Config.monitorConfig.getInstace().getCell(23);
//            TAEvent_VideoAds_Start(cell.keytype, cell.valuetype, GlobalDataManager.GetInstance().playerData.playerZoo.playerNumberOfVideosWatched.playerLockGainDoubleAdsVideoCount);
//            ThirdPartMgr.ShowRewardedAd(OnGetIsLockAdsSucceedBool);
//        }
//    }

//    /// <summary>
//    /// 更新
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
//        MessageManager.GetInstance().Regist((int)GameMessageDefine.AddBuffSucceed, this.OnAddBuffSucceed);

//        InitCompent();
//        SetCorrectShowImage();
//    }

//    private void OnAddBuffSucceed(Message obj)
//    {
//        float timeCount = 0.1f;
//        DOTween.To(() => timeCount, a => timeCount = a, 0.1f, 0.1f).OnComplete(new TweenCallback(InitCompent));
//    }

//    private void InitCompent()
//    {
//        rewardNumCoin = PlayerDataModule.AllScenePerMinCoin(true) * Config.globalConfig.getInstace().AdvertGoldReward;
//        var buffCell = Config.buffConfig.getInstace().getCell(14);
//        int count = GlobalDataManager.GetInstance().playerData.playerZoo.playerNumberOfVideosWatched.playerLockGainDoubleAdsVideoCount;
//        int AdvertBUFFTimes = Config.globalConfig.getInstace().AdvertBUFFTimes;

//        tipsText_1.text = string.Format(GetL10NString("Ui_Text_23"), buffCell.multiple, UFrame.Math_F.IntToFloat_SToHr(buffCell.time));
//        tipsText_2.text = string.Format(GetL10NString("Ui_Text_24"), 2);
//        goldText.text = string.Format(GetL10NString("Ui_Text_21"), buffCell.multiple);
//        rewardNumText.text = MinerBigInt.ToDisplay(rewardNumCoin);
//        tipsText.text = string.Format(GetL10NString("Ui_Text_26"), AdvertBUFFTimes - count, count, AdvertBUFFTimes);
//        slider.value = count / (float)AdvertBUFFTimes;
//    }

//    public override void Tick(int deltaTimeMS)
//    {
//        if (!this.isActive())
//        {
//            return;
//        }
//        SetCountDownShow(deltaTimeMS);

//    }
//    /// <summary>
//    /// 设置倒计时功能
//    /// </summary>
//    protected void SetCountDownShow(int deltaTimeMS)
//    {
//        var buffList = GlobalDataManager.GetInstance().playerData.playerZoo.buffList;
//        for (int i = 0; i < buffList.Count; i++)
//        {
//            if (buffList[i].buffID == 14)
//            {
//                double time = buffList[i].CD.cd;
//                timeText.text = Math_F.OnDounbleToFormatTime_Anhour((int)time); ;
//                return;
//            }
//            else
//            {
//                if (i == buffList.Count - 1)
//                {
//                    timeText.text = "00:00:00";
//                }
//            }

//        }
//        if (buffList.Count == 0)
//        {
//            timeText.text = "00:00:00";
//        }


//    }
//    /// <summary>
//    /// 隐藏
//    /// </summary>
//    public override void Hide()
//    {
//        base.Hide();
//        MessageManager.GetInstance().UnRegist((int)GameMessageDefine.AddBuffSucceed, this.OnAddBuffSucceed);

//    }

//    /// <summary>
//    /// 玩家打点事件
//    /// </summary>
//    public void TAEvent_VideoAds_Start(string isVideo_type, string isVideo_name, int isVideo_num)
//    {
//        Utils.DebugLogger.GetInstance().Log("广告打点测试: 广告类型=" + isVideo_type + "    广告名称=" + isVideo_name + "    广告次数=" + isVideo_num);
//        var gameEventDic = PlayerMonitorEnumUtility.GetInstance().GetAddDictionary_Video_Ads(isVideo_type, isVideo_name, isVideo_num);
//        string gameEventKey = TAEventsMonitorEnum.video_start;
//        ThirdPartTA.Track(gameEventKey, gameEventDic);
//    }

//    public void TAEvent_VideoAds_Finish(string isVideo_type, string isVideo_name, int isVideo_num)
//    {
//        Utils.DebugLogger.GetInstance().Log("广告打点测试: 广告类型=" + isVideo_type + "    广告名称=" + isVideo_name + "    广告次数=" + isVideo_num);
//        var gameEventDic = PlayerMonitorEnumUtility.GetInstance().GetAddDictionary_Video_Ads(isVideo_type, isVideo_name, isVideo_num);
//        string gameEventKey = TAEventsMonitorEnum.video_finish;
//        ThirdPartTA.Track(gameEventKey, gameEventDic);
//    }
//}
