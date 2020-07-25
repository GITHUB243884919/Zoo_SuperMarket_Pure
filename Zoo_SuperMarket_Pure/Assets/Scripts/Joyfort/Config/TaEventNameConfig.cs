using UnityEngine;
using System;
using System.Security;
using System.Collections.Generic;
namespace Config
{
	public enum TaEventNameEnumConfig
	{
		///<summary>
		///注册日志
		///<summary>
		register=0,
 		///<summary>
		///启动游戏
		///<summary>
		gamestart=1,
 		///<summary>
		///进入引导
		///<summary>
		guide_start=2,
 		///<summary>
		///引导过程
		///<summary>
		guide_process=3,
 		///<summary>
		///结束引导
		///<summary>
		guide_finish=4,
 		///<summary>
		///停车场升级
		///<summary>
		parking_upgrade=5,
 		///<summary>
		///售票口升级
		///<summary>
		ticket_upgrade=6,
 		///<summary>
		///动物栏升级
		///<summary>
		build_upgrade=7,
 		///<summary>
		///动物培养
		///<summary>
		animal_upgrade=8,
 		///<summary>
		///登陆游戏时星级数量
		///<summary>
		start_star=9,
 		///<summary>
		///发生改变时星级数量
		///<summary>
		level_star=10,
 		///<summary>
		///开启场景
		///<summary>
		open_scene=11,
 		///<summary>
		///完成激励视频
		///<summary>
		video_finish=12,
 		///<summary>
		///完成任务
		///<summary>
		mission_finish=13,
 	}
}