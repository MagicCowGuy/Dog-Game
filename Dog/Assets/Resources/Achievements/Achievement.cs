using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Achievement", menuName = "Achievement")]
public class Achievement : ScriptableObject
{
  public string title;
  public string description;
  public int saveCode;
  public int progress;
  public int targetAmount;
  public int typeCode;
  public bool completed;
  public GameObject GuiPanel;
}
