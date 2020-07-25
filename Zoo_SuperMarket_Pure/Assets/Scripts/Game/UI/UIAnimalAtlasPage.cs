using DG.Tweening;
using Game;
using Game.GlobalData;
using Game.MessageCenter;
using UFrame.Logger;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UFrame;
using UFrame.MessageCenter;
using UFrame.MiniGame;
using UnityEngine;
using UnityEngine.UI;

public class UIAnimalAtlasPage : UIPage
{
    private Text earningsText;
    private Text goldText;
    private Text starText;
    private Text diamondText;


    private Image Money_1_GoldIcon;
    private Image Money_2_GoldIcon;

    Text percentageText;
    Text valueText;
    Slider slider;
    /// <summary>
    /// 列表对象
    /// </summary>
    Transform tableView;
    Button closeButton;
    PlayerData playerData;
    BigInteger incomeCoinMS;
    List<LittleZooModuleDataMSS> littleZooModuleDatas;
    /// <summary>
    /// 玩家的动物数据
    /// </summary>
    PlayerAnimal_MSS playerAnimal;
    public UIAnimalAtlasPage() : base(UIType.Fixed, UIMode.DoNothing, UICollider.None)
    {
        uiPath = "uiprefab/UICollect";
    }

    public override void Awake(GameObject go)
    {
        base.Awake(go);
        GetTransPrefabAllTextShow(this.transform);
        MessageManager.GetInstance().Regist((int)GameMessageDefine.GetAnimalAtlasDataMessage, this.OnGetSingleAnimalAtlasData);
        MessageManager.GetInstance().Regist((int)GameMessageDefine.BroadcastCoinOfPlayerDataMSSC, this.OnGetBroadcastCoinOfPlayerDataMSSC);

        playerData = GlobalDataManager.GetInstance().playerData;

        RegistAllCompent();

    }

    private void OnGetBroadcastCoinOfPlayerDataMSSC(Message obj)
    {
        goldText.text = playerData.playerZoo.playerCoin.GetCoinByScene(playerData.playerZoo.currSceneID).coinShow;
    }

    private void OnGetSingleAnimalAtlasData(Message obj)
    {
        var msg = obj as MessageInt;
        animalAtlasData = GlobalDataManager.GetInstance().logicAnimalAtlasData.animalAtlasData;

        var animalupCell = Config.animalupConfig.getInstace().getCell(msg.val);
        int a = animalupCell.bigtype;
        int b = animalupCell.smalltype;

        SetOnlyAnimalaAtlas(a,b);
    }
    int[,] animalAtlasData;
    /// <summary>
    /// 内部组件的查找
    /// </summary>
    private void RegistAllCompent()
    {

        tableView = RegistCompent<Transform>("Parameter/ScorllView/TableView");
        closeButton = RegistBtnAndClick("CloseButton", OnClickCloseButton);
        earningsText = RegistCompent<Text>("MoneyGroup/Money_1/MText");
        goldText = RegistCompent<Text>("MoneyGroup/Money_1/Text");
        starText = RegistCompent<Text>("MoneyGroup/Money_2/Text");
        diamondText = RegistCompent<Text>("MoneyGroup/Money_3/Text");
        percentageText = RegistCompent<Text>("CollectInfo/InfoBg/PercentageText");
        valueText = RegistCompent<Text>("CollectInfo/InfoBg/ValueText");
        slider = RegistCompent<Slider>("CollectInfo/InfoBg/Slider");

        Money_1_GoldIcon = RegistCompent<Image>("MoneyGroup/Money_1/GoldIcon");
        //Money_2_GoldIcon = RegistCompent<Image>("MoneyGroup/Money_2/GoldIcon");
        foreach (Transform child in tableView)
        {
            GameObject.Destroy(child.gameObject);
        }
        littleZooIDs = GetSenceLittleZooIDs();
        SetCorrectShowImage();
        DynamicDrawTableView();
    }
    private void SetCorrectShowImage()
    {
        int scenetype = Config.sceneConfig.getInstace().getCell(playerData.playerZoo.currSceneID).moneyid;
        string iconPath = Config.moneyConfig.getInstace().getCell(scenetype).moneyicon;
        Money_1_GoldIcon.sprite = ResourceManager.LoadSpriteFromPrefab(iconPath);
        //Money_2_GoldIcon.sprite = ResourceManager.LoadSpriteFromPrefab(iconPath);
    }
    /// <summary>
    /// 设置单一的图鉴状态
    /// </summary>
    /// <param name="a">行</param>
    /// <param name="b">列</param>
    private void SetOnlyAnimalaAtlas(int a,int b)
    {
        int idx_a = 0;
        foreach (var item in Config.animalatlasConfig.getInstace().AllData)
        {
            if (item.Value.bigtype == a)
            {
                idx_a = int.Parse(item.Key);
            }
        }
        //LogWarp.LogErrorFormat("测试：   {0}对应的应该是  {1}   B={2}",a,idx_a,b);
        // tableView
        for (int i = 0; i < tableView.childCount; i++)
        {
            if (i== idx_a-1)
            {
                Transform animalCellGB = tableView.GetChild(i).Find("AnimalKindView/AnimalCell");
                var smalltypesort = Config.animalatlasConfig.getInstace().getCell(idx_a).smalltypesort;
                for (int j = 0; j < animalCellGB.childCount; j++)
                {
                    if (smalltypesort[j] == b)
                    {
                        var animalAtlasGB = animalCellGB.GetChild(j);
                        Transform lockIcon = animalAtlasGB.Find("LockIcon");
                        lockIcon.gameObject.SetActive(false);
                        Image collectIcon = animalAtlasGB.Find("CollectIcon").GetComponent<Image>();
                        collectIcon.material = null;
                        animalAtlasGB.GetComponent<Button>().enabled = true;
                        //LogWarp.LogErrorFormat("测试：    是否能重下标找到对应对象并修改   {0} {1}", a,b);
                    }
                }
            }
        }
    }

