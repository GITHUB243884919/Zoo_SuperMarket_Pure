using Game.GlobalData;
using System.Collections;
using System.Collections.Generic;
using UFrame.Logger;
using UnityEngine;

namespace Game
{
    public partial class LittleZooModule : GameModule
    {
        /// <summary>
        /// 动物期望等级
        /// </summary>
        /// <param name="animalID"></param>
        /// <returns></returns>
        public static int GetAnimalExpectLevel(int animalID)
        { // Y = animallvbase+Dvalue
            int animallvbase = Config.animalupConfig.getInstace().getCell(animalID).animallvbase;
            float dvalue = Config.animalupConfig.getInstace().getCell(animalID).Dvalue;
            var PlayerAnimal = GlobalDataManager.GetInstance().playerData.GetPlayerAnimalData();
            int animalLevel = 10;
            return (int)(animallvbase + dvalue*(animalLevel - 1));
        }

        /// <summary>
        /// 动物升级消耗价格   Y=base*1.06^（lvsum-1）
        /// </summary>
        /// <param name="number">购买数量</param>
        public static  int GetAnimalUpLevelPriceFormula(int animalID)
        {   //Y=base
            int AnimalPriceBase = Config.globalConfig.getInstace().AnimalPriceBase;
            return AnimalPriceBase;

        }

        /// <summary>
        /// 动物栏产出基数
        /// </summary>
        /// <param name="littleZooID"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static float GetLittleZooBaseTollPrice(int littleZooID, int level)
        {   /* Y=0.1*lv+0.9  */
            return (0.1f * level + 0.9f);
        }


        /// <summary>
        /// 动物栏门票：  
        /// </summary>
        /// <param name="level">动物栏等级</param>
        /// <returns></returns>
        public static System.Numerics.BigInteger GetLittleZooPrice(int littleZooID,int level,bool isNeedMulBuff =false)
        {   /*      Y=pricebase*等级段加成*动物栏产出基数*场景加成*BUFF加成+MIN(5*lv,500)    */
            var buildupCell = Config.buildupConfig.getInstace().getCell(littleZooID);
            int sceneID = buildupCell.scene;
            //基础值
            System.Numerics.BigInteger pricebase = System.Numerics.BigInteger.Parse(buildupCell.pricebase);
            //等级段加成
            var lvAddition = GetLevelAddition(buildupCell, level);
            //动物栏产出基数
            var baseTollPrice = GetLittleZooBaseTollPrice(littleZooID,level);
            //场景加成
            int doublenum = Config.sceneConfig.getInstace().getCell(sceneID).doublenum;
            //buff加成
            float buffAll = PlayerDataModule.PlayerRatioCoinInComeAll(sceneID, isNeedMulBuff);
            //MIN(5*lv,500)
            int idx_2 = level;

            var price = pricebase * lvAddition* doublenum* GetInteger( baseTollPrice  * buffAll*100)/100 + idx_2;

            //LogWarp.LogErrorFormat("测试：pricebase={0}，等级段加成={1}，动物栏产出基数={2}，场景加成={3}，BUFF加成={4}",
            //    pricebase, lvAddition, baseTollPrice, doublenum, buffAll);
            return price;
        }
        /// <summary>
        /// 动物栏门票   返回的是多次升级变化值
        /// </summary>
        /// <param name="level">等级</param>
        /// <param name="allbase">初始产出</param>
        /// <param name="getUpLevel">升级规模</param>
        /// <returns></returns>
        public static System.Numerics.BigInteger GetLittleZooPrice(int littleZooID,int level, int changeNumber)
        {
            return GetLittleZooPrice(littleZooID,level + changeNumber) - GetLittleZooPrice(littleZooID,level);
        }

        ///// <summary>
        ///// 动物升级消耗基数   
        ///// </summary>
        ///// <param name="littleZooID">动物栏ID</param>
        ///// <param name="level">初始基数</param>   
        ///// <returns></returns>
        //public static System.Numerics.BigInteger GetUpGradeBaseConsumption(int littleZooID,int level)
        //{
        //    //         Y=basenumber^(lv-1)+1
        //    var basenumber = Config.buildupConfig.getInstace().getCell(littleZooID).basenumber;

