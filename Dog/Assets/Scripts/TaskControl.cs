using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class TaskControl : MonoBehaviour
{

  public List <Task> toDoTaskPrefabs;
  public List <bool> taskUnlockList;
  public List <bool> taskCompList;
  public List <int> taskProgList;

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
        toDoTaskPrefabs.Add(subToDo);
      }
      toDoTaskPrefabs.Sort(SortByCode);
    }

    private void PopulateToDoPanel() {
      foreach (Task todoPrefab in toDoTaskPrefabs){
        GameObject newTaskMiniPanel = Instantiate (taskMiniPrefab, transform);
        newTaskMiniPanel.transform.SetParent(taskListContentPanel.transform);
        newTaskMiniPanel.transform.localScale = Vector3.one;

        TaskSmallPanel todoPrefabScript = newTaskMiniPanel.GetComponent<TaskSmallPanel>();
        todoPrefabScript.taskPrefab = todoPrefab;
        todoPrefabScript.contentUpdate();
      }
    }
}
