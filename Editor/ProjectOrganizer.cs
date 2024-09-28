using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
public class ProjectOrganizer : EditorWindow
{
    int selectedTabIndex = 0;
    string[] tabs = { "Organizer", "Asset Type Mapping" };
    int countOFAssetTypeRows;
    int totalNumberofFileExtensions;
    List<AssetTypeRow> assetTypeRows;
    private string[] assetTypeName;
    private bool isDirty = false;

    private int countOrganizerRow;
    List<OraganizerRow> oraganizerRows;
    class OraganizerRow
    {
        public int selectedObjIndex;
        public string folderPath;
        public Object obj;
    }
    class AssetTypeRow
    {
        public string name;
        public string fileExtension;
    }

    Dictionary<string, List<string>> assetTypes = new Dictionary<string, List<string>>
    {
        {"Prefabs" , new List<string>() {".prefab"} },
        {"Animations" , new List<string>() {".anim"} },
        {"Images" , new List<string>(){".png",".jpeg"}},
        {"Music" , new List<string>(){".mp3",".wav"} }
    };
    private void Awake()
    {
        InitializeFields();
    }
    [MenuItem("Dronnzer/ProjectOrganizerWindow")]
    public static void ShowWindow()
    {
        EditorWindow window = GetWindow(typeof(ProjectOrganizer));
        GUIContent gUIContent = new GUIContent();
        gUIContent.text = "Project Organizer Tool ";
        window.titleContent = gUIContent;
        window.Show();
    }
    private void OnGUI()
    {
        DrawToolBarTabs();
        EditorGUILayout.Space(20);
        if(selectedTabIndex == 0)
        {
            if(isDirty)
            {
                isDirty = false;
              updateAssetType(assetTypeName.Length);
            }
            for(int i = 0;i < countOrganizerRow; i++)
            {
                DrawOrganizerRow(i);
            }
            DrawAddAndRemoveControls();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            if (GUILayout.Button("Organize"))
            {
                OrganizeFolder();
            }

        }
        else
        {
            for (int i = 0; i < countOFAssetTypeRows; i++) 
            {
                DrawAssetTypeRow(i);
            }
            DrawAddAndRemoveControls();
        }
    }

