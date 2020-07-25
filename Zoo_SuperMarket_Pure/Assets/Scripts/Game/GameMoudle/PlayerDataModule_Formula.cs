using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using Game.GlobalData;
using UFrame.Logger;
using System;

namespace Game
{
    public partial class PlayerDataModule : GameModule
    {
        private static readonly int length;

        /// <summary>
        /// 当前所有动物栏收益和：动物栏收益 =（停车场来人速度，售票口收费速度（全），60/观光速度* 观光点数）三值取最小后* 动物栏产出
        /// </summary>
        /// <returns></returns>
        public static System.Numerics.BigInteger GetAllZooPrice(bool noMulBuffAffect = false)
        {
            //停车场来人速度
            int parkNumber = ParkingCenter.GetParkingEnterCarSpawn();
            //售票口收费速度
            float entryGateNumber = EntryGateModule.GetAllEntryChargeValMs();
            //LogWarp.LogErrorFormat("本场景 停车场来人速度{0}  售票口收费速度{1}", parkNumber, entryGateNumber);

            System.Numerics.BigInteger littleZooNumber = 0;

            PlayerData playerData = GlobalDataManager.GetInstance().playerData;
            var littleZooModuleDataList_MS = GlobalDataManager.GetInstance().playerData.playerZoo.littleZooModuleDatasMSS;
            int sceneID = GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID;
            for (int i = 0; i < littleZooModuleDataList_MS.Count; i++)
            {
                if (littleZooModuleDataList_MS[i].littleZooTicketsLevel == 0|| littleZooModuleDataList_MS[i].sceneID != sceneID)
                {
                    continue;
                }
                //每分钟动物栏基础收益=动物栏产出*min（停车场来人速度，售票口收费速度（全），60/(观光速度*观光点数)）
                //动物栏产出
                var number1 = LittleZooModule.GetLittleZooPrice(littleZooModuleDataList_MS[i].littleZooID, littleZooModuleDataList_MS[i].littleZooTicketsLevel, noMulBuffAffect);
                //观光速度*观光点数
                var number4 = LittleZooModule.GetLittleZooVisitorNumberMS(littleZooModuleDataList_MS[i].littleZooID, littleZooModuleDataList_MS[i]);
                var number5 = (int)(Mathf.Min(parkNumber, entryGateNumber, number4)) * number1;
                //LogWarp.LogErrorFormat("测试：本 parkNumber={0} entryGateNumber={1} number4={2} number1={3} ", parkNumber, entryGateNumber, number4, number1);
                littleZooNumber += number5;

            }
            return littleZooNumber;
        }

        /// <summary>
        /// 其他场景的动物栏收益和
        /// </summary>
        /// <param name="isNeedMulBuff"></param>
        /// <returns></returns>
        public static System.Numerics.BigInteger GetOtherSceneAllZooPrice(int sceneID, bool isNeedMulBuff = false)
        {
            int parkNumber = ParkingCenter.GetOtherSceneParkingEnterCarSpawn(sceneID);
            float entryGateNumber = EntryGateModule.GetAllEntryChargeValMs(sceneID);
            //LogWarp.LogErrorFormat("其他场景 停车场来人速度{0}  售票口收费速度{1}", parkNumber, entryGateNumber);

            System.Numerics.BigInteger littleZooNumber = 0;

            PlayerData playerData = GlobalDataManager.GetInstance().playerData;
            var littleZooModuleDataList_MS = GlobalDataManager.GetInstance().playerData.playerZoo.littleZooModuleDatasMSS;
            //int sceneID = GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID;
            for (int i = 0; i < littleZooModuleDataList_MS.Count; i++)
            {
                if (littleZooModuleDataList_MS[i].littleZooTicketsLevel == 0|| littleZooModuleDataList_MS[i].sceneID != sceneID)
                {
                    continue;
                }
                //每分钟动物栏基础收益=动物栏产出*min（停车场来人速度，售票口收费速度（全），60/(观光速度*观光点数)）
                //动物栏产出
                var number1 = LittleZooModule.GetLittleZooPrice(littleZooModuleDataList_MS[i].littleZooID, littleZooModuleDataList_MS[i].littleZooTicketsLevel, isNeedMulBuff);
                //观光速度*观光点数
                var number4 = LittleZooModule.GetLittleZooVisitorNumberMS(littleZooModuleDataList_MS[i].littleZooID, littleZooModuleDataList_MS[i]);
                var number5 = (int)(Mathf.Min(parkNumber, entryGateNumber, number4)) * number1;
                //LogWarp.LogErrorFormat("测试：非 parkNumber={0} entryGateNumber={1} number4={2} number1={3} ",parkNumber, entryGateNumber, number4,number1);
                littleZooNumber += number5;

            }
            return littleZooNumber;
        }


