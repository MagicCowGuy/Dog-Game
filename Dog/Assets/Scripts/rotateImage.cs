using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateImage : MonoBehaviour
{

  public float rotSpeed;
  private RectTransform rt;

    // Start is called before the first frame update
    void Start()
    {
          rt = this.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rt.Rotate(new Vector3(0,0,-rotSpeed));
    }
}
