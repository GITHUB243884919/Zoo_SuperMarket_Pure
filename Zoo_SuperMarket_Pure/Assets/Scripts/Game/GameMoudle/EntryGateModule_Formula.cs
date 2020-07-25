using Game.GlobalData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UFrame.Logger;

namespace Game
{
    /// <summary>
    /// 动物加成   buff都没有做
    /// </summary>
    public partial class EntryGateModule : GameModule
    {

        /// <summary>
        /// 全部售票口的收费速度（返回的是分钟）
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static float GetAllEntryChargeValMs(int senceID = -1)
        { /*  Y=售票口收费速度（1）+售票口收费速度（2）+售票口收费速度（3）……+售票口收费速度（8）  */
            float number = 0;
            var entryGateList = GlobalDataManager.GetInstance().playerData.GetEntryDateDataIDIndexOfDataIdx(senceID).entryGateList;
            foreach (var item in entryGateList)
            {
                number += GetCheckinSpeed(item.entryID, item.level);
            }
            return number;
        }


        /// <summary>
        /// 售票口收费速度（单）   Y=base+0.1*lv  
        /// </summary>
        /// <param name="baseVal"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static float GetCheckinSpeed(int entryID, int level)
        { /*   y=speedbase+（lv-1）*speedratio    */
            float speedbase = Config.ticketConfig.getInstace().getCell(entryID).speedbase;
            float speedratio = Config.ticketConfig.getInstace().getCell(entryID).speedratio;
            float number = (level-1)* speedratio;
            return speedbase + number;
        }
        /// <summary>
        /// 售票口收费速度（单）升级变化值   Y=base+0.1*lv  
        /// </summary>
        /// <param name="entryID"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static float GetCheckinSpeed(int entryID, int level, int changeNumber)
        {
            //Logger.LogWarp.LogErrorFormat("测试： 当前   售票口 ID={0}   ", entryID);

            var number = GetCheckinSpeed(entryID, level + changeNumber) - GetCheckinSpeed(entryID, level);
            return number;
        }
        /// <summary>
        /// 售票口cd时间(单位秒)   Y=60/售票口收费速度（单）
        /// 转毫秒 * 1000
        /// </summary>
        /// <param name="entryID"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static int GetCheckinCDValMs(int entryID, int level)
        {
            //Logger.LogWarp.LogErrorFormat("测试： 当前   售票口 ID={0}   ", entryID);

            return (int)(60 * 1000 / GetCheckinSpeed(entryID, level));
        }
        /// <summary>
        /// 升级收费速度的花费  Y=lv*10
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>


        /// <summary>
        /// 每分钟购票顾客升级消耗
        /// </summary>
        /// <param name="entryID"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static System.Numerics.BigInteger GetUpGradeCheckinSpeedConsumption(int entryID, int level)
        {   /*  Y=lvupcastbase*lvupcastratio^(lv-1)*加成预期【进客速度期望等级】+lv   */

            Config.ticketCell ticketCell= Config.ticketConfig.getInstace().getCell(entryID);
            System.Numerics.BigInteger baseVal = System.Numerics.BigInteger.Parse(ticketCell.lvupcastbase);
            int expectlevel = GetUpVisitorSpeedExpectLevel(entryID, level);
            float lvupcastratio = ticketCell.lvupcastratio;
            var computePowFloatToBigInteger = PlayerDataModule.GetComputePowFloatToBigInteger(lvupcastratio, level - 1);

            int sceneID = ticketCell.scene;
            int number = (int)(PlayerDataModule.GetAdditionExpect(sceneID, expectlevel) * 100);
            var price = baseVal * computePowFloatToBigInteger.str_numerator * number / (computePowFloatToBigInteger.str_denominator*100);
            //Logger.LogWarp.LogErrorFormat("测试：baseVal={0}  ，baseConsumption={1} number={2}", baseVal, baseConsumption, PlayerDataModule.GetAdditionExpect(level));
            return price + level;
        }

        ///// <summary>
        ///// 售票口基础产出公式：Y=base*0.05*1.07^((lv*1.6)-1)
        ///// </summary>
        ///// <returns></returns>
        //public static System.Numerics.BigInteger GetEntryBaseTollPrice(int level)
        //{
        //    if (level<1)
        //    {
        //        //Logger.LogWarp.LogError("等级为0");
        //        level = 1;
        //    }
        //    int baseprice = Config.ticketConfig.getInstace().getCell(1).pricebase;
        //    int number = (int)(level * 1.6f) - 1;
        //    //1.07^((lv*1.6)-1)
        //    var numerator = System.Numerics.BigInteger.Parse("107");
        //    numerator = System.Numerics.BigInteger.Pow(numerator, number);
        //    var denominator = System.Numerics.BigInteger.Parse("100");
        //    denominator = System.Numerics.BigInteger.Pow(denominator, number);

        //    //base * 0.05 * numerator/(denominator*100)
        //    var price = numerator * baseprice*5 / (denominator*100);

