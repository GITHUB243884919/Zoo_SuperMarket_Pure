using DG.Tweening;
using Game;
using Game.GlobalData;
using Game.MessageCenter;
using UFrame.Logger;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UFrame;
using UFrame.EntityFloat;
using UFrame.MiniGame;
using UFrame.OrthographicCamera;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIGuidePage : UIPage
{
    /// <summary>
    /// 对话的按钮    即下一步
    /// </summary>
    private Button dialogBoxButton;
    /// <summary>
    /// 对话的文本
    /// </summary>
    private Text dialogText;
    /// <summary>
    /// NPC的名字
    /// </summary>
    private Text npcNameText;
    /// <summary>
    /// 引导页面的遮罩
    /// </summary>
    private MaskableGraphic OpenTouch;
    /// <summary>
    /// 对话按钮的遮罩
    /// </summary>
    private MaskableGraphic dialogButtonMaskableGraphic;

    private GameObject mainMesh;

    private Button uibg;

    /// <summary>
    /// 当前引导步骤
    /// </summary>
    public int procedure;

    /// <summary>
    /// 当前引导步骤  枚举
    /// </summary>
    public NewBieGuild newBieGuild_step =NewBieGuild.Step_0;

    /// <summary>
    /// 相机跟随需要的对象
    /// </summary>
    public EntityMovable entity;

    /// <summary>
    /// 场景特效节点
    /// </summary>
    private Transform effectNode;

    public int number;
    PlayerZoo playerZoo;

    public UIGuidePage() : base(UIType.Fixed, UIMode.DoNothing, UICollider.None, UITickedMode.Update)
    {
        uiPath = "uiprefab/UIGuide";
    }
    public override void Awake(GameObject go)
    {
        base.Awake(go);
        this.RegistCompent();
        GetTransPrefabAllTextShow(this.transform);
    }

    /// <summary>
    /// 活跃
    /// </summary>
    public override void Active()
    {
        base.Active();
        MessageString.Send((int)GameMessageDefine.UIMessage_ActiveButHidePart, "UIMainPage");
        if (GlobalDataManager.GetInstance().playerData.GetLittleZooModuleData(1001).littleZooTicketsLevel >= 5&& newBieGuild_step==NewBieGuild.Step_0)
        {
            GlobalDataManager.GetInstance().playerData.playerZoo.isGuide = false;
            Hide();
        }
        dialogBoxButton.enabled = true;
        uibg.enabled = true;

        OnClickDialogBoxButton();
        number = 0;
    }
    /// <summary>
    /// 隐藏
    /// </summary>
    public override void Hide()
    {
        if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide == false)
        {
            ZooCamera.GetInstance().dragMoveTo = ZooCamera.GetInstance().editorInitPos;
            ZooCamera.GetInstance().cacheTrans.position = ZooCamera.GetInstance().editorInitPos;
        }
        base.Hide();
    }
    /// <summary>
    /// 内部组件的查找赋值
    /// </summary>
    private void RegistCompent()
    {
        //dialogBoxButton = RegistBtnAndClick("GameObject/UiBg/MainMesh/GuideGroup", OnClickDialogBoxButton);
        //uibg = transform.Find("GameObject/UiBg").GetComponent<Button>();
        //uibg.onClick.AddListener(delegate
        //{
        //    OnClickDialogBoxButton("uibg");
        //    BtnScaleAnim(dialogBoxButton.gameObject, 1.1f, 0.95f);
        //});

        dialogBoxButton = transform.Find("GameObject/UiBg/MainMesh/GuideGroup").GetComponent<Button>();
        uibg = transform.Find("GameObject/UiBg").GetComponent<Button>();
        dialogBoxButton.onClick.AddListener(OnClickDialogBoxButton);
        uibg.onClick.AddListener(OnClickDialogBoxButton);

        dialogText = RegistCompent<Text>("GameObject/UiBg/MainMesh/GuideGroup/GuideText");
        npcNameText = RegistCompent<Text>("GameObject/UiBg/MainMesh/GuideGroup/NpcNameBg/NpcNameText");
        npcNameText.text = this.AcquireData(0);
        dialogButtonMaskableGraphic = RegistCompent<MaskableGraphic>("GameObject/UiBg/MainMesh/GuideGroup");

        mainMesh = transform.Find("GameObject/UiBg/MainMesh").gameObject;
        //默认进入步骤第一步
        newBieGuild_step  = 0;
        newBieGuild_step = NewBieGuild.Step_0;
        //游戏暂停
        GameManager.GetInstance().Pause(true);
    }

    /// <summary>
    /// 获取引导页面的点击事件
    /// </summary>
    /// <param name="obj"></param>
    public void OnClickDialogBoxButton()
    {

        //Debug.LogError("新手阶段：  OnClickDialogBoxButton   步骤 = "+ procedure );


        switch (newBieGuild_step)
        {
            case NewBieGuild.Step_0:
                newBieGuild_step = NewBieGuild.Step_1;
                TAEvent_start();
                this.InitCommint(newBieGuild_step);        //文本刷新
                TAEvent_process();
                break;
            case NewBieGuild.Step_1:
                TAEvent_finish();
                newBieGuild_step = NewBieGuild.Step_2;
                TAEvent_start();                    //  很幸运，动物园这么快就迎来了第一位游客！
                this.InitCommint(newBieGuild_step);        //文本刷新
                TAEvent_process();
                break;
            case NewBieGuild.Step_2:
                TAEvent_finish();
                newBieGuild_step = NewBieGuild.Step_3;
                TAEvent_start();                 //游戏运行，镜头跟随第一位游客移动 监听是否到达停车场
                GameManager.GetInstance().Pause(false);
                TAEvent_process();
                SetCameraToFollow();                //设置相机跟随
                DelayedHide();                      //隐藏新手引导界面
                break;
            case NewBieGuild.Step_3:
                TAEvent_finish();
                newBieGuild_step = NewBieGuild.Step_4;
                TAEvent_start();                    //本阶段到达了停车场 游戏暂停
                GameManager.GetInstance().Pause(true);
                TAEvent_process();
                this.InitCommint(newBieGuild_step);        //文本刷新
                break;
            case NewBieGuild.Step_4:
                TAEvent_finish();
                newBieGuild_step = NewBieGuild.Step_5;
                TAEvent_start();                   //本阶段需要取消文本  添加场景UI特效
                TAEvent_process();
                playerZoo = GlobalDataManager.GetInstance().playerData.playerZoo;
                if (GlobalDataManager.GetInstance().playerData.GetParkingCenterDataIDIndexOfDataIdx().parkingSpaceLevel > 1)
                {
                    //LogWarp.LogError("新手阶段：    "+ GlobalDataManager.GetInstance().playerData.GetParkingCenterDataIDIndexOfDataIdx().parkingSpaceLevel);
                    OnClickDialogBoxButton();
                }
                else
                {
                    //LogWarp.LogError("新手阶段：   添加场景UI特效    隐藏新手引导界面");
                    this.SetSceneAnimateGameObject(newBieGuild_step);//添加场景UI特效
                    DelayedHide();                      //隐藏新手引导界面
                }
                break;
            case NewBieGuild.Step_5:
                TAEvent_finish((int)newBieGuild_step + 1);
                newBieGuild_step = NewBieGuild.Step_7;
                TAEvent_start();                     //本阶段显示文本
                TAEvent_process();
                this.InitCommint(newBieGuild_step);
                break;
            case NewBieGuild.Step_6:
                break;
            case NewBieGuild.Step_7:
                TAEvent_finish();
                newBieGuild_step = NewBieGuild.Step_8;
                TAEvent_start();                      //本阶段隐藏新手UI  显示停车场UI
                playerZoo = GlobalDataManager.GetInstance().playerData.playerZoo;
                if (GlobalDataManager.GetInstance().playerData.GetParkingCenterDataIDIndexOfDataIdx().parkingEnterCarSpawnLevel >= 5)
                {
                    OnClickDialogBoxButton();
                    DestroyEffectChild();
                }
                else
                {
                    //this.SetSceneAnimateGameObject(procedure);//添加场景UI特效
                    DelayedHide();                      //隐藏新手引导界面
                    TAEvent_process();
                    PageMgr.ShowPage<UIParkPage>();     //显示停车场UI
                }
                break;
            case NewBieGuild.Step_8:
                TAEvent_finish();
                newBieGuild_step = NewBieGuild.Step_9;
                TAEvent_start();                    //本阶段显示文本  并取消游戏暂停  开启相机跟随
                this.InitCommint(newBieGuild_step);                 //文本刷新
                TAEvent_process();
                GameManager.GetInstance().Pause(false);
                float timeCount = 0.1f;
                DOTween.To(() => timeCount, a => timeCount = a, 0.1f, 0.1f).OnComplete(new TweenCallback(SetCameraToFollow));             //延时设置相机跟随

                break;
            case NewBieGuild.Step_9:
                TAEvent_finish();
                newBieGuild_step = NewBieGuild.Step_10;
                TAEvent_start();
                TAEvent_process();
                DelayedHide();                      //隐藏新手引导界面
                break;
            case NewBieGuild.Step_10:
                TAEvent_finish();
                newBieGuild_step = NewBieGuild.Step_11;
                TAEvent_start();                     //本阶段显示新手引导  游戏暂停
                TAEvent_process();
                this.InitCommint(newBieGuild_step);                 //文本刷新
                GameManager.GetInstance().Pause(true);
                break;
            case NewBieGuild.Step_11:
                TAEvent_finish();
                newBieGuild_step = NewBieGuild.Step_12;
                TAEvent_start();
                TAEvent_process();
                playerZoo = GlobalDataManager.GetInstance().playerData.playerZoo;
                if (GlobalDataManager.GetInstance().playerData.GetEntryDateDataIDIndexOfDataIdx().entryTicketsLevel > 4)
                {
                    OnClickDialogBoxButton();
                    DestroyEffectChild();
                }
                else
                {
                    DelayedHide();                      //隐藏新手引导界面
                    this.SetSceneAnimateGameObject(newBieGuild_step);//添加场景UI特效
                }
                break;
            case NewBieGuild.Step_12:
                TAEvent_finish((int)newBieGuild_step + 1);
                newBieGuild_step = NewBieGuild.Step_14;
                TAEvent_start();                   //本阶段显示新手引导  游戏运行
                this.InitCommint(newBieGuild_step);                 //文本刷新
                TAEvent_process();
                GameManager.GetInstance().Pause(false); //取消游戏暂停
                SetCameraToFollow();
                break;
            case NewBieGuild.Step_13:
                
                break;
            case NewBieGuild.Step_14:
                TAEvent_finish();
                newBieGuild_step = NewBieGuild.Step_15;
                TAEvent_start();                  //本阶段隐藏新手引导  游戏运行 等待游戏购票完成
                DelayedHide();                      //隐藏新手引导界面
                TAEvent_process();
                break;
            case NewBieGuild.Step_15:
                TAEvent_finish();
                newBieGuild_step = NewBieGuild.Step_16;
                TAEvent_start();                     //本阶段显示新手引导  游戏运行
                this.InitCommint(newBieGuild_step);                 //文本刷新
                TAEvent_process();
                break;
            case NewBieGuild.Step_16:
                TAEvent_finish();
                newBieGuild_step = NewBieGuild.Step_17;
                TAEvent_start();                  //本阶段隐藏新手引导  游戏运行  监听到达观光
                DelayedHide();                      //隐藏新手引导界面
                TAEvent_process();
                break;
            case NewBieGuild.Step_17:
                TAEvent_finish();

                newBieGuild_step = NewBieGuild.Step_18;
                TAEvent_start();                   //本阶段显示新手引导  游戏暂停  
                this.InitCommint(newBieGuild_step);                 //文本刷新
                TAEvent_process();
                GameManager.GetInstance().Pause(true); //游戏暂停
                break;
            case NewBieGuild.Step_18:
                TAEvent_finish();

                newBieGuild_step = NewBieGuild.Step_19;
                TAEvent_start();                  //本阶段显示新手引导    
                this.InitCommint(newBieGuild_step);                 //文本刷新
                TAEvent_process();
                break;
            case NewBieGuild.Step_19:
                TAEvent_finish();
                newBieGuild_step = NewBieGuild.Step_20;
                TAEvent_start();                  //本阶段隐藏新手引导  动物栏场景动画  
                DelayedHide();                      //隐藏新手引导界面
                TAEvent_process();

                if (GlobalDataManager.GetInstance().playerData.GetLittleZooModuleData(1001).littleZooTicketsLevel >= 5)
                {
                    OnClickDialogBoxButton();
                    DestroyEffectChild();
                }
                else
                {
                    DelayedHide();                      //隐藏新手引导界面
                    this.SetSceneAnimateGameObject(newBieGuild_step);//添加场景UI特效
                }
                break;
            case NewBieGuild.Step_20:
                //TAEvent_finish(-2);
                //TAEvent_start(-1);
                //GlobalDataManager.GetInstance().playerData.playerZoo.isGuide = false;

                newBieGuild_step = NewBieGuild.Step_23;
                TAEvent_finish((int)newBieGuild_step -1);
                TAEvent_start();                   //本阶段显示新手引导  游戏继续  
                this.InitCommint(newBieGuild_step);                 //文本刷新
                TAEvent_process();
                GameManager.GetInstance().Pause(false);
                SetCameraToFollow();                //设置相机跟随
                break;
            
            case NewBieGuild.Step_23:
                TAEvent_finish();

                newBieGuild_step = NewBieGuild.Step_24;
                TAEvent_start();                    //本阶段显示新手引导  
                TAEvent_process();
                this.InitCommint(newBieGuild_step);                 //文本刷新

                break;
            case NewBieGuild.Step_24:
                TAEvent_finish();
                newBieGuild_step = NewBieGuild.Step_25;
                TAEvent_start();                   //本阶段显示新手引导   
                this.InitCommint(newBieGuild_step);                 //文本刷新
                TAEvent_process();
                break;
            case NewBieGuild.Step_25:
                TAEvent_finish();
                newBieGuild_step = NewBieGuild.Step_26;
                TAEvent_start();                    //本阶段隐藏新手引导  游戏继续  
                TAEvent_process();
                SetNewGuideOver();
                TAEvent_finish();
                break;
            case NewBieGuild.Step_26:
                break;
            default:
                break;
        }



       

    }

    /// <summary>
    /// 玩家打点事件
    /// </summary>
    public void TAEvent_start(int step=0)
    {
        if (step ==0)
        {
            step = (int)newBieGuild_step;
        }

    }
    /// <summary>
    /// 玩家打点事件
    /// </summary>
    public void TAEvent_process(int step=0)
    {
        if (step == 0)
        {
            step = (int)newBieGuild_step;
        }
    }
    
    /// <summary>
    /// 玩家打点事件
    /// </summary>
    public void TAEvent_finish(int step=0)
    {
        if (step == 0)
        {
            step = (int)newBieGuild_step;
        }
    }

    /// <summary>
    /// 设置相机跟随
    /// </summary>
    public void SetCameraToFollow()
    {
        if (entity != null)
        {
            TraceCamera.GetInstance().BeginTrace(entity.GetTrans());
        }
        else
        {   //若为空  等待下一秒
            float timeCount = 0.1f;
            DOTween.To(() => timeCount, a => timeCount = a, 0.1f, 0.1f).OnComplete(new TweenCallback(SetCameraToFollow));
        }
    }

    /// <summary>
    /// 设置点击场景Ui后的相机偏移
    /// </summary>
    /// <param name="transform"></param>
    public void SetCameraOnClickScene(Transform transform)
    {
        GameObject gameObject = new GameObject();
        //gameObject.transform.position = transform.position + new Vector3(-40, -40, 50);
        TraceCamera.GetInstance().FinishTrace();
        ZooCamera.GetInstance().PointAtScreenUpCenter(transform.position);

    }

    /// <summary>
    /// 控件显示（无参数）
    /// </summary>
    private void InitCommint(NewBieGuild isStep)
    {
        dialogText.text = this.AcquireData((int)isStep);
        dialogText.GetComponent<TyperTest>().TyperEffect();
        if (isStep == NewBieGuild.Step_1)
        {
            dialogText.text = this.AcquireData((int)isStep);
        }
    }



    /// <summary>
    /// 通过步骤的ID去获取对应的文本并返回
    /// </summary>
    /// <param name="number">对应的ID</param>
    /// <returns></returns>
    private string AcquireData(int number)
    {
        //LogWarp.LogError("测试：  当前需要读的文本步骤为 " + number);
        string string1 = Config.guideConfig.getInstace().getCell(number).guidetext;
        ///再从translate语言表里读取里读取
        string string2 = GlobalDataManager.GetInstance().i18n.GetL10N(string1);
        return string2;
    }
    /// <summary>
    /// 延时隐藏本UI，防止穿透
    /// </summary>
    private void DelayedHide()
    {
        dialogBoxButton.enabled = false;
        uibg.enabled = false;
        float timeCount = 0.1f;
        DOTween.To(() => timeCount, a => timeCount = a, 0.1f, 0.1f).OnComplete(new TweenCallback(Hide));
    }
    /// <summary>
    /// 给场景中添加特效
    /// </summary>
    public void SetSceneAnimateGameObject(NewBieGuild step)
    {
        DestroyEffectChild();
        switch (step)
        {
            case NewBieGuild.Step_5:
                effectNode = GameObject.Find(Config.globalConfig.getInstace().GuideParking).transform;
                break;
            case NewBieGuild.Step_12:
                effectNode = GameObject.Find(Config.globalConfig.getInstace().GuideTicket).transform;
                break;
            case NewBieGuild.Step_20:
                effectNode = GameObject.Find(Config.globalConfig.getInstace().GuideBuild).transform;
                break;
            default:
                break;
        }
        Transform trans = effectNode.Find("Fx_Ui_Hand");
        if (trans!=null)
        {
            trans.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 清除节点下的特效
    /// </summary>
    public void DestroyEffectChild()
    {
        /*  清除场景特效  */
        if (effectNode != null)
        {
            Transform trans = effectNode.Find("Fx_Ui_Hand");
            if (trans != null)
            {
                trans.gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 设置新手引导结束
    /// </summary>
    private void SetNewGuideOver()
    {
        TraceCamera.GetInstance().FinishTrace();
        GlobalDataManager.GetInstance().playerData.playerZoo.isGuide = false;
        //关闭页面
        float timeCount = 0.1f;
        DOTween.To(() => timeCount, a => timeCount = a, 0.1f, 0.1f).OnComplete(new TweenCallback(Hide));

        Image image = transform.Find("Image").GetComponent<Image>();
        image.DOFade(80, 200);//透明度改变
        MessageString.Send((int)GameMessageDefine.UIMessage_ActiveButShowPart, "UIMainPage");
        PageMgr.allPages.TryGetValue("UIMainPage",out UIPage uIPage);
        var mainPage = uIPage as UIMainPage;
        mainPage.ButtonShowCountDown();

    }
}
