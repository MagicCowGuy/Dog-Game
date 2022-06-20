using UnityEngine;
using System;

public enum MovementType { Walk, Run, Jump, Fly, Swim }

[Serializable]
public class PathNavPoint {
  public MovementType moveType;
  public Vector3 pathPointPos;
}
