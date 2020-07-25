using UFrame.Logger;
using System.Collections;
using System.Collections.Generic;
using UFrame.Common;
using UnityEngine;

namespace Game
{
    public partial class LittleZooPosManager : Singleton<LittleZooPosManager>, ISingleton
    {
        Dictionary<int, Vector3> posMap = new Dictionary<int, Vector3>();

        public void Init()
        {
            //AddAll();
            this.Add_Style_Nine_LittleZoo();
            this.Add_Style_Ten_LittleZoo();
        }

        public Vector3 GetPos(int littleZooID)
        {
            int maplittleZooID = LittleZooModule.MapToFirstSceneLittleZooID(littleZooID);
            Vector3 pos = Vector3.zero;

            posMap.TryGetValue(maplittleZooID, out pos);

            return pos;
        }
    }
}

