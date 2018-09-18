using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Security;

public class TerrainFace{
	ShapeGenerator shapeGenerator;

	int resolution;
	int tileCount;

	Vector3 localUp;
	Vector3 axisA;
	Vector3 axisB;

	Vector3 [] vertices;

	public TerrainTile[] tiles{ get; private set;}
	public GameObject[] tileObjects { get; private set;}

	public TerrainFace (ShapeGenerator shapeGenerator, int resolution, Vector3 localUp, GameObject tilesParent) {
		this.shapeGenerator = shapeGenerator;
		this.resolution = resolution;
		this.localUp = localUp;

		axisA = new Vector3 (localUp.y, localUp.z, localUp.x);
		axisB = Vector3.Cross (localUp, axisA);

		tileCount = (resolution - 1) * (resolution - 1);

		InitializeTiles (tilesParent);
	}

	public void ConstructMesh() {
		vertices = new Vector3[resolution * resolution];

		for (int y = 0; y < resolution; y++) {
			for(int x = 0; x < resolution; x++) {
				Vector2 percent = new Vector2 (x, y) / (resolution - 1);
				Vector3 pointOnUnitCube = localUp + (percent.x - 0.5f) * 2 * axisA + (percent.y - 0.5f) * 2 * axisB;

				Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;

				int i = x + y * resolution;
				vertices [i] = shapeGenerator.CalculatePointOnPlanet (pointOnUnitSphere);
			}
		}

		int mod = -1;
		for (int i = 0; i< tileCount; i++) {
			Vector3[] tileVertices = new Vector3[4];

			mod += (i % (resolution - 1) == 0) ? 1 : 0;

			tileVertices [0] = vertices [i + mod];
			tileVertices [1] = vertices [i + 1 + mod];
			tileVertices [2] = vertices [i + resolution + mod];
			tileVertices [3] = vertices [i + resolution + 1 + mod];


			tiles [i].UpdateMesh (tileVertices);
		}

	}

	public IEnumerator Destroy() {
		yield return null;
		for(int i = 0; i< tileCount; i++) {
			if(tileObjects [i] != null) {
				Object.DestroyImmediate(tileObjects [i]);
			}
		}
	}

	private void InitializeTiles(GameObject tilesParent) {
		tiles = new TerrainTile[tileCount];
		tileObjects = new GameObject[tileCount];

		for (int i = 0; i < tileCount; i++) {
			tileObjects [i] = new GameObject ("Tile-" + i);
			tileObjects [i].transform.parent = tilesParent.transform;

			tiles [i] = tileObjects [i].AddComponent<TerrainTile> ();

			MeshRenderer tileRenderer = tileObjects [i].AddComponent<MeshRenderer> ();
			MeshFilter tileFilter = tileObjects [i].AddComponent<MeshFilter> ();

			tiles [i].Initialize (tileRenderer, tileFilter);
		}
	}
}
