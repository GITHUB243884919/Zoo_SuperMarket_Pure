using DG.Tweening;
using Game;
using Game.GlobalData;
using Game.MessageCenter;
using UFrame.Logger;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
//using System.Resources;
using UFrame;
using UFrame.MessageCenter;
using UFrame.MiniGame;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public partial class UIZooPage : UIPage
{
    string switchButton;//存储修改Ui 名字
    private Dictionary<int, Button> setButtonsDic;//设置UI显示的按钮列表
    private Dictionary<int, Transform> animalTransformData;//UI显示的按钮列表
    private Dictionary<string, int> animalAndID ;//设置UI显示的按钮列表
    private List<Transform> animalCellList;
    private List<int> animalCellID = new List<int>();

    PlayerData playerData;


    Config.groupCell groupCell;
    /// <summary>
    /// 当前动物栏的id
    /// </summary>
    int nameID;
    /// <summary>
    /// 语言表   当前动物栏的名字可以从表里获取
    /// </summary>
    Config.translateCell littleZooName;
    /// <summary>
    /// 动物栏Data  信息集合
    /// </summary>
    Config.buildupCell buildUpCell;
    /// <summary>
    /// 玩家的动物数据
    /// </summary>
    PlayerAnimal_MSS_15 animalMSS15;
    bool isAnimalUp=false;
    /// <summary>
    /// 动物栏门票等级
    /// </summary>
    int littleZooTicketsLevel;
    /// <summary>
    /// 观光位数量等级
    /// </summary>
    int littleZooVisitorSeatLevel;
    /// <summary>
    /// 观光游客的流量等级
    /// </summary>
    int littleZooEnterVisitorSpawnLevel;
    /// <summary>
    /// 动物栏的动物数量
    /// </summary>
    int zooNumber;
    /// <summary>
    /// 当前要升级的规模参数,   默认为0
    /// </summary>
    int upGradeNumber = 1;
    /// <summary>
    /// 是否可以升级
    /// </summary>
    bool setGradeBool;
    /// <summary>
    /// 门票当前最大等级段
    /// </summary>
    int maxGrade;

    int oldMaxGrade;
    /// <summary>
    /// 策划表配置的最大等级
    /// </summary>
    int TicketsMaxGrade;
    /// <summary>
    /// 观光位数量当前最大等级段
    /// </summary>
    int VisitorSeatMaxGrade;
    /// <summary>
    /// 观光游客的流量当前最大等级段
    /// </summary>
    int EnterVisitorSpawnMaxGrade;
    /// <summary>
    /// 当前金钱
    /// </summary>
    BigInteger coinVal;
    /// <summary>
    /// 记录动物栏利润要升级需要消费的钱币
    /// </summary
    BigInteger ticketsLevelConsumeCoins;
    /// <summary>
    /// 记录动物栏观光位数量要升级需要消费的钱币
    /// </summary
    BigInteger visitorSeatLevelConsumeCoins;
    /// <summary>
    /// 记录动物栏流量等级要升级需要消费的钱币
    /// </summary
    BigInteger EnterVisitorSpawnLevelConsumeCoins;
    ///// <summary>
    ///// 记录动物要升级需要消费的钱币
    ///// </summary
    //BigInteger animalConsumeCoins;
    /// <summary>
    /// 是否扣钱成功是否收到回复
    /// </summary>
    bool isGetCoin=true;
    /// <summary>
    /// 判断是否是长按状态
    /// </summary>
    bool isLongPress = false;

    int animalLvUpLimit;
    /// <summary>
    /// 判断是否是长按状态
    /// </summary>
    bool isLongPressBuyAnimal = false;
    /// <summary>
    /// 当前动物栏的初始票价
    /// </summary>
    BigInteger pricebase;

    Image profitCoins_Icon;
    Image profitCoins_GoldIcon;

    #region 动物栏父类控件字段（共显）
    Button zooCultivateButton;   //切换Cultivate按钮
    Button zooKindButton;        //切换Kind按钮
    GameObject zooCultivateUI;
    GameObject zooKindUI;
    GameObject animalGroup;
    Text titleText;
    Text lvText;
    Text tipsText;          //释义语言
    Text tipsTextAnimalup;  //动物栏动物养成的释义语言
    Text scoreNumTest;     //UI的星星收集显示
    int starLevelReached;
    Button lefeButton;
    Button reghtButton;
    Button hideUIButton;
    Image animalIcon;
    Image goldIcon;
    #endregion
    #region 动物栏子类控件字段（养成独显）
    Slider LVUpSlider;
    Image LVUpSlider_animalIcon;
    Image LVUpSlider_goldIcon;
    Text LVUpSlider_Text;
    Image animalShow;
    Image iconSlider;
    Text lVUpSliderText;

    Text ticketsText;      //利润text
    Text tickets_Text2;
    Text tickets_Text3;
    Text tickets_LvText;
    Button tickets_Button;
    Text tickets_Button_NeedGoldText;
    Text tickets_Button_buttonLvUpText;
    Transform tickets_EffectNode;

    Text visitorSeatText;        //数量Text
    Text visitorSeat_Text2;
    Text visitorSeat_Text3;
    Text visitorSeat_LvText;
    Button visitorSeat_Button;
    Text visitorSeat_Button_NeedGoldText;
    Text visitorSeat_Button_buttonLvUpText;
    Transform visitorSeat_EffectNode;

    Text visitorSpawnText;       //冷却时间
    Text visitorSpawn_Text2;
    Text visitorSpawn_Text3;
    Text visitorSpawn_LvText;
    Button visitorSpawn_Button;
    Text visitorSpawn_Button_NeedGoldText;
    Text visitorSpawn_Button_buttonLvUpText;
    Transform visitorSpawn_EffectNode;

    Transform effectNode;
   
    Text animalNumber;
    Image buttonIcon_1;
    Image buttonIcon_2;
    Image buttonIcon_3;
    #endregion
    #region 动物养成界面（独显）
    Transform animal_1;
    Transform animal_2;
    Transform animal_3;
    Transform animal_4;
    Transform animal_5;
    private Sprite sprite;

    #endregion

    public UIZooPage() : base(UIType.Fixed, UIMode.DoNothing, UICollider.None)
    {
        uiPath = "uiprefab/UINewBuild";
    }
    public override void Awake(GameObject go)
    {
        base.Awake(go);

        GetTransPrefabAllTextShow(this.transform);

        //默认开启ZooCultivate
        switchButton = "ZooCultivateButton";
        animalTransformData = new Dictionary<int, Transform>();
        animalAndID = new Dictionary<string, int>();
        playerData = GlobalDataManager.GetInstance().playerData;
        //初始化控件  
        this.RegistAllCompent();
        this.RegistZooKindCompent();
        this.RegistAnimalCompent();
       
    }

    /// <summary>
    /// 给动物分页分别添加控件
    /// </summary>
    private void RegistAnimalCompent()
    {
        buildUpCell = Config.buildupConfig.getInstace().getCell(m_data.ToString());
        int childCount = animalGroup.transform.childCount;

        float height = animalGroup.GetComponent<RectTransform>().sizeDelta.y;
        
        animalGroup.GetComponent<RectTransform>().sizeDelta = new UnityEngine.Vector2( 20f + 238f * childCount, height);
        /*按钮*/
        animal_1 = RegistCompent<Transform>("UIPage/UiBg2/UIBuild_Animalup/ScorllView/AnimalGroup/Animal_1");
        Button animal_1_button = RegistBtnAndClick("UIPage/UiBg2/UIBuild_Animalup/ScorllView/AnimalGroup/Animal_1/BuyButton", BuyPlayerAnimal01);
        animal_2 = RegistCompent<Transform>("UIPage/UiBg2/UIBuild_Animalup/ScorllView/AnimalGroup/Animal_2");
        Button animal_2_button = RegistBtnAndClick("UIPage/UiBg2/UIBuild_Animalup/ScorllView/AnimalGroup/Animal_2/BuyButton", BuyPlayerAnimal02);
        animal_3 = RegistCompent<Transform>("UIPage/UiBg2/UIBuild_Animalup/ScorllView/AnimalGroup/Animal_3");
        Button animal_3_button = RegistBtnAndClick("UIPage/UiBg2/UIBuild_Animalup/ScorllView/AnimalGroup/Animal_3/BuyButton", BuyPlayerAnimal03);
        animal_4 = RegistCompent<Transform>("UIPage/UiBg2/UIBuild_Animalup/ScorllView/AnimalGroup/Animal_4");
        Button animal_4_button = RegistBtnAndClick("UIPage/UiBg2/UIBuild_Animalup/ScorllView/AnimalGroup/Animal_4/BuyButton", BuyPlayerAnimal04);
        animal_5 = RegistCompent<Transform>("UIPage/UiBg2/UIBuild_Animalup/ScorllView/AnimalGroup/Animal_5");
        Button animal_5_button = RegistBtnAndClick("UIPage/UiBg2/UIBuild_Animalup/ScorllView/AnimalGroup/Animal_5/BuyButton", BuyPlayerAnimal05);

        /*往动物购买按钮中添加长按*/
        animal_1_button.gameObject.AddComponent<RepeatButton>();
        animal_1_button.GetComponent<RepeatButton>().onPress.AddListener(OnLongPressBuyAnimal01);//按下。
        animal_1_button.GetComponent<RepeatButton>().onRelease.AddListener(OnReleaseBuyAnimal);//抬起，调用一次
        animal_2_button.gameObject.AddComponent<RepeatButton>();
        animal_2_button.GetComponent<RepeatButton>().onPress.AddListener(OnLongPressBuyAnimal02);//按下。
        animal_2_button.GetComponent<RepeatButton>().onRelease.AddListener(OnReleaseBuyAnimal);//抬起，调用一次
        animal_3_button.gameObject.AddComponent<RepeatButton>();
        animal_3_button.GetComponent<RepeatButton>().onPress.AddListener(OnLongPressBuyAnimal03);//按下。
        animal_3_button.GetComponent<RepeatButton>().onRelease.AddListener(OnReleaseBuyAnimal);//抬起，调用一次
        animal_4_button.gameObject.AddComponent<RepeatButton>();
        animal_4_button.GetComponent<RepeatButton>().onPress.AddListener(OnLongPressBuyAnimal04);//按下。
        animal_4_button.GetComponent<RepeatButton>().onRelease.AddListener(OnReleaseBuyAnimal);//抬起，调用一次
        animal_5_button.gameObject.AddComponent<RepeatButton>();
        animal_5_button.GetComponent<RepeatButton>().onPress.AddListener(OnLongPressBuyAnimal05);//按下。
        animal_5_button.GetComponent<RepeatButton>().onRelease.AddListener(OnReleaseBuyAnimal);//抬起，调用一次
        animalCellList = new List<Transform>();
        animalCellList.Add(animal_1);
        animalCellList.Add(animal_2);
        animalCellList.Add(animal_3);
        animalCellList.Add(animal_4);
        animalCellList.Add(animal_5);
    }
    /// <summary>
    /// 内部组件的查找赋值
    /// </summary>
    private void RegistAllCompent()
    {
        zooCultivateButton = RegistBtnAndClick("UIPage/ZooCultivateButton", OnClickSetUIButton);
        zooKindButton = RegistBtnAndClick("UIPage/ZooKindButton", OnClickSetUIButton);

        setButtonsDic = new Dictionary<int, Button>();
        setButtonsDic.Add(1, zooCultivateButton);
        setButtonsDic.Add(10, zooKindButton);
        zooCultivateUI = GameObject.Find("UIPage/UiBg2/UIBuild_Animalup");
        zooKindUI = GameObject.Find("UIPage/UiBg2/UIBuild_LvUp");
        animalGroup = GameObject.Find("UIPage/UiBg2/UIBuild_Animalup/ScorllView/AnimalGroup");
        titleText = RegistCompent<Text>("UIPage/TitleGroup/TitleText");
        lvText = RegistCompent<Text>("UIPage/TitleGroup/LvText");
        tipsText = RegistCompent<Text>("UIPage/TitleGroup/TipsText");
        tipsTextAnimalup = RegistCompent<Text>("UIPage/UiBg2/TipsTextAnimalup");
        //GetTransPrefabText(tipsTextAnimalup);
        //lefeButton = RegistBtnAndClick("UIPage/TitleGroup/ArrowButtonLeft", SwitchLittleZooSend);
        //reghtButton = RegistBtnAndClick("UIPage/TitleGroup/ArrowButtonRight", SwitchLittleZooSend);
        animalShow = RegistCompent<Image>("UIPage/AnimalShow");
        hideUIButton = RegistBtnAndClick("UIPage/AnimalShow/Image", HideButtonUI);
    

    }
    private void RegistZooKindCompent()
    {
        
        /*查找控件*/
        LVUpSlider = RegistCompent<Slider>("UIPage/UiBg2/LvUpSchedule/Schedule/Slider2");
        lVUpSliderText = RegistCompent<Text>("UIPage/UiBg2/LvUpSchedule/Schedule/Text_2");
        LVUpSlider_animalIcon = RegistCompent<Image>("UIPage/UiBg2/LvUpSchedule/Schedule/IconBg/AnimalIcon");
        LVUpSlider_goldIcon = RegistCompent<Image>("UIPage/UiBg2/LvUpSchedule/Schedule/IconBg/GoldIcon");

        LVUpSlider_Text = RegistCompent<Text>("UIPage/UiBg2/LvUpSchedule/Schedule/IconBg/Num");
        scoreNumTest = RegistCompent<Text>("UIPage/UiBg2/ScoreGroup/ScoreNum");
        iconSlider = RegistCompent<Image>("UIPage/UiBg2/LvUpSchedule/Schedule/IconBg");
        //iconSlider.gameObject.SetActive(false);
        ticketsText = RegistCompent<Text>("UIPage/UiBg2/UIBuild_LvUp/ParameterGroup/Parameter/Parameter_1/Text_1");      //收益text
        //GetTransPrefabText(ticketsText);
        tickets_Text2 = RegistCompent<Text>("UIPage/UiBg2/UIBuild_LvUp/ParameterGroup/Parameter/Parameter_1/TextAll/Text_2");
        tickets_Text3 = RegistCompent<Text>("UIPage/UiBg2/UIBuild_LvUp/ParameterGroup/Parameter/Parameter_1/TextAll/Text_3");
        tickets_LvText = RegistCompent<Text>("UIPage/UiBg2/UIBuild_LvUp/ParameterGroup/Parameter/Parameter_1/Level/LvText");
        tickets_Button = RegistBtnAndClick("UIPage/UiBg2/UIBuild_LvUp/ParameterGroup/Parameter/Parameter_1/Button", OnClickUpGrade_Tickets);
        tickets_Button.gameObject.AddComponent<RepeatButton>();
        tickets_Button.GetComponent<RepeatButton>().onPress.AddListener(OnLongPress_Tickets);//按下。频繁的调用
        tickets_Button.GetComponent<RepeatButton>().onRelease.AddListener(OnRelease_Tickets);//抬起，调用一次
        tickets_Button_NeedGoldText = RegistCompent<Text>("UIPage/UiBg2/UIBuild_LvUp/ParameterGroup/Parameter/Parameter_1/Button/NeedGoldNum");
        tickets_Button_buttonLvUpText = RegistCompent<Text>("UIPage/UiBg2/UIBuild_LvUp/ParameterGroup/Parameter/Parameter_1/Button/ButtonLvUpText");
        tickets_EffectNode = RegistCompent<Transform>("UIPage/UiBg2/UIBuild_LvUp/ParameterGroup/Parameter/Parameter_1/effectNode");

        visitorSeatText = RegistCompent<Text>("UIPage/UiBg2/UIBuild_LvUp/ParameterGroup/Parameter/Parameter_2/Text_1");        //数量Text
        //GetTransPrefabText(visitorSeatText);
        visitorSeat_Text2 = RegistCompent<Text>("UIPage/UiBg2/UIBuild_LvUp/ParameterGroup/Parameter/Parameter_2/TextAll/Text_2");
        visitorSeat_Text3 = RegistCompent<Text>("UIPage/UiBg2/UIBuild_LvUp/ParameterGroup/Parameter/Parameter_2/TextAll/Text_3");
        visitorSeat_LvText = RegistCompent<Text>("UIPage/UiBg2/UIBuild_LvUp/ParameterGroup/Parameter/Parameter_2/Level/LvText");
        visitorSeat_Button = RegistBtnAndClick("UIPage/UiBg2/UIBuild_LvUp/ParameterGroup/Parameter/Parameter_2/Button", OnClickUpGrade_VisitorSeat);
        visitorSeat_Button.gameObject.AddComponent<RepeatButton>();
        visitorSeat_Button.GetComponent<RepeatButton>().onPress.AddListener(OnLongPress_VisitorSeat);//按下。频繁的调用
        visitorSeat_Button.GetComponent<RepeatButton>().onRelease.AddListener(OnRelease_VisitorSeat);//抬起，调用一次
        visitorSeat_Button_NeedGoldText = RegistCompent<Text>("UIPage/UiBg2/UIBuild_LvUp/ParameterGroup/Parameter/Parameter_2/Button/NeedGoldNum");
        visitorSeat_Button_buttonLvUpText = RegistCompent<Text>("UIPage/UiBg2/UIBuild_LvUp/ParameterGroup/Parameter/Parameter_2/Button/ButtonLvUpText");
        visitorSeat_EffectNode = RegistCompent<Transform>("UIPage/UiBg2/UIBuild_LvUp/ParameterGroup/Parameter/Parameter_2/effectNode");


        visitorSpawnText = RegistCompent<Text>("UIPage/UiBg2/UIBuild_LvUp/ParameterGroup/Parameter/Parameter_3/Text_1");       //冷却时间
        //GetTransPrefabText(visitorSpawnText);
        visitorSpawn_Text2 = RegistCompent<Text>("UIPage/UiBg2/UIBuild_LvUp/ParameterGroup/Parameter/Parameter_3/TextAll/Text_2");
        visitorSpawn_Text3 = RegistCompent<Text>("UIPage/UiBg2/UIBuild_LvUp/ParameterGroup/Parameter/Parameter_3/TextAll/Text_3");
        visitorSpawn_LvText = RegistCompent<Text>("UIPage/UiBg2/UIBuild_LvUp/ParameterGroup/Parameter/Parameter_3/Level/LvText");
        visitorSpawn_Button = RegistBtnAndClick("UIPage/UiBg2/UIBuild_LvUp/ParameterGroup/Parameter/Parameter_3/Button", OnClickUpGrade_VisitorSpawn);
        visitorSpawn_Button.gameObject.AddComponent<RepeatButton>();
        visitorSpawn_Button.GetComponent<RepeatButton>().onPress.AddListener(OnLongPress_VisitorSpawn);//按下。频繁的调用
        visitorSpawn_Button.GetComponent<RepeatButton>().onRelease.AddListener(OnRelease_VisitorSpawn);//抬起，调用一次
        visitorSpawn_Button_NeedGoldText = RegistCompent<Text>("UIPage/UiBg2/UIBuild_LvUp/ParameterGroup/Parameter/Parameter_3/Button/NeedGoldNum");
        visitorSpawn_Button_buttonLvUpText = RegistCompent<Text>("UIPage/UiBg2/UIBuild_LvUp/ParameterGroup/Parameter/Parameter_3/Button/ButtonLvUpText");
        visitorSpawn_EffectNode = RegistCompent<Transform>("UIPage/UiBg2/UIBuild_LvUp/ParameterGroup/Parameter/Parameter_3/effectNode");



        buttonIcon_1 = RegistCompent<Image>("UIPage/UiBg2/UIBuild_LvUp/ParameterGroup/Parameter/Parameter_1/Button/GoldIcon");
        buttonIcon_2 = RegistCompent<Image>("UIPage/UiBg2/UIBuild_LvUp/ParameterGroup/Parameter/Parameter_2/Button/GoldIcon");
        buttonIcon_3 = RegistCompent<Image>("UIPage/UiBg2/UIBuild_LvUp/ParameterGroup/Parameter/Parameter_3/Button/GoldIcon");

        profitCoins_Icon = RegistCompent<Image>("UIPage/UiBg2/UIBuild_LvUp/ParameterGroup/Parameter/Parameter_1/Icon");
        profitCoins_GoldIcon = RegistCompent<Image>("UIPage/UiBg2/UIBuild_LvUp/ParameterGroup/Parameter/Parameter_1/GoldIcon");
        SetCorrectShowImage();

        
    }

    /// <summary>
    /// 设置切换UI界面的按钮点击事件  根据按钮切换UI显隐
    /// </summary>
    /// <param name="button"></param>
    private void OnClickSetUIButton(string name)
    {
        foreach (var item in setButtonsDic.Keys)
        {
            setButtonsDic.TryGetValue(item, out Button value);
            Transform text_Butoon = value.transform.Find("Text");
            Transform image_button01 = value.transform.Find("ButtonBg_1");
            Transform image_button02 = value.transform.Find("ButtonBg_2");
            Transform image_button03 = value.transform.Find("Image");
            if (value.name == name)
            {
                text_Butoon.gameObject.SetActive(true);
                image_button01.gameObject.SetActive(true);
                image_button02.gameObject.SetActive(false);
                image_button03.gameObject.SetActive(true);
                switchButton = name;
            }
            else//切换非选中状态的图片
            {
                text_Butoon.gameObject.SetActive(false);
                image_button01.gameObject.SetActive(true);
                image_button02.gameObject.SetActive(true);
                image_button03.gameObject.SetActive(true);
            }
        }
        //刷新显示Ui界面
        this.InitCompent();
    }

    public override void Refresh()
    {
        base.Refresh();
    }
    public override void Active()
    {
        base.Active();
        this.InitData();
        SetCorrectShowImage();
        InitAnimalData();

        //控件显示赋值
        OnClickSetUIButton("ZooCultivateButton");
        //监听动物栏升级广播
        MessageManager.GetInstance().Regist((int)GameMessageDefine.BroadcastLittleZooTicketsLevelPlayerData, this.OnGetBroadcastLittleZooTicketsLevelPlayerData);
        MessageManager.GetInstance().Regist((int)GameMessageDefine.BroadcastLittleZooEnterVisitorSpawnLevelOfPlayerData, this.OnGetBroadcastLittleZooEnterVisitorSpawnLevelOfPlayerData);
        MessageManager.GetInstance().Regist((int)GameMessageDefine.BroadcastLittleZooVisitorLocationLevelOfPlayerData, this.OnGetBroadcastLittleZooVisitorLocationLevelOfPlayerData);
        MessageManager.GetInstance().Regist((int)GameMessageDefine.BroadcastCoinOfPlayerDataMSSC, this.OnGetBroadcastCoinOfPlayerDataMSSC);//接受金钱变动的信息
        MessageManager.GetInstance().Regist((int)GameMessageDefine.GetAnimalLevel, this.GetAchievementSetObject);
        MessageManager.GetInstance().Regist((int)GameMessageDefine.UIMessage_OpenOfflinePage, OnOpenOfflineUIPage);
        /*  若是新手引导阶段，进入特殊处理方法  */
        if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide == true)
        {
            DelayedOperationNewbieGuideStage();
        }
    }

    private void SetCorrectShowImage()
    {
        int scenetype = Config.sceneConfig.getInstace().getCell(playerData.playerZoo.currSceneID).moneyid;
        string iconPath = Config.moneyConfig.getInstace().getCell(scenetype).moneyicon;
        sprite = ResourceManager.LoadSpriteFromPrefab(iconPath);
        profitCoins_Icon.sprite = sprite;
        profitCoins_GoldIcon.sprite = sprite;
        buttonIcon_1.sprite = sprite;
        buttonIcon_2.sprite = sprite;
        buttonIcon_3.sprite = sprite;
    }
    /// <summary>
    /// 隐藏
    /// </summary>
    public override void Hide()
    {
        isPause = false;
        OnRelease_Tickets();
        OnRelease_VisitorSeat();
        OnRelease_VisitorSpawn();
        OnReleaseBuyAnimal();
        MessageManager.GetInstance().UnRegist((int)GameMessageDefine.BroadcastLittleZooTicketsLevelPlayerData, this.OnGetBroadcastLittleZooTicketsLevelPlayerData);
        MessageManager.GetInstance().UnRegist((int)GameMessageDefine.BroadcastLittleZooEnterVisitorSpawnLevelOfPlayerData, this.OnGetBroadcastLittleZooEnterVisitorSpawnLevelOfPlayerData);
        MessageManager.GetInstance().UnRegist((int)GameMessageDefine.BroadcastLittleZooVisitorLocationLevelOfPlayerData, this.OnGetBroadcastLittleZooVisitorLocationLevelOfPlayerData);
        MessageManager.GetInstance().UnRegist((int)GameMessageDefine.BroadcastCoinOfPlayerDataMSSC, this.OnGetBroadcastCoinOfPlayerDataMSSC);//接受金钱变动的信息
        MessageManager.GetInstance().UnRegist((int)GameMessageDefine.GetAnimalLevel, this.GetAchievementSetObject);
        MessageManager.GetInstance().UnRegist((int)GameMessageDefine.UIMessage_OpenOfflinePage, OnOpenOfflineUIPage);
        MessageString.Send((int)GameMessageDefine.UIMessage_ActiveButShowPart, "UIMainPage");
        //UIInteractive.GetInstance().iPage = null;
        DestroyEffectChild();
        base.Hide();
    }
    /// <summary>
    /// 争对新手引导阶段做些操作
    /// </summary>
    private void DelayedOperationNewbieGuideStage()
    {
        //根据新手引导阶段的步骤显示对应的特效和隐藏对应的按钮点击事件
        UIGuidePage uIGuidePage = PageMgr.GetPage<UIGuidePage>();
        if (uIGuidePage == null)
        {
            string e = string.Format("新手引导界面  PageMgr.allPages里 UIGuidePage   为空");
            throw new System.Exception(e);
        }
        if (uIGuidePage.newBieGuild_step == NewBieGuild.Step_20)
        {
            //停车场停车位按钮处显示小手点击动画
            effectNode = tickets_Button.transform.Find("effectNode");
            Transform trans = null;
            trans = ResourceManager.GetInstance().LoadGameObject(Config.globalConfig.getInstace().GuideUiClickEffect).transform;
            trans.SetParent(effectNode, true);
            trans.localScale = UnityEngine.Vector3.one;
            trans.position = effectNode.position;
            trans.localPosition = new UnityEngine.Vector3(
                trans.localPosition.x,
                trans.localPosition.y+4,
                trans.localPosition.z);
        }
    }
    private void HideButtonUI(string str)
    {
        UIInteractive.GetInstance().SetClosePage(this);
    }

    /// <summary>
    /// 判断利润是否可以升级（钱够/等级不超过最大值）
    /// </summary>
    /// <returns></returns>
    private bool SetGradeBool_Tickets()
    {
        if (ticketsLevelConsumeCoins <= coinVal && littleZooTicketsLevel <= TicketsMaxGrade)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 判断 观光位数量是否可以升级（钱够/等级不超过最大值）
    /// </summary>
    /// <returns></returns>
    private bool SetGradeBool_VisitorSeat()
    {
        if (visitorSeatLevelConsumeCoins <= coinVal && littleZooVisitorSeatLevel <= VisitorSeatMaxGrade)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 判断观光游客的流量是否可以升级（钱够/等级不超过最大值）
    /// </summary>
    /// <returns></returns>
    private bool SetGradeBool_VisitorSpawn()
    {
        if (EnterVisitorSpawnLevelConsumeCoins <= coinVal && littleZooEnterVisitorSpawnLevel <= EnterVisitorSpawnMaxGrade)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 判断动物购买是否可以升级（钱够/等级不超过最大值）   等级判断没做
    /// </summary>
    /// <returns></returns>
    private bool SetBuyAnimalGradeBool(int animalID)
    {
        if (LittleZooModule.GetAnimalUpLevelPriceFormula(animalID) <= playerData.playerZoo.diamond)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 获取等级段对应的奖励信息
    /// </summary>
    /// <returns></returns>
    private void GradeSliderAwardImage()
    {
        var lvreward = buildUpCell.lvreward;
        int itemID;
        int idx = PlayerDataModule.FindLevelRangIndex(buildUpCell.lvshage, littleZooTicketsLevel);
        if (buildUpCell.lvrewardtype[idx]==2)
        {  //动物
            itemID = lvreward[idx];
            string path = Config.animalupConfig.getInstace().getCell(itemID).icon;
            LVUpSlider_animalIcon.gameObject.SetActive(true);
            LVUpSlider_animalIcon.sprite = ResourceManager.LoadSpriteFromPrefab(path);
            LVUpSlider_goldIcon.gameObject.SetActive(false);
            LVUpSlider_Text.text = "";
        }
        else if (buildUpCell.lvrewardtype[idx] == 1)
        {
            itemID = lvreward[idx];
            Config.itemCell itemCell = Config.itemConfig.getInstace().getCell(itemID);
            LVUpSlider_goldIcon.gameObject.SetActive(true);
            LVUpSlider_goldIcon.sprite = ResourceManager.LoadSpriteFromPrefab(itemCell.icon);
            LVUpSlider_animalIcon.gameObject.SetActive(false);

            LVUpSlider_Text.text = MinerBigInt.ToDisplay(itemCell.itemval);
        }
        if (littleZooTicketsLevel >= TicketsMaxGrade)
        {
            iconSlider.gameObject.SetActive(false);
        }
        else
        {
            iconSlider.gameObject.SetActive(true);

        }
    }



    
}