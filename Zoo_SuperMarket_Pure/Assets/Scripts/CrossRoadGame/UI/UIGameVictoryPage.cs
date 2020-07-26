using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UFrame.Logger;
using UFrame.MiniGame;
using Game.GlobalData;
using UFrame;
using Game.MessageCenter;
using UFrame.MessageCenter;

namespace CrossRoadGame
{
    /// <summary>
    /// 小游戏成功界面
    /// </summary>
    public class UIGameVictoryPage : UIPage
    {
        public UIGameVictoryPage() : base(UIType.Fixed, UIMode.DoNothing, UICollider.None)
        {
            uiPath = "UIPrefab/UIGameVictory";
        }

        /// <summary>
        /// 获取当前配置的过马路文件 crossroadstageCell
        /// </summary>
        Config.crossroadstageCell cellStage {
            get {
                int stageID = CrossRoadModelManager.GetInstance().stageID;
                return Config.crossroadstageConfig.getInstace().getCell(stageID);
            }
        }

        PlayerData playerData;

        int coin;

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
        Button receiveButton;
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
        public override void Awake(GameObject go)
        {
            base.Awake(go);
            GetTransPrefabAllTextShow(this.transform, true);
            playerData = GlobalDataManager.GetInstance().playerData;
            RegistAllCompent();
        }
        public override void Active()
        {
            base.Active();
            MessageManager.GetInstance().Send((int)GameMessageDefine.IncreaseCrossRoadStageID);
            UIPage.SwitchButtonUnClickable(receiveButton, true);
            //设置新手阶段 的按钮置灰
            //UIPage.SwitchButtonUnClickable(receiveButton, ! playerData.playerLittleGame.isFirst);
            //if (playerData.playerLittleGame.isFirst)
            //{
            //    OnGuideCrossRoad();
            //}
            //else
            {
                tipsHand.gameObject.SetActive(false);
            }


            var parkingCenterData = GlobalDataManager.GetInstance().playerData.GetParkingCenterDataIDIndexOfDataIdx(playerData.playerZoo.currSceneID);
            //coin = PlayerDataModule.CurrScenePerMinCoin(true) * (int)(2 + (parkingCenterData.parkingProfitLevel + playerData.GetEntryDateDataIDIndexOfDataIdx(playerData.playerZoo.currSceneID).entryTicketsLevel) / 260f);
            coin = 10;
            IninCompentData();
            returnButton.enabled = true;
            //receiveButton.enabled = true;
        }
        public override void Hide()
        {
            base.Hide();
            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.BroadcastDiamondOfPlayerData, this.OnGetBroadcastDiamondOfPlayerData);//接受钻石变动的信息

        }
        /// <summary>
        /// 内部组件的查找
        /// </summary>
        private void RegistAllCompent()
        {
            tipsHand = RegistCompent<Transform>("ButtonGroup/ReturnButton/TipsHand");

            returnButton = RegistBtnAndClick("ButtonGroup/ReturnButton", OnClickReturnButton);
            receiveButton = RegistBtnAndClick("ButtonGroup/ReceiveButton", OnClickReceiveButton);

            money_1_Text = RegistCompent<Text>("up/coinBg/Text");
            money_2_Text = RegistCompent<Text>("up/diamondBg/Text");

            money_1_Text.text = playerData.playerZoo.playerCoin.GetCoinByScene(0).coinShow;
            money_2_Text.text = "0";

            rewardGold_text = RegistCompent<Text>("Reward/RewardGold/Text");
           
        }
       

        /// <summary>
        /// 按钮  返回主界面
        /// </summary>
        /// <param name="obj"></param>
        private void OnClickReturnButton(string obj)
        {
            SendCrossRoadAward();
            MessageManager.GetInstance().Regist((int)GameMessageDefine.BroadcastDiamondOfPlayerData, this.OnGetBroadcastDiamondOfPlayerData);//接受钻石变动的信息

            //CrossRoadStageManager.GetInstance().UnLoad();
            //ZooGameLoader.GetInstance().BackFromCrossRoad();
            returnButton.enabled = false;
            receiveButton.enabled = false;


        }

        /// <summary>
        /// 再来一次  
        /// </summary>
        /// <param name="obj"></param>
        private void OnClickReceiveButton(string obj)
        {
            if (playerData.playerLittleGame.strength > 0)
            {
                SendCrossRoadAward();
                //UFrame.MessageInt.Send((int)GameMessageDefine.AddStrength, -1);
                CrossRoadStageManager.GetInstance().UnLoad();
                CrossRoadGame.CrossRoadStageManager.GetInstance().Load(playerData.playerLittleGame.stageID + 1);
                returnButton.enabled = false;
                receiveButton.enabled = false;
            }
            else
            {
                PromptText.CreatePromptText("Ui_Text_133");
            }
        }

        /// <summary>
        /// 发送消息（奖励）
        /// </summary>
        private void SendCrossRoadAward()
        {
            SetValueOfPlayerData.Send((int)GameMessageDefine.SetDiamondOfPlayerData,coin,0, 0);
            playerData.playerLittleGame.isFirst = false;

        }

        private void IninCompentData()
        {
            money_1_Text.text = playerData.playerZoo.playerCoin.GetCoinByScene(0).coinShow;
            money_2_Text.text = "0";
            rewardGold_text.text = coin.ToString();
            //rewardRmb_text.text = "0";
        }

        /// <summary>
        /// 接收金钱改变后返回主界面
        /// </summary>
        /// <param name="obj"></param>
        private void OnGetBroadcastDiamondOfPlayerData(Message obj)
        {
            //money_1_Text.text = playerData.playerZoo.playerCoin.GetCoinByScene(playerData.playerZoo.currSceneID).coinShow;

            CrossRoadStageManager.GetInstance().UnLoad();
            ZooGameLoader.GetInstance().BackFromCrossRoad();
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