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
using DG.Tweening;

public partial class UIParkPage : UIPage
{
    int currSceneID;//当前场景
    PlayerData playerData;
    /// <summary>
    /// 当前玩家的金钱
    /// </summary>
    BigInteger coinVal;
    Config.parkingCell parkingCell;
    /// <summary>
    /// 停车场利润等级
    /// </summary>
    int profitLevel;
    /// <summary>
    /// 停车位等级
    /// </summary>
    int parkingSpaceLevel;
    /// <summary>
    /// 流量等级
    /// </summary>
    int enterCarSpawnLevel;
    /// <summary>
    /// 停车场数据
    /// </summary>
    ParkingCenterData_MS parkingCenterData;

    /// <summary>
    /// 当前等级段的最大值
    /// </summary>
    int maxGrade;
    int oldMaxGrade;

    /// <summary>
    /// 策划表配置的最大等级
    /// </summary>
    int parkingProfitMaxGrade;
    int parkingSpaceMaxGrade;
    int parkingEnterCarSpawnMaxGrade;

    /// <summary>
    /// 利润要升级需要消费的钱币
    /// </summary>
    BigInteger consumeProfitCoins;
    /// <summary>
    /// 停车位要升级需要消费的钱币
    /// </summary>
    BigInteger consumeParkingSpaceCoins;
    /// <summary>
    /// 流量要升级需要消费的钱币
    /// </summary>
    BigInteger consumeEnterCarSpawnCoins;


    /// <summary>
    /// 是否扣钱成功是否收到回复
    /// </summary>
    bool isGetCoin=true;

    /// <summary>
    /// 假计时   区分单点和长按
    /// </summary>
    int fakeTime = 0;

    /// <summary>
    /// 判断是否是长按状态
    /// </summary>
    bool isLongPress=false;

    //#region 全局UI控件属性
    Text titleText;    //名称：停车场
    Text tipsText;          //释义语言
    Text scoreNumTest;     //UI的星星收集显示
    int starLevelReached;

    Transform effectNode;  //新手引导节点
    Image gradeSlider_IconBg;
    Text lvText;            //等级text

    Text profitCoinsText;      //收益text
    Text profitCoins_Text_2;  
    Text profitCoins_Text_3;  
    Text profitCoins_LvText;
    Button profitCoins_Button;   //升级按钮
    Text profitCoins_Button_NeedGoldNum;       //升级模式需要的金钱
    Text profitCoins_Button_ButtonLvUpText;    //升级模式要升的级数
    Transform profitCoins_EffectNode;

    Text parkingSpaceText;        //数量Text
    Text parkingSpace_Text2; 
    Text parkingSpace_Text3; 
    Text parkingSpace_LvText;
    Button parkingSpace_Button;   //升级按钮
    Text parkingSpace_Button_NeedGoldNum;       //升级模式需要的金钱
    Text parkingSpace_Button_ButtonLvUpText;    //升级模式要升的级数
    Transform parkingSpace_EffectNode;

    Text enterCarSpawnText;       //冷却时间
    Text enterCarSpawn_Text2; 
    Text enterCarSpawn_Text3; 
    Text enterCarSpawn_LvText;
    Button enterCarSpawn_Button;   //升级按钮
    Text enterCarSpawn_Button_NeedGoldNum;       //升级模式需要的金钱
    Text enterCarSpawn_Button_ButtonLvUpText;    //升级模式要升的级数
    Transform enterCarSpawn_EffectNode;

    Slider gradeSlider;
    Image gradeSlider_Image;
    Text gradeSlider_Text;

    Button hideUIButton;

    Text gradeText_2;  //价格标签3
                       // #endregion

    Image profitCoins_Icon;
    Image profitCoins_GoldIcon;


    public UIParkPage() : base(UIType.Fixed, UIMode.DoNothing, UICollider.None)
    {
        uiPath = "uiprefab/UINewParking";
    }
    public override void Awake(GameObject go)
    {
        base.Awake(go);
        GetTransPrefabAllTextShow(this.transform);
        playerData = GlobalDataManager.GetInstance().playerData;

        //初始化控件
        this.RegistAllCompent(); 
    }
    
