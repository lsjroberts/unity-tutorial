using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SplineMesh))]
public class SplineMeshInspector : Editor {
  private SplineMesh splineMesh;

  private void OnSceneGUI() {
    splineMesh = target as SplineMesh;

    GUIStyle style = new GUIStyle();
    style.normal.textColor = Color.green;

    for (int i = 0; i < splineMesh.vertices.Length; i++) {
      Handles.Label(splineMesh.vertices[i], "v" + i, style);
    }
  }
}
