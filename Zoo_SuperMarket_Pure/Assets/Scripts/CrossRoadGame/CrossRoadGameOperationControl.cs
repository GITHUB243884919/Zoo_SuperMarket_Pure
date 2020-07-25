//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UFrame.Logger;
//using UnityEngine.EventSystems;
//using Game.MessageCenter;
//using UFrame;
//using System;
//using UFrame.MessageCenter;

//namespace CrossRoadGame
//{
//    public class CrossRoadGameOperationControl : MonoBehaviour
//    {
//        #region 处理鼠标点击到UI上的问题
//        /// <summary>
//        /// 处理鼠标点击到UI上的问题
//        /// </summary>
//        bool FingerGestureShouldProcessTouch(int fingerIndex, Vector2 position)
//        {
//            if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
//            {
//                //屏幕触摸触发
//                //if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {
//                //    return false;
//                //} else {
//                //    return true;
//                //}
//                //鼠标点击触发
//                if (EventSystem.current.IsPointerOverGameObject())
//                {
//                    return false;
//                }
//                else
//                {
//                    return true;
//                }
//            }
//            return false;
//        }

//        #endregion

//        /// <summary>
//        /// 获取当前配置的过马路文件 crossroadstageCell
//        /// </summary>
//        Config.crossroadstageCell cellStage {
//            get {
//                int stageID = CrossRoadModelManager.GetInstance().stageID;
//                return Config.crossroadstageConfig.getInstace().getCell(stageID);
//            }
//        }
//        /// <summary>
//        /// 是否是移动当前马路最后一个
//        /// </summary>
//        bool IsSucceed = true;
//        bool IsCameraStopMove = false;

//        private void Awake()
//        {
//            Debug.LogError("AAA=   CrossRoadGameOperationControl    Awake   ");

//            FingerGestures.GlobalTouchFilter = FingerGestureShouldProcessTouch;
//            GetComponent<LongPressRecognizer>().Duration = CrossRoadStageManager.GetInstance().buttonClickLongPressTime;

//            //PageMgr.ClosePage<UIMainPage>();
//            PageMgr.ShowPage<UILittleGameMainPage>();
//            MessageManager.GetInstance().Regist((int)GameMessageDefine.CrossRoadCameraStopMove, this.OnGetCrossRoadCameraStopMove);

//        }
//        /// <summary>
//        /// 收到相机移动结束的消息
//        /// </summary>
//        /// <param name="obj"></param>
//        private void OnGetCrossRoadCameraStopMove(Message obj)
//        {
//            IsCameraStopMove = false;
//            IsSucceed = true;

//        }

//        void OnTap(TapGesture gesture)
//        {
//            SetProceedGameOperate();
//            //Debug.LogError("AAA=   短按     " );

//        }
//        void OnLongPress(LongPressGesture gesture)
//        {
//            // 长按持续时间
//            //Debug.LogError("AAA=   长按     " + gesture.ElapsedTime);
            
//            SetProceedGameOperate();
//        }

//        int idx=-1;
//        /// <summary>
//        /// 设置游戏操作
//        /// </summary>
//        void SetProceedGameOperate()
//        {
//            //发送消息：
//            MessageManager.GetInstance().Send((int)GameMessageDefine.BroadcastForwardOneStepIRoad);

//        }




//    }
//}