        /// <summary>
        /// 当前每分钟产出      
        /// 所有动物栏收益+售票口票价*min（停车场来人速度，售票口收费速度（全））
        /// </summary>
        /// <returns></returns>
        public static System.Numerics.BigInteger CurrScenePerMinCoin(bool isNeedMulBuff =false)
        {
            //所有动物栏收益
            var allZooPrice = GetAllZooPrice(isNeedMulBuff );
            //每分钟售票口基础收益=售票口票价*min（停车场来人速度，售票口收费速度（全））
            //售票口票价：
            int sceneID = GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID;
            var entryPrice = EntryGateModule.GetEntryPrice(GlobalDataManager.GetInstance().playerData.GetEntryDateDataIDIndexOfDataIdx().entryTicketsLevel, sceneID, isNeedMulBuff );
            //min（停车场来人速度，售票口收费速度（全））
            var number = Mathf.Min(ParkingCenter.GetParkingEnterCarSpawn(), EntryGateModule.GetAllEntryChargeValMs());
            //所有动物栏收益+售票口票价*min（停车场来人速度，售票口收费速度（全））
            System.Numerics.BigInteger coin = allZooPrice + (entryPrice * (int)(number * 100)) / 100;
            //Logger.LogWarp.LogErrorFormat("测试：当前场景  每分钟 收益    allZooPrice={0},    entryPrice={1},    number={2}", allZooPrice ,entryPrice ,number );
            return coin;
        }

        /// <summary>
        /// 其他场景产出每分钟产     
        /// 所有动物栏收益+售票口票价*min（停车场来人速度，售票口收费速度（全））
        /// </summary>
        /// <returns></returns>
        public static System.Numerics.BigInteger LeaveScenePerMinCoin(int sceneID, bool isNeedMulBuff = false)
        {
            System.Numerics.BigInteger coin = 0;
            var playerData = GlobalDataManager.GetInstance().playerData;
            var scenePlayerData = playerData.playerZoo.scenePlayerDataMSS;
            if (scenePlayerData == null)
            {
                return coin;
            }
            var sceneStates = scenePlayerData.sceneStates;
            if (sceneStates == null)
            {
                return coin;
            }
            for (int i = 0; i < sceneStates.Count; i++)
            {
                var sceneState = sceneStates[i];
                if ((sceneState.enterCount > 0 || sceneState.sceneId == GameConst.First_SceneID) &&
                    sceneState.sceneId != playerData.playerZoo.currSceneID &&
                    sceneState.sceneId == sceneID)
                {
                    //所有动物栏收益
                    var allZooPrice = GetOtherSceneAllZooPrice(sceneID, isNeedMulBuff);
                    //每分钟售票口基础收益=售票口票价*min（停车场来人速度，售票口收费速度（全））
                    //售票口票价：
                    var entryPrice = EntryGateModule.GetEntryPrice(GlobalDataManager.GetInstance().playerData.GetEntryDateDataIDIndexOfDataIdx(sceneID).entryTicketsLevel,sceneID,isNeedMulBuff);
                    //min（停车场来人速度，售票口收费速度（全））
                    var number = Mathf.Min(ParkingCenter.GetOtherSceneParkingEnterCarSpawn(sceneID), EntryGateModule.GetAllEntryChargeValMs(sceneID));
                    //所有动物栏收益+售票口票价*min（停车场来人速度，售票口收费速度（全））
                    coin = allZooPrice + (entryPrice * (int)(number * 100)) / 100;
                    //Logger.LogWarp.LogErrorFormat("测试：其他场景{0}   每分钟 收益    allZooPrice={1},    entryPrice={2},    number={3}", sceneID, allZooPrice, entryPrice, number);
                }
            }
            
            return coin;
        }        

