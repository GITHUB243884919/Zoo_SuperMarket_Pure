using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Config;
using Game;
using Game.MessageCenter;
using UFrame;
using Game.GlobalData;
using System;
using UFrame.MessageCenter;
using System.Numerics;
using UFrame.Logger;
using UFrame.MiniGame;
using UFrame.BehaviourFloat;
using Game.Path.StraightLine;
using DG.Tweening;
using UFrame.EntityFloat;
using UnityEngine.EventSystems;

public partial class UIMainPage : UIPage
{
    /// <summary>
    /// 观看广告   加倍收益   UIAdvertActivity
    /// </summary>
    /// <param name="obj"></param>
    private void OnClickAdvertButton(string obj)
    {
        float timeCount = 0.1f;
        DOTween.To(() => timeCount, a => timeCount = a, 0.1f, 0.1f).OnComplete(new TweenCallback(delegate
        {
            //PageMgr.ShowPage<UIAdvertActivityPage>(obj);  //加倍收益
            PageMgr.ShowPage<UINewCurrencyAdvertPage>(AdTagFM.Add_Double_Advert);

        }));
    }
    /// <summary>
    /// 图鉴入口
    /// </summary>
    /// <param name="obj"></param>
    private void OnClickCollectButton(string obj)
    {
        float timeCount = 0.1f;
        DOTween.To(() => timeCount, a => timeCount = a, 0.1f, 0.1f).OnComplete(new TweenCallback(delegate
        {
            PageMgr.ShowPage<UIAnimalAtlasPage>();  //图鉴入口
        }));
    }
    /// <summary>
    /// 增加轮船游客
    /// </summary>
    /// <param name="obj"></param>
    private void OnClickAdsButton_TouristButton(string obj)
    {
        float timeCount=0.1f;

        DOTween.To(() => timeCount, a => timeCount = a, 0.1f, 0.1f).OnComplete(new TweenCallback(delegate
        {
            //PageMgr.ShowPage<UIAdvertTouristPage>(obj);  //增加轮船游客

            PageMgr.ShowPage<UINewCurrencyAdvertPage>(AdTagFM.Add_Tourist_Advert);
            touristButton.gameObject.SetActive(false);
            int touristButton_Time = Math_F.FloatToInt1000(Config.globalConfig.getInstace().IncreaseTouristCD);
            if (touristIntCD ==null)
            {
                multiTickObj.AddCD(touristButton_Time, AdsButtonShow_Tourist);
            }
            else
            {
                touristIntCD.Reset();
                touristIntCD.Run();
            }
        }));
    }
    /// <summary>
    /// 秒售票口CD
    /// </summary>
    /// <param name="obj"></param>
    private void OnClickAdsButton_TicketButton(string obj)
    {
        float timeCount = 0.1f;
        DOTween.To(() => timeCount, a => timeCount = a, 0.1f, 0.1f).OnComplete(new TweenCallback(delegate
        {
            //PageMgr.ShowPage<UIAdvertTouristPage>(obj);
            PageMgr.ShowPage<UINewCurrencyAdvertPage>(AdTagFM.Add_Ticket_Advert);

            ticketButton.gameObject.SetActive(false);
            int ticketButton_Time = Math_F.FloatToInt1000(Config.globalConfig.getInstace().TicketAccelerateCD);
            if (ticketIntCD == null)
            {
                multiTickObj.AddCD(ticketButton_Time, AdsButtonShow_Ticket);
            }
            else
            {
                ticketIntCD.Reset();
                ticketIntCD.Run();
            }
        }));
    }
    /// <summary>
    /// 秒观光CD
    /// </summary>
    /// <param name="obj"></param>
    private void OnClickAdsButton_VisitButton(string obj)
    {
        float timeCount = 0.1f;
        DOTween.To(() => timeCount, a => timeCount = a, 0.1f, 0.1f).OnComplete(new TweenCallback(delegate
        {
            //PageMgr.ShowPage<UIAdvertTouristPage>(obj);
            PageMgr.ShowPage<UINewCurrencyAdvertPage>(AdTagFM.Add_Visit_Advert);

            visitButton.gameObject.SetActive(false);
            int visitButton_Time = Math_F.FloatToInt1000(Config.globalConfig.getInstace().VisitAccelerateCD);
           
            if (visitIntCD == null)
            {
                multiTickObj.AddCD(visitButton_Time, AdsButtonShow_Visit);
            }
            else
            {
                visitIntCD.Reset();
                visitIntCD.Run();
            }
        }));
    }
    /// <summary>
    /// 动物培养开启功能
    /// </summary>
    /// <param name="obj"></param>
    private void OnClickAdsButton_StrengthenButton(string obj)
    {
        float timeCount = 0.1f;
        DOTween.To(() => timeCount, a => timeCount = a, 0.1f, 0.1f).OnComplete(new TweenCallback(delegate
        {
            PageMgr.ShowPage<UIAnimalTipsPage>();
        }));

    }
    protected void OnClickLittleGameButton()
    {
        ZooGameLoader.GetInstance().UnLoad();
        SceneMgr.Inst.LoadSceneAsync("Load", () => { });
    }
    /// <summary>
    /// 隐藏星星开启动物培养的按钮
    /// </summary>
    public void SetHideAnimalCultureOpenHintButton()
    {
        animalCultureOpenHintButton.gameObject.SetActive(false);
    }

