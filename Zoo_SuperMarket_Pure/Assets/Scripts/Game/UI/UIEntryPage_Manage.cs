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

public partial class UIEntryPage : UIPage
{
    #region UI页面刷新
    /// <summary>
    /// 初始化属性数值
    /// </summary>
    private void InitData()
    {
        playerData = GlobalDataManager.GetInstance().playerData;
        sortGateIDs = GlobalDataManager.GetInstance().logicTableEntryGate.GetSortGateIDs(playerData.playerZoo.currSceneID);
        //获取玩家出口等级
        entryTicketsLevel = playerData.GetEntryDateDataIDIndexOfDataIdx().entryTicketsLevel;

        //获取玩家现有金币
        coinVal = playerData.playerZoo.playerCoin.GetCoinByScene(playerData.playerZoo.currSceneID).coinBigInt;
        consumeCoins = EntryGateModule.GetUpGradeConsumption(entryTicketsLevel);//下一级需要的金钱

        ticketCell = Config.ticketConfig.getInstace().getCell(sortGateIDs[0]);
        int idx = PlayerDataModule.FindLevelRangIndex(ticketCell.lvshage, entryTicketsLevel);
        maxGrade = ticketCell.lvshage[idx];
        oldMaxGrade = ticketCell.lvshage[idx - 1];
        entryMaxGrade = ticketCell.lvmax;
        starLevelReached = PlayerDataModule.FindLevelRangIndex01(Config.ticketConfig.getInstace().getCell(sortGateIDs[0]).lvshage, entryTicketsLevel);
        if (entryTicketsLevel >= entryMaxGrade)
        {
            starLevelReached = PlayerDataModule.FindLevelRangIndex01(Config.ticketConfig.getInstace().getCell(sortGateIDs[0]).lvshage, entryTicketsLevel);
        }
        entryGateList = GlobalDataManager.GetInstance().playerData.GetEntryDateDataIDIndexOfDataIdx().entryGateList;

        InitCompent();
    }
    /// <summary>
    /// 控件显示赋值
    /// </summary>
    private void InitCompent()
    {
        InitEveryCompent();
        if (maxGrade >= entryMaxGrade)
        {
            maxGrade = entryMaxGrade;
        }
        lvText.text = string.Format(GetL10NString("Ui_Text_2"), playerData.GetEntryDateDataIDIndexOfDataIdx().entryTicketsLevel.ToString());            //等级text
        scoreNumTest.text = starLevelReached + "/" + Config.ticketConfig.getInstace().getCell(sortGateIDs[0]).starsum;
        gradeSlider.value = AddPercentage(entryTicketsLevel - oldMaxGrade, maxGrade - oldMaxGrade);
        gradeText.text = entryTicketsLevel.ToString() + "/" + maxGrade.ToString();  //最大等级上限
        Config.itemCell itemCell = GradeSliderAwardImage();
        gradeSlider_Image.sprite = ResourceManager.LoadSpriteFromPrefab(itemCell.icon);
        gradeSlider_Text.text = MinerBigInt.ToDisplay(itemCell.itemval);

        if (playerData.GetEntryDateDataIDIndexOfDataIdx().entryTicketsLevel >= entryMaxGrade)
        {
            lvText.text = string.Format(GetL10NString("Ui_Text_2"), GetL10NString("Ui_Text_126")); ;
            gradeSlider.value = 1;
            gradeText.text = GetL10NString("Ui_Text_126");  //最大等级上限
            gradeSlider_IconBg.gameObject.SetActive(false);
            scoreNumTest.text = GetL10NString("Ui_Text_126");

        }
        else
        {
            gradeSlider_IconBg.gameObject.SetActive(true);
        }

    }
    /// <summary>
    /// 根据数据源显示正确的UI
    /// </summary>
    private void InitEveryCompent()
    {
        string path = "UIFerryCar_LvUp/ParameterGroup/Parameter/ScorllView/AnimalGroup/{0}/{1}";

        float width = allEntryCell.GetComponent<RectTransform>().sizeDelta.x;
        int count = playerData.GetEntryDateDataIDIndexOfDataIdx().entryGateList.Count;
        if (count == 8)
        {
            count = count - 1;
        }
        allEntryCell.GetComponent<RectTransform>().sizeDelta = new UnityEngine.Vector2(width, 220f + 133f * count);
        for (int i = 0; i < allEntryCell.childCount; i++)
        {
            /*查找所有的用的上的对象*/
            string name = allEntryCell.GetChild(i).name;
            Text nameText = RegistCompent<Text>(string.Format(path, name, "Text_1"));
            Image imageIcon = RegistCompent<Image>(string.Format(path, name, "Icon"));
            Text Text_2 = RegistCompent<Text>(string.Format(path, name, "TextAll/Text_2"));
            Text Text_3 = RegistCompent<Text>(string.Format(path, name, "TextAll/Text_3"));
            Text LvText = RegistCompent<Text>(string.Format(path, name, "level/LvText"));
            Text serialText = RegistCompent<Text>(string.Format(path, name, "ID/LvText"));
            Button button = RegistCompent<Button>(string.Format(path, name, "Button"));
            Text button_NeedGoldNum = button.transform.Find("NeedGoldNum").GetComponent<Text>();
            Text button_ButtonLvUpText = button.transform.Find("ButtonLvUpText").GetComponent<Text>();
            button.gameObject.SetActive(false);
            Image button_GoldIcon = button.transform.Find("GoldIcon").GetComponent<Image>();
            button_GoldIcon.sprite = sprite;
            UIEntryCell_Enum iEntryCell_Enum = (UIEntryCell_Enum)(i - 1);

            if (i == 0)
            {
                /* 第一个cell的  门票价格 显示 */
                nameText.text = GetL10NString("Ui_Text_13");
                Text_2.text = MinerBigInt.ToDisplay(EntryGateModule.GetEntryPrice(entryTicketsLevel, playerData.playerZoo.currSceneID, true).ToString());
                Text_3.text = "+" + MinerBigInt.ToDisplay(EntryGateModule.GetEntryPrice_Add(entryTicketsLevel, 1, playerData.playerZoo.currSceneID).ToString());
                LvText.text = entryTicketsLevel.ToString();
                allEntryCell.GetChild(i).Find("ID").gameObject.SetActive(false);
                Image iconImage = RegistCompent<Image>(string.Format(path, name, "Icon"));
                iconImage.sprite = sprite;
                Image goldIconImage = RegistCompent<Image>(string.Format(path, name, "GoldIcon"));
                goldIconImage.sprite = sprite;
                button.gameObject.SetActive(true);
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(delegate
                {
                    OnLongPressButton_New(iEntryCell_Enum);
                    BtnScaleAnim(button.gameObject, 1.1f, 0.95f);
                });
                if (button.gameObject.GetComponent<RepeatButton>() == null && GlobalDataManager.GetInstance().playerData.playerZoo.isGuide == false)
                {
                    button.gameObject.AddComponent<RepeatButton>();//需要长按
                    button.GetComponent<RepeatButton>().onPress.AddListener(delegate
                    {
                        OnLongPressButton_New(iEntryCell_Enum);
                    });//按下。频繁的调用
                    button.GetComponent<RepeatButton>().onRelease.AddListener(OnReleaseButton);//抬起，调用一次
                }

                button_NeedGoldNum.text = MinerBigInt.ToDisplay(EntryGateModule.GetUpGradeConsumption(entryTicketsLevel).ToString());
                button_ButtonLvUpText.text = GetL10NString("Ui_Text_7");

                //判断是否钱够
                var coin = EntryGateModule.GetUpGradeConsumption(entryTicketsLevel);
                if (coin > coinVal)
                {
                    SwitchButtonUnClickable(button, false);
                }
                else
                {
                    SwitchButtonUnClickable(button, true);
                }

                //判断是否是最大值
                var max_Level = Config.ticketConfig.getInstace().getCell(sortGateIDs[i]).lvmax;
                if (entryTicketsLevel >= max_Level)
                {
                    button_ButtonLvUpText.text = GetL10NString("Ui_Text_46");
                    button_NeedGoldNum.gameObject.SetActive(false);
                    Text_3.gameObject.SetActive(false);
                    SwitchButtonUnClickable(button, false);
                }
                else
                {
                    button_NeedGoldNum.gameObject.SetActive(true);
                    Text_3.gameObject.SetActive(true);
                }
                if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide && effectNode == null)
                {
                    //停车场停车位按钮处显示小手点击动画
                    effectNode = button.transform.Find("effectNode");
                    Transform trans = null;
                    trans = ResourceManager.GetInstance().LoadGameObject(Config.globalConfig.getInstace().GuideUiClickEffect).transform;
                    trans.SetParent(effectNode, true);
                    trans.localScale = UnityEngine.Vector3.one;
                    trans.position = effectNode.position;
                    trans.localPosition = new UnityEngine.Vector3(
                        trans.localPosition.x,
                        trans.localPosition.y + 4,
                        trans.localPosition.z);
                }
            }
            else
            {
                if (playerData.GetEntryDateDataIDIndexOfDataIdx().entryGateList.Count + 1 > i)
                {
                    /* 正常单售票口开启后的ui显示 */
                    nameText.text = GetL10NString("Ui_Text_14");
                    var entryGateData = playerData.GetEntryDateDataIDIndexOfDataIdx().entryGateList[i - 1];
                    Text_2.text = EntryGateModule.GetCheckinSpeed(entryGateData.entryID, entryGateData.level).ToString("F2") + GetL10NString("Ui_Text_67");
                    Text_3.text = "+" + EntryGateModule.GetCheckinSpeed(entryGateData.entryID, entryGateData.level, 1).ToString("f2");
                    Text_2.gameObject.SetActive(true);
                    Text_3.gameObject.SetActive(true);
                    LvText.text = entryGateData.level.ToString();
                    serialText.text = i.ToString();
                    string iconPath = Config.globalConfig.getInstace().LvUpTicketIcon;
                    imageIcon.sprite = ResourceManager.LoadSpriteFromPrefab(iconPath);
                    button.gameObject.SetActive(true);
                    button.onClick.RemoveAllListeners();
                    int isID = i - 1;
                    button.onClick.AddListener(delegate
                    {
                        BtnScaleAnim(button.gameObject, 1.1f, 0.95f);
                        OnLongPressButton_New(iEntryCell_Enum);
                    });
                    if (button.gameObject.GetComponent<RepeatButton>() == null )
                    {
                        button.gameObject.AddComponent<RepeatButton>();//需要长按
                        button.GetComponent<RepeatButton>().onPress.AddListener(delegate
                        {
                            OnLongPressButton_New(iEntryCell_Enum);
                        });//按下。频繁的调用
                        button.GetComponent<RepeatButton>().onRelease.AddListener(OnReleaseButton);//抬起，调用一次
                    }
                    button_NeedGoldNum.text = MinerBigInt.ToDisplay(EntryGateModule.GetUpGradeCheckinSpeedConsumption(entryGateData.entryID, entryGateData.level).ToString());
                    button_ButtonLvUpText.text = GetL10NString("Ui_Text_7");

                    //判断是否钱够
                    var coin = EntryGateModule.GetUpGradeCheckinSpeedConsumption(entryGateData.entryID, entryGateData.level);
                    //判断是否是最大值
                    var max_Level = Config.ticketConfig.getInstace().getCell(sortGateIDs[i - 1]).speedmaxlv;
                    if (entryGateData.level >= max_Level)
                    {
                        button_ButtonLvUpText.text = GetL10NString("Ui_Text_46");
                        button_NeedGoldNum.gameObject.SetActive(false);
                        Text_3.gameObject.SetActive(false);
                        SwitchButtonUnClickable(button, false);
                    }
                    else
                    {
                        button_NeedGoldNum.gameObject.SetActive(true);
                        Text_3.gameObject.SetActive(true);
                        SwitchButtonUnClickable(button, true);
                        if (coin > coinVal)
                        {
                            SwitchButtonUnClickable(button, false);
                        }
                        else
                        {
                            SwitchButtonUnClickable(button, true);
                            if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide)
                            {
                                SwitchButtonUnClickable(button, false);
                            }
                        }
                    }
                }
                else if (playerData.GetEntryDateDataIDIndexOfDataIdx().entryGateList.Count + 1 == i)
                {
                    /**** 下一个待开的售票口ui显示 ****/
                    nameText.text = GetL10NString("Ui_Text_45");
                    Text_2.gameObject.SetActive(false);
                    Text_3.gameObject.SetActive(false);
                    LvText.text = "0";
                    serialText.text = i.ToString();

                    string iconPath = Config.globalConfig.getInstace().AddTicketIcon;
                    imageIcon.sprite = ResourceManager.LoadSpriteFromPrefab(iconPath);

                    button.gameObject.SetActive(true);
                    button.onClick.RemoveAllListeners();
                    /*不需要长按*/
                    button.onClick.AddListener(delegate
                    {
                        OnClickOpenNewEntry();
                        BtnScaleAnim(button.gameObject, 1.1f, 0.95f);
                    });
                    var price = Config.ticketConfig.getInstace().getCell(sortGateIDs[i - 1]).number;
                    button_NeedGoldNum.text = MinerBigInt.ToDisplay(BigInteger.Parse(price));
                    button_NeedGoldNum.gameObject.SetActive(true);
                    button_ButtonLvUpText.text = GetL10NString("Ui_Text_68");
                    BigInteger coin = BigInteger.Parse(price);
                    //判断是否钱够
                    if (coin > coinVal)
                    {
                        SwitchButtonUnClickable(button, false);
                    }
                    else
                    {
                        SwitchButtonUnClickable(button, true);
                        if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide)
                        {
                            SwitchButtonUnClickable(button, false);
                        }
                    }
                }
                else if (playerData.GetEntryDateDataIDIndexOfDataIdx().entryGateList.Count + 1 < i)
                { /**** 不在列表中的隐藏 ****/
                    allEntryCell.GetChild(i).gameObject.SetActive(false);
                    nameText.text = "&&&&&&&";
                    Text_2.text = "AAAAAA";
                    Text_3.text = "BBB";
                }
            }
        }
    }

    /// <summary>
    /// 恢复新手阶段对按钮的限制
    /// </summary>
    private void InitNewGuideEveryCompent()
    {
        string path = "UIFerryCar_LvUp/ParameterGroup/Parameter/ScorllView/AnimalGroup/{0}/{1}";
        for (int i = 0; i < allEntryCell.childCount; i++)
        {
            /*查找所有的用的上的对象*/
            string name = allEntryCell.GetChild(i).name;
            Button button = RegistCompent<Button>(string.Format(path, name, "Button"));
            button.gameObject.SetActive(false);

            if (i == 0 || i == 1 || i == 2)
            {
                if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide)
                {
                    SwitchButtonUnClickable(button, false);
                }
            }
        }
    }

    /// <summary>
    /// 根据ID显示某一个售票口正常的UI
    /// </summary>
    /// <param name="id"></param>
    private void InitAloneEntryCompent(int id)
    {
        string path = "UIFerryCar_LvUp/ParameterGroup/Parameter/ScorllView/AnimalGroup/{0}/{1}";
        /*查找所有的用的上的对象*/
        string name = allEntryCell.GetChild(id + 1).name;
        Text nameText = RegistCompent<Text>(string.Format(path, name, "Text_1"));
        Image imageIcon = RegistCompent<Image>(string.Format(path, name, "Icon"));
        Text Text_2 = RegistCompent<Text>(string.Format(path, name, "TextAll/Text_2"));
        Text Text_3 = RegistCompent<Text>(string.Format(path, name, "TextAll/Text_3"));
        Text LvText = RegistCompent<Text>(string.Format(path, name, "level/LvText"));
        Text serialText = RegistCompent<Text>(string.Format(path, name, "ID/LvText"));
        Button button = RegistCompent<Button>(string.Format(path, name, "Button"));
        Text button_NeedGoldNum = button.transform.Find("NeedGoldNum").GetComponent<Text>();
        Text button_ButtonLvUpText = button.transform.Find("ButtonLvUpText").GetComponent<Text>();

        /* 正常单售票口开启后的ui显示 */
        nameText.text = GetL10NString("Ui_Text_14");

        //var cell = Config.ticketConfig.getInstace().getCell(entryID);
        Text_2.text = EntryGateModule.GetCheckinSpeed(sortGateIDs[id], playerData.GetEntryDateDataIDIndexOfDataIdx().entryGateList[id].level).ToString("f2") + GetL10NString("Ui_Text_67");
        Text_3.text = "+" + EntryGateModule.GetCheckinSpeed(sortGateIDs[id], playerData.GetEntryDateDataIDIndexOfDataIdx().entryGateList[id].level, 1).ToString("f2");
        LvText.text = playerData.GetEntryDateDataIDIndexOfDataIdx().entryGateList[id].level.ToString();
        serialText.text = (id + 1).ToString();
        var coin = EntryGateModule.GetUpGradeCheckinSpeedConsumption(sortGateIDs[id], playerData.GetEntryDateDataIDIndexOfDataIdx().entryGateList[id].level);
        Text_2.gameObject.SetActive(true);
        Text_3.gameObject.SetActive(true);
        button_NeedGoldNum.text = MinerBigInt.ToDisplay(coin).ToString();
        button_ButtonLvUpText.text = GetL10NString("Ui_Text_7");
        //判断是否是最大值
        var max_Level = Config.ticketConfig.getInstace().getCell(sortGateIDs[id]).speedmaxlv;
        //LogWarp.LogErrorFormat("测试：   entryGateList[entryID].level={0}    max_Level={1}  ", playerData.GetEntryDateDataIDIndexOfDataIdx().entryGateList[id].level, max_Level);
        if (playerData.GetEntryDateDataIDIndexOfDataIdx().entryGateList[id].level >= max_Level)
        {
            button_ButtonLvUpText.text = GetL10NString("Ui_Text_46");
            button_NeedGoldNum.gameObject.SetActive(false);
            Text_3.gameObject.SetActive(false);
            SwitchButtonUnClickable(button, false);
        }
        else
        {
            button_NeedGoldNum.gameObject.SetActive(true);
            Text_3.gameObject.SetActive(true);
            SwitchButtonUnClickable(button, true);
            if (coin > coinVal)
            {
                SwitchButtonUnClickable(button, false);
            }
            else
            {
                SwitchButtonUnClickable(button, true);
            }
        }
    }
    /// <summary>
    /// 根据ID开启某个的售票UI逻辑
    /// </summary>
    /// <param name="entryID"></param>
    private void InitOpenNewEntryCompent(int entryID)
    {
        //LogWarp.LogError("aaaaaaaaaaaaa " + entryID);
        float width = allEntryCell.GetComponent<RectTransform>().sizeDelta.x;
        int cellHeight = playerData.GetEntryDateDataIDIndexOfDataIdx().entryGateList.Count;
        if (entryID == 7)
        {
            cellHeight = playerData.GetEntryDateDataIDIndexOfDataIdx().entryGateList.Count - 1;
            allEntryCell.GetComponent<RectTransform>().sizeDelta = new UnityEngine.Vector2(width, 220f + 133f * cellHeight);
        }
        else
        {
            allEntryCell.GetComponent<RectTransform>().sizeDelta = new UnityEngine.Vector2(width, 220f + 133f * cellHeight);
        }
        string path = "UIFerryCar_LvUp/ParameterGroup/Parameter/ScorllView/AnimalGroup/{0}/{1}";
        entryGateList = playerData.GetEntryDateDataIDIndexOfDataIdx().entryGateList;
        for (int i = 0; i < allEntryCell.childCount; i++)
        {
            if (i < entryGateList.Count + 2)
            {
                allEntryCell.GetChild(i).gameObject.SetActive(true);
            }

            /*查找所有的用的上的对象*/
            string name = allEntryCell.GetChild(i).name;
            Text nameText = RegistCompent<Text>(string.Format(path, name, "Text_1"));
            Image imageIcon = RegistCompent<Image>(string.Format(path, name, "Icon"));
            Text Text_2 = RegistCompent<Text>(string.Format(path, name, "TextAll/Text_2"));
            Text Text_3 = RegistCompent<Text>(string.Format(path, name, "TextAll/Text_3"));
            Text LvText = RegistCompent<Text>(string.Format(path, name, "level/LvText"));
            Text serialText = RegistCompent<Text>(string.Format(path, name, "ID/LvText"));
            Button button = RegistCompent<Button>(string.Format(path, name, "Button"));
            Text button_NeedGoldNum = button.transform.Find("NeedGoldNum").GetComponent<Text>();
            Text button_ButtonLvUpText = button.transform.Find("ButtonLvUpText").GetComponent<Text>();
            //button.gameObject.SetActive(false);
            UIEntryCell_Enum iEntryCell_Enum = (UIEntryCell_Enum)(i - 1);

            if (i == entryID + 1)
            {
                var entryGateData = playerData.GetEntryDateDataIDIndexOfDataIdx().entryGateList[i - 1];

                /* 正常单售票口开启后的ui显示 */
                nameText.text = GetL10NString("Ui_Text_14");
                var cell = Config.ticketConfig.getInstace().getCell(entryGateData.entryID);
                Text_2.text = EntryGateModule.GetCheckinSpeed(entryGateData.entryID, entryGateData.level).ToString("f2") + GetL10NString("Ui_Text_67");
                Text_3.text = "+" + EntryGateModule.GetCheckinSpeed(entryGateData.entryID, entryGateData.level, 1).ToString("f2");
                LvText.text = entryGateData.level.ToString();
                serialText.text = i.ToString();

                Text_2.gameObject.SetActive(true);
                Text_3.gameObject.SetActive(true);
                button.gameObject.SetActive(true);
                button.onClick.RemoveAllListeners();
                int isID = i - 1;
                string iconPath = Config.globalConfig.getInstace().LvUpTicketIcon;
                imageIcon.sprite = ResourceManager.LoadSpriteFromPrefab(iconPath);
                button.onClick.AddListener(delegate
                {
                    OnLongPressButton_New(iEntryCell_Enum);
                    BtnScaleAnim(button.gameObject, 1.1f, 0.95f);
                });
                button_NeedGoldNum.text = MinerBigInt.ToDisplay(EntryGateModule.GetUpGradeCheckinSpeedConsumption(entryGateData.entryID, entryGateData.level).ToString());
                button_ButtonLvUpText.text = GetL10NString("Ui_Text_7");

            }
            else if (i == entryID + 2)
            {
                nameText.text = GetL10NString("Ui_Text_45");
                Text_2.gameObject.SetActive(false);
                Text_3.gameObject.SetActive(false);
                LvText.text = "0";
                serialText.text = i.ToString();

                string iconPath = Config.globalConfig.getInstace().AddTicketIcon;
                imageIcon.sprite = ResourceManager.LoadSpriteFromPrefab(iconPath);

                button.gameObject.SetActive(true);
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(delegate
                {
                    OnClickOpenNewEntry();
                    BtnScaleAnim(button.gameObject, 1.1f, 0.95f);
                });
                var parce = Config.ticketConfig.getInstace().getCell(sortGateIDs[i - 1]).number;
                button_NeedGoldNum.text = MinerBigInt.ToDisplay(BigInteger.Parse(parce));
                button_ButtonLvUpText.text = GetL10NString("Ui_Text_68");
            }
            else if (i > entryID + 2)
            {
                allEntryCell.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    #endregion
    private bool IsOpenEntryDateID(int ID)
    {
        if (playerData.GetEntryDateDataIDIndexOfDataIdx().entryGateList.Count>ID)
        {
            return true;
        }
        return false;
    }
    
    /// <summary>
    /// 长按按钮回调事件
    /// </summary>
    private void OnLongPressButton_New(UIEntryCell_Enum iEntryCell_Enum )
    {
        int idx = (int)iEntryCell_Enum;
        if (JudgePressButton(iEntryCell_Enum))
        {
            switch (iEntryCell_Enum)
            {
                case UIEntryCell_Enum.Tickets:
                    //门票的判断
                    if (SetGradeBool_entryTicketsLevel())
                    {
                        SendSetTicketsLevelMessageManager();  //发送升级消息
                        isGetCoin = false;

                    }
                    break;
                default:
                    if (!IsOpenEntryDateID(idx))
                    {
                        return;
                    }
                    if (SetGradeBool_entryGatasLevel(idx))
                    {
                        var speedmaxlv = Config.ticketConfig.getInstace().getCell(sortGateIDs[idx]).speedmaxlv;
                        if (playerData.GetEntryDateDataIDIndexOfDataIdx().entryGateList[idx].level < speedmaxlv)
                        {
                            int level = playerData.GetEntryDateDataIDIndexOfDataIdx().entryGateList[idx].level;
                            var coin = EntryGateModule.GetUpGradeCheckinSpeedConsumption(sortGateIDs[idx], level);
                            if (coin <= coinVal)
                            {
                                SendEntryGatePureLevelMessageManager(idx);  //发送升级消息
                                isGetCoin = false;
                            }
                        }
                        isGetCoin = false;
                    }

                    break;
            }
        }
        PageMgr.PlayButtonSound();
    }

    /// <summary>
    /// 门票：判断是否可以升级（钱够/等级不超过最大值）
    /// </summary>
    /// <returns></returns>
    private bool SetGradeBool_entryTicketsLevel()
    {
        if (consumeCoins <= coinVal && entryTicketsLevel < entryMaxGrade)
        {
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// 售票口：判断是否可以升级（钱够/等级不超过最大值）
    /// </summary>
    /// <returns></returns>
    private bool SetGradeBool_entryGatasLevel(int idx)
    {
        var entryGateData = playerData.GetEntryDateDataIDIndexOfDataIdx().entryGateList[idx];
        var max_Level = Config.ticketConfig.getInstace().getCell(sortGateIDs[idx]).speedmaxlv;
        var coin = EntryGateModule.GetUpGradeCheckinSpeedConsumption(entryGateData.entryID, entryGateData.level);

        if (coin <= coinVal && entryGateData.level < max_Level)
        {
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// 是否可以升级
    /// </summary>
    /// <returns></returns>
    private bool JudgePressButton(UIEntryCell_Enum iEntryCell_Enum)
    {
        //第一个  是否扣钱成功   第二  判断是否可以升级
        if (isGetCoin)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 离开长按按钮回调事件
    /// </summary>
    private void OnReleaseButton()
    {
    }

    
}
