using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskSmallPanel : MonoBehaviour
{

  public TextMeshProUGUI taskTitleText;
  public TextMeshProUGUI taskTimeText;
  public Image taskImage;
  public GameObject unlockOverlay;

  public Task taskPrefab;

  public bool floatyTask;
  private float rotFlo;
  public float rdmOffset;
  private Vector3 posVect;
  public RectTransform rtobject;

    // Start is called before the first frame update
    void Awake()
    {
      rtobject = this.GetComponent<RectTransform>();
      rdmOffset = Random.Range(0,100);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void unlock(){
      unlockOverlay.SetActive(false);
    }

    public void contentUpdate() {
      taskTitleText.text = taskPrefab.taskName;
      taskTimeText.text = "0";
      taskImage.sprite = taskPrefab.taskIcon;
      LayoutRebuilder.ForceRebuildLayoutImmediate(taskTimeText.GetComponent<RectTransform>());
      this.GetComponent<RectTransform>().transform.eulerAngles = new Vector3(0,0,Random.Range(-3, 3));
    }

    public IEnumerator floatyTaskEffect() {
      floatyTask = true;
      while(floatyTask){
        rotFlo = (Mathf.Sin(Time.time + rdmOffset/10)) * (3 + rdmOffset / 50);
        rtobject.rotation = Quaternion.Euler(0,0,rotFlo);
        rtobject.pivot = new Vector2(Mathf.Sin(Time.time + rdmOffset)/50 + 0.5f,Mathf.Cos(Time.time + rdmOffset)/50 + 0.5f);
        yield return null;
      }
      yield break;
    }

}