    private void DrawToolBarTabs()
    {
        GUILayout.BeginHorizontal();
        selectedTabIndex = GUILayout.Toolbar(selectedTabIndex, tabs);
        GUILayout.EndHorizontal();
        
    }
    void DrawAssetTypeRow(int currentIndex)
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.Space();
        GUILayout.BeginVertical();
        EditorGUILayout.LabelField("Name");
        EditorGUI.BeginChangeCheck();
        if(assetTypeRows != null)
        {
            assetTypeRows[currentIndex].name = EditorGUILayout.TextField(assetTypeRows[currentIndex].name);
        }
        if (EditorGUI.EndChangeCheck())
        {
            isDirty = true;
        }
        GUILayout.EndVertical();
        EditorGUILayout.Space();
        GUILayout.BeginVertical();
        EditorGUILayout.LabelField("File Extension");
        EditorGUI.BeginChangeCheck();
        if (assetTypeRows != null)
        {
            assetTypeRows[currentIndex].fileExtension = EditorGUILayout.TextField(assetTypeRows[currentIndex].fileExtension);
        }
        if (EditorGUI.EndChangeCheck() && assetTypes.ContainsKey(assetTypeRows[currentIndex].name))
        {
            isDirty = true;
        }
        GUILayout.EndVertical();
        EditorGUILayout.Space();
        GUILayout.EndHorizontal();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

    }

    void InitializeFields()
    {
        foreach(string key in assetTypes.Keys)
        {
            totalNumberofFileExtensions += assetTypes[key].Count;
        }
        countOFAssetTypeRows = totalNumberofFileExtensions;
        assetTypeRows = new List<AssetTypeRow>();
        assetTypeName = new string[totalNumberofFileExtensions];
        assetTypes.Keys.CopyTo(assetTypeName, 0);

        for (int i = 0; i < totalNumberofFileExtensions; i++)
        {
            string key = assetTypeName[i];
            if(key != null)
            {
                int numberOfFileextensionAssetType = assetTypes[key].Count;
                for(int j = 0; j < numberOfFileextensionAssetType; j++)
                {
                    assetTypeRows.Add(new AssetTypeRow()
                    {
                        name = assetTypeName[i],
                        fileExtension = assetTypes[assetTypeName[i]][j]
                    });
                }
            }
        }
        countOrganizerRow = assetTypes.Keys.Count;
        oraganizerRows = new List<OraganizerRow>();
        for (int i = 0; i < countOrganizerRow; i++)
        {
            oraganizerRows.Add(new OraganizerRow()
            {
                selectedObjIndex = i,
                folderPath = "Assets/" + assetTypeName[i]
            });
        }
    }

    void updateAssetType(int currentIndex)
    {
        assetTypes.Add(assetTypeRows[currentIndex].name , new List<string>() { } );
        assetTypes[assetTypeRows[currentIndex].name].Add(assetTypeRows[currentIndex].fileExtension);
        totalNumberofFileExtensions = 0;
        foreach(string key in assetTypes.Keys)
        {
            totalNumberofFileExtensions += assetTypes[key].Count;
        }
        assetTypeName = new string[totalNumberofFileExtensions - 1];
        assetTypes.Keys.CopyTo(assetTypeName,0);
    }

    void DrawOrganizerRow(int currentIndex)
    {
        GUILayout.BeginHorizontal();

        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Asset Type");
        EditorGUI.BeginChangeCheck();

        oraganizerRows[currentIndex].selectedObjIndex = EditorGUILayout.Popup(
            "",
            oraganizerRows[currentIndex].selectedObjIndex,
            assetTypeName
                );
        if (EditorGUI.EndChangeCheck())
        {
            oraganizerRows[currentIndex ].folderPath = "Assets/"+ assetTypeName[oraganizerRows[currentIndex].selectedObjIndex];
        }
        GUILayout.EndVertical();
        EditorGUILayout.Space();
        GUILayout.BeginVertical();
        EditorGUILayout.LabelField("Path to Folder");
        oraganizerRows[currentIndex].folderPath = EditorGUILayout.TextField(
           oraganizerRows[currentIndex].folderPath
            );
        GUILayout.EndVertical();
        EditorGUILayout.Space();
        GUILayout.BeginVertical();
        EditorGUILayout.LabelField("Select Folder");
        EditorGUI.BeginChangeCheck();
        oraganizerRows[currentIndex].obj = EditorGUILayout.ObjectField(
            oraganizerRows[currentIndex].obj,
            typeof(UnityEditor.DefaultAsset), true
            );
        if(EditorGUI.EndChangeCheck())
        {
            oraganizerRows[currentIndex].folderPath = "Assets/" + oraganizerRows[currentIndex].obj;
        }
        GUILayout.EndVertical();
        EditorGUILayout.Space();
        GUILayout.EndHorizontal();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }

    void DrawAddAndRemoveControls()
    {
        
        if (selectedTabIndex == 0)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            GUIContent add = new GUIContent();
            add.text = "+";

            if (GUILayout.Button(add))
            {
                countOrganizerRow++;
                oraganizerRows.Add(new OraganizerRow());
            }

            GUIContent remove = new GUIContent();
             remove.text = "-";

            if (GUILayout.Button(remove))
            {
                countOrganizerRow--;
                oraganizerRows.Remove(new OraganizerRow());
            }
          GUILayout.EndHorizontal();
        }else
        {
            EditorGUILayout.LabelField("If you want to add the extension u can add through code .");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Open the ProjectOrganizer.cs go the assetTypes Dictionary and add the extension");
            EditorGUILayout.LabelField("Imp * In First Parameter of Dictoinary add the FileType - " + "Example('Music')");
            EditorGUILayout.LabelField("Imp * In Second Parameter of Dictoinary add the ExtensionType - " + "Example('.mp3')");


        }

    }
    private void OrganizeFolder()
    {
        Dictionary<string,string> filesExtensionToFolderPathMap = new Dictionary<string,string>();
        foreach(string assetTypeName in assetTypes.Keys)
        {
            string pathToPrefabsFolder = "Assets/" + assetTypeName;
            bool doesPrefabsFolderExist = AssetDatabase.IsValidFolder(pathToPrefabsFolder);
            if (!doesPrefabsFolderExist)
            {
                AssetDatabase.CreateFolder("Assets", assetTypeName);
            }
            for (int i = 0; i < assetTypes[assetTypeName].Count; i++)
            {
                string folderPath = "Assets/" + assetTypeName + "/";
                filesExtensionToFolderPathMap.Add(assetTypes[assetTypeName][i], folderPath);
            }
        }
        DirectoryInfo dir = new DirectoryInfo("Assets/");
        foreach(string fileExtension in filesExtensionToFolderPathMap.Keys)
        {
            string query = "*" + fileExtension;
            FileInfo[] info = dir.GetFiles(query);
            foreach (FileInfo infoItem in info)
            {
                string filePath = filesExtensionToFolderPathMap[fileExtension] + infoItem.Name;
                AssetDatabase.MoveAsset("Assets/" + infoItem.Name, filePath);
            }
        }
    }
}
