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
public class MIssingRefrenceDetector : EditorWindow
{
    [MenuItem("Dronnzer/MissingRefrenceWindow")]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow(typeof(MIssingRefrenceDetector));
        window.maxSize = new Vector2(250, 100);
        window.minSize = window.maxSize;
        GUIContent guiContent = new GUIContent();
        guiContent.text = "Find Missing Refrence";
        window.titleContent = guiContent;
        window.Show();
    }
    private void OnGUI()
    {
        EditorGUILayout.Space(25);
        if (GUILayout.Button("Find Missing Refrence"))
        {
          GameObject[] gameObjects = FindObjectsOfType<GameObject>();
            foreach (var item in gameObjects)
            {
                Component[] components = item.GetComponents<Component>();
                foreach (var component in components)
                {
                    SerializedObject serializedObject = new SerializedObject(component);
                    SerializedProperty serializedProperty = serializedObject.GetIterator();
                    while (serializedProperty.NextVisible(true))
                    {
                        if (serializedProperty.propertyType == SerializedPropertyType.ObjectReference)
                        {
                            if (serializedProperty.objectReferenceValue == null)
                            {
                                Debug.Log("<color=red><b>Missing refrence : </b></color>" + serializedProperty.displayName + " on " +
                                    item.name );
                            }
                        }

                    }
                }
            }
        }
       
        EditorGUILayout.Space();
        Repaint();
    }
}
