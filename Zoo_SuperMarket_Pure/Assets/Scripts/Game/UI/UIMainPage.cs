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

/// <summary>
/// 主界面
/// </summary>
public partial class UIMainPage : UIPage
{
    /// <summary>
    /// 小游戏的跳转按钮
    /// </summary>
    private Button littleGameButton;
    /// <summary>
    /// /m  收益
    /// </summary>
    private Text earningsText;
    /// <summary>
    /// 金币总数量
    /// </summary>
    private Text goldText;
    /// <summary>
    /// 星星总数量
    /// </summary>
    private Text starText;
    /// <summary>
    /// 钻石总数量
    /// </summary>
    private Text diamondText;
    /// <summary>
    /// 增益buff时间
    /// </summary>
    private Text advertButton_Time;
    /// <summary>
    /// 小游戏体力文本
    /// </summary>
    private Text strengthText;
    /// <summary>
    /// 小游戏体力恢复时间
    /// </summary>
    private Text strength_Time;




    private Image money_1_GoldIcon;
    private Image strengthenButton_GoldIcon;
    private Image buttonGold;

    private Text strengthenButton_Text;

    PlayerData playerData { get { return GlobalDataManager.GetInstance().playerData; } }                       //获取Data，方便获取动物园
    Text txtVisistorNum;
    Text txtMaxVisistorNum;
    Text txtShuttleVisistorNum;
    Button AdvertButton;
    Button touristButton; //观看广告 增加轮船游客
    Button visitButton;     //观看广告  加快动物栏观光速度
    Button ticketButton; //观看广告 加快大门乘车速度

    Button animalCultureOpenHintButton;
    Text animalCultureOpenHintButton_Text;

    BigInteger incomeCoinMS;

    MultiIntCD multiTickObj;

    Transform celerityVisit;
    Transform celerityTicket;

    private Button mapButton;
    private Transform unlockSceneTips;

