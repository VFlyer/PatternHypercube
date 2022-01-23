using UnityEngine;

public class ButtonComponent : MonoBehaviour {
	public Transform MeshTransform;
	public TextMesh TextMesh;
	public KMSelectable Selectable;
	public Transform HighlightTransform;

	private string _label;
	public string Label { get { return _label; } set { if (value == _label) return; _label = value; UpdateLabel(); } }

	private Color _labelColor = Color.white;
	public Color LabelColor { get { return _labelColor; } set { if (value == _labelColor) return; _labelColor = value; UpdateLabelColor(); } }

	private void Start() {
		UpdateLabel();
		UpdateLabelColor();
	}

	public void MakeWide() {
		MeshTransform.localScale = new Vector3(0.035f, 0.004f, 0.015f);
		HighlightTransform.localScale = new Vector3(0.04f, 0.004f, 0.02f);
	}

	private void UpdateLabel() {
		TextMesh.text = Label;
	}

	private void UpdateLabelColor() {
		TextMesh.color = LabelColor;
	}
}
