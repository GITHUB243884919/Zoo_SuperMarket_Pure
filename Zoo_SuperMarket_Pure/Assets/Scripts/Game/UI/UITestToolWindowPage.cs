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
public class UITestToolWindowPage : UIPage
{
    public UITestToolWindowPage() : base(UIType.Fixed, UIMode.DoNothing, UICollider.None)
    {
        uiPath = "uiprefab/UITestToolWindow";
    }

    public override void Awake(GameObject go)
    {
        base.Awake(go);
        RegistAllCompent();
        //GetTransPrefabAllTextShow(this.transform);
    }
    /// <summary>
    /// 更新:动态修改图片大小
    /// </summary>
    public override void Refresh()
    {
        base.Refresh();
    }
    PlayerData playerData;
    bool isButton = true;
    /// <summary>
    /// 活跃
    /// </summary>
    public override void Active()
    {
        base.Active();
        playerData = GlobalDataManager.GetInstance().playerData;
        MessageManager.GetInstance().Regist((int)GameMessageDefine.BroadcastCoinOfPlayerDataMSSC, this.OnGetBroadcastCoinOfPlayerDataMSSC);
        MessageManager.GetInstance().Regist((int)GameMessageDefine.BroadcastDiamondOfPlayerData, this.OnBroadcastDiamondOfPlayerData);

    }

    private void OnBroadcastDiamondOfPlayerData(Message obj)
    {
        //LogWarp.LogErrorFormat("测试 ：   接到钻石修改    ：{0}", this.playerData.playerZoo.diamond);
    }

    private void OnGetBroadcastCoinOfPlayerDataMSSC(Message obj)
    {
        isButton = true;
    }

    /// <summary>
    /// 隐藏
    /// </summary>
    public override void Hide()
    {
        base.Hide();
    }
    InputField OfflineTimeText;
    InputField LittleZooIDText;
    InputField CrossRoadStageIDText;
    InputField CrossRoadStrengthText;



    public int littleZooID=UFrame.Const.Invalid_Int;
    /// <summary>
    /// 内部组件查找
    /// </summary>
    private void RegistAllCompent()
    {
        RegistBtnAndClick("TestToolButtonPanel/Panel/TestAddCoin", OnClickTestAddCoinButton);
        RegistBtnAndClick("TestToolButtonPanel/Panel/TestClearCoin", OnClickTestClearCoinButton);
        RegistBtnAndClick("TestToolButtonPanel/Panel/TestAddStar", OnClickTestAddStarButton);
        RegistBtnAndClick("TestToolButtonPanel/Panel/TestClearBuffTime", OnClickTestClearBuffTimeButton);
        RegistBtnAndClick("CloseButton", OnClickCloseButton);

        OfflineTimeText = RegistCompent<InputField>("TestToolTablePanel/CellGroup/OfflineTime/OfflineTimeText");
        RegistBtnAndClick("TestToolTablePanel/CellGroup/OfflineTime/TestOfflineTime", OnClickTestOfflineTimeButton);
        RegistBtnAndClick("TestToolTablePanel/CellGroup/TestUpgrade/ParkingUpLv", OnClickTestParkingUpLvButton);
        RegistBtnAndClick("TestToolTablePanel/CellGroup/TestUpgrade/TicketUpLv", OnClickTestTicketUpLvButton);
        LittleZooIDText = RegistCompent<InputField>("TestToolTablePanel/CellGroup/TestUpgrade/LittleZooID");
        RegistBtnAndClick("TestToolTablePanel/CellGroup/TestUpgrade/LittleZooUpLv", OnClickTestLittleZooUpLvButton);
        RegistBtnAndClick("TestToolTablePanel/CellGroup/TestUpgrade/TestAddDiamond", OnClickTestAddDiamondButton);

        CrossRoadStageIDText = RegistCompent<InputField>("TestToolTablePanel/CellGroup/CrossRoadTest/CrossRoadStageID");
        RegistBtnAndClick("TestToolTablePanel/CellGroup/CrossRoadTest/CrossRoadStageSet", OnClickSetCrossRoadStage);

        //CrossRoadStrengthText = RegistCompent<InputField>("TestToolTablePanel/CellGroup/CrossRoadTest01/CrossRoadStrength");
        RegistBtnAndClick("TestToolTablePanel/CellGroup/CrossRoadTest01/CrossRoadStrengthSet", OnClickSetCrossRoadStrength);
    }
    /// <summary>
    /// 设置小游戏体力
    /// </summary>
    /// <param name="obj"></param>
    private void OnClickSetCrossRoadStrength(string obj)
    {
        UFrame.MessageInt.Send((int)GameMessageDefine.AddStrength, Config.globalConfig.getInstace().MaxStrength);

    }

