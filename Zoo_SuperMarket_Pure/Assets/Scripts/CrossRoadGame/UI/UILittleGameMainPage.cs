using Game.GlobalData;
using Game.MessageCenter;
using System;
using System.Collections;
using System.Collections.Generic;
using UFrame;
using UFrame.Logger;
using UFrame.MessageCenter;
using UFrame.MiniGame;
using UnityEngine;
using UnityEngine.UI;

namespace CrossRoadGame
{
    public class UILittleGameMainPage : UIPage
    {
        Image uIBackgroundImage;
        Button reviveButton;
     
        private Text numberText;
        PlayerData playerData {
            get {
                return GlobalDataManager.GetInstance().playerData;
            }
        }
        Sprite sprite;
        private Slider scheduleSlider;
        private Text scheduleSlider_text;

        private Transform tipsGroup;

        /// <summary>
        /// 获取当前配置的过马路文件 crossroadstageCell
        /// </summary>
        Config.crossroadstageCell cellStage {
            get {
                int stageID = CrossRoadModelManager.GetInstance().stageID;
                return Config.crossroadstageConfig.getInstace().getCell(stageID);
            }
        }

        public int singleRoadSucceed =0;
        public UILittleGameMainPage() : base(UIType.Fixed, UIMode.DoNothing, UICollider.None)
        {
            uiPath = "UIPrefab/UIGame";
        }
        public override void Awake(GameObject go)
        {
            base.Awake(go);
            GetTransPrefabAllTextShow(this.transform, true);

            RegistAllCompent();

            MessageManager.GetInstance().Regist((int)GameMessageDefine.RewardADLoadSuccess, OnRewardADLoadSuccess);

            MessageManager.GetInstance().Regist((int)GameMessageDefine.RewardADLoadFail, OnRewardADLoadFail);

        }
        /// <summary>
        /// 小动物过马路成功
        /// </summary>
        /// <param name="obj"></param>
        private void OnGetCrossRoadAnimalTeamArrived(Message obj)
        {
            PageMgr.ShowPage<UIGameVictoryPage>();

        }

        public override void Active()
        {
            base.Active();
            MessageManager.GetInstance().Regist((int)GameMessageDefine.CrossRoadCameraStopMove, this.OnGetCrossRoadCameraStopMove);
            MessageManager.GetInstance().Regist((int)GameMessageDefine.CrossRoadAnimalTeamArrived, this.OnGetCrossRoadAnimalTeamArrived);
            uIBackgroundImage.gameObject.SetActive(true);
            reviveButton.gameObject.SetActive(true);
            IninCompentData();
            if (!playerData.playerLittleGame.isFirst)
            {
                OnGuideCrossRoad();
            }
            tipsGroup.gameObject.SetActive(true);

            PageMgr.ClosePage<UIGameVictoryPage>();
            PageMgr.ClosePage<UIGameFailPage>();
        }

        private void OnGuideCrossRoad()
        {
            tipsGroup.gameObject.SetActive(true);
            Transform effectNode = tipsGroup.Find("TipsHand");
            Transform trans = null;
            trans = ResourceManager.GetInstance().LoadGameObject(Config.globalConfig.getInstace().GuideUiClickEffect).transform;
            trans.SetParent(effectNode, true);
            trans.localScale = UnityEngine.Vector3.one*10;
            trans.position = effectNode.position;
            trans.localPosition = new UnityEngine.Vector3(
                trans.localPosition.x,
                trans.localPosition.y + 4,
                trans.localPosition.z);
        }