    /// <summary>
    /// 内部组件的查找
    /// </summary>
    private void RegistAllCompent()
    {
        titleText = RegistCompent<Text>("UIParking_LvUp/UiBg/TitleGroup/TitleText");
        //当前等级
        lvText = RegistCompent<Text>("UIParking_LvUp/UiBg/TitleGroup/LvText");
        tipsText = RegistCompent<Text>("UIParking_LvUp/UiBg/TitleGroup/TipsText");
        //等级进度条控件
        gradeSlider = RegistCompent<Slider>("UIParking_LvUp/UiBg/LvUpSchedule/Schedule/Slider2");
        gradeText_2 = RegistCompent<Text>("UIParking_LvUp/UiBg/LvUpSchedule/Schedule/Text_2");
        gradeSlider_IconBg = RegistCompent<Image>("UIParking_LvUp/UiBg/LvUpSchedule/IconBg");

        gradeSlider_Image = RegistCompent<Image>("UIParking_LvUp/UiBg/LvUpSchedule/IconBg/Icon");
        gradeSlider_Text = RegistCompent<Text>("UIParking_LvUp/UiBg/LvUpSchedule/IconBg/Num");
        scoreNumTest = RegistCompent<Text>("UIParking_LvUp/UiBg/ScoreGroup/ScoreNum");

        /* 利润text */
        profitCoinsText = RegistCompent<Text>("UIParking_LvUp/UiBg/ParameterGroup/Parameter/Parameter_1/Text_1");
        //当前停车场价格
        profitCoins_Text_2 = RegistCompent<Text>("UIParking_LvUp/UiBg/ParameterGroup/Parameter/Parameter_1/TextAll/Text_2");
        //升级后的价格
        profitCoins_Text_3 = RegistCompent<Text>("UIParking_LvUp/UiBg/ParameterGroup/Parameter/Parameter_1/TextAll/Text_3");
        profitCoins_LvText = RegistCompent<Text>("UIParking_LvUp/UiBg/ParameterGroup/Parameter/Parameter_1/Level/LvText");
        profitCoins_EffectNode = RegistCompent<Transform>("UIParking_LvUp/UiBg/ParameterGroup/Parameter/Parameter_1/effectNode");
        //升级按钮
        profitCoins_Button = RegistBtnAndClick("UIParking_LvUp/UiBg/ParameterGroup/Parameter/Parameter_1/LvUpButton", OnClickUpGrade_ProfitCoins);
        profitCoins_Button.gameObject.AddComponent<RepeatButton>();
        profitCoins_Button.GetComponent<RepeatButton>().onPress.AddListener(OnLongPressButton_ProfitCoins);//按下。频繁的调用
        profitCoins_Button.GetComponent<RepeatButton>().onRelease.AddListener(OnReleaseButton);//抬起，调用一次

        //升级需要的金币
        profitCoins_Button_NeedGoldNum = RegistCompent<Text>("UIParking_LvUp/UiBg/ParameterGroup/Parameter/Parameter_1/LvUpButton/NeedGoldNum");
        //可以升级的级数
        profitCoins_Button_ButtonLvUpText = RegistCompent<Text>("UIParking_LvUp/UiBg/ParameterGroup/Parameter/Parameter_1/LvUpButton/ButtonLvUpText");

        hideUIButton = RegistBtnAndClick("UIParking_LvUp/UiBg/BgImage/BgImage", HideButtonUI);

        /* 数量Text */
        parkingSpaceText = RegistCompent<Text>("UIParking_LvUp/UiBg/ParameterGroup/Parameter/Parameter_2/Text_1");
        //当前的停车场数量
        parkingSpace_Text2 = RegistCompent<Text>("UIParking_LvUp/UiBg/ParameterGroup/Parameter/Parameter_2/TextAll/Text_2");
        //升级后的停车场数量
        parkingSpace_Text3 = RegistCompent<Text>("UIParking_LvUp/UiBg/ParameterGroup/Parameter/Parameter_2/TextAll/Text_3");
        parkingSpace_LvText = RegistCompent<Text>("UIParking_LvUp/UiBg/ParameterGroup/Parameter/Parameter_2/Level/LvText");
        parkingSpace_EffectNode = RegistCompent<Transform>("UIParking_LvUp/UiBg/ParameterGroup/Parameter/Parameter_2/effectNode");

        //升级按钮
        parkingSpace_Button = RegistBtnAndClick("UIParking_LvUp/UiBg/ParameterGroup/Parameter/Parameter_2/LvUpButton", OnClickUpGrade_ParkingSpace);
        parkingSpace_Button.gameObject.AddComponent<RepeatButton>();
        parkingSpace_Button.GetComponent<RepeatButton>().onPress.AddListener(OnLongPressButton_ParkingSpace);//按下。频繁的调用
        parkingSpace_Button.GetComponent<RepeatButton>().onRelease.AddListener(OnReleaseButton);//抬起，调用一次
        //升级需要的金币
        parkingSpace_Button_NeedGoldNum = RegistCompent<Text>("UIParking_LvUp/UiBg/ParameterGroup/Parameter/Parameter_2/LvUpButton/NeedGoldNum");
        //可以升级的级数
        parkingSpace_Button_ButtonLvUpText = RegistCompent<Text>("UIParking_LvUp/UiBg/ParameterGroup/Parameter/Parameter_2/LvUpButton/ButtonLvUpText");



        /* 冷却时间 */
        enterCarSpawnText = RegistCompent<Text>("UIParking_LvUp/UiBg/ParameterGroup/Parameter/Parameter_3/Text_1");
        //当前停车场的流量
        enterCarSpawn_Text2 = RegistCompent<Text>("UIParking_LvUp/UiBg/ParameterGroup/Parameter/Parameter_3/TextAll/Text_2");
        //升级后的停车场流量
        enterCarSpawn_Text3 = RegistCompent<Text>("UIParking_LvUp/UiBg/ParameterGroup/Parameter/Parameter_3/TextAll/Text_3");
        enterCarSpawn_LvText = RegistCompent<Text>("UIParking_LvUp/UiBg/ParameterGroup/Parameter/Parameter_3/Level/LvText");
        enterCarSpawn_EffectNode = RegistCompent<Transform>("UIParking_LvUp/UiBg/ParameterGroup/Parameter/Parameter_3/effectNode");
        //升级按钮
        enterCarSpawn_Button = RegistBtnAndClick("UIParking_LvUp/UiBg/ParameterGroup/Parameter/Parameter_3/LvUpButton", OnClickUpGrade_EnterCarSpawn);
        enterCarSpawn_Button.gameObject.AddComponent<RepeatButton>();
        enterCarSpawn_Button.GetComponent<RepeatButton>().onPress.AddListener(OnLongPressButton_EnterCarSpawn);//按下。频繁的调用
        enterCarSpawn_Button.GetComponent<RepeatButton>().onRelease.AddListener(OnReleaseButton);//抬起，调用一次
        //升级需要的金币
        enterCarSpawn_Button_NeedGoldNum = RegistCompent<Text>("UIParking_LvUp/UiBg/ParameterGroup/Parameter/Parameter_3/LvUpButton/NeedGoldNum");
        //可以升级的级数
        enterCarSpawn_Button_ButtonLvUpText = RegistCompent<Text>("UIParking_LvUp/UiBg/ParameterGroup/Parameter/Parameter_3/LvUpButton/ButtonLvUpText");

        profitCoins_Icon = RegistCompent<Image>("UIParking_LvUp/UiBg/ParameterGroup/Parameter/Parameter_1/Icon");
        profitCoins_GoldIcon = RegistCompent<Image>("UIParking_LvUp/UiBg/ParameterGroup/Parameter/Parameter_1/GoldIcon");
        profitCoins_LvUpButton_Icon = RegistCompent<Image>("UIParking_LvUp/UiBg/ParameterGroup/Parameter/Parameter_1/LvUpButton/GoldIcon");
        enterCarSpawn_LvUpButton_Icon = RegistCompent<Image>("UIParking_LvUp/UiBg/ParameterGroup/Parameter/Parameter_2/LvUpButton/GoldIcon");
        parkingSpace_LvUpButton_Icon = RegistCompent<Image>("UIParking_LvUp/UiBg/ParameterGroup/Parameter/Parameter_3/LvUpButton/GoldIcon");
        //显示正常的Image
        SetCorrectShowImage();
    }
    Image profitCoins_LvUpButton_Icon;
    Image enterCarSpawn_LvUpButton_Icon;
    Image parkingSpace_LvUpButton_Icon;
    Sprite sprite;
    private void SetCorrectShowImage()
    {
        int scenetype = Config.sceneConfig.getInstace().getCell(playerData.playerZoo.currSceneID).moneyid;
        string iconPath = Config.moneyConfig.getInstace().getCell(scenetype).moneyicon;
        sprite = ResourceManager.LoadSpriteFromPrefab(iconPath);
        profitCoins_Icon.sprite = sprite;
        profitCoins_GoldIcon.sprite = sprite;
        profitCoins_LvUpButton_Icon.sprite = sprite;
        enterCarSpawn_LvUpButton_Icon.sprite = sprite;
        parkingSpace_LvUpButton_Icon.sprite = sprite;
    }

