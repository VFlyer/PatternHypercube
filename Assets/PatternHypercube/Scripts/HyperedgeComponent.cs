using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HyperedgeComponent : MonoBehaviour {
	public MeshFilter MeshFilter;

	public _4D.Matrix5x5Int LocalTransform;
	public _4D.Matrix5x5 LocalAnimationTransform;

	private Vector3[] _vertices;

	private void Start() {
		Mesh mesh = MeshFilter.mesh;
		mesh.Clear();
		mesh.vertices = Enumerable.Range(0, 4).Select(_ => Vector3.zero).ToArray();
	}
}
