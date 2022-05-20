using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour {

    public PickupProperties currentPickup;
    public PickupProperties emptyPickup;
    public Image pickupimage;
    private GameObject playerinfoobj;

	// Use this for initialization
	void Start () {
        //playerinfoobj = GameObject.Find("PlayerInfo");
        //playerinfoobj.GetComponent<PlayerInfo>().currentPickup = gameObject;
        //pickupimage.sprite = currentPickup.artwork;
        pickupimage.sprite = currentPickup.artwork;
    }
	
    public void ItemChange(PickupProperties newPickup)
    {
        pickupimage.sprite = newPickup.artwork;
    }

    public void ItemDrop()
    {
        pickupimage.sprite = emptyPickup.artwork;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