    // 引导任务UI
    private Transform guideTaskRoot;
    private Transform guideTaskContainer;
    private Text guideTaskNameText;
    private Image guideTaskRewardIcon;
    private Text guideTaskRewardQuantityText;
    private Text guideTaskProgressText;
    private Button guideTaskActionButton;
    private Text guideTaskActionButtonText;
    private Button guideTaskOpenButton;
    private Image guideTaskOpenButtonIcon;
    private bool guideTaskOpened = true;
    private UnityEngine.Vector3 guideTaskOrigPos = UnityEngine.Vector3.zero;
    private float guideTaskOpenedSize = 310;
    private string guideTaskProgressFmt = "({0}/{1})";
    private string guideTaskActionTxtKey = "Ui_Text_86";
    private string guideTaskActionCmplTxtKey = "Ui_Text_83";
    private string guideTaskActionUnopenTxtKey = "Ui_Text_87";
    private Animator guideTaskHintAnimator;
    private Transform guideTaskCmplFx;
    private string guideHandPrefabPath = "prefabs/Effect/Fx_Ui_Hand";
    private Transform guideTaskHand;
    private bool _lastCloseWithManual = false;
    public bool lastCloseWithManual { get { return _lastCloseWithManual; } }
    private Transform setMainShow;
    public int refreshUIShowData = 1000;
    public UIMainPage() : base(UIType.Fixed, UIMode.DoNothing, UICollider.None, UITickedMode.Update)
    {
        uiPath = "uiprefab/UIMain";
    }
    public override void Awake(GameObject go)
    {
        base.Awake(go);

        GetTransPrefabAllTextShow(this.transform);
        
        this.RegistAllCompent( go);

        InitForGuideTask();
        UpdateDisplayForSceneStates();
        PageMgr.AwakePage<UIAnimalAtlasPage>();
#if NO_BIGINT
#else
        Init();
#endif

        //MessageManager.GetInstance().Regist((int)GameMessageDefine.BroadcastCoinOfPlayerData, this.OnBroadcastCoinOfPlayerData);
        MessageManager.GetInstance().Regist((int)GameMessageDefine.BroadcastCoinOfPlayerDataMSSC, this.OnGetBroadcastCoinOfPlayerDataMSSC);
        MessageManager.GetInstance().Regist((int)GameMessageDefine.BroadcastStarOfPlayerData, this.OnBroadcastStarOfPlayerData);
        MessageManager.GetInstance().Regist((int)GameMessageDefine.BroadcastDiamondOfPlayerData, this.OnBroadcastDiamondOfPlayerData);
        MessageManager.GetInstance().Regist((int)GameMessageDefine.AnimalBuffAlterSucceed, this.OnAnimalBuffAlterSucceed);

        MessageManager.GetInstance().Regist((int)GameMessageDefine.AddBuffSucceed, this.OnAddBuffSucceed);

        MessageManager.GetInstance().Regist((int)GameMessageDefine.UIMessage_ActiveButHidePart, OnUIMessage_ActiveButHidePart);
        MessageManager.GetInstance().Regist((int)GameMessageDefine.UIMessage_ActiveButShowPart, OnUIMessage_ActiveButShowPart);
        MessageManager.GetInstance().Regist((int)GameMessageDefine.UIMessage_OpenOfflinePage, OnOpenOfflineUIPage);

        MessageManager.GetInstance().Regist((int)GameMessageDefine.LoadZooSceneFinished, OnLoadZooSceneFinished);
        MessageManager.GetInstance().Regist((int)GameMessageDefine.BroadcastChangedStrength, OnBroadcastChangedStrength);


        multiTickObj = new MultiIntCD();
        multiTickObj.Run();
        multiTickObj.AddCD(refreshUIShowData, RefreshUIShowData);

        //初始化图鉴的Awake
        //PageMgr.AwakePage<UIAnimalAtlasPage>();
        
    }
    /// <summary>
    /// 监听体力修改了
    /// </summary>
    /// <param name="obj"></param>
    private void OnBroadcastChangedStrength(Message obj)
    {
        if (playerData.playerLittleGame.strength < Config.globalConfig.getInstace().MaxStrength)
        {
            strength_Time.gameObject.SetActive(true);
        }
        else
        {
            strength_Time.gameObject.SetActive(false);
        }
        strengthText.text = playerData.playerLittleGame.strength.ToString();

    }

    private void OnAnimalBuffAlterSucceed(Message obj)
    {
        goldText.text = playerData.playerZoo.playerCoin.GetCoinByScene(playerData.playerZoo.currSceneID).coinShow;
        strengthenButton_Text.text = "X" + LittleZooModule.GetAllAnimalsBuff().ToString();

    }

    private void OnLoadZooSceneFinished(Message obj)
    {
        //显示正常的Image
        Init();

    }

    private void SetCorrectShowImage()
    {
        int scenetype = Config.sceneConfig.getInstace().getCell(playerData.playerZoo.currSceneID).moneyid;
        string iconPath = Config.moneyConfig.getInstace().getCell(scenetype).moneyicon;
        money_1_GoldIcon.sprite = ResourceManager.LoadSpriteFromPrefab(iconPath);
        strengthenButton_GoldIcon.sprite = ResourceManager.LoadSpriteFromPrefab(iconPath);
        guideTaskRewardIcon.sprite = ResourceManager.LoadSpriteFromPrefab(iconPath);
        buttonGold.sprite = ResourceManager.LoadSpriteFromPrefab(iconPath);


    }