    /// <summary>
    /// 监听buff成功
    /// </summary>
    /// <param name="obj"></param>
    private void OnAddBuffSucceed(Message obj)
    {
        float timeCount = 0.1f;
        DOTween.To(() => timeCount, a => timeCount = a, 0.1f, 0.1f).OnComplete(new TweenCallback( delegate {
            incomeCoinMS = PlayerDataModule.CurrScenePerMinCoin(true);
            earningsText.text = MinerBigInt.ToDisplay(incomeCoinMS) + GetL10NString("Ui_Text_67");
        }));
    }
    /// <summary>
    /// 监听玩家星星数量
    /// </summary>
    /// <param name="obj"></param>
    private void OnBroadcastStarOfPlayerData(Message obj)
    {
        ShowAnimalCultivate();
        starText.text = playerData.playerZoo.star.ToString();
        UpdateDisplayForSceneStates(false);
    }
    /// <summary>
    /// 开启动物培养功能
    /// </summary>
    private void ShowAnimalCultivate()
    {
        if (GlobalDataManager.GetInstance().playerData.playerZoo.star >= Config.globalConfig.getInstace().AnimalupgradingNeed)
        {
            GlobalDataManager.GetInstance().playerData.playerZoo.isShowAnimalCultivate = true;
            SetHideAnimalCultureOpenHintButton();
        }
    }

    /// <summary>
    /// 监听是否开启离线UI
    /// </summary>
    /// <param name="obj"></param>
    private void OnOpenOfflineUIPage(Message msg)
    {
        //PageMgr.ShowPage<UIOfflineRewardPage>();  //离线UI
        PageMgr.ShowPage<UINewCurrencyAdvertPage>(AdTagFM.Add_Offline_Advert);

    }
    ///// <summary>
    ///// 监听玩家金钱变化
    ///// </summary>
    ///// <param name="obj"></param>
    //protected void OnBroadcastCoinOfPlayerData(Message obj)
    //{
    //    goldText.text = playerData.playerZoo.playerCoin.GetCoinByScene(playerData.playerZoo.currSceneID).coinShow;
    //    incomeCoinMS = PlayerDataModule.CurrScenePerMinCoin(true);
    //    earningsText.text = MinerBigInt.ToDisplay(incomeCoinMS) + GetL10NString("Ui_Text_67");

