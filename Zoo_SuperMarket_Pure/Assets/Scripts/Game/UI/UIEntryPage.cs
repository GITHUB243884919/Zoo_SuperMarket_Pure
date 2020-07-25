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
using UFrame.MiniGame;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public partial class UIEntryPage : UIPage
{
    private enum UIEntryCell_Enum
    {
        Tickets = -1,
        Gate_01 = 0,
        Gate_02 = 1,
        Gate_03 = 2,
        Gate_04 = 3,
        Gate_05 = 4,
        Gate_06 = 5,
        Gate_07 = 6,
        Gate_08 = 7,
    }

    BigInteger coinVal;
    int entryTicketsLevel;
    Image gradeSlider_IconBg;
    BigInteger consumeCoins;                            //记录要升级需要消费的钱币
    int maxGrade;//最大等级
    int oldMaxGrade;
    PlayerData playerData;
    private List<GateData> entryGateList = new List<GateData>();
    List<int> sortGateIDs;
    /// <summary>
    /// 策划表配置的最大等级
    /// </summary>
    int entryMaxGrade;

    /// <summary>
    /// 是否扣钱成功是否收到回复
    /// </summary>
    bool isGetCoin = true;

    /// <summary>
    /// 判断是否是长按状态
    /// </summary>
    bool isLongPress = false;

    Config.ticketCell ticketCell;
    #region 全局UI控件属性
    Button hideUIButton;
    Text titleText;         //名称
    Text tipsText;          //释义语言
    Text lvText;            //等级text

    Transform allEntryCell;//所有的售票口父类

    Transform effectNode;   //新手引导手势节点
    Slider gradeSlider;
    Image gradeSlider_Image;
    Text gradeSlider_Text;

    Text gradeText;  //价格标签3

    Text scoreNumTest;     //UI的星星收集显示
    int starLevelReached;

    Sprite sprite;



    #endregion
    public UIEntryPage() : base(UIType.Fixed, UIMode.DoNothing, UICollider.None)
    {
        uiPath = "uiprefab/UINewTicket";
    }
    public override void Awake(GameObject go)
    {
        base.Awake(go);
        playerData = GlobalDataManager.GetInstance().playerData;
        SetCorrectShowImage();
        //初始化控件
        this.RegistAllCompent();
        GetTransPrefabAllTextShow(this.transform);
    }

    #region 内部组件查找
    /// <summary>
    /// 内部组件的查找
    /// </summary>
    private void RegistAllCompent()
    {
        /*  若是新手引导阶段，进入特殊处理方法  */
        entryGateList = GlobalDataManager.GetInstance().playerData.GetEntryDateDataIDIndexOfDataIdx().entryGateList;

        titleText = RegistCompent<Text>("UIFerryCar_LvUp/TitleGroup/TitleText");
        //GetTransPrefabText(titleText);
        tipsText = RegistCompent<Text>("UIFerryCar_LvUp/TitleGroup/TipsText");
        //GetTransPrefabText(tipsText);
        //当前等级
        lvText = RegistCompent<Text>("UIFerryCar_LvUp/TitleGroup/LvText");

        hideUIButton = RegistBtnAndClick("UIFerryCar_LvUp/Image/UIImage", HideButtonUI);

        //等级进度条控件
        gradeSlider = RegistCompent<Slider>("LvUpSchedule/Schedule/Slider2");
        gradeSlider_Image = RegistCompent<Image>("LvUpSchedule/Schedule/IconBg/Icon");
        gradeSlider_IconBg = RegistCompent<Image>("LvUpSchedule/Schedule/IconBg");

        gradeSlider_Text = RegistCompent<Text>("LvUpSchedule/Schedule/IconBg/Num");
        gradeText = RegistCompent<Text>("LvUpSchedule/Schedule/Text_2");

        scoreNumTest = RegistCompent<Text>("UIFerryCar_LvUp/ScoreGroup/ScoreNum");

        allEntryCell = RegistCompent<Transform>("UIFerryCar_LvUp/ParameterGroup/Parameter/ScorllView/AnimalGroup");
        RegistInitEveryCompent();
        
    }
    /// <summary>
    /// 初始化所有的售票口UI为未开启状态
    /// </summary>
    private void RegistInitEveryCompent()
    {
        string path = "UIFerryCar_LvUp/ParameterGroup/Parameter/ScorllView/AnimalGroup/{0}/{1}";
        for (int i = 0; i < allEntryCell.childCount; i++)
        {
            string name = allEntryCell.GetChild(i).name;
            allEntryCell.GetChild(i).gameObject.SetActive(true);
            Text nameText = RegistCompent<Text>(string.Format(path, name, "Text_1"));
            //GetTransPrefabText(nameText);
            nameText.text = "未开启";

            Text Text_2 = RegistCompent<Text>(string.Format(path, name, "TextAll/Text_2"));
            //GetTransPrefabText(Text_2);
            Text Text_3 = RegistCompent<Text>(string.Format(path, name, "TextAll/Text_3"));
            Text LvText = RegistCompent<Text>(string.Format(path, name, "level/LvText"));
            Text serialText = RegistCompent<Text>(string.Format(path, name, "ID/LvText"));
            //GetTransPrefabText(Text_3);
            Button button = RegistCompent<Button>(string.Format(path, name, "Button"));
            Text button_Text_2 = button.transform.Find("NeedGoldNum").GetComponent<Text>();
            Text button_Text_3 = button.transform.Find("ButtonLvUpText").GetComponent<Text>();
            button.gameObject.SetActive(false);
            Image button_GoldIcon = button.transform.Find("GoldIcon").GetComponent<Image>();
            button_GoldIcon.sprite = sprite;
            UIEntryCell_Enum iEntryCell_Enum = (UIEntryCell_Enum)(i - 1);

            if (i == 0)
            {
                nameText.text = GetL10NString("Ui_Text_13");
                Text_2.text = MinerBigInt.ToDisplay(EntryGateModule.GetEntryPrice(entryTicketsLevel,playerData.playerZoo.currSceneID,true));
                Text_3.text = MinerBigInt.ToDisplay(EntryGateModule.GetEntryPrice_Add(entryTicketsLevel, 1, playerData.playerZoo.currSceneID));
                LvText.text =  entryTicketsLevel.ToString();
                serialText.gameObject.SetActive(false);
                button = RegistCompent<Button>(string.Format(path, name, "Button"));
                button.onClick.AddListener(delegate {
                    OnLongPressButton_New(iEntryCell_Enum);
                });
                button_Text_2.text = MinerBigInt.ToDisplay(EntryGateModule.GetUpGradeConsumption(entryTicketsLevel));
                button_Text_3.text = GetL10NString("Ui_Text_7");
                button.gameObject.SetActive(true);
            }
        }

    }
    #endregion

    /// <summary>
    /// 设置对应的图片
    /// </summary>
    private void SetCorrectShowImage()
    {
        int scenetype = Config.sceneConfig.getInstace().getCell(playerData.playerZoo.currSceneID).moneyid;
        string iconPath = Config.moneyConfig.getInstace().getCell(scenetype).moneyicon;
        sprite = ResourceManager.LoadSpriteFromPrefab(iconPath);
    }
    
    #region 按钮升级事件
    private void SendSetTicketsLevelMessageManager()
    {
        //if (entryTicketsLevel >= entryMaxGrade)
        //    return;

        //if (!SetGradeBool())
        //    return;
        //发送消息       SetValueOfPlayerData  消息体    消息ID，UpGradeNumber  升多少级
        SetDetailValueOfPlayerData.Send((int)GameMessageDefine.SetEntryGateLevelOfPlayerData,
               1, 0, 0);
    }

    private void SendEntryGatePureLevelMessageManager(int id)
    {   //没有对钱进行判断和传值
        SetDetailValueOfPlayerData.Send((int)GameMessageDefine.SetEntryGatePureLevelOfPlayerData, id, 1, 0);
    }

    /// <summary>
    /// 点击开启新的售票口事件
    /// </summary>
    public void OnClickOpenNewEntry()
    {
        //获取下一个应该开启的ID
        int ID = playerData.GetEntryDateDataIDIndexOfDataIdx().entryGateList[playerData.GetEntryDateDataIDIndexOfDataIdx().entryGateList.Count - 1].entryID;
        SetValueOfPlayerData.Send((int)GameMessageDefine.SetEntryGateNumOfPlayerData,
               1, 0, ID + 1);
        isGetCoin = false;  //设置等待回复状态
        ///播放音乐
        string btnSoundPath = Config.globalConfig.getInstace().BuildUpButtonMusic;
        SoundManager.GetInstance().PlaySound(btnSoundPath);
    }
    #endregion

    #region 监听消息
    /// <summary>
    /// 监听玩家coin金钱发生改变，是否需要重新计算升级规模
    /// </summary>
    /// <param name="obj"></param>
    protected void OnGetBroadcastCoinOfPlayerDataMSSC(Message obj)
    {   //旧计算金钱不够，则开始新的计算
        this.InitData();
    }
    /// <summary>
    /// 监听售票口的门票等级
    /// </summary>
    /// <param name="msg"></param>
    protected void OnGetBroadcastEntryGateLevelOfPlayerData(Message msg)
    {
        //刷新售票口的UI显示
        this.InitData();

        if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide == true && entryTicketsLevel >= 5)
        {   /*新手阶段   隐藏停车场Ui  显示新手引导UI    步骤应该是  14    */
            DestroyEffectChild();
            InitNewGuideEveryCompent();
            this.Hide();
            PageMgr.ShowPage<UIGuidePage>();
        }
        if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide == true)
        {
            PromptText.CreatePromptText_TestUI(string.Format(GetL10NString("Ui_Text_136"), entryTicketsLevel, 5));
        }
        isGetCoin = true;

    }
    /// <summary>
    /// 监听某个售票口的等级变换
    /// </summary>
    /// <param name="obj"></param>
    protected void OnGetBroadcastEntryGatePureLevelOfPlayerData(Message obj)
    {
        var _msg = obj as SetDetailValueOfPlayerData;
        int idx = 0;
        for (int i = 0; i < playerData.GetEntryDateDataIDIndexOfDataIdx().entryGateList.Count; i++)
        {
            if (playerData.GetEntryDateDataIDIndexOfDataIdx().entryGateList[i].entryID == _msg.detailVal)
            {
                idx = i;
            }
        }
        InitAloneEntryCompent(idx);
        isGetCoin = true;
    }
    /// <summary>
    /// 监听售票口开启成功的消息
    /// </summary>
    /// <param name="obj"></param>
    protected void OnGetBroadcastEntryGateNumOfPlayerData(Message obj)
    {
        var _msg = obj as SetValueOfPlayerData;
        this.InitData();
        int subscript = EntryGateModule.GetPresentSceneTicketCell(_msg.channelID);

        InitOpenNewEntryCompent(subscript);
        isGetCoin = true;
    }

    private void OnOpenOfflineUIPage(Message obj)
    {
        HideButtonUI("");
    }

    #endregion

    /// <summary>
    /// 更新
    /// </summary>
    public override void Refresh()
    {
        base.Refresh();
    }
    /// <summary>
    /// 活跃
    /// 添加按钮事件
    /// </summary>
    public override void Active()
    {
        base.Active();
        RegistInitEveryCompent();
        SetCorrectShowImage();
        //初始化属性数值
        this.InitData();
        //注册监听消息     
        MessageManager.GetInstance().Regist((int)GameMessageDefine.BroadcastCoinOfPlayerDataMSSC, this.OnGetBroadcastCoinOfPlayerDataMSSC);//接受金钱变动的信息
        MessageManager.GetInstance().Regist((int)GameMessageDefine.BroadcastEntryGateLevelOfPlayerData, this.OnGetBroadcastEntryGateLevelOfPlayerData);
        MessageManager.GetInstance().Regist((int)GameMessageDefine.BroadcastEntryGatePureLevelOfPlayerData, OnGetBroadcastEntryGatePureLevelOfPlayerData);
        MessageManager.GetInstance().Regist((int)GameMessageDefine.BroadcastEntryGateNumOfPlayerData, this.OnGetBroadcastEntryGateNumOfPlayerData);
        MessageManager.GetInstance().Regist((int)GameMessageDefine.UIMessage_OpenOfflinePage, OnOpenOfflineUIPage);

    }
    /// <summary>
    /// 隐藏
    /// </summary>
    public override void Hide()
    {
        base.Hide();
        OnReleaseButton();
        MessageManager.GetInstance().UnRegist((int)GameMessageDefine.BroadcastEntryGateLevelOfPlayerData, this.OnGetBroadcastEntryGateLevelOfPlayerData);
        MessageManager.GetInstance().UnRegist((int)GameMessageDefine.BroadcastCoinOfPlayerDataMSSC, this.OnGetBroadcastCoinOfPlayerDataMSSC);//接受金钱变动的信息
        MessageManager.GetInstance().UnRegist((int)GameMessageDefine.BroadcastEntryGatePureLevelOfPlayerData, OnGetBroadcastEntryGatePureLevelOfPlayerData);
        MessageManager.GetInstance().UnRegist((int)GameMessageDefine.BroadcastEntryGateNumOfPlayerData, this.OnGetBroadcastEntryGateNumOfPlayerData);
        MessageManager.GetInstance().UnRegist((int)GameMessageDefine.UIMessage_OpenOfflinePage, OnOpenOfflineUIPage);

        MessageString.Send((int)GameMessageDefine.UIMessage_ActiveButShowPart, "UIMainPage");
        DestroyEffectChild();
        //LogWarp.LogError(" 售票口  ");

    }
    private void HideButtonUI(string str)
    {
        UIInteractive.GetInstance().SetClosePage(this);
    }
    /// <summary>
    /// 获取等级段对应的奖励信息
    /// </summary>
    /// <returns></returns>
    private Config.itemCell GradeSliderAwardImage()
    {
        var lvreward = ticketCell.lvreward;
        int idx = PlayerDataModule.FindLevelRangIndex(ticketCell.lvshage, entryTicketsLevel);
        int itemID = lvreward[idx];

        Config.itemCell itemCell = Config.itemConfig.getInstace().getCell(itemID);
        return itemCell;
    }
    /// <summary>
    /// 清除节点下的特效
    /// </summary>
    private void DestroyEffectChild()
    {
        /*  清除场景特效  */
        if (effectNode != null)
        {
            for (int i = 0; i < effectNode.childCount; i++)
            {
                GameObject.Destroy(effectNode.GetChild(i).gameObject);
            }
        }
    }



}


