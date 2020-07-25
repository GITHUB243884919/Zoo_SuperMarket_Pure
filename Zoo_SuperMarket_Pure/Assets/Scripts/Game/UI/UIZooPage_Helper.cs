using DG.Tweening;
using Game;
using Game.GlobalData;
using Game.MessageCenter;
using UFrame.Logger;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
//using System.Resources;
using UFrame;
using UFrame.MessageCenter;
using UFrame.MiniGame;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public partial class UIZooPage : UIPage
{
    /// <summary>
    /// 01 动物升级—单点
    /// </summary>
    void BuyPlayerAnimal01(string str)
    {
        int animalID = animalCellID[0];

        if (!JudgePressAnimal(animalID))
        {
            LogWarp.LogError("不能升级");
            return;
        }

        SendSetAnimalLevelMessageManager(animalID);  //发送升级消息
    }
    /// <summary>
    /// 02 动物升级—单点
    /// </summary>
    void BuyPlayerAnimal02(string str)
    {
        int animalID = animalCellID[1];

        if (!JudgePressAnimal(animalID))
        {
            LogWarp.LogError("不能升级");
            return;
        }

        SendSetAnimalLevelMessageManager(animalID);  //发送升级消息

    }
    /// <summary>
    ///  03 动物升级—单点
    /// </summary>
    void BuyPlayerAnimal03(string str)
    {
        int animalID = animalCellID[2];

        if (!JudgePressAnimal(animalID))
        {
            LogWarp.LogError("不能升级");
            return;
        }

        SendSetAnimalLevelMessageManager(animalID);  //发送升级消息

    }
    /// <summary>
    ///  04 动物升级—单点
    /// </summary>
    void BuyPlayerAnimal04(string str)
    {
        int animalID = animalCellID[3];

        if (!JudgePressAnimal(animalID))
        {
            LogWarp.LogError("不能升级");
            return;
        }

        SendSetAnimalLevelMessageManager(animalID);  //发送升级消息

    }
    /// <summary>
    ///  05 动物升级—单点
    /// </summary>
    void BuyPlayerAnimal05(string str)
    {
        int animalID = animalCellID[4];

        if (!JudgePressAnimal(animalID))
        {
            return;
        }
        SendSetAnimalLevelMessageManager(animalID);  //发送升级消息

    }

    /// <summary>
    /// 长按按钮回调事件
    /// </summary>
    protected void OnLongPress_Tickets()
    {
        if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide)
        {
            return;
        }
        isLongPress = true;//进入长按状态
        
        if (JudgePressButton())
        {
            SendSetZooTicketsLevelMessageManager();  //发送升级消息
        }
    }
    protected void OnLongPress_VisitorSeat()
    {
        
        if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide)
        {
            return;
        }
        isLongPress = true;//进入长按状态
        
        if (JudgePressButton01())
        {
            SendSetZooVisitorSeatLevelMessageManager();  //发送升级消息
        }

    }
    protected void OnLongPress_VisitorSpawn()
    {
       
        if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide)
        {
            return;
        }
        isLongPress = true;//进入长按状态
       
        if (JudgePressButton02())
        {
            SendSetZooVisitorSpawnLevelMessageManager();  //发送升级消息
        }
    }

    /// <summary>
    /// 01动物升级长按按钮回调事件
    /// </summary>
    protected void OnLongPressBuyAnimal01()
    {
        
        isLongPressBuyAnimal = true;//进入长按状态
        if (isGetCoin)
        {
            SendSetAnimalLevelMessageManager(animalCellID[0]);  //发送升级消息
            isGetCoin = false;
        }
    }
    /// <summary>
    /// 02动物升级长按按钮回调事件
    /// </summary>
    protected void OnLongPressBuyAnimal02()
    {
        isLongPressBuyAnimal = true;//进入长按状态
        if (isGetCoin)
        {
            SendSetAnimalLevelMessageManager(animalCellID[1]);  //发送升级消息
            isGetCoin = false;
        }
    }
    /// <summary>
    /// 03动物升级长按按钮回调事件
    /// </summary>
    protected void OnLongPressBuyAnimal03()
    {
        isLongPressBuyAnimal = true;//进入长按状态
        if (isGetCoin)
        {
            SendSetAnimalLevelMessageManager(animalCellID[2]);  //发送升级消息
            isGetCoin = false;
        }
    }
    /// <summary>
    /// 04动物升级长按按钮回调事件
    /// </summary>
    protected void OnLongPressBuyAnimal04()
    {
        isLongPressBuyAnimal = true;//进入长按状态
        if (isGetCoin)
        {
            SendSetAnimalLevelMessageManager(animalCellID[3]);  //发送升级消息
            isGetCoin = false;
        }
    }
    /// <summary>
    /// 05动物升级长按按钮回调事件
    /// </summary>
    protected void OnLongPressBuyAnimal05()
    {
        isLongPressBuyAnimal = true;//进入长按状态
        //LogWarp.LogError("测试             长按  按钮         ");
        if (isGetCoin)
        {
            SendSetAnimalLevelMessageManager(animalCellID[4]);  //发送升级消息
            isGetCoin = false;
        }
    }

    /// <summary>
    /// 动物购买离开长按按钮回调事件
    /// </summary>
    private void OnReleaseBuyAnimal()
    {
        isLongPressBuyAnimal = false;
        isGetCoin = true;
    }
    /// <summary>
    /// 离开长按按钮回调事件
    /// </summary>
    private void OnRelease_VisitorSeat()
    {
        isLongPress = false;
        visitorSeat_Button.GetComponent<RepeatButton>().isPointerDown = false;
        isGetCoin = true;
    }
    private void OnRelease_Tickets()
    {
        isLongPress = false;
        tickets_Button.GetComponent<RepeatButton>().isPointerDown = false;
    }
    private void OnRelease_VisitorSpawn()
    {
        isLongPress = false;
        visitorSpawn_Button.GetComponent<RepeatButton>().isPointerDown = false;
        isGetCoin = true;
    }

    /// <summary>
    /// 点击升级按钮事件
    /// </summary>
    private void OnClickUpGrade_Tickets(string name)
    {
        if (!JudgePressButton())
        {
            return;
        }
        if (isLongPress)
        {
            return;
        }
        /*  新手引导  */
        if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide == true)
        {
            if (littleZooTicketsLevel >= 5)
            {
                effectNode.gameObject.SetActive(false);
                return;
            }
        }
        SendSetZooTicketsLevelMessageManager();
        string btnSoundPath = Config.globalConfig.getInstace().BuildUpButtonMusic;
        SoundManager.GetInstance().PlaySound(btnSoundPath);

    }
    private void OnClickUpGrade_VisitorSeat(string name)
    {
        //isLongPress为true则是长按状态，单点关闭  返回
        if (!JudgePressButton01())
        {
            return;
        }
        if (isLongPress)
        {
            return;
        }

        SendSetZooVisitorSeatLevelMessageManager();

        string btnSoundPath = Config.globalConfig.getInstace().BuildUpButtonMusic;
        SoundManager.GetInstance().PlaySound(btnSoundPath);
    }
    private void OnClickUpGrade_VisitorSpawn(string name)
    {
        //isLongPress为true则是长按状态，单点关闭  返回
        if (!JudgePressButton02())
        {
            return;
        }
        if (isLongPress)
        {
            return;
        }
        SendSetZooVisitorSpawnLevelMessageManager();

        string btnSoundPath = Config.globalConfig.getInstace().BuildUpButtonMusic;
        SoundManager.GetInstance().PlaySound(btnSoundPath);
    }

    /// <summary>
    /// 动物栏是否可以升级
    /// </summary>
    /// <returns></returns>
    private bool JudgePressButton()
    {
        //第一个  是否扣钱成功   第二  判断是否可以升级
        if (isGetCoin && SetGradeBool_Tickets())
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 动物栏观光点是否可以升级
    /// </summary>
    /// <returns></returns>
    private bool JudgePressButton01()
    {
        //第一个  是否扣钱成功   第二  判断是否可以升级
        if (isGetCoin && SetGradeBool_VisitorSeat())
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// 动物栏观光流量是否可以升级
    /// </summary>
    /// <returns></returns>
    private bool JudgePressButton02()
    {
        //第一个  是否扣钱成功   第二  判断是否可以升级
        if (isGetCoin && SetGradeBool_VisitorSpawn())
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 动物购买是否可以升级
    /// </summary>
    /// <returns></returns>
    private bool JudgePressAnimal(int animalID)
    {
        //第一个  是否扣钱成功   第二  判断是否可以升级
        if (isGetCoin && SetBuyAnimalGradeBool(animalID))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 切换选中状态的图片
    /// </summary>
    /// <param name="button"></param>
    void SetSwitchCheckButtonImage(Button button)
    {
        Transform text_Butoon = button.transform.Find("Text");
        Transform image_button01 = button.transform.Find("ButtonBg_1");
        Transform image_button02 = button.transform.Find("ButtonBg_2");
        Transform image_button03 = button.transform.Find("Image");

        text_Butoon.gameObject.SetActive(true);
        image_button01.gameObject.SetActive(false);
        image_button02.gameObject.SetActive(true);
        image_button03.gameObject.SetActive(false);
    }
    /// <summary>
    /// 切换非选中状态的图片
    /// </summary>
    /// <param name="button"></param>
    void SetSwitchOffCheckButtonImage(Button button)
    {
        Transform text_Butoon = button.transform.Find("Text");
        Transform image_button01 = button.transform.Find("ButtonBg_1");
        Transform image_button02 = button.transform.Find("ButtonBg_2");
        Transform image_button03 = button.transform.Find("Image");

        text_Butoon.gameObject.SetActive(false);
        image_button01.gameObject.SetActive(true);
        image_button02.gameObject.SetActive(false);
        image_button03.gameObject.SetActive(true);

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
