/*
	Created by @HarshBhardwaj follow at github https://github.com/HarshBhardwaj08
	Thanks so much for checking this out and I hope you find it helpful! 
	If you have any further queries, questions or feedback feel free to reach out on my email - harshbhardwaj882536@gmail.com

	Feel free to use this in your own games, and I'd love to see anything you make!
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
public class AutoSaveTool : EditorWindow
{

    const string menuString = "Dronnzer/AutoSave";
    static EditorWindow window;
    int choice;
    const string ONE_SECOND = "1 sec";
    const string Thirty_SECOND = "30 sec";
    const string One_Min = "1 min";
    const string Five_Minutes = "5 min";
    string[] choices = { ONE_SECOND, Thirty_SECOND, One_Min, Five_Minutes };
    static float saveTime = 1;
    static float nextSave = 0;

    public static bool IsEnable
    {
        get { return EditorPrefs.GetBool(menuString, false); }
        set { EditorPrefs.SetBool(menuString, value); }
    }

    [MenuItem(menuString, false, 175)]
    public static void ToggleAutoSave()
    {
        IsEnable = !IsEnable;
        if (IsEnable)
        {
            ShowWindow();
            EditorApplication.update += AutoSaveUpdate;  // Register update callback
        }
        else
        {
            EditorApplication.update -= AutoSaveUpdate;  // Unregister when disabled
        }
    }

    [MenuItem(menuString, true)]
    public static bool ToggleActionValidate()
    {
        Menu.SetChecked(menuString, IsEnable);
        return true;
    }

    static void ShowWindow()
    {
        window = GetWindow(typeof(AutoSaveTool));
        GUIContent gUIContent = new GUIContent();
        gUIContent.text = "Autosave Settings";
        window.titleContent = gUIContent;
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Interval :  ");
        EditorGUILayout.Space();
        EditorGUI.BeginChangeCheck();
        choice = EditorGUILayout.Popup("", choice, choices);
        if (EditorGUI.EndChangeCheck())
        {
            switch (choices[choice])
            {
                case ONE_SECOND:
                    saveTime = 1;
                    break;

                case Thirty_SECOND:
                    saveTime = 30;
                    break;

                case One_Min:
                    saveTime = 60;
                    break;

                case Five_Minutes:
                    saveTime = 300;
                    break;
            }
        }
    }

    static void AutoSaveUpdate()
    {
        if (IsEnable)
        {
            if (EditorApplication.timeSinceStartup > nextSave)
            {
                string[] path = EditorSceneManager.GetActiveScene().path.Split(char.Parse("/"));
                bool saveSuccess = EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene(), string.Join("/", path));
                nextSave = (float)EditorApplication.timeSinceStartup + saveTime;
                Debug.Log("Auto-save was successful? " + saveSuccess);
            }
        }
    }
}
