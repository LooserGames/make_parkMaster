using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Car : MonoBehaviour
{
    public GameObject startingPoint;
    [SerializeField] VoiceController voiceController;
    public float speed;
    public float impulsePower;

    public Queue<Vector3> path;
    private Queue<Vector3> prevPath;

    public Vector3 mousePos;
    private Vector3 nextPos;

    public bool canMove = false;
    
    public GameObject head;
    private Vector3 headVec;
    private Vector3 dir;
    public Quaternion startAngle;
    [SerializeField] GameObject stars;
    [SerializeField] MenuController menuController;

    // Start is called before the first frame update
    void Start()
    {
        if(startingPoint != null)
        {
            this.gameObject.transform.position = startingPoint.transform.position 
                                                    + new Vector3(0.0f, this.transform.localScale.y / 2, 0.0f);
        }

        path = new Queue<Vector3>();
        prevPath = new Queue<Vector3>();

        headVec = (head.transform.position - this.transform.position).normalized;
        dir = headVec;
        startAngle = this.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove == true)
        {
            if (path.Count != 0)
            {
                float dist = Vector3.Distance(this.transform.position, nextPos);

                if (dist < 0.05f)
                {
                    nextPos = path.Dequeue();
                }

                nextPos = new Vector3(nextPos.x, this.transform.position.y, nextPos.z);
                dir = (nextPos - this.transform.position);
            }

            float angle = Quaternion.FromToRotation(headVec, dir).eulerAngles.y;
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 10.0f);
            transform.position = Vector3.MoveTowards(transform.position, nextPos, speed);
            transform.GetComponent<Animator>().SetBool("Run", true);
        }
        else
            transform.GetComponent<Animator>().SetBool("Run", false);
    }

    public Vector3 GetDir()
    {
        return dir;
    }

    public void CarReset()
    {
        this.gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);

        if (prevPath.Count != 0)
        {
            path = new Queue<Vector3>(prevPath.ToArray());
        }

        canMove = false;

        this.gameObject.transform.position = startingPoint.transform.position
                                                    + new Vector3(0.0f, this.transform.localScale.y / 2, 0.0f);
        dir = headVec;

        this.transform.rotation = startAngle;
    }

    public void CarMove()
    {
        if(path.Count > 2)
        {
            nextPos = path.Dequeue();
            nextPos = new Vector3(nextPos.x, this.transform.position.y, nextPos.z);

            canMove = true;
        }
        else
        {
            this.gameObject.transform.position = startingPoint.transform.position
                                                    + new Vector3(0.0f, this.transform.localScale.y / 2, 0.0f);
        }   
    }

    public void SetCarPath(Queue<Vector3> prevVec, Queue<Vector3> currentVec)
    {
        path = new Queue<Vector3>(currentVec.ToArray());
        prevPath = new Queue<Vector3>(prevVec.ToArray());
    }
    
    public void ResetPaths()
    {
        prevPath.Clear();
        path.Clear();
        dir = headVec;
        this.transform.rotation = startAngle;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Car>())
        {
            Debug.Log("çarptı");
            canMove = false;
            transform.GetComponent<Animator>().SetBool("Carpisma", true);
            GameObject.Find("Main Camera").GetComponent<VoiceController>().playVoice(4);
            stars.SetActive(true);
            GameObject.Find("Main Camera").GetComponent<CameraShake>().Shake();
            StartCoroutine(RestartLevel());
            /*Car otherCar = collision.gameObject.GetComponent<Car>();
            Vector3 otherDir = otherCar.GetDir();
            Vector3 myInvDir = -dir;

            Vector3 powerDir = (otherDir + myInvDir) + Vector3.up;
            this.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(powerDir * impulsePower, collision.transform.position, ForceMode.Impulse); */
        }
        else if(collision.gameObject.tag != "Ground")
        {
            canMove = false;

            Vector3 myInvDir = -dir;

            Vector3 powerDir =  myInvDir + Vector3.up;
            this.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(powerDir * impulsePower, collision.transform.position, ForceMode.Impulse);
        }
    }
    IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger= " + other.tag);
        if (other.transform.tag == "Diamond")
        {
            other.transform.GetChild(0).GetComponent<Animator>().enabled = true;
            StartCoroutine(DiamondAnim(other));
            voiceController.playVoice(3);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(this.transform.position, head.transform.position);

        Gizmos.color = Color.green;

        Gizmos.DrawLine(this.transform.position, this.transform.position + dir * 3.0f);
    }
    IEnumerator DiamondAnim(Collider other)
    {
        yield return new WaitForSeconds(.3f);
        other.GetComponent<Diamond>().AddCoin();
        menuController.SetCoinText(int.Parse(menuController.coinText.text) + 1);
    }
}
