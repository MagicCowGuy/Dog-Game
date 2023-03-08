using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class touchCircle : MonoBehaviour
{
	private float cooldown;
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.touchCount == 1){
			Touch touch = Input.GetTouch(0);
			this.GetComponent<RectTransform>().position = touch.position;
		}

		if(Input.GetMouseButton(0)){
			this.GetComponent<RectTransform>().position = Input.mousePosition;
			cooldown = 1;
		} else {
			cooldown -= 5 * Time.unscaledDeltaTime;
		}

		if(cooldown > 0){
			this.GetComponent<Image>().enabled = true;
		} else {
			this.GetComponent<Image>().enabled = false;
		}
	}
}
