using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficAI : MonoBehaviour
{

    public float maxSpeed;
    public GameObject throwItem;
    public Transform[] wheels;
    public Transform body;

    private Vector3 bodyStartPos;
    private Vector3 bodyOffsetPos;
    private Vector3 rdmOffset;
    void Awake()
    {
      bodyStartPos = body.transform.localPosition;
      rdmOffset = new Vector3(Random.Range(0,10),Random.Range(0,10),Random.Range(0,10));
      foreach(Transform wheel in wheels){
        wheel.Rotate(0,0,Random.Range(0,360));
      }
    }

    // Update is called once per frame
    void Update()
    {
      foreach(Transform wheel in wheels){
        wheel.Rotate(0,0,10);
      }
      //bodyOffsetPos.z = Mathf.Sin(rdmOffset.z + Time.time * 15) * 0.03f;
      bodyOffsetPos.y = Mathf.Cos(rdmOffset.y + Time.time * 16) * 0.02f;
      bodyOffsetPos.x = Mathf.Cos(rdmOffset.x + Time.time * 17) * 0.015f;
      //body.transform.position.x = Mathf.Sin(Time.time * 2) * 0.5f;
      body.transform.localPosition = bodyStartPos + bodyOffsetPos;


      transform.position += Vector3.right * maxSpeed;
      if(transform.position.x > 50){
        Destroy(this.gameObject);
      }
    }


}
