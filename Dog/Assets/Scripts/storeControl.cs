using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class storeControl : MonoBehaviour
{
	public List <StoreItem> allStoreItems;
	public List <bool> itemOwned;
	public GameObject storeItemPrefab;
	public GameObject storeContentPanel;

	public expandMenu menuPannel;
	public GameObject storePannel;

	public UIBgControl uIBg;

	private Scoring scoreScript;
	
	// Start is called before the first frame update
	void Start()
	{
		scoreScript = this.GetComponent<Scoring>();
		uIBg = this.GetComponent<UIBgControl>();
		PopulateStoreList();
		PopulateStorePanel();
	}

	private void PopulateStoreList() {
		Object[] resourceStoreItems = Resources.LoadAll("Store", typeof(StoreItem));
		foreach(StoreItem sItem in resourceStoreItems) {
		allStoreItems.Add(sItem);
		itemOwned.Add(false);
	  }
	  allStoreItems.Sort(SortByItemCode);
	}

	static int SortByItemCode(StoreItem t1, StoreItem t2) {
		return t1.itemCode.CompareTo(t2.itemCode);
	}

	private void PopulateStorePanel() {
		foreach (StoreItem sItem in allStoreItems){
			GameObject newStoreItem = Instantiate(storeItemPrefab, transform);
			newStoreItem.transform.SetParent(storeContentPanel.transform);
			newStoreItem.transform.localScale = Vector3.one;

			newStoreItem.GetComponent<storeItemScript>().storeConScript = this.GetComponent<storeControl>();
			newStoreItem.GetComponent<storeItemScript>().setupStoreItem(sItem);
		}
	}
	
	public void purchaseItem(int purCode, storeItemScript guiItemScript) {
		
		for (int i = 0; i < allStoreItems.Count; i++) {
			if(allStoreItems[i].itemCode == purCode){
				if(itemOwned[i] == false){
					if(allStoreItems[i].itemCost <= scoreScript.coinNumber){
						print("YOU JUST BOUGHT ITEM #" + i);
						scoreScript.ScoreUpdate(-allStoreItems[i].itemCost);
					} else {
						print("You can't afford that item");
					}
				} else {
					print("Already purchased that item");
				}
			}
		}
	}

	public void ShowStoreUI () {
		menuPannel.menuHide();
		storePannel.SetActive(true);
		uIBg.fullOut();
		//blackoutPannel.SetActive(true);
	}

	public void HideStoreUI () {
		menuPannel.menuShow();
		storePannel.SetActive(false);
		uIBg.noneOut();
		//blackoutPannel.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
		
	}
}
