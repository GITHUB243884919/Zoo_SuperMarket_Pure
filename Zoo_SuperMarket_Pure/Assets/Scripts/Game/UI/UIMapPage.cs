using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UFrame.MessageCenter;
using DG.Tweening;
using Game;
using Game.MessageCenter;
using UFrame;
using Game.GlobalData;
using System.Linq;
using UFrame.Logger;
using UFrame.MiniGame;

/// <summary>
/// 世界地图界面
/// </summary>
public class UIMapPage : UIPage
{
    class MapPiece
    {
        public Text nameText;
        public Button actButton;
        public Image pieceImage;
        public Transform requiredStar;
        public Image requiredIcon;
        public Text requiredStarText;
        public Image lockedSymbol;
        public Transform tipsBubbleAnchor;
        public Transform tipsBubble;
        public Tweener unbrowseTween;
        public Vector2 lockedSymbolOrigPos;
        public Shadow selectedShadow;
        public Text perfecttext;
    }

    class MapPieceGroup
    {
        public Transform requiredStar;
        public Text requiredStarText;
        public Image lockedSymbol;
        public Dictionary<int, MapPiece> pieceDict = new Dictionary<int, MapPiece>();
    }

    private Button enterSceneButton;
    private Button closeButton;
    private Text coinQuantityText;
    private Text starQuantityText;
    private Text earningsText;
    private Text diamondText;

    private Transform tipsBubbleTmpl;
    private Transform tipsPointerTmpl;
    private Transform coinEffectTmpl;

    private string configPath = "configs/world_map_config";
    private WorldMapConfig mapConfig;
    private int pieceGroupCount = 5;
    private string pieceGroupFmtStr = "MapBg/SceneGroup_{0}";
    private string pieceFmtStr = "Scene_{0}";
    private CanvasGroup pieceInfoHint;
    private Text pieceInfoNameText;
    private Text sceneTipsText;

    private Image[] pieceInfoCrownImages;
    private float pieceInfoHintOrigPosy = 0;
    private Coroutine pieceInfoHintAnimCoroutine;
    private Transform currScenePointer;

    private Dictionary<int, MapPieceGroup> pieceGroupDict = new Dictionary<int, MapPieceGroup>();
    private int selectedPieceId = -1;

    public UIMapPage() : base(UIType.PopUp, UIMode.DoNothing, UICollider.None, UITickedMode.Update)
    {
        uiPath = "uiprefab/UIMap";
    }
    private Image money_1_GoldIcon;