    /// <summary>
    /// 设置小游戏关卡
    /// </summary>
    /// <param name="obj"></param>
    private void OnClickSetCrossRoadStage(string obj)
    {
        if (int.TryParse(CrossRoadStageIDText.text, out int timeNumber))
        {
            //转换成功, 输出数字
            playerData.playerLittleGame.stageID = timeNumber-1;
            PromptText.CreatePromptText_TestUI("设置成功");
            OnClickCloseButton("");
        }
        else
        {
            //转换失败, 字符串不是只是数字
            PromptText.CreatePromptText_TestUI("输入字符串不是纯数字");
            CrossRoadStageIDText.text = "";
        }

    }

    /// <summary>
    /// 增加钻石
    /// </summary>
    /// <param name="obj"></param>
    private void OnClickTestAddDiamondButton(string obj)
    {
        //LogWarp.LogError("增加钻石");

        SetValueOfPlayerData.Send((int)GameMessageDefine.SetDiamondOfPlayerData, 1000, 0, 0);
    }

    private void OnClickTestOfflineTimeButton(string obj)
    {
        //LogWarp.LogErrorFormat("测试：   {0}  ", OfflineTimeText.text);
        //尝试把 input 变为整数(integer), 并储入 number 中
        if (int.TryParse(OfflineTimeText.text, out int timeNumber))
        {
            //转换成功, 输出数字
            playerData.isTestOfflineRewardPage = true;
            playerData.isTestOfflineRewardTime = timeNumber;
            MessageManager.GetInstance().Send((int)GameMessageDefine.CalcOffline);
        }
        else
        {
            //转换失败, 字符串不是只是数字
            PromptText.CreatePromptText_TestUI("输入字符串不是纯数字");
            OfflineTimeText.text = "";
        }
    }

    /// <summary>
    /// 关闭UI
    /// </summary>
    /// <param name="obj"></param>
    private void OnClickCloseButton(string obj)
    {
        float timeCount = 0.1f;
        DOTween.To(() => timeCount, a => timeCount = a, 0.1f, 0.1f).OnComplete(new TweenCallback(delegate
        {
            PageMgr.ClosePage<UITestToolWindowPage>();
        }));
        //Hide();
    }
    /// <summary>
    /// 增加星星
    /// </summary>
    /// <param name="obj"></param>
    private void OnClickTestAddStarButton(string obj)
    {
        //仅仅测试工具写法
        playerData.playerZoo.star += 10;
        BroadcastValueOfPlayerData.Send((int)GameMessageDefine.BroadcastStarOfPlayerData, this.playerData.playerZoo.star, 0, 0, 0);

    }
    /// <summary>
    /// 清除增益buff的时间
    /// </summary>
    /// <param name="obj"></param>
    private void OnClickTestClearBuffTimeButton(string obj)
    {
        var buffList = GlobalDataManager.GetInstance().playerData.playerZoo.buffList;
        foreach (var item in buffList)
        {
            if (item.buffID ==14)
            {
                item.CD.cd -= 3600;
            }
        }
    }
    /// <summary>
    /// 清除当前金币
    /// </summary>
    /// <param name="obj"></param>
    private void OnClickTestClearCoinButton(string obj)
    {
        var pd = GlobalDataManager.GetInstance().playerData;
        var addCoin = pd.playerZoo.playerCoin.GetCoinByScene(pd.playerZoo.currSceneID).coinBigInt;
        SetValueOfPlayerData.Send((int)GameMessageDefine.AddCoinOfPlayerDataMSSC, 0, -addCoin, 0);

        GlobalDataManager.GetInstance().leaveSceneCoinData.LeaveSceneCoinDict.Clear();
    }
    /// <summary>
    /// 增加当前金钱
    /// </summary>
    /// <param name="obj"></param>
    private void OnClickTestAddCoinButton(string obj)
    {
        LogWarp.LogFormat("AAAAAAAAAAAAAAA");
        //点一下,把当前的金币变成2倍
        var pd = GlobalDataManager.GetInstance().playerData;
        var addCoin = pd.playerZoo.playerCoin.GetCoinByScene(pd.playerZoo.currSceneID).coinBigInt;
        SetValueOfPlayerData.Send((int)GameMessageDefine.AddCoinOfPlayerDataMSSC, 0, addCoin, 0);

    }