        /// <summary>
        /// 计算离线收益（包括buff等计算）
        /// </summary>
        /// <returns></returns>
        public static System.Numerics.BigInteger OnGetCalculateOfflineSecondCoin(long offlineTime)
        {
            //获取离线Buff的列表
            List<Buff> offlineBuffList = GlobalDataManager.GetInstance().playerData.playerZoo.offlineBuffList;
            //离线buff按照时间的长短排序
            var orderBuffList = OnGetBuffCoefficient(offlineBuffList);
            int sceneID = GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID;
            //每秒收益  带false是不算翻倍buff
            var baseEarnings = AllScenePerMinCoin(false) / 60;

            BigInteger buffParce = 0; 
            BigInteger noBuffParce = 0;
            
            if (orderBuffList.Count > 0)
            {
                if (offlineTime >= orderBuffList[orderBuffList.Count - 1].CD.org)
                {
                    noBuffParce = (offlineTime - (int)orderBuffList[orderBuffList.Count - 1].CD.org) * baseEarnings;
                }
                buffParce = OnCalculateCoin(baseEarnings, orderBuffList);
                return buffParce + noBuffParce;
            }
            else
            {
                buffParce = baseEarnings * offlineTime;
                return buffParce;
            }
        }

        /// <summary>
        /// 当前岛屿的所有场景/m收益
        /// </summary>
        /// <param name="isNeedMulBuff">false为不需要增益buff</param>
        /// <returns></returns>
        public static System.Numerics.BigInteger AllScenePerMinCoin(bool isNeedMulBuff)
        {
            var playerData = GlobalDataManager.GetInstance().playerData;
            int idx = Config.sceneConfig.getInstace().getCell(playerData.playerZoo.currSceneID).scenetype;
            //当前场景/m收益
            var big = CurrScenePerMinCoin(isNeedMulBuff);
            //非当前场景的离线金币奖励
            var sceneStates = playerData.playerZoo.scenePlayerDataMSS.sceneStates;
            for (int i = 0; i < sceneStates.Count; i++)
            {
                var sceneState = sceneStates[i];
                if ((sceneState.enterCount > 0 || sceneState.sceneId == GameConst.First_SceneID) &&
                    sceneState.sceneId != playerData.playerZoo.currSceneID)
                {
                    if (Config.sceneConfig.getInstace().getCell(sceneState.sceneId).scenetype ==idx)
                    {
                        var perMinCoin = PlayerDataModule.LeaveScenePerMinCoin(sceneState.sceneId, isNeedMulBuff);
                        big += perMinCoin;
                    }
                }
            }

            return big;
        }


        /// <summary>
        /// 根据离线buff的时间进行排列
        /// </summary>
        public static List<Buff> OnGetBuffCoefficient(List<Buff> offlineBuffList)
        {
            for (int j = 1; j <= offlineBuffList.Count - 1; j++)//外层for循环用来控制子for循环执行的次数
            {
                //让下面的for循环执行length-1次
                for (int i = 0; i < offlineBuffList.Count - 1 - j + 1; i++)
                {
                    //numArray[i]  numArray[i+1]做比较 把最大的放在后面
                    if (offlineBuffList[i + 1].CD.org < offlineBuffList[i].CD.org)
                    {
                        var temp = offlineBuffList[i];
                        offlineBuffList[i] = offlineBuffList[i + 1];
                        offlineBuffList[i + 1] = temp;
                    }
                }
            }
            return offlineBuffList;
        }

