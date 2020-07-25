using Game;
using Game.GlobalData;
using UFrame.MiniGame;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 新场景解锁界面
/// </summary>
public class UISceneUnlockPage : UIPage
{
    private Text tipsText;
    private Text multipleText;
    private Text sceneNameText;
    private Button confirmButton;
    private Image goldIcon;
    private string multipleFmtStr = "X{0}";

    public UISceneUnlockPage() : base(UIType.PopUp, UIMode.DoNothing, UICollider.None, UITickedMode.Update)
    {
        uiPath = "uiprefab/UISceneOpen";
    }

    public override void Awake(GameObject go)
    {
        base.Awake(go);

        GetTransPrefabAllTextShow(this.transform);

        tipsText = RegistCompent<Text>("ReceiveBg/SortCanvas/TipsText");
        multipleText = RegistCompent<Text>("ReceiveBg/SortCanvas/NumText");
        sceneNameText = RegistCompent<Text>("ReceiveBg/SortCanvas/NameText");
        confirmButton = RegistBtnAndClick("ReceiveBg/SortCanvas/CloseButton", OnConfirmButtonClick);
        goldIcon = RegistCompent<Image>("ReceiveBg/SortCanvas/GoldIcon");
    }

    public override void Active()
    {
        base.Active();

        int sceneId = (int)this.m_data;
        Config.sceneCell sceneCell = sceneConfig.getCell(sceneId);
        multipleText.text = string.Format(multipleFmtStr, sceneCell.doublenum);
        sceneNameText.text = string.Format(GetL10NString(sceneCell.scenename), sceneCell.sceneorder);
        int scenetype = Config.sceneConfig.getInstace().getCell(sceneId).moneyid;
        string iconPath = Config.moneyConfig.getInstace().getCell(scenetype).bigmoneyicon;
        goldIcon.sprite = ResourceManager.LoadSpriteFromPrefab(iconPath);
    }

    private WorldMapModel mapModel { get { return WorldMapModel.GetInstance(); } }

    private Config.sceneConfig sceneConfig { get { return Config.sceneConfig.getInstace(); } }

    private void OnConfirmButtonClick(string str)
    {
        mapModel.BrowseScene(int.Parse(m_data.ToString()));
        var mapPage = PageMgr.GetPage<UIMapPage>();
        mapPage.UpdatePieceStatesDisplay();
        var mainPage = PageMgr.GetPage<UIMainPage>();
        if (mainPage != null)
            mainPage.UpdateDisplayForSceneStates();
        PageMgr.ClosePage(this);
    }

    public override void Tick(int deltaTimeMS)
    {
        SetAndroidQuit();
    }

    [System.Diagnostics.Conditional("UNITY_ANDROID")]
    protected void SetAndroidQuit()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape))
#else
        if (Application.platform == RuntimePlatform.Android && Input.GetKeyDown(KeyCode.Escape)) 
#endif
        {
            PageMgr.ClosePage(this);
        }
    }
}
