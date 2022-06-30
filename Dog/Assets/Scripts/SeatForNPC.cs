using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif // UNITY_EDITOR

public class SeatForNPC : MonoBehaviour
{
  public SeatPoint[] seatSpots;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}



#if UNITY_EDITOR
[CustomEditor(typeof(SeatForNPC))]
public class SeatForNPC_Editor : Editor
{
  private Vector3 curPos;

	public void OnSceneGUI()
	{
		var LinkSeatScript = target as SeatForNPC;
    curPos = LinkSeatScript.gameObject.transform.position;
    drawPoints(LinkSeatScript.seatSpots);

	}

	public void drawPoints(SeatPoint[] pointsToDraw){
		for(int i = 0; i < pointsToDraw.Length; i++){
			EditorGUI.BeginChangeCheck();
			Vector3 newPoint = Handles.PositionHandle(pointsToDraw[i].seatPointPos + curPos, Quaternion.identity);
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(target, "Update Sitting Spot Point");
				pointsToDraw[i].seatPointPos = newPoint - curPos;
			}
		}

	}

}
#endif // UNITY_EDITOR
