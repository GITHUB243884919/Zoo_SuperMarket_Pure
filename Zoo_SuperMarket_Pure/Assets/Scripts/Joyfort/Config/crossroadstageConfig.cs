using UnityEngine;
using System;
using System.Security;
using System.Collections.Generic;
namespace Config
{
	public class crossroadstageConfig
	{
		private crossroadstageConfig(){ 
		}
		private static crossroadstageConfig _inst;
		public static crossroadstageConfig getInstace(){
			if (_inst != null) {
				return _inst;
			}
			_inst = new crossroadstageConfig ();
			_inst.InitData ();
			return _inst;
		}
		public Dictionary<string,crossroadstageCell> AllData;
		public crossroadstageCell getCell(string key){
			crossroadstageCell t = null;
			this.AllData.TryGetValue (key, out t);
			return t;
		}
		public crossroadstageCell getCell(int key){
			crossroadstageCell t = null;
			this.AllData.TryGetValue (key.ToString(), out t);
			return t;
		}
		public readonly int RowNum = 500;
		private void InitData(){
			this.AllData = new Dictionary<string,crossroadstageCell> ();
			this.AllData.Add("1",new crossroadstageCell(1,10,6,5f,1,1,8f,8f,4.992f,6.19f,2,1,2));
			this.AllData.Add("2",new crossroadstageCell(2,10,6,5f,1,1,8f,8f,4.984f,6.18f,2,1,2));
			this.AllData.Add("3",new crossroadstageCell(3,10,6,5f,1,1,8f,8f,4.976f,6.17f,2,1,2));
			this.AllData.Add("4",new crossroadstageCell(4,10,6,5f,1,1,8f,8f,4.968f,6.16f,2,1,2));
			this.AllData.Add("5",new crossroadstageCell(5,10,6,5f,1,1,8f,8f,4.96f,6.15f,2,1,2));
			this.AllData.Add("6",new crossroadstageCell(6,10,6,5f,1,1,8f,8f,4.952f,6.14f,2,1,2));
			this.AllData.Add("7",new crossroadstageCell(7,10,6,5f,1,1,8f,8f,4.944f,6.13f,2,1,2));
			this.AllData.Add("8",new crossroadstageCell(8,10,6,5f,1,2,8f,8f,4.936f,6.12f,2,1,2));
			this.AllData.Add("9",new crossroadstageCell(9,10,6,5f,1,2,8f,8f,4.928f,6.11f,2,1,2));
			this.AllData.Add("10",new crossroadstageCell(10,10,6,5f,1,2,8f,8f,4.92f,6.1f,2,1,2));
			this.AllData.Add("11",new crossroadstageCell(11,10,6,5f,1,2,8f,9f,4.912f,6.09f,2,1,2));
			this.AllData.Add("12",new crossroadstageCell(12,10,6,5f,1,2,8f,9f,4.904f,6.08f,2,1,2));
			this.AllData.Add("13",new crossroadstageCell(13,10,6,5f,1,2,8f,9f,4.896f,6.07f,2,1,2));
			this.AllData.Add("14",new crossroadstageCell(14,10,6,5f,1,2,8f,9f,4.888f,6.06f,2,1,2));
			this.AllData.Add("15",new crossroadstageCell(15,10,6,5f,1,3,8f,9f,4.88f,6.05f,2,1,2));
			this.AllData.Add("16",new crossroadstageCell(16,10,6,5f,1,3,8f,9f,4.872f,6.04f,2,1,2));
			this.AllData.Add("17",new crossroadstageCell(17,10,6,5f,1,3,8f,9f,4.864f,6.03f,2,1,2));
			this.AllData.Add("18",new crossroadstageCell(18,10,6,5f,1,3,8f,9f,4.856f,6.02f,2,1,2));
			this.AllData.Add("19",new crossroadstageCell(19,10,6,5f,1,3,8f,9f,4.848f,6.01f,2,1,2));
			this.AllData.Add("20",new crossroadstageCell(20,10,6,5f,1,3,8f,9f,4.84f,6f,2,1,2));
			this.AllData.Add("21",new crossroadstageCell(21,10,6,5f,1,3,8f,10f,4.832f,5.99f,2,1,2));
			this.AllData.Add("22",new crossroadstageCell(22,10,6,5f,2,2,8f,10f,4.824f,5.98f,2,1,2));
			this.AllData.Add("23",new crossroadstageCell(23,10,6,5f,2,2,8f,10f,4.816f,5.97f,2,1,2));
			this.AllData.Add("24",new crossroadstageCell(24,10,6,5f,2,2,8f,10f,4.808f,5.96f,2,1,2));
			this.AllData.Add("25",new crossroadstageCell(25,10,6,5f,2,2,8f,10f,4.8f,5.95f,2,1,2));
			this.AllData.Add("26",new crossroadstageCell(26,10,6,5f,2,2,8f,10f,4.792f,5.94f,2,1,2));
			this.AllData.Add("27",new crossroadstageCell(27,10,6,5f,2,2,8f,10f,4.784f,5.93f,2,1,2));
			this.AllData.Add("28",new crossroadstageCell(28,10,6,5f,2,2,8f,10f,4.776f,5.92f,2,1,2));
			this.AllData.Add("29",new crossroadstageCell(29,10,6,5f,2,3,8f,10f,4.768f,5.91f,2,1,2));
			this.AllData.Add("30",new crossroadstageCell(30,10,6,5f,2,3,8f,10f,4.76f,5.9f,2,1,2));
			this.AllData.Add("31",new crossroadstageCell(31,10,6,5f,2,3,9f,11f,4.752f,5.89f,2,1,2));
			this.AllData.Add("32",new crossroadstageCell(32,10,6,5f,2,3,9f,11f,4.744f,5.88f,2,1,2));
			this.AllData.Add("33",new crossroadstageCell(33,10,6,5f,2,3,9f,11f,4.736f,5.87f,2,1,2));
			this.AllData.Add("34",new crossroadstageCell(34,10,6,5f,2,3,9f,11f,4.728f,5.86f,2,1,2));
			this.AllData.Add("35",new crossroadstageCell(35,10,6,5f,2,3,9f,11f,4.72f,5.85f,2,1,2));
			this.AllData.Add("36",new crossroadstageCell(36,10,6,5f,2,4,9f,11f,4.712f,5.84f,2,1,2));
			this.AllData.Add("37",new crossroadstageCell(37,10,6,5f,2,4,9f,11f,4.704f,5.83f,2,1,2));
			this.AllData.Add("38",new crossroadstageCell(38,10,6,5f,2,4,9f,11f,4.696f,5.82f,2,1,2));
			this.AllData.Add("39",new crossroadstageCell(39,10,6,5f,2,4,9f,11f,4.688f,5.81f,2,1,2));
			this.AllData.Add("40",new crossroadstageCell(40,10,6,5f,2,4,9f,11f,4.68f,5.8f,2,1,2));
			this.AllData.Add("41",new crossroadstageCell(41,10,6,5f,2,4,10f,12f,4.672f,5.79f,2,1,2));
			this.AllData.Add("42",new crossroadstageCell(42,10,6,5f,2,4,10f,12f,4.664f,5.78f,2,1,2));
			this.AllData.Add("43",new crossroadstageCell(43,10,6,5f,3,3,10f,12f,4.656f,5.77f,2,1,2));
			this.AllData.Add("44",new crossroadstageCell(44,10,6,5f,3,3,10f,12f,4.648f,5.76f,2,1,2));
			this.AllData.Add("45",new crossroadstageCell(45,10,6,5f,3,3,10f,12f,4.64f,5.75f,2,1,2));
			this.AllData.Add("46",new crossroadstageCell(46,10,6,5f,3,3,10f,12f,4.632f,5.74f,2,1,2));
			this.AllData.Add("47",new crossroadstageCell(47,10,6,5f,3,3,10f,12f,4.624f,5.73f,2,1,2));
			this.AllData.Add("48",new crossroadstageCell(48,10,6,5f,3,3,10f,12f,4.616f,5.72f,2,1,2));
			this.AllData.Add("49",new crossroadstageCell(49,10,6,5f,3,3,10f,12f,4.608f,5.71f,2,1,2));
			this.AllData.Add("50",new crossroadstageCell(50,10,6,5f,3,4,10f,12f,4.6f,5.7f,2,1,2));
			this.AllData.Add("51",new crossroadstageCell(51,15,6,5f,3,4,11f,13f,4.592f,5.69f,2,1,2));
			this.AllData.Add("52",new crossroadstageCell(52,15,6,5f,3,4,11f,13f,4.584f,5.68f,2,1,2));
			this.AllData.Add("53",new crossroadstageCell(53,15,6,5f,3,4,11f,13f,4.576f,5.67f,2,1,2));
			this.AllData.Add("54",new crossroadstageCell(54,15,6,5f,3,4,11f,13f,4.568f,5.66f,2,1,2));
			this.AllData.Add("55",new crossroadstageCell(55,15,6,5f,3,4,11f,13f,4.56f,5.65f,2,1,2));
			this.AllData.Add("56",new crossroadstageCell(56,15,6,5f,3,4,11f,13f,4.552f,5.64f,2,1,2));
			this.AllData.Add("57",new crossroadstageCell(57,15,6,5f,3,5,11f,13f,4.544f,5.63f,2,1,2));
			this.AllData.Add("58",new crossroadstageCell(58,15,6,5f,3,5,11f,13f,4.536f,5.62f,2,1,2));
			this.AllData.Add("59",new crossroadstageCell(59,15,6,5f,3,5,11f,13f,4.528f,5.61f,2,1,2));
			this.AllData.Add("60",new crossroadstageCell(60,15,6,5f,3,5,11f,13f,4.52f,5.6f,2,1,2));
			this.AllData.Add("61",new crossroadstageCell(61,15,6,5f,3,5,12f,14f,4.512f,5.59f,2,1,2));
			this.AllData.Add("62",new crossroadstageCell(62,15,6,5f,3,5,12f,14f,4.504f,5.58f,2,1,2));
			this.AllData.Add("63",new crossroadstageCell(63,15,6,5f,3,5,12f,14f,4.496f,5.57f,2,1,2));
			this.AllData.Add("64",new crossroadstageCell(64,15,6,5f,4,4,12f,14f,4.488f,5.56f,2,1,2));
			this.AllData.Add("65",new crossroadstageCell(65,15,6,5f,4,4,12f,14f,4.48f,5.55f,2,1,2));
			this.AllData.Add("66",new crossroadstageCell(66,15,6,5f,4,4,12f,14f,4.472f,5.54f,2,1,2));
			this.AllData.Add("67",new crossroadstageCell(67,15,6,5f,4,4,12f,14f,4.464f,5.53f,2,1,2));
			this.AllData.Add("68",new crossroadstageCell(68,15,6,5f,4,4,12f,14f,4.456f,5.52f,2,1,2));
			this.AllData.Add("69",new crossroadstageCell(69,15,6,5f,4,4,12f,14f,4.448f,5.51f,2,1,2));
			this.AllData.Add("70",new crossroadstageCell(70,15,6,5f,4,4,12f,14f,4.44f,5.5f,2,1,2));
			this.AllData.Add("71",new crossroadstageCell(71,15,6,5f,4,5,13f,15f,4.432f,5.49f,2,1,2));
			this.AllData.Add("72",new crossroadstageCell(72,15,6,5f,4,5,13f,15f,4.424f,5.48f,2,1,2));
			this.AllData.Add("73",new crossroadstageCell(73,15,6,5f,4,5,13f,15f,4.416f,5.47f,2,1,2));
			this.AllData.Add("74",new crossroadstageCell(74,15,6,5f,4,5,13f,15f,4.408f,5.46f,2,1,2));
			this.AllData.Add("75",new crossroadstageCell(75,15,6,5f,4,5,13f,15f,4.4f,5.45f,2,1,2));
			this.AllData.Add("76",new crossroadstageCell(76,15,6,5f,4,5,13f,15f,4.392f,5.44f,2,1,2));
			this.AllData.Add("77",new crossroadstageCell(77,15,6,5f,4,5,13f,15f,4.384f,5.43f,2,1,2));
			this.AllData.Add("78",new crossroadstageCell(78,15,6,5f,4,6,13f,15f,4.376f,5.42f,2,1,2));
			this.AllData.Add("79",new crossroadstageCell(79,15,6,5f,4,6,13f,15f,4.368f,5.41f,2,1,2));
			this.AllData.Add("80",new crossroadstageCell(80,15,6,5f,4,6,13f,15f,4.36f,5.4f,2,1,2));
			this.AllData.Add("81",new crossroadstageCell(81,15,6,5f,4,6,14f,16f,4.352f,5.39f,2,1,2));
			this.AllData.Add("82",new crossroadstageCell(82,15,6,5f,4,6,14f,16f,4.344f,5.38f,2,1,2));
			this.AllData.Add("83",new crossroadstageCell(83,15,6,5f,4,6,14f,16f,4.336f,5.37f,2,1,2));
			this.AllData.Add("84",new crossroadstageCell(84,15,6,5f,4,6,14f,16f,4.328f,5.36f,2,1,2));
			this.AllData.Add("85",new crossroadstageCell(85,15,6,5f,5,5,14f,16f,4.32f,5.35f,2,1,2));
			this.AllData.Add("86",new crossroadstageCell(86,15,6,5f,5,5,14f,16f,4.312f,5.34f,2,1,2));
			this.AllData.Add("87",new crossroadstageCell(87,15,6,5f,5,5,14f,16f,4.304f,5.33f,2,1,2));
			this.AllData.Add("88",new crossroadstageCell(88,15,6,5f,5,5,14f,16f,4.296f,5.32f,2,1,2));
			this.AllData.Add("89",new crossroadstageCell(89,15,6,5f,5,5,14f,16f,4.288f,5.31f,2,1,2));
			this.AllData.Add("90",new crossroadstageCell(90,15,6,5f,5,5,14f,16f,4.28f,5.3f,2,1,2));
			this.AllData.Add("91",new crossroadstageCell(91,15,6,5f,5,5,15f,17f,4.272f,5.29f,2,1,2));
			this.AllData.Add("92",new crossroadstageCell(92,15,6,5f,5,6,15f,17f,4.264f,5.28f,2,1,2));
			this.AllData.Add("93",new crossroadstageCell(93,15,6,5f,5,6,15f,17f,4.256f,5.27f,2,1,2));
			this.AllData.Add("94",new crossroadstageCell(94,15,6,5f,5,6,15f,17f,4.248f,5.26f,2,1,2));
			this.AllData.Add("95",new crossroadstageCell(95,15,6,5f,5,6,15f,17f,4.24f,5.25f,2,1,2));
			this.AllData.Add("96",new crossroadstageCell(96,15,6,5f,5,6,15f,17f,4.232f,5.24f,2,1,2));
			this.AllData.Add("97",new crossroadstageCell(97,15,6,5f,5,6,15f,17f,4.224f,5.23f,2,1,2));
			this.AllData.Add("98",new crossroadstageCell(98,15,6,5f,5,6,15f,17f,4.216f,5.22f,2,1,2));
			this.AllData.Add("99",new crossroadstageCell(99,15,6,5f,6,6,15f,17f,4.208f,5.21f,2,1,2));
			this.AllData.Add("100",new crossroadstageCell(100,15,6,5f,6,6,15f,17f,4.2f,5.2f,2,1,2));
			this.AllData.Add("101",new crossroadstageCell(101,15,6,5f,6,6,16f,18f,4.192f,5.19f,2,1,2));
			this.AllData.Add("102",new crossroadstageCell(102,15,6,5f,6,6,16f,18f,4.184f,5.18f,2,1,2));
			this.AllData.Add("103",new crossroadstageCell(103,15,6,5f,6,6,16f,18f,4.176f,5.17f,2,1,2));
			this.AllData.Add("104",new crossroadstageCell(104,15,6,5f,6,6,16f,18f,4.168f,5.16f,2,1,2));
			this.AllData.Add("105",new crossroadstageCell(105,15,6,5f,6,6,16f,18f,4.16f,5.15f,2,1,2));
			this.AllData.Add("106",new crossroadstageCell(106,15,6,5f,6,6,16f,18f,4.152f,5.14f,2,1,2));
			this.AllData.Add("107",new crossroadstageCell(107,15,6,5f,6,6,16f,18f,4.144f,5.13f,2,1,2));
			this.AllData.Add("108",new crossroadstageCell(108,15,6,5f,6,6,16f,18f,4.136f,5.12f,2,1,2));
			this.AllData.Add("109",new crossroadstageCell(109,15,6,5f,6,6,16f,18f,4.128f,5.11f,2,1,2));
			this.AllData.Add("110",new crossroadstageCell(110,15,6,5f,6,6,16f,18f,4.12f,5.1f,2,1,2));
			this.AllData.Add("111",new crossroadstageCell(111,15,6,5f,6,6,17f,19f,4.112f,5.09f,2,1,2));
			this.AllData.Add("112",new crossroadstageCell(112,15,6,5f,6,6,17f,19f,4.104f,5.08f,2,1,2));
			this.AllData.Add("113",new crossroadstageCell(113,15,6,5f,6,6,17f,19f,4.096f,5.07f,2,1,2));
			this.AllData.Add("114",new crossroadstageCell(114,15,6,5f,6,6,17f,19f,4.088f,5.06f,2,1,2));
			this.AllData.Add("115",new crossroadstageCell(115,15,6,5f,6,6,17f,19f,4.08f,5.05f,2,1,2));
			this.AllData.Add("116",new crossroadstageCell(116,15,6,5f,6,6,17f,19f,4.072f,5.04f,2,1,2));
			this.AllData.Add("117",new crossroadstageCell(117,15,6,5f,6,6,17f,19f,4.064f,5.03f,2,1,2));
			this.AllData.Add("118",new crossroadstageCell(118,15,6,5f,6,6,17f,19f,4.056f,5.02f,2,1,2));
			this.AllData.Add("119",new crossroadstageCell(119,15,6,5f,6,6,17f,19f,4.048f,5.01f,2,1,2));
			this.AllData.Add("120",new crossroadstageCell(120,15,6,5f,6,6,17f,19f,4.04f,5f,2,1,2));
			this.AllData.Add("121",new crossroadstageCell(121,15,6,5f,6,6,18f,20f,4.032f,4.99f,2,1,2));
			this.AllData.Add("122",new crossroadstageCell(122,15,6,5f,6,6,18f,20f,4.024f,4.98f,2,1,2));
			this.AllData.Add("123",new crossroadstageCell(123,15,6,5f,6,6,18f,20f,4.016f,4.97f,2,1,2));
			this.AllData.Add("124",new crossroadstageCell(124,15,6,5f,6,6,18f,20f,4.008f,4.96f,2,1,2));
			this.AllData.Add("125",new crossroadstageCell(125,15,6,5f,6,6,18f,20f,4f,4.95f,2,1,2));
			this.AllData.Add("126",new crossroadstageCell(126,15,6,5f,6,6,18f,20f,3.992f,4.94f,2,1,2));
			this.AllData.Add("127",new crossroadstageCell(127,15,6,5f,6,6,18f,20f,3.984f,4.93f,2,1,2));
			this.AllData.Add("128",new crossroadstageCell(128,15,6,5f,6,6,18f,20f,3.976f,4.92f,2,1,2));
			this.AllData.Add("129",new crossroadstageCell(129,15,6,5f,6,6,18f,20f,3.968f,4.91f,2,1,2));
			this.AllData.Add("130",new crossroadstageCell(130,15,6,5f,6,6,18f,20f,3.96f,4.9f,2,1,2));
			this.AllData.Add("131",new crossroadstageCell(131,15,6,5f,6,6,19f,21f,3.952f,4.89f,2,1,2));
			this.AllData.Add("132",new crossroadstageCell(132,15,6,5f,6,6,19f,21f,3.944f,4.88f,2,1,2));
			this.AllData.Add("133",new crossroadstageCell(133,15,6,5f,6,6,19f,21f,3.936f,4.87f,2,1,2));
			this.AllData.Add("134",new crossroadstageCell(134,15,6,5f,6,6,19f,21f,3.928f,4.86f,2,1,2));
			this.AllData.Add("135",new crossroadstageCell(135,15,6,5f,6,6,19f,21f,3.92f,4.85f,2,1,2));
			this.AllData.Add("136",new crossroadstageCell(136,15,6,5f,6,6,19f,21f,3.912f,4.84f,2,1,2));
			this.AllData.Add("137",new crossroadstageCell(137,15,6,5f,6,6,19f,21f,3.904f,4.83f,2,1,2));
			this.AllData.Add("138",new crossroadstageCell(138,15,6,5f,6,6,19f,21f,3.896f,4.82f,2,1,2));
			this.AllData.Add("139",new crossroadstageCell(139,15,6,5f,6,6,19f,21f,3.888f,4.81f,2,1,2));
			this.AllData.Add("140",new crossroadstageCell(140,15,6,5f,6,6,19f,21f,3.88f,4.8f,2,1,2));
			this.AllData.Add("141",new crossroadstageCell(141,15,6,5f,6,6,20f,22f,3.872f,4.79f,2,1,2));
			this.AllData.Add("142",new crossroadstageCell(142,15,6,5f,6,6,20f,22f,3.864f,4.78f,2,1,2));
			this.AllData.Add("143",new crossroadstageCell(143,15,6,5f,6,6,20f,22f,3.856f,4.77f,2,1,2));
			this.AllData.Add("144",new crossroadstageCell(144,15,6,5f,6,6,20f,22f,3.848f,4.76f,2,1,2));
			this.AllData.Add("145",new crossroadstageCell(145,15,6,5f,6,6,20f,22f,3.84f,4.75f,2,1,2));
			this.AllData.Add("146",new crossroadstageCell(146,15,6,5f,6,6,20f,22f,3.832f,4.74f,2,1,2));
			this.AllData.Add("147",new crossroadstageCell(147,15,6,5f,6,6,20f,22f,3.824f,4.73f,2,1,2));
			this.AllData.Add("148",new crossroadstageCell(148,15,6,5f,6,6,20f,22f,3.816f,4.72f,2,1,2));
			this.AllData.Add("149",new crossroadstageCell(149,15,6,5f,6,6,20f,22f,3.808f,4.71f,2,1,2));
			this.AllData.Add("150",new crossroadstageCell(150,15,6,5f,6,6,20f,22f,3.8f,4.7f,2,1,2));
			this.AllData.Add("151",new crossroadstageCell(151,15,6,5f,6,6,21f,23f,3.792f,4.69f,2,1,2));
			this.AllData.Add("152",new crossroadstageCell(152,15,6,5f,6,6,21f,23f,3.784f,4.68f,2,1,2));
			this.AllData.Add("153",new crossroadstageCell(153,15,6,5f,6,6,21f,23f,3.776f,4.67f,2,1,2));
			this.AllData.Add("154",new crossroadstageCell(154,15,6,5f,6,6,21f,23f,3.768f,4.66f,2,1,2));
			this.AllData.Add("155",new crossroadstageCell(155,15,6,5f,6,6,21f,23f,3.76f,4.65f,2,1,2));
			this.AllData.Add("156",new crossroadstageCell(156,15,6,5f,6,6,21f,23f,3.752f,4.64f,2,1,2));
			this.AllData.Add("157",new crossroadstageCell(157,15,6,5f,6,6,21f,23f,3.744f,4.63f,2,1,2));
			this.AllData.Add("158",new crossroadstageCell(158,15,6,5f,6,6,21f,23f,3.736f,4.62f,2,1,2));
			this.AllData.Add("159",new crossroadstageCell(159,15,6,5f,6,6,21f,23f,3.728f,4.61f,2,1,2));
			this.AllData.Add("160",new crossroadstageCell(160,15,6,5f,6,6,21f,23f,3.72f,4.6f,2,1,2));
			this.AllData.Add("161",new crossroadstageCell(161,15,6,5f,6,6,22f,24f,3.712f,4.59f,2,1,2));
			this.AllData.Add("162",new crossroadstageCell(162,15,6,5f,6,6,22f,24f,3.704f,4.58f,2,1,2));
			this.AllData.Add("163",new crossroadstageCell(163,15,6,5f,6,6,22f,24f,3.696f,4.57f,2,1,2));
			this.AllData.Add("164",new crossroadstageCell(164,15,6,5f,6,6,22f,24f,3.688f,4.56f,2,1,2));
			this.AllData.Add("165",new crossroadstageCell(165,15,6,5f,6,6,22f,24f,3.68f,4.55f,2,1,2));
			this.AllData.Add("166",new crossroadstageCell(166,15,6,5f,6,6,22f,24f,3.672f,4.54f,2,1,2));
			this.AllData.Add("167",new crossroadstageCell(167,15,6,5f,6,6,22f,24f,3.664f,4.53f,2,1,2));
			this.AllData.Add("168",new crossroadstageCell(168,15,6,5f,6,6,22f,24f,3.656f,4.52f,2,1,2));
			this.AllData.Add("169",new crossroadstageCell(169,15,6,5f,6,6,22f,24f,3.648f,4.51f,2,1,2));
			this.AllData.Add("170",new crossroadstageCell(170,15,6,5f,6,6,22f,24f,3.64f,4.5f,2,1,2));
			this.AllData.Add("171",new crossroadstageCell(171,15,6,5f,6,6,23f,25f,3.632f,4.49f,2,1,2));
			this.AllData.Add("172",new crossroadstageCell(172,15,6,5f,6,6,23f,25f,3.624f,4.48f,2,1,2));
			this.AllData.Add("173",new crossroadstageCell(173,15,6,5f,6,6,23f,25f,3.616f,4.47f,2,1,2));
			this.AllData.Add("174",new crossroadstageCell(174,15,6,5f,6,6,23f,25f,3.608f,4.46f,2,1,2));
			this.AllData.Add("175",new crossroadstageCell(175,15,6,5f,6,6,23f,25f,3.6f,4.45f,2,1,2));
			this.AllData.Add("176",new crossroadstageCell(176,15,6,5f,6,6,23f,25f,3.592f,4.44f,2,1,2));
			this.AllData.Add("177",new crossroadstageCell(177,15,6,5f,6,6,23f,25f,3.584f,4.43f,2,1,2));
			this.AllData.Add("178",new crossroadstageCell(178,15,6,5f,6,6,23f,25f,3.576f,4.42f,2,1,2));
			this.AllData.Add("179",new crossroadstageCell(179,15,6,5f,6,6,23f,25f,3.568f,4.41f,2,1,2));
			this.AllData.Add("180",new crossroadstageCell(180,15,6,5f,6,6,23f,25f,3.56f,4.4f,2,1,2));
			this.AllData.Add("181",new crossroadstageCell(181,15,6,5f,6,6,23f,25f,3.552f,4.39f,2,1,2));
			this.AllData.Add("182",new crossroadstageCell(182,15,6,5f,6,6,23f,25f,3.544f,4.38f,2,1,2));
			this.AllData.Add("183",new crossroadstageCell(183,15,6,5f,6,6,23f,25f,3.536f,4.37f,2,1,2));
			this.AllData.Add("184",new crossroadstageCell(184,15,6,5f,6,6,23f,25f,3.528f,4.36f,2,1,2));
			this.AllData.Add("185",new crossroadstageCell(185,15,6,5f,6,6,23f,25f,3.52f,4.35f,2,1,2));
			this.AllData.Add("186",new crossroadstageCell(186,15,6,5f,6,6,23f,25f,3.512f,4.34f,2,1,2));
			this.AllData.Add("187",new crossroadstageCell(187,15,6,5f,6,6,23f,25f,3.504f,4.33f,2,1,2));
			this.AllData.Add("188",new crossroadstageCell(188,15,6,5f,6,6,23f,25f,3.496f,4.32f,2,1,2));
			this.AllData.Add("189",new crossroadstageCell(189,15,6,5f,6,6,23f,25f,3.488f,4.31f,2,1,2));
			this.AllData.Add("190",new crossroadstageCell(190,15,6,5f,6,6,23f,25f,3.48f,4.3f,2,1,2));
			this.AllData.Add("191",new crossroadstageCell(191,15,6,5f,6,6,23f,25f,3.472f,4.29f,2,1,2));
			this.AllData.Add("192",new crossroadstageCell(192,15,6,5f,6,6,23f,25f,3.464f,4.28f,2,1,2));
			this.AllData.Add("193",new crossroadstageCell(193,15,6,5f,6,6,23f,25f,3.456f,4.27f,2,1,2));
			this.AllData.Add("194",new crossroadstageCell(194,15,6,5f,6,6,23f,25f,3.448f,4.26f,2,1,2));
			this.AllData.Add("195",new crossroadstageCell(195,15,6,5f,6,6,23f,25f,3.44f,4.25f,2,1,2));
			this.AllData.Add("196",new crossroadstageCell(196,15,6,5f,6,6,23f,25f,3.432f,4.24f,2,1,2));
			this.AllData.Add("197",new crossroadstageCell(197,15,6,5f,6,6,23f,25f,3.424f,4.23f,2,1,2));
			this.AllData.Add("198",new crossroadstageCell(198,15,6,5f,6,6,23f,25f,3.416f,4.22f,2,1,2));
			this.AllData.Add("199",new crossroadstageCell(199,15,6,5f,6,6,23f,25f,3.408f,4.21f,2,1,2));
			this.AllData.Add("200",new crossroadstageCell(200,15,6,5f,6,6,23f,25f,3.4f,4.2f,2,1,2));
			this.AllData.Add("201",new crossroadstageCell(201,15,6,5f,6,6,23f,25f,3.392f,4.19f,2,1,2));
			this.AllData.Add("202",new crossroadstageCell(202,15,6,5f,6,6,23f,25f,3.384f,4.18f,2,1,2));
			this.AllData.Add("203",new crossroadstageCell(203,15,6,5f,6,6,23f,25f,3.376f,4.17f,2,1,2));
			this.AllData.Add("204",new crossroadstageCell(204,15,6,5f,6,6,23f,25f,3.368f,4.16f,2,1,2));
			this.AllData.Add("205",new crossroadstageCell(205,15,6,5f,6,6,23f,25f,3.36f,4.15f,2,1,2));
			this.AllData.Add("206",new crossroadstageCell(206,15,6,5f,6,6,23f,25f,3.352f,4.14f,2,1,2));
			this.AllData.Add("207",new crossroadstageCell(207,15,6,5f,6,6,23f,25f,3.344f,4.13f,2,1,2));
			this.AllData.Add("208",new crossroadstageCell(208,15,6,5f,6,6,23f,25f,3.336f,4.12f,2,1,2));
			this.AllData.Add("209",new crossroadstageCell(209,15,6,5f,6,6,23f,25f,3.328f,4.11f,2,1,2));
			this.AllData.Add("210",new crossroadstageCell(210,15,6,5f,6,6,23f,25f,3.32f,4.1f,2,1,2));
			this.AllData.Add("211",new crossroadstageCell(211,15,6,5f,6,6,23f,25f,3.312f,4.09f,2,1,2));
			this.AllData.Add("212",new crossroadstageCell(212,15,6,5f,6,6,23f,25f,3.304f,4.08f,2,1,2));
			this.AllData.Add("213",new crossroadstageCell(213,15,6,5f,6,6,23f,25f,3.296f,4.07f,2,1,2));
			this.AllData.Add("214",new crossroadstageCell(214,15,6,5f,6,6,23f,25f,3.288f,4.06f,2,1,2));
			this.AllData.Add("215",new crossroadstageCell(215,15,6,5f,6,6,23f,25f,3.28f,4.05f,2,1,2));
			this.AllData.Add("216",new crossroadstageCell(216,15,6,5f,6,6,23f,25f,3.272f,4.04f,2,1,2));
			this.AllData.Add("217",new crossroadstageCell(217,15,6,5f,6,6,23f,25f,3.264f,4.03f,2,1,2));
			this.AllData.Add("218",new crossroadstageCell(218,15,6,5f,6,6,23f,25f,3.256f,4.02f,2,1,2));
			this.AllData.Add("219",new crossroadstageCell(219,15,6,5f,6,6,23f,25f,3.248f,4.01f,2,1,2));
			this.AllData.Add("220",new crossroadstageCell(220,15,6,5f,6,6,23f,25f,3.24f,4f,2,1,2));
			this.AllData.Add("221",new crossroadstageCell(221,15,6,5f,6,6,23f,25f,3.232f,3.99f,2,1,2));
			this.AllData.Add("222",new crossroadstageCell(222,15,6,5f,6,6,23f,25f,3.224f,3.98f,2,1,2));
			this.AllData.Add("223",new crossroadstageCell(223,15,6,5f,6,6,23f,25f,3.216f,3.97f,2,1,2));
			this.AllData.Add("224",new crossroadstageCell(224,15,6,5f,6,6,23f,25f,3.208f,3.96f,2,1,2));
			this.AllData.Add("225",new crossroadstageCell(225,15,6,5f,6,6,23f,25f,3.2f,3.95f,2,1,2));
			this.AllData.Add("226",new crossroadstageCell(226,15,6,5f,6,6,23f,25f,3.192f,3.94f,2,1,2));
			this.AllData.Add("227",new crossroadstageCell(227,15,6,5f,6,6,23f,25f,3.184f,3.93f,2,1,2));
			this.AllData.Add("228",new crossroadstageCell(228,15,6,5f,6,6,23f,25f,3.176f,3.92f,2,1,2));
			this.AllData.Add("229",new crossroadstageCell(229,15,6,5f,6,6,23f,25f,3.168f,3.91f,2,1,2));
			this.AllData.Add("230",new crossroadstageCell(230,15,6,5f,6,6,23f,25f,3.16f,3.9f,2,1,2));
			this.AllData.Add("231",new crossroadstageCell(231,15,6,5f,6,6,23f,25f,3.152f,3.89f,2,1,2));
			this.AllData.Add("232",new crossroadstageCell(232,15,6,5f,6,6,23f,25f,3.144f,3.88f,2,1,2));
			this.AllData.Add("233",new crossroadstageCell(233,15,6,5f,6,6,23f,25f,3.136f,3.87f,2,1,2));
			this.AllData.Add("234",new crossroadstageCell(234,15,6,5f,6,6,23f,25f,3.128f,3.86f,2,1,2));
			this.AllData.Add("235",new crossroadstageCell(235,15,6,5f,6,6,23f,25f,3.12f,3.85f,2,1,2));
			this.AllData.Add("236",new crossroadstageCell(236,15,6,5f,6,6,23f,25f,3.112f,3.84f,2,1,2));
			this.AllData.Add("237",new crossroadstageCell(237,15,6,5f,6,6,23f,25f,3.104f,3.83f,2,1,2));
			this.AllData.Add("238",new crossroadstageCell(238,15,6,5f,6,6,23f,25f,3.096f,3.82f,2,1,2));
			this.AllData.Add("239",new crossroadstageCell(239,15,6,5f,6,6,23f,25f,3.088f,3.81f,2,1,2));
			this.AllData.Add("240",new crossroadstageCell(240,15,6,5f,6,6,23f,25f,3.08f,3.8f,2,1,2));
			this.AllData.Add("241",new crossroadstageCell(241,15,6,5f,6,6,23f,25f,3.072f,3.79f,2,1,2));
			this.AllData.Add("242",new crossroadstageCell(242,15,6,5f,6,6,23f,25f,3.064f,3.78f,2,1,2));
			this.AllData.Add("243",new crossroadstageCell(243,15,6,5f,6,6,23f,25f,3.056f,3.77f,2,1,2));
			this.AllData.Add("244",new crossroadstageCell(244,15,6,5f,6,6,23f,25f,3.048f,3.76f,2,1,2));
			this.AllData.Add("245",new crossroadstageCell(245,15,6,5f,6,6,23f,25f,3.04f,3.75f,2,1,2));
			this.AllData.Add("246",new crossroadstageCell(246,15,6,5f,6,6,23f,25f,3.032f,3.74f,2,1,2));
			this.AllData.Add("247",new crossroadstageCell(247,15,6,5f,6,6,23f,25f,3.024f,3.73f,2,1,2));
			this.AllData.Add("248",new crossroadstageCell(248,15,6,5f,6,6,23f,25f,3.016f,3.72f,2,1,2));
			this.AllData.Add("249",new crossroadstageCell(249,15,6,5f,6,6,23f,25f,3.008f,3.71f,2,1,2));
			this.AllData.Add("250",new crossroadstageCell(250,15,6,5f,6,6,23f,25f,3f,3.7f,2,1,2));
			this.AllData.Add("251",new crossroadstageCell(251,15,6,5f,6,6,23f,25f,2.992f,3.69f,2,1,2));
			this.AllData.Add("252",new crossroadstageCell(252,15,6,5f,6,6,23f,25f,2.984f,3.68f,2,1,2));
			this.AllData.Add("253",new crossroadstageCell(253,15,6,5f,6,6,23f,25f,2.976f,3.67f,2,1,2));
			this.AllData.Add("254",new crossroadstageCell(254,15,6,5f,6,6,23f,25f,2.968f,3.66f,2,1,2));
			this.AllData.Add("255",new crossroadstageCell(255,15,6,5f,6,6,23f,25f,2.96f,3.65f,2,1,2));
			this.AllData.Add("256",new crossroadstageCell(256,15,6,5f,6,6,23f,25f,2.952f,3.64f,2,1,2));
			this.AllData.Add("257",new crossroadstageCell(257,15,6,5f,6,6,23f,25f,2.944f,3.63f,2,1,2));
			this.AllData.Add("258",new crossroadstageCell(258,15,6,5f,6,6,23f,25f,2.936f,3.62f,2,1,2));
			this.AllData.Add("259",new crossroadstageCell(259,15,6,5f,6,6,23f,25f,2.928f,3.61f,2,1,2));
			this.AllData.Add("260",new crossroadstageCell(260,15,6,5f,6,6,23f,25f,2.92f,3.6f,2,1,2));
			this.AllData.Add("261",new crossroadstageCell(261,15,6,5f,6,6,23f,25f,2.912f,3.59f,2,1,2));
			this.AllData.Add("262",new crossroadstageCell(262,15,6,5f,6,6,23f,25f,2.904f,3.58f,2,1,2));
			this.AllData.Add("263",new crossroadstageCell(263,15,6,5f,6,6,23f,25f,2.896f,3.57f,2,1,2));
			this.AllData.Add("264",new crossroadstageCell(264,15,6,5f,6,6,23f,25f,2.888f,3.56f,2,1,2));
			this.AllData.Add("265",new crossroadstageCell(265,15,6,5f,6,6,23f,25f,2.88f,3.55f,2,1,2));
			this.AllData.Add("266",new crossroadstageCell(266,15,6,5f,6,6,23f,25f,2.872f,3.54f,2,1,2));
			this.AllData.Add("267",new crossroadstageCell(267,15,6,5f,6,6,23f,25f,2.864f,3.53f,2,1,2));
			this.AllData.Add("268",new crossroadstageCell(268,15,6,5f,6,6,23f,25f,2.856f,3.52f,2,1,2));
			this.AllData.Add("269",new crossroadstageCell(269,15,6,5f,6,6,23f,25f,2.848f,3.51f,2,1,2));
			this.AllData.Add("270",new crossroadstageCell(270,15,6,5f,6,6,23f,25f,2.84f,3.5f,2,1,2));
			this.AllData.Add("271",new crossroadstageCell(271,15,6,5f,6,6,23f,25f,2.832f,3.49f,2,1,2));
			this.AllData.Add("272",new crossroadstageCell(272,15,6,5f,6,6,23f,25f,2.824f,3.48f,2,1,2));
			this.AllData.Add("273",new crossroadstageCell(273,15,6,5f,6,6,23f,25f,2.816f,3.47f,2,1,2));
			this.AllData.Add("274",new crossroadstageCell(274,15,6,5f,6,6,23f,25f,2.808f,3.46f,2,1,2));
			this.AllData.Add("275",new crossroadstageCell(275,15,6,5f,6,6,23f,25f,2.8f,3.45f,2,1,2));
			this.AllData.Add("276",new crossroadstageCell(276,15,6,5f,6,6,23f,25f,2.792f,3.44f,2,1,2));
			this.AllData.Add("277",new crossroadstageCell(277,15,6,5f,6,6,23f,25f,2.784f,3.43f,2,1,2));
			this.AllData.Add("278",new crossroadstageCell(278,15,6,5f,6,6,23f,25f,2.776f,3.42f,2,1,2));
			this.AllData.Add("279",new crossroadstageCell(279,15,6,5f,6,6,23f,25f,2.768f,3.41f,2,1,2));
			this.AllData.Add("280",new crossroadstageCell(280,15,6,5f,6,6,23f,25f,2.76f,3.4f,2,1,2));
			this.AllData.Add("281",new crossroadstageCell(281,15,6,5f,6,6,23f,25f,2.752f,3.39f,2,1,2));
			this.AllData.Add("282",new crossroadstageCell(282,15,6,5f,6,6,23f,25f,2.744f,3.38f,2,1,2));
			this.AllData.Add("283",new crossroadstageCell(283,15,6,5f,6,6,23f,25f,2.736f,3.37f,2,1,2));
			this.AllData.Add("284",new crossroadstageCell(284,15,6,5f,6,6,23f,25f,2.728f,3.36f,2,1,2));
			this.AllData.Add("285",new crossroadstageCell(285,15,6,5f,6,6,23f,25f,2.72f,3.35f,2,1,2));
			this.AllData.Add("286",new crossroadstageCell(286,15,6,5f,6,6,23f,25f,2.712f,3.34f,2,1,2));
			this.AllData.Add("287",new crossroadstageCell(287,15,6,5f,6,6,23f,25f,2.704f,3.33f,2,1,2));
			this.AllData.Add("288",new crossroadstageCell(288,15,6,5f,6,6,23f,25f,2.696f,3.32f,2,1,2));
			this.AllData.Add("289",new crossroadstageCell(289,15,6,5f,6,6,23f,25f,2.688f,3.31f,2,1,2));
			this.AllData.Add("290",new crossroadstageCell(290,15,6,5f,6,6,23f,25f,2.68f,3.3f,2,1,2));
			this.AllData.Add("291",new crossroadstageCell(291,15,6,5f,6,6,23f,25f,2.672f,3.29f,2,1,2));
			this.AllData.Add("292",new crossroadstageCell(292,15,6,5f,6,6,23f,25f,2.664f,3.28f,2,1,2));
			this.AllData.Add("293",new crossroadstageCell(293,15,6,5f,6,6,23f,25f,2.656f,3.27f,2,1,2));
			this.AllData.Add("294",new crossroadstageCell(294,15,6,5f,6,6,23f,25f,2.648f,3.26f,2,1,2));
			this.AllData.Add("295",new crossroadstageCell(295,15,6,5f,6,6,23f,25f,2.64f,3.25f,2,1,2));
			this.AllData.Add("296",new crossroadstageCell(296,15,6,5f,6,6,23f,25f,2.632f,3.24f,2,1,2));
			this.AllData.Add("297",new crossroadstageCell(297,15,6,5f,6,6,23f,25f,2.624f,3.23f,2,1,2));
			this.AllData.Add("298",new crossroadstageCell(298,15,6,5f,6,6,23f,25f,2.616f,3.22f,2,1,2));
			this.AllData.Add("299",new crossroadstageCell(299,15,6,5f,6,6,23f,25f,2.608f,3.21f,2,1,2));
			this.AllData.Add("300",new crossroadstageCell(300,15,6,5f,6,6,23f,25f,2.6f,3.2f,2,1,2));
			this.AllData.Add("301",new crossroadstageCell(301,15,6,5f,6,6,23f,25f,2.592f,3.19f,2,1,2));
			this.AllData.Add("302",new crossroadstageCell(302,15,6,5f,6,6,23f,25f,2.584f,3.18f,2,1,2));
			this.AllData.Add("303",new crossroadstageCell(303,15,6,5f,6,6,23f,25f,2.576f,3.17f,2,1,2));
			this.AllData.Add("304",new crossroadstageCell(304,15,6,5f,6,6,23f,25f,2.568f,3.16f,2,1,2));
			this.AllData.Add("305",new crossroadstageCell(305,15,6,5f,6,6,23f,25f,2.56f,3.15f,2,1,2));
			this.AllData.Add("306",new crossroadstageCell(306,15,6,5f,6,6,23f,25f,2.552f,3.14f,2,1,2));
			this.AllData.Add("307",new crossroadstageCell(307,15,6,5f,6,6,23f,25f,2.544f,3.13f,2,1,2));
			this.AllData.Add("308",new crossroadstageCell(308,15,6,5f,6,6,23f,25f,2.536f,3.12f,2,1,2));
			this.AllData.Add("309",new crossroadstageCell(309,15,6,5f,6,6,23f,25f,2.528f,3.11f,2,1,2));
			this.AllData.Add("310",new crossroadstageCell(310,15,6,5f,6,6,23f,25f,2.52f,3.1f,2,1,2));
			this.AllData.Add("311",new crossroadstageCell(311,15,6,5f,6,6,23f,25f,2.512f,3.09f,2,1,2));
			this.AllData.Add("312",new crossroadstageCell(312,15,6,5f,6,6,23f,25f,2.504f,3.08f,2,1,2));
			this.AllData.Add("313",new crossroadstageCell(313,15,6,5f,6,6,23f,25f,2.496f,3.07f,2,1,2));
			this.AllData.Add("314",new crossroadstageCell(314,15,6,5f,6,6,23f,25f,2.488f,3.06f,2,1,2));
			this.AllData.Add("315",new crossroadstageCell(315,15,6,5f,6,6,23f,25f,2.48f,3.05f,2,1,2));
			this.AllData.Add("316",new crossroadstageCell(316,15,6,5f,6,6,23f,25f,2.472f,3.04f,2,1,2));
			this.AllData.Add("317",new crossroadstageCell(317,15,6,5f,6,6,23f,25f,2.464f,3.03f,2,1,2));
			this.AllData.Add("318",new crossroadstageCell(318,15,6,5f,6,6,23f,25f,2.456f,3.02f,2,1,2));
			this.AllData.Add("319",new crossroadstageCell(319,15,6,5f,6,6,23f,25f,2.448f,3.01f,2,1,2));
			this.AllData.Add("320",new crossroadstageCell(320,15,6,5f,6,6,23f,25f,2.44f,3f,2,1,2));
			this.AllData.Add("321",new crossroadstageCell(321,15,6,5f,6,6,23f,25f,2.432f,2.99f,2,1,2));
			this.AllData.Add("322",new crossroadstageCell(322,15,6,5f,6,6,23f,25f,2.424f,2.98f,2,1,2));
			this.AllData.Add("323",new crossroadstageCell(323,15,6,5f,6,6,23f,25f,2.416f,2.97f,2,1,2));
			this.AllData.Add("324",new crossroadstageCell(324,15,6,5f,6,6,23f,25f,2.408f,2.96f,2,1,2));
			this.AllData.Add("325",new crossroadstageCell(325,15,6,5f,6,6,23f,25f,2.4f,2.95f,2,1,2));
			this.AllData.Add("326",new crossroadstageCell(326,15,6,5f,6,6,23f,25f,2.392f,2.94f,2,1,2));
			this.AllData.Add("327",new crossroadstageCell(327,15,6,5f,6,6,23f,25f,2.384f,2.93f,2,1,2));
			this.AllData.Add("328",new crossroadstageCell(328,15,6,5f,6,6,23f,25f,2.376f,2.92f,2,1,2));
			this.AllData.Add("329",new crossroadstageCell(329,15,6,5f,6,6,23f,25f,2.368f,2.91f,2,1,2));
			this.AllData.Add("330",new crossroadstageCell(330,15,6,5f,6,6,23f,25f,2.36f,2.9f,2,1,2));
			this.AllData.Add("331",new crossroadstageCell(331,15,6,5f,6,6,23f,25f,2.352f,2.89f,2,1,2));
			this.AllData.Add("332",new crossroadstageCell(332,15,6,5f,6,6,23f,25f,2.344f,2.88f,2,1,2));
			this.AllData.Add("333",new crossroadstageCell(333,15,6,5f,6,6,23f,25f,2.336f,2.87f,2,1,2));
			this.AllData.Add("334",new crossroadstageCell(334,15,6,5f,6,6,23f,25f,2.328f,2.86f,2,1,2));
			this.AllData.Add("335",new crossroadstageCell(335,15,6,5f,6,6,23f,25f,2.32f,2.85f,2,1,2));
			this.AllData.Add("336",new crossroadstageCell(336,15,6,5f,6,6,23f,25f,2.312f,2.84f,2,1,2));
			this.AllData.Add("337",new crossroadstageCell(337,15,6,5f,6,6,23f,25f,2.304f,2.83f,2,1,2));
			this.AllData.Add("338",new crossroadstageCell(338,15,6,5f,6,6,23f,25f,2.296f,2.82f,2,1,2));
			this.AllData.Add("339",new crossroadstageCell(339,15,6,5f,6,6,23f,25f,2.288f,2.81f,2,1,2));
			this.AllData.Add("340",new crossroadstageCell(340,15,6,5f,6,6,23f,25f,2.28f,2.8f,2,1,2));
			this.AllData.Add("341",new crossroadstageCell(341,15,6,5f,6,6,23f,25f,2.272f,2.79f,2,1,2));
			this.AllData.Add("342",new crossroadstageCell(342,15,6,5f,6,6,23f,25f,2.264f,2.78f,2,1,2));
			this.AllData.Add("343",new crossroadstageCell(343,15,6,5f,6,6,23f,25f,2.256f,2.77f,2,1,2));
			this.AllData.Add("344",new crossroadstageCell(344,15,6,5f,6,6,23f,25f,2.248f,2.76f,2,1,2));
			this.AllData.Add("345",new crossroadstageCell(345,15,6,5f,6,6,23f,25f,2.24f,2.75f,2,1,2));
			this.AllData.Add("346",new crossroadstageCell(346,15,6,5f,6,6,23f,25f,2.232f,2.74f,2,1,2));
			this.AllData.Add("347",new crossroadstageCell(347,15,6,5f,6,6,23f,25f,2.224f,2.73f,2,1,2));
			this.AllData.Add("348",new crossroadstageCell(348,15,6,5f,6,6,23f,25f,2.216f,2.72f,2,1,2));
			this.AllData.Add("349",new crossroadstageCell(349,15,6,5f,6,6,23f,25f,2.208f,2.71f,2,1,2));
			this.AllData.Add("350",new crossroadstageCell(350,15,6,5f,6,6,23f,25f,2.2f,2.7f,2,1,2));
			this.AllData.Add("351",new crossroadstageCell(351,15,6,5f,6,6,23f,25f,2.192f,2.69f,2,1,2));
			this.AllData.Add("352",new crossroadstageCell(352,15,6,5f,6,6,23f,25f,2.184f,2.68f,2,1,2));
			this.AllData.Add("353",new crossroadstageCell(353,15,6,5f,6,6,23f,25f,2.176f,2.67f,2,1,2));
			this.AllData.Add("354",new crossroadstageCell(354,15,6,5f,6,6,23f,25f,2.168f,2.66f,2,1,2));
			this.AllData.Add("355",new crossroadstageCell(355,15,6,5f,6,6,23f,25f,2.16f,2.65f,2,1,2));
			this.AllData.Add("356",new crossroadstageCell(356,15,6,5f,6,6,23f,25f,2.152f,2.64f,2,1,2));
			this.AllData.Add("357",new crossroadstageCell(357,15,6,5f,6,6,23f,25f,2.144f,2.63f,2,1,2));
			this.AllData.Add("358",new crossroadstageCell(358,15,6,5f,6,6,23f,25f,2.136f,2.62f,2,1,2));
			this.AllData.Add("359",new crossroadstageCell(359,15,6,5f,6,6,23f,25f,2.128f,2.61f,2,1,2));
			this.AllData.Add("360",new crossroadstageCell(360,15,6,5f,6,6,23f,25f,2.12f,2.6f,2,1,2));
			this.AllData.Add("361",new crossroadstageCell(361,15,6,5f,6,6,23f,25f,2.112f,2.59f,2,1,2));
			this.AllData.Add("362",new crossroadstageCell(362,15,6,5f,6,6,23f,25f,2.104f,2.58f,2,1,2));
			this.AllData.Add("363",new crossroadstageCell(363,15,6,5f,6,6,23f,25f,2.096f,2.57f,2,1,2));
			this.AllData.Add("364",new crossroadstageCell(364,15,6,5f,6,6,23f,25f,2.088f,2.56f,2,1,2));
			this.AllData.Add("365",new crossroadstageCell(365,15,6,5f,6,6,23f,25f,2.08f,2.55f,2,1,2));
			this.AllData.Add("366",new crossroadstageCell(366,15,6,5f,6,6,23f,25f,2.072f,2.54f,2,1,2));
			this.AllData.Add("367",new crossroadstageCell(367,15,6,5f,6,6,23f,25f,2.064f,2.53f,2,1,2));
			this.AllData.Add("368",new crossroadstageCell(368,15,6,5f,6,6,23f,25f,2.056f,2.52f,2,1,2));
			this.AllData.Add("369",new crossroadstageCell(369,15,6,5f,6,6,23f,25f,2.048f,2.51f,2,1,2));
			this.AllData.Add("370",new crossroadstageCell(370,15,6,5f,6,6,23f,25f,2.04f,2.5f,2,1,2));
			this.AllData.Add("371",new crossroadstageCell(371,15,6,5f,6,6,23f,25f,2.032f,2.49f,2,1,2));
			this.AllData.Add("372",new crossroadstageCell(372,15,6,5f,6,6,23f,25f,2.024f,2.48f,2,1,2));
			this.AllData.Add("373",new crossroadstageCell(373,15,6,5f,6,6,23f,25f,2.016f,2.47f,2,1,2));
			this.AllData.Add("374",new crossroadstageCell(374,15,6,5f,6,6,23f,25f,2.008f,2.46f,2,1,2));
			this.AllData.Add("375",new crossroadstageCell(375,15,6,5f,6,6,23f,25f,2f,2.45f,2,1,2));
			this.AllData.Add("376",new crossroadstageCell(376,15,6,5f,6,6,23f,25f,1.992f,2.44f,2,1,2));
			this.AllData.Add("377",new crossroadstageCell(377,15,6,5f,6,6,23f,25f,1.984f,2.43f,2,1,2));
			this.AllData.Add("378",new crossroadstageCell(378,15,6,5f,6,6,23f,25f,1.976f,2.42f,2,1,2));
			this.AllData.Add("379",new crossroadstageCell(379,15,6,5f,6,6,23f,25f,1.968f,2.41f,2,1,2));
			this.AllData.Add("380",new crossroadstageCell(380,15,6,5f,6,6,23f,25f,1.96f,2.4f,2,1,2));
			this.AllData.Add("381",new crossroadstageCell(381,15,6,5f,6,6,23f,25f,1.952f,2.39f,2,1,2));
			this.AllData.Add("382",new crossroadstageCell(382,15,6,5f,6,6,23f,25f,1.944f,2.38f,2,1,2));
			this.AllData.Add("383",new crossroadstageCell(383,15,6,5f,6,6,23f,25f,1.936f,2.37f,2,1,2));
			this.AllData.Add("384",new crossroadstageCell(384,15,6,5f,6,6,23f,25f,1.928f,2.36f,2,1,2));
			this.AllData.Add("385",new crossroadstageCell(385,15,6,5f,6,6,23f,25f,1.92f,2.35f,2,1,2));
			this.AllData.Add("386",new crossroadstageCell(386,15,6,5f,6,6,23f,25f,1.912f,2.34f,2,1,2));
			this.AllData.Add("387",new crossroadstageCell(387,15,6,5f,6,6,23f,25f,1.904f,2.33f,2,1,2));
			this.AllData.Add("388",new crossroadstageCell(388,15,6,5f,6,6,23f,25f,1.896f,2.32f,2,1,2));
			this.AllData.Add("389",new crossroadstageCell(389,15,6,5f,6,6,23f,25f,1.888f,2.31f,2,1,2));
			this.AllData.Add("390",new crossroadstageCell(390,15,6,5f,6,6,23f,25f,1.88f,2.3f,2,1,2));
			this.AllData.Add("391",new crossroadstageCell(391,15,6,5f,6,6,23f,25f,1.872f,2.29f,2,1,2));
			this.AllData.Add("392",new crossroadstageCell(392,15,6,5f,6,6,23f,25f,1.864f,2.28f,2,1,2));
			this.AllData.Add("393",new crossroadstageCell(393,15,6,5f,6,6,23f,25f,1.856f,2.27f,2,1,2));
			this.AllData.Add("394",new crossroadstageCell(394,15,6,5f,6,6,23f,25f,1.848f,2.26f,2,1,2));
			this.AllData.Add("395",new crossroadstageCell(395,15,6,5f,6,6,23f,25f,1.84f,2.25f,2,1,2));
			this.AllData.Add("396",new crossroadstageCell(396,15,6,5f,6,6,23f,25f,1.832f,2.24f,2,1,2));
			this.AllData.Add("397",new crossroadstageCell(397,15,6,5f,6,6,23f,25f,1.824f,2.23f,2,1,2));
			this.AllData.Add("398",new crossroadstageCell(398,15,6,5f,6,6,23f,25f,1.816f,2.22f,2,1,2));
			this.AllData.Add("399",new crossroadstageCell(399,15,6,5f,6,6,23f,25f,1.808f,2.21f,2,1,2));
			this.AllData.Add("400",new crossroadstageCell(400,15,6,5f,6,6,23f,25f,1.8f,2.2f,2,1,2));
			this.AllData.Add("401",new crossroadstageCell(401,15,6,5f,6,6,23f,25f,1.792f,2.19f,2,1,2));
			this.AllData.Add("402",new crossroadstageCell(402,15,6,5f,6,6,23f,25f,1.784f,2.18f,2,1,2));
			this.AllData.Add("403",new crossroadstageCell(403,15,6,5f,6,6,23f,25f,1.776f,2.17f,2,1,2));
			this.AllData.Add("404",new crossroadstageCell(404,15,6,5f,6,6,23f,25f,1.768f,2.16f,2,1,2));
			this.AllData.Add("405",new crossroadstageCell(405,15,6,5f,6,6,23f,25f,1.76f,2.15f,2,1,2));
			this.AllData.Add("406",new crossroadstageCell(406,15,6,5f,6,6,23f,25f,1.752f,2.14f,2,1,2));
			this.AllData.Add("407",new crossroadstageCell(407,15,6,5f,6,6,23f,25f,1.744f,2.13f,2,1,2));
			this.AllData.Add("408",new crossroadstageCell(408,15,6,5f,6,6,23f,25f,1.736f,2.12f,2,1,2));
			this.AllData.Add("409",new crossroadstageCell(409,15,6,5f,6,6,23f,25f,1.728f,2.11f,2,1,2));
			this.AllData.Add("410",new crossroadstageCell(410,15,6,5f,6,6,23f,25f,1.72f,2.1f,2,1,2));
			this.AllData.Add("411",new crossroadstageCell(411,15,6,5f,6,6,23f,25f,1.712f,2.09f,2,1,2));
			this.AllData.Add("412",new crossroadstageCell(412,15,6,5f,6,6,23f,25f,1.704f,2.08f,2,1,2));
			this.AllData.Add("413",new crossroadstageCell(413,15,6,5f,6,6,23f,25f,1.696f,2.07f,2,1,2));
			this.AllData.Add("414",new crossroadstageCell(414,15,6,5f,6,6,23f,25f,1.688f,2.06f,2,1,2));
			this.AllData.Add("415",new crossroadstageCell(415,15,6,5f,6,6,23f,25f,1.68f,2.05f,2,1,2));
			this.AllData.Add("416",new crossroadstageCell(416,15,6,5f,6,6,23f,25f,1.672f,2.04f,2,1,2));
			this.AllData.Add("417",new crossroadstageCell(417,15,6,5f,6,6,23f,25f,1.664f,2.03f,2,1,2));
			this.AllData.Add("418",new crossroadstageCell(418,15,6,5f,6,6,23f,25f,1.656f,2.02f,2,1,2));
			this.AllData.Add("419",new crossroadstageCell(419,15,6,5f,6,6,23f,25f,1.648f,2.01f,2,1,2));
			this.AllData.Add("420",new crossroadstageCell(420,15,6,5f,6,6,23f,25f,1.64f,2f,2,1,2));
			this.AllData.Add("421",new crossroadstageCell(421,15,6,5f,6,6,23f,25f,1.632f,1.99f,2,1,2));
			this.AllData.Add("422",new crossroadstageCell(422,15,6,5f,6,6,23f,25f,1.624f,1.98f,2,1,2));
			this.AllData.Add("423",new crossroadstageCell(423,15,6,5f,6,6,23f,25f,1.616f,1.97f,2,1,2));
			this.AllData.Add("424",new crossroadstageCell(424,15,6,5f,6,6,23f,25f,1.608f,1.96f,2,1,2));
			this.AllData.Add("425",new crossroadstageCell(425,15,6,5f,6,6,23f,25f,1.6f,1.95f,2,1,2));
			this.AllData.Add("426",new crossroadstageCell(426,15,6,5f,6,6,23f,25f,1.592f,1.94f,2,1,2));
			this.AllData.Add("427",new crossroadstageCell(427,15,6,5f,6,6,23f,25f,1.584f,1.93f,2,1,2));
			this.AllData.Add("428",new crossroadstageCell(428,15,6,5f,6,6,23f,25f,1.576f,1.92f,2,1,2));
			this.AllData.Add("429",new crossroadstageCell(429,15,6,5f,6,6,23f,25f,1.568f,1.91f,2,1,2));
			this.AllData.Add("430",new crossroadstageCell(430,15,6,5f,6,6,23f,25f,1.56f,1.9f,2,1,2));
			this.AllData.Add("431",new crossroadstageCell(431,15,6,5f,6,6,23f,25f,1.552f,1.89f,2,1,2));
			this.AllData.Add("432",new crossroadstageCell(432,15,6,5f,6,6,23f,25f,1.544f,1.88f,2,1,2));
			this.AllData.Add("433",new crossroadstageCell(433,15,6,5f,6,6,23f,25f,1.536f,1.87f,2,1,2));
			this.AllData.Add("434",new crossroadstageCell(434,15,6,5f,6,6,23f,25f,1.528f,1.86f,2,1,2));
			this.AllData.Add("435",new crossroadstageCell(435,15,6,5f,6,6,23f,25f,1.52f,1.85f,2,1,2));
			this.AllData.Add("436",new crossroadstageCell(436,15,6,5f,6,6,23f,25f,1.512f,1.84f,2,1,2));
			this.AllData.Add("437",new crossroadstageCell(437,15,6,5f,6,6,23f,25f,1.504f,1.83f,2,1,2));
			this.AllData.Add("438",new crossroadstageCell(438,15,6,5f,6,6,23f,25f,1.496f,1.82f,2,1,2));
			this.AllData.Add("439",new crossroadstageCell(439,15,6,5f,6,6,23f,25f,1.488f,1.81f,2,1,2));
			this.AllData.Add("440",new crossroadstageCell(440,15,6,5f,6,6,23f,25f,1.48f,1.8f,2,1,2));
			this.AllData.Add("441",new crossroadstageCell(441,15,6,5f,6,6,23f,25f,1.472f,1.79f,2,1,2));
			this.AllData.Add("442",new crossroadstageCell(442,15,6,5f,6,6,23f,25f,1.464f,1.78f,2,1,2));
			this.AllData.Add("443",new crossroadstageCell(443,15,6,5f,6,6,23f,25f,1.456f,1.77f,2,1,2));
			this.AllData.Add("444",new crossroadstageCell(444,15,6,5f,6,6,23f,25f,1.448f,1.76f,2,1,2));
			this.AllData.Add("445",new crossroadstageCell(445,15,6,5f,6,6,23f,25f,1.44f,1.75f,2,1,2));
			this.AllData.Add("446",new crossroadstageCell(446,15,6,5f,6,6,23f,25f,1.432f,1.74f,2,1,2));
			this.AllData.Add("447",new crossroadstageCell(447,15,6,5f,6,6,23f,25f,1.424f,1.73f,2,1,2));
			this.AllData.Add("448",new crossroadstageCell(448,15,6,5f,6,6,23f,25f,1.416f,1.72f,2,1,2));
			this.AllData.Add("449",new crossroadstageCell(449,15,6,5f,6,6,23f,25f,1.408f,1.71f,2,1,2));
			this.AllData.Add("450",new crossroadstageCell(450,15,6,5f,6,6,23f,25f,1.4f,1.7f,2,1,2));
			this.AllData.Add("451",new crossroadstageCell(451,15,6,5f,6,6,23f,25f,1.392f,1.69f,2,1,2));
			this.AllData.Add("452",new crossroadstageCell(452,15,6,5f,6,6,23f,25f,1.384f,1.68f,2,1,2));
			this.AllData.Add("453",new crossroadstageCell(453,15,6,5f,6,6,23f,25f,1.376f,1.67f,2,1,2));
			this.AllData.Add("454",new crossroadstageCell(454,15,6,5f,6,6,23f,25f,1.368f,1.66f,2,1,2));
			this.AllData.Add("455",new crossroadstageCell(455,15,6,5f,6,6,23f,25f,1.36f,1.65f,2,1,2));
			this.AllData.Add("456",new crossroadstageCell(456,15,6,5f,6,6,23f,25f,1.352f,1.64f,2,1,2));
			this.AllData.Add("457",new crossroadstageCell(457,15,6,5f,6,6,23f,25f,1.344f,1.63f,2,1,2));
			this.AllData.Add("458",new crossroadstageCell(458,15,6,5f,6,6,23f,25f,1.336f,1.62f,2,1,2));
			this.AllData.Add("459",new crossroadstageCell(459,15,6,5f,6,6,23f,25f,1.328f,1.61f,2,1,2));
			this.AllData.Add("460",new crossroadstageCell(460,15,6,5f,6,6,23f,25f,1.32f,1.6f,2,1,2));
			this.AllData.Add("461",new crossroadstageCell(461,15,6,5f,6,6,23f,25f,1.312f,1.59f,2,1,2));
			this.AllData.Add("462",new crossroadstageCell(462,15,6,5f,6,6,23f,25f,1.304f,1.58f,2,1,2));
			this.AllData.Add("463",new crossroadstageCell(463,15,6,5f,6,6,23f,25f,1.296f,1.57f,2,1,2));
			this.AllData.Add("464",new crossroadstageCell(464,15,6,5f,6,6,23f,25f,1.288f,1.56f,2,1,2));
			this.AllData.Add("465",new crossroadstageCell(465,15,6,5f,6,6,23f,25f,1.28f,1.55f,2,1,2));
			this.AllData.Add("466",new crossroadstageCell(466,15,6,5f,6,6,23f,25f,1.272f,1.54f,2,1,2));
			this.AllData.Add("467",new crossroadstageCell(467,15,6,5f,6,6,23f,25f,1.264f,1.53f,2,1,2));
			this.AllData.Add("468",new crossroadstageCell(468,15,6,5f,6,6,23f,25f,1.256f,1.52f,2,1,2));
			this.AllData.Add("469",new crossroadstageCell(469,15,6,5f,6,6,23f,25f,1.248f,1.51f,2,1,2));
			this.AllData.Add("470",new crossroadstageCell(470,15,6,5f,6,6,23f,25f,1.24f,1.5f,2,1,2));
			this.AllData.Add("471",new crossroadstageCell(471,15,6,5f,6,6,23f,25f,1.232f,1.49f,2,1,2));
			this.AllData.Add("472",new crossroadstageCell(472,15,6,5f,6,6,23f,25f,1.224f,1.48f,2,1,2));
			this.AllData.Add("473",new crossroadstageCell(473,15,6,5f,6,6,23f,25f,1.216f,1.47f,2,1,2));
			this.AllData.Add("474",new crossroadstageCell(474,15,6,5f,6,6,23f,25f,1.208f,1.46f,2,1,2));
			this.AllData.Add("475",new crossroadstageCell(475,15,6,5f,6,6,23f,25f,1.2f,1.45f,2,1,2));
			this.AllData.Add("476",new crossroadstageCell(476,15,6,5f,6,6,23f,25f,1.192f,1.44f,2,1,2));
			this.AllData.Add("477",new crossroadstageCell(477,15,6,5f,6,6,23f,25f,1.184f,1.43f,2,1,2));
			this.AllData.Add("478",new crossroadstageCell(478,15,6,5f,6,6,23f,25f,1.176f,1.42f,2,1,2));
			this.AllData.Add("479",new crossroadstageCell(479,15,6,5f,6,6,23f,25f,1.168f,1.41f,2,1,2));
			this.AllData.Add("480",new crossroadstageCell(480,15,6,5f,6,6,23f,25f,1.16f,1.4f,2,1,2));
			this.AllData.Add("481",new crossroadstageCell(481,15,6,5f,6,6,23f,25f,1.152f,1.39f,2,1,2));
			this.AllData.Add("482",new crossroadstageCell(482,15,6,5f,6,6,23f,25f,1.144f,1.38f,2,1,2));
			this.AllData.Add("483",new crossroadstageCell(483,15,6,5f,6,6,23f,25f,1.136f,1.37f,2,1,2));
			this.AllData.Add("484",new crossroadstageCell(484,15,6,5f,6,6,23f,25f,1.128f,1.36f,2,1,2));
			this.AllData.Add("485",new crossroadstageCell(485,15,6,5f,6,6,23f,25f,1.12f,1.35f,2,1,2));
			this.AllData.Add("486",new crossroadstageCell(486,15,6,5f,6,6,23f,25f,1.112f,1.34f,2,1,2));
			this.AllData.Add("487",new crossroadstageCell(487,15,6,5f,6,6,23f,25f,1.104f,1.33f,2,1,2));
			this.AllData.Add("488",new crossroadstageCell(488,15,6,5f,6,6,23f,25f,1.096f,1.32f,2,1,2));
			this.AllData.Add("489",new crossroadstageCell(489,15,6,5f,6,6,23f,25f,1.088f,1.31f,2,1,2));
			this.AllData.Add("490",new crossroadstageCell(490,15,6,5f,6,6,23f,25f,1.08f,1.3f,2,1,2));
			this.AllData.Add("491",new crossroadstageCell(491,15,6,5f,6,6,23f,25f,1.072f,1.29f,2,1,2));
			this.AllData.Add("492",new crossroadstageCell(492,15,6,5f,6,6,23f,25f,1.064f,1.28f,2,1,2));
			this.AllData.Add("493",new crossroadstageCell(493,15,6,5f,6,6,23f,25f,1.056f,1.27f,2,1,2));
			this.AllData.Add("494",new crossroadstageCell(494,15,6,5f,6,6,23f,25f,1.048f,1.26f,2,1,2));
			this.AllData.Add("495",new crossroadstageCell(495,15,6,5f,6,6,23f,25f,1.04f,1.25f,2,1,2));
			this.AllData.Add("496",new crossroadstageCell(496,15,6,5f,6,6,23f,25f,1.032f,1.24f,2,1,2));
			this.AllData.Add("497",new crossroadstageCell(497,15,6,5f,6,6,23f,25f,1.024f,1.23f,2,1,2));
			this.AllData.Add("498",new crossroadstageCell(498,15,6,5f,6,6,23f,25f,1.016f,1.22f,2,1,2));
			this.AllData.Add("499",new crossroadstageCell(499,15,6,5f,6,6,23f,25f,1.008f,1.21f,2,1,2));
			this.AllData.Add("500",new crossroadstageCell(500,15,6,5f,6,6,23f,25f,1f,1.2f,2,1,2));
		}
	}
	public class crossroadstageCell
	{
		///<summary>
		///关卡数
		///<summary>
		public readonly int level;
		///<summary>
		///初始动物数量
		///<summary>
		public readonly int animalnum;
		///<summary>
		///马路数量
		///<summary>
		public readonly int roadnum;
		///<summary>
		///小动物移动速度
		///<summary>
		public readonly float animalspeed;
		///<summary>
		///车道刷量下限
		///<summary>
		public readonly int roadweightmin;
		///<summary>
		///车道刷量上限
		///<summary>
		public readonly int roadweightmax;
		///<summary>
		///行驶速度下限
		///<summary>
		public readonly float speedmin;
		///<summary>
		///行驶速度上限
		///<summary>
		public readonly float speedmax;
		///<summary>
		///刷新间隔下限
		///<summary>
		public readonly float Intervalmin;
		///<summary>
		///刷新间隔上限
		///<summary>
		public readonly float Intervalmax;
		///<summary>
		///货币奖励类型
		///<summary>
		public readonly int rewardtype;
		///<summary>
		///奖励数量
		///<summary>
		public readonly int firstgoldreward;
		///<summary>
		///奖励翻倍
		///<summary>
		public readonly int warddouble;
		public crossroadstageCell(int level,int animalnum,int roadnum,float animalspeed,int roadweightmin,int roadweightmax,float speedmin,float speedmax,float Intervalmin,float Intervalmax,int rewardtype,int firstgoldreward,int warddouble){
			this.level = level;
			this.animalnum = animalnum;
			this.roadnum = roadnum;
			this.animalspeed = animalspeed;
			this.roadweightmin = roadweightmin;
			this.roadweightmax = roadweightmax;
			this.speedmin = speedmin;
			this.speedmax = speedmax;
			this.Intervalmin = Intervalmin;
			this.Intervalmax = Intervalmax;
			this.rewardtype = rewardtype;
			this.firstgoldreward = firstgoldreward;
			this.warddouble = warddouble;
		}
	}
}