    private void RegistAllCompent(GameObject go)
    {
        setMainShow = RegistCompent<Transform>("SetMainShow");

        earningsText = RegistCompent<Text>("MoneyGroup/Money_1/MText");
        goldText = RegistCompent<Text>("MoneyGroup/Money_1/Text");
        starText = RegistCompent<Text>("MoneyGroup/Money_2/Text");
        diamondText = RegistCompent<Text>("MoneyGroup/Money_3/Text");


        celerityVisit = RegistCompent<Transform>("SetMainShow/LowerButtonGroup/BuffTimeTips_1");
        celerityTicket = RegistCompent<Transform>("SetMainShow/LowerButtonGroup/BuffTimeTips_2");
        celerityVisit.gameObject.SetActive(false);
        celerityTicket.gameObject.SetActive(false);

        AdvertButton = AddCompentInChildren<Button>(AdvertButton, "SetMainShow/LowerButtonGroup/AdvertButton");
        touristButton = AddCompentInChildren<Button>(touristButton, "SetMainShow/TipsButtonGroup/TouristButton");
        visitButton = AddCompentInChildren<Button>(visitButton, "SetMainShow/TipsButtonGroup/VisitButton");
        ticketButton = AddCompentInChildren<Button>(ticketButton, "SetMainShow/TipsButtonGroup/TicketButton");

        AdvertButton = RegistBtnAndClick("SetMainShow/LowerButtonGroup/AdvertButton", OnClickAdvertButton);
        touristButton = RegistBtnAndClick("SetMainShow/TipsButtonGroup/TouristButton", OnClickAdsButton_TouristButton);
        visitButton = RegistBtnAndClick("SetMainShow/TipsButtonGroup/VisitButton", OnClickAdsButton_VisitButton);
        ticketButton = RegistBtnAndClick("SetMainShow/TipsButtonGroup/TicketButton", OnClickAdsButton_TicketButton);
        animalCultureOpenHintButton = RegistBtnAndClick("SetMainShow/LowerButtonGroup/StrengthenButton", OnClickAdsButton_StrengthenButton);
        animalCultureOpenHintButton_Text = RegistCompent<Text>("SetMainShow/LowerButtonGroup/StrengthenButton/TipsNum");
        animalCultureOpenHintButton_Text.text = Config.globalConfig.getInstace().AnimalupgradingNeed.ToString();
        touristButton.gameObject.SetActive(false);
        visitButton.gameObject.SetActive(false);
        ticketButton.gameObject.SetActive(false);

        txtVisistorNum = RegistCompent<Text>("SetMainShow/Debug/Txt_VisitorNum");
        txtMaxVisistorNum = RegistCompent<Text>("SetMainShow/Debug/Txt_MaxVisitorNum");
        txtShuttleVisistorNum = RegistCompent<Text>("SetMainShow/Debug/Txt_ShuttleVisistorNum");
#if !DEBUG_VISIT
        txtVisistorNum.gameObject.SetActive(false);
        txtMaxVisistorNum.gameObject.SetActive(false);
        txtShuttleVisistorNum.gameObject.SetActive(false);
#endif

        Awake_TestPart(go);

        AdvertButton = RegistBtnAndClick("SetMainShow/LowerButtonGroup/CollectButton", OnClickCollectButton);
        mapButton = RegistBtnAndClick("SetMainShow/LowerButtonGroup/MapButton", OnMapButtonClick);
        unlockSceneTips = RegistCompent<Transform>("SetMainShow/LowerButtonGroup/TipsBubble");

        // 引导任务
        guideTaskRoot = RegistCompent<Transform>("SetMainShow/LowerButtonGroup/MissionWin");
        guideTaskContainer = RegistCompent<Transform>("SetMainShow/LowerButtonGroup/MissionWin/Mask/Container");
        guideTaskNameText = RegistCompent<Text>("SetMainShow/LowerButtonGroup/MissionWin/Mask/Container/MissionText");
        guideTaskRewardIcon = RegistCompent<Image>("SetMainShow/LowerButtonGroup/MissionWin/Mask/Container/GoldIcon");
        guideTaskRewardQuantityText = RegistCompent<Text>("SetMainShow/LowerButtonGroup/MissionWin/Mask/Container/GoldNum");
        guideTaskProgressText = RegistCompent<Text>("SetMainShow/LowerButtonGroup/MissionWin/Mask/Container/MissionNum");
        guideTaskActionButtonText = RegistCompent<Text>("SetMainShow/LowerButtonGroup/MissionWin/Mask/Container/GoToButton/Text");
        guideTaskActionButton = RegistBtnAndClick("SetMainShow/LowerButtonGroup/MissionWin/Mask/Container/GoToButton", OnGuideTaskActionButtonClick);
        guideTaskOpenButton = RegistBtnAndClick("SetMainShow/LowerButtonGroup/MissionWin/Mask/Container/OpenButton", OnGuideTaskOpenButtonClick);
        RegistBtnAndClick("SetMainShow/LowerButtonGroup/MissionWin/Mask/Container/OpenButton/ButtonIcon", OnGuideTaskOpenButtonClick);
        guideTaskOpenButtonIcon = RegistCompent<Image>("SetMainShow/LowerButtonGroup/MissionWin/Mask/Container/OpenButton/ButtonIcon");
        guideTaskHintAnimator = RegistCompent<Animator>("SetMainShow/LowerButtonGroup/MissionWin/Mask");
        guideTaskCmplFx = RegistCompent<Transform>("SetMainShow/LowerButtonGroup/MissionWin/Mask/Container/Fx_Ui_Mission");

        money_1_GoldIcon = RegistCompent<Image>("MoneyGroup/Money_1/GoldIcon");
        strengthenButton_GoldIcon = RegistCompent<Image>("SetMainShow/UpButtonGroup/StrengthenButton/GoldIcon");
        buttonGold = RegistCompent<Image>("SetMainShow/LowerButtonGroup/AdvertButton/ButtonGold");

        strengthenButton_Text = RegistCompent<Text>("SetMainShow/UpButtonGroup/StrengthenButton/Text");

        RegistBtnAndClick("SetMainShow/UpButtonGroup/GameButton", OnGameButtonClick);

        advertButton_Time = RegistCompent<Text>("SetMainShow/LowerButtonGroup/AdvertButton/Time");

        strengthText = RegistCompent<Text>("SetMainShow/UpButtonGroup/GameButton/NumText");
        strength_Time = RegistCompent<Text>("SetMainShow/UpButtonGroup/GameButton/Time");
    }

