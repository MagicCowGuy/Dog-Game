using UnityEngine;
using System;

public enum SeatStatus { Open, Booked, Taken }

[Serializable]
public class SeatPoint {
  public SeatStatus seatStat;
  public Vector3 seatPointPos;
}