    /// <summary>
    /// 初始化属性数值
    /// </summary>
    private void InitData( )
    {
        //LogWarp.LogError(" 测试：   InitData   ");
        currSceneID = playerData.playerZoo.currSceneID;
        parkingCell = GlobalDataManager.GetInstance().logicTableParkingData.GetParkingCell(currSceneID);
        parkingProfitMaxGrade = parkingCell.lvmax;
        parkingSpaceMaxGrade = parkingCell.spacemaxlv;
        parkingEnterCarSpawnMaxGrade = parkingCell.touristmaxlv;

        InitCoin();

        int idx = PlayerDataModule.FindLevelRangIndex(parkingCell.lvshage, profitLevel);
        maxGrade = parkingCell.lvshage[idx];
        oldMaxGrade = parkingCell.lvshage[idx - 1];
        starLevelReached = PlayerDataModule.FindLevelRangIndex01(parkingCell.lvshage, profitLevel);
        if (profitLevel >= parkingProfitMaxGrade)
        {
            starLevelReached = PlayerDataModule.FindLevelRangIndex01(parkingCell.lvshage, profitLevel);
        }

        InitCompent();
    }

    private void InitCoin()
    {
        //获取玩家停车场等级       获取玩家现有金币
        parkingCenterData = GlobalDataManager.GetInstance().playerData.GetParkingCenterDataIDIndexOfDataIdx();

        profitLevel = parkingCenterData.parkingProfitLevel;
        parkingSpaceLevel = parkingCenterData.parkingSpaceLevel;
        enterCarSpawnLevel = parkingCenterData.parkingEnterCarSpawnLevel;

        coinVal = playerData.playerZoo.playerCoin.GetCoinByScene(currSceneID).coinBigInt;
        consumeProfitCoins = ParkingCenter.GetUpGradeParkingProfitConsumption(profitLevel);
        consumeParkingSpaceCoins = ParkingCenter.GetUpGradeNumberConsumption(parkingSpaceLevel);
        consumeEnterCarSpawnCoins = ParkingCenter.GetUpGradeEnterCarSpawnConsumption(enterCarSpawnLevel);
    }

