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
    /// 初始化属性数值  切换动物栏ID时候需要调用
    /// </summary>
    private void InitData()
    {
        if (m_data == null)
        {
            this.Hide();
            return;
        }
        nameID = int.Parse(m_data.ToString());
        buildUpCell = Config.buildupConfig.getInstace().getCell(m_data.ToString());
        animalLvUpLimit = Config.globalConfig.getInstace().AnimalLvUpLimit;
        pricebase = BigInteger.Parse(buildUpCell.pricebase);
        animalMSS15 = GlobalDataManager.GetInstance().playerData.playerZoo.animalMSS15;
        littleZooName = Config.translateConfig.getInstace().getCell(buildUpCell.buildname);//根据动物栏的名字ID去语言表获取动物栏的中文显示
        littleZooTicketsLevel = GlobalDataManager.GetInstance().playerData.GetLittleZooModuleData(nameID).littleZooTicketsLevel;
        littleZooVisitorSeatLevel = GlobalDataManager.GetInstance().playerData.GetLittleZooModuleData(nameID).littleZooVisitorSeatLevel;
        littleZooEnterVisitorSpawnLevel = GlobalDataManager.GetInstance().playerData.GetLittleZooModuleData(nameID).littleZooEnterVisitorSpawnLevel;

        coinVal = playerData.playerZoo.playerCoin.GetCoinByScene(playerData.playerZoo.currSceneID).coinBigInt;//获取玩家现有金币
        ticketsLevelConsumeCoins = LittleZooModule.GetUpGradeConsumption(nameID, littleZooTicketsLevel + 1);//动物栏下一级需要的金钱
        visitorSeatLevelConsumeCoins = LittleZooModule.GetUpGradeVisitorLocationLevelConsumption(nameID, littleZooVisitorSeatLevel + 1);//动物栏下一级需要的金钱
        EnterVisitorSpawnLevelConsumeCoins = LittleZooModule.GetUpGradeEnterVisitorSpawnLevelConsumption(nameID, littleZooEnterVisitorSpawnLevel + 1);//动物栏下一级需要的金钱

        int idx = PlayerDataModule.FindLevelRangIndex(buildUpCell.lvshage, littleZooTicketsLevel);
        maxGrade = buildUpCell.lvshage[idx];
        oldMaxGrade = buildUpCell.lvshage[idx - 1];

        TicketsMaxGrade = Config.buildupConfig.getInstace().getCell(nameID).lvmax;
        if (TicketsMaxGrade == 0)
            TicketsMaxGrade = 1;
        VisitorSeatMaxGrade = Config.buildupConfig.getInstace().getCell(nameID).watchmaxlv;
        EnterVisitorSpawnMaxGrade = Config.buildupConfig.getInstace().getCell(nameID).itemmaxlv;

        starLevelReached = PlayerDataModule.FindLevelRangIndex01(Config.buildupConfig.getInstace().getCell(nameID).lvshage, littleZooTicketsLevel);
        if (littleZooTicketsLevel >= TicketsMaxGrade)
        {
            starLevelReached = PlayerDataModule.FindLevelRangIndex01(Config.buildupConfig.getInstace().getCell(nameID).lvshage, littleZooTicketsLevel);
        }

        if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide == true)
        {
            zooCultivateButton.enabled = false;
            zooKindButton.enabled = false;
        }
        else
        {
            zooCultivateButton.enabled = true;
            zooKindButton.enabled = true;
        }

        var array = buildUpCell.animalid;
        animalCellID.Clear();
        animalCellID = array.OfType<int>().ToList();
    }
    private void InitAnimalData()
    {
        int childCount = animalGroup.transform.childCount;
        animalTransformData.Clear();
        for (int i = 0; i < childCount; i++)
        {
            int num = buildUpCell.animalid[i];
            animalTransformData.Add(num, animalGroup.transform.GetChild(i));
        }
    }
    /// <summary>
    /// 控件显示赋值   并切换按钮图片
    /// </summary>
    private void InitCompent()
    {
        titleText.text = GetL10NString(buildUpCell.buildname);
        lvText.text = string.Format(GetL10NString("Ui_Text_2"), GlobalDataManager.GetInstance().playerData.GetLittleZooModuleData(nameID).littleZooTicketsLevel);
        string iconPath = buildUpCell.icon;
        animalShow.sprite = ResourceManager.LoadSpriteFromPrefab(iconPath);
        scoreNumTest.text = starLevelReached + "/" + Config.buildupConfig.getInstace().getCell(nameID).starsum;
        if (littleZooTicketsLevel >= TicketsMaxGrade)
        {
            scoreNumTest.text = GetL10NString("Ui_Text_126");
            lvText.text = string.Format(GetL10NString("Ui_Text_2"), GetL10NString("Ui_Text_126"));

        }
        //切换子UI显示页面   
        switch (switchButton)
        {
            case "ZooCultivateButton":
                zooCultivateUI.SetActive(false);
                tipsTextAnimalup.gameObject.SetActive(false);
                tipsText.gameObject.SetActive(true);
                zooKindUI.SetActive(true);
                this.InitCompentZoo();
                break;
            case "ZooKindButton":
                zooCultivateUI.SetActive(true);
                tipsTextAnimalup.gameObject.SetActive(true);
                tipsText.gameObject.SetActive(false);
                zooKindUI.SetActive(false);
                this.InitCompentCultivate();
                break;
            default:
                break;
        }
    }
    private void InitCompentZoo()
    {
        if (maxGrade > TicketsMaxGrade)
        {
            maxGrade = TicketsMaxGrade;
        }
            LVUpSlider.value = AddPercentage(littleZooTicketsLevel - oldMaxGrade, maxGrade - oldMaxGrade);
            lVUpSliderText.text = littleZooTicketsLevel.ToString() + "/" + maxGrade.ToString();  //最大等级上限
            tipsText.text = GetL10NString("Ui_Text_8");
            GradeSliderAwardImage();

        if (littleZooTicketsLevel >= TicketsMaxGrade)
        {
            LVUpSlider.value = 1;
            lVUpSliderText.text = GetL10NString("Ui_Text_126");  //最大等级上限
            tipsText.text = GetL10NString("Ui_Text_8");
            GradeSliderAwardImage();
        }

        //LVUpSlider.value = AddPercentage(littleZooTicketsLevel - oldMaxGrade, maxGrade - oldMaxGrade);
        //lVUpSliderText.text = littleZooTicketsLevel.ToString() + "/" + maxGrade.ToString();  //最大等级上限
        //tipsText.text = GetL10NString("Ui_Text_8");
        //GradeSliderAwardImage();


        tickets_Text2.text = MinerBigInt.ToDisplay(LittleZooModule.GetLittleZooPrice(nameID, littleZooTicketsLevel,true).ToString()); ;  //价格标签2
        tickets_Text3.text = "+" + MinerBigInt.ToDisplay(LittleZooModule.GetLittleZooPrice(nameID, littleZooTicketsLevel, 1).ToString());  //价格标签3
        tickets_LvText.text = littleZooTicketsLevel.ToString();

        tickets_Button_NeedGoldText.text = MinerBigInt.ToDisplay(ticketsLevelConsumeCoins.ToString());
        tickets_Button_buttonLvUpText.text = GetL10NString("Ui_Text_7");

        visitorSeat_Text2.text = LittleZooModule.OpenVisitPosNumber(nameID, littleZooVisitorSeatLevel).ToString(); //数量标签2
        visitorSeat_Text3.text = "+" + LittleZooModule.OpenVisitPosNumber(nameID, littleZooVisitorSeatLevel, 1).ToString(); //数量标签3
        visitorSeat_Button_NeedGoldText.text = MinerBigInt.ToDisplay(visitorSeatLevelConsumeCoins.ToString());
        visitorSeat_LvText.text = littleZooVisitorSeatLevel.ToString();
        visitorSeat_Button_buttonLvUpText.text = GetL10NString("Ui_Text_7");

        visitorSpawn_Text2.text = LittleZooModule.GetVisitDurationMS(nameID, littleZooEnterVisitorSpawnLevel).ToString("f2") + GetL10NString("Ui_Text_67"); //流量标签2
        visitorSpawn_Text3.text = "+" + (LittleZooModule.GetVisitDurationMS(nameID, littleZooEnterVisitorSpawnLevel, 1)).ToString("f2"); //流量标签3
        visitorSpawn_Button_NeedGoldText.text = MinerBigInt.ToDisplay(EnterVisitorSpawnLevelConsumeCoins.ToString());
        visitorSpawn_LvText.text = littleZooEnterVisitorSpawnLevel.ToString();
        visitorSpawn_Button_buttonLvUpText.text = GetL10NString("Ui_Text_7");

        buttonIcon_1.gameObject.SetActive(true);
        buttonIcon_2.gameObject.SetActive(true);
        buttonIcon_3.gameObject.SetActive(true);

        if (!SetGradeBool_Tickets())
        {
            SwitchButtonUnClickable(tickets_Button, false);
        }
        else
        {
            SwitchButtonUnClickable(tickets_Button, true);
        }

        if (!SetGradeBool_VisitorSeat())
        {
            SwitchButtonUnClickable(visitorSeat_Button, false);
        }
        else
        {
            SwitchButtonUnClickable(visitorSeat_Button, true);
        }
        if (!SetGradeBool_VisitorSpawn())
        {
            SwitchButtonUnClickable(visitorSpawn_Button, false);
        }
        else
        {
            SwitchButtonUnClickable(visitorSpawn_Button, true);
        }

        if (littleZooTicketsLevel >= TicketsMaxGrade)
        {
            setGradeBool = false;
            tickets_Button_NeedGoldText.text = GetL10NString("Ui_Text_47");       //升级模式需要的金钱
            tickets_Button_buttonLvUpText.text = GetL10NString("Ui_Text_46");    //升级模式要升的级数
            //lvText.text = Config.buildupConfig.getInstace().getCell(nameID).lvmax.ToString();            //等级text
            lvText.text = string.Format(GetL10NString("Ui_Text_2"), GetL10NString("Ui_Text_126"));
            tickets_Text3.text = GetL10NString("Ui_Text_47");  //价格变化标签
            buttonIcon_1.gameObject.SetActive(false);
            SwitchButtonUnClickable(tickets_Button, false);

        }
        if (littleZooVisitorSeatLevel >= VisitorSeatMaxGrade)
        {
            setGradeBool = false;
            visitorSeat_Button_NeedGoldText.text = GetL10NString("Ui_Text_47");       //升级模式需要的金钱
            visitorSeat_Button_buttonLvUpText.text = GetL10NString("Ui_Text_46");    //升级模式要升的级数
            visitorSeat_Text3.text = GetL10NString("Ui_Text_47"); //数量变化标签
            SwitchButtonUnClickable(visitorSeat_Button, false);
            buttonIcon_2.gameObject.SetActive(false);
        }
        if (littleZooEnterVisitorSpawnLevel >= EnterVisitorSpawnMaxGrade)
        {
            setGradeBool = false;
            visitorSpawn_Button_NeedGoldText.text = GetL10NString("Ui_Text_47");       //升级模式需要的金钱
            visitorSpawn_Button_buttonLvUpText.text = GetL10NString("Ui_Text_46");    //升级模式要升的级数
            visitorSpawn_Text3.text = GetL10NString("Ui_Text_47"); //速度变化标签
            buttonIcon_3.gameObject.SetActive(false);
            SwitchButtonUnClickable(visitorSpawn_Button, false);
        }
        if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide == true)
        {
            SwitchButtonUnClickable(visitorSeat_Button, false);
            SwitchButtonUnClickable(visitorSpawn_Button, false);
        }
    }
    private void InitCompentCultivate()
    {
        if (m_data == null)
        {
            return;
        }
        bool isShowAnimalCultivate = GlobalDataManager.GetInstance().playerData.playerZoo.isShowAnimalCultivate;

        tipsText.text = GetL10NString("Ui_Text_61");
        int childCount = animalGroup.transform.childCount;
        animalCellList.Add(animal_1);
        for (int i = 0; i < childCount; i++)
        {
            int animalID = buildUpCell.animalid[i];
            animalCellID[i] = animalID;
            Text nameText = animalCellList[i].transform.Find("NameText").GetComponent<Text>();
            nameText.text = GetL10NString(Config.animalupConfig.getInstace().getCell(animalID).nametranslate);

            Image iconImage = animalCellList[i].transform.Find("Icon").GetComponent<Image>();
            string iconPath = Config.animalupConfig.getInstace().getCell(animalID).icon;
            iconImage.sprite = ResourceManager.LoadSpriteFromPrefab(iconPath);
            //动物等级
            int zooLevel = 0;
            Text NameText = animalCellList[i].transform.Find("LvBg/LvNum").GetComponent<Text>();
            if (animalMSS15.GetAnimalProp(animalID)!=null)
            {
                zooLevel = animalMSS15.GetAnimalProp(animalID).lv;

            }
            NameText.text = zooLevel.ToString();

            //提示文本
            Text AnimalNumber = animalCellList[i].transform.Find("AnimalNumber").GetComponent<Text>();
            AnimalNumber.text = LittleZooModule.GetAnimalsBuff(animalID).ToString() + "%";
            //购买、解锁按钮
            Button buyButton = animalCellList[i].transform.Find("BuyButton").GetComponent<Button>();

            ////小锁标识
            Image lockIcon = animalCellList[i].transform.Find("LockIcon").GetComponent<Image>();
            ////购买价格
            if (buyButton == null)
            {
                string e = string.Format("紧急   注意：   buyButton  为null");
                throw new System.Exception(e);
            }
            Text buyText = buyButton.transform.Find("NumText").GetComponent<Text>();
            Text tipsText = animalTransformData[animalID].transform.Find("TipsText").GetComponent<Text>();
            tipsText.gameObject.SetActive(true);

            //解锁提示文本
            Text openText = buyButton.transform.Find("OpenText").GetComponent<Text>();
            Image goldImage = buyButton.transform.Find("Gold").GetComponent<Image>();

            if (animalMSS15.GetAnimalProp(animalID)==null)
            {
                AnimalNumber.gameObject.SetActive(true);
                //Ui_Text_62
                tipsText.text = string.Format(GetL10NString("Ui_Text_62"), buildUpCell.lvanimal[i], titleText.text);
                buyButton.gameObject.SetActive(false);
                lockIcon.gameObject.SetActive(true);

                int openScene = Config.animalupConfig.getInstace().getCell(animalID).openscene;
                if (openScene == playerData.playerZoo.currSceneID)
                {
                    string str = GetL10NString("Ui_Text_62");
                    tipsText.text = string.Format(str, buildUpCell.lvanimal[i], titleText.text);
                }
                else
                {
                    string str = GetL10NString("Ui_Text_125");
                    var sceneCell = Config.sceneConfig.getInstace().getCell(openScene);
                    string str1 = string.Format(GetL10NString(sceneCell.scenename), sceneCell.sceneorder);
                    tipsText.text = string.Format(str, str1);
                }
            }
            else if (animalMSS15.GetAnimalProp(animalID).lv>0)
            {
                AnimalNumber.gameObject.SetActive(true);
                lockIcon.gameObject.SetActive(false);
                openText.gameObject.SetActive(false);
                buyText.text = LittleZooModule.GetAnimalUpLevelPriceFormula(animalID).ToString();
                tipsText.gameObject.SetActive(false);
                buyButton.gameObject.SetActive(true);
                buyText.gameObject.SetActive(true);
                goldImage.gameObject.SetActive(true);

                if (LittleZooModule.GetAnimalUpLevelPriceFormula(animalID) <= playerData.playerZoo.diamond && zooLevel < animalLvUpLimit)
                {
                    SwitchButtonUnClickable(buyButton, true);
                }
                else if (LittleZooModule.GetAnimalUpLevelPriceFormula(animalID) > playerData.playerZoo.diamond)
                {
                    //buyText.color = Color.white;
                    SwitchButtonUnClickable(buyButton, false);
                }
                if (zooLevel >= animalLvUpLimit)
                {
                    openText.text = GetL10NString("Ui_Text_46");
                    openText.gameObject.SetActive(true);
                    buyText.gameObject.SetActive(false);
                    goldImage.gameObject.SetActive(false);
                    AnimalNumber.gameObject.SetActive(true);
                    buyButton.transform.Find("ButtonTipsText").GetComponent<Text>().gameObject.SetActive(false);
                    SwitchButtonUnClickable(buyButton, false);
                }
                else
                {
                    buyButton.transform.Find("ButtonTipsText").GetComponent<Text>().gameObject.SetActive(true);

                }
                //if (playerAnimal.getPlayerAnimalCell(animalID).animalLevel>0)
                //{
                //    tipsText.text = GetL10NString("Ui_Text_114");
                //}
                //关于动物培养的显隐
                if (isShowAnimalCultivate)
                {
                    buyButton.gameObject.SetActive(true);
                }
                else
                {
                    buyButton.gameObject.SetActive(false);
                }
            }
        }


    }

    private void SendSetZooTicketsLevelMessageManager()
    {
        if (littleZooTicketsLevel >= TicketsMaxGrade)
        {
            SwitchButtonUnClickable(tickets_Button, false);
            return;
        }
        //发送消息      
        SetDetailValueOfPlayerData.Send((int)GameMessageDefine.SetLittleZooTicketsLevelPlayerData,
                nameID, 1, 0);
    }
    private void SendSetZooVisitorSeatLevelMessageManager()
    {
        if (littleZooVisitorSeatLevel >= VisitorSeatMaxGrade)
        {
            SwitchButtonUnClickable(visitorSeat_Button, false);
            return;
        }
        //发送消息       
        SetDetailValueOfPlayerData.Send((int)GameMessageDefine.SetLittleZooVisitorLocationLevelOfPlayerData,
                nameID, 1, 0);
    }
    private void SendSetZooVisitorSpawnLevelMessageManager()
    {
        if (littleZooEnterVisitorSpawnLevel >= EnterVisitorSpawnMaxGrade)
        {
            SwitchButtonUnClickable(visitorSpawn_Button, false);
            return;
        }
        //发送消息     
        SetDetailValueOfPlayerData.Send((int)GameMessageDefine.SetLittleZooEnterVisitorSpawnLevelOfPlayerData,
                nameID, 1, 0);
    }
    /// <summary>
    /// 动物升级消息
    /// </summary>
    private void SendSetAnimalLevelMessageManager(int animalID)
    {
        if (animalMSS15.GetAnimalProp(animalID).lv >= animalLvUpLimit)
        {
            return;
        }
        SetBuyAnimalObjectData.Send((int)GameMessageDefine.SetAnimalLevel,
                1, animalID, 1, 0, LittleZooModule.GetAnimalUpLevelPriceFormula(animalID),  nameID);
    }

    /// <summary>
    /// 监听玩家数据方法
    /// </summary>
    /// <param name="msg"></param>
    public void OnGetBroadcastLittleZooTicketsLevelPlayerData(Message msg)
    {
        //刷新UI显示
        coinVal = playerData.playerZoo.playerCoin.GetCoinByScene(playerData.playerZoo.currSceneID).coinBigInt;
        SwitchButtonUnClickable(tickets_Button, true);
        InitData();
        this.InitCompent();
        if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide == true)
        {
            PromptText.CreatePromptText_TestUI(string.Format(GetL10NString("Ui_Text_136"), littleZooTicketsLevel, 5));
        }

        if (GlobalDataManager.GetInstance().playerData.playerZoo.isGuide == true && littleZooTicketsLevel == 5)
        {   /*新手阶段     显示新手引导UI    步骤应该是  22  */
            DestroyEffectChild();
            SwitchButtonUnClickable(visitorSeat_Button, true);
            SwitchButtonUnClickable(visitorSpawn_Button, true);
            SwitchButtonUnClickable(zooCultivateButton, true);
            SwitchButtonUnClickable(zooKindButton, true);
            this.Hide();

        }
        isGetCoin = true;
    }
    private void OnGetBroadcastLittleZooEnterVisitorSpawnLevelOfPlayerData(Message obj)
    {
        //刷新UI显示
        coinVal = playerData.playerZoo.playerCoin.GetCoinByScene(playerData.playerZoo.currSceneID).coinBigInt;
        SwitchButtonUnClickable(visitorSpawn_Button, true);

        InitData();
        this.InitCompent();

        isGetCoin = true;
    }
    private void OnGetBroadcastLittleZooVisitorLocationLevelOfPlayerData(Message obj)
    {
        //刷新UI显示
        coinVal = playerData.playerZoo.playerCoin.GetCoinByScene(playerData.playerZoo.currSceneID).coinBigInt;
        SwitchButtonUnClickable(visitorSeat_Button, true);
        InitData();
        this.InitCompent();

        isGetCoin = true;
    }
    /// <summary>
    /// 监听玩家coin金钱发生改变，是否需要重新计算升级规模
    /// </summary>
    /// <param name="obj"></param>
    private void OnGetBroadcastCoinOfPlayerDataMSSC(Message obj)
    {   //旧计算金钱不够，则开始新的计算
        this.InitData();
        this.InitCompent();
    }
    private void OnOpenOfflineUIPage(Message obj)
    {
        HideButtonUI("");
    }
    /// <summary>
    /// 动物升级成功返回消息
    /// </summary>
    /// <param name="obj"></param>
    private void GetAchievementSetObject(Message obj)
    {
        var _msg = obj as GetAddNewAnimalData;
        isGetCoin = true;
        this.InitData();
        this.InitCompent();
    }
}
