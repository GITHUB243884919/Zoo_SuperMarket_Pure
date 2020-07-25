using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using Game.GlobalData;
using UFrame.Logger;

namespace Game
{
    public partial class ParkingCenter : GameModule
    {
        public static float GetParkingEnterCarSpawn_UI(int level = -1)
        {   /*  Y=base+lv*0.8   */
            if (level == -1)
            {
                level = GlobalDataManager.GetInstance().playerData.GetParkingCenterDataIDIndexOfDataIdx().parkingEnterCarSpawnLevel;
            }

            int sceneID = GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID;
            Config.parkingCell parkingCell = GlobalDataManager.GetInstance().logicTableParkingData.GetParkingCell(sceneID);

            int baseVal = parkingCell.touristbase;
            var number = baseVal + level * 0.8f;

            return number;
        }

        /// <summary>
        /// 停车场每分钟招揽游客速度
        /// </summary>
        /// <param name="level">停车场每分钟招揽游客等级(流量等级)</param>
        /// <returns></returns>
        public static int GetParkingEnterCarSpawn(int level = -1)
        {   /*  Y=base+lv*0.8   */
            if (level == -1)
            {
                level = GlobalDataManager.GetInstance().playerData.GetParkingCenterDataIDIndexOfDataIdx().parkingEnterCarSpawnLevel;
            }

            int sceneID = GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID;
            Config.parkingCell parkingCell = GlobalDataManager.GetInstance().logicTableParkingData.GetParkingCell(sceneID);

            int baseVal = parkingCell.touristbase;
            int number = GetInteger( baseVal + level * 0.8f);

            return number;
        }
        /// <summary>
        /// (每分钟)停车场来人速度  返回升级多数后增加的客流量
        /// </summary>
        /// <param name="level">当前等级</param>
        /// <param name="getUpLevel">变换等级</param>
        /// <returns></returns>
        public static float GetParkingEnterCarSpawn(int level, int getUpLevel)
        {
            return GetParkingEnterCarSpawn_UI(level + getUpLevel) - GetParkingEnterCarSpawn_UI(level);
        }

        /// <summary>
        /// 其他场景停车场每分钟招揽游客速度
        /// </summary>
        /// <param name="level">停车场每分钟招揽游客等级(流量等级)</param>
        /// <returns></returns>
        public static int GetOtherSceneParkingEnterCarSpawn(int sceneID)
        {   /*  Y=base+lv*1   */
            int level = GlobalDataManager.GetInstance().playerData.GetParkingCenterDataIDIndexOfDataIdx(sceneID).parkingEnterCarSpawnLevel;
            Config.parkingCell parkingCell = GlobalDataManager.GetInstance().logicTableParkingData.GetParkingCell(sceneID);
            int baseVal = parkingCell.touristbase;
            int number = baseVal + level * 1;

            return number;
        }

        /// <summary>
        /// 每分钟招揽游客升级消耗
        /// </summary>
        /// <returns></returns>
        public static System.Numerics.BigInteger GetUpGradeEnterCarSpawnConsumption(int level = -1)
        {   /*   Y=base*停售消耗基数【来客速度期望等级】*加成预期【来客速度期望等级】   */
            if (level == -1)
            {
                level = GlobalDataManager.GetInstance().playerData.GetParkingCenterDataIDIndexOfDataIdx().parkingEnterCarSpawnLevel;
            }
            int expectlevel = ParkingEnterCarSpawnExpectLevel(level);
            int sceneID = GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID;
            Config.parkingCell parkingCell = GlobalDataManager.GetInstance().logicTableParkingData.GetParkingCell(sceneID);
            string str = parkingCell.touristcastbase;
            System.Numerics.BigInteger baseVal = System.Numerics.BigInteger.Parse(str);
            float basenumber = parkingCell.basenumber;
            var computePowFloatToBigInteger = PlayerDataModule.GetComputePowFloatToBigInteger(basenumber, expectlevel - 1);
            int number = (int)(PlayerDataModule.GetAdditionExpect(sceneID, expectlevel) * 100);

            var price = baseVal * computePowFloatToBigInteger.str_numerator * number / (computePowFloatToBigInteger .str_denominator* 100);

            //LogWarp.LogErrorFormat("测试：  baseVal={0}, computePowFloatToBigInteger.str_numerator={1}, number={2}, computePowFloatToBigInteger.str_denominator={3}", baseVal, computePowFloatToBigInteger.str_numerator, number, computePowFloatToBigInteger.str_denominator);
            return price;
        }


