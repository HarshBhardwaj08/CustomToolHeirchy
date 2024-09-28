/*
	Created by @HarshBhardwaj follow at github https://github.com/HarshBhardwaj08
	Thanks so much for checking this out and I hope you find it helpful! 
	If you have any further queries, questions or feedback feel free to reach out on my email - harshbhardwaj882536@gmail.com

	Feel free to use this in your own games, and I'd love to see anything you make!
 */
using Custom.Herichy;
using System;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class CustomHerichyTool
{
     static CustomHerichyTool()
    {
        EditorApplication.hierarchyWindowItemOnGUI += heirchyWindowItemONGUI;
    }

    private static void heirchyWindowItemONGUI(int instanceID, Rect selectionRect)
    {
        DrawActiveToggleButton(instanceID, selectionRect);
        AddInfoSCriptToGameObject(instanceID);
        DrawInfoButton(instanceID, selectionRect,"");
        DrawZoomInButton(instanceID, selectionRect, "Frame this Game Object");
        DrawPrefabButton(instanceID, selectionRect, "Save as Prefab");
        DrawDeleteButton(instanceID, selectionRect, "Delete GameObject");
    }
    static Rect DrawReact(float x , float y , float size)
    {
        return new Rect(x, y, size,size);
    }
    static void DrawButtonWithToggle(int id , float x , float y , float size)
    {
        GameObject obj = EditorUtility.InstanceIDToObject(id) as GameObject;
        if(obj != null)
        {
            Rect r = DrawReact(x , y , size);
            obj.SetActive(GUI.Toggle(r, obj.activeSelf, string.Empty));
        }
    }
    static void DrawActiveToggleButton(int id , Rect rect)
    {
        DrawButtonWithToggle(id, rect.x - 30, rect.y + 3, 10);
    }
    static void DrawButtonWithTexture(float x,float y , float size , string name ,
        Action action , GameObject gameObject,string tooltip)
    {
        if(gameObject != null)
        {
            GUIStyle style = new GUIStyle();
            style.fixedHeight = 0;
            style.fixedWidth = 0;
            style.stretchHeight = true;
            style.stretchWidth = true;
            Rect r = DrawReact(x, y , size);
            Texture t = Resources.Load(name) as Texture ;
            GUIContent gUIContent = new GUIContent();
            gUIContent.image = t;
            gUIContent.text = "";
            gUIContent.tooltip = tooltip;
            bool isClicked = GUI.Button(r, gUIContent, style);
            if(isClicked)
            {
                action.Invoke();
            }
        }
    }

    static void DrawInfoButton(int id , Rect rect,string tooltip)
    {
        GameObject gameObject = EditorUtility.InstanceIDToObject(id) as GameObject;
        if(gameObject != null)
        {
            bool hasInfoScript = gameObject.GetComponent<Info>();
            if(hasInfoScript)
            {
                Info info = gameObject.GetComponent<Info>();
                if (info)
                {
                    tooltip = info.info;
                }
            }
        }
        DrawButtonWithTexture(rect.x + 150 , rect.y + 2 ,14 ,
            "info",() => { },gameObject,tooltip);
    }
    static void AddInfoSCriptToGameObject(int id)
    {
        GameObject gameObject = EditorUtility.InstanceIDToObject(id) as GameObject;
        if (gameObject != null)
        {
            bool hasInfoScript = gameObject.GetComponent<Info>();
            if (!hasInfoScript)
            {
                gameObject.AddComponent<Info>();
            }
        }
    }
    static void DrawZoomInButton(int id , Rect rect,string tooltip)
    {
        GameObject gameObject = EditorUtility.InstanceIDToObject(id) as GameObject;
        if (gameObject != null)
        {
            DrawButtonWithTexture(rect.x + 175, rect.y + 2, 14, "zoomin", () =>
            {
                Selection.activeObject = gameObject;
                SceneView.FrameLastActiveSceneView();
            }, gameObject, tooltip);
        }
    }
    static void DrawPrefabButton(int id , Rect rect,string tooltip)
    {
        GameObject gameObject = EditorUtility.InstanceIDToObject(id ) as GameObject;
        if(gameObject != null)
        {
            DrawButtonWithTexture(rect.x + 200, rect.y + 2, 14, "prefab", () =>
            {
                const string pathToPrefabsFolder = "Assets/Prefabs";
                bool doesPrefabsFolderExist = AssetDatabase.IsValidFolder(pathToPrefabsFolder);
                if (!doesPrefabsFolderExist)
                {
                    AssetDatabase.CreateFolder("Assets", "Prefabs");
                }
                string prefabName = gameObject.name + ".prefab";
                string prefabPath = pathToPrefabsFolder + "/" + prefabName;
                AssetDatabase.DeleteAsset(prefabName);
                GameObject prefab = PrefabUtility.SaveAsPrefabAsset(gameObject, prefabPath);
                EditorGUIUtility.PingObject(prefab);
            },gameObject,tooltip);
        }
    }
    static void DrawDeleteButton(int id , Rect rect,string tooltip)
    {
        GameObject gameObject = EditorUtility.InstanceIDToObject (id ) as GameObject;
        if(gameObject != null)
        {
            DrawButtonWithTexture(rect.x + 225, rect.y + 2, 14, "delete", () =>
            {
               // UnityEngine.Object.DestroyImmediate(gameObject);
                Undo.DestroyObjectImmediate(gameObject);
            },gameObject,tooltip);
        }
    }
}

