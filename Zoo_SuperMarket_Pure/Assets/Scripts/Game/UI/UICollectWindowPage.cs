using DG.Tweening;
using Game.GlobalData;
using System.Collections;
using System.Collections.Generic;
using UFrame.MiniGame;
using UnityEngine;
using UnityEngine.UI;

public class UICollectWindowPage : UIPage
{
    Button closeButton;
    private Transform animalShowCamera;
    private Transform mainCamera;
    Text wordText;
    Text titleText;
    public UICollectWindowPage() : base(UIType.PopUp, UIMode.DoNothing, UICollider.None)
    {
        uiPath = "uiprefab/UICollectWindow";
    }

    public override void Awake(GameObject go)
    {
        base.Awake(go);
        GetTransPrefabAllTextShow(this.transform);

        RegistAllCompent();
    }
    /// <summary>
    /// 内部组件的查找
    /// </summary>
    private void RegistAllCompent()
    {
        closeButton = RegistBtnAndClick("CloseButton", OnClickCloseButton);
        //查找选择的相机
        GameObject camera = GlobalDataManager.GetInstance().zooGameSceneData.camera;
        animalShowCamera = camera.transform.Find("AnimalShowCamera");
        mainCamera = camera.transform.Find("main_camera");
        wordText = RegistCompent<Text>("WordText/WordBg/Scroll View/Viewport/Content");
        titleText = RegistCompent<Text>("TitleText");
    }

    private void OnClickCloseButton(string obj)
    {
        mainCamera.gameObject.SetActive(true);
        animalShowCamera.GetComponent<ShowAnimelCamera>().ShowBool = false;
        animalShowCamera.gameObject.SetActive(false);
        float timeCount = 0.1f;
        DOTween.To(() => timeCount, a => timeCount = a, 0.1f, 0.1f).OnComplete(new TweenCallback(this.Hide));
    }

    /// <summary>
    /// 初始化属性数值
    /// </summary>
    private void InitData()
    {
    }

    /// <summary>
    /// 控件显示赋值
    /// </summary>
    private void InitCompent()
    {
        RegistAllCompent();
        //对相机的显示隐藏
        mainCamera.gameObject.SetActive(true);
        animalShowCamera.gameObject.SetActive(true);
        string zooID = m_data.ToString();
        var resourceID = Config.animalupConfig.getInstace().getCell(zooID).resourceload;
        titleText.text = GetL10NString(Config.animalupConfig.getInstace().getCell(zooID).nametranslate);
        wordText.text = GetL10NString(Config.animalupConfig.getInstace().getCell(zooID).desctranslate);
        //删除Plane  对象下面的子对象，添加新的动物预制体
        Transform gameObject = animalShowCamera.transform.Find("Plane");
        int childCount = gameObject.childCount;
        for (int i = 0; i < childCount; i++)
        {
            UnityEngine.Object.Destroy(gameObject.GetChild(i).gameObject);
        }

        var cellRes = Config.resourceConfig.getInstace().getCell(resourceID);
        var goPart = ResourceManager.GetInstance().LoadGameObject(cellRes.prefabpath);
        goPart.transform.SetParent(animalShowCamera.transform.Find("Plane").transform, false);
        var scale = goPart.transform.localScale;
        goPart.transform.localScale = scale * cellRes.zoomratio;


        Vector3 vector = goPart.transform.position;
        goPart.transform.position = new Vector3(vector.x + cellRes.Xoffset, vector.y + cellRes.Yoffset, vector.z + cellRes.Zoffset);
        Animation animation = goPart.GetComponentInChildren<Animation>();
        animalShowCamera.GetComponent<ShowAnimelCamera>().animation = animation;
        animalShowCamera.GetComponent<ShowAnimelCamera>().ShowBool = true;
    }
    public override void Active()
    {
        base.Active();
        InitCompent();
    }
    public override void Refresh()
    {
        base.Refresh();
    }

    public override void Hide()
    {
        base.Hide();
    }
}
