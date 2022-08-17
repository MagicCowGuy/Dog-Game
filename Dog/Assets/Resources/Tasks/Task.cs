using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Task", menuName = "Task")]
public class Task : ScriptableObject
{

  public int taskCode;
  public string taskName;
  public string taskDes;
  public string GuideDes;

  public Sprite taskIcon;
  public Sprite[] supportImages;

  public bool unlocked;
  public string[] unlockReq;

  public int rewardAmount;
  public bool completed;
  public int targetCount;
  public int progressCount;

  public bool dailyRefresh;
  public float hoursCooldown;
  public float timeToReset;
}
