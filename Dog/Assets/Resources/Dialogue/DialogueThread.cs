using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue Thread", menuName = "Dialogue Thread")]
public class DialogueThread : ScriptableObject
{
  public DialogueLine[] dialogueThread;
  public bool importantInfo;
  public Quest[] QuestToOffer;
  public Task[] TaskUnlock;
}