        /// <summary>
        /// 停车场停车位数量最大位置
        /// </summary>
        /// <param name="level">停车场停车位等级</param>
        /// <returns></returns>
        public static int GetParkingSpace(int level = -1)
        {   /*  Y=base+lv*6   */
            if (level == -1)
            {
                level = GlobalDataManager.GetInstance().playerData.GetParkingCenterDataIDIndexOfDataIdx().parkingSpaceLevel;
            }
            int sceneID = GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID;
            Config.parkingCell parkingCell = GlobalDataManager.GetInstance().logicTableParkingData.GetParkingCell(sceneID);
            int baseVal = parkingCell.spacebase;
            int number = baseVal + level * 6;
            return number;
        }
        /// <summary>
        /// 停车场最大位置 返回升级多数后增加的停车位数量
        /// </summary>
        /// <param name="level">等级</param>
        /// <param name="getUpLevel">变换值</param>
        /// <returns></returns>
        public static int GetParkingSpace(int level, int getUpLevel)
        {
            return GetParkingSpace(level + getUpLevel) - GetParkingSpace(level);
        }
        /// <summary>
        /// 停车位数量最大位置升级消耗
        /// </summary>
        /// <returns></returns>
        public static System.Numerics.BigInteger GetUpGradeNumberConsumption(int level = -1)
        {   /*   Y=base*停售消耗基数【停车位数期望等级】*加成预期【停车位数期望等级】   */
            if (level == -1)
            {
                level = GlobalDataManager.GetInstance().playerData.GetParkingCenterDataIDIndexOfDataIdx().parkingSpaceLevel;
            }
            int expectlevel = ParkingSpaceExpectLevel(level);
            int sceneID = GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID;
            Config.parkingCell parkingCell = GlobalDataManager.GetInstance().logicTableParkingData.GetParkingCell(sceneID);
            string str = parkingCell.spaceupcastbase;
            System.Numerics.BigInteger baseVal = System.Numerics.BigInteger.Parse(str);
            var computePowFloatToBigInteger = PlayerDataModule.GetComputePowFloatToBigInteger(parkingCell.basenumber, expectlevel - 1);
            int number = (int)(PlayerDataModule.GetAdditionExpect(sceneID, expectlevel) * 100);

            var price = baseVal * computePowFloatToBigInteger.str_numerator * number / (100* computePowFloatToBigInteger.str_denominator);
            return price;
        }


        /// <summary>
        /// 停车场利润提升   返回值扩大100倍
        /// </summary>
        /// <param name="level">停车场利润等级</param>
        /// <returns></returns>
        public static int GetParkingProfit(int level = -1)
        {   /*  Y=0.05*lv   */
            if (level == -1)
            {
                level = GlobalDataManager.GetInstance().playerData.GetParkingCenterDataIDIndexOfDataIdx().parkingProfitLevel;
            }
            int number = 1 * level;
            return number;
        }
        /// <summary>
        /// 利润 升级若干规模的变化值
        /// </summary>
        /// <param name="level">等级</param>
        /// <param name="BasicPrice">等级变换</param>
        /// <returns></returns>
        public static int GetParkingProfit(int level, int getUpLevel)
        {
            int price;
            price = GetParkingProfit(level + getUpLevel) - GetParkingProfit(level);
            return price;
        }

