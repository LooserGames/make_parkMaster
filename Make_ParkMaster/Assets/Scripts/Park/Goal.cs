using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public GameObject myCar;

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
        if(other.gameObject == myCar)
        {
            Debug.Log("Parkta");
            isCarHere = true;
            Debug.Log(myCar.name);
            kareAnim.SetActive(true);
            GameObject.Find("Main Camera").GetComponent<VoiceController>().playVoice(1);
            myCar.GetComponent<Animator>().SetBool("Win", true);
            fillColor.SetActive(true);
            //myPlayer.GetComponent<Animator>().SetBool("Run", false);
        //    this.GetComponent<MeshRenderer>().material.color = new Color(color.r + 0.5f, color.g + 0.5f, color.b + 0.5f);

            if (stageManager != null)
            {
                stageManager.CheckEveryCarInGoal();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == myCar)
        {
         //   this.GetComponent<MeshRenderer>().material.color = color;
            isCarHere = false;
        }
    }
}
