using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficControl : MonoBehaviour
{
    public GameObject carObj;
    public float speedLimit;
    public GameObject[] RandomCarPool;
    public GameObject[] QueCars;
    //public float spawnCooldown;
    public bool isTraffic = true;
    public Vector3 spawnLocation;
    // Start is called before the first frame update
    void Start()
    {
      StartCoroutine("spawnWaveCoRo");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator spawnWaveCoRo() {
      while(isTraffic){
        spawnTraffic();
        yield return new WaitForSeconds(Random.Range(3, 8));
      }
      yield return null;
    }

    public void spawnTraffic(){
      GameObject trafficSpawn = (GameObject)Instantiate (RandomCarPool[Random.Range(0,RandomCarPool.Length)], spawnLocation + new Vector3(0,0,Random.Range(-2,2)), Quaternion.Euler (0, 0, 0));
    }
}
