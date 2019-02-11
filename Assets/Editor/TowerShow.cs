using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof(GameManager))]
public class TowerShow : Editor
{

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		GameManager myScript = (GameManager)target;

        for (int x = 0; x < GameConstants.TOWER_X; x++)
        {
            EditorGUILayout.BeginHorizontal (GUIStyle.none);
            for (int y = 0; y < GameConstants.TOWER_Y; y++)
            {
                int height = myScript.maxXYheights[x, y];
                GUILayout.Label(height.ToString(), EditorStyles.label);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Separator();
        }

		for (int h = 0; h < GameConstants.TOWER_Z; h++)
		{
			EditorGUILayout.BeginVertical();

			for (int i = 0; i < GameConstants.TOWER_X; i++)
			{
				//GUILayout.BeginHorizontal();
				EditorGUILayout.BeginHorizontal (GUIStyle.none);
				for (int j = 0; j < GameConstants.TOWER_Y; j++)
				{
					//GUILayout.Label(myScript.tower[i,j,h].ToString(), EditorStyles.label);
                    Block m = myScript.tower[i,j,h];
                    if(m == null)
						EditorGUILayout.Toggle(false);
					else
						EditorGUILayout.Toggle(true);
					
					//EditorGUILayout.();
					//myScript.tower[i,j,h];
				}
				EditorGUILayout.EndHorizontal ();
			}
			EditorGUILayout.EndVertical();

			EditorGUILayout.Separator ();

		}
//		if(GUILayout.Button("Build Level"))
//		{
//			myScript.spwanLevel();
//		}
	}
}