    private void OnGameButtonClick(string obj)
    {
        if (playerData.playerLittleGame.strength > 0)
        {
            float timeCount = 0.1f;
            DOTween.To(() => timeCount, a => timeCount = a, 0.1f, 0.1f).OnComplete(new TweenCallback(delegate
            {
                playerData.playerLittleGame.increaseStrengthTicks = DateTime.Now.Ticks;

                ZooGameLoader.GetInstance().UnLoad();
                int testStageID = CrossRoadGame.CrossRoadStageManager.GetInstance().TeststageID;
                if (testStageID != Const.Invalid_Int)
                {
                    CrossRoadGame.CrossRoadStageManager.GetInstance().Load(testStageID);
                }
                else
                {
                    CrossRoadGame.CrossRoadStageManager.GetInstance().Load(playerData.playerLittleGame.stageID + 1);
                }
            }));
        }
        else
        {
            PromptText.CreatePromptText("Ui_Text_133");
        }
        
    }

    void Init()
    {
        //playerData = GlobalDataManager.GetInstance().playerData;
        incomeCoinMS = PlayerDataModule.CurrScenePerMinCoin(true);
        goldText.text = playerData.playerZoo.playerCoin.GetCoinByScene(playerData.playerZoo.currSceneID).coinShow;
        starText.text = playerData.playerZoo.star.ToString();
        earningsText.text = MinerBigInt.ToDisplay(incomeCoinMS) + GetL10NString("Ui_Text_67");
        diamondText.text = playerData.playerZoo.diamond.ToString();
        strengthenButton_Text.text = "X"+LittleZooModule.GetAllAnimalsBuff().ToString();
        //显示正常的Image
        SetCorrectShowImage();

        strengthText.text = playerData.playerLittleGame.strength.ToString();
        if (playerData.playerLittleGame.strength>= Config.globalConfig.getInstace().MaxStrength)
        {
            strength_Time.gameObject.SetActive(false);
        }
        
    }
    public override void Active()
    {
        base.Active();
        //playerData = GlobalDataManager.GetInstance().playerData;
        //如果老用户已经开启了动物培养了，直接开启动物培养功能
        if (GlobalDataManager.GetInstance().playerData.playerZoo.isShowAnimalCultivate == true)
        {
            SetHideAnimalCultureOpenHintButton();
        }

        if (playerData.playerZoo.isGuide == true)
        {
#if NOVICEGUIDE
            PageMgr.ShowPage<UIGuidePage>();  //新手引导页面交互
            LogWarp.Log("NOVICEGUIDE  时候打开 新手引导");
            Game.Event_SDK.AppsFlyerEvent.GetInstance().SendEvent(AppsFlyerEnum.first_enter_game);
#endif
        }
        else
        {
            if (playerData.playerZoo.LastLogingDate_Day != System.DateTime.Now.Day)
            {
                playerData.playerZoo.playerNumberOfVideosWatched.SetResetVideosWatchedData();
            }
            ButtonShowCountDown();
        }
        if (GlobalDataManager.GetInstance().playerData.playerZoo.isLoadingShowOffline)
        {
            OnOpenOfflineUIPage(null);
            GlobalDataManager.GetInstance().playerData.playerZoo.isLoadingShowOffline = false;
        }

        //判断当前星星是否可以开启动物培养
        ShowAnimalCultivate();


    }
    /// <summary>
    /// 按钮显示的倒计时
    /// </summary>
    public void ButtonShowCountDown()
    {
        int IncreaseTouristTime = Math_F.FloatToInt1000(Config.globalConfig.getInstace().IncreaseTouristTime);
        multiTickObj.AddCD(IncreaseTouristTime, Begin_AdsButtonShow_Tourist);

        int VisitAccelerateTime = Math_F.FloatToInt1000(Config.globalConfig.getInstace().VisitAccelerateTime);
        multiTickObj.AddCD(VisitAccelerateTime, Begin_AdsButtonShow_Visit);

        int TicketAccelerateTime = Math_F.FloatToInt1000(Config.globalConfig.getInstace().TicketAccelerateTime);
        multiTickObj.AddCD(TicketAccelerateTime, Begin_AdsButtonShow_Ticket);

    }

