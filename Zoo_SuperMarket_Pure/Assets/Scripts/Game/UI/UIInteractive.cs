using Game;
using Game.GlobalData;
using Game.MessageCenter;
using UFrame.Logger;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UFrame;
using UFrame.Common;
using UFrame.MessageCenter;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class UIInteractive : SingletonMono<UIInteractive>
{
    /// <summary>
    /// 获取Data，方便获取动物园
    /// </summary>
    public UIPage iPage;
    Dictionary<string, System.Action> clickCallbacks = new Dictionary<string, System.Action>();
    string nameID;
    Transform sceneUIButtonPos;
    /// <summary>
    /// 点击动画播放器
    /// </summary>
    SimpleAnimation buildingClickSa;

    public override void Awake()
    {
        base.Awake();

        buildingClickSa = new SimpleAnimation();

        //初始化
        //Init();
        MessageManager.GetInstance().Regist((int)GameMessageDefine.OpenNewLittleZoo, this.OnOpenNewLittleZoo);
        
    }
    private void OnOpenNewLittleZoo(Message obj)
    {
        this.Init();
    }

    /// <summary>
    /// 点击事件
    /// </summary>
    /// <param name="gesture"></param>
    public void OnTapUIGB()
    {
        //若没有UI显示界面，显示，否则隐藏Ui界面
        if (iPage == null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                DisposeUIInteractive(hit.collider.gameObject);
            }
        }
        else
        {
            SetClosePage(iPage);
            iPage = null;

        }
    }
    public void SetClosePage(UIPage closeIPage)
    {
        if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide != true)
        {
            float timeCount = 0.1f;
            DOTween.To(() => timeCount, a => timeCount = a, 0.1f, 0.1f).OnComplete(new TweenCallback(delegate
            {
                PageMgr.ClosePage(closeIPage.name);
                MessageString.Send((int)GameMessageDefine.UIMessage_ActiveButShowPart, "UIMainPage");

            }));
            iPage = null;

        }
        else
        {
            return;
        }
    }

    /// <summary>
    /// 处理养成UI点击交互
    /// </summary>
    /// <param name="gameObject">点击对象</param>
    void DisposeUIInteractive(GameObject gameObject)
    {
        nameID = gameObject.name;
        //LogWarp.LogError("AAAAAAAAAAAAAAAAAAAAAAAAAA                "+nameID);

        Action action = null;
        if (clickCallbacks.TryGetValue(gameObject.name, out action))
        {
            sceneUIButtonPos = gameObject.transform;
            action?.Invoke();
            var anim = gameObject.GetComponentInChildren<Animation>();
            if (anim != null)
            {
                buildingClickSa.Init(anim);
                buildingClickSa.Play(Config.globalConfig.getInstace().BuildClickAnim);
            }
            ZooCamera.GetInstance().PointAtScreenUpCenter(gameObject.transform.position);

        }
        //if (gameObject.name == needShowID)
        //{
        //    ZooCamera.GetInstance().PointAtScreenUpCenter(gameObject.transform.position);
        //    MessageString.Send((int)GameMessageDefine.UIMessage_OnClickButHidePart, "UIMainPage");
        //}
    }

    private static void SetMainPageHidePart(GameObject gameObject)
    {
        MessageString.Send((int)GameMessageDefine.UIMessage_ActiveButHidePart, "UIMainPage");
    }

    public void OnClickParking()
    {
        if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide == true)
        {
            UIGuidePage uIGuidePage = PageMgr.GetPage<UIGuidePage>();
            if (uIGuidePage ==null)
            {
                string e = string.Format("新手引导界面  PageMgr.allPages里 UIGuidePage   为空");
                throw new System.Exception(e);
            }
            if (uIGuidePage.newBieGuild_step != NewBieGuild.Step_5)
            {
                return;
            }
            else
            {  //取消场景特效  进入场景点击事件
                uIGuidePage.DestroyEffectChild();
                uIGuidePage.TAEvent_finish();
                uIGuidePage.TAEvent_start((int)uIGuidePage.newBieGuild_step + 1);
                uIGuidePage.TAEvent_process((int)uIGuidePage.newBieGuild_step + 1);
                uIGuidePage.SetCameraOnClickScene(sceneUIButtonPos);
            }
        }
        PageMgr.ShowPage<UIParkPage>();  //停车场UI交互
        iPage = new UIParkPage();

        //PageMgr.ShowPage<UIZooPage>(1001);  //大象馆UI交互 
        //iPage = new UIZooPage();
        SetMainPageHidePart(gameObject);

    }

    public void OnClickEntry()
    {
        if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide == true)
        {
            UIGuidePage uIGuidePage = PageMgr.GetPage<UIGuidePage>();
            if (uIGuidePage == null)
            {
                string e = string.Format("新手引导界面  PageMgr.allPages里 UIGuidePage   为空");
                throw new System.Exception(e);
            }
            if (uIGuidePage.newBieGuild_step != NewBieGuild.Step_12)
            {
                return;
            }
            else
            {  //取消场景特效  进入场景点击事件
                uIGuidePage.DestroyEffectChild();
                uIGuidePage.TAEvent_finish();
                uIGuidePage.TAEvent_start((int)uIGuidePage.newBieGuild_step + 1);
                uIGuidePage.TAEvent_process((int)uIGuidePage.newBieGuild_step + 1);
                uIGuidePage.SetCameraOnClickScene(sceneUIButtonPos);
            }
        }
        PageMgr.ShowPage<UIEntryPage>();  //摆渡车UI交互
        iPage = new UIEntryPage();
        SetMainPageHidePart(gameObject);

    }
    public void OnClickZoo()
    {
        //判断当前动物栏的等级是否不为0   为0开启新的动物栏
        int litteZooLevel = GlobalDataManager.GetInstance().playerData.GetLittleZooModuleData(int.Parse(nameID)).littleZooTicketsLevel;
        if (litteZooLevel > 0)
        {
            if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide == true)
            {
                UIGuidePage uIGuidePage = PageMgr.GetPage<UIGuidePage>();
                if (uIGuidePage == null)
                {
                    string e = string.Format("新手引导界面  PageMgr.allPages里 UIGuidePage   为空");
                    throw new System.Exception(e);
                }
                //LogWarp.LogErrorFormat("测试：  新手引导步骤   动物栏点击   {0}  ", uIGuidePage.procedure);
                if (uIGuidePage.newBieGuild_step != NewBieGuild.Step_20)
                {
                    return;
                }
                else
                {  //取消场景特效  进入场景点击事件
                    uIGuidePage.DestroyEffectChild();
                    uIGuidePage.TAEvent_finish();
                    uIGuidePage.TAEvent_start((int)uIGuidePage.newBieGuild_step+1);
                    uIGuidePage.TAEvent_process((int)uIGuidePage.newBieGuild_step + 1);
                    uIGuidePage.SetCameraOnClickScene(sceneUIButtonPos);
                    //uIGuidePage.TAEvent_finish(1);
                    //uIGuidePage.TAEvent_start(2);

                }
            }
            //LogWarp.LogErrorFormat("ssssssssssssssss      {0}", nameID);
            //显示UI
            PageMgr.ShowPage<UIZooPage>(nameID);  //动物栏UI交互 
            iPage = new UIZooPage();
            UITestToolWindowPage uITestToolWindowPage = PageMgr.GetPage<UITestToolWindowPage>();
            if (uITestToolWindowPage != null)
            {
                uITestToolWindowPage.littleZooID = int.Parse(nameID);
            }
            SetMainPageHidePart(gameObject);

        }
        else
        {
            int idx = Config.buildupConfig.getInstace().getCell(nameID).affirmopen;

            if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide == true)
            {
                return;
            }
            if (idx == 0)
            {
                PromptText.CreatePromptText("Ui_Text_122");
                return;
            }
            PageMgr.ShowPage<UIBuildOpenPage>(nameID);  //开启新的动物园交互
            iPage = new UIBuildOpenPage();
            SetMainPageHidePart(gameObject);

        }
    }
    public void Init(int sceneID=-1)
    {
        if (sceneID == -1)
        {
            sceneID = GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID;
        }
        PageMgr.ClosePage<UIParkPage>();
        PageMgr.ClosePage<UIEntryPage>();
        PageMgr.ClosePage<UIZooPage>();
        SetPackingToClickCallbacks(sceneID);
        SetTicketToClickCallbacks(sceneID);

        //对clickCallbacks字典进行数据填充
        //clickCallbacks.Clear();
        //获取数据
        var littleZooModuleDatas = GlobalDataManager.GetInstance().playerData.playerZoo.littleZooModuleDatasMSS;
        for (int i = 0; i < littleZooModuleDatas.Count; i++)
        {
            if ( littleZooModuleDatas[i].sceneID == sceneID)
            {
                if (!clickCallbacks.ContainsKey(littleZooModuleDatas[i].littleZooID.ToString()))
                {                  
                    GlobalDataManager.GetInstance().playerData.GetLittleZooIDIndexOfDataIdx(littleZooModuleDatas[i].littleZooID);
                    //LogWarp.LogError("不包含此  " + littleZooModuleDatas[i].littleZooID + "    等级=" + GlobalDataManager.GetInstance().playerData.GetLittleZooIDIndexOfDataIdx(littleZooModuleDatas[i].littleZooID));
                    int level = littleZooModuleDatas[i].littleZooTicketsLevel;

                    clickCallbacks.Add(littleZooModuleDatas[i].littleZooID.ToString(), OnClickZoo);
                    if (level < 1)
                    {
                        break;
                    }
                }
            }
        }
    }

    

    private void SetPackingToClickCallbacks(int sceneID)
    {
        var allParkingData = Config.parkingConfig.getInstace().AllData;
        Config.parkingCell parkingCell = null;
        foreach (var item in allParkingData.Values)
        {
            if (item.scene == sceneID)
            {
                parkingCell = item;
                break;
            }
        }
        if (!clickCallbacks.ContainsKey(Config.globalConfig.getInstace().ParkingButton)) {
            clickCallbacks.Add(Config.globalConfig.getInstace().ParkingButton, OnClickParking);

        }
    }

    private void SetTicketToClickCallbacks(int sceneID)
    {
        var sortGateIDs = GlobalDataManager.GetInstance().logicTableEntryGate.GetSortGateIDs(sceneID);
        Config.ticketCell ticketCell = Config.ticketConfig.getInstace().getCell( sortGateIDs[0]);
        if (!clickCallbacks.ContainsKey(ticketCell.prefabsname))
        {
            clickCallbacks.Add(ticketCell.prefabsname, OnClickEntry);
        }
    }
}
