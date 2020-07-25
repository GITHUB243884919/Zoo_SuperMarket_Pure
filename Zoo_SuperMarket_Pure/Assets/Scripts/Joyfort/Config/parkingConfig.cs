using UnityEngine;
using System;
using System.Security;
using System.Collections.Generic;
namespace Config
{
	public class parkingConfig
	{
		private parkingConfig(){ 
		}
		private static parkingConfig _inst;
		public static parkingConfig getInstace(){
			if (_inst != null) {
				return _inst;
			}
			_inst = new parkingConfig ();
			_inst.InitData ();
			return _inst;
		}
		public Dictionary<string,parkingCell> AllData;
		public parkingCell getCell(string key){
			parkingCell t = null;
			this.AllData.TryGetValue (key, out t);
			return t;
		}
		public parkingCell getCell(int key){
			parkingCell t = null;
			this.AllData.TryGetValue (key.ToString(), out t);
			return t;
		}
		public readonly int RowNum = 20;
		private void InitData(){
			this.AllData = new Dictionary<string,parkingCell> ();
			this.AllData.Add("1",new parkingCell("停车场",0,0,4,8,1,100,"2",2,6,2,23,"2",4,"2",3,350,new int[]{1,2,3,4,5,6,7,8},"parking_01",new string[]{"9001","9002","9003","9004","9005","9006","9007","9008"},new int[]{-1,1,1,-1,1,-1,1,-1},new int[]{0,25,50,75,100,150,200,300,350},new int[]{1,1,1,1,1,1,1,1,1},new int[]{3004,3004,3004,3004,3005,3006,3007,3010,3011},new int[]{0,1,1,1,1,1,1,1,1},8,1.06f));
			this.AllData.Add("2",new parkingCell("停车场",1,0,4,8,1,100,"2",2,6,2,23,"2",4,"2",3,600,new int[]{1,2,3,4,5,6,7,8},"parking_01",new string[]{"9001","9002","9003","9004","9005","9006","9007","9008"},new int[]{-1,1,1,-1,1,-1,1,-1},new int[]{0,25,50,75,100,150,200,300,400,500,600},new int[]{1,1,1,1,1,1,1,1,1,1,1},new int[]{2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001},new int[]{0,1,1,1,1,1,1,1,1,1,1},10,1.06f));
			this.AllData.Add("3",new parkingCell("停车场",2,0,4,8,1,100,"2",2,6,2,23,"2",4,"2",3,730,new int[]{1,2,3,4,5,6,7,8},"parking_01",new string[]{"9001","9002","9003","9004","9005","9006","9007","9008"},new int[]{-1,1,1,-1,1,-1,1,-1},new int[]{0,25,50,75,100,150,200,300,400,500,600,700,730},new int[]{1,1,1,1,1,1,1,1,1,1,1,1,1},new int[]{2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001},new int[]{0,1,1,1,1,1,1,1,1,1,1,1,1},12,1.06f));
			this.AllData.Add("4",new parkingCell("停车场",3,0,4,8,1,100,"2",2,6,2,23,"2",4,"2",3,1150,new int[]{1,2,3,4,5,6,7,8},"parking_01",new string[]{"9001","9002","9003","9004","9005","9006","9007","9008"},new int[]{-1,1,1,-1,1,-1,1,-1},new int[]{0,25,50,75,100,150,200,300,400,500,600,700,800,900,1000,1150},new int[]{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},new int[]{2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001},new int[]{0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},15,1.06f));
			this.AllData.Add("5",new parkingCell("停车场",4,0,4,8,1,100,"2",2,6,2,23,"2",4,"2",3,1550,new int[]{1,2,3,4,5,6,7,8},"parking_01",new string[]{"9001","9002","9003","9004","9005","9006","9007","9008"},new int[]{-1,1,1,-1,1,-1,1,-1},new int[]{0,25,50,75,100,150,200,300,400,500,600,700,800,900,1000,1250,1550},new int[]{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},new int[]{2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001},new int[]{0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},16,1.06f));
			this.AllData.Add("6",new parkingCell("停车场",5,0,4,8,1,100,"2500000000",2,6,2,23,"2500000000",4,"2500000000",3,350,new int[]{1,2,3,4,5,6,7,8},"parking_01",new string[]{"9001","9002","9003","9004","9005","9006","9007","9008"},new int[]{-1,1,1,-1,1,-1,1,-1},new int[]{0,25,50,75,100,150,200,300,350},new int[]{1,1,1,1,1,1,1,1,1},new int[]{2001,2001,2001,2001,2001,2001,2001,2001,2001},new int[]{0,1,1,1,1,1,1,1,1},8,1.06f));
			this.AllData.Add("7",new parkingCell("停车场",6,0,4,8,1,100,"2500000000",2,6,2,23,"2500000000",4,"2500000000",3,600,new int[]{1,2,3,4,5,6,7,8},"parking_01",new string[]{"9001","9002","9003","9004","9005","9006","9007","9008"},new int[]{-1,1,1,-1,1,-1,1,-1},new int[]{0,25,50,75,100,150,200,300,400,500,600},new int[]{1,1,1,1,1,1,1,1,1,1,1},new int[]{2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001},new int[]{0,1,1,1,1,1,1,1,1,1,1},10,1.06f));
			this.AllData.Add("8",new parkingCell("停车场",7,0,4,8,1,100,"2500000000",2,6,2,23,"2500000000",4,"2500000000",3,730,new int[]{1,2,3,4,5,6,7,8},"parking_01",new string[]{"9001","9002","9003","9004","9005","9006","9007","9008"},new int[]{-1,1,1,-1,1,-1,1,-1},new int[]{0,25,50,75,100,150,200,300,400,500,600,700,730},new int[]{1,1,1,1,1,1,1,1,1,1,1,1,1},new int[]{2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001},new int[]{0,1,1,1,1,1,1,1,1,1,1,1,1},12,1.06f));
			this.AllData.Add("9",new parkingCell("停车场",8,0,4,8,1,100,"2500000000",2,6,2,23,"2500000000",4,"2500000000",3,1150,new int[]{1,2,3,4,5,6,7,8},"parking_01",new string[]{"9001","9002","9003","9004","9005","9006","9007","9008"},new int[]{-1,1,1,-1,1,-1,1,-1},new int[]{0,25,50,75,100,150,200,300,400,500,600,700,800,900,1000,1150},new int[]{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},new int[]{2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001},new int[]{0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},15,1.06f));
			this.AllData.Add("10",new parkingCell("停车场",9,0,4,8,1,100,"2500000000",2,6,2,23,"2500000000",4,"2500000000",3,1550,new int[]{1,2,3,4,5,6,7,8},"parking_01",new string[]{"9001","9002","9003","9004","9005","9006","9007","9008"},new int[]{-1,1,1,-1,1,-1,1,-1},new int[]{0,25,50,75,100,150,200,300,400,500,600,700,800,900,1000,1250,1550},new int[]{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},new int[]{2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001},new int[]{0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},16,1.06f));
			this.AllData.Add("11",new parkingCell("停车场",10,0,4,8,1,100,"3125000000000000000",2,6,2,23,"3125000000000000000",4,"3125000000000000000",3,350,new int[]{1,2,3,4,5,6,7,8},"parking_01",new string[]{"9001","9002","9003","9004","9005","9006","9007","9008"},new int[]{-1,1,1,-1,1,-1,1,-1},new int[]{0,25,50,75,100,150,200,300,350},new int[]{1,1,1,1,1,1,1,1,1},new int[]{2001,2001,2001,2001,2001,2001,2001,2001,2001},new int[]{0,1,1,1,1,1,1,1,1},8,1.06f));
			this.AllData.Add("12",new parkingCell("停车场",11,0,4,8,1,100,"3125000000000000000",2,6,2,23,"3125000000000000000",4,"3125000000000000000",3,600,new int[]{1,2,3,4,5,6,7,8},"parking_01",new string[]{"9001","9002","9003","9004","9005","9006","9007","9008"},new int[]{-1,1,1,-1,1,-1,1,-1},new int[]{0,25,50,75,100,150,200,300,400,500,600},new int[]{1,1,1,1,1,1,1,1,1,1,1},new int[]{2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001},new int[]{0,1,1,1,1,1,1,1,1,1,1},10,1.06f));
			this.AllData.Add("13",new parkingCell("停车场",12,0,4,8,1,100,"3125000000000000000",2,6,2,23,"3125000000000000000",4,"3125000000000000000",3,730,new int[]{1,2,3,4,5,6,7,8},"parking_01",new string[]{"9001","9002","9003","9004","9005","9006","9007","9008"},new int[]{-1,1,1,-1,1,-1,1,-1},new int[]{0,25,50,75,100,150,200,300,400,500,600,700,730},new int[]{1,1,1,1,1,1,1,1,1,1,1,1,1},new int[]{2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001},new int[]{0,1,1,1,1,1,1,1,1,1,1,1,1},12,1.06f));
			this.AllData.Add("14",new parkingCell("停车场",13,0,4,8,1,100,"3125000000000000000",2,6,2,23,"3125000000000000000",4,"3125000000000000000",3,1150,new int[]{1,2,3,4,5,6,7,8},"parking_01",new string[]{"9001","9002","9003","9004","9005","9006","9007","9008"},new int[]{-1,1,1,-1,1,-1,1,-1},new int[]{0,25,50,75,100,150,200,300,400,500,600,700,800,900,1000,1150},new int[]{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},new int[]{2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001},new int[]{0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},15,1.06f));
			this.AllData.Add("15",new parkingCell("停车场",14,0,4,8,1,100,"3125000000000000000",2,6,2,23,"3125000000000000000",4,"3125000000000000000",3,1550,new int[]{1,2,3,4,5,6,7,8},"parking_01",new string[]{"9001","9002","9003","9004","9005","9006","9007","9008"},new int[]{-1,1,1,-1,1,-1,1,-1},new int[]{0,25,50,75,100,150,200,300,400,500,600,700,800,900,1000,1250,1550},new int[]{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},new int[]{2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001},new int[]{0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},16,1.06f));
			this.AllData.Add("16",new parkingCell("停车场",15,0,4,8,1,100,"3906250000000000000000000000",2,6,2,23,"3906250000000000000000000000",4,"3906250000000000000000000000",3,350,new int[]{1,2,3,4,5,6,7,8},"parking_01",new string[]{"9001","9002","9003","9004","9005","9006","9007","9008"},new int[]{-1,1,1,-1,1,-1,1,-1},new int[]{0,25,50,75,100,150,200,300,350},new int[]{1,1,1,1,1,1,1,1,1},new int[]{2001,2001,2001,2001,2001,2001,2001,2001,2001},new int[]{0,1,1,1,1,1,1,1,1},8,1.06f));
			this.AllData.Add("17",new parkingCell("停车场",16,0,4,8,1,100,"3906250000000000000000000000",2,6,2,23,"3906250000000000000000000000",4,"3906250000000000000000000000",3,600,new int[]{1,2,3,4,5,6,7,8},"parking_01",new string[]{"9001","9002","9003","9004","9005","9006","9007","9008"},new int[]{-1,1,1,-1,1,-1,1,-1},new int[]{0,25,50,75,100,150,200,300,400,500,600},new int[]{1,1,1,1,1,1,1,1,1,1,1},new int[]{2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001},new int[]{0,1,1,1,1,1,1,1,1,1,1},10,1.06f));
			this.AllData.Add("18",new parkingCell("停车场",17,0,4,8,1,100,"3906250000000000000000000000",2,6,2,23,"3906250000000000000000000000",4,"3906250000000000000000000000",3,730,new int[]{1,2,3,4,5,6,7,8},"parking_01",new string[]{"9001","9002","9003","9004","9005","9006","9007","9008"},new int[]{-1,1,1,-1,1,-1,1,-1},new int[]{0,25,50,75,100,150,200,300,400,500,600,700,730},new int[]{1,1,1,1,1,1,1,1,1,1,1,1,1},new int[]{2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001},new int[]{0,1,1,1,1,1,1,1,1,1,1,1,1},12,1.06f));
			this.AllData.Add("19",new parkingCell("停车场",18,0,4,8,1,100,"3906250000000000000000000000",2,6,2,23,"3906250000000000000000000000",4,"3906250000000000000000000000",3,1150,new int[]{1,2,3,4,5,6,7,8},"parking_01",new string[]{"9001","9002","9003","9004","9005","9006","9007","9008"},new int[]{-1,1,1,-1,1,-1,1,-1},new int[]{0,25,50,75,100,150,200,300,400,500,600,700,800,900,1000,1150},new int[]{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},new int[]{2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001},new int[]{0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},15,1.06f));
			this.AllData.Add("20",new parkingCell("停车场",19,0,4,8,1,100,"3906250000000000000000000000",2,6,2,23,"3906250000000000000000000000",4,"3906250000000000000000000000",3,1550,new int[]{1,2,3,4,5,6,7,8},"parking_01",new string[]{"9001","9002","9003","9004","9005","9006","9007","9008"},new int[]{-1,1,1,-1,1,-1,1,-1},new int[]{0,25,50,75,100,150,200,300,400,500,600,700,800,900,1000,1250,1550},new int[]{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},new int[]{2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001,2001},new int[]{0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},16,1.06f));
		}
	}
	public class parkingCell
	{
		///<summary>
		///停车场名称
		///<summary>
		public readonly string buildname;
		///<summary>
		///所属场景
		///<summary>
		public readonly int scene;
		///<summary>
		///停车费初始
		///<summary>
		public readonly int price;
		///<summary>
		///停车费公式
		///<summary>
		public readonly int priceformula;
		///<summary>
		///停车场来人速度初始
		///<summary>
		public readonly int touristbase;
		///<summary>
		///停车场来人速度公式
		///<summary>
		public readonly int touristformula;
		///<summary>
		///速度最大等级
		///<summary>
		public readonly int touristmaxlv;
		///<summary>
		///停车场来人速度升级消耗初始
		///<summary>
		public readonly string touristcastbase;
		///<summary>
		///停车场来人速度升级公式
		///<summary>
		public readonly int touristcastformula;
		///<summary>
		///停车场最大位置初始
		///<summary>
		public readonly int spacebase;
		///<summary>
		///停车场最大位置公式
		///<summary>
		public readonly int spaceformula;
		///<summary>
		///最大位置等级
		///<summary>
		public readonly int spacemaxlv;
		///<summary>
		///停车场最大位置升级消耗初始
		///<summary>
		public readonly string spaceupcastbase;
		///<summary>
		///停车场最大位置升级消耗公式
		///<summary>
		public readonly int spaceupcastformula;
		///<summary>
		///停车场利润升级初始消耗
		///<summary>
		public readonly string depletebase;
		///<summary>
		///停车场利润升级消耗公式
		///<summary>
		public readonly int depleteformula;
		///<summary>
		///等级上限
		///<summary>
		public readonly int lvmax;
		///<summary>
		///停车场开启等级
		///<summary>
		public readonly int[] openlv;
		///<summary>
		///停车场预制体名称
		///<summary>
		public readonly string prefabsname;
		///<summary>
		///开启组资源
		///<summary>
		public readonly string[] openggroup;
		///<summary>
		///停车位位置
		///<summary>
		public readonly int[] openseatdir;
		///<summary>
		///等级阶段
		///<summary>
		public readonly int[] lvshage;
		///<summary>
		///奖励类型
		///<summary>
		public readonly int[] lvrewardtype;
		///<summary>
		///奖励道具ID
		///<summary>
		public readonly int[] lvreward;
		///<summary>
		///奖励星星数量
		///<summary>
		public readonly int[] star;
		///<summary>
		///总星数
		///<summary>
		public readonly int starsum;
		///<summary>
		///底数
		///<summary>
		public readonly float basenumber;
		public parkingCell(string buildname,int scene,int price,int priceformula,int touristbase,int touristformula,int touristmaxlv,string touristcastbase,int touristcastformula,int spacebase,int spaceformula,int spacemaxlv,string spaceupcastbase,int spaceupcastformula,string depletebase,int depleteformula,int lvmax,int[] openlv,string prefabsname,string[] openggroup,int[] openseatdir,int[] lvshage,int[] lvrewardtype,int[] lvreward,int[] star,int starsum,float basenumber){
			this.buildname = buildname;
			this.scene = scene;
			this.price = price;
			this.priceformula = priceformula;
			this.touristbase = touristbase;
			this.touristformula = touristformula;
			this.touristmaxlv = touristmaxlv;
			this.touristcastbase = touristcastbase;
			this.touristcastformula = touristcastformula;
			this.spacebase = spacebase;
			this.spaceformula = spaceformula;
			this.spacemaxlv = spacemaxlv;
			this.spaceupcastbase = spaceupcastbase;
			this.spaceupcastformula = spaceupcastformula;
			this.depletebase = depletebase;
			this.depleteformula = depleteformula;
			this.lvmax = lvmax;
			this.openlv = openlv;
			this.prefabsname = prefabsname;
			this.openggroup = openggroup;
			this.openseatdir = openseatdir;
			this.lvshage = lvshage;
			this.lvrewardtype = lvrewardtype;
			this.lvreward = lvreward;
			this.star = star;
			this.starsum = starsum;
			this.basenumber = basenumber;
		}
	}
}