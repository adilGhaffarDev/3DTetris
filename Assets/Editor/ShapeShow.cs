using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof(Shape))]
public class ShapeShow : Editor
{
    bool isFirst = true;
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

        Shape myScript = (Shape)target;
//
//        if (isFirst)
//        {
//            isFirst = false;
//            myScript.Start();
//        }

        EditorGUILayout.BeginHorizontal (GUIStyle.none);

        if(GUILayout.Button("Rotate 90"))
        {
            myScript.rotateShape(90);
        }

        if(GUILayout.Button("Rotate -90"))
        {
            myScript.rotateShape(-90);
        }

        EditorGUILayout.EndHorizontal ();

        for (int h = 0; h < myScript.blockHeight; h++)
		{
			EditorGUILayout.BeginVertical();

			for (int i = 0; i < GameConstants.TOWER_X; i++)
			{
				//GUILayout.BeginHorizontal();
				EditorGUILayout.BeginHorizontal (GUIStyle.none);
				for (int j = 0; j < GameConstants.TOWER_Y; j++)
				{
//                    GUILayout.Label(myScript.wholeBlock[i,j,h].ToString(), EditorStyles.label);
//                    int m = myScript.wholeBlock[i,j,h];
//                    if(m == 0)
//						EditorGUILayout.Toggle(false);
//					else
//						EditorGUILayout.Toggle(true);
				}
				EditorGUILayout.EndHorizontal ();
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.Separator ();

		}
	}
}
