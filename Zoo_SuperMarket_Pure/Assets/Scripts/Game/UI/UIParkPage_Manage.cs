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
    /// <summary>
    /// 点击利润按钮事件
    /// </summary>
    public void OnClickUpGrade_ProfitCoins(string str)
    {
        //isLongPress为true则是长按状态，单点关闭  返回
        if (!JudgePressButton_Profit() && isLongPress)
        {
            LogWarp.LogError("不能升级");
            return;
        }
        /*  新手引导  */
        if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide == true)
        {
            if (profitLevel >= 5)
            {
                return;
            }
        }
        SendSetParkingLevelMessageManager();
        isGetCoin = false;  //设置等待回复状态
        //upGradeButton.enabled = false; //设置按钮不能点击
        ///播放音乐
        string btnSoundPath = Config.globalConfig.getInstace().BuildUpButtonMusic;
        SoundManager.GetInstance().PlaySound(btnSoundPath);

        //LogWarp.LogError("测试             单点  按钮         ");
    }
    /// <summary>
    /// 点击停车场数量按钮事件
    /// </summary>
    public void OnClickUpGrade_ParkingSpace(string str)
    {
        if (!JudgePressButton_ParkingSpace() && isLongPress)
        {
            LogWarp.LogError("不能升级");
            return;
        }
        SendSetParkingSpaceLevelMessageManager();
        isGetCoin = false;  //设置等待回复状态
        ///播放音乐
        string btnSoundPath = Config.globalConfig.getInstace().BuildUpButtonMusic;
        SoundManager.GetInstance().PlaySound(btnSoundPath);
    }
    /// <summary>
    /// 点击流量按钮事件
    /// </summary>
    public void OnClickUpGrade_EnterCarSpawn(string str)
    {
        if (!JudgePressButton_EnterCarSpawn() && isLongPress)
        {
            LogWarp.LogError("不能升级");
            return;
        }
        SendSetParkingCoolingLevelMessageManager();
        isGetCoin = false;  //设置等待回复状态
        ///播放音乐
        string btnSoundPath = Config.globalConfig.getInstace().BuildUpButtonMusic;
        SoundManager.GetInstance().PlaySound(btnSoundPath);
    }

    private void SendSetParkingLevelMessageManager()
    {
        if (profitLevel >= parkingProfitMaxGrade)
        {
            SwitchButtonUnClickable(profitCoins_Button, false);
            return;
        }
        //发送消息       SetValueOfPlayerData  消息体   
        SetDetailValueOfPlayerData.Send((int)GameMessageDefine.SetParkingProfitLevelOfPlayerData,
        1, 0, 0);
    }
    private void SendSetParkingSpaceLevelMessageManager()
    {
        if (parkingSpaceLevel >= parkingSpaceMaxGrade)
        {
            //parkingSpace_Button.enabled = false;
            SwitchButtonUnClickable(parkingSpace_Button, false);
            //parkingSpace_Button_ButtonLvUpText.color = Color.red;
            return;
        }
        SetValueOfPlayerData.Send((int)GameMessageDefine.SetParkingSpaceLevelOfPlayerData,
        1, 0, 0);
    }
    private void SendSetParkingCoolingLevelMessageManager()
    {
        if (enterCarSpawnLevel >= parkingEnterCarSpawnMaxGrade)
        {
            SwitchButtonUnClickable(enterCarSpawn_Button, false);
            return;
        }

        SetValueOfPlayerData.Send((int)GameMessageDefine.SetParkingEnterCarSpawnLevelOfPlayerData,
        1, 0, 0);
    }

    /// <summary>
    /// 监听广播停车场等级
    /// </summary>
    /// <param name="msg"></param>
    protected void OnGetBroadcastParkingProfitLevelOfPlayerData(Message msg)
    {
        this.InitData();
        //UIButtonEffectSimplePlayer(profitCoins_EffectNode);
        isGetCoin = true;

    }

    /// <summary>
    /// 监听广播停车场停车位数量等级
    /// </summary>
    /// <param name="obj"></param>
    private void OnGetBroadcastParkingSpaceLevelOfPlayerData(Message obj)
    {
        this.InitData();

        if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide == true && parkingSpaceLevel == 2)
        {   /*新手阶段   隐藏停车场Ui  显示新手引导UI    步骤应该是  7    */
            DestroyEffectChild();
            this.Hide();
            PageMgr.ShowPage<UIGuidePage>();
        }
        if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide == true )
        {
            PromptText.CreatePromptText_TestUI(string.Format( GetL10NString("Ui_Text_134"), parkingSpaceLevel,2));
        }
        isGetCoin = true;

    }
    /// <summary>
    /// 监听广播停车场来客数量等级
    /// </summary>
    private void OnGetBroadcastParkingEnterCarSpawnLevelOfPlayerData(Message obj)
    {
        this.InitData();
        if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide == true && enterCarSpawnLevel >= 5)
        {   /*新手阶段   隐藏停车场Ui  显示新手引导UI    步骤应该是  9   */
            DestroyEffectChild();
            SwitchButtonUnClickable(parkingSpace_Button, true);
            SwitchButtonUnClickable(profitCoins_Button, true);
            SwitchButtonUnClickable(enterCarSpawn_Button, true);

            this.Hide();
            PageMgr.ShowPage<UIGuidePage>();
            //Logger.LogWarp.LogErrorFormat("AAAAAAAAAAAAAAAA");

        }
        if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide == true)
        {
            PromptText.CreatePromptText_TestUI(string.Format(GetL10NString("Ui_Text_135"), enterCarSpawnLevel, 5));
        }
        isGetCoin = true;

    }

    /// <summary>
    /// 监听玩家coin金钱发生改变，是否需要重新计算升级规模
    /// </summary>
    /// <param name="obj"></param>
    private void OnGetBroadcastCoinOfPlayerDataMSSC(Message obj)
    {   //旧计算金钱不够，则开始新的计算
        this.InitData();
    }

    private void OnOpenOfflineUIPage(Message obj)
    {
        HideButtonUI("");
    }
    /// <summary>
    /// 判断是否可以升级（钱够/等级不超过最大值）
    /// </summary>
    /// <returns></returns>
    private bool SetGradeBool_Profit()
    {
        InitCoin();
        if (consumeProfitCoins <= coinVal && profitLevel < parkingProfitMaxGrade)
        {
            SwitchButtonUnClickable(profitCoins_Button, true);

            return true;
        }
        else
        {
            SwitchButtonUnClickable(profitCoins_Button, false);

            return false;
        }
    }
    /// <summary>
    /// 判断是否可以升级（钱够/等级不超过最大值）
    /// </summary>
    /// <returns></returns>
    private bool SetGradeBool_ParkingSpace()
    {
        InitCoin();
        //LogWarp.LogErrorFormat("测试：{0}  {1}   {2}   {3}    ",consumeParkingSpaceCoins , coinVal ,parkingSpaceLevel ,Config.parkingConfig.getInstace().getCell(1).spacemaxlv);
        if (consumeParkingSpaceCoins <= coinVal && parkingSpaceLevel < parkingSpaceMaxGrade)
        {
            SwitchButtonUnClickable(parkingSpace_Button, true);

            return true;
        }
        else
        {
            SwitchButtonUnClickable(parkingSpace_Button, false);
            return false;
        }
    }
    /// <summary>
    /// 判断是否可以升级（钱够/等级不超过最大值）
    /// </summary>
    /// <returns></returns>
    private bool SetGradeBool_EnterCarSpawn()
    {
        InitCoin();
        if (consumeEnterCarSpawnCoins <= coinVal && enterCarSpawnLevel < parkingEnterCarSpawnMaxGrade)
        {
            SwitchButtonUnClickable(enterCarSpawn_Button, true);
            return true;
        }
        else
        {
            SwitchButtonUnClickable(enterCarSpawn_Button, false);
            return false;
        }
    }
    /// <summary>
    /// 是否可以升级
    /// </summary>
    /// <returns></returns>
    private bool JudgePressButton_Profit()
    {
        //第一个  是否扣钱成功   第二  判断是否可以升级
        if (isGetCoin && SetGradeBool_Profit())
        {
            SwitchButtonUnClickable(profitCoins_Button, true);
            return true;
        }
        SwitchButtonUnClickable(profitCoins_Button, false);
        return false;
    }
    private bool JudgePressButton_ParkingSpace()
    {
        //第一个  是否扣钱成功   第二  判断是否可以升级
        if (isGetCoin && SetGradeBool_ParkingSpace())
        {
            SwitchButtonUnClickable(parkingSpace_Button, true);
            return true;
        }
        SwitchButtonUnClickable(parkingSpace_Button, false);
        return false;
    }
    private bool JudgePressButton_EnterCarSpawn()
    {
        //第一个  是否扣钱成功   第二  判断是否可以升级

        if (isGetCoin && SetGradeBool_EnterCarSpawn())
        {
            SwitchButtonUnClickable(enterCarSpawn_Button, true);
            return true;
        }
        SwitchButtonUnClickable(enterCarSpawn_Button, false);

        return false;
    }
    /// <summary>
    /// 长按按钮回调事件
    /// </summary>
    protected void OnLongPressButton_ProfitCoins()
    {
       
        if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide)
        {
            return;
        }
        isLongPress = true;//进入长按状态
        if (JudgePressButton_Profit())
        {
            SendSetParkingLevelMessageManager();  //发送升级消息
            isGetCoin = false;
        }
    }

    /// <summary>
    /// 离开长按按钮回调事件
    /// </summary>
    private void OnReleaseButton()
    {
        //LogWarp.LogError("测试             离开  按钮         ");
        isLongPress = false;
        fakeTime = 0;
    }
    /// <summary>
    /// 长按按钮回调事件
    /// </summary>
    protected void OnLongPressButton_ParkingSpace()
    {
        
        if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide)
        {
            return;
        }
        isLongPress = true;//进入长按状态
        //LogWarp.LogError("测试             长按  按钮         "+fakeTime);
        if (JudgePressButton_ParkingSpace())
        {
            SendSetParkingSpaceLevelMessageManager();  //发送升级消息
            isGetCoin = false;
        }
    }

    /// <summary>
    /// 长按按钮回调事件
    /// </summary>
    protected void OnLongPressButton_EnterCarSpawn()
    {
       
        if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide)
        {
            return;
        }
        isLongPress = true;//进入长按状态
        //LogWarp.LogError("测试             长按  按钮         "+fakeTime);
        if (JudgePressButton_EnterCarSpawn())
        {
            SendSetParkingCoolingLevelMessageManager();  //发送升级消息
            isGetCoin = false;
        }
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
        if (uIGuidePage.newBieGuild_step == NewBieGuild.Step_5 && parkingSpaceLevel < 2)
        {
            //停车场停车位按钮处显示小手点击动画
            effectNode = parkingSpace_Button.transform.Find("effectNode");
            Transform trans = null;
            trans = ResourceManager.GetInstance().LoadGameObject(Config.globalConfig.getInstace().GuideUiClickEffect).transform;
            trans.SetParent(effectNode, true);
            trans.localScale = UnityEngine.Vector3.one ;
            trans.position = effectNode.position;
            trans.localPosition = new UnityEngine.Vector3(
                trans.localPosition.x,
                trans.localPosition.y+4,
                trans.localPosition.z);
        }
        else if (uIGuidePage.newBieGuild_step == NewBieGuild.Step_8 && enterCarSpawnLevel < 6)
        {
            effectNode = enterCarSpawn_Button.transform.Find("effectNode");
            Transform trans = null;
            trans = ResourceManager.GetInstance().LoadGameObject(Config.globalConfig.getInstace().GuideUiClickEffect).transform;
            trans.SetParent(effectNode, true);
            trans.localScale = UnityEngine.Vector3.one ;
            trans.position = effectNode.position;
            trans.localPosition = new UnityEngine.Vector3(
                trans.localPosition.x,
                trans.localPosition.y+4,
                trans.localPosition.z);
        }

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

    /// <summary>
    /// 获取等级段对应的奖励信息
    /// </summary>
    /// <returns></returns>
    private Config.itemCell GradeSliderAwardImage( )
    {
        var lvreward = parkingCell.lvreward;

        int idex = PlayerDataModule.FindLevelRangIndex(parkingCell.lvshage, profitLevel);
        int itemID = lvreward[idex];
        Config.itemCell itemCell = Config.itemConfig.getInstace().getCell(itemID);

        // Logger.LogWarp.LogErrorFormat("测试： 等级={0}，等级段对应的奖励={1}", profitLevel,itemID);
        return itemCell;
    }


}
