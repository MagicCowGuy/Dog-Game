using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdControl : MonoBehaviour
{

    public int birdCount;
    public float birdDelay;
    private int maxBirds = 5;

    public GameObject birdPrefab;
    public GameObject playerObj;

    // Start is called before the first frame update
    public void LoadWithTime(float timeLapsed)
    {
      int birdsSinceSave = (int) Mathf.Floor(timeLapsed / 5);
      birdCount = PlayerPrefs.GetInt("birdCount") + birdsSinceSave;
      birdCount = Mathf.Clamp(birdCount, 0, maxBirds);
      print("It's been " + timeLapsed + " seconds since las save, when you had " + PlayerPrefs.GetInt("birdCount") + " birds already.");
      //birdCount =  , 0 , maxBirds);
      print("SPAWN " + birdCount + " Birds now on the ground." );
      print("TESTING BIRD THING");
      FastPopulate(birdCount);
      birdDelay = Random.Range(125,280);
    }

    // Update is called once per frame
    void Update()
    {
      if(birdCount < maxBirds){
        birdDelay -= 0.1f;

        if(birdDelay < 0){
          FastPopulate(1);
          birdCount += 1;
          birdDelay = Random.Range(125,280);
        }
      }
    }

    void FastPopulate (int birdsToAdd){
      for (int i = 0; i < birdsToAdd; i++)
        {
          Vector3 RdmSpawnPos = new Vector3(Random.Range(-19,16), 2.5f, Random.Range(-10,1));
          GameObject birdspawn = (GameObject)Instantiate (birdPrefab, RdmSpawnPos, Quaternion.Euler (0, 0, 0));
          birdspawn.GetComponent<BirdAI>().playerTransform = playerObj.transform;
          print("Spawning Bird!");
        }
    }

    void BirdFlyIn () {

    }

    void OnApplicationQuit()
    {
      PlayerPrefs.SetInt("birdCount", Mathf.Clamp(birdCount, 0, maxBirds));
    }
}
