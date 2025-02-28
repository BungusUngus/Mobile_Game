using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
public class Random : EditorWindow
{
    public static Random thisWindow;

    [MenuItem("Random", menuItem = "MyGame/Random")]

    public static void Open()
    {
        thisWindow = GetWindow<Random>("Randomizer");
    }
  
    public GameObject ball;

    public Vector3 scenePosition;

    public List<GameObject> ballsList = new List<GameObject>();

    private void OnGUI()
    {
        ball = (GameObject)EditorGUILayout.ObjectField("balls to spawn", ball, typeof(GameObject), false);

        scenePosition = EditorGUILayout.Vector3Field("Position", scenePosition);

        EditorGUI.BeginDisabledGroup(ball == null);
        if (GUILayout.Button("Spawn Balls"))
        {
            Instantiate(ball, scenePosition, Quaternion.identity);
        }
        EditorGUI.EndDisabledGroup();
    }

    
}
