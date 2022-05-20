using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face_Cam_XZ : MonoBehaviour
{

    private Quaternion camDirection;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      camDirection = Quaternion.LookRotation(transform.position - Camera.main.transform.position, Vector3.up);
      transform.eulerAngles = new Vector3(0, camDirection.eulerAngles.y, 0);
    }
}
