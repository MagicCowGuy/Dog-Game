using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildControl : MonoBehaviour {

	public bool buildMode = false;
	public expandMenu menuPannel;
	public GameObject buildPannel;
	public GameObject buildOptionsPannel;
	public GameObject buildLocationsFY;
	public CamTarget mainCameraScript;
	public TouchMovement movementScript;
	public Canvas canvas;

	private int tempitemsize;
	private int tempitemprefabcode;
	private placementLocation tempPlaceScript;
	private placementLocation tempHighlightScript;
	private placementLocation selectedAreaScript;
	public GameObject[] placementAreas;
	private int[] placementSaveArray;
	public List<int> placementSaveList;

	public bool placing = false;
	//placing sizes
	public bool placeLarge;
	public bool placeSmall;

	private int numberOfPlacements;

	public GameObject[] prefabIndex;

	// Use this for initialization
	void Start () {
		buildPannel.SetActive(false);
		buildOptionsPannel.SetActive(false);
		buildLocationsFY.SetActive(false);

		numberOfPlacements = placementAreas.Length;
		placementSaveArray = new int[numberOfPlacements];

		LoadBuild();

        //placementAreas = GameObject.FindGameObjectsWithTag("ItemArea");


    }

    void Awake () {
    }

	public void ActivateBuildMode () {
		menuPannel.menuHide();
		//menuPannel.SetActive(false);
		mainCameraScript.cameraMode = 2;
		buildPannel.SetActive(true);
		buildPannel.GetComponent<Animator>().SetTrigger("Open");
		buildLocationsFY.SetActive(true);
		movementScript.moveable = false;
		buildMode = true;
	}

	public void DeactivateBuildMode () {

		PAHighlighting(0);
		menuPannel.menuShow();
		//menuPannel.SetActive(true);
		mainCameraScript.cameraMode = 1;
		//buildPannel.SetActive(false);
		buildPannel.GetComponent<Animator>().SetTrigger("Close");
		buildOptionsPannel.SetActive(false);
		buildLocationsFY.SetActive(false);
		movementScript.moveable = true;
		buildMode = false;
	}

	public void DisableBuildPannel (){
		buildPannel.SetActive(false);

	}

	public void SelectPlacementItem (GameObject bmenuitemobj) {
		tempitemsize = bmenuitemobj.GetComponent<BuildMenuIcon>().itemsizecode;
		tempitemprefabcode = bmenuitemobj.GetComponent<BuildMenuIcon>().itemprefabcode;
		PAHighlighting(tempitemsize);
        SaveBuild();
    }

	public void PAHighlighting(int sizeCode){

		placementAreas = GameObject.FindGameObjectsWithTag("ItemArea");
		foreach (GameObject placementArea in placementAreas){
			tempHighlightScript = placementArea.GetComponent<placementLocation>();
				//highlight possible placement areas
				if (sizeCode == tempHighlightScript.sizeCode && !tempHighlightScript.occupiedSpace){
					tempHighlightScript.highlightArea();
				} else {
					tempHighlightScript.unhighlightArea();
				}
			}
			buildOptionsPannel.SetActive(false);

		}

		public void ClearHighlights(){
			foreach (GameObject placementArea in placementAreas){
				tempHighlightScript = placementArea.GetComponent<placementLocation>();
				tempHighlightScript.unhighlightArea();
			}
		}

	public void SaveBuild (){
		print("Saving players object placements");
		for(int i = 0; i < placementAreas.Length; i++){
			tempPlaceScript = placementAreas[i].GetComponent<placementLocation>();
			placementSaveList[i] = tempPlaceScript.saveArrayCode;
		}
		PlayerPrefsX.SetIntArray ("SavePlacedObjects", placementSaveList.ToArray());
        PlayerPrefs.Save();
    }

	public void LoadBuild (){
		//Load playerprefsX save into save Array
		placementSaveArray = PlayerPrefsX.GetIntArray ("SavePlacedObjects", 0, numberOfPlacements);

		//Set up List with a count of the number of placements currently in the game
		for (int i = 0; i<numberOfPlacements; i++){
			if(placementSaveArray.Length > i){
				placementSaveList.Add(placementSaveArray[i]);
			}else{
				placementSaveList.Add(0);
				print("EXTENDING TO SIZE");
			}

		}

		for(int i = 0; i < numberOfPlacements; i++){
			tempPlaceScript = placementAreas[i].GetComponent<placementLocation>();
			if(placementSaveList[i] != 0 && tempPlaceScript.occupiedSpace == false){
				tempPlaceScript.selectArea(prefabIndex[placementSaveList[i]], placementSaveList[i]);
			}
		}
	}

public void ClearLocation (){
	Destroy(selectedAreaScript.placedObject);
	selectedAreaScript.occupiedSpace = false;
	selectedAreaScript.saveArrayCode = 0;
	SaveBuild();
	ClearHighlights();
	selectedAreaScript = null;
	buildOptionsPannel.SetActive(false);
}

public Vector3 worldToUISpace(Canvas parentCanvas, Vector3 worldPos)
{
    //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
    Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
    Vector2 movePos;

    //Convert the screenpoint to ui rectangle local point
    RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);
    //Convert the local point to world point
    return parentCanvas.transform.TransformPoint(movePos);
}

private bool IsPointerOverUIObject() {
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
		eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
		return results.Count > 0;
}

void Update () {

		if(!buildMode){return;}

		if(Input.GetMouseButtonUp(0)){
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			int layerMask = LayerMask.GetMask ("ItemAreas");
			if (Physics.Raycast (ray, out hit, 100, layerMask)){
				tempPlaceScript = hit.collider.gameObject.GetComponent<placementLocation>();
				if(tempPlaceScript.occupiedSpace){
					ClearHighlights();
					tempPlaceScript.highlightArea();
					selectedAreaScript = tempPlaceScript;
					//activate pannel for options
					buildOptionsPannel.SetActive(true);
					//print(worldToUISpace(canvas,hit.collider.gameObject.transform.position));
					buildOptionsPannel.GetComponent<RectTransform>().position = worldToUISpace(canvas,hit.collider.gameObject.transform.position);
				}
				if(!tempPlaceScript.highlighted){
					print("Deselect");
					PAHighlighting(0);
					tempitemprefabcode = 0;
					buildOptionsPannel.SetActive(false);
					return;
				}
				if(!tempPlaceScript.occupiedSpace && tempitemprefabcode != 0 && tempPlaceScript.highlighted){
					tempPlaceScript.selectArea(prefabIndex[tempitemprefabcode], tempitemprefabcode);
					tempitemprefabcode = 0;
					PAHighlighting(0);
					SaveBuild();
				}

			} else {
				//Ignore clicks on GUI
				if (EventSystem.current.IsPointerOverGameObject()) return;
				if (EventSystem.current.IsPointerOverGameObject() || EventSystem.current.currentSelectedGameObject != null)	return;
				if (IsPointerOverUIObject()) return;

				print("Deselect");
				PAHighlighting(0);
				tempitemprefabcode = 0;
				buildOptionsPannel.SetActive(false);

			}
		}
	}
}
