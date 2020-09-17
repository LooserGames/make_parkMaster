using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private Goal[] goals;
    private int notClearNum;
    [SerializeField] MenuController menuController;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("stage manager: "+gameObject.name);
        goals = FindObjectsOfType<Goal>();
        notClearNum = goals.Length;
    }

    public void CheckEveryCarInGoal()
    {
        
        if (goals[1].IsCarHere() == true)
        {
            menuController.OpenWinPanel();
            GetComponent<VoiceController>().playVoice(0);
        }
        
        if (goals[0].IsCarHere() == true)
        {
            menuController.OpenGameOverPanel();
            GameObject.Find("Player").GetComponent<Player>().enabled = false;
        }

        //  ClearGame();
    }
}
