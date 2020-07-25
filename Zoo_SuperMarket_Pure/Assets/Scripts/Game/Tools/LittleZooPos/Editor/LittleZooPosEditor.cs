/*******************************************************************
* FileName:     LittleZooBuildinPosEditor.cs
* Author:       Fan Zheng Yong
* Date:         2019-8-17
* Description:  
* other:    
********************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Game.Tools;
using System.IO;
using System.Text;
using UFrame.Logger;
using UFrame.Path.StraightLine;

namespace Game.Tools
{
    [CustomEditor(typeof(LittleZooPos))]
    public class LittleZooPosEditor : Editor
    {
        private LittleZooPos littleZooPos;

        void OnEnable()
        {
            littleZooPos = target as LittleZooPos;
        }

        public override void OnInspectorGUI()
        {
            if (target == null || littleZooPos == null)
            {
                return;
            }

            base.OnInspectorGUI();


            if (GUILayout.Button("导出"))
            {
                littleZooPos.ProtecteRoot();
                ExportToCSHARP();
            }
        }

        public void ExportToCSHARP()
        {
            string csharpFile = "///本代码由动物栏 位置编辑器 自动生成, 请勿手动修改! by : Fan Zheng Yong\r\n" +
                "using System.Collections.Generic;\r\n" +
                "using UFrame.Common;\r\n" +
                "using UnityEngine;\r\n" +
                "namespace Game\r\n" +
                "{\r\n" +
                "    public partial class LittleZooPosManager : Singleton<LittleZooPosManager>, ISingleton\r\n" +
                "    {\r\n" +
                "        public void $AddFunc$()\r\n" +
                "        {\r\n";
            int littleZooNum = littleZooPos.transform.childCount;
            for (int i = 0; i < littleZooNum; i++)
            {
                var littleZooNode = littleZooPos.transform.GetChild(i);

                //动物栏ID
                int mapLittleZooID = int.Parse(littleZooNode.name);
                mapLittleZooID = LittleZooModule.MapToFirstSceneLittleZooID(mapLittleZooID);
                string littleZooID = "littleZoo_" + mapLittleZooID.ToString();

                //声明
                Debug.Log(littleZooNode.position);
                string littleZooST = string.Format("\r\n            var {0} = new Vector3({1}f, {2}f, {3}f);\r\n",
                    littleZooID, littleZooNode.position.x, littleZooNode.position.y, littleZooNode.position.z);
                csharpFile += littleZooST;


                string addToMap = string.Format("            posMap.Add({0}, {1});\r\n", mapLittleZooID.ToString(), littleZooID);
                csharpFile += addToMap;

            }

            csharpFile += "        }\r\n";
            csharpFile += "    }\r\n";
            csharpFile += "}\r\n";
            FileUtil.CreateDir("./EditorConfig", true);
            string fileName = "LittleZooPosManager_Style_Ten_LittleZoo.cs";
            if (littleZooNum == 9)
            {
                fileName = "LittleZooPosManager_Style_Nine_LittleZoo.cs";
                csharpFile = csharpFile.Replace("$AddFunc$", "Add_Style_Nine_LittleZoo");
            }
            else
            {
                csharpFile = csharpFile.Replace("$AddFunc$", "Add_Style_Ten_LittleZoo");
            }
            //string filepath = "./EditorConfig/LittleZooPosManager_AddAll.cs";
            string filepath = string.Format("./EditorConfig/{0}", fileName);
            File.WriteAllText(filepath, csharpFile, Encoding.UTF8);
            AssetDatabase.Refresh();
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }

}
