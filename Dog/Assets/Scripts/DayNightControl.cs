using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DayNightControl : MonoBehaviour
{

    public Gradient SunColourCycle;

    [Range(0.0f, 1.0f)]
    public float timeSlider = 0.5f;

    public GameObject sunPivotParent;
    public GameObject sunObject;
    public Volume nightPost;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        nightPost.weight = SunColourCycle.Evaluate(timeSlider).a;


      sunObject.GetComponent<Light>().color = SunColourCycle.Evaluate(timeSlider);
      //sunObject.GetComponent<Light>().shadowStrength = SunColourCycle.Evaluate(timeSlider).a/2;
      timeSlider += 0.0001f;
      if(timeSlider > 1) timeSlider = 0;
    }

    void SunMoonMovement(){
      if(timeSlider >= 0.25f && timeSlider <= 0.75f){
        sunPivotParent.transform.eulerAngles = new Vector3(0,0,-timeSlider*180+90);
        nightPost.weight = 0;
      } else {
        if(timeSlider<0.5){
          sunPivotParent.transform.eulerAngles = new Vector3(0,0,timeSlider*180);
        } else {
          sunPivotParent.transform.eulerAngles = new Vector3(0,0,timeSlider*180-180);
        }
      }
    }
}