        /// <summary>
        /// 停车场利润升级消耗
        /// </summary>
        /// <returns></returns>
        public static System.Numerics.BigInteger GetUpGradeParkingProfitConsumption(int level = -1)
        {   /*   Y=base*停售消耗基数【停车位数期望等级】*加成预期【停车位数期望等级】   */
            if (level == -1)
            {
                level = GlobalDataManager.GetInstance().playerData.GetParkingCenterDataIDIndexOfDataIdx().parkingProfitLevel;
            }
            int expectLevel = ParkingProfitExpectLevel(level);
            int sceneID = GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID;
            Config.parkingCell parkingCell = GlobalDataManager.GetInstance().logicTableParkingData.GetParkingCell(sceneID);
            string str = parkingCell.depletebase;
            System.Numerics.BigInteger baseVal = System.Numerics.BigInteger.Parse(str);
            var computePowFloatToBigInteger = PlayerDataModule.GetComputePowFloatToBigInteger(parkingCell.basenumber, expectLevel - 1);
            int additionExpect = (int)(PlayerDataModule.GetAdditionExpect(sceneID, expectLevel) * 100);

            var price = baseVal * computePowFloatToBigInteger.str_numerator * additionExpect / (100* computePowFloatToBigInteger.str_denominator);
            //LogWarp.LogErrorFormat("测试：  depbase ={0},分子={1},AA={2},加成预期={3},等级={4}",
            //   baseVal, computePowFloatToBigInteger.str_numerator, parkingCell.basenumber, additionExpect, level);
            return price+level;
        }
        /// <summary>
        /// 停车场升级花费  升级若干规模的变化值   
        /// </summary>
        /// <param name="level">当前等级</param>
        /// <param name="getUpLevel">升级规模的数量</param>
        /// <returns></returns>
        public static BigInteger GetUpGradeParkingProfitConsumption(int level, int changeNumber)
        {
            var price = System.Numerics.BigInteger.Parse("0");
            for (int i = 0; i < changeNumber; i++)
            {
                price = price + GetUpGradeParkingProfitConsumption(level + i);
            }
            return price;
        }

        /// <summary>
        /// 地面停车场的停车位数量
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static int GetGroundParkingNumber(int level=-1)
        {
            /*  Y=min（base+lv*6，48）   */
            if (level == -1)
            {
                level = GlobalDataManager.GetInstance().playerData.GetParkingCenterDataIDIndexOfDataIdx().parkingSpaceLevel;
            }
            int sceneID = GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID;
            Config.parkingCell parkingCell = GlobalDataManager.GetInstance().logicTableParkingData.GetParkingCell(sceneID);
            int baseVal = parkingCell.spacebase;
            int number01 = Config.globalConfig.getInstace().NumGroundParkingGroupSpace * Config.globalConfig.getInstace().MaxNumGroundParkingGroup;
            int number = Mathf.Min(baseVal + level * 6, number01);
            return number;
        }

        /// <summary>
        /// 来客速度期望等级
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static int ParkingEnterCarSpawnExpectLevel(int level = -1)
        {  //Y=1+12*（lv-1）
            
            return 1+12*(level-1);
        }
        /// <summary>
        /// 停车位数期望等级
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static int ParkingSpaceExpectLevel(int level = -1)
        {   //Y=MIN(44*(lv-1),308)+MIN(MAX(64*(lv-8),0),380)+MIN(MAX(6*(lv-14),0),30)+1
            int number01 = Mathf.Min(44 * (level - 1), 308);

            int number02 = Mathf.Min(Mathf.Max(64 * (level - 8), 0), 308);

            int number03 = Mathf.Min(Mathf.Max(6 * (level - 14), 0), 30);

            return number01 + number02 + number03 + 1;

            ////Y = 30 *（LV - 1）+1

            //return 30 * (level - 1) + 1;
        }
        /// <summary>
        /// 利润提升期望等级
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static int ParkingProfitExpectLevel(int level = -1)
        {   //Y=lv
            return level;
        }

        /// <summary>
        /// 获取一个浮点数的整数，向上取整
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        static int GetInteger(float number)
        {
            int number1 = (int)(number + 0.999999f);
            return number1;
        }


    }
}
