using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor {
	
	public override void  OnInspectorGUI() {
		LevelGenerator levelGen = target as LevelGenerator;

		levelGen.gridWidth = EditorGUILayout.IntSlider(levelGen.gridWidth, 0, 100);
		levelGen.gridHeight = EditorGUILayout.IntSlider(levelGen.gridHeight, 0, 100);
		levelGen.randomBlocksCount = EditorGUILayout.IntSlider(levelGen.randomBlocksCount, 0, 32);
		levelGen.blockSize = EditorGUILayout.IntField(levelGen.blockSize);

		if(GUILayout.Button("Generate")) {
			GenerateLevel(levelGen.gridWidth, levelGen.gridHeight, levelGen.blockSize);
		}
		EditorGUILayout.Separator();

		if(GUILayout.Button("Add Enemies")) {
			
		}

		EditorGUILayout.Separator();

		if(GUILayout.Button ("Select Ground")) {
			SelectGround();
		}

		if(GUILayout.Button ("Select Buildings")) {
			SelectBuildings();
		}
	}

	private void SelectGround() {
		GameObject[] objects = GameObject.FindGameObjectsWithTag("Ground");
		Selection.objects = objects;
	}

	private void SelectBuildings() {
		GameObject[] objects = GameObject.FindGameObjectsWithTag("Buildings");
		Selection.objects = objects;		
	}

	private void GenerateLevel(int w,int h, int size) {
		LevelGenerator levelGen = target as LevelGenerator;
		Transform parent = GameObject.FindGameObjectWithTag("Level").transform;

		for(int x=0;x<w;x++) {
			for(int y=0;y<h;y++) {
				Vector3 pos = new Vector3(x*size,0,y*size);

				GameObject gridHolder = new GameObject();
				gridHolder.name = "Grid"+x+"x"+y;
		
				int randomBlock = Random.Range(1,levelGen.randomBlocksCount+1);
				Debug.Log (randomBlock);
				//string prefabName = "Assets/Prefabs/Buildings/Block"+randomBlock.ToString()+".prefab";
				string prefabName = "Assets/Prefabs/Buildings/RGBlendBlock.prefab";
				Debug.Log (prefabName);
				Object prefab = AssetDatabase.LoadAssetAtPath(prefabName, typeof(GameObject));
				GameObject clone =  Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject;
				clone.transform.parent = gridHolder.transform;
				gridHolder.transform.position = pos;
				gridHolder.transform.parent = parent;

			}
		}
	}
}