    public override void Awake(GameObject go)
    {
        base.Awake(go);

        GetTransPrefabAllTextShow(this.transform, true);

        mapConfig = Resources.Load<WorldMapConfig>(configPath);
        closeButton = RegistBtnAndClick("MapBg/CloseButton", OnCloseButtonClick);
        enterSceneButton = RegistBtnAndClick("MapBg/GetIntoButton", OnEnterSceneButtonClick);
        pieceInfoHint = RegistCompent<CanvasGroup>("SceneInfo");
        pieceInfoNameText = RegistCompent<Text>("SceneInfo/NameText");
        sceneTipsText = RegistCompent<Text>("SceneInfo/SceneTipsText");

        coinQuantityText = RegistCompent<Text>("MapBg/MoneyGroup/Money_1/Text");
        starQuantityText = RegistCompent<Text>("MapBg/MoneyGroup/Money_2/Text");
        earningsText = RegistCompent<Text>("MapBg/MoneyGroup/Money_1/MText");
        diamondText = RegistCompent<Text>("MapBg/MoneyGroup/Money_3/Text");

        tipsBubbleTmpl = RegistCompent<Transform>("TipsBubble");
        tipsPointerTmpl = RegistCompent<Transform>("TipsPointer");
        coinEffectTmpl = RegistCompent<Transform>("CoinEffect");
        tipsBubbleTmpl.gameObject.SetActive(false);
        tipsPointerTmpl.gameObject.SetActive(false);
        coinEffectTmpl.gameObject.SetActive(false);
        money_1_GoldIcon = RegistCompent<Image>("MapBg/MoneyGroup/Money_1/GoldIcon");
        

        for (int i = 0; i < pieceGroupCount; i ++)
        {
            Transform group = RegistCompent<Transform>(string.Format(pieceGroupFmtStr, i + 1));
            int groupId = ExtraPieceGroupId(group.name);
            pieceGroupDict.Add(groupId, new MapPieceGroup()
            {
                lockedSymbol = group.Find("BigLock").GetComponent<Image>(),
                requiredStar = group.Find("BigLock/StarBg"),
                requiredStarText = group.Find("BigLock/StarBg/StarNum").GetComponent<Text>()
            });

            Dictionary<int, MapPiece> pieceDict = pieceGroupDict[groupId].pieceDict;
            for (int j = 0; j < group.childCount; j ++)
            {
                Transform pieceTrans = group.GetChild(j);

                if (!pieceTrans.name.StartsWith("Scene"))
                    continue;

                int pieceId = ExtraPieceId(pieceTrans.name);
                pieceDict.Add(pieceId, new MapPiece()
                {
                    pieceImage = pieceTrans.GetComponent<Image>(),
                    actButton = pieceTrans.GetComponent<Button>(),
                    nameText = pieceTrans.Find("MapName").GetComponent<Text>(),
                    lockedSymbol = pieceTrans.Find("LockIcon").GetComponent<Image>(),
                    requiredStar = pieceTrans.Find("StarBg"),
                    requiredStarText = pieceTrans.Find("StarBg/StarNum").GetComponent<Text>(),
                    requiredIcon = pieceTrans.Find("StarBg/StarIcon").GetComponent<Image>(),
                    tipsBubbleAnchor = pieceTrans.Find("TipsBubbleNode"),
                    perfecttext =pieceTrans.Find("Perfecttext").GetComponent<Text>(),
                }); ; 
                pieceDict[pieceId].lockedSymbolOrigPos = pieceDict[pieceId].lockedSymbol.transform.localPosition;
                pieceDict[pieceId].pieceImage.alphaHitTestMinimumThreshold = 0.1f;
                pieceDict[pieceId].actButton.onClick.AddListener(() => { OnPieceActButtonClick(pieceId); });
            }
        }

        UpdatePieceInfoDisplay();
        UpdatePieceStatesDisplay();
        //UpdatePieceCoinDisplay();

        pieceInfoHintOrigPosy = pieceInfoHint.transform.localPosition.y;
        pieceInfoHint.gameObject.SetActive(false);
        enterSceneButton.gameObject.SetActive(false);

        
        MessageManager.GetInstance().Regist((int)GameMessageDefine.BroadcastCoinOfPlayerDataMSSC, OnGetBroadcastCoinOfPlayerDataMSSC);
        MessageManager.GetInstance().Regist((int)GameMessageDefine.BroadcastStarOfPlayerData, OnBroadcastStarOfPlayerData);
        MessageManager.GetInstance().Regist((int)GameMessageDefine.BroadcastDiamondOfPlayerData, OnBroadcastDiamondOfPlayerData);
        MessageManager.GetInstance().Regist((int)GameMessageDefine.BroadcastLeaveSceneCoin, OnLeaveSceneCoinChanged);
    }

   

    public override void Active()
    {
        base.Active();

        UpdatePieceInfoDisplay();
        UpdatePieceStatesDisplay();
        SetCurrScenePointer();
        int scenetype = Config.sceneConfig.getInstace().getCell(GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID).moneyid;
        string iconPath = Config.moneyConfig.getInstace().getCell(scenetype).moneyicon;
        money_1_GoldIcon.sprite = ResourceManager.LoadSpriteFromPrefab(iconPath);
        coinQuantityText.text = mapModel.playerCoinQuantity;
        starQuantityText.text = mapModel.playerStarQuantity.ToString();
        earningsText.text = mapModel.playerEarningsQuantity + GetL10NString("Ui_Text_67");
        diamondText.text = mapModel.playerDiamondQuantity.ToString();
    }
    Sprite iconSprite;
    private Config.sceneConfig sceneConfig { get { return Config.sceneConfig.getInstace(); } }

    private WorldMapModel mapModel { get { return WorldMapModel.GetInstance(); } }

    private int ExtraPieceGroupId(string goName)
    {
        return ExtraPieceId(goName);
    }

    private int ExtraPieceId(string goName)
    {
        int splitIdx = goName.LastIndexOf("_");
        string idStr = goName.Substring(splitIdx + 1);
        int ret = -1;
        int.TryParse(idStr, out ret);
        return ret;
    }

