using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class TerrainTile : MonoBehaviour {
	Mesh mesh;
	MeshRenderer meshRenderer;
	MeshFilter meshFilter;
	MeshCollider collider;

	Vector3[] vertices;

	private static int[] _triangles = {0,1,2,3,2,1};

	public void Initialize(MeshRenderer meshRenderer, MeshFilter meshFilter) {
		mesh = new Mesh();

		this.meshRenderer = meshRenderer;
		this.meshFilter = meshFilter;

		this.meshFilter.sharedMesh = mesh;
		this.meshRenderer.sharedMaterial = new Material (Shader.Find ("Standard"));
	}

	public void UpdateMesh(Vector3[] vertices) {
		this.vertices = vertices;

		mesh.Clear ();
		mesh.vertices = vertices;
		mesh.triangles = _triangles;
		mesh.RecalculateNormals ();

		collider = gameObject.AddComponent<MeshCollider> ();
		collider.sharedMesh = mesh;
		collider.convex = true;
		collider.inflateMesh = true;
		collider.skinWidth = Mathf.Epsilon;
	}
}
