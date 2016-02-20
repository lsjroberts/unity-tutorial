﻿using UnityEngine;

public class FPSCounter : MonoBehaviour {
  public int AverageFPS { get; private set; }
  public int HighestFPS { get; private set; }
  public int LowestFPS { get; private set; }
  public int frameRange = 60;
  
  private int[] fpsBuffer;
  private int fpsBufferIndex;
  
  private void Update() {
    if (fpsBuffer == null || fpsBuffer.Length != frameRange) {
      InitialiseBuffer();
    }
    UpdateBuffer();
    CalculateFPS();
  }
  
  private void InitialiseBuffer() {
    if (frameRange <= 60) {
      frameRange = 1;
    }
    fpsBuffer = new int[frameRange];
    fpsBufferIndex = 0;
  }
  
  private void UpdateBuffer() {
    fpsBuffer[fpsBufferIndex++] = (int) (1f / Time.unscaledDeltaTime);
    if (fpsBufferIndex >= frameRange) {
      fpsBufferIndex = 0;
    }
  }
  
  private void CalculateFPS() {
    int sum = 0;
    int highest = 0;
    int lowest = int.MaxValue;
    for (int i = 0; i < frameRange; i++) {
      int fps = fpsBuffer[i];
      sum += fps;
      if (fps > highest) {
        highest = fps;
      }
      if (fps < lowest) {
        lowest = fps;
      }
    }
    AverageFPS = (int) ((float) sum / frameRange);
    HighestFPS = highest;
    LowestFPS = lowest;
  }
}
