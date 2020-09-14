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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckEveryCarInGoal()
    {
        for(int i = 0; i < goals.Length; i++)
        {
            if(goals[i].IsCarHere() == false)
            {
                return;
            }
        }
        menuController.OpenWinPanel();
        GetComponent<VoiceController>().playVoice(0);
      //  ClearGame();
    }

    private void ClearGame()
    {
        CarManager[] carManagers = FindObjectsOfType<CarManager>();

        for(int i = 0; i < carManagers.Length; i++)
        {
            carManagers[i].TurnOffCollider();
        }
    }
}
