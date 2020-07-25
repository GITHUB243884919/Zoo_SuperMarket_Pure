using UnityEngine;
using System;
using System.Security;
using System.Collections.Generic;
namespace Config
{
	public class sceneConfig
	{
		private sceneConfig(){ 
		}
		private static sceneConfig _inst;
		public static sceneConfig getInstace(){
			if (_inst != null) {
				return _inst;
			}
			_inst = new sceneConfig ();
			_inst.InitData ();
			return _inst;
		}
		public Dictionary<string,sceneCell> AllData;
		public sceneCell getCell(string key){
			sceneCell t = null;
			this.AllData.TryGetValue (key, out t);
			return t;
		}
		public sceneCell getCell(int key){
			sceneCell t = null;
			this.AllData.TryGetValue (key.ToString(), out t);
			return t;
		}
		public readonly int RowNum = 25;
		private void InitData(){
			this.AllData = new Dictionary<string,sceneCell> ();
			this.AllData.Add("0",new sceneCell("Ui_Text_103","海岛1","Ui_Text_115",new int[]{10101,10301,10601,10801,11001},1,1,0,"UIAtlas/UIIcon/Tiger","dwy_9",1,1,1,0,0,new int[]{100,25,350,20,6},0,3,"1000",1));
			this.AllData.Add("1",new sceneCell("Ui_Text_103","海岛2","Ui_Text_115",new int[]{20101,20301,20601,20801,21001},1,2,38,"UIAtlas/UIIcon/Tiger","dwy_9",50,1,0,0,0,new int[]{280,10,394,6,30},0,6,"10000",1));
			this.AllData.Add("2",new sceneCell("Ui_Text_103","海岛3","Ui_Text_115",new int[]{30101,30301,30601,30801,31001},1,3,102,"UIAtlas/UIIcon/Tiger","dwy_9",50,1,0,0,0,new int[]{228,12,380,15,10},0,9,"25000",1));
			this.AllData.Add("3",new sceneCell("Ui_Text_103","海岛4","Ui_Text_115",new int[]{40101,40301,40601,40801,41001},1,4,193,"UIAtlas/UIIcon/Tiger","dwy_9",100,1,0,0,0,new int[]{210,16,350,50,10},0,14,"75000",1));
			this.AllData.Add("4",new sceneCell("Ui_Text_103","海岛5","Ui_Text_115",new int[]{50101,50301,50601,50801,51001},1,5,323,"UIAtlas/UIIcon/Tiger","dwy_9",100,1,0,0,0,new int[]{700,28,950,11,40},0,20,"155000",1));
			this.AllData.Add("5",new sceneCell("Ui_Text_104","城市1","Ui_Text_116",new int[]{60101,60201,60401,60701,60801},2,1,491,"UIAtlas/UIIcon/Tiger","city_scene",50,1,0,0,0,new int[]{99,11,141,2,6},0,3,"7503125000000000",50));
			this.AllData.Add("6",new sceneCell("Ui_Text_104","城市2","Ui_Text_116",new int[]{70101,70201,70401,70701,70801},2,2,529,"UIAtlas/UIIcon/Tiger","city_scene",50,1,0,0,0,new int[]{280,10,350,6,20},0,6,"7778750000000000",50));
			this.AllData.Add("7",new sceneCell("Ui_Text_104","城市3","Ui_Text_116",new int[]{80101,80201,80401,80701,80801},2,3,593,"UIAtlas/UIIcon/Tiger","city_scene",50,1,0,0,0,new int[]{228,12,380,15,13},0,9,"8544375000000000",50));
			this.AllData.Add("8",new sceneCell("Ui_Text_104","城市4","Ui_Text_116",new int[]{90101,90201,90401,90701,90801},2,4,684,"UIAtlas/UIIcon/Tiger","city_scene",100,1,0,0,0,new int[]{270,17,650,16,28},0,14,"10075625000000000",50));
			this.AllData.Add("9",new sceneCell("Ui_Text_104","城市5","Ui_Text_116",new int[]{100101,100201,100401,100701,100801},2,5,814,"UIAtlas/UIIcon/Tiger","city_scene",100,1,0,0,0,new int[]{580,28,650,100,13},0,20,"12525625000000000",50));
			this.AllData.Add("10",new sceneCell("Ui_Text_105","沙漠1","Ui_Text_117",new int[]{110101,110201,110401,110601,111001},3,1,974,"UIAtlas/UIIcon/Tiger","west_scene",50,1,0,0,0,new int[]{80,10,179,3,6},1,3,"389064062500000000000000000",95));
			this.AllData.Add("11",new sceneCell("Ui_Text_105","沙漠2","Ui_Text_117",new int[]{120101,120201,120401,120601,121001},3,2,1012,"UIAtlas/UIIcon/Tiger","west_scene",50,1,0,0,0,new int[]{280,10,394,6,30},1,6,"396081250000000000000000000",95));
			this.AllData.Add("12",new sceneCell("Ui_Text_105","沙漠3","Ui_Text_117",new int[]{130101,130201,130401,130601,131001},3,3,1076,"UIAtlas/UIIcon/Tiger","west_scene",50,1,0,0,0,new int[]{228,12,380,15,10},1,9,"415573437500000000000000000",95));
			this.AllData.Add("13",new sceneCell("Ui_Text_105","沙漠4","Ui_Text_117",new int[]{140101,140201,140401,140601,141001},3,4,1167,"UIAtlas/UIIcon/Tiger","west_scene",100,1,0,0,0,new int[]{210,16,350,50,10},1,14,"454557812500000000000000000",95));
			this.AllData.Add("14",new sceneCell("Ui_Text_105","沙漠5","Ui_Text_117",new int[]{150101,150201,150401,150601,151001},3,5,1297,"UIAtlas/UIIcon/Tiger","west_scene",100,1,0,0,0,new int[]{700,28,950,11,40},1,20,"516932812500000000000000000",95));
			this.AllData.Add("15",new sceneCell("Ui_Text_106","绿洲1","Ui_Text_119",new int[]{160101,160401,160601,160701,160801},4,1,1465,"UIAtlas/UIIcon/Tiger","dwy_9_m",50,0,0,0,0,new int[]{99,11,141,2,6},0,3,"1107439453125000000000000000000000000",145));
			this.AllData.Add("16",new sceneCell("Ui_Text_106","绿洲2","Ui_Text_119",new int[]{170101,170401,170601,170701,170801},4,2,1503,"UIAtlas/UIIcon/Tiger","dwy_9_m",50,0,0,0,0,new int[]{280,10,350,6,20},0,6,"1120675781250000000000000000000000000",145));
			this.AllData.Add("17",new sceneCell("Ui_Text_106","绿洲3","Ui_Text_119",new int[]{180101,180401,180601,180701,180801},4,3,1567,"UIAtlas/UIIcon/Tiger","dwy_9_m",50,0,0,0,0,new int[]{228,12,380,15,13},0,9,"1157443359375000000000000000000000000",145));
			this.AllData.Add("18",new sceneCell("Ui_Text_106","绿洲4","Ui_Text_119",new int[]{190101,190401,190601,190701,190801},4,4,1658,"UIAtlas/UIIcon/Tiger","dwy_9_m",100,0,0,0,0,new int[]{270,17,650,16,28},0,14,"1230978515625000000000000000000000000",145));
			this.AllData.Add("19",new sceneCell("Ui_Text_106","绿洲5","Ui_Text_119",new int[]{200101,200401,200601,200701,200801},4,5,1788,"UIAtlas/UIIcon/Tiger","dwy_9_m",100,0,0,0,0,new int[]{580,28,650,100,13},0,20,"1348634765625000000000000000000000000",145));
			this.AllData.Add("20",new sceneCell("Ui_Text_107","花海1","Ui_Text_119",new int[]{160101,160401,160601,160701,160801},5,1,2088,"UIAtlas/UIIcon/Tiger","dwy_9_m",50,0,0,0,0,new int[]{80,10,179,3,6},0,3,"1480998046875000000000000000000000000",190));
			this.AllData.Add("21",new sceneCell("Ui_Text_107","花海2","Ui_Text_119",new int[]{170101,170401,170601,170701,170801},5,2,2388,"UIAtlas/UIIcon/Tiger","dwy_9_m",50,0,0,0,0,new int[]{280,10,394,6,30},0,6,"1494234375000000000000000000000000000",190));
			this.AllData.Add("22",new sceneCell("Ui_Text_107","花海3","Ui_Text_119",new int[]{180101,180401,180601,180701,180801},5,3,2688,"UIAtlas/UIIcon/Tiger","dwy_9_m",50,0,0,0,0,new int[]{228,12,380,15,10},0,9,"1531001953125000000000000000000000000",190));
			this.AllData.Add("23",new sceneCell("Ui_Text_107","花海4","Ui_Text_119",new int[]{190101,190401,190601,190701,190801},5,4,2988,"UIAtlas/UIIcon/Tiger","dwy_9_m",100,0,0,0,0,new int[]{210,16,350,50,10},0,14,"1604537109375000000000000000000000000",190));
			this.AllData.Add("24",new sceneCell("Ui_Text_107","花海5","Ui_Text_119",new int[]{200101,200401,200601,200701,200801},5,5,3288,"UIAtlas/UIIcon/Tiger","dwy_9_m",100,0,0,0,0,new int[]{700,28,950,11,40},0,20,"1722193359375000000000000000000000000",190));
		}
	}
	public class sceneCell
	{
		///<summary>
		///场景名称
		///<summary>
		public readonly string scenename;
		///<summary>
		///ta调用名称
		///<summary>
		public readonly string tascenename;
		///<summary>
		///场景说明文本
		///<summary>
		public readonly string scenetips;
		///<summary>
		///场景包含动物id
		///<summary>
		public readonly int[] sceneanimal;
		///<summary>
		///场景类型
		///<summary>
		public readonly int scenetype;
		///<summary>
		///场景顺序
		///<summary>
		public readonly int sceneorder;
		///<summary>
		///开启星级
		///<summary>
		public readonly int openstar;
		///<summary>
		///图标
		///<summary>
		public readonly string icon;
		///<summary>
		///场景资源加载
		///<summary>
		public readonly string resourceid;
		///<summary>
		///翻倍系数
		///<summary>
		public readonly int doublenum;
		///<summary>
		///版本开放控制
		///<summary>
		public readonly int israwopen;
		///<summary>
		///起始任务ID
		///<summary>
		public readonly int missionstart;
		///<summary>
		///场景等级
		///<summary>
		public readonly int scenelv;
		///<summary>
		///关联货币
		///<summary>
		public readonly int moneyid;
		///<summary>
		///等级系数
		///<summary>
		public readonly int[] lvcoefficient;
		///<summary>
		///额外游客入场路线
		///<summary>
		public readonly int visitorpath;
		///<summary>
		///广告翻倍系数
		///<summary>
		public readonly int adcoefficient;
		///<summary>
		///场景初始货币数量
		///<summary>
		public readonly string scenelnitialgoldnum;
		///<summary>
		///期望倍数
		///<summary>
		public readonly int expefficient;
		public sceneCell(string scenename,string tascenename,string scenetips,int[] sceneanimal,int scenetype,int sceneorder,int openstar,string icon,string resourceid,int doublenum,int israwopen,int missionstart,int scenelv,int moneyid,int[] lvcoefficient,int visitorpath,int adcoefficient,string scenelnitialgoldnum,int expefficient){
			this.scenename = scenename;
			this.tascenename = tascenename;
			this.scenetips = scenetips;
			this.sceneanimal = sceneanimal;
			this.scenetype = scenetype;
			this.sceneorder = sceneorder;
			this.openstar = openstar;
			this.icon = icon;
			this.resourceid = resourceid;
			this.doublenum = doublenum;
			this.israwopen = israwopen;
			this.missionstart = missionstart;
			this.scenelv = scenelv;
			this.moneyid = moneyid;
			this.lvcoefficient = lvcoefficient;
			this.visitorpath = visitorpath;
			this.adcoefficient = adcoefficient;
			this.scenelnitialgoldnum = scenelnitialgoldnum;
			this.expefficient = expefficient;
		}
	}
}