using System.Linq;
using UnityEngine;

public class HyperfaceComponent : MonoBehaviour {
	public MeshFilter MeshFilter;
	public Renderer Renderer;

	public _4D.Matrix5x5Int SelfRotation = _4D.Matrix5x5Int.IDENTITY;
	public _4D.Matrix5x5 AnimationRotation = _4D.Matrix5x5.IDENTITY;
	public _4D.Matrix5x5Int LocalTransform = _4D.Matrix5x5Int.IDENTITY;

	private int[] _symbols = Enumerable.Repeat(-1, 6).ToArray();
	public int[] Symbols {
		get { return _symbols.ToArray(); }
		set {
			if (value.Length != 6) throw new System.Exception();
			_symbols = value.ToArray();
			UpdateUV2();
		}
	}

	private int[] _expectedSymbols = Enumerable.Repeat(-1, 6).ToArray();
	public int[] ExpectedSymbols {
		get { return _expectedSymbols.ToArray(); }
		set {
			if (value.Length != 6) throw new System.Exception();
			_expectedSymbols = value.ToArray();
		}
	}

	private bool _placed = false;
	public bool Placed { get { return _placed; } set { if (_placed == value) return; _placed = value; UpdateTransparency(); } }

	private Vector3[] _vertices;

	private void Start() {
		Vector2[] axles = new[] { Vector2.right, Vector2.up, Vector2.left, Vector2.down };
		_vertices = new[] { -1, 1 }.SelectMany(dz => (
			Enumerable.Range(0, 4).Select(i => (_4D.Utils.ToVector3(axles[i] + axles[(i + 1) % 4], dz)) * 2f)
		)).Concat(new[] { Vector3.back, Vector3.forward, Vector3.right, Vector3.left, Vector3.up, Vector3.down }.Select(v3 => v3 * 2f)).ToArray();
		Mesh mesh = MeshFilter.mesh;
		mesh.Clear();
		BuildMeshVertices(_vertices);
		mesh.triangles = Enumerable.Range(0, 6).SelectMany(i => Enumerable.Range(0, 4).SelectMany(j => new[] { 24 + i, 4 * i + j, 4 * i + (j + 1) % 4 })).ToArray();
		mesh.uv = Enumerable.Range(0, 6).SelectMany(_ => (
			new[] { Vector2.right, Vector2.zero, Vector2.up, Vector2.one }
		)).Concat(Enumerable.Repeat(Vector2.one / 2f, 6)).ToArray();
		UpdateTransparency();
		UpdateUV2();
	}

	public void Transform(_4D.Matrix5x5 parentTransfrom) {
		Mesh mesh = MeshFilter.mesh;
		_4D.Matrix5x5 m5 = SelfRotation * AnimationRotation * LocalTransform * parentTransfrom;
		Vector3[] transformedVertices = _vertices.Select(v3 => v3 * m5).ToArray();
		BuildMeshVertices(transformedVertices);
	}

	public void ResetSymbols() {
		Symbols = Enumerable.Repeat(-1, 6).ToArray();
	}

	public bool HasValidSymbols() {
		for (int i = 0; i < 6; i++) {
			if (Symbols[i] != ExpectedSymbols[i]) return false;
		}
		return true;
	}

	private void UpdateTransparency() {
		Renderer.material.SetFloat("_Transparency", Placed ? 0.5f : 0f);
	}

	private void UpdateUV2() {
		Mesh mesh = MeshFilter.mesh;
		if (mesh.vertices.Length != 30) return;
		Vector2[] symbolsUVs = Symbols.Select(symbol => new Vector2(symbol / 12 + 0.5f, 15 - symbol % 12 + 0.5f) / 16f).ToArray();
		mesh.uv2 = Enumerable.Range(0, 6).SelectMany(i => Enumerable.Repeat(symbolsUVs[i], 4)).Concat(symbolsUVs).ToArray();
	}

	private void BuildMeshVertices(Vector3[] v3s) {
		Mesh mesh = MeshFilter.mesh;
		mesh.vertices = new[] {
			0, 1, 2, 3, // back
			4, 7, 6, 5, // front
			0, 3, 7, 4, // right
			1, 5, 6, 2, // left
			0, 4, 5, 1, // up
			2, 6, 7, 3, // down
		}.Concat(Enumerable.Range(8, 6)).Select(ind => v3s[ind]).ToArray();
	}
}
