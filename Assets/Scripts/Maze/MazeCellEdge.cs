using UnityEngine;

public class MazeCellEdge : MonoBehaviour {
  public MazeCell cell, otherCell;
  public MazeDirection direction;
  
  public void Initialise(MazeCell cell, MazeCell otherCell, MazeDirection direction) {
    this.cell = cell;
    this.otherCell = otherCell;
    this.direction = direction;
    
    // cell.SetEdge(direction, this);
    transform.parent = cell.transform;
    transform.localPosition = Vector3.zero;
  }
}