        //    var computePowFloatToBigInteger = PlayerDataModule.GetComputePowFloatToBigInteger(basenumber,level-1);
        //    var number = computePowFloatToBigInteger.str_numerator / computePowFloatToBigInteger.str_denominator + 1;

        //    return number;
        //}


        /// <summary>
        /// 动物栏门票价格升级消耗
        /// </summary>
        /// <param name="littleZooID"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static System.Numerics.BigInteger GetUpGradeConsumption(int littleZooID, int level)
        {   /*  Y=castbase*动物消耗基数【动物栏门票期望等级】*加成预期【动物栏门票期望等级】*0.7 + lv    */
            int isLevel = level;
            var str = Config.buildupConfig.getInstace().getCell(littleZooID).castbase;
            System.Numerics.BigInteger allbase = System.Numerics.BigInteger.Parse(str);
            int sceneID = Config.buildupConfig.getInstace().getCell(littleZooID).scene;

            var expectLevel = GetUpLittleZooPriceExpectLevel(littleZooID, level);

            var basenumber = Config.buildupConfig.getInstace().getCell(littleZooID).basenumber;
            var computePowFloatToBigInteger = PlayerDataModule.GetComputePowFloatToBigInteger(basenumber, level - 1);

            //System.Numerics.BigInteger upGradeBaseConsumption = GetUpGradeBaseConsumption(littleZooID, level);

            float additionExpect = PlayerDataModule.GetAdditionExpect(sceneID, expectLevel);
            int number1 = (int)((additionExpect*0.7f) * 100);

            System.Numerics.BigInteger price = computePowFloatToBigInteger.str_numerator * allbase * number1 / (computePowFloatToBigInteger.str_denominator * 100);
            //LogWarp.LogErrorFormat("测试：动物栏={0} 期望等级={1}，depbase ={2},底={3},加成预期={4},分子={5},",
            //                              littleZooID, expectLevel, allbase, basenumber, additionExpect, computePowFloatToBigInteger.str_numerator);
            return price +level;
        }


        /// <summary>
        /// 当前动物栏开启的观光点数量   
        /// </summary>
        /// <returns></returns>
        public static int OpenVisitPosNumber(int littleZooID, int littleZooLevel)
        {   /*      Y=watchbase+lv-1            */
            int baseVal = Config.buildupConfig.getInstace().getCell(littleZooID).watchbase;
            int number = baseVal + littleZooLevel - 1;

            return number;
        }

        /// <summary>
        /// 当前动物栏的观光点开启数量  返回的是变化值
        /// </summary>
        /// <param name="littleZooLevel"></param>
        /// <param name="getUpLevel"></param>
        /// <returns></returns>
        public static int OpenVisitPosNumber(int littleZooID, int littleZooLevel, int getUpLevel)
        {
            int number = OpenVisitPosNumber(littleZooID,littleZooLevel + getUpLevel) - OpenVisitPosNumber(littleZooID,littleZooLevel);
            return number;
        }

        /// <summary>
        /// 当前动物栏的观光点数量升级花费
        /// </summary>
        /// <param name="littleZooID"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static System.Numerics.BigInteger GetUpGradeVisitorLocationLevelConsumption(int littleZooID, int level)
        {   /* Y=watchupbase*期望等级消耗基数【观光点期望等级】*加成预期【观光点期望等级】 */
            string baseVal = Config.buildupConfig.getInstace().getCell(littleZooID).watchupbase;
            int expectlevel = GetUpGradeVisitorLocationExpectLevel(littleZooID, level);
            System.Numerics.BigInteger number01 = System.Numerics.BigInteger.Parse(baseVal);

            var basenumber = Config.buildupConfig.getInstace().getCell(littleZooID).basenumber;
            var computePowFloatToBigInteger = PlayerDataModule.GetComputePowFloatToBigInteger(basenumber, expectlevel - 1);

            int sceneID = GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID;
            int number03 = (int)(PlayerDataModule.GetAdditionExpect(sceneID, expectlevel) *100);
            var price = number01 * computePowFloatToBigInteger.str_numerator * number03/(100* computePowFloatToBigInteger.str_denominator);

            //LogWarp.LogErrorFormat("  baseVal={0}, expectlevel={1}, number02={2}, number03={3} ", baseVal, expectlevel, number02, number03);

            return price;
        }


