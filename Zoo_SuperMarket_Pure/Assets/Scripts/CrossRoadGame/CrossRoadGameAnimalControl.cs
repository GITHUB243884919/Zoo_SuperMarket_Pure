﻿
using UnityEngine;
using UFrame.Logger;
using UnityEngine.EventSystems;
using Game.MessageCenter;
using UFrame;

namespace CrossRoadGame
{
    public class CrossRoadGameAnimalControl : MonoBehaviour
    {
        bool isFingerUp = true;

        bool isCameraMoving
        {
            get
            {
                return CrossRoadModelManager.GetInstance().isCameraMoving;
            }
        }

        public CrossRoadAnimalTeamModel animalTeamModel {
            get {
                return CrossRoadModelManager.GetInstance().animalTeamModel;
            }
        }

        #region 处理鼠标点击到UI上的问题
        /// <summary>
        /// 处理鼠标点击到UI上的问题
        /// </summary>
        bool FingerGestureShouldProcessTouch(int fingerIndex, Vector2 position)
        {
            if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            {
                //屏幕触摸触发
#if UNITY_EDITOR
                if (EventSystem.current.IsPointerOverGameObject())
#else
                if (EventSystem.current.IsPointerOverGameObject(fingerIndex))
#endif
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

#endregion

        private void Awake()
        {
            FingerGestures.GlobalTouchFilter = FingerGestureShouldProcessTouch;
            GetComponent<LongPressRecognizer>().Duration = CrossRoadStageManager.GetInstance().buttonClickLongPressTime;
            PageMgr.ShowPage<UILittleGameMainPage>();
        }

        
        void OnFingerDown(FingerDownEvent e)
        {
            //Debug.LogError("OnFingerDown");
            if (!isCameraMoving && isFingerUp)
            {
                isFingerUp = false;
                DebugFile.GetInstance().WriteKeyFile("OnFingerDown", "OnFingerDown");

                MessageManager.GetInstance().Send(
                    (int)GameMessageDefine.CrossRoadAnimalTeamMove);
            }

        }

        void OnLongPress(LongPressGesture gesture)
        {
            // 长按持续时间
            //Debug.LogErrorFormat("OnLongPress {0}", gesture.ElapsedTime);

            if (!isCameraMoving && !isFingerUp)
            {
                DebugFile.GetInstance().WriteKeyFile("OnLongPress", "OnLongPress");
                MessageManager.GetInstance().Send(
                    (int)GameMessageDefine.CrossRoadAnimalTeamMove);

            }
        }

        void OnFingerUp(FingerUpEvent e)
        {
            if (!isCameraMoving)
            {
                isFingerUp = true;
                DebugFile.GetInstance().WriteKeyFile("OnFingerUp", "OnFingerUp");
                MessageManager.GetInstance().Send(
                    (int)GameMessageDefine.CrossRoadAnimalTeamStopMove);

            }
        }

    }
}
