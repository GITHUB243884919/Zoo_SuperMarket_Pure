using DG.Tweening;
using Game;
using Game.GlobalData;
using Game.MessageCenter;
using System;
using System.Collections;
using System.Collections.Generic;
using UFrame;
using UFrame.Logger;
using UFrame.MiniGame;
using UnityEngine;
using UnityEngine.UI;

public class UIBuildShowPage : UIPage
{
    MultiIntCD multiTickObj;
    public Transform zooBuildShow;
    private GameObject BuildUpEffect;
    private Transform rotationCamera;
    private Transform mainCamera;
    //Text titleText;
    //Text titleText1;
    //Text explainText1;
    //Text explainText;
    Button buttonHide;   //背景按钮点击事件
    Text buttonText;
    //Image Icon;
    Transform effectNode;
    public UIBuildShowPage() : base(UIType.Fixed, UIMode.DoNothing, UICollider.None, UITickedMode.Update)
    {
        uiPath = "UIPrefab/UIBuildShow";
    }
    private void RegistAllCompent()
    {
        GameObject camera = GlobalDataManager.GetInstance().zooGameSceneData.camera;
        //查找选择的相机
        rotationCamera = camera.transform.Find("RotationCamera");
        mainCamera = camera.transform.Find("main_camera");
        buttonHide = AddCompentInChildren<Button>(buttonHide, "UIBg/UIShowGroup/OkButton");
        buttonHide = RegistBtnAndClick("UIBg/UIShowGroup/OkButton", HideUI);
        buttonText = RegistCompent<Text>("UIBg/UIShowGroup/OkButton/ButtonText");
        //GetTransPrefabText(buttonText);

        //新手引导手势组件
        effectNode = RegistCompent<Transform>("UIBg/UIShowGroup/OkButton/effectNode");
        if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide == true)
        {
            Transform trans = null;
            trans = ResourceManager.GetInstance().LoadGameObject(Config.globalConfig.getInstace().GuideUiClickEffect).transform;
            trans.SetParent(effectNode, true);
            trans.localScale = UnityEngine.Vector3.one;
            trans.position = effectNode.position;
            trans.localPosition = new UnityEngine.Vector3(
                trans.localPosition.x,
                trans.localPosition.y+4,
                trans.localPosition.z);
            UIGuidePage uIGuidePage = PageMgr.GetPage<UIGuidePage>();
            if (uIGuidePage == null)
            {
                string e = string.Format("新手引导界面  PageMgr.allPages里 UIGuidePage   为空");
                throw new System.Exception(e);
            }
            uIGuidePage.TAEvent_finish((int)uIGuidePage.newBieGuild_step+1);
            uIGuidePage.TAEvent_start((int)uIGuidePage.newBieGuild_step + 2);
            uIGuidePage.TAEvent_process((int)uIGuidePage.newBieGuild_step + 2);
        }
    }
    public override void Awake(GameObject go)
    {
        base.Awake(go);
        GetTransPrefabAllTextShow(this.transform);
        
    }
    private void InitCompent()
    {
        rotationCamera.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
        BuildUpEffect.SetActive(true);
        BuildUpEffect.transform.SetParent(zooBuildShow, false);
        Config.buildupCell buildUpCell = Config.buildupConfig.getInstace().getCell(m_data.ToString());
        string nameStr = GetL10NString(buildUpCell.buildname);
        string iconPath = buildUpCell.icon;
        string str = GetL10NString("Ui_Text_48");
        //titleText.text = nameStr;
        //explainText.text = str;
        //titleText1.text = nameStr;
        //explainText1.text = str;
        //Icon.sprite = ResourceManager.LoadSpriteFromPrefab(iconPath);
    }
    public override void Refresh()
    {
        base.Refresh();
    }

    public override void Active()
    {
        base.Active();
        RegistAllCompent();
        zooBuildShow = transform.Find("UIBg/Effect");

        if (BuildUpEffect == null)
        {
            BuildUpEffect = ResourceManager.GetInstance().LoadGameObject(Config.globalConfig.getInstace().BuildUpEffect);
        }
        InitCompent();

        if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide == true)
        {
            GameManager.GetInstance().Pause(true); //游戏暂停
            //新手阶段   延时若干秒结束旋转继续下一阶段
            GameManager.GetInstance().StartCoroutine(Wait(Config.globalConfig.getInstace().BuildUpShowTime));

        }
        else
        {
            //Hide();
        }

        //UIZooPage uIZooPage = PageMgr.GetPage<UIZooPage>();
        //if (uIZooPage != null)
        //{
        //    uIZooPage.Hide();
        //}
        MessageString.Send((int)GameMessageDefine.UIMessage_ActiveButHidePart, "UIMainPage");

    }

    IEnumerator Wait(float t)
    {
        yield return new WaitForSeconds(t);//运行到这，暂停5秒
        HideUI("00");
    }

    private void HideUI(string str)
    {
        float timeCount = 0.1f;
        DOTween.To(() => timeCount, a => timeCount = a, 0.1f, 0.2f).OnComplete(new TweenCallback(delegate
        {
            this.isHide();
        }));
    }

    private void isHide()
    {
        DestroyEffectChild();
        
        //PageMgr.ShowPage<UIMainPage>(); 
        MessageString.Send((int)GameMessageDefine.UIMessage_ActiveButShowPart, "UIMainPage");

        if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide != true)
        {
            PageMgr.ShowPage<UIZooPage>(m_data);
            UIInteractive.GetInstance().iPage = new UIZooPage();

            this.Hide();
        }
        else
        {
            this.Hide();
            PageMgr.ShowPage<UIGuidePage>();
            return;
        }
    }

    public override void Hide()
    {
        base.Hide();
        if (rotationCamera!=null)
        {
            rotationCamera.gameObject.SetActive(false);
        }
        if (mainCamera !=null)
        {
            mainCamera.gameObject.SetActive(true);
        }
        BuildUpEffect.SetActive(false);
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