    private MapPiece GetMapPiece(int pieceId, out int ownedGroup)
    {
        ownedGroup = -1;
        foreach (var group in pieceGroupDict.Keys)
        {
            foreach (var id in pieceGroupDict[group].pieceDict.Keys)
            {
                if (id == pieceId)
                {
                    ownedGroup = group;
                    return pieceGroupDict[group].pieceDict[id];
                } 
            }
        }
        return null;
    }

    private void SetCurrScenePointer()
    {
        int ownedGroup;
        MapPiece currPiece = GetMapPiece(mapModel.currSceneId, out ownedGroup);
        if (currScenePointer == null)
        {
            GameObject go = Object.Instantiate(tipsPointerTmpl.gameObject);
            go.SetActive(true);
            go.transform.SetParent(transform);
            currScenePointer = go.transform;
        }
        currScenePointer.localScale = Vector3.one;
        currScenePointer.localRotation = Quaternion.identity;
        currScenePointer.position = currPiece.tipsBubbleAnchor.position;
        currScenePointer.localPosition = currScenePointer.localPosition + (Vector3)mapConfig.currPointerOffset;
    }

    private IEnumerator FinishShowPieceInfoHint(int pieceId)
    {
        Config.sceneCell sceneCell = sceneConfig.getCell(pieceId);
        if (sceneCell.israwopen < 1)
        {
            pieceInfoNameText.text = GetL10NString("Ui_Text_113");
        }
        else
        {
            pieceInfoNameText.text = string.Format(GetL10NString(sceneCell.scenename), sceneCell.sceneorder);

        }
        sceneTipsText.text = GoToSceneTipsTextString(pieceId);

        pieceInfoHint.gameObject.SetActive(true);
        pieceInfoHint.transform.localPosition = new Vector3(
            pieceInfoHint.transform.localPosition.x,
            pieceInfoHintOrigPosy,
            pieceInfoHint.transform.localPosition.z);
        pieceInfoHint.alpha = 1;
        pieceInfoHint.transform.DOLocalMoveY(pieceInfoHintOrigPosy + 60, 0.3f);
        yield return new WaitForSeconds(2f);
        pieceInfoHint.DOFade(0, 0.29f);
        pieceInfoHint.transform.DOLocalMoveY(pieceInfoHintOrigPosy + 120, 0.3f).OnComplete(() =>
        {
            pieceInfoHint.gameObject.SetActive(false);
        });
    }

    private void OnSceneUnlockPageClose(int detail)
    {
        ScenePlayerDataMSS.SceneStateMSS sceneState = mapModel.GetSceneState(selectedPieceId);
        bool unlocked = sceneState != null ? sceneState.unlocked > 0 : false;
        bool broswed = sceneState != null ? sceneState.browsed > 0 : false;
        enterSceneButton.gameObject.SetActive(unlocked && broswed && mapModel.currSceneId != selectedPieceId);

        OnEnterSceneButtonClick(null);
    }

    private void OnPieceActButtonClick(int pieceId)
    {
        int oldSelectedId = selectedPieceId;
        selectedPieceId = pieceId;

        int ownedGroup;
        MapPiece oldSelectedPiece = GetMapPiece(oldSelectedId, out ownedGroup);
        if (oldSelectedPiece != null)
        {
            if (oldSelectedPiece.selectedShadow != null)
            {
                oldSelectedPiece.selectedShadow.enabled = false;
                Object.Destroy(oldSelectedPiece.selectedShadow);
                oldSelectedPiece.selectedShadow = null;
            }
            oldSelectedPiece.pieceImage.transform.localScale = Vector3.one;
        }

        MapPiece piece = GetMapPiece(pieceId, out ownedGroup);
        if (piece.selectedShadow == null)
        {
            piece.pieceImage.transform.SetAsLastSibling();
            pieceGroupDict[ownedGroup].lockedSymbol.transform.SetAsLastSibling();
            piece.selectedShadow = piece.pieceImage.gameObject.AddComponent<Shadow>();
            piece.selectedShadow.effectColor = mapConfig.selectedShadowColor;
            piece.selectedShadow.effectDistance = mapConfig.selectedShadowDist;
            piece.pieceImage.transform.DOScale(mapConfig.selectedScale, mapConfig.selectedAnimDuration).SetEase(mapConfig.selectedAnimCurve);
        }

        SetSceneStateData(pieceId);
    }

