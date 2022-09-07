using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class TaskControl : MonoBehaviour
{

  public List <Task> taskPrefabList;
  public List <bool> taskUnlockList;
  public List <bool> taskCompList;
  public List <int> taskProgList;
  public List <float> taskTimeReset;
  public List <TaskSmallPanel> taskSmlPanelList;

  public int[] saveTaskProg;
  public List <int> saveTaskUnlockList;
  //private int[] saveTaskUnlock;

  public GameObject taskMiniPrefab;
  public TextMeshProUGUI taskDescText;

  public GameObject taskListPanel;
  public GameObject taskListContentPanel;
  public GameObject TaskUnlockPanel;
  public TaskSmallPanel taskDisplayUnlock;

  public GameObject blackoutPanel;
  public expandMenu menuPanel;
  public DialogueManager dioManager;

    void Awake()
    {

      PopulateToDoPrefabs();
      PopulateToDoPanel();
      loadTaskData();
      
      StartCoroutine("TimeTicker");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenTaskList(){
      menuPanel.menuHide();
  		taskListPanel.SetActive(true);
      blackoutPanel.SetActive(true);
    }

    public void CloseTaskList(){
      menuPanel.menuShow();
  		taskListPanel.SetActive(false);
      blackoutPanel.SetActive(false);
    }

    public void showTaskUnlock(Task taskToUnlock) {
      menuPanel.menuHide();
  		TaskUnlockPanel.SetActive(true);
      blackoutPanel.SetActive(true);
      taskDisplayUnlock.unlock();
      taskDisplayUnlock.StartCoroutine("floatyTaskEffect");
      taskDisplayUnlock.taskPrefab = taskToUnlock;
      taskDisplayUnlock.contentUpdate();
      taskDescText.text = taskToUnlock.taskDes;

      for (int i = 0; i<taskPrefabList.Count; i++){
        if(taskPrefabList[i].taskCode == taskToUnlock.taskCode){
          taskUnlockList[i] = true;
        }
      }
      saveTaskData();
      UpdateTaskPanel();
    }

    public void TaskProgress(int tcode, int tprog) {
      for (int i = 0; i<taskPrefabList.Count; i++){
        if(taskPrefabList[i].taskCode == tcode){
          taskProgList[i] += tprog;
          
        }
      }
      //Check if you completed the task
      CompleteCheck();
      saveTaskData();     
    }

    IEnumerator TimeTicker() {
      while(true){
        for (int i = 0; i<taskPrefabList.Count; i++){
          if(taskCompList[i] == true && taskTimeReset[i] != 0){
            taskTimeReset[i] -= 1;

            if(taskTimeReset[i] <= 0){
              taskCompList[i] = false;
              taskProgList[i] = 0;
              taskSmlPanelList[i].reset();
              print(taskPrefabList[i].taskName + " is now available to progress through again!");
            } else {
              if(taskTimeReset[i] < 60){
                taskSmlPanelList[i].timeUpdate(Mathf.Round(taskTimeReset[i] * 10)/10 + " sec");
              }
              if(taskTimeReset[i] >= 60 && taskTimeReset[i] < 3600){
                taskSmlPanelList[i].timeUpdate(Mathf.FloorToInt(taskTimeReset[i]/30)/2.0f + " min");
              }
              if(taskTimeReset[i] >= 3600 && taskTimeReset[i] < 86400){
                taskSmlPanelList[i].timeUpdate(Mathf.FloorToInt(taskTimeReset[i]/1800)/2.0f + " hrs");
              }
              if(taskTimeReset[i] >= 86400){
                taskSmlPanelList[i].timeUpdate(Mathf.FloorToInt(taskTimeReset[i]/43200)/2.0f + " days");
              }
            }
          }
        }
        yield return new WaitForSeconds(1);
      }
      yield return null;
    }

    public void UpdateTaskPanel() {
      for (int i = 0; i<taskSmlPanelList.Count; i++){
        if(taskUnlockList[i] == true){
          taskSmlPanelList[i].unlock();
        }
      }
    }

    private void CompleteCheck() {
      for (int i = 0; i<taskPrefabList.Count; i++){
        if(taskProgList[i] >= taskPrefabList[i].targetCount){
          if(taskCompList[i] == false && taskTimeReset[i] == 0){
            print("ITS A NEW COMPLETION FOR: "+ taskPrefabList[i].taskName);
            //ADD TIME COOLDOWN or set the time.time+cooldown as the value?
            taskTimeReset[i] = 140f;
          }
          taskCompList[i] = true;
          taskSmlPanelList[i].complete();
        } else {
          if(taskProgList[i] > 0){
            taskSmlPanelList[i].progUpdate((taskProgList[i] *2.0f)/(taskPrefabList[i].targetCount * 2.0f));
          }
        }
      }
    }

    public void closeTaskUnlock() {
      menuPanel.menuShow();
      TaskUnlockPanel.SetActive(false);
      blackoutPanel.SetActive(false);
      dioManager.currentNPC.GetComponent<NPC>().ReleaseNPC();
    }

    static int SortByCode(Task t1, Task t2) {
      return t1.taskCode.CompareTo(t2.taskCode);
    }

    private void PopulateToDoPrefabs() {
      Object[] subToDoListTasks = Resources.LoadAll("Tasks", typeof(Task));
      foreach(Task subToDo in subToDoListTasks) {
        taskPrefabList.Add(subToDo);
        taskUnlockList.Add(false);
        taskCompList.Add(false);
        taskProgList.Add(0);
        taskTimeReset.Add(0);
      }
      taskPrefabList.Sort(SortByCode);
    }

    private void PopulateToDoPanel() {
      foreach (Task todoPrefab in taskPrefabList){
        GameObject newTaskMiniPanel = Instantiate (taskMiniPrefab, transform);
        newTaskMiniPanel.transform.SetParent(taskListContentPanel.transform);
        newTaskMiniPanel.transform.localScale = Vector3.one;

        TaskSmallPanel todoPrefabScript = newTaskMiniPanel.GetComponent<TaskSmallPanel>();
        todoPrefabScript.taskPrefab = todoPrefab;
        todoPrefabScript.contentUpdate();

        taskSmlPanelList.Add(newTaskMiniPanel.GetComponent<TaskSmallPanel>());
      }
    }

    private void saveTaskData() {
      //Convert bool to int
      saveTaskUnlockList.Clear();
      for (int i = 0; i<taskUnlockList.Count; i++){
        if(taskUnlockList[i] == true) {
          saveTaskUnlockList.Add(1);
        } else {
          saveTaskUnlockList.Add(0);
        }
      }
      saveTaskProg = taskProgList.ToArray();

      PlayerPrefsX.SetIntArray ("TaskProgress", saveTaskProg);
      PlayerPrefsX.SetIntArray ("TaskUnlock", saveTaskUnlockList.ToArray());
    }

    private void loadTaskData() {
      taskProgList = PlayerPrefsX.GetIntArray("TaskProgress", 0, taskPrefabList.Count).ToList();
      saveTaskUnlockList = PlayerPrefsX.GetIntArray("TaskUnlock", 0, taskPrefabList.Count).ToList();
      //taskProgList = saveTaskProg.ToList();

      for (int i = 0; i<saveTaskUnlockList.Count; i++){
        if(saveTaskUnlockList[i] == 1) {
          taskUnlockList[i] = true;
        } else {
          taskUnlockList[i] = false;
        }
      }
      CompleteCheck();
      UpdateTaskPanel();
    }
    
}
