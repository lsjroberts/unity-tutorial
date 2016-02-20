using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Stuff : PooledObject {
  public Rigidbody Body { get; private set; }
  
  private MeshRenderer[] meshRenderers;
  
  public void SetMaterial(Material m) {
    for (int i = 0; i < meshRenderers.Length; i++) {
      meshRenderers[i].material = m;
    }
  }
  
  private void Awake() {
    Body = GetComponent<Rigidbody>();
    meshRenderers = GetComponentsInChildren<MeshRenderer>();
  }
  
  private void OnTriggerEnter(Collider enteredCollider) {
    if (enteredCollider.CompareTag("Kill Zone")) {
      ReturnToPool();
    }
  }
}