    private void SetSceneStateData(int pieceId)
    {
        ScenePlayerDataMSS.SceneStateMSS sceneState = mapModel.GetSceneState(pieceId);
        bool unlocked = sceneState != null ? sceneState.unlocked > 0 : false;
        bool broswed = sceneState != null ? sceneState.browsed > 0 : false;
        if (unlocked && !broswed)
        {
            //mapModel.BrowseScene(pieceId);
            //UpdatePieceStatesDisplay();
            //var mainPage = PageMgr.GetPage<UIMainPage>();
            //if (mainPage != null)
            //    mainPage.UpdateDisplayForSceneStates();
            PageMgr.ShowPage<UISceneUnlockPage>(pieceId);
            PageMgr.GetPage<UISceneUnlockPage>().onClose.AddListener(OnSceneUnlockPageClose);
            
        }
        else
        {
            enterSceneButton.gameObject.SetActive(false);

            //pieceInfoHint.gameObject.SetActive(false);

            if (pieceInfoHintAnimCoroutine != null)
            {
                GameManager.GetInstance().StopCoroutine(pieceInfoHintAnimCoroutine);
                pieceInfoHintAnimCoroutine = null;
            }
            var playerData = GlobalDataManager.GetInstance().playerData;
            int idx = PlayerData.GetcurrSceneIDByStar(playerData);
            ///回退    老场景不可点击
            //if (sceneState != null && sceneState.sceneId == idx)
            //{
            //    enterSceneButton.gameObject.SetActive(unlocked && broswed && mapModel.currSceneId != selectedPieceId);
            //}
            enterSceneButton.gameObject.SetActive(unlocked && broswed && mapModel.currSceneId != selectedPieceId);

            pieceInfoHintAnimCoroutine = GameManager.GetInstance().StartCoroutine(FinishShowPieceInfoHint(pieceId));


        }
    }


    private void OnCloseButtonClick(string str)
    {

        float timeCount = 0.1f;
        DOTween.To(() => timeCount, a => timeCount = a, 0.1f, 0.1f).OnComplete(new TweenCallback(delegate
        {
            PageMgr.ClosePage(this);
        }));




    }

    private void OnEnterSceneButtonClick(string str)
    {
        ScenePlayerDataMSS.SceneStateMSS sceneState = mapModel.GetSceneState(selectedPieceId);
        bool unlocked = sceneState != null ? sceneState.unlocked > 0 : false;
        if (unlocked && mapModel.currSceneId != selectedPieceId)
        {
            enterSceneButton.gameObject.SetActive(false);
            PageMgr.ClosePage(this);
            ZooGameLoader.GetInstance().ChangeScene(selectedPieceId);
        }   
    }