    /// <summary>
    /// 升级停车场利润等级
    /// </summary>
    /// <param name="obj"></param>
    private void OnClickTestParkingUpLvButton(string obj)
    {
        LogWarp.LogError(" 升级停车场利润等级 ");
        //获取当前等级
        var level = playerData.GetParkingCenterDataIDIndexOfDataIdx().parkingProfitLevel;
        var parkingCell = GlobalDataManager.GetInstance().logicTableParkingData.GetParkingCell(playerData.playerZoo.currSceneID);
        //获取当前需要升级的级别
        int needUpLV = GetNeedUpLv(parkingCell.lvshage,level);
        //发送消息
        //发送消息       SetValueOfPlayerData  消息体   
        SetDetailValueOfPlayerData.Send((int)GameMessageDefine.SetParkingProfitLevelOfPlayerData,
        needUpLV, 0, 0);
        //提升升级成功
        PromptText.CreatePromptText_TestUI("开始升级停车场门票等级= "+ level + "  增加= "+ needUpLV);
        isButton = false;
    }



    /// <summary>
    /// 升级售票口门票等级
    /// </summary>
    /// <param name="obj"></param>
    private void OnClickTestTicketUpLvButton(string obj)
    {
        LogWarp.LogError(" 升级售票口门票等级 ");
        //获取当前等级
        var level = playerData.GetEntryDateDataIDIndexOfDataIdx().entryTicketsLevel;
        var sortGateIDs = GlobalDataManager.GetInstance().logicTableEntryGate.GetSortGateIDs(playerData.playerZoo.currSceneID);
        var ticketCell = Config.ticketConfig.getInstace().getCell(sortGateIDs[0]);
        //获取当前需要升级的级别
        int needUpLV = GetNeedUpLv(ticketCell.lvshage, level);
        //发送消息
        //发送消息       SetValueOfPlayerData  消息体   
        SetDetailValueOfPlayerData.Send((int)GameMessageDefine.SetEntryGateLevelOfPlayerData,
                       needUpLV, 0,0 );
        //提升升级成功
        PromptText.CreatePromptText_TestUI("开始升级售票口门票等级+" + needUpLV);
        isButton = false;

    }
    /// <summary>
    /// 升级某个动物栏门票等级
    /// </summary>
    /// <param name="obj"></param>
    private void OnClickTestLittleZooUpLvButton(string obj)
    {
        LogWarp.LogError(" 升级某个动物栏门票等级 ");

        if (littleZooID != UFrame.Const.Invalid_Int)
        {
            //转换成功, 输出数字
            var level = playerData.GetLittleZooModuleData(littleZooID).littleZooTicketsLevel ;
            var ticketCell = Config.buildupConfig.getInstace().getCell(littleZooID);
            //获取当前需要升级的级别
            int needUpLV = GetNeedUpLv(ticketCell.lvshage, level);
            //发送消息
            //发送消息       SetValueOfPlayerData  消息体   
            SetDetailValueOfPlayerData.Send((int)GameMessageDefine.SetLittleZooTicketsLevelPlayerData,
                           littleZooID, needUpLV, 0);
            //提升升级成功
            PromptText.CreatePromptText_TestUI("开始升级动物栏门票等级+" + needUpLV);
            isButton = false;
        }
        else
        {
            //转换失败, 字符串不是只是数字
            PromptText.CreatePromptText_TestUI("升级动物栏前先点击下动物栏");
        }
    }

    
    /// <summary>
    /// 返回当前等级在等级段的差距
    /// </summary>
    /// <param name="array"></param>
    /// <param name="level"></param>
    /// <returns></returns>
    private int GetNeedUpLv(int[] array,int level )
    {
        int idx = PlayerDataModule.FindLevelRangIndex(array,level);
        int needLv = array[idx] - level;
        LogWarp.LogErrorFormat(" 测试：      当前等级    {0}    在数组下标{1}    需要的等级{2} ",level,idx, needLv);
        return needLv;
    }
}