        //    return price;
        //}
        /// <summary>
        /// 售票口门票价格
        /// </summary>
        /// <returns></returns>
        public static System.Numerics.BigInteger GetEntryPrice(int level, int sceneID, bool noBuffAffect)
        {   //Y=pricebase*等级段加成*（0.1*lv+0.9）*场景加成*BUFF加成+MIN(5*lv,500)
            if (sceneID == -1)
            {
                sceneID = GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID;
            }
            var allData = Config.ticketConfig.getInstace().AllData;
            Config.ticketCell ticketCell = null;
            foreach (var item in allData)
            {
                if (item.Value.scene == sceneID)
                {
                    ticketCell = item.Value;
                    break;
                }
            }
            //pricebase
            var pricebase = System.Numerics.BigInteger.Parse(ticketCell.pricebase);
            //等级段加成
            var lvAddition = GetLevelAddition(ticketCell, level);
            //（0.1*lv+0.9）
            float idx_1 = 0.1f * level + 0.9f;
            //场景加成
            int doublenum = Config.sceneConfig.getInstace().getCell(sceneID).doublenum;
            //BUFF加成
            float buffAll = PlayerDataModule.PlayerRatioCoinInComeAll(sceneID, noBuffAffect);
            //MIN(5*lv,500)
            int idx_2 = level;
            var price = pricebase * doublenum* lvAddition* GetInteger( idx_1  * buffAll*100)/100 + idx_2;

            //LogWarp.LogErrorFormat("测试：pricebase={0}，等级段加成={1}，产出基数={2}，场景加成={3}，BUFF加成={4}，MIN(5*lv,500)={5}",
            //    pricebase, lvAddition, idx_1, doublenum, buffAll, idx_2);
            return price;
        }
        /// <summary>
        /// 获取升级规模后售票口票价变化  
        /// </summary>
        /// <returns></returns>
        public static System.Numerics.BigInteger GetEntryPrice_Add(int level, int changeNumber, int sceneID)
        {

            var price = GetEntryPrice(level + changeNumber, sceneID, true) - GetEntryPrice(level, sceneID, true);
            return price;
        }



        /// <summary>
        /// 售票口门票升级 
        /// </summary>
        /// <param name="expectLevel"></param>
        /// <returns></returns>
        public static System.Numerics.BigInteger GetUpGradeConsumption(int level)
        {
            /*  Y=depbase*停售消耗基数【门票升级期望等级】*加成预期【门票升级期望等级】   */
            Config.ticketCell ticketCell = Config.ticketConfig.getInstace().getCell(sortEntryGateIDs[0]);
            string depbase = ticketCell.depbase;
            int sceneID = ticketCell.scene;

            System.Numerics.BigInteger baseVal = System.Numerics.BigInteger.Parse(depbase);
           int expectLevel = GetUpTicketExpectLevel(level);
            float basenumber = ticketCell.basenumber;
            var computePowFloatToBigInteger = PlayerDataModule.GetComputePowFloatToBigInteger(basenumber, level + 1);
            int additionExpect = (int)(PlayerDataModule.GetAdditionExpect(sceneID, expectLevel) * 100);
            var price = baseVal * computePowFloatToBigInteger.str_numerator * additionExpect / (100* computePowFloatToBigInteger.str_denominator);
            //LogWarp.LogErrorFormat("测试：  depbase ={0},分子={1},AA={2},加成预期={3},等级={4}",
            //    baseVal, computePowFloatToBigInteger.str_numerator, basenumber, additionExpect, level);
            return price + level;
        }
        /// <summary>
        /// 获取售票口门票 升级规模的花费
        /// </summary>
        /// <param name="level">当前等级</param>
        /// <param name="number">等级变化</param>
        /// <returns></returns>
        public static System.Numerics.BigInteger GetUpGradeConsumption(int level, int changeNumber)
        {
            var price = System.Numerics.BigInteger.Parse("0");
            for (int i = 0; i < changeNumber; i++)
            {
                price = price + GetUpGradeConsumption(level + i);
            }
            return price;
        }


        ///// <summary>
        ///// 售票口收费口增加价格
        ///// </summary>
        ///// <returns></returns>
        //public static System.Numerics.BigInteger GetAddEntryPrice(int entryID, int level)
        //{   /*  Y=base  */
        //    string str = Config.ticketConfig.getInstace().getCell(entryID).number;
        //    System.Numerics.BigInteger bigInteger = System.Numerics.BigInteger.Parse(str);
        //    return bigInteger;
        //}

        /// <summary>
        /// 进客速度期望等级
        /// </summary>
        /// <returns></returns>
        public static int GetUpVisitorSpeedExpectLevel(int entryID, int level)
        {   //Y=lvratio*lv+lvratio1
            float lvratio = Config.ticketConfig.getInstace().getCell(entryID).lvratio;
            int lvratio1 = Config.ticketConfig.getInstace().getCell(entryID).lvratio1;
            float idx = lvratio * level + lvratio1;
            return GetInteger(idx);
        }
        /// <summary>
        /// 门票升级期望等级
        /// </summary>
        /// <returns></returns>
        public static int GetUpTicketExpectLevel(int level)
        {   /*      Y=lv +1       */
            return level+1;
        }

        /// <summary>
        /// 售票口等级段加成
        /// </summary>
        /// <returns></returns>
        public static System.Numerics.BigInteger GetLevelAddition(Config.ticketCell ticketCell, int level)
        {
            var lvshage = ticketCell.lvshage;
            int idx = PlayerDataModule.FindLevelRangIndex01(lvshage, level);
            var idx01 = System.Numerics.BigInteger.Parse("1");
            for (int i = 0; i <= idx; i++)
            {
                idx01 = idx01 * ticketCell.lvshagecoefficient[i];
            }
            //LogWarp.LogErrorFormat(" 测试：售票口  Lv={0}， 等级段加成={1}", level, idx01);
            return idx01;
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
