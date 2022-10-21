using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cloudMovement : MonoBehaviour
{
  //public GameObject postmanObj;
  private Vector3 posloop;
    // Start is called before the first frame update
    void Start()
    {
      posloop = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        posloop.x -= Time.deltaTime * 2.5f;
        if(posloop.x < -40){posloop.x = 40;}
        transform.position = posloop;
    }
}
