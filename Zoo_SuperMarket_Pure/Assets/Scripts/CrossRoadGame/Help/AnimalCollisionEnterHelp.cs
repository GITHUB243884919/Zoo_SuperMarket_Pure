using CrossRoadGame;
using Game.MessageCenter;
using System.Collections;
using System.Collections.Generic;
using UFrame;
using UnityEngine;

public class AnimalCollisionEnterHelp : MonoBehaviour
{

    /// <summary>
    /// 是否开启碰撞
    /// </summary>
    bool isCollision {
        get {
            return CrossRoadStageManager.GetInstance().isCollision;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Car"))
        {
            if (isCollision)
            {
                var sp = new SimpleParticle();
                sp.Init(transform.parent.Find("Effect/ColliderEffect").gameObject);
                sp.Play();
                MessageManager.GetInstance().Send((int)GameMessageDefine.CrossRoadGameFailure);
            }

        }
    }
  
}
