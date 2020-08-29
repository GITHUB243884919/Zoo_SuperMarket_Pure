﻿using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UFrame.Logger;
using Game.GlobalData;
using UFrame;
using UFrame.MessageCenter;
using Game.MessageCenter;
using UFrame.MiniGame;

namespace CrossRoadGame
{
    /// <summary>
    /// 小游戏失败UI
    /// </summary>
    public class UIGameFailPage : UIPage
    {
        PlayerData playerData;

        /// <summary>
        /// 当前场景对应的金钱图标
        /// </summary>
        Sprite sprite;

        /// <summary>
        /// 返回主界面
        /// </summary>
        Button returnButton;
        /// <summary>
        /// 再来一次
        /// </summary>
        Button againButton;
        /// <summary>
        /// 玩家金钱
        /// </summary>
        Text money_1_Text;
        /// <summary>
        /// 玩家钻石
        /// </summary>
        Text money_2_Text;
        /// <summary>
        /// 收益金钱
        /// </summary>
        Text rewardGold_text;
        /// <summary>
        /// 收益钻石
        /// </summary>
        Text rewardRmb_text;

        Image money_1_GoldIcon;
        Image mewardGold_Image;

        Transform tipsHand;

        bool isShowAD = false;

        bool requestADButUnload = false;

        Text next_text;

        public UIGameFailPage() : base(UIType.Fixed, UIMode.DoNothing, UICollider.None)
        {
            uiPath = "UIPrefab/UIGameFail";
        }

        public override void Awake(GameObject go)
        {
            base.Awake(go);
            GetTransPrefabAllTextShow(this.transform, true);
            playerData = GlobalDataManager.GetInstance().playerData;
            RegistAllCompent();
            MessageManager.GetInstance().Regist((int)GameMessageDefine.BroadcastCoinOfPlayerDataMSSC, this.OnGetBroadcastCoinOfPlayerDataMSSC);//接受金钱变动的信息

        }
        public override void Active()
        {
            base.Active();
            IninCompentData();

            //设置新手阶段 的按钮置灰
            //UIPage.SwitchButtonUnClickable(returnButton,! playerData.playerLittleGame.isFirst);
            PageMgr.ClosePage<UIGameVictoryPage>();

            //if (playerData.playerLittleGame.isFirst)
            //{
            //    OnGuideCrossRoad();
            //}
            //else
            {
                tipsHand.gameObject.SetActive(false);
            }

            float p = UnityEngine.Random.Range(0f, 1f);
            isShowAD = false;
            next_text.text = next_text.text.Replace("AD ", "");
            if (p >= 0.5f) {
                isShowAD = true;
                next_text.text = "AD " + next_text.text;
            }
        }

        public override void Hide()
        {
            base.Hide();
            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.BroadcastCoinOfPlayerDataMSSC, this.OnGetBroadcastCoinOfPlayerDataMSSC);//接受金钱变动的信息

        }

        /// <summary>
        /// 内部组件的查找
        /// </summary>
        private void RegistAllCompent()
        {
            tipsHand = RegistCompent<Transform>("ButtonGroup/AgainButton/Button/TipsHand");

            returnButton = RegistBtnAndClick("ButtonGroup/ReturnButton/Button", OnClickReturnButton);
            againButton = RegistBtnAndClick("ButtonGroup/AgainButton/Button", OnClickAgainButton);

            money_1_Text = RegistCompent<Text>("MoneyGroup/Money_1/Text");
            money_2_Text = RegistCompent<Text>("MoneyGroup/Money_2/Text");
            rewardGold_text = RegistCompent<Text>("Reward/RewardGold/Text");
            //rewardRmb_text = RegistCompent<Text>("Reward/RewardRmb/BomusNum");

            money_1_Text.text = playerData.playerZoo.playerCoin.GetCoinByScene(playerData.playerZoo.currSceneID).coinShow;
            money_2_Text.text = "0";

            //money_1_GoldIcon = RegistCompent<Image>("MoneyGroup/Money_1/GoldIcon");
            //mewardGold_Image = RegistCompent<Image>("Reward/RewardGold/Image");
            //SetCorrectShowImage();

            next_text = RegistCompent<Text>("ButtonGroup/AgainButton/Button/Text");
        }
        ///// <summary>
        ///// 修改对应的UiImage的sprite
        ///// </summary>
        //private void SetCorrectShowImage()
        //{
        //    int sceneID = GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID;
        //    int scenetype = Config.sceneConfig.getInstace().getCell(sceneID).moneyid;
        //    string iconPath = Config.moneyConfig.getInstace().getCell(scenetype).moneyicon;
        //    sprite = ResourceManager.LoadSpriteFromPrefab(iconPath);
        //    money_1_GoldIcon.sprite = sprite;
        //    mewardGold_Image.sprite = sprite;
        //}

        /// <summary>
        /// 按钮  返回主界面
        /// </summary>
        /// <param name="obj"></param>
        private void OnClickReturnButton(string obj)
        {
            CrossRoadStageManager.GetInstance().UnLoad();
            ZooGameLoader.GetInstance().BackFromCrossRoad();


        }
        /// <summary>
        /// 按钮  再来一次界面
        /// </summary>
        /// <param name="obj"></param>
        private void OnClickAgainButton(string obj)
        {
            ////if (playerData.playerLittleGame.strength>0)
            //{
            //    int stageID = CrossRoadModelManager.GetInstance().stageID;
            //    CrossRoadStageManager.GetInstance().UnLoad();
            //    CrossRoadGame.CrossRoadStageManager.GetInstance().Load(stageID);
            //}
            ////else
            ////{
            ////    PromptText.CreatePromptText("Ui_Text_133");

            ////}
            

            if (!isShowAD) {
                Next();
                return;
            }

            ShowAD();
        }

        void ShowAD()
        {
            if (AdmobManager.GetInstance().isLoaded) {
                requestADButUnload = false;
                AdmobManager.GetInstance().UserChoseToWatchAd(Next);
            } else {
                requestADButUnload = true;
                PageMgr.ShowPage<UIWaitAd>(5000);
            }
        }

        void Next()
		{
            PageMgr.ClosePage<UIGameVictoryPage>();
            PageMgr.ClosePage<UIGameFailPage>();

            int stageID = CrossRoadModelManager.GetInstance().stageID;
            CrossRoadStageManager.GetInstance().UnLoad();
            CrossRoadGame.CrossRoadStageManager.GetInstance().Load(stageID);
        }

        private void IninCompentData()
        {
            rewardGold_text.text = "0";
            //rewardRmb_text.text = "0";

        }
        private void OnGetBroadcastCoinOfPlayerDataMSSC(Message obj)
        {
            money_1_Text.text = playerData.playerZoo.playerCoin.GetCoinByScene(playerData.playerZoo.currSceneID).coinShow;
            //money_2_Text.text = "0";
        }

        private void OnGuideCrossRoad()
        {
            tipsHand.gameObject.SetActive(true);
            
            Transform trans = null;
            trans = ResourceManager.GetInstance().LoadGameObject(Config.globalConfig.getInstace().GuideUiClickEffect).transform;
            trans.SetParent(tipsHand, true);
            trans.localScale = UnityEngine.Vector3.one * 10;
            trans.position = tipsHand.position;
            trans.localPosition = new UnityEngine.Vector3(
                trans.localPosition.x,
                trans.localPosition.y + 4,
                trans.localPosition.z);
        }
    }
}
