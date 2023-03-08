using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Store Item", menuName = "Store Item")]
public class StoreItem : ScriptableObject
{

	public int itemCode;
	public string itemName;
	public string itemDes;
	public Sprite itemIcon;
	public Sprite itemIconAlt;

	public int itemCost;
	public string[] itemTags;
}