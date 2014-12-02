// ===================================================================================================
// <file="DebugInventoryWindow.cs" product="Homeward">
// <date>2014-11-11</date>
// <summary>Contains an Editor Window for Debugging by adding items in the inventory manually</summary>
// ====================================================================================================

#region Header Files

using UnityEditor;
using UnityEngine;

#endregion

public class DebugInventoryWindow : EditorWindow
{
    private ItemDatabase database;

    // Item attributes
    private string iname = "Item Name";
    private int id = 0;
    private string desc = "Item Description";
    private Item.ItemType type;
    private Texture2D icon;
    private Texture2D iconEmpty;
    private Texture2D iconReplace;
    private GameObject obj;
    private Vector2 scrollPos;

    private int value;

    // Where to locate the Editor Window?
    [MenuItem("Component/(Debug Tool) Homeward Inventory/MainPlayer Backpack")]
    static void OpenWindow()
    {
        DebugInventoryWindow window = (DebugInventoryWindow)EditorWindow.GetWindow(typeof(DebugInventoryWindow), false, "Debug Tool");
    }

    void OnGUI()
    {
        GUI.skin = (GUISkin)(Resources.LoadAssetAtPath("Assets/Editor/Inventory/InvGUIskin.guiskin", typeof(GUISkin)));
        EditorGUILayout.Space();
        GUI.backgroundColor = Color.red;
        GUILayout.Label("// DEBUGGING PURPOSE ONLY //", GUILayout.ExpandWidth(true));
        GUI.backgroundColor = Color.white;
        GUILayout.Label("Homeward Inventory Debugging System", GUILayout.ExpandWidth(true));
        GUILayout.Label("This tool is used to add items to Player's Backpack", GUILayout.ExpandWidth(true));

        // Is there a gameObject with the name "Item Database"?
        if (GameObject.Find("Item Database") != null)
        {
            // Yes. Set DB instance = DB.component
            database = GameObject.Find("Item Database").GetComponent<ItemDatabase>();

            // Begins ScrollView
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            EditorGUILayout.Space();

            // Create new item fields
            iname = EditorGUILayout.TextField("Name ", iname);
            id = EditorGUILayout.IntField("ID ", id);
            desc = EditorGUILayout.TextField("Description ", desc);
            type = (Item.ItemType)EditorGUILayout.EnumPopup("Type ", type);
            obj = EditorGUILayout.ObjectField("Game Object ", obj, typeof(GameObject), true) as GameObject;
            icon = (Texture2D)EditorGUILayout.ObjectField("Icon ", icon, typeof(Texture2D), false);
            iconEmpty = (Texture2D)EditorGUILayout.ObjectField("Empty Icon ", iconEmpty, typeof(Texture2D), false);
            iconReplace = (Texture2D)EditorGUILayout.ObjectField("Icon (acts as temp var) ", iconReplace, typeof(Texture2D), false);

            // Create new item (press button)
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Create Item"))
            {
                database.items.Add(new Item(iname, id, desc, type, icon, iconEmpty, iconReplace, obj, value));
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            GUI.backgroundColor = Color.white;

            // Look at all items in DB
            for (int i = 0; i < database.items.Count; i++)
            {
                // Temporary variable
                Item item = database.items[i];

                EditorGUILayout.BeginHorizontal();
                GUI.backgroundColor = Color.red;

                // Delete item (press button)
                if (GUILayout.Button("Delete"))
                {
                    database.items.RemoveAt(i);
                }

                GUI.backgroundColor = Color.white;
                EditorGUILayout.EndHorizontal();

                EditorGUI.indentLevel++;
                item.itemName = EditorGUILayout.TextField("Name ", item.itemName);
                item.itemID = EditorGUILayout.IntField("ID ", item.itemID);
                item.itemDescription = EditorGUILayout.TextField("Desc ", item.itemDescription);
                item.itemType = (Item.ItemType)EditorGUILayout.EnumPopup("Type ", item.itemType);
                item.itemObject = (GameObject)EditorGUILayout.ObjectField("Game Object ", item.itemObject, typeof(GameObject), true);
                item.itemIcon = (Texture2D)EditorGUILayout.ObjectField("Icon ", item.itemIcon, typeof(Texture2D), true);
                item.itemIconEmpty = (Texture2D)EditorGUILayout.ObjectField("Empty Icon ", item.itemIconEmpty, typeof(Texture2D), true);
                item.itemIconReplace = (Texture2D)EditorGUILayout.ObjectField("Icon (replace) ", item.itemIconReplace, typeof(Texture2D), true);
                EditorGUILayout.Space();
                EditorGUI.indentLevel--;
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            //Ends ScrollView
            EditorGUILayout.EndScrollView();
        }
    }
}
