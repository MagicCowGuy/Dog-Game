using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pickup Properties", menuName = "Pickup Properties")]
public class PickupProperties : ScriptableObject {

	public new string name;
	public string description;
	public string hint;

	public Sprite artwork;

	public bool eatable;
	public bool droppable;
	public bool destroyable;

}
