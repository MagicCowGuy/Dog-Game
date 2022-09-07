using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Control : MonoBehaviour
{

  public List<GameObject> NPCCommonPool;
  public List<GameObject> NPCUncommonPool;
  public float cooldownUncom;
  public List<GameObject> NPCRarePool;
  public float cooldownRare;

  public List<GameObject> CurPrefabs;
  public List<GameObject> CurClones;

  private List<GameObject> tempSpawnPool;
  private GameObject tempSpwanPrefab;

  public int maxMobCount = 5;

  public bool spawning = true;

  public int CurMobCount;

    // Start is called before the first frame update
    void Start()
    {
      //NPCPrefabRollCall = new bool[NPCPrefabIndex.Length];
      //StartSpawning();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartSpawning(){
      StartCoroutine(SpawnCheckCoRo());
    }

    public void DespawnMob (GameObject MobToDespawn){
      print("Despawning this NPC - " + MobToDespawn);
      for (int i = 0; i<CurPrefabs.Count; i++){
        if(CurPrefabs[i].GetComponent<NPC>().MobCodeNo == MobToDespawn.GetComponent<NPC>().MobCodeNo){
          CurPrefabs.RemoveAt(i);
        }
      }
      Destroy(MobToDespawn);
      CurMobCount --;
    }

    void SpawnMob (GameObject NPCtoSpawn){
      GameObject newNPC = Instantiate (NPCtoSpawn, Vector3.zero, Quaternion.LookRotation(Vector3.forward));
      newNPC.GetComponent<NPC>().gameControlObj = this.gameObject;
      //NPCPrefabRollCall[prefabRef] = true;
      newNPC.GetComponent<NPC>().SpawnSetup();
      CurPrefabs.Add(NPCtoSpawn);
      CurClones.Add(newNPC);
    }

    private void buildSpawnPool(){
        
    }

    IEnumerator SpawnCheckCoRo(){

      while(true){
        print("Spawn NPC");
        //buildSpawnPool();

        //clear pool
        tempSpawnPool = new List<GameObject>();
        tempSpwanPrefab = null;

        //add common, uncommon and rares providing their cooldown is reached.
        for (int i = 0; i<NPCCommonPool.Count; i++){
          tempSpawnPool.Add(NPCCommonPool[i]);
        }

        if(cooldownUncom <= 0){
          for (int i = 0; i<NPCUncommonPool.Count; i++){
          tempSpawnPool.Add(NPCUncommonPool[i]);
          }
        }

        if(cooldownRare <= 0){
          for (int i = 0; i<NPCRarePool.Count; i++){
          tempSpawnPool.Add(NPCRarePool[i]);
          }
        }

        //remove any currently spawned NPCs from list.
        foreach (GameObject NPCprefab in CurPrefabs){
          if(tempSpawnPool.Contains(NPCprefab)){
            tempSpawnPool.Remove(NPCprefab);
          }
        }


        if(tempSpawnPool.Count > 0){
          tempSpwanPrefab = tempSpawnPool[Random.Range(0,tempSpawnPool.Count)];
          SpawnMob(tempSpwanPrefab);
          if(NPCUncommonPool.Contains(tempSpwanPrefab)){
            cooldownUncom = 20;
          }
          if(NPCRarePool.Contains(tempSpwanPrefab)){
            cooldownRare = 50;
          }
        }
        
        cooldownUncom -= 1;
        cooldownRare -= 1;
        yield return new WaitForSeconds(5);
      }

    }


}
