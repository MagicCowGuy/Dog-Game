using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class blackoutControl : MonoBehaviour
{
	public GameObject bgPannel;
	public Sprite botGrad;
	private Image bgImg;
	
	public Color colourDefault;
	public Color colourClear;
	public Color colourTask;

	// Start is called before the first frame update
	void Start()
	{
		bgImg = this.GetComponent<Image>();
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public void halfOut() {
		bgPannel.SetActive(true);
		bgImg.sprite = botGrad;
		bgImg.color = colourDefault;
	}

	public void fullOut(float colourOpt) {
		bgPannel.SetActive(true);
		bgImg.sprite = null;
		if(colourOpt == 1){bgImg.color = colourDefault;}
		if(colourOpt == 2){bgImg.color = colourTask;}
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
