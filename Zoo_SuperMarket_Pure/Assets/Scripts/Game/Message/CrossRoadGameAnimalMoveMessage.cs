using CrossRoadGame;
using System.Collections;
using System.Collections.Generic;
using UFrame;
using UFrame.MessageCenter;
using UnityEngine;

namespace Game.MessageCenter
{
    public class CrossRoadGameAnimalMoveMessage : Message
    {
        public static ObjectPool<CrossRoadGameAnimalMoveMessage> pool = new ObjectPool<CrossRoadGameAnimalMoveMessage>();

        /// <summary>
        /// 下标
        /// </summary>
        public int location;

        /// <summary>
        /// 下个目标位置
        /// </summary>
        public Vector3 vector;

        public bool isFinally;

        public CrossRoadGameAnimalMoveMessage()
        {
            //this.messageID = (int)GameMessageDefine.CrossRoadGameAnimalMove;
        }
        public override void Release()
        {
            pool.Delete(this);
        }

        public void Init( int messageID, int idx, Vector3 vector,bool isFinally)
        {
            this.messageID = messageID;
            this.location = idx;
            this.vector = vector;
            this.isFinally = isFinally;
        }
        public static CrossRoadGameAnimalMoveMessage Send(int messageID, int idx, Vector3 vector,bool isFinally)
        {
            var msg = pool.New();
            msg.Init(messageID,idx, vector, isFinally);
            MessageManager.GetInstance().Send(msg);
            return msg;
        }

        public override string ToString()
        {
            return string.Format("AdWatchComplete entityCrossRoadAnimal={0}   vector={1}", location, vector);
        }

    }
}
