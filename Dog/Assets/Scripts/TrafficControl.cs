using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficControl : MonoBehaviour
{
    public GameObject carObj;
    public float speedLimit;
    public GameObject[] RandomCarPool;
    public GameObject[] QueCars;
    public float spawnCooldown;
    public Vector3 spawnLocation;
    // Start is called before the first frame update
    void Start()
    {
      spawnCooldown = Random.Range(50,100);
    }

    // Update is called once per frame
    void Update()
    {
      spawnCooldown -= 0.5f;
      if(spawnCooldown <= 0){
        spawnCooldown = Random.Range(50,200);
          GameObject trafficSpawn = (GameObject)Instantiate (RandomCarPool[Random.Range(0,RandomCarPool.Length)], spawnLocation + new Vector3(0,0,Random.Range(-2,2)), Quaternion.Euler (0, 0, 0));
          //print("Spawning Traffic");
      }
    }
}
