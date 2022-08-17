using UnityEngine;
using System;

//public enum SpeakerID { Tutorial, Player, Postie, OldMan, QueenBee }

[Serializable]
public class ProgressStatus {
  public string StatusDesciption;
  public DialogueThread diaThread;
  public SmallTalkThread[] stThreads;
}
