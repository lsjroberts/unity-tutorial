using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SplineMesh : MonoBehaviour {
  public BezierSpline spline;
  public int xSize;

  public Vector3[] vertices;
  private Mesh mesh;
  private float depth = 2f;
  private float width = 4f;
  private int ySize = 1;
  private int zSize = 1;

  private void Awake() {
    // if (quadsPerCurve <= 0) {
    //   return;
    // }

    Generate();
  }

  private void Generate() {
    GetComponent<MeshFilter>().mesh = mesh = new Mesh();
    mesh.name = "Spline Mesh";

    StartCoroutine(CreateVertices());
  }

  private IEnumerator CreateVertices() {
    WaitForSeconds wait = new WaitForSeconds(0.05f);

    Log("Creating vertices");

    int cornerVertices = 8;
    int edgeVertices = (xSize + ySize + zSize - 3) * 4;
    int faceVertices = (
      (xSize - 1) * (ySize - 1) +
      (xSize - 1) * (zSize - 1) +
      (ySize - 1) * (zSize - 1)
    ) * 2;
    vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];

    Log("Number of vertices: " + vertices.Length);

    int v = 0;
    for (int y = 0; y <= ySize; y++) {

      // Side
      for (int x = 0; x < xSize; x++) {
        // vertices[v++] = new Vector3(x, y, 0);
        float t = (float) x / xSize;
        Vector3 point = spline.GetPoint(t);
        Vector3 halfWidth = (Vector3.Cross(Vector3.up, spline.GetDirection(t)).normalized * (width / 2f));
        vertices[v++] = point + halfWidth + new Vector3(0, y * depth, 0);
        // yield return wait;
      }

      // End
      for (int z = 1; z <= zSize; z++) {
        // vertices[v++] = new Vector3(xSize, y, z);
      }

      // Side
      for (int x = xSize - 1; x >= 0; x--) {
        // vertices[v++] = new Vector3(x, y, zSize);
        float t = (float) x / xSize;
        Vector3 point = spline.GetPoint(t);
        Vector3 halfWidth = (Vector3.Cross(Vector3.up, spline.GetDirection(t)).normalized * (width / 2f));
        vertices[v++] = point - halfWidth + new Vector3(0, y * depth, 0);
        // yield return wait;
      }

      // End
      for (int z = zSize - 1; z > 0; z--) {
        // vertices[v++] = new Vector3(0, y, z);
      }
    }

    // Top
    // for (int z = 1; z < zSize; z++) {
		// 	for (int x = 1; x < xSize; x++) {
		// 		vertices[v++] = new Vector3(x, ySize, z);
		// 	}
		// }

    // Bottom
		// for (int z = 1; z < zSize; z++) {
		// 	for (int x = 1; x < xSize; x++) {
		// 		vertices[v++] = new Vector3(x, 0, z);
		// 	}
		// }
    /**/

    mesh.vertices = vertices;

    yield return CreateTriangles();
  }

  private IEnumerator CreateTriangles() {
    WaitForSeconds wait = new WaitForSeconds(0.05f);

    Log("Creating triangles");

    /**/
    int quads = (xSize * ySize + xSize * zSize + ySize * zSize) * 2;
    int[] triangles = new int[quads * 6];
    int t = 0, v = 0;
    int ring = xSize * 2;

    Log("Number of triangles: " + triangles.Length);

    for (int y = 0; y < ySize; y++, v++) {
      for (int q = 0; q < ring - 1; q++, v++) {
        t = SetQuad(triangles, t, v, v + 1, v + ring, v + ring + 1);

        mesh.triangles = triangles;
        yield return wait;
      }
      t = SetQuad(triangles, t, v, v - ring + 1, v + ring, v + 1);

      mesh.triangles = triangles;
      yield return wait;
    }

    yield return CreateTopFace(triangles, t, ring);

    yield return wait;
  }

  private IEnumerator CreateTopFace(int[] triangles, int t, int ring) {
    WaitForSeconds wait = new WaitForSeconds(0.05f);

    int v = ring * ySize;

    Log("CreateTopFace :: Initial v: " + v);
    Log("CreateTopFace :: Ring: " + ring);

    for (int i = 1; i < xSize; i++, v++) {
      t = SetQuad(triangles, t, v, v + 1, (ring * (ySize + 1)) - i, (ring * (ySize + 1)) - (i + 1));
      mesh.triangles = triangles;
      yield return wait;
    }

    mesh.triangles = triangles;
    yield return wait;

    // return t;
    yield return CreateBottomFace(triangles, t, ring);
  }

  private IEnumerator CreateBottomFace(int[] triangles, int t, int ring) {
    WaitForSeconds wait = new WaitForSeconds(0.05f);

    int v = 0;

    Log("CreateBottomFace :: Initial v: " + v);
    Log("CreateBottomFace :: Ring: " + ring);

    for (int i = 1; i < xSize; i++, v++) {
      t = SetQuad(triangles, t, v + 1, v, (ring * ySize) - (i + 1), (ring * ySize) - i);
      mesh.triangles = triangles;
      yield return wait;
    }

    mesh.triangles = triangles;
    yield return wait;

    // return t;
    yield return wait;
  }

  private static int SetQuad(int[] triangles, int i, int v00, int v10, int v01, int v11) {
    Log("SetQuad :: i:" + i + ", v00:" + v00 + ", v10:" + v10 + ", v01:" + v01 + ", v11:" + v11);

    triangles[i] = v00;
    triangles[i + 1] = triangles[i + 4] = v01;
    triangles[i + 2] = triangles[i + 3] = v10;
    triangles[i + 5] = v11;

    return i + 6;
  }

  private void OnDrawGizmos() {
    if (vertices == null) {
      return;
    }

    for (int i = 0; i < vertices.Length; i++) {
      Gizmos.color = Color.black;
      Gizmos.DrawSphere(vertices[i], 0.3f);

      // Gizmos.color = Color.yellow;
      // Gizmos.DrawRay(vertices[i], normals[i]);
    }
  }

  private static void Log(string message) {
    Debug.Log("[ProcGen] SplineMesh :: " + message);
  }
}
