using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest")]
public class Quest : ScriptableObject
{

  public new string name;
  public string description;
  public Sprite artwork;
  public int type;
  public int rewardAmount;

  public bool completed;
  public int targetAmount;
  public int progressAmount;

  public int SaveCode;
  //public int[] ExpireDate;
  //public int[] ExpireTime;
}
