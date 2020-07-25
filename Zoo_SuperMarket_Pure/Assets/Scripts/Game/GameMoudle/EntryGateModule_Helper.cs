using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UFrame;
using UFrame.BehaviourFloat;
using Game.Path.StraightLine;
using UFrame.MessageCenter;
using Game.MiniGame;
using Config;
using UFrame.Logger;
using Game.MessageCenter;
using UFrame.EntityFloat;
using Game.GlobalData;
using UFrame.OrthographicCamera;
using UFrame.MiniGame;
using UnityEngine.UI;
using System.Linq;

namespace Game
{
    public partial class EntryGateModule : GameModule
    {
        public static int GetPresentSceneTicketCell(int id)
        {
            int subscript = -1;
            int sceneID = GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID;
            var sortGateIDs = GlobalDataManager.GetInstance().logicTableEntryGate.GetSortGateIDs(sceneID);
            for (int i = 0; i < sortGateIDs.Count; i++)
            {
                if (sortGateIDs[i] == id)
                {
                    subscript = i;
                    return subscript;
                }
            }
            string e = string.Format("场景为{0}的售票口数据没有ID为{1}的", sceneID,id);
            throw new System.Exception(e);
            return subscript;
        }

        /// <summary>
        /// 获取节点下的观光点技能CD对象，
        /// </summary>
        /// <param name="vector">技能对象出现的位置</param>
        /// <returns></returns>
        public static GameObject GetVisitCDGameObject( int idx)
        {
            GameObject visitCDPrefabs = GlobalDataManager.GetInstance().littleSceneUI.GetEntryUISceneGameObject( idx, 0);

            if (visitCDPrefabs == null)
            {
                LogWarp.LogError("测试：visitCDPrefabs为空     idx" + idx);
            }
            visitCDPrefabs.transform.position = GlobalDataManager.GetInstance().zooGameSceneData.entryGateSenceData.entryGatesVector[idx%8];
            GameObject skill = visitCDPrefabs.transform.Find("Text_UI").gameObject;
            float scale = Config.globalConfig.getInstace().VisitSeatCDScale;
            skill.transform.localScale = new Vector3(scale, scale, scale);
            Image image_Skill = skill.transform.Find("Image_Skill").GetComponent<Image>();
            image_Skill.fillAmount = 0f;
            return visitCDPrefabs;
        }

        /// <summary>
        /// 获取节点下的观光点飘字对象，
        /// </summary>
        /// <param name="littleZooID">动物栏ID</param>
        /// <param name="vector">技能对象出现的位置</param>
        /// <returns></returns>
        public static GameObject GetFlutterTextGameObject(int idx)
        {
            GameObject SceneFlutterTextPrefabs = GlobalDataManager.GetInstance().littleSceneUI.GetEntryUISceneGameObject( idx, 1);
            Text earnings = SceneFlutterTextPrefabs.transform.Find("SceneFlutterText01").GetComponent<Text>();
            int sceneID = GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID;

#if NO_BIGINT
            earnings.text = "+" + "3.14ab";
#else
            earnings.text = "+" + MinerBigInt.ToDisplay(GetEntryPrice(GlobalDataManager.GetInstance().playerData.GetEntryDateDataIDIndexOfDataIdx().entryTicketsLevel, sceneID,true));
#endif
            Animator m_Animator = SceneFlutterTextPrefabs.transform.Find("SceneFlutterText01").GetComponent<Animator>();
            m_Animator.enabled = true;
            m_Animator.Play("UINumber", 0, 0f);
            return SceneFlutterTextPrefabs;

        }

    }
}