        public static System.Numerics.BigInteger OnCalculateCoin(System.Numerics.BigInteger big, List<Buff> offlineBuffList, double isDouble = 0)
        {
            //时间 秒
            var time = offlineBuffList[0].CD.org - isDouble;
            //收益= time* 每分钟实际收益* buff加成
            /*   去除buff加成的每分钟实际收益: 所有动物栏收益+售票口票价*min（停车场来人速度，售票口收费速度（全）） */
            //buff加成
            float buffCalculate = PlayerRatioCoinInComeAll_Calculate(offlineBuffList);
            big = (int)(time * buffCalculate) * big;
            offlineBuffList.RemoveAt(0);
            if (offlineBuffList.Count == 0)
            {
                return big;
            }
            OnCalculateCoin(big, offlineBuffList, time);
            return big;
        }

        public static float PlayerRatioCoinInComeAll_Calculate(List<Buff> offlineBuffList)
        {
            //float buffRatioCoinInComeAdd = 1f;
            float buffRatioCoinInComeMul = 1f;
            for (int i = 0; i < offlineBuffList.Count; i++)
            {
                Buff buff = offlineBuffList[i];
                switch (buff.buffType)
                {
                    //case BuffType.RatioCoinInComeAdd:
                    //    buffRatioCoinInComeAdd += buff.buffVal;
                    //    //Logger.LogWarp.LogError("测试：   buffRatioCoinInCome= "+ buffRatioCoinInComeAdd+ "   buff.buffVal= "+ buff.buffVal);
                    //    break;
                    case BuffType.RatioCoinInComeMul:
                        buffRatioCoinInComeMul *= buff.buffVal;
                        //Logger.LogWarp.LogError("测试：   buffRatioCoinInCome= " + buffRatioCoinInComeAdd + "   buff.buffVal= " + buff.buffVal);
                        break;
                }
            }
            int level = GlobalDataManager.GetInstance().playerData.GetParkingCenterDataIDIndexOfDataIdx().parkingProfitLevel;
            int sceneID = GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID;
            //float number = LittleZooModule.GetAllAnimalsBuff(sceneID) + ParkingCenter.GetParkingProfit(level) / 100f + buffRatioCoinInComeAdd;
            return  buffRatioCoinInComeMul;
        }

        /// <summary>
        /// 轮船过来的游客数量
        /// </summary>
        /// <returns></returns>
        public static int SteameVisitorNameber()
        {  
            //base + min(int(售票口等级 / 20), 40)
            int baseVal = UnityEngine.Random.Range(Config.globalConfig.getInstace().AdvertVisitorMin, Config.globalConfig.getInstace().AdvertVisitorMax);

            PlayerData playerData = GlobalDataManager.GetInstance().playerData;
            int entryLevel = playerData.GetEntryDateDataIDIndexOfDataIdx().entryTicketsLevel;
            int numberVisitor = baseVal + Mathf.Min((entryLevel / 20),40);
            //LogWarp.LogError("测试：轮船游客 "+ baseVal+ "  entryLevel="+ entryLevel+ "  numberVisitor="+ numberVisitor);
            return numberVisitor;
        }

        /// <summary>
        /// 玩家所有产出需要相加的倍数值  y=1+动物加成+停车场利润加成+道具BUFF
        /// </summary>
        /// <returns></returns>
        public static float PlayerRatioCoinInComeAdd(int sceneID=-1)
        {
            int level = GlobalDataManager.GetInstance().playerData.GetParkingCenterDataIDIndexOfDataIdx(sceneID).parkingProfitLevel;
            float number = LittleZooModule.GetAllAnimalsBuff() + ParkingCenter.GetParkingProfit(level) / 100f + GlobalDataManager.GetInstance().playerData.playerZoo.buffRatioCoinInComeAdd;
          
            return number;
        }