    private void UpdatePieceInfoDisplay()
    {
        List<int> allSceneType = mapModel.GetAllSceneType();
        Dictionary<int, Config.sceneCell> sceneCellOfType = new Dictionary<int, Config.sceneCell>();
        foreach(int t in allSceneType)
        {
            if (pieceGroupDict.ContainsKey(t))
            {
                pieceGroupDict[t].lockedSymbol.gameObject.SetActive(true);
                sceneCellOfType.Clear();
                mapModel.FilterSceneCell(t, sceneCellOfType);
                pieceGroupDict[t].requiredStarText.text = sceneCellOfType.First().Value.openstar.ToString();

                Dictionary<int, MapPiece> pieceDict = pieceGroupDict[t].pieceDict;
                foreach (var sceneId in sceneCellOfType.Keys)
                {
                    if (pieceDict.ContainsKey(sceneId))
                    {
                        pieceDict[sceneId].requiredStarText.text = sceneCellOfType[sceneId].openstar.ToString();
                        pieceDict[sceneId].nameText.text = string.Format(GetL10NString(sceneCellOfType[sceneId].scenename), sceneCellOfType[sceneId].sceneorder);
                        //if (sceneId == GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID)
                        //{
                        //    pieceDict[sceneId].requiredStarText.text = sceneCellOfType[sceneId].openstar.ToString();
                        //    pieceDict[sceneId].nameText.text = string.Format(GetL10NString(sceneCellOfType[sceneId].scenename), sceneCellOfType[sceneId].sceneorder);
                        //}
                        //else
                        //{
                        //    pieceDict[sceneId].requiredStarText.gameObject.SetActive(false);
                        //    pieceDict[sceneId].nameText.text = string.Format(GetL10NString(sceneCellOfType[sceneId].scenename), sceneCellOfType[sceneId].sceneorder);
                        //    pieceDict[sceneId].perfecttext.gameObject.SetActive(true);
                        //    pieceDict[sceneId].perfecttext.text = GetL10NString("Ui_Text_120");
                        //}

                    }  
                }
            }
        }
    }
    /// <summary>
    /// 返回sceneTipsText需要的字符
    /// </summary>
    /// <returns></returns>
    private string GoToSceneTipsTextString(int sceneId)
    {
        var sceneCell = Config.sceneConfig.getInstace().getCell(sceneId);
        string str = sceneCell.scenetips;
        int[] array = sceneCell.sceneanimal;
        //str = string.Format(GetL10NString(str),
        //    GetL10NString(Config.animalupConfig.getInstace().getCell(array[0]).nametranslate),
        //    GetL10NString(Config.animalupConfig.getInstace().getCell(array[1]).nametranslate),
        //    GetL10NString(Config.animalupConfig.getInstace().getCell(array[2]).nametranslate), 
        //    GetL10NString(Config.animalupConfig.getInstace().getCell(array[3]).nametranslate),
        //    GetL10NString(Config.animalupConfig.getInstace().getCell(array[4]).nametranslate));

        //LogWarp.LogErrorFormat("AAAAAAAAAAAAAAAAAAAA   {0}",str);

        return GetL10NString(str);
    }

    private void UpdatePieceStateDisplayOfType(int sceneType, bool hasOneUnlocked, Dictionary<int, Config.sceneCell> sceneCellOfType)
    {
        if (!pieceGroupDict.ContainsKey(sceneType))
            return;

        Dictionary<int, MapPiece> pieceDict = pieceGroupDict[sceneType].pieceDict;
        foreach (int k in sceneCellOfType.Keys)
        {
            if (!pieceDict.ContainsKey(k))
                continue;

            ScenePlayerDataMSS.SceneStateMSS sceneState = mapModel.GetSceneState(k);
            bool unlocked = sceneState != null ? sceneState.unlocked > 0 : false;
            bool broswed = sceneState != null ? sceneState.browsed > 0 : false;

            pieceDict[k].requiredStar.gameObject.SetActive(hasOneUnlocked);
            if (unlocked)
            {
                if (broswed)
                {
                    if (pieceDict[k].unbrowseTween != null)
                    {
                        pieceDict[k].unbrowseTween.Kill();
                        pieceDict[k].unbrowseTween = null;
                    }
                    if (pieceDict[k].tipsBubble != null)
                    {
                        pieceDict[k].tipsBubble.gameObject.SetActive(false);
                        Object.Destroy(pieceDict[k].tipsBubble.gameObject);
                    }
                    pieceDict[k].lockedSymbol.gameObject.SetActive(false);
                }
                else
                {
                    pieceDict[k].lockedSymbol.gameObject.SetActive(true);
                    if (pieceDict[k].unbrowseTween == null)
                    {
                        pieceDict[k].unbrowseTween = pieceDict[k].lockedSymbol.transform.DOLocalMoveX(
                            pieceDict[k].lockedSymbolOrigPos.x + mapConfig.unbrowseSymbolAnimRange * 0.5f, mapConfig.unbrowseSymbolAnimDuration)
                            .SetEase(mapConfig.unbrowseSymbolAnimCurve).SetLoops(int.MaxValue);
                    }
                    if (pieceDict[k].tipsBubble == null)
                    {
                        GameObject go = Object.Instantiate(tipsBubbleTmpl.gameObject);
                        go.SetActive(true);
                        go.transform.SetParent(/*pieceDict[k].pieceImage.transform*/transform);
                        go.transform.localScale = Vector3.one;
                        go.transform.localRotation = Quaternion.identity;
                        go.transform.position = pieceDict[k].tipsBubbleAnchor.position;
                        go.transform.localPosition = go.transform.localPosition + (Vector3)mapConfig.unbrowseBubbleOffset;
                        pieceDict[k].tipsBubble = go.transform;
                    }
                }
                pieceDict[k].pieceImage.material = null;
            }
            else
            {
                pieceDict[k].lockedSymbol.gameObject.SetActive(hasOneUnlocked);
                pieceDict[k].pieceImage.material = mapConfig.pieceDarkMaterial;
            }

            if (unlocked && broswed)
            {
                pieceDict[k].nameText.gameObject.SetActive(true);
                int scenetype = Config.sceneConfig.getInstace().getCell(sceneState.sceneId).moneyid;
                string iconPath = Config.moneyConfig.getInstace().getCell(scenetype).moneyicon;

                int idx = PlayerData.GetcurrSceneIDByStar(GlobalDataManager.GetInstance().playerData);
                // 回退  修改完美经营
                //if (sceneState.sceneId == GlobalDataManager.GetInstance().playerData.playerZoo.currSceneID || sceneState.sceneId == idx)

                if (sceneState.sceneId== idx)
                {
                    pieceDict[k].requiredStarText.text = MinerBigInt.ToDisplay(mapModel.GetSceneEarningsPerMinute(k)) + GetL10NString("Ui_Text_67");
                    pieceDict[k].requiredIcon.sprite = ResourceManager.LoadSpriteFromPrefab(iconPath);
                    pieceDict[k].requiredIcon.SetNativeSize();
                    pieceDict[k].requiredStarText.gameObject.SetActive(true);
                    pieceDict[k].requiredIcon.gameObject.SetActive(true);
                    pieceDict[k].perfecttext.gameObject.SetActive(false);


                }
                else
                {
                    pieceDict[k].requiredStarText.gameObject.SetActive(false);
                    pieceDict[k].requiredIcon.gameObject.SetActive(false);
                    pieceDict[k].perfecttext.gameObject.SetActive(true);
                    //pieceDict[k].perfecttext.text = GetL10NString("Ui_Text_120");

                }

            }
            else
            {
                pieceDict[k].nameText.gameObject.SetActive(false);
                pieceDict[k].requiredIcon.sprite = mapConfig.starSprite;
                pieceDict[k].requiredIcon.SetNativeSize();
                pieceDict[k].requiredStarText.text = sceneCellOfType[k].openstar.ToString();
            }
        }
    }

