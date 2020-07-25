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
    public  void Awake_TestPart(GameObject go)
    { 
        RegistBtnAndClick("SetMainShow/Debug/Btn_Extend", OnClickTestButton);
        RegistBtnAndClick("SetMainShow/Debug/Btn_Extend01", OnClickTestButton01);
        //RegistBtnAndClick("SetMainShow/Debug/Btn_SceneOpen", OnClickTestClearCoin);
        RegistBtnAndClick("SetMainShow/Debug/Btn_SceneOpen", OnTestAdvert);

    }

    protected void OnClickTestButton(string str)
    {
        OnTestAddCoin();
        //OnTestEntryGate();
        //OnTestQuit();
        //OnTestBuff();
        //OnTestShip();
        //OnTestNewLittleZoo();
        //OnTestMoneyEffect();
        //OnTestRotaCamera();
        //OnTestDeactiveAnimal();
        //OnTestAnimalShowUI3D();
        //OnTestLongPressButton();
        //OnTestFormule();

        //OnTestOfflineBuff();
        // OnTestAddItem();
        //OnTestOldDataGoToMoreSceneData();
        //OnTestNoBuff();
    }
    /// <summary>
    /// 测试不包含buff的/m
    /// </summary>
    private void OnTestNoBuff()
    {
        var incomeCoinMS01 = PlayerDataModule.CurrScenePerMinCoin(true);
        var incomeCoinMS02 = PlayerDataModule.LeaveScenePerMinCoin(1,true);
        LogWarp.LogError("测试:    附带buff的" + MinerBigInt.ToDisplay(incomeCoinMS01));
        LogWarp.LogError("测试:    不带buff的" + MinerBigInt.ToDisplay(incomeCoinMS02));

    }
    private void OnTestOldDataGoToMoreSceneData()
    {
        var playerData = GlobalDataManager.GetInstance().playerData ;
        var parkingCenterData = playerData.GetParkingCenterDataIDIndexOfDataIdx();
        //LogWarp.LogErrorFormat("测试：新场景的数据 利润等级={0}   停车位等级={1}   来人速度={2} ",parkingCenterData.parkingProfitLevel,parkingCenterData.parkingSpaceLevel,parkingCenterData.parkingEnterCarSpawnLevel);

    }
    private void OnTestModificationSceneData()
    {
        Debug.LogError("修改了场景1");
        ZooGameLoader.GetInstance().ChangeScene(1);
        //GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID = 1;
        //GlobalDataManager.GetInstance().playerData.playerZoo.SetDefaultParkingCenterData();
        //GlobalDataManager.GetInstance().playerData.playerZoo.SetDefaultEntryGateData();
        //GlobalDataManager.GetInstance().playerData.playerZoo.SetDefaultlittleZooData(1);
        //UIInteractive.GetInstance().Init();

    }
    /// <summary>
    /// 测试Item的消息机制
    /// </summary>
    private void OnTestAddItem()
    {
        MessageInt.Send((int)GameMessageDefine.GetItem, 4);
    }

    private void OnTestOfflineBuff()
    {
        List<int> removeList = new List<int>{
            0,
            1,
            3,
            5,
        };
        List<string> myList = new List<string>
        {
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
        };

        List<string> myList01 = new List<string>();

        for (int i = 0; i < removeList.Count; i++)
        {
            int removeIdx = removeList[i];
            var str = myList[removeIdx];
            myList01.Add(str);
        }
        foreach (var item in myList01)
        {
            myList.Remove(item);
        }
        foreach (var item in myList)
        {
            LogWarp.LogError("测试：       "+item);
        }
    }

    protected void OnClickTestButton01(string str)
    {
        //OnTestBuffReturn();
        // OnTestItemData();
        //OnTestModificationSceneData();
        //ZooGameLoader.GetInstance().ChangeScene(0);

        //OnTestMultiIntCD();

        PageMgr.ShowPage<UITestToolWindowPage>();
        //LogWarp.LogError("   点击了   ");
        //OnTestMultiIntCD();

    }

    protected void OnClickTestChangeScene(string str)
    {
        Debug.LogError("场景1");
        //ZooGameLoader.GetInstance().ChangeScene(1);
        //GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID = 1;
        //GlobalDataManager.GetInstance().playerData.playerZoo.SetDefaultParkingCenterData();
        //GlobalDataManager.GetInstance().playerData.playerZoo.SetDefaultEntryGateData();
        //GlobalDataManager.GetInstance().playerData.playerZoo.SetDefaultlittleZooData(1);
        //UIInteractive.GetInstance().Init();

    }

    protected void OnClickTestClearCoin(string str)
    {
        //Debug.LogError("OnClickTestClearCoin");

        //var pd = GlobalDataManager.GetInstance().playerData;
        //var addCoin = BigInteger.Parse(pd.playerZoo.coin);
        //SetValueOfPlayerData.Send((int)GameMessageDefine.SetCoinOfPlayerData, 0, -addCoin, 0);

        //GlobalDataManager.GetInstance().leaveSceneCoinData.LeaveSceneCoinDict.Clear();

        //ZooGameLoader.GetInstance().ChangeScene(1);
        //GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID = 1;
        //GlobalDataManager.GetInstance().playerData.playerZoo.SetDefaultParkingCenterData();
        //GlobalDataManager.GetInstance().playerData.playerZoo.SetDefaultEntryGateData();
        //GlobalDataManager.GetInstance().playerData.playerZoo.SetDefaultlittleZooData(1);
        //UIInteractive.GetInstance().Init();

        //var animalProp = playerData.playerZoo.animalMSS15.GetAnimalProp(20101);
        //LogWarp.LogErrorFormat("Test:    level", animalProp.lv);


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

    int idx=0;
    private void OnTestAdvert(string str)
    {
        AdTagFM[] adTagFMs = { AdTagFM.Add_Double_Advert, AdTagFM.Add_Offline_Advert, AdTagFM.Add_Ticket_Advert, AdTagFM.Add_Viptiming_Advert, AdTagFM.Add_Tourist_Advert, AdTagFM.Add_Visit_Advert };
        if (idx == 6)
        {
            idx = 0;
        }
        float timeCount = 0.1f;
        DOTween.To(() => timeCount, a => timeCount = a, 0.1f, 0.1f).OnComplete(new TweenCallback(delegate
        {
            //PageMgr.ShowPage<UINewCurrencyAdvertPage>(AdTagFM.Add_Double_Advert);
            //PageMgr.ShowPage<UINewCurrencyAdvertPage>(AdTagFM.Add_Offline_Advert);
            //PageMgr.ShowPage<UINewCurrencyAdvertPage>(AdTagFM.Add_Ticket_Advert);
            //PageMgr.ShowPage<UINewCurrencyAdvertPage>(AdTagFM.Add_Tourist_Advert);
            PageMgr.ShowPage<UINewCurrencyAdvertPage>(adTagFMs[idx]);
            idx++;
            //PageMgr.ShowPage<UINewCurrencyAdvertPage>(AdTagFM.Add_Visit_Advert);
        }));

    }

    private void OnTestItemData()
    {
        var pd = GlobalDataManager.GetInstance().playerData;

        //Logger.LogWarp.LogErrorFormat("测试: item   钻石diamond={0}，   星星star={1}，   现金coin={2} ",pd.playerZoo.diamond,pd.playerZoo. star,pd.playerZoo.coin);



    }
    private void OnTestMultiIntCD()
    {
        //multiTickObj = new MultiIntCD();
        LogWarp.LogErrorFormat("运行并添加 multiTickObj");
        //multiTickObj.Run();
        //multiTickObj.AddCD(1000, Test01);
        //multiTickObj.AddCD(5000, Test02);


    }

    private void Test01(int arg1, IntCD arg2)
    {
        LogWarp.LogErrorFormat("AAAAAAAAAAAAAAAAA   {0}    {1}",arg1,arg2.org);
        arg2.Reset();
        arg2.Run();
    }
    private void Test02(int arg1, IntCD arg2)
    {
        LogWarp.LogErrorFormat("BBBBBBBBBBBBBBBBB   {0}    {1}", arg1, arg2.org);
        arg2.Reset();
        arg2.Run();

    }

    /// <summary>
    /// 测试数值
    /// </summary>
    private void OnTestFormule()
    {
        for (int i = 1; i < 50; i++)
        {
            //var number01 = PlayerDataModule.GetAdditionExpect(i);
            //LogWarp.LogErrorFormat("测试数值  加成预期：  等级={0}   value={1}    ", i, number01);
            //var number02 = ParkingCenter.ParkingEnterCarSpawnExpectLevel(i);
            //LogWarp.LogErrorFormat("测试数值  来客速度期望等级：  等级={0}   value={1}    ", i, number02);
            //var number03 = ParkingCenter.ParkingSpaceExpectLevel(i);
            //LogWarp.LogErrorFormat("测试数值  停车位数期望等级：  等级={0}   value={1}    ", i, number03);
            var number04 = ParkingCenter.ParkingProfitExpectLevel(i);
            //LogWarp.LogErrorFormat("测试数值  利润提升期望等级等级：  等级={0}   value={1}    ", i, number04);


            //var number05 = LittleZooModule.GetAnimalExpectLevel(i);
            //LogWarp.LogErrorFormat("测试数值  动物期望等级：  等级={0}   value={1}    ", i, number05);


            //var number06 = LittleZooModule.GetAnimalUpLevelPriceFormula( i);
            //LogWarp.LogErrorFormat("测试数值  动物升级消耗价格：  等级={0}   value={1}    ", i, number06);

        }
    }

    private void TestOnAddBuffSucceed(Message obj)
    {
        LogWarp.LogError("测试，buff便换了");

    }

    /// <summary>
    /// 测试:3D动物显示在UI上
    /// </summary>
    private void OnTestAnimalShowUI3D()
    {
        //PageMgr.ShowPage<UIReceivePage>("10101");  //旋转视角UI
        //PageMgr.ClosePage<UIZooPage>();
        ////PageMgr.ClosePage<UIMainPage>();
        //MessageString.Send((int)GameMessageDefine.UIMessage_ActiveButHidePart, "UIMainPage");
    }

    /// <summary>
    /// 测试：动物购买
    /// </summary>
    private void OnTestDeactiveAnimal()
    {
        LogWarp.LogError("测试：：   删除动物");
        // LittleZooModule.DeactiveAnimal(1001, 10101);
        Dictionary<int, EntityMovable> entityMovables = EntityManager.GetInstance().entityMovables;
        EntityManager.GetInstance().RemoveFromEntityMovables(entityMovables[entityMovables.Count - 1]);
    }

    /// <summary>
    /// 测试 建筑升级相机旋转
    /// </summary>
    private void OnTestRotaCamera()
    {
        //int littleZooID = 1001;
        //string path = string.Format("LittleZoo/{0}", littleZooID.ToString());
        ////LogWarp.LogError("测试：   路径   " + path);
        //GlobalDataManager.GetInstance().playerData.playerZoo.BuildShowTransform = GameObject.Find(path).transform;
        //PageMgr.ClosePage<UIZooPage>();
        ////PageMgr.ClosePage<UIMainPage>();
        //MessageString.Send((int)GameMessageDefine.UIMessage_ActiveButHidePart, "UIMainPage");

        //PageMgr.ShowPage<UIBuildShowPage>();  //旋转视角UI
    }

    protected void OnTestAddCoin()
    {
        //点一下,把当前的金币变成2倍
        var pd = GlobalDataManager.GetInstance().playerData;
        var addCoin = pd.playerZoo.playerCoin.GetCoinByScene(pd.playerZoo.currSceneID).coinBigInt;
        SetValueOfPlayerData.Send((int)GameMessageDefine.AddCoinOfPlayerDataMSSC, 0, addCoin, 0); 
    }

    protected void OnTestEntryGate()
    {
        SetDetailValueOfPlayerData.Send((int)GameMessageDefine.SetEntryGatePureLevelOfPlayerData, 0, 1, 0);
        //SetValueOfPlayerData.Send((int)GameMessageDefine.SetEntryGateNumOfPlayerData, 1, 0, 0);
    }

    protected void OnTestQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPaused = true;
#else
        Application.Quit();
#endif

    }

    protected void OnTestBuff()
    {
        //Buff测试
        //BroadcastNum.Send((int)GameMessageDefine.AddBuff, 1, 0, 0);
        //BroadcastNum.Send((int)GameMessageDefine.AddBuff, 9, 0, 0);
        //BroadcastNum.Send((int)GameMessageDefine.AddBuff, 10, 0, 0);
        BroadcastNum.Send((int)GameMessageDefine.AddBuff, 1, 0, 0);
        BroadcastNum.Send((int)GameMessageDefine.AddBuff, 2, 0, 0);
        BroadcastNum.Send((int)GameMessageDefine.AddBuff, 5, 0, 0);
        BroadcastNum.Send((int)GameMessageDefine.AddBuff, 14, 0, 0);

    }
    protected void OnTestBuffReturn()
    {
        
        var buffList = GlobalDataManager.GetInstance().playerData.playerZoo.buffList;

        //var offlineBuffList01 = PlayerDataModule.OnGetBuffCoefficient(buffList);

        foreach (var item in buffList)
        {
            LogWarp.LogErrorFormat("测试： offlineBuffList:  buffID={0},  buff使用时间={1}，buff还剩时间={2}， buff类型={3},  buff值={4}     加倍系数{5}",
            item.buffID,    item.CD.org,        item.CD.cd,    item.buffType,  item.buffVal,PlayerDataModule.PlayerRatioCoinInComeMul());
        }



    }

    protected void OnTestShip()
    {
        EntityShip.GetoffVisitor(15);
    }

    protected void OnTestNewLittleZoo()
    {

        ////开启新动物栏测试var 
        //var littleZooModuleDatas = GlobalDataManager.GetInstance().playerData.playerZoo.littleZooModuleDataList_New;
        //int level = Const.Invalid_Int;
        //int littleZooID = Const.Invalid_Int;
        //for (int i = 0; i < littleZooModuleDatas.Count; i++)
        //{
        //    littleZooID = littleZooModuleDatas[i].littleZooID;
        //    level = littleZooModuleDatas[i].littleZooTicketsLevel;
        //    if (level == 0)
        //    {
        //        OpenNewLittleZoo.Send(littleZooID);
        //        break;
        //    }
        //}
    }

    ///// <summary>
    ///// 测试长按按钮事件
    ///// </summary>
    //protected void OnTestLongPressButton()
    //{
    //    LogWarp.LogError("AAAAAAAAAAAAAAAAA    测试长按按钮事件   !");

    //    if (isBool)
    //    {
    //        SetValueOfPlayerData.Send((int)GameMessageDefine.SetParkingProfitLevelOfPlayerData,
    //            1, 0, 0);
    //        LogWarp.LogError("AAAAAAAAAAAAAAAAA    成功   停车场等级 =" + GlobalDataManager.GetInstance().playerData.GetParkingCenterDataIDIndexOfDataIdx().parkingProfitLevel);
    //        isBool = false;
    //    }

    //}
    //bool isBool = true;
    //protected void OnTestReleaseButton()
    //{
    //    LogWarp.LogError("BBBBBBBBBBBBBBBBB    测试离开长按按钮事件   !");
    //    isBool = true;
    //}

}