    //    //isBool = true;
    //}
    /// <summary>
    /// 监听钻石数量变化的消息
    /// </summary>
    /// <param name="obj"></param>
    private void OnBroadcastDiamondOfPlayerData(Message obj)
    {
        diamondText.text = playerData.playerZoo.diamond.ToString();

    }
    protected void OnGetBroadcastCoinOfPlayerDataMSSC(Message msg)
    {
        var multiCoin = playerData.playerZoo.playerCoin.GetCoinByScene(playerData.playerZoo.currSceneID);
        if (goldText.text != multiCoin.coinShow)
        {
            goldText.text = multiCoin.coinShow;
        }
    }

    /// <summary>
    /// 隐藏部分UI
    /// </summary>
    /// <param name="msg"></param>
    protected void OnUIMessage_ActiveButHidePart(Message msg)
    {
        var _msg = msg as MessageString;
        if (_msg.str == this.name)
        {
            setMainShow.gameObject.SetActive(false);
            SetGuideTaskPanelActive(false);
        }
    }
    /// <summary>
    /// 回复隐藏的部分UI
    /// </summary>
    /// <param name="msg"></param>
    protected void OnUIMessage_ActiveButShowPart(Message msg)
    {
        var _msg = msg as MessageString;
        if (_msg.str == this.name)
        {
            setMainShow.gameObject.SetActive(true);
            SetGuideTaskPanelActive(true);
        }

    }

    /// <summary>
    /// 需要修改，半分钟刷新每分钟收益
    /// </summary>
    public override void Tick(int deltaTimeMS)
    {
        if (GlobalDataManager.GetInstance().isLoadingScene)
        {
            return;
        }
        multiTickObj.Tick(deltaTimeMS);
        /*设置安卓返回键功能*/
        SetAndroidQuit();
        /*做秒CDbuff的持续时间的倒计时*/
        SetCountDownShow();
        /*增益buff倒计时*/
        SetCountDownShow_Double(deltaTimeMS);

        SetCountDownShow_CrossRoad(deltaTimeMS);
    }

    private void SetCountDownShow_CrossRoad(int deltaTimeMS)
    {
        var countDownTicks = playerData.playerLittleGame.intCD;
        //LogWarp.LogErrorFormat("小游戏倒计时    {0}     ", (int)(countDownTicks.cd / 1000));
        if (countDownTicks!=null)
        {
            strength_Time.text = Math_F.OnDounbleToFormatTime_Anhour((int)(countDownTicks.cd / 1000));

        }

    }

    /// <summary>
    /// 设置增益buff的倒计时功能
    /// </summary>
    protected void SetCountDownShow_Double(int deltaTimeMS)
    {
        var buffList = GlobalDataManager.GetInstance().playerData.playerZoo.buffList;
        string time_str = "00:00:00";
        for (int i = 0; i < buffList.Count; i++)
        {
            if (buffList[i].buffID == 14)
            {
                double time = buffList[i].CD.cd;
                advertButton_Time.text = Math_F.OnDounbleToFormatTime_Anhour((int)time); ;
                return;
            }
            else
            {
                if (i == buffList.Count - 1)
                {
                    advertButton_Time.text = time_str;
                }
            }

        }
        if (buffList.Count == 0)
        {
            advertButton_Time.text = time_str;
        }


    }

    /// <summary>
    /// 刷新主界面的数据
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    private void RefreshUIShowData(int arg1, IntCD arg2)
    {
        incomeCoinMS = PlayerDataModule.CurrScenePerMinCoin(true);
        earningsText.text = MinerBigInt.ToDisplay(incomeCoinMS) + GetL10NString("Ui_Text_67");
        arg2.Reset();
        arg2.Run();
    }
    IntCD touristIntCD;
    IntCD visitIntCD;
    IntCD ticketIntCD;

