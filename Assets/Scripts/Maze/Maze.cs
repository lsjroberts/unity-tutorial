﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Maze : MonoBehaviour {
	 public IntVector2 size;
   public MazeCell cellPrefab;
   public float generationStepDelay;
   
   private MazeCell[,] cells;
   
   public IEnumerator Generate() {
     WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
     
     cells = new MazeCell[size.x, size.z];
     List<MazeCell> activeCells = new List<MazeCell>();
     DoFirstGenerationStep(activeCells);
     while (activeCells.Count > 0) {
       yield return delay;
       DoNextGenerationStep(activeCells);
     }
   }
   
   private void DoFirstGenerationStep(List<MazeCell> activeCells) {
     activeCells.Add(CreateCell(RandomCoordinates));
   }
   
   private void DoNextGenerationStep(List<MazeCell> activeCells) {
     int currentIndex = activeCells.Count - 1;
     MazeCell currentCell = activeCells[currentIndex];
     MazeDirection direction = MazeDirections.RandomValue;     
     IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();
     
     if (ContainsCoordinates(coordinates) && GetCell(coordinates) == null) {
       activeCells.Add(CreateCell(coordinates));
     } else {
       activeCells.RemoveAt(currentIndex);
     }
   }
   
   private MazeCell CreateCell(IntVector2 coordinates) {
     MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
     cells[coordinates.x, coordinates.z] = newCell;
     
     newCell.coordinates = coordinates;
     newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
     newCell.transform.parent = transform;
     newCell.transform.localPosition = new Vector3(
       coordinates.x - size.x * 0.5f + 0.5f, 0f,
       coordinates.z - size.z * 0.5f + 0.5f
     );
     
     return newCell;
   }
   
   public IntVector2 RandomCoordinates {
     get {
       return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
     }
   }
   public bool ContainsCoordinates(IntVector2 coordinates) {
     return coordinates.x >= 0 && coordinates.x < size.x && coordinates.z >= 0 && coordinates.z < size.z;
   }
   
   public MazeCell GetCell(IntVector2 coordinates) {
     return cells[coordinates.x, coordinates.z];
   }
}
