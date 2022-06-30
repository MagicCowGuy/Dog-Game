using UnityEngine;
using System;

//public enum SpeakerID { Tutorial, Player, Postie, OldMan, QueenBee }

[Serializable]
public class ProgressChapter {
  public string ChapterDescription;
  public ProgressStatus[] progressStatus;
}
