using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveSprite : MonoBehaviour
{
    public float spritesize;
    public float sizechange;
    public float maxsize;

    // Update is called once per frame
    void Update()
    {
      transform.localScale = new Vector3(spritesize,spritesize,1);
      if (spritesize > maxsize) {
        Destroy(gameObject);
      }
      spritesize += sizechange;
    }
}