    public void OnMoneyEffect()
    {
        var effTarget = goldText.transform;
        List<object> moneyEffectData = new List<object>();
        moneyEffectData.Clear();
        moneyEffectData.Add((UnityEngine.Vector2)effTarget.position);
        moneyEffectData.Add(MoneyType.Money);
        PageMgr.ShowPage<UIMoneyEffect>(moneyEffectData);
    }

    #region --- 场景状态 ----

    private WorldMapModel mapModel { get { return WorldMapModel.GetInstance(); } }

    /// <summary>
    /// 世界地图按钮点击
    /// </summary>
    /// <param name="obj"></param>
    private void OnMapButtonClick(string obj)
    {
        PageMgr.ShowPage<UIMapPage>();
    }

    private IEnumerator FinishUpdateDisplayForSceneStates()
    {
        yield return null;
        unlockSceneTips.gameObject.SetActive(mapModel.GetUnbrowsedSceneCount() > 0);
    }

    /// <summary>
    /// 更新场景状态相关显示
    /// </summary>
    public void UpdateDisplayForSceneStates(bool nextFrame = true)
    {
        if (nextFrame)
            GameManager.GetInstance().StartCoroutine(FinishUpdateDisplayForSceneStates());
        else
            unlockSceneTips.gameObject.SetActive(mapModel.GetUnbrowsedSceneCount() > 0);
    }

    #endregion

    #region ---- guide task ----

    private GuideMissionModel guideMissionModel { get { return GuideMissionModel.GetInstance(); } }

    private void InitForGuideTask()
    {
        guideTaskOrigPos = guideTaskContainer.localPosition;
        UpdateGuideTaskDisplay();
        MessageManager.GetInstance().Regist((int)GameMessageDefine.GuideMissionStateChanged, OnGuideMissionStateChanged);
    }

    private void OnGuideMissionStateChanged(Message message)
    {
        GuideMissionStateChanged missionMessage = message as GuideMissionStateChanged;
        if (missionMessage.detail == GuideMissionStateChanged.ChangeDetail_NewTask)
            UpdateGuideTaskDisplay();
        else if (missionMessage.detail == GuideMissionStateChanged.ChangeDetail_TaskProgress)
            UpdateGuideTaskProgressDisplay();
    }