    /// <summary>
    /// 广告按钮显示——游轮
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    private void AdsButtonShow_Tourist(int arg1, IntCD arg2)
    {
        touristButton.gameObject.SetActive(true);
        touristIntCD = arg2;
    }
    /// <summary>
    /// 广告按钮显示——动物栏观光速度
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    private void AdsButtonShow_Visit(int arg1, IntCD arg2)
    {
        visitButton.gameObject.SetActive(true);
        visitIntCD = arg2;
    }
    /// <summary>
    /// 广告按钮显示——加快大门乘车速度
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    private void AdsButtonShow_Ticket(int arg1, IntCD arg2)
    {
        ticketButton.gameObject.SetActive(true);
        ticketIntCD = arg2;
    }

    /// <summary>
    /// 广告按钮显示——游轮
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    private void Begin_AdsButtonShow_Tourist(int arg1, IntCD arg2)
    {
        //LogWarp.LogErrorFormat("测试： Begin_AdsButtonShow_Tourist    {0}   ", arg1);
        touristButton.gameObject.SetActive(true);
    }
    /// <summary>
    /// 广告按钮显示——动物栏观光速度
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    private void Begin_AdsButtonShow_Visit(int arg1, IntCD arg2)
    {
        visitButton.gameObject.SetActive(true);
    }
    /// <summary>
    /// 广告按钮显示——加快大门乘车速度
    /// </summary>
    /// <param name="arg1"></param>
    /// <param name="arg2"></param>
    private void Begin_AdsButtonShow_Ticket(int arg1, IntCD arg2)
    {
        ticketButton.gameObject.SetActive(true);
    }


    /// <summary>
    /// 设置安卓返回键功能
    /// </summary>
    [System.Diagnostics.Conditional("UNITY_ANDROID")]
    protected void SetAndroidQuit()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape))
#else
        if (Application.platform == RuntimePlatform.Android && Input.GetKeyDown(KeyCode.Escape)) 
#endif
        {
            if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide)
            {
                return;
            }

            var activePages = PageMgr.GetActivePages();
            if (activePages.Count == 1 && activePages[0].name == "UIMainPage")
            {
                //只有主界面存在,并且点了返回键
                PageMgr.ShowPage<UIQuitGamePage>();
                return;
            }

            foreach (var item in PageMgr.allPages)
            {
                if (item.Key != "UIMainPage")
                {
                    item.Value.Hide();
                }
            }
        }
    }
   

    /// <summary>
    /// 设置倒计时功能
    /// </summary>
    protected void SetCountDownShow()
    {
        var buffList = GlobalDataManager.GetInstance().playerData.playerZoo.buffList;
        bool isVisitBool = false;
        bool isTicketBool = false;
        for (int i = 0; i < buffList.Count; i++)
        {
            if (buffList[i].buffID == 10)
            {
                double time = buffList[i].CD.cd;
                isVisitBool = true;
                TimeFormatCalculate_Visit(time);
            }
            else if (buffList[i].buffID == 12)
            {
                double time = buffList[i].CD.cd;
                isTicketBool = true;
                TimeFormatCalculate_Ticket(time);
            }
        }
        if (!isVisitBool)
        {
            celerityVisit.gameObject.SetActive(false);
        }
        else
        {
            celerityVisit.gameObject.SetActive(true);
        }
        if (!isTicketBool)
        {
            celerityTicket.gameObject.SetActive(false);
        }
        else
        {
            celerityTicket.gameObject.SetActive(true);
        }

    }
   
    /// <summary>
    /// 秒观光CD倒计时
    /// </summary>
    /// <param name="isTime"></param>
    protected void TimeFormatCalculate_Visit(double isTime)
    {
        celerityVisit.Find("Text").GetComponent<Text>().text = Math_F.OnDounbleToFormatTime_Minute((int)isTime);
        if (isTime <= 1)
        {
            celerityVisit.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 秒售票CD倒计时
    /// </summary>
    /// <param name="isTime"></param>
    protected void TimeFormatCalculate_Ticket(double isTime)
    {
        celerityTicket.Find("Text").GetComponent<Text>().text = Math_F.OnDounbleToFormatTime_Minute((int)isTime);
        if (isTime <= 1)
        {
            celerityTicket.gameObject.SetActive(false);
        }
    }

}