    public void UpdatePieceStatesDisplay()
    {
        List<int> allSceneType = mapModel.GetAllSceneType();
        Dictionary<int, Config.sceneCell> sceneCellOfType = new Dictionary<int, Config.sceneCell>();
        foreach (int t in allSceneType)
        {
            if (pieceGroupDict.ContainsKey(t))
            {
                bool hasUnlocked = mapModel.HasUnlockedSceneByType(t);
                pieceGroupDict[t].lockedSymbol.gameObject.SetActive(!hasUnlocked);
                pieceGroupDict[t].requiredStar.gameObject.SetActive(!hasUnlocked);

                sceneCellOfType.Clear();
                mapModel.FilterSceneCell(t, sceneCellOfType);
                foreach (var sceneId in sceneCellOfType.Keys)
                    UpdatePieceStateDisplayOfType(t, hasUnlocked, sceneCellOfType);
            }
        }
    }

    private void UpdatePieceCoinDisplay()
    {
        List<int> allSceneType = mapModel.GetAllSceneType();
        Dictionary<int, Config.sceneCell> sceneCellOfType = new Dictionary<int, Config.sceneCell>();
        foreach (int t in allSceneType)
        {
            if (pieceGroupDict.ContainsKey(t))
            {
                sceneCellOfType.Clear();
                mapModel.FilterSceneCell(t, sceneCellOfType);
                
                Dictionary<int, MapPiece> pieceDict = pieceGroupDict[t].pieceDict;
                foreach (var sceneId in sceneCellOfType.Keys)
                {
                    if (pieceDict.ContainsKey(sceneId))
                    {
                        ScenePlayerDataMSS.SceneStateMSS sceneState = mapModel.GetSceneState(sceneId);
                        bool unlocked = sceneState != null ? sceneState.unlocked > 0 : false;
                        bool broswed = sceneState != null ? sceneState.browsed > 0 : false;
                        if (unlocked && broswed)
                            pieceDict[sceneId].requiredStarText.text = MinerBigInt.ToDisplay(mapModel.GetSceneEarningsPerMinute(sceneId)) + GetL10NString("Ui_Text_67"); 
                    }
                }
            }
        }
    }

