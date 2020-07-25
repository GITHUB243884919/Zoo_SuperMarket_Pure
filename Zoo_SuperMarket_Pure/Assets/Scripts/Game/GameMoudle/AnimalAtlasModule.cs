//using Game;
//using Game.GlobalData;
//using Game.MessageCenter;
//using UFrame.Logger;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics;
//using UFrame;
//using UFrame.MessageCenter;
//using UnityEngine;

//namespace Game
//{
//    public class AnimalAtlasModule : GameModule
//    {
//        public AnimalAtlasModule(int orderID) : base(orderID) { }

//        public PlayerAnimal_MSS playerAnimal { get; private set; }
//        //public int[,] animalAtlasData;
//        public override void Init()
//        {
//            playerAnimal = GlobalDataManager.GetInstance().playerData.GetPlayerAnimalData();
//            MessageManager.GetInstance().Regist((int)GameMessageDefine.SetAnimalAtlasDataMessage, this.OnSetSingleAnimalAtlasData);
            
//        }

//        public override void Release()
//        {
//            MessageManager.GetInstance().UnRegist((int)GameMessageDefine.SetAnimalAtlasDataMessage, this.OnSetSingleAnimalAtlasData);
//        }

//        public override void Tick(int deltaTimeMS)
//        {
//            if (!this.CouldRun())
//            {
//                return;
//            }
//        }

//        /// <summary>
//        /// 获取图鉴数据
//        /// </summary>
//        private void GetAnimalAtlasData()
//        {
//            //赋值:
//            var animalIDList = playerAnimal.animalID;
//            for (int i = 0; i < animalIDList.Count; i++)
//            {
//                var littleAnimal_MS = playerAnimal.getPlayerAnimalCell(animalIDList[i]);
//                //大于0代表是有动物数据
//                if (littleAnimal_MS.animalLevel > 0)
//                {
//                    var cell = Config.animalupConfig.getInstace().getCell(animalIDList[i]);
//                    int a = cell.bigtype;
//                    int b = cell.smalltype;
//                    GlobalDataManager.GetInstance().logicAnimalAtlasData.animalAtlasData[a, b] = 1;
//                }
//            }

//            int count01 = Config.animalatlasConfig.getInstace().RowNum;
//        }

//        /// <summary>
//        /// 设置单一的动物数据在图鉴Data里面的修改
//        /// </summary>
//        private void OnSetSingleAnimalAtlasData(Message obj)
//        {
//            var msg = obj as MessageInt;
//            //var cell = Config.animalupConfig.getInstace().getCell(msg.val);
//            //int a = cell.bigtype;
//            //int b = cell.smalltype;
//            //GetAnimalAtlasDataSubscript(ref a,ref b);
//            //GlobalDataManager.GetInstance().logicAnimalAtlasData.SetAnimalAtlasData(a,b);
//            //send 消息  图鉴UI改变
//            MessageInt.Send((int)GameMessageDefine.GetAnimalAtlasDataMessage, msg.val);

//        }

//        /// <summary>
//        /// 设置单一的动物数据在图鉴Data里面的修改
//        /// </summary>
//        public static void OnSetSingleAnimalAtlasData(int animalID)
//        {
//            var cell = Config.animalupConfig.getInstace().getCell(animalID);
//            int a = cell.bigtype;
//            int b = cell.smalltype;
//            GetAnimalAtlasDataSubscript(ref a, ref b);
//            GlobalDataManager.GetInstance().logicAnimalAtlasData.SetAnimalAtlasData(a, b);
//            //LogWarp.LogErrorFormat("测试： OnSetSingleAnimalAtlasData   收到消息  修改了 行={0}   列={1} ", a,b);
//            //send 消息  图鉴UI改变
//            MessageInt.Send((int)GameMessageDefine.GetAnimalAtlasDataMessage, animalID);

//        }

//        private static void GetAnimalAtlasDataSubscript( ref int a,ref int b)
//        {
//            for (int i = 0; i < Config.animalatlasConfig.getInstace().RowNum; i++)
//            {
//                var animalatlasCell = Config.animalatlasConfig.getInstace().getCell(i+1);
//                if (animalatlasCell.bigtype ==a)
//                {
//                    for (int j = 0; j < animalatlasCell.smalltypesort.Length; j++)
//                    {
//                        if (animalatlasCell.smalltypesort[j] == b)
//                        {
//                            a = i;
//                            b = j;
//                            return;
//                        }
//                    }
//                }
//            }

//        }

//    }
//}
