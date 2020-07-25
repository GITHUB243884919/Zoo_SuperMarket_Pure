﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UFrame.Common;
using UFrame.Logger;
using UFrame;
using Game.MessageCenter;
using Game.MiniGame;
using Game.GlobalData;
using UFrame.MiniGame;

namespace Game
{
    public class ZooGameLoader : Singleton<ZooGameLoader>, ISingleton
    {
        System.Action callbacks;

        public FSMGameLoad fsmLoad;
        public void Init()
        {
            fsmLoad = new FSMGameLoad();
            fsmLoad.AddState(new StateServerForGame((int)GameLoaderState.Server, fsmLoad));
            fsmLoad.AddState(new StateLoadOrgSceneForGame((int)GameLoaderState.LoadOrgScene, fsmLoad));
            fsmLoad.AddState(new StateLoadPartSceneForGame((int)GameLoaderState.LoadPartScenes, fsmLoad));
            fsmLoad.AddState(new StateLoadAnimalInLittleZooForGame((int)GameLoaderState.LoadAnimalInLittleZoo, fsmLoad));

            fsmLoad.SetDefaultState((int)GameLoaderState.Server);
        }

        public void Tick(int deltaTimeMs)
        {
            if (fsmLoad == null)
            {
                return;
            }
            fsmLoad.Tick(deltaTimeMs);
        }

        /// <summary>
        /// 加载动物游戏
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="progress"></param>
        public void Load(System.Action firstCallback, System.Action lastCallback, System.Action<float> progress = null)
        {
            LogWarp.LogFormat("ZooGameLoader.Load");
            switch (LoadingMgr.Inst.runTimeLoaderType)
            {
                case RunTimeLoaderType.Editor:
                    callbacks += firstCallback;
                    callbacks += LoadPartSceneForEditor;
                    string sceneName = Config.sceneConfig.getInstace().getCell(LoadingMgr.Inst.editor_SceneID).resourceid;
                    SceneMgr.Inst.LoadSceneAsync(sceneName, callbacks, progress);
                    break;
                case RunTimeLoaderType.Game:
                    GameSoundManager.GetInstance().PlayBGMusicAsync(Config.globalConfig.getInstace().ZooSceneBGM);
                    GlobalDataManager.GetInstance().isLoadingScene = true;
                    fsmLoad.Run();
                    break;
                default:
                    string e = string.Format("runTimeLoaderType 类型错误{0}", LoadingMgr.Inst.runTimeLoaderType);
                    throw new System.Exception(e);
            }
        }

        /// <summary>
        /// 编辑器模式下分块加载
        /// </summary>
        protected void LoadPartSceneForEditor()
        {            
            LogWarp.LogFormat("LoadPartSceneForEditor");

            int sceneID = LoadingMgr.Inst.editor_SceneID;
            //加载地块
            var littleZooRoot = GameObject.Find("LittleZoo").transform;

            Config.resourceCell cellRes;
            int idx = 0;
            float offset = Config.globalConfig.getInstace().ZooPartResLen;
            float extendOffset = 0;
            Config.groupCell preCell = null;
            Config.groupCell lastCell = null;
            foreach (var kv in Config.groupConfig.getInstace().AllData)
            {
                if (kv.Value.scene != sceneID)
                {
                    continue;
                }

                var cellGroup = kv.Value;
                //加载动物栏
                for (int i = 0; i < cellGroup.startid.Length; i++)
                {
                    //编辑器加载的都是0级的动物栏prefab
                    LittleZooModule.LoadLittleZoo(sceneID, cellGroup.startid[i], 0, littleZooRoot);
                }

                //加载地块
                if (cellGroup.zoopartresID > 0)
                {
                    cellRes = Config.resourceConfig.getInstace().getCell(cellGroup.zoopartresID);
                    var goPart = ResourceManager.GetInstance().LoadGameObject(cellRes.prefabpath);
                    //goPart.transform.position = new Vector3(goPart.transform.position.x - idx * offset, 0, 0);
                    if (preCell != null)
                    {
                        extendOffset += preCell.groundsize;
                    }
                    //goPart.transform.position = new Vector3(goPart.transform.position.x - extendOffset, 0, 0);
                    goPart.transform.position += GlobalDataManager.GetInstance().SceneForward * extendOffset;
                    goPart.name = string.Format("Group_{0}", cellGroup.zoopartresID);
                    ++idx;
                    preCell = cellGroup;
                    lastCell = cellGroup;
                    if (LoadingMgr.Inst.ExtendLoadGroupNum != Const.Invalid_Int && 
                        LoadingMgr.Inst.ExtendLoadGroupNum == idx) 
                    {
                        break;
                    }
                }
            }

            if (lastCell != null)
            {
                extendOffset += lastCell.groundsize;
            }

            LittleZooModule.LoadExitGate(sceneID, idx, extendOffset);
        }

        /// <summary>
        /// 卸载动物园游戏
        /// </summary>
        public void UnLoad()
        {
            this.callbacks = null;
            ////关闭主界面
            //PageMgr.ClosePage();

            CloseAllPage();

            //停止游戏
            GameModuleManager.GetInstance().Stop();
            //卸载module
            GameManager.GetInstance().UnLoadModule();
            //GameManager.GetInstance().RemoveAllTickPage();

            //卸载加载出来的实体
            EntityManager.GetInstance().Release();

            //卸载各pool
            PoolManager.GetInstance().Release();

            ResourceManager.GetInstance().Release();

            //全局数据释放
            GlobalDataManager.GetInstance().Release();
            //卸载场景

            Resources.UnloadUnusedAssets();
            System.GC.Collect(); 
        }
        private void CloseAllPage()
        {
            var allPages = PageMgr.allPages;
            foreach (var item in allPages)
            {
                if (item.Value.name != "UIMainPage" )
                {
                    PageMgr.ClosePage(item.Value);
                }
            }
        }

        public void ChangeScene(int sceneID, bool isCheckCurrSceneID = true)
        {
            int currSceneID = GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID;
            if (sceneID == currSceneID && isCheckCurrSceneID)
            {
                return;
            }
            ZooGameLoader.GetInstance().UnLoad();
            OpenLoadingPage(sceneID);
            GameSoundManager.GetInstance().PlayBGMusicAsync(Config.globalConfig.getInstace().ZooSceneBGM);
            GlobalDataManager.GetInstance().isLoginLoadScene = false;
            GlobalDataManager.GetInstance().isLoadingScene = true;
            ZooGameLoader.GetInstance().fsmLoad.GotoState((int)GameLoaderState.LoadOrgScene);
        }

        public void BackFromCrossRoad()
        {
            ChangeScene(GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID, false);
        }

        public void OpenLoadingPage(int sceneID)
        {
            PageMgr.ShowPage<UILoading>();
            fsmLoad.sceneID = sceneID;
        }
    }
}
