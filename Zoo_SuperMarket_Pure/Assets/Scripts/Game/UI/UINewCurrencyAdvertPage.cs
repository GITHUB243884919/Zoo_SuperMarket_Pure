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
    Text TitleText;
    Text TipsText_1;
    Text GoldText;
    Slider Schedule_Slider;
    Text ButtonBg_GoldNum;
    Image RewardIcon;
    Text RewardNumText;
    Text ButtonBg_ButtonText;
    Button advertButton;
    /// <summary>
    /// 看视频类型
    /// </summary>
    AdTagFM adTagFM;
    /// <summary>
    /// 离线广告收益
    /// </summary>
    BigInteger rewardNumCoin;
    /// <summary>
    /// icon的路径
    /// </summary>
    string iconPath = null;
    /// <summary>
    /// 免费视频看广告的收益
    /// </summary>
    private BigInteger freeItemRwdCoinQuantity = 0;
    /// <summary>
    /// 视频广告翻倍倍数
    /// </summary>
    private int waitingADProfit { get { return Config.globalConfig.getInstace().WaitingADProfit; } }
    /// <summary>
    /// 离线奖励金钱
    /// </summary>
    BigInteger offlineRewardCoin = 0;

    /// <summary>
    /// 视频广告的数据
    /// </summary>
    private  PlayerNumberOfVideosWatched playerNumberOfVideosWatched { get { return GlobalDataManager.GetInstance().playerData.playerZoo.playerNumberOfVideosWatched; } }


    private PlayerData playerData { get { return GlobalDataManager.GetInstance().playerData; } }
    Config.monitorCell monitorCell;
    int numberVisitor;

    public UINewCurrencyAdvertPage() : base(UIType.Fixed, UIMode.DoNothing, UICollider.None, UITickedMode.Update)
    {
        uiPath = "uiprefab/UINewCurrencyAdvert";
    }
    public override void Awake(GameObject go)
    {
        base.Awake(go);
        //初始化控件
        GetTransPrefabAllTextShow(this.transform);
        RegistAllCompent();

        

    }
    private void RegistAllCompent()
    {
        TitleText = RegistCompent<Text>("UIAdvertUpper/UiBg/TextGroup/TitleText") ;
        TipsText_1 = RegistCompent<Text>("UIAdvertUpper/UiBg/TextGroup/TipsText_1");
        GoldText = RegistCompent<Text>("UIAdvertUpper/UiBg/TextGroup/GoldText");
        Schedule_Slider = RegistCompent<Slider>("UIAdvertUpper/UiBg/Schedule/Slider");
        ButtonBg_GoldNum = RegistCompent<Text>("UIAdvertUpper/ButtonBg/GoldNum");
        RewardIcon = RegistCompent<Image>("UIAdvertLower/UiBg/TextGroup/RewardIcon");
        RewardNumText = RegistCompent<Text>("UIAdvertLower/UiBg/TextGroup/RewardNumText");
        ButtonBg_ButtonText = RegistCompent<Text>("UIAdvertLower/UiBg/TextGroup/AdvertButton/ButtonBg/ButtonText");
        RegistBtnAndClick("UIAdvertUpper/UiBg/CloseButton", OnClickCloseButton);
        advertButton = RegistBtnAndClick("UIAdvertLower/UiBg/TextGroup/AdvertButton/ButtonBg", OnClickAdvertPalyButton);
    }
    /// <summary>
    /// 关闭页面
    /// </summary>
    /// <param name="obj"></param>
    private void OnClickCloseButton(string obj)
    {
        float timeCount = 0.1f;
        DOTween.To(() => timeCount, a => timeCount = a, 0.1f, 0.1f).OnComplete(new TweenCallback(delegate
        {
            PageMgr.ClosePage<UINewCurrencyAdvertPage>();
        }));
        MessageString.Send((int)GameMessageDefine.UIMessage_ActiveButShowPart, "UIMainPage");
        if (adTagFM ==AdTagFM.Add_Offline_Advert)
        {
            ActualEarnings(offlineRewardCoin);
            HideUICallBack();
        }
    }
    private void HideUICallBack()
    {
        float timeCount = 0.1f;
        DOTween.To(() => timeCount, a => timeCount = a, 0.1f, 0.1f).OnComplete(new TweenCallback(delegate
        {
            UIMainPage uIMainPage = PageMgr.GetPage<UIMainPage>();
            uIMainPage.OnMoneyEffect();//飘钱特效
        }));
    }
    /// <summary>
    /// 播放视频
    /// </summary>
    /// <param name="obj"></param>
    private void OnClickAdvertPalyButton(string obj)
    {
        LogWarp.LogError("播放视频   UINewCurrencyAdvertPage ");
        //ThirdPartMgr.ShowRewardedAd(OnGetIsLockAdsSucceedBool);

    }

    /// <summary>
    /// 更新
    /// </summary>
    public override void Refresh()
    {
        base.Refresh();

    }
    /// <summary>
    /// 活跃
    /// </summary>
    public override void Active()
    {
        base.Active();
        //取消其他的UI
        var allPage = PageMgr.allPages;
        foreach (var item in allPage)
        {
            if (item.Value.name != this.name&& item.Value.name != "UIMainPage")
            {
                PageMgr.ClosePage(item.Value);
            }
        }    
        MessageString.Send((int)GameMessageDefine.UIMessage_ActiveButHidePart, "UIMainPage");
        numberVisitor = PlayerDataModule.SteameVisitorNameber();
        adTagFM = InitCompentToAdTagFM();
    }

    /// <summary>
    /// 将m_data转为AdTagFM类型
    /// </summary>
    /// <returns></returns>
    private AdTagFM InitCompentToAdTagFM()
    {
        int playerWatchAdsVideoCount = playerNumberOfVideosWatched.playerWatchAdsVideoCount;
        int advertRewardNumber = Config.globalConfig.getInstace().AdvertRewardNumber;
        int advertRewardRmbNumber = Config.globalConfig.getInstace().AdvertRewardRmbNumber;

        GoldText.text = string.Format( GetL10NString("Ui_Text_127"), playerWatchAdsVideoCount, advertRewardNumber);
        Schedule_Slider.value = AddPercentage(playerWatchAdsVideoCount,advertRewardNumber);
        ButtonBg_GoldNum.text = string.Format( GetL10NString("Ui_Text_128"), advertRewardRmbNumber);
        ButtonBg_ButtonText.text = GetL10NString("Ui_Text_27");
        SwitchButtonUnClickable(advertButton, true);

        AdTagFM adTagFM = AdTagFM.Add_Double_Advert;
        if (m_data != null)
        {
            var isAdTagFM = m_data.ToString();
            switch (isAdTagFM)
            {
                case "Add_Double_Advert":
                    adTagFM = AdTagFM.Add_Double_Advert;
                    LogWarp.LogErrorFormat(" 视频UI  {0}    翻倍广告视频 ", isAdTagFM);
                    InitCompent_Double();

                    break;
                case "Add_Tourist_Advert":
                    adTagFM = AdTagFM.Add_Tourist_Advert;
                    LogWarp.LogErrorFormat(" 视频UI  {0}    增加游客广告视频 ", isAdTagFM);
                    InitCompent_Tourist();

                    break;
                case "Add_Ticket_Advert":
                    adTagFM = AdTagFM.Add_Ticket_Advert;
                    LogWarp.LogErrorFormat(" 视频UI  {0}    快速售票广告视频 ", isAdTagFM);
                    InitCompent_Ticket();

                    break;
                case "Add_Visit_Advert":
                    adTagFM = AdTagFM.Add_Visit_Advert;
                    LogWarp.LogErrorFormat(" 视频UI  {0}    快速观光广告视频 ", isAdTagFM);
                    InitCompent_Visit();

                    break;
                case "Add_Offline_Advert":
                    adTagFM = AdTagFM.Add_Offline_Advert;
                    LogWarp.LogErrorFormat(" 视频UI  {0}    离线广告视频 ", isAdTagFM);
                    InitCompent_Offline();

                    break;
                case "Add_Viptiming_Advert":
                    adTagFM = AdTagFM.Add_Viptiming_Advert;
                    LogWarp.LogErrorFormat(" 视频UI  {0}    免费广告视频 ", isAdTagFM);
                    InitCompent_Viptiming();

                    break;
                default:
                    
                    break;
            }
        }
        
        return adTagFM;
    }

  
    /// <summary>
    /// 免费气球
    /// </summary>
    private void InitCompent_Viptiming()
    {
        //贵宾广告只算最新解锁的场景 / M收益
        BigInteger perMinCoin = 0;
        if (playerData.playerZoo.currSceneID == playerData.playerZoo.lastUnLockSceneID)
        {
            perMinCoin = PlayerDataModule.CurrScenePerMinCoin(true);
        }
        else
        {
            perMinCoin = PlayerDataModule.LeaveScenePerMinCoin(playerData.playerZoo.lastUnLockSceneID, true);
        }
        freeItemRwdCoinQuantity = perMinCoin * GetSceneAdcoefficient();

        TitleText.text = GetL10NString("Ui_Text_111");
        TipsText_1.text = GetL10NString("Ui_Text_112");
        RewardNumText.text = MinerBigInt.ToDisplay(freeItemRwdCoinQuantity);
        int scenetype = Config.sceneConfig.getInstace().getCell(playerData.playerZoo.currSceneID).moneyid;
        iconPath = Config.moneyConfig.getInstace().getCell(scenetype).bigmoneyicon;
        RewardIcon.sprite = ResourceManager.LoadSpriteFromPrefab(iconPath);
    }
    /// <summary>
    /// 离线奖励
    /// </summary>
    private void InitCompent_Offline()
    {
        CalcOfflineRewardCoinMMSC();

        TitleText.text = GetL10NString("Ui_Text_19");
        TipsText_1.text = GetL10NString("Ui_Text_20");
        //RewardIcon
        RewardNumText.text = MinerBigInt.ToDisplay(offlineRewardCoin);
        int scenetype = Config.sceneConfig.getInstace().getCell(playerData.playerZoo.currSceneID).moneyid;
        iconPath = Config.moneyConfig.getInstace().getCell(scenetype).bigmoneyicon;
        RewardIcon.sprite = ResourceManager.LoadSpriteFromPrefab(iconPath);
        ButtonBg_ButtonText.text = string.Format(GetL10NString("Ui_Text_21"), waitingADProfit);
    }
    /// <summary>
    /// 快速浏览
    /// </summary>
    private void InitCompent_Visit()
    {
        TitleText.text = GetL10NString("Ui_Text_31");
        TipsText_1.text = GetL10NString("Ui_Text_32");
        var cell = Config.buffConfig.getInstace().getCell(10);
        RewardNumText.text = string.Format(GetL10NString("Ui_Text_33"), cell.time);
        iconPath = Config.globalConfig.getInstace().AdvertAddVisit;
        RewardIcon.sprite = ResourceManager.LoadSpriteFromPrefab(iconPath);
    }
    /// <summary>
    /// 快速售票
    /// </summary>
    private void InitCompent_Ticket()
    {
        TitleText.text = GetL10NString("Ui_Text_34");
        TipsText_1.text = GetL10NString("Ui_Text_35");
        var cell1 = Config.buffConfig.getInstace().getCell(12);
        RewardNumText.text = string.Format(GetL10NString("Ui_Text_36"), cell1.time);
        iconPath = Config.globalConfig.getInstace().AdvertAddTicket;
        RewardIcon.sprite = ResourceManager.LoadSpriteFromPrefab(iconPath);
    }
    /// <summary>
    /// 增加游客
    /// </summary>
    private void InitCompent_Tourist()
    {
        TitleText.text = GetL10NString("Ui_Text_28");
        TipsText_1.text = GetL10NString("Ui_Text_29");
        //RewardIcon
        RewardNumText.text = string.Format(GetL10NString("Ui_Text_30"), numberVisitor);
        iconPath = Config.globalConfig.getInstace().AdvertAddTourist;
        RewardIcon.sprite = ResourceManager.LoadSpriteFromPrefab(iconPath);
    }
    /// <summary>
    /// 收益翻倍
    /// </summary>
    private void InitCompent_Double()
    {
        rewardNumCoin = PlayerDataModule.AllScenePerMinCoin(true) * Config.globalConfig.getInstace().AdvertGoldReward;
        var buffCell = Config.buffConfig.getInstace().getCell(14);
        int scenetype = Config.sceneConfig.getInstace().getCell(GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID).moneyid;
        string bigMoneyiconPath = Config.globalConfig.getInstace().AdvertProfitDouble;
        int AdvertBUFFTimes = Config.globalConfig.getInstace().GoldRewardMaxNumber;
        int count = GlobalDataManager.GetInstance().playerData.playerZoo.playerNumberOfVideosWatched.playerLockGainDoubleAdsVideoCount;


        TitleText.text = GetL10NString("Ui_Text_22");
        TipsText_1.text = string.Format(GetL10NString("Ui_Text_23"), buffCell.multiple, UFrame.Math_F.IntToFloat_SToHr(buffCell.time), AdvertBUFFTimes);
        RewardIcon.sprite = ResourceManager.LoadSpriteFromPrefab(bigMoneyiconPath);
        RewardNumText.text = GetL10NString("Ui_Text_129");


        if ((AdvertBUFFTimes - count) > 0)
        {
        }
        else
        {
            SwitchButtonUnClickable(advertButton, false);
        }


    }

    public override void Tick(int deltaTimeMS)
    {
        if (!this.isActive())
        {
            return;
        }

    }
    /// <summary>
    /// 隐藏
    /// </summary>
    public override void Hide()
    {
        base.Hide();

    }

    private void OnGetIsLockAdsSucceedBool(bool isBool)
    {
        if (isBool)
        {
            LogWarp.LogErrorFormat("测试：   看视频成功");
            switch (adTagFM)
            {
                case AdTagFM.Add_Double_Advert:
                    LockAdsSucceed_Double();
                    break;
                case AdTagFM.Add_Tourist_Advert:
                    LockAdsSucceed_Tourist();
                    break;
                case AdTagFM.Add_Ticket_Advert:
                    LockAdsSucceed_Ticket();
                    break;
                case AdTagFM.Add_Visit_Advert:
                    LockAdsSucceed_Visit();
                    break;
                case AdTagFM.Add_Offline_Advert:
                    LockAdsSucceed_Offline();
                    break;
                case AdTagFM.Add_Viptiming_Advert:
                    LockAdsSucceed_FreeItem();
                    break;
                default:
                    break;
            }
            //利用dotweeen做延时操作 防止穿透
            float timeCount = 0.1f;
            DOTween.To(() => timeCount, a => timeCount = a, 0.1f, 0.1f).OnComplete(new TweenCallback(delegate
            {
                OnClickCloseButton("");
            }));
        }
        else
        {
            LogWarp.LogErrorFormat("测试：   看视频失败");
        }
    }


}