    /// <summary>
    /// 控件显示赋值
    /// </summary>
    private void InitCompent()
    {
        //LogWarp.LogError(" 测试：   InitCompent   ");

        if (maxGrade >= parkingProfitMaxGrade)
        {
            maxGrade = parkingProfitMaxGrade;
        }

        lvText.text = string.Format(GetL10NString("Ui_Text_2"), profitLevel.ToString());
        gradeSlider.value = AddPercentage(profitLevel - oldMaxGrade, maxGrade- oldMaxGrade);
        gradeText_2.text = profitLevel.ToString() + "/" + maxGrade.ToString();  //最大等级上限
        //获取UI image =  
        Config.itemCell itemCell = GradeSliderAwardImage();
        gradeSlider_Image.sprite = ResourceManager.LoadSpriteFromPrefab(itemCell.icon);

        gradeSlider_Text.text =MinerBigInt.ToDisplay(  itemCell.itemval);
        scoreNumTest.text = starLevelReached + "/" + parkingCell.starsum;
        profitCoins_Text_2.text = ParkingCenter.GetParkingProfit(profitLevel).ToString()+"%";//a.ToString("#0.0")
        profitCoins_Text_3.text = "+" + ParkingCenter.GetParkingProfit(profitLevel, 1).ToString()+"%";
        profitCoins_LvText.text = profitLevel.ToString();
        parkingSpace_Text2.text = ParkingCenter.GetParkingSpace(parkingSpaceLevel).ToString();
        parkingSpace_Text3.text = "+" + (ParkingCenter.GetParkingSpace(parkingSpaceLevel, 1)).ToString();
        parkingSpace_LvText.text =  parkingSpaceLevel.ToString();
        enterCarSpawn_Text2.text = ParkingCenter.GetParkingEnterCarSpawn_UI(enterCarSpawnLevel).ToString("f2")+ GetL10NString("Ui_Text_67");
        enterCarSpawn_Text3.text = "+" + ParkingCenter.GetParkingEnterCarSpawn(enterCarSpawnLevel, 1).ToString("f2");
        enterCarSpawn_LvText.text = enterCarSpawnLevel.ToString();

        profitCoins_Button_NeedGoldNum.text = MinerBigInt.ToDisplay(consumeProfitCoins).ToString();       //升级模式需要的金钱
        profitCoins_Button_ButtonLvUpText.text = GetL10NString("Ui_Text_7");

        parkingSpace_Button_NeedGoldNum.text = MinerBigInt.ToDisplay(consumeParkingSpaceCoins).ToString();       //升级模式需要的金钱
        parkingSpace_Button_ButtonLvUpText.text = GetL10NString("Ui_Text_7");

        enterCarSpawn_Button_NeedGoldNum.text = MinerBigInt.ToDisplay(consumeEnterCarSpawnCoins).ToString();       //升级模式需要的金钱
        enterCarSpawn_Button_ButtonLvUpText.text = GetL10NString("Ui_Text_7");

        SetGradeBool_Profit();
        SetGradeBool_ParkingSpace();
        SetGradeBool_EnterCarSpawn();
        if (profitLevel >= parkingProfitMaxGrade)
        {
            profitCoins_Button_NeedGoldNum.text = GetL10NString("Ui_Text_47");       //升级模式需要的金钱
            profitCoins_Button_ButtonLvUpText.text = GetL10NString("Ui_Text_46");    //升级模式要升的级数
            lvText.text = parkingCell.lvmax.ToString();//等级text
            profitCoins_Text_3.text = GetL10NString("Ui_Text_47");  //价格变化标签
            SwitchButtonUnClickable(profitCoins_Button, false);
        }
        if (parkingSpaceLevel >= parkingSpaceMaxGrade)
        {
            parkingSpace_Button_NeedGoldNum.text = GetL10NString("Ui_Text_47");       //升级模式需要的金钱
            parkingSpace_Button_ButtonLvUpText.text = GetL10NString("Ui_Text_46");    //升级模式要升的级数
            parkingSpace_Text3.text = GetL10NString("Ui_Text_47"); //数量变化标签
            SwitchButtonUnClickable(parkingSpace_Button, false);
        }
        if (enterCarSpawnLevel >= parkingEnterCarSpawnMaxGrade)
        {
            enterCarSpawn_Button_NeedGoldNum.text = GetL10NString("Ui_Text_47");       //升级模式需要的金钱
            enterCarSpawn_Button_ButtonLvUpText.text = GetL10NString("Ui_Text_46");    //升级模式要升的级数
            enterCarSpawn_Text3.text = GetL10NString("Ui_Text_47"); //速度变化标签
            SwitchButtonUnClickable(enterCarSpawn_Button, false);
        }
        if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide)
        {
            UIGuidePage uIGuidePage = PageMgr.GetPage<UIGuidePage>();
            if (uIGuidePage == null)
            {
                string e = string.Format("新手引导界面  PageMgr.allPages里 UIGuidePage   为空");
                throw new System.Exception(e);
            }
            if (uIGuidePage.newBieGuild_step == NewBieGuild.Step_5)
            {
                SwitchButtonUnClickable(parkingSpace_Button, true);
                SwitchButtonUnClickable(profitCoins_Button, false);
                SwitchButtonUnClickable(enterCarSpawn_Button, false);
            }
            else if (uIGuidePage.newBieGuild_step == NewBieGuild.Step_8)
            {
                SwitchButtonUnClickable(parkingSpace_Button, false);
                SwitchButtonUnClickable(profitCoins_Button, false);
                SwitchButtonUnClickable(enterCarSpawn_Button, true);
            }
        }
        //else
        //{
        //    SwitchButtonUnclickable(parkingSpace_Button, true);
        //    SwitchButtonUnclickable(profitCoins_Button, true);
        //    SwitchButtonUnclickable(enterCarSpawn_Button, true);
        //}

