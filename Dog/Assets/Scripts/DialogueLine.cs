using UnityEngine;
using System;

public enum SpeakerID { Tutorial, Player, Postie, OldMan, QueenBee }

[Serializable]
public class DialogueLine {
  public SpeakerID diaSpeaker;
  public string diaLine;
}