        /// <summary>
        /// 观光速率  
        /// </summary>
        /// <param name="littleZooID">动物栏ID</param>
        /// <param name="level">等级</param>
        /// <returns></returns>
        public static float GetVisitDurationMS(int littleZooID, int level)
        {   /*  Y=timebase+（lv-1）*base/20    */
            var allbase = Config.buildupConfig.getInstace().getCell(littleZooID).timebase;
            float fDuration = allbase+ (level - 1)*allbase/20f;
            return fDuration;
        }
        /// <summary>
        /// 观光速率   返回的是变化值
        /// </summary>
        /// <param name="littleZooID">动物栏ID</param>
        /// <param name="level"></param>
        /// <param name="getUpLeve"></param>
        /// <returns></returns>
        public static float GetVisitDurationMS(int littleZooID,int level, int getUpLeve)
        {
            return GetVisitDurationMS(littleZooID,level + getUpLeve) - GetVisitDurationMS(littleZooID,level);
        }
        /// <summary>
        /// 当前动物栏的观光游客数量升级花费
        /// </summary>
        /// <param name="littleZooID"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static System.Numerics.BigInteger GetUpGradeEnterVisitorSpawnLevelConsumption(int littleZooID, int level)
        {
            // Y=timeupbase*basenumber1^(lv-1)*加成预期【动物栏速率期望等级】
            string baseVal = Config.buildupConfig.getInstace().getCell(littleZooID).timeupbase;
            System.Numerics.BigInteger number01 = System.Numerics.BigInteger.Parse(baseVal);
            int expectlevel = GetUpVisitDurationExpectLevel(littleZooID, level);

            var basenumber = Config.buildupConfig.getInstace().getCell(littleZooID).basenumber1;
            var computePowFloatToBigInteger = PlayerDataModule.GetComputePowFloatToBigInteger(basenumber, level - 1);

            int sceneID = Config.buildupConfig.getInstace().getCell(littleZooID).scene;
            int number03 = (int)(PlayerDataModule.GetAdditionExpect(sceneID, expectlevel) * 100);
            var price = number01 * computePowFloatToBigInteger.str_numerator * number03 / (100* computePowFloatToBigInteger.str_denominator);
            return price;
        }


        /// <summary>
        /// 观光CD时间   Y=60/观光速率
        /// </summary>
        /// <returns></returns>
        public static float GetComeVisitorSpeedCD(int littleZooID, int level)
        {
            //            return (int)(60f/GetVisitDurationMS(littleZooID,level))*1000;
            var cdTime = 60 / GetVisitDurationMS(littleZooID,level) *1000;
            return (int)cdTime;
        }

        /// <summary>
        /// 动物栏展示每分钟观光游客数量  Y=观光速率*观光点数
        /// </summary>
        /// <returns></returns>
        public static float GetLittleZooVisitorNumberMS(int littleZooID, LittleZooModuleDataMSS littleZooModuleData)
        {
            var visitorNumber = GetVisitDurationMS(littleZooID, littleZooModuleData.littleZooEnterVisitorSpawnLevel) * OpenVisitPosNumber(littleZooID, littleZooModuleData.littleZooVisitorSeatLevel);
            return visitorNumber;
        }

