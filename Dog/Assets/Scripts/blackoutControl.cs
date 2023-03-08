using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBgControl : MonoBehaviour
{
	public GameObject bgPannel;
	public Sprite botGrad;
	public Image bgImg;
	
	public Color colourDefault;
	public Color colourClear;
	public Color colourTask;

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	void Awake(){
		bgImg = bgPannel.GetComponent<Image>();
	}

	public void halfOut() {
		bgPannel.SetActive(true);
		bgImg.sprite = botGrad;
		bgImg.color = colourDefault;
	}

	public void fullOut() {
		bgPannel.SetActive(true);
		bgImg.sprite = null;
		bgImg.color = colourDefault;
		
	}

	public void clearOut() {
		bgPannel.SetActive(true);
		bgImg.sprite = null;
		bgImg.color = colourClear;
	}

	public void noneOut() {
		bgPannel.SetActive(false);
	}
}
