using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Control : MonoBehaviour
{

  public GameObject[] NPCPrefabIndex;
  public bool[] NPCPrefabRollCall;
  public int maxMobCount = 5;
  public bool spawning = true;
  public int CurMobCount;
  private int TempMobSpawnNo;
    // Start is called before the first frame update
    void Start()
    {
      NPCPrefabRollCall = new bool[NPCPrefabIndex.Length];

      //StartCoroutine(SpawnCheckCoRo());

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DespawnMob (GameObject MobToDespawn){
      print("Despawning this NPC - " + MobToDespawn);
      NPCPrefabRollCall[MobToDespawn.GetComponent<NPC>().MobCodeNo] = false;
      Destroy(MobToDespawn);
      CurMobCount --;
    }

    void SpawnMob (int prefabRef){
      GameObject newNPC = Instantiate (NPCPrefabIndex[prefabRef], new Vector3(0,0,0), Quaternion.LookRotation(Vector3.forward));
      newNPC.GetComponent<NPC>().MobCodeNo = prefabRef;
      newNPC.GetComponent<NPC>().gameControlObj = this.gameObject;
      NPCPrefabRollCall[prefabRef] = true;

      newNPC.GetComponent<NPC>().SpawnSetup();
    }

    IEnumerator SpawnCheckCoRo(){
        while(true){

          bool AllTrue=true;
          for(int loop=0; loop<NPCPrefabRollCall.Length;++loop) {
            if(NPCPrefabRollCall[loop] == false) {
              AllTrue = false;
              break;
            }
          }

          if(AllTrue){print("THERES NO NPCS LEFT!!");}

          if(CurMobCount < maxMobCount && AllTrue == false){
            TempMobSpawnNo = Random.Range(0, NPCPrefabIndex.Length);
          while (NPCPrefabRollCall[TempMobSpawnNo] == true){
            //print("NPC Count is " + TempMobSpawnNo);
            TempMobSpawnNo = Random.Range(0, NPCPrefabIndex.Length);
          }


          SpawnMob(TempMobSpawnNo);
          CurMobCount ++;
        }
          //check if there are too many NPCs and spawn
          yield return new WaitForSeconds(5);
       }
    }

}