        /// <summary>
        /// 玩家所有产出需要相乘的倍数值  y=1+广告buff+月卡buff+ 道具buff
        /// </summary>
        /// <returns></returns>
        public static float PlayerRatioCoinInComeMul()
        {
            float number = GlobalDataManager.GetInstance().playerData.playerZoo.buffRatioCoinInComeMul;
            return number;
        }
        /// <summary>
        /// Buff加成
        /// </summary>
        /// <returns></returns>
        public static float PlayerRatioCoinInComeAll( int sceneID, bool isNeedMulBuff)
        {   /*  Y=（1+动物加成）*（1+停车场利润加成）*（1+广告BUFF+月卡BUFF+道具BUFF）  */

            var animalBuff = 1 + LittleZooModule.GetAllAnimalsBuff();
            int level = GlobalDataManager.GetInstance().playerData.GetParkingCenterDataIDIndexOfDataIdx(sceneID).parkingProfitLevel;
            var parkingProfit = 1 + ParkingCenter.GetParkingProfit(level) / 100f;
            var timeBuff = PlayerRatioCoinInComeMul();

            if (!isNeedMulBuff)
            {   // 不需要时间段buff加成  Y=（1+动物加成）*（1+停车场利润加成）
                return animalBuff*parkingProfit;
            }
            else
            {   // Y=（1+动物加成）*（1+停车场利润加成）*（1+广告BUFF+月卡BUFF+道具BUFF）
                return animalBuff * parkingProfit * timeBuff;
            }
        }


        /// <summary>
        /// 加成预期
        /// </summary>
        /// <param name="level"></param>
        public static float GetAdditionExpect(int sceneID,int level)
        {
            float expect_parking = GetAdditionExpect_Parking(level);

            float expect_entryGate = GetAdditionExpect_Animal(level);


            return expect_parking* expect_entryGate;
        }
        /// <summary>
        /// 加成预期_停车场
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static float GetAdditionExpect_Parking(int level)
        {
            float idx = 1 + level * 0.01f;
            //LogWarp.LogErrorFormat(" 测试：加成预期_停车场    level={0},   加成={1}", level, idx);
            return idx;
        }
        /// <summary>
        /// 加成预期_动物培养
        /// </summary>
        /// <returns></returns>
        public static float GetAdditionExpect_Animal(int level)
        {
            /* Y=expefficient+max（（lv【期望等级】-350）/100，0）+MIN(【期望等级】/20,25)+MIN(MAX((【期望等级】-500),0)/50,25) */

            int sceneID = GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID;
            var expefficient = Config.sceneConfig.getInstace().getCell(sceneID).expefficient;
            int idx = (int)Mathf.Max((level - 350) / 100f, 0);

            float idx01 =((int)Mathf.Min(level / 20f, 25)) * Mathf.Min(level/150f,1);

            int idx02 = (int)Mathf.Min(Mathf.Max(level-500,0)/50f,25);

            return expefficient + idx + idx01 + idx02;
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

        /// <summary>
        /// 大数据对数计算
        /// </summary>
        /// <param name="idx_numerator">分子</param>
        /// <param name="idx_denominator">分母</param>
        /// <param name="exponent">对数</param>
        /// <returns></returns>
        public static ComputePowFloatToBigInteger GetComputePowFloatToBigInteger(float baseVar, int exponent)
        {
            int idx_numerator = GetInteger(baseVar * 1000);
            int idx_denominator = 1000;
            var numerator = System.Numerics.BigInteger.Parse(idx_numerator.ToString());
            numerator = System.Numerics.BigInteger.Pow(numerator, exponent);
            var denominator = System.Numerics.BigInteger.Parse(idx_denominator.ToString());
            denominator = System.Numerics.BigInteger.Pow(denominator, exponent);
            ComputePowFloatToBigInteger computePowFloatToBigInteger = new ComputePowFloatToBigInteger()
            {
                str_numerator = numerator,
                str_denominator = denominator,
            };
            return computePowFloatToBigInteger;
        }


    }
    public class ComputePowFloatToBigInteger
    {
        /// <summary>
        /// 分子
        /// </summary>
        public System.Numerics.BigInteger str_numerator = System.Numerics.BigInteger.Parse("0");
        /// <summary>
        /// 分母
        /// </summary>
        public System.Numerics.BigInteger str_denominator = System.Numerics.BigInteger.Parse("0");

    }
}
