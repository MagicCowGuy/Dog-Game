using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeControl : MonoBehaviour
{

    DateTime currentDate;
    DateTime oldDate;
    TimeSpan difference;

    public float tarTimeScale = 1;

    public BirdControl BirdControlScript;

    // Start is called before the first frame update
    void Start()
    {
      print("Game started at: " + System.DateTime.Now);
      currentDate = System.DateTime.Now;
        //Grab the old time from the player prefs as a long
      if(PlayerPrefs.HasKey("sysTimeString")){
        long temp = Convert.ToInt64(PlayerPrefs.GetString("sysTimeString"));

        //Convert the old time from binary to a DataTime variable
        DateTime oldDate = DateTime.FromBinary(temp);
        print("oldDate: " + oldDate);

        //Use the Subtract method and store the result as a timespan variable
        difference = currentDate.Subtract(oldDate);
        print("Difference: " + difference);
        //BirdControlScript.LoadWithTime((float)difference.TotalSeconds);

      }else{
        print("No previous close time detected, must be new save game.");
      }
      if(!PlayerPrefs.HasKey("sysOriginTimeString")){
        print("No saved origin time detected, saving current time as origin time: " + System.DateTime.Now);
        PlayerPrefs.SetString("sysOriginTimeString", System.DateTime.Now.ToBinary().ToString());
      }

      BirdControlScript.LoadWithTime((float)difference.TotalSeconds);
      print("Difference in seconds: " + difference.TotalSeconds);
    }

    // Update is called once per frame
    void Update()
    {
      Time.timeScale = Mathf.MoveTowards(Time.timeScale, tarTimeScale, 2 * Time.unscaledDeltaTime);
// NOTES TO FINISH      Time.TimeScale - Time.unscaledDeltaTime
    }

    public void setTimeScale(float newtarTimeS){
      if(newtarTimeS < tarTimeScale || newtarTimeS == 1){
        tarTimeScale = newtarTimeS;
      }
      //set new time scale if its lower than current time scale
    }

    void OnApplicationQuit()
         {
             //Savee the current system time as a string in the player prefs class
             PlayerPrefs.SetString("sysTimeString", System.DateTime.Now.ToBinary().ToString());

             print("Saving this date to prefs: " + System.DateTime.Now);
         }

}
