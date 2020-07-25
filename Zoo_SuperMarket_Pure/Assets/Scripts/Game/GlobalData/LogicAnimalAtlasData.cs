using Game.GlobalData;
using UFrame.Logger;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogicAnimalAtlasData
{
    /// <summary>
    /// 动物图鉴的多维数组data
    /// </summary>
    public int[,] animalAtlasData;

    /// <summary>
    /// 玩家收集到的动物图鉴数量
    /// </summary>
    public int animalAtlasNambe=0 ;

    public LogicAnimalAtlasData()
    {
        InitAnimalAtlasData();
    }


    /// <summary>
    /// 初始化动物图鉴的数据
    /// </summary>
    protected void InitAnimalAtlasData()
    {
        int count01 = Config.animalatlasConfig.getInstace().RowNum;
        int littleZooID = Game.GameConst.First_LittleZooID;
        int count02 = Config.buildupConfig.getInstace().getCell(littleZooID).animalid.Length;
        animalAtlasData = new int[count01, count02];
        //初始化
        for (int i = 0; i < count01; i++)
        {
            for (int j = 0; j < count02; j++)
            {
                animalAtlasData[i, j] = 0;
            }
        }
        //LogWarp.LogErrorFormat("测试：  初始化   图鉴动物信息     ");
    }

    /// <summary>
    /// 获取图鉴数据
    /// </summary>
    public void GetAnimalAtlasData()
    {
        var sceneStates = GlobalDataManager.GetInstance().playerData.playerZoo.scenePlayerDataMSS.sceneStates;
        foreach (var item in sceneStates)
        {
            var playerAnimal = GlobalDataManager.GetInstance().playerData.GetPlayerAnimalData(item.sceneId);
            if (playerAnimal !=null)
            {
                //赋值:
                var animalIDList = playerAnimal.animalID;
                for (int i = 0; i < animalIDList.Count; i++)
                {
                    var littleAnimal_MS = playerAnimal.getPlayerAnimalCell(animalIDList[i]);
                    //大于0代表是有动物数据
                    if (littleAnimal_MS.animalLevel > 0)
                    {
                        var cell = Config.animalupConfig.getInstace().getCell(animalIDList[i]);
                        int a = cell.bigtype;
                        int b = cell.smalltype;
                        if (animalAtlasData[a, b] ==0)
                        {
                            animalAtlasData[a, b] = 1;
                            animalAtlasNambe += 1;
                        }
                    }
                }
            }
            
        }
        //int count01 = Config.animalatlasConfig.getInstace().RowNum;
        //int count02 = 5;
        //for (int i = 0; i < count01; i++)
        //{
        //    for (int j = 0; j < count02; j++)
        //    {
        //        //LogWarp.LogErrorFormat("测试： 和动物数据同步  图鉴【{0}】【{1}】=={2}", i, j, animalAtlasData[i, j]);
        //    }
        //}
    }
    /// <summary>
    /// 设置单一图鉴数据
    /// </summary>
    /// <param name="a">大类</param>
    /// <param name="b">小类</param>
    public void SetAnimalAtlasData(int a, int b)
    {
        //LogWarp.LogErrorFormat("修改前：{0}  {1} =  {2}", a, b, animalAtlasData[a, b]);

        if (animalAtlasData.GetLength(0) <= a || animalAtlasData.GetLength(0)<=b)
        {
            return;
        }
        if (animalAtlasData[a, b] == 0)
        {
            animalAtlasData[a, b] = 1;
            animalAtlasNambe += 1;
        }
        //LogWarp.LogErrorFormat("修改后：{0}  {1} =  {2}",a,b, animalAtlasData[a, b]);
    }
    public int GetAnimalAtlasData(int a, int b)
    {
        if (animalAtlasData.GetLength(0) <= a || animalAtlasData.GetLength(0) <= b)
        {
            return UFrame.Const.Invalid_Int;
        }
        return animalAtlasData[a, b];

    }
}