    private string ComposeTaskName(missionCell missionCell)
    {
        string fmt = GetL10NString(missionCell.nametranslate);
        if (fmt.Contains("{1}") && fmt.Contains("{0}"))
        {
            TaskType taskType = missionCell.ParseTaskType();
            string p1 = string.Empty;
            if (taskType == TaskType.LittleZooLevelup || taskType == TaskType.LittleZooVisit)
            {
                Config.buildupCell buildupCell = Config.buildupConfig.getInstace().getCell(missionCell.GetLittleZooId());
                if (buildupCell == null)
                    throw new Exception(string.Format("无法找到动物栏定义，Id={0}", missionCell.GetLittleZooId()));
                p1 = GetL10NString(buildupCell.buildname);
            }
            else if (taskType == TaskType.EntryGateLevelup)
            {
                p1 = (missionCell.GetEntryGateId() + 1).ToString();
            }
            
                
            return string.Format(fmt, p1, missionCell.need);
        }
        else if (fmt.Contains("{0}"))
        {
            TaskType taskType = missionCell.ParseTaskType();
            if (taskType == TaskType.OpenNewLittleZoo)
            {
                Config.buildupCell buildupCell = Config.buildupConfig.getInstace().getCell(missionCell.taskparam1);

                return string.Format(fmt, GetL10NString(buildupCell.buildname));
            }

            return string.Format(fmt, missionCell.need);
        }
        return fmt;
    }

    private void SetGuideTaskPanelActive(bool value)
    {
        Config.missionCell missionCell = guideMissionModel.GetTaskCell(guideMissionModel.currTaskId);
        guideTaskRoot.gameObject.SetActive(value && missionCell != null && !guideMissionModel.IsAllTasksCleared());
    }

    private void UpdateGuideTaskInfoDisplay()
    {
        Config.missionCell missionCell = guideMissionModel.GetTaskCell(guideMissionModel.currTaskId);
        if (missionCell != null)
        {
            _lastCloseWithManual = false;
            System.Numerics.BigInteger itemQuantity;
            ItemType itemType;
            guideMissionModel.GetTaskReward(guideMissionModel.currTaskId, out itemType, out itemQuantity);
            guideTaskRewardQuantityText.text = MinerBigInt.ToDisplay(itemQuantity.ToString());
            guideTaskNameText.text = ComposeTaskName(missionCell);

            if (!guideMissionModel.selfGuideComplete && guideTaskHand == null)
            {
                GameObject tmpl = Resources.Load<GameObject>(guideHandPrefabPath);
                if (tmpl != null)
                {
                    GameObject go = GameObject.Instantiate(tmpl);
                    guideTaskHand = go.transform;
                    go.transform.SetParent(guideTaskRoot.transform);
                    go.transform.localScale = UnityEngine.Vector3.one * 8;
                    go.transform.position = guideTaskActionButton.transform.position;
                    go.transform.localPosition = new UnityEngine.Vector3(
                        go.transform.localPosition.x,
                        go.transform.localPosition.y,
                        go.transform.localPosition.z);
                }
            }
        }
    }

