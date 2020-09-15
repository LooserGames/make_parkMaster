using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField]private GameObject player;

    public bool isCarHere;
    private StageManager stageManager;
    [SerializeField] GameObject kareAnim;
    [SerializeField] GameObject fillColor;
  //  private Color color;

    // Start is called before the first frame update
    void Start()
    {
        isCarHere = false;
        stageManager = FindObjectOfType<StageManager>();
      //  color = this.GetComponent<MeshRenderer>().material.color;
    }

    public bool IsCarHere()
    {
        if(isCarHere)
        {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            Debug.Log("Parkta");
            isCarHere = true;
            Debug.Log(player.name);
            kareAnim.SetActive(true);
            GameObject.Find("Main Camera").GetComponent<VoiceController>().playVoice(1);
            player.GetComponent<Animator>().SetBool("Win", true);
            fillColor.SetActive(true);
            //player1.GetComponent<Animator>().SetBool("Run", false);
        //    this.GetComponent<MeshRenderer>().material.color = new Color(color.r + 0.5f, color.g + 0.5f, color.b + 0.5f);

            if (stageManager != null)
            {
                stageManager.CheckEveryCarInGoal();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
         //   this.GetComponent<MeshRenderer>().material.color = color;
            isCarHere = false;
        }
    }
}