        public override void Hide()
        {
            base.Hide();
            IninCompentData();

            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.CrossRoadCameraStopMove, this.OnGetCrossRoadCameraStopMove);
            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.CrossRoadAnimalTeamArrived, this.OnGetCrossRoadAnimalTeamArrived);

        }
        private void OnGetCrossRoadGameSingleRoadSucceed(Message obj)
        {
            //singleRoadSucceed += 1;
            //float value = AddPercentage(singleRoadSucceed, cellStage.roadnum);
            //scheduleSlider.value = value;
            //scheduleSlider_text.text = value * 100+"%";
            //numberText.text = string.Format( GetL10NString("Ui_Text_123"), CrossRoadModelManager.GetInstance().stageID);
        }

        /// <summary>
        /// 内部组件的查找
        /// </summary>
        private void RegistAllCompent()
        {
            RegistBtnAndClick("Btn_AD_Test", OnClickBtnADTest);

            uIBackgroundImage = RegistCompent<Image>("UIBackgroundImage");
            reviveButton = RegistBtnAndClick("ReviveButton", OnClickReceiveButton);
            uIBackgroundImage.gameObject.SetActive(true);
            reviveButton.gameObject.SetActive(true);

            scheduleSlider = RegistCompent<Slider>("UP/ScheduleSlider");
            scheduleSlider_text = RegistCompent<Text>("UP/ScheduleSlider/FillArea/ValueText");
            numberText = RegistCompent<Text>("UP/MoneyGroup/NumberText");
            tipsGroup = RegistCompent<Transform>("TipsGroup");

            SetCorrectShowImage();
        }

      
        /// <summary>
        /// 修改对应的UiImage的sprite
        /// </summary>
        private void SetCorrectShowImage()
        {
            int sceneID = GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID;
            int scenetype = Config.sceneConfig.getInstace().getCell(sceneID).moneyid;
            string iconPath = Config.moneyConfig.getInstace().getCell(scenetype).moneyicon;
            sprite = ResourceManager.LoadSpriteFromPrefab(iconPath);
            

        }
        /// <summary>
        /// 相机移动结束
        /// </summary>
        /// <param name="obj"></param>
        private void OnGetCrossRoadCameraStopMove(Message obj)
        {
            //LogWarp.LogError("AAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            //取消不可点击
            uIBackgroundImage.gameObject.SetActive(false);
            //UI显示更新
            singleRoadSucceed += 1;
            float value = AddPercentage(singleRoadSucceed, cellStage.roadnum);
            scheduleSlider.value = value;
            scheduleSlider_text.text = value * 100 + "%";
            numberText.text = string.Format(GetL10NString("Ui_Text_123"), CrossRoadModelManager.GetInstance().stageID);
        }

        /// <summary>
        /// 开始游戏隐藏按钮
        /// </summary>
        /// <param name="obj"></param>
        private void OnClickReceiveButton(string obj)
        {
            uIBackgroundImage.gameObject.SetActive(false);
            reviveButton.gameObject.SetActive(false);
            UFrame.MessageManager.GetInstance().Send((int)GameMessageDefine.CrossRoadStartGame);
            if (CrossRoadModelManager.GetInstance().stageID !=1)
            {
                UFrame.MessageInt.Send((int)GameMessageDefine.AddStrength, -1);
            }
            tipsGroup.gameObject.SetActive(false);
            //if (!playerData.playerLittleGame.isFirst)
            //{
            //    tipsGroup.gameObject.SetActive(false);
            //}
        }

        private void IninCompentData()
        {
            singleRoadSucceed = 0;
            scheduleSlider.value = 0;
            scheduleSlider_text.text = "0%";
            numberText.text = string.Format(GetL10NString("Ui_Text_123"), CrossRoadModelManager.GetInstance().stageID);
        }
        /// <summary>
        /// 获取金钱改变
        /// </summary>
        /// <param name="obj"></param>
        private void OnGetBroadcastCoinOfPlayerDataMSSC(Message obj)
        {
            //money_1_Text.text = playerData.playerZoo.playerCoin.GetCoinByScene(playerData.playerZoo.currSceneID).coinShow;
            //money_2_Text.text = "0";
        }

        bool requestADButUnload = false;

        private void OnClickBtnADTest(string str)
        {
            if (AdmobManager.GetInstance().isLoaded)
            {
                requestADButUnload = false;
                AdmobManager.GetInstance().UserChoseToWatchAd(OnWatchRewardAdSuccessed_Test);
            }
            else
            {
                requestADButUnload = true;
                PageMgr.ShowPage<UIWaitAd>(5000);
            }
        }

        void OnRewardADLoadSuccess(Message msg)
        {
            PageMgr.ClosePage<UIWaitAd>();
            if (requestADButUnload)
            {
                requestADButUnload = false;
                AdmobManager.GetInstance().UserChoseToWatchAd(OnWatchRewardAdSuccessed_Test);
            }
        }

        void OnRewardADLoadFail(Message msg)
        {
            PageMgr.ClosePage<UIWaitAd>();
            if (requestADButUnload)
            {
                requestADButUnload = false;
                PromptText.CreatePromptText(false, "Load AD Fail");
            }
        }

        void OnWatchRewardAdSuccessed_Test()
        {
        }
    }
}

