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
using UnityEngine.UI;

public class UIBuildOpenPage : UIPage
{

    int littleZooID;//需要开启的动物栏ID
    int needStar;//需要金钱
    int playerStar;//

    #region UI控件字段
    Button advertPalyButton;   //开启按钮
    Text needNumText;           //需要的金钱
    Button closeButton;     //关闭页面按钮

    Text titleText;
    Text wordText;
    Text openButtonText;
    Text rewardText;
    Image touristIcon;
    Image rewardIcon;
    #endregion
    public UIBuildOpenPage() : base(UIType.Fixed, UIMode.DoNothing, UICollider.None)
    {
        uiPath = "uiprefab/UIBuildOpen";
    }
    public override void Awake(GameObject go)
    {
        base.Awake(go);
        //初始化数据
        //初始化控件
        this.RegistAllCompent();
        GetTransPrefabAllTextShow(this.transform);

        this.InitData();
    }
    private void InitData()
    {
        littleZooID = int.Parse( m_data.ToString());
        playerStar = GlobalDataManager.GetInstance().playerData.playerZoo.star;
        needStar = LittleZooModule.AddNewlittleZooCoin(littleZooID);

        //LogWarp.Log("UIBuildOpenPage 测试：需要开启的 当前动物栏ID"+littleZooID+"  需要花费多少钱："+needCoin+"  现有财产"+coinval);
        this.InitCompent();
    }
    private void RegistAllCompent()
    {
        titleText = RegistCompent<Text>("UpButtonGroup/TitleText");
        wordText = RegistCompent<Text>("UpButtonGroup/WordText");
        openButtonText = RegistCompent<Text>("UpButtonGroup/AdvertPalyButton/OpenButtonText");
        rewardText = RegistCompent<Text>("UpButtonGroup/GameObject/RewardText");
        touristIcon = RegistCompent<Image>("UpButtonGroup/TouristIcon");
        //GetTransPrefabText(titleText);
        //GetTransPrefabText(wordText);
        //GetTransPrefabText(openButtonText);

        needNumText = RegistCompent<Text>("UpButtonGroup/AdvertPalyButton/NeedNumText");
        closeButton = AddCompentInChildren(closeButton, "UpButtonGroup/CloseButton");
        closeButton = RegistBtnAndClick("UpButtonGroup/CloseButton", IsHide);
        advertPalyButton = AddCompentInChildren(advertPalyButton, "UpButtonGroup/AdvertPalyButton");
        advertPalyButton = RegistBtnAndClick("UpButtonGroup/AdvertPalyButton", OnClickAddZool);

        rewardIcon = RegistCompent<Image>("UpButtonGroup/GameObject/RewardIcon");
    }
    /// <summary>
    /// 控件显示赋值
    /// </summary>
    private void InitCompent()
    {
        needNumText.text =needStar.ToString();
        if (needStar >playerStar)
        {
            SwitchButtonUnClickable(advertPalyButton, false);
        }
        else
        {
            SwitchButtonUnClickable(advertPalyButton, true);
        }
        //LogWarp.LogErrorFormat("测试： 动物栏={0},收益={1}　　",littleZooID,LittleZooModule.GetlittleZooShowExpenditure(littleZooID, 1));
        rewardText.text = MinerBigInt.ToDisplay(LittleZooModule.GetlittleZooShowExpenditure(littleZooID,1))+ GetL10NString("Ui_Text_67");
        string path = Config.buildupConfig.getInstace().getCell(littleZooID).icon;
        touristIcon.sprite = ResourceManager.LoadSpriteFromPrefab(path);
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
    /// 添加按钮事件
    /// </summary>
    public override void Active()
    {
        base.Active();
        this.InitData();
        MessageManager.GetInstance().UnRegist((int)GameMessageDefine.BroadcastExitGateLevelOfPlayerData, this.OnGetPlayerData);
        int scenetype = Config.sceneConfig.getInstace().getCell(GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID).moneyid;
        string iconPath = Config.moneyConfig.getInstace().getCell(scenetype).moneyicon;
        rewardIcon.sprite = ResourceManager.LoadSpriteFromPrefab(iconPath);
    }

    /// <summary>
    /// 隐藏
    /// </summary>
    public override void Hide()
    {
        base.Hide();
    }

    private void OnGetPlayerData(Message obj)
    {
        IsHide("");
    }

    void IsHide(string name )
    {
        this.Hide();
        MessageManager.GetInstance().UnRegist((int)GameMessageDefine.BroadcastExitGateLevelOfPlayerData, this.OnGetPlayerData);
        MessageString.Send((int)GameMessageDefine.UIMessage_ActiveButShowPart, "UIMainPage");

    }
    /// <summary>
    /// 开启新动物栏事件
    /// </summary>
    /// <param name="goname"></param>
    private void OnClickAddZool(string goname)
    {
        if (needStar <= playerStar)
        {
            //发送消息开启动物栏
            int level = GlobalDataManager.GetInstance().playerData.GetLittleZooModuleData(littleZooID).littleZooTicketsLevel;
            if (level<1)
            {
                OpenNewLittleZoo.Send(littleZooID);
                string btnSoundPath = Config.globalConfig.getInstace().BuildOpenMusic;
                UFrame.MiniGame.SoundManager.GetInstance().PlaySound(btnSoundPath);
                this.IsHide("");
            }
            else
            {
                LogWarp.Log("UIBuildOpenPage测试：  新开启的动物栏ID" + littleZooID+"，动物栏等级"+level+"，不需要开启");
            }
        }
        else
        {
            LogWarp.Log("钱不够，新动物栏无法开启");
        }
            
    }
}