        if (profitLevel >= parkingProfitMaxGrade)
        {
            lvText.text = string.Format(GetL10NString("Ui_Text_2"), GetL10NString("Ui_Text_126")); ;
            gradeSlider.value = 1;
            gradeText_2.text = GetL10NString("Ui_Text_126");  //最大等级上限
            gradeSlider_IconBg.gameObject.SetActive(false);
            scoreNumTest.text = GetL10NString("Ui_Text_126");

        }
        else
        {
            gradeSlider_IconBg.gameObject.SetActive(true);
        }

    }

    /// <summary>
    /// 更新数据  页面显示
    /// </summary> 
    public override void Refresh()
    {
        base.Refresh();
    }
    /// <summary>
    /// 激活
    /// </summary>
    public override void Active()
    {
        base.Active();
        SetCorrectShowImage();
        this.InitData();//初始化属性数值
        //注册监听消息     
        MessageManager.GetInstance().Regist((int)GameMessageDefine.BroadcastCoinOfPlayerDataMSSC, this.OnGetBroadcastCoinOfPlayerDataMSSC);//接受金钱变动的信息
        MessageManager.GetInstance().Regist((int)GameMessageDefine.BroadcastParkingProfitLevelOfPlayerData, this.OnGetBroadcastParkingProfitLevelOfPlayerData);//接收停车场的利润等级的广播
        MessageManager.GetInstance().Regist((int)GameMessageDefine.BroadcastParkingSpaceLevelOfPlayerData, this.OnGetBroadcastParkingSpaceLevelOfPlayerData);//接收停车场的位置数量等级的广播
        MessageManager.GetInstance().Regist((int)GameMessageDefine.BroadcastParkingEnterCarSpawnLevelOfPlayerData, this.OnGetBroadcastParkingEnterCarSpawnLevelOfPlayerData);//接收停车场的来客流量等级的广播
        MessageManager.GetInstance().Regist((int)GameMessageDefine.UIMessage_OpenOfflinePage, OnOpenOfflineUIPage);


        /*  若是新手引导阶段，进入特殊处理方法  */
        if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide == true)
        {
            DelayedOperationNewbieGuideStage();
        }
    }



    /// <summary>
    /// 隐藏
    /// </summary>
    public override void Hide()
    {
        base.Hide();
        OnReleaseButton();
        MessageManager.GetInstance().UnRegist((int)GameMessageDefine.BroadcastParkingProfitLevelOfPlayerData, this.OnGetBroadcastParkingProfitLevelOfPlayerData);//接收停车场的利润等级的广播
        MessageManager.GetInstance().UnRegist((int)GameMessageDefine.BroadcastParkingSpaceLevelOfPlayerData, this.OnGetBroadcastParkingSpaceLevelOfPlayerData);//接收停车场的位置数量等级的广播
        MessageManager.GetInstance().UnRegist((int)GameMessageDefine.BroadcastParkingEnterCarSpawnLevelOfPlayerData, this.OnGetBroadcastParkingEnterCarSpawnLevelOfPlayerData);//接收停车场的来客流量等级的广播
        MessageManager.GetInstance().UnRegist((int)GameMessageDefine.BroadcastCoinOfPlayerDataMSSC, this.OnGetBroadcastCoinOfPlayerDataMSSC);//接受金钱变动的信息
        MessageManager.GetInstance().UnRegist((int)GameMessageDefine.UIMessage_OpenOfflinePage, OnOpenOfflineUIPage);


        MessageString.Send((int)GameMessageDefine.UIMessage_ActiveButShowPart, "UIMainPage");
        //UIInteractive.GetInstance().iPage = null;

    }

    private void HideButtonUI(string str)
    {
        UIInteractive.GetInstance().SetClosePage(this);
    }  
}

