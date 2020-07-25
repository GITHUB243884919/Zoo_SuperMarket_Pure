using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.GlobalData
{
    public class LogicTableParkingData
    {

        public LogicTableParkingData()
        {
            Init();
        }

        protected void Init()
        {
        }

        public Config.parkingCell GetParkingCell(int sceneID)
        {
            Config.parkingCell cell = null;
            foreach (var val in Config.parkingConfig.getInstace().AllData.Values)
            {
                if (val.scene == sceneID)
                {
                    return val;
                }
            }

            return cell;
        }


    }

}

