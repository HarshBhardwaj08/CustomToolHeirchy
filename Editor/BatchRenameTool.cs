using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class BatchRenameTool : EditorWindow
{
    string batchname = "";
    string batchNumber;
    bool showOptions;
    [MenuItem("Dronnzer/Batch Rename")]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow(typeof(BatchRenameTool));
        window.maxSize = new Vector2(500, 150);
        window.minSize = window.maxSize;
        GUIContent gUIContent = new GUIContent();
        gUIContent.text = "Batch Rename";
        window.titleContent = gUIContent;
        window.Show();
    }
    private void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Step 1 : Select Object in the hierarchy",EditorStyles.boldLabel);
        EditorGUILayout.Space();
      
        GUIStyle gUIStyle = new GUIStyle(EditorStyles.foldout);
        gUIStyle.fontStyle = FontStyle.Bold;
        showOptions = EditorGUILayout.Foldout(showOptions, "Step 2 : Enter rename info", gUIStyle);
        if(showOptions)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("\t Enter name for batch");
            batchname = EditorGUILayout.TextField(batchname);
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("\t Enter Starting Number");
            batchNumber = EditorGUILayout.TextField(batchNumber);
            EditorGUILayout.Space();
            EditorGUILayout.EndHorizontal();
        }
       
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Step 3 : Click the rename button", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Space();
        
        EditorGUILayout.Space();
        if (GUILayout.Button("Rename"))
        {
            int numberAsInt = int.Parse(batchNumber);
            foreach(GameObject obj in Selection.gameObjects)
            {
                obj.name = batchname + "_" + numberAsInt.ToString();
                numberAsInt++;
            }
        }
        EditorGUILayout.Space();
        EditorGUILayout.EndHorizontal ();
        Repaint();
    }
}
