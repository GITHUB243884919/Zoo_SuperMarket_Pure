using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class UILittleGameMainPage : UIPage
    {
        public UILittleGameMainPage() : base(UIType.Fixed, UIMode.DoNothing, UICollider.None)
        {
            uiPath = "UIPrefab/UILittleGameMain";
        }
        public override void Awake(GameObject go)
        {
            base.Awake(go);
        }

    }

