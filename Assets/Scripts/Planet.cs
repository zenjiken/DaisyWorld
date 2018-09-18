using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {
	[Range(2, 256)]
	public int resolution = 4;
	public bool autoUpdate = true;

	ShapeGenerator shapeGenerator = new ShapeGenerator();
	TerrainFace [] terrainFaces;
	GameObject[] terrainFaceObjects = new GameObject[6];

	void OnValidate() {
		GeneratePlanet ();
	}

	void Initialize ()
	{
		if(terrainFaces != null) {
			foreach(TerrainFace face in terrainFaces) {
				StartCoroutine (face.Destroy());
			}
		}

		terrainFaces = new TerrainFace[6];

		Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.back, Vector3.forward };
		string[] directionNames = { "up", "down", "left", "right", "back", "forward" };

		for(int i=0; i<6; i++) {
			GameObject meshObject;
			if(terrainFaceObjects[i] == null) {
				meshObject = new GameObject ("mesh-face-" + directionNames[i]);
				meshObject.transform.parent = transform;
				
				terrainFaceObjects[i] = meshObject;
				
			} else {
				meshObject = terrainFaceObjects [i];
			}
			terrainFaces [i] = new TerrainFace (shapeGenerator, resolution, directions [i], meshObject);
		}
	}

	public void GeneratePlanet () {
		Initialize ();
		GenerateMesh ();
	}

	void GenerateMesh() {
		foreach (TerrainFace face in terrainFaces) {
			face.ConstructMesh ();
		}
	}
}