    private IEnumerator WaitDestroyObject(float waitSeconds, GameObject go)
    {
        yield return new WaitForSeconds(waitSeconds);
        go.SetActive(false);
        Object.Destroy(go);
    }

    private void OnLeaveSceneCoinChanged_old(Message message)
    {
        BroadcastLeaveSceneCoin msg = message as BroadcastLeaveSceneCoin;
        MapPiece piece;
        int ownedGroup = -1;
        foreach (var k in msg.sceneCoinDict.Keys)
        {
            piece = GetMapPiece(k, out ownedGroup);
            GameObject effGo = GameObject.Instantiate(coinEffectTmpl.gameObject);
            effGo.gameObject.SetActive(true);
            effGo.transform.SetParent(transform);
            effGo.transform.localScale = Vector3.one;
            effGo.transform.position = piece.tipsBubbleAnchor.position;
            var p = effGo.GetComponentInChildren<ParticleSystem>();
            p.time = 0;
            p.Play(true);
            Text quantityText = effGo.transform.Find("UIFlutterText/Text").GetComponent<Text>();
            quantityText.text = MinerBigInt.ToDisplay(msg.sceneCoinDict[k]);
            Animator txtAnimator = quantityText.GetComponent<Animator>();
            txtAnimator.enabled = true;
            txtAnimator.Play("UINumber", 0, 0);

            GameManager.GetInstance().StartCoroutine(WaitDestroyObject(1, effGo));
        }

        //UpdatePieceCoinDisplay();
    }

    private void OnLeaveSceneCoinChanged(Message message)
    {
        BroadcastLeaveSceneCoin msg = message as BroadcastLeaveSceneCoin;
        MapPiece piece;
        int ownedGroup = -1;
        foreach (var kv in msg.sceneCoinDict)
        {
            piece = GetMapPiece(kv.Key, out ownedGroup);
            //GameObject effGo = GameObject.Instantiate(coinEffectTmpl.gameObject);
            var cellScene = Config.sceneConfig.getInstace().getCell(kv.Key);
            var cellMoney = Config.moneyConfig.getInstace().getCell(cellScene.moneyid);
            //transform.Find(cellMoney.mapmoneyeffect);
            GameObject effGo = GameObject.Instantiate(transform.Find(cellMoney.mapmoneyeffect).gameObject);
            effGo.gameObject.SetActive(true);
            effGo.transform.SetParent(transform);
            effGo.transform.localScale = Vector3.one;
            effGo.transform.position = piece.tipsBubbleAnchor.position;
            var p = effGo.GetComponentInChildren<ParticleSystem>();
            p.time = 0;
            p.Play(true);
            Text quantityText = effGo.transform.Find("UIFlutterText/Text").GetComponent<Text>();
            quantityText.text = MinerBigInt.ToDisplay(msg.sceneCoinDict[kv.Key]);
            Animator txtAnimator = quantityText.GetComponent<Animator>();
            txtAnimator.enabled = true;
            txtAnimator.Play("UINumber", 0, 0);

            GameManager.GetInstance().StartCoroutine(WaitDestroyObject(1, effGo));
        }

        //UpdatePieceCoinDisplay();
    }

    private void OnGetBroadcastCoinOfPlayerDataMSSC(Message message)
    {
        coinQuantityText.text = mapModel.playerCoinQuantity;
    }

    private void OnBroadcastStarOfPlayerData(Message message)
    {
        starQuantityText.text = mapModel.playerStarQuantity.ToString();
    }
    private void OnBroadcastDiamondOfPlayerData(Message obj)
    {
        diamondText.text = mapModel.playerDiamondQuantity.ToString();
    }
    public override void Tick(int deltaTimeMS)
    {
        SetAndroidQuit();
    }

    [System.Diagnostics.Conditional("UNITY_ANDROID")]
    protected void SetAndroidQuit()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape))
#else
        if (Application.platform == RuntimePlatform.Android && Input.GetKeyDown(KeyCode.Escape)) 
#endif
        {
            PageMgr.ClosePage(this);
        }
    }
}