        /// <summary>
        /// 动物加成
        /// </summary>
        public static float GetAllAnimalsBuff()
        {   //Y = 拥有的动物类型数量 * 0.9 + 动物类型等级和 * 10%
            var animalMSS15 = GlobalDataManager.GetInstance().playerData.playerZoo.animalMSS15;
            int animal_number = animalMSS15.animalProps.Count;
            int animal_allLv = animalMSS15.sumLv;
            float allAnimalsBuff = animal_number * 0.9f + animal_allLv * 0.1f;
            return allAnimalsBuff;
        }

        /// <summary>
        /// 单个动物加成算法
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static int GetAnimalsBuff(int animalID)
        {   /*   Y=90%+本类型lv*10%   */
            var animalProp = GlobalDataManager.GetInstance().playerData.playerZoo.animalMSS15.GetAnimalProp(animalID);
            int idx =100;
            if (animalProp !=null)
            {
                int level = animalProp.lv;
                idx = 90 + level * 10;
            }
            return idx;
        }

        /// <summary>
        /// 动物栏开启展示收益  Y=初始产出*观光速率
        /// </summary>
        /// <param name="littleZooID"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static System.Numerics.BigInteger GetlittleZooShowExpenditure(int littleZooID, int level)
        {
            var number = System.Numerics.BigInteger.Parse( Config.buildupConfig.getInstace().getCell(littleZooID).pricebase);
            var visitorNumber = (number * GetInteger(GetVisitDurationMS(littleZooID, level)*100))/100;
            return visitorNumber;
        }

        /// <summary>
        /// 观光点期望等级
        /// </summary>
        /// <param name="littleZooID"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static int GetUpGradeVisitorLocationExpectLevel(int littleZooID, int level)
        {   /*   y=watchlvbase+watchlvcoefficient*(lv-1)     */
            int watchlvbase = Config.buildupConfig.getInstace().getCell(littleZooID).watchlvbase;
            int watchlvcoefficient = Config.buildupConfig.getInstace().getCell(littleZooID).watchlvcoefficient;
            return watchlvbase+watchlvcoefficient * (level-1);
        }
        /// <summary>
        /// 动物栏速率期望等级
        /// </summary>
        /// <param name="littleZooID"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static int GetUpVisitDurationExpectLevel(int littleZooID, int level)
        {   /*   y=timelvbase+timelvcoefficient*(lv-1)    */
            int timelvbase = Config.buildupConfig.getInstace().getCell(littleZooID).timelvbase;
            int timelvcoefficient = Config.buildupConfig.getInstace().getCell(littleZooID).timelvcoefficient;
            return timelvbase + timelvcoefficient * (level-1);
        }
        /// <summary>
        /// 动物栏门票期望等级
        /// </summary>
        /// <param name="littleZooID"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public static int GetUpLittleZooPriceExpectLevel(int littleZooID, int level)
        {   /*  Y=lvcoefficient+lv*lvcoefficient1 */
            var cell = Config.buildupConfig.getInstace().getCell(littleZooID);
            var idx = cell.lvcoefficient + level * cell.lvcoefficient1;
            return (int)idx;
        }

        /// <summary>
        /// 动物栏等级段加成
        /// </summary>
        /// <returns></returns>
        public static System.Numerics.BigInteger GetLevelAddition(Config.buildupCell buildupCell, int level)
        {
            var lvshage = buildupCell.lvshage;
            int idx = PlayerDataModule.FindLevelRangIndex01(lvshage, level);
            var idx01 = System.Numerics.BigInteger.Parse("1");
            for (int i = 0; i <= idx; i++)
            {
                idx01 = idx01 * buildupCell.lvshagecoefficient[i];
            }
            //LogWarp.LogErrorFormat(" 测试：动物栏 Lv={0}，下标={1} 等级段加成={2}", level, idx, idx01);
            return idx01;
        }


        /// <summary>
        /// 获取一个浮点数的整数，向上取整
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        static int GetInteger(float number)
        {
            int number1 = (int)(number+0.999999f);
            return number1;
        }
        /// <summary>
        /// 四舍五入
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        static int GetInteger_RoundingOff(float number)
        {
            float number01 = number + 0.5f;
            return (int)number01;
        }

    }
   
}