    private void UpdateGuideTaskProgressDisplay()
    {
        Config.missionCell missionCell = guideMissionModel.GetTaskCell(guideMissionModel.currTaskId);
        if (missionCell == null)
            return;

        int progress, goal;
        bool completed = guideMissionModel.GetTaskProgress(guideMissionModel.currTaskId, out progress, out goal);
        guideTaskProgressText.text = string.Format(guideTaskProgressFmt, progress, goal);

        if (progress < 1 && missionCell.ParseTaskType() == TaskType.LittleZooLevelup) // 此动物栏还未开启
        {
            guideTaskActionButtonText.text = GetL10NString(guideTaskActionUnopenTxtKey);
            guideTaskActionButton.interactable = false;
        }
        else if (missionCell.ParseTaskType() == TaskType.OpenNewLittleZoo)
        {
            //获取动物栏等级，若大于0 ，则开启  否则关闭
            int level = playerData.GetLittleZooModuleData(int.Parse(missionCell.taskparam1)).littleZooTicketsLevel;
            completed = (level > 0);
            guideTaskActionButtonText.text = completed ? GetL10NString(guideTaskActionCmplTxtKey) : GetL10NString(guideTaskActionTxtKey);
            if (completed)
            {
                guideMissionModel.SetTaskProgress(guideMissionModel.currTaskId, 1);
            }

            guideTaskActionButton.interactable = true;
        }
        else
        {
            guideTaskActionButtonText.text = completed ? GetL10NString(guideTaskActionCmplTxtKey) : GetL10NString(guideTaskActionTxtKey);
            guideTaskActionButton.interactable = true;
        }
        if (!completed && missionCell.skip < 1)
            guideTaskActionButton.gameObject.SetActive(false);
        else
            guideTaskActionButton.gameObject.SetActive(true);

        if (completed)
        {
            guideTaskCmplFx.gameObject.SetActive(true);
            guideTaskHintAnimator.speed = 0;
            guideTaskHintAnimator.Play("Mission", 0, 0);
        }
        else
        {
            guideTaskCmplFx.gameObject.SetActive(false);
            //guideTaskHintAnimator.speed = 1;
            guideTaskHintAnimator.speed = 0;
            guideTaskHintAnimator.Play("Mission", 0, 0);
        }
    }

    private void UpdateGuideTaskDisplay()
    {
        SetGuideTaskPanelActive(true);
        UpdateGuideTaskOpenButtonIcon();
        UpdateGuideTaskInfoDisplay();
        UpdateGuideTaskProgressDisplay();
#if UNITY_EDITOR
        var a = MonoForDebug.inst;
#endif
    }

    private void UpdateGuideTaskOpenButtonIcon()
    {
        //guideTaskOpenButtonIcon.transform.localScale = guideTaskOpened ? new UnityEngine.Vector3(-1, 1, 1) : UnityEngine.Vector3.one;
        guideTaskOpenButtonIcon.transform.rotation = guideTaskOpened ? UnityEngine.Quaternion.Euler(0, 0, 180) : UnityEngine.Quaternion.identity;
    }

    private void OnGuideTaskActionButtonClick(string obj)
    {
        if (!guideMissionModel.selfGuideComplete)
        {
            if (guideTaskHand != null)
            {
                guideTaskHand.gameObject.SetActive(false);
                UnityEngine.Object.Destroy(guideTaskHand.gameObject);
            }
            guideMissionModel.selfGuideComplete = true;
        }
        GuideTaskActionButtonClick.Send();
    }

    private void OnGuideTaskOpenButtonClick(string obj)
    {
        if (guideTaskOpened)
            CloseGuideTaskPanel(true);
        else
            OpenGuideTaskPanel();
    }

    public void OpenGuideTaskPanel()
    {
        if (!guideTaskRoot.gameObject.activeSelf)
            return;

        _lastCloseWithManual = false;
        guideTaskContainer.DOLocalMoveX(guideTaskOrigPos.x, 0.2f).
            SetEase(Ease.InCirc).OnComplete(() =>
            {
                guideTaskOpened = true;
                UpdateGuideTaskOpenButtonIcon();
                if (guideTaskHand != null)
                    guideTaskHand.gameObject.SetActive(true);
            });
    }

    public void CloseGuideTaskPanel(bool withManual = false)
    {
        if (!guideTaskRoot.gameObject.activeSelf)
            return;

        _lastCloseWithManual = withManual;
        if (guideTaskHand != null)
            guideTaskHand.gameObject.SetActive(false);
        guideTaskContainer.DOLocalMoveX(guideTaskOrigPos.x - guideTaskOpenedSize, 0.2f).
            SetEase(Ease.InCirc).OnComplete(() =>
            {
                guideTaskOpened = false;
                UpdateGuideTaskOpenButtonIcon();
            });
    }

#endregion
}
