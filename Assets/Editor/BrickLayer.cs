using UnityEngine;
using System.Collections.Generic;
//this namespace has all the stuff to do with editor functionality
using UnityEditor;
using NUnit.Framework;
using UnityEditor.SceneManagement;

//EditorWindow is the base class for any custom editor windows
public class BrickLayer : EditorWindow
{
    public static BrickLayer thisWindow;

    //MenuItem lets us run a static function from the menu bar
    //menuItem is the path (e.g. File/Save)
    [MenuItem("BrickLayer", menuItem = "MyGame/Brick Layer")]
    public static void Open()
    {
        //get or open a new window
        thisWindow = GetWindow<BrickLayer>("BrickLayer");
    }

    public int rows, columns;
    public GameObject prefab;

    public Transform parent;

    public List<GameObject> bricksInScene = new List<GameObject>();

    //This runs every frame the window is currently Active (last thing selected)
    private void OnGUI()
    {
        //EditorGUILayout has all the functions to auto-layout our window fields
        //(GameObject) means cast our result into the GameObject type
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab to spawn", prefab, typeof(GameObject), false);

        //"true" here lets us use scene objects as well as game objects
        parent = (Transform)EditorGUILayout.ObjectField("Parent game object", parent, typeof(Transform), true);

        //set rows to whatever value is put in the field (which will be rows if no new number has been added)
              //prevents the amount of rows and columns from going below zero        
        rows = Mathf.Max(EditorGUILayout.IntField("Rows", rows), 0);
        columns = Mathf.Max(EditorGUILayout.IntField("Columns", columns), 0);

        //lets you define a condition under which the following GUI items can be interacted with or not
        EditorGUI.BeginDisabledGroup(prefab == null);

        //in one go, draw the button, and also check if it's been clicked
        if (GUILayout.Button("Lay Bricks"))
        {
            SpawnObjects();
        }

        EditorGUI.EndDisabledGroup();

        //only draw the button if there are bricks to be cleared
        if (bricksInScene.Count > 0)
        {
            if (GUILayout.Button("Clear bricks"))
            {
                ClearObjects();
            }
        }
    }
    private void SpawnObjects()
    {
        float gap = 0.1f;
        //get the prefab's x scale
        float brickWidth = prefab.transform.localScale.x;
        
        //get the brick heigh as well as the gap to leave
        float brickHeightPlusGap = prefab.transform.localScale.y + gap;

        //get the wdith of all our bricks, plus their gaps ( 1 less than the brick count), then half that
                                                                        //and then add half our brick width on to re-centre
        float halfwidth = -((brickWidth  * columns + gap * (columns-1))) / 2f + brickWidth/2f;

        int count = 0;

        //repeat down rows for as many columns as we have
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                GameObject spawned = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                //spawn a brick, starting in the top left, and using our width and height to calculate where to spawn the next one

                spawned.transform.position = new Vector3(halfwidth + brickWidth * i + gap * i, 4.5f - brickHeightPlusGap * j);

                //add a suffix to the name which is the number of bricks we're up to
                spawned.name += count;

                if (parent)
                    spawned.transform.parent = parent;

                bricksInScene.Add(spawned);

                count++;
               
            }
        }

    }

    private void ClearObjects()
    {
        for (int i = 0; i < bricksInScene.Count; )
        {
            GameObject current = bricksInScene[0];

            bricksInScene.Remove(current);

            if (current == null || !current.scene.IsValid())
                continue;

            DestroyImmediate(current);
        }

        MarkSceneDirty();
    }

    private void MarkSceneDirty()
    {
        //get the currently active scene, and mark it dirty
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
}