    private void OnClickCloseButton(string obj)
    {
        float timeCount = 0.1f;
        DOTween.To(() => timeCount, a => timeCount = a, 0.1f, 0.1f).OnComplete(new TweenCallback(Hide));
    }

    /// <summary>
    /// 初始化属性数值
    /// </summary>
    private void InitAnimalupCellData(int a,int b,out Config.animalupCell animalupCell, out int animalID)
    {
        animalupCell = null;
        animalID = 0;
        //LogWarp.LogErrorFormat("测试：  {0}     {1}", a, b);
        var animalatlasCell = Config.animalatlasConfig.getInstace().getCell(a+1);
        int idx1 = animalatlasCell.bigtype;
        int idx2 = animalatlasCell.smalltypesort[b];
        var animalupAllData = Config.animalupConfig.getInstace().AllData;
        foreach (var item in animalupAllData)
        {
            if (item.Value.bigtype == idx1 && item.Value.smalltype == idx2)
            {
                animalID = int.Parse(item.Key);
                animalupCell = item.Value;
                return;
            }
        }
    }

    /// <summary>
    /// 控件显示赋值
    /// </summary>
    private void InitCompent()
    {
        playerAnimal = GlobalDataManager.GetInstance().playerData.GetPlayerAnimalData();
        playerData = GlobalDataManager.GetInstance().playerData;
        incomeCoinMS = PlayerDataModule.CurrScenePerMinCoin(true);
        goldText.text = playerData.playerZoo.playerCoin.GetCoinByScene(playerData.playerZoo.currSceneID).coinShow;
        starText.text = playerData.playerZoo.star.ToString();
        diamondText.text = playerData.playerZoo.diamond.ToString();
        earningsText.text = MinerBigInt.ToDisplay(incomeCoinMS) + GetL10NString("Ui_Text_67");  
        littleZooModuleDatas = playerData.playerZoo.littleZooModuleDatasMSS;
        int animalAtlasNambe = GlobalDataManager.GetInstance().playerData.playerZoo.animalMSS15.animalProps.Count;
        float allNumber = Config.animalatlasConfig.getInstace().RowNum * 5f;
        percentageText.text = (animalAtlasNambe * 100/ allNumber).ToString("f2") + "%";
        valueText.text = string.Format(GetL10NString("Ui_Text_60"), animalAtlasNambe, allNumber);
        slider.value = (animalAtlasNambe / allNumber);

        //UI绘制
    }
    public override void Active()
    {
        base.Active();
        RegistAllCompent();
        InitCompent();
        SetCorrectShowImage();
    }
    public override void Refresh()
    {
        base.Refresh();
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void DynamicDrawTableView()
    {
        //设置滑动区域
        float width = tableView.GetComponent<RectTransform>().sizeDelta.x;

        //测试；tableView的子节点
        int count = Config.animalatlasConfig.getInstace().RowNum;

        tableView.GetComponent<RectTransform>().sizeDelta = new UnityEngine.Vector2(width, 235f * count);

        for (int i = 0; i < count; i++)
        {
            Transform trans = null;
            trans = GameObject.Instantiate(transform.Find(Config.globalConfig.getInstace().AnimalKindCellPath));
            trans.SetParent(tableView, false);
            trans.gameObject.SetActive(true);
            //赋值
            InitCompentTableView(i,trans);
        }
    }

    List<int> littleZooIDs = new List<int>();
    public void InitCompentTableView(int idx, Transform transform)
    {
        int littleZooID = littleZooIDs[idx];
        var buildUpCell = Config.buildupConfig.getInstace().getCell(littleZooID);

        //var animalArray = buildUpCell.animalid;
        string nameStr = "AnimalKindView/AnimalCell/CollectInfo_{0}";
        for (int i = 0; i < 5; i++)
        {
            Transform collectInfo = transform.Find(string.Format(nameStr, i));
            InitAnimalupCellData(idx, i, out Config.animalupCell animalupCell, out int animalID);

            //LogWarp.LogErrorFormat("测试：   图鉴： 动物栏ID={0} name={1}    animalID={2}  name={3}    idx ={4}", littleZooID, GetL10NString(buildUpCell.buildname),animalID, GetL10NString(animalupCell.nametranslate), idx);

            Image collectIcon = collectInfo.Find("CollectIcon").GetComponent<Image>();
            string iconPath = animalupCell.icon;
            collectIcon.sprite = ResourceManager.LoadSpriteFromPrefab(iconPath);

            Text collectText = collectInfo.Find("CollectText").GetComponent<Text>();
            collectText.text = GetL10NString(animalupCell.nametranslate);
            Transform lockIcon = collectInfo.Find("LockIcon");

            bool IsInclude = GlobalDataManager.GetInstance().playerData.playerZoo.animalMSS15.FindAnimalProp(idx, i, out AnimalProp animalProp);
            if (IsInclude)
            {
                collectIcon.material = null;
                lockIcon.gameObject.SetActive(false);
                collectInfo.GetComponent<Button>().enabled = true;
            }
            else
            {
                lockIcon.gameObject.SetActive(true);

                var testImage = Resources.Load<Material>("TestImage");
                collectIcon.material = testImage;
                collectInfo.GetComponent<Button>().enabled = false; ;
            }

            collectInfo.GetComponent<Button>().onClick.AddListener(delegate
            {
                OnClickGoToAnimalPage(animalID);
            });
            Text nameText = transform.Find("AnimalKindView/AnimalKind/NameText").GetComponent<Text>();
            nameText.text = GetL10NString(buildUpCell.buildname);
        }
    }

    private void OnClickGoToAnimalPage(int animalID)
    {
        float timeCount = 0.1f;
        DOTween.To(() => timeCount, a => timeCount = a, 0.1f, 0.1f).OnComplete(new TweenCallback(delegate
        {
            PageMgr.ShowPage<UICollectWindowPage>(animalID);  //图鉴入口
        }));
    }

    private List<int> GetSenceLittleZooIDs()
    {
        List<int> LittleZooIDs = new List<int>();

        var animalatlasConfig = Config.animalatlasConfig.getInstace();
        for (int i = 1; i < animalatlasConfig.RowNum+1; i++)
        {
            int buildid = Config.animalatlasConfig.getInstace().getCell(i).buildid;
            LittleZooIDs.Add(buildid);
        }

        return LittleZooIDs;
    }
}
