using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class storeItemScript : MonoBehaviour
{
	public int itemCode;
	public StoreItem itemInfo;
	public bool unlocked;
	public bool purchased;
	
	public TextMeshProUGUI itemHead;
	public TextMeshProUGUI itemPrice;
	public Image itemImg;

	public storeControl storeConScript;

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public void itemTap() {
		storeConScript.purchaseItem(itemCode, this.GetComponent<storeItemScript>());
	}

	public void setupStoreItem(StoreItem setupItemInfo){
		itemInfo = setupItemInfo;
		itemCode = itemInfo.itemCode;
		itemHead.text = itemInfo.itemName;
		itemPrice.text = itemInfo.itemCost.ToString();
		itemImg.sprite = itemInfo.itemIconAlt;
	}
}
