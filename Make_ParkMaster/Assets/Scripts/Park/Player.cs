using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public GameObject startingPoint;
    [SerializeField] VoiceController voiceController;
    public float speed;
    public float impulsePower;


    public Vector3 mousePos;
    
    public GameObject head;
    private Vector3 headVec;
    private Vector3 dir;
    public Quaternion startAngle;
    [SerializeField] private GameObject stars;
    [SerializeField] private MenuController menuController;

    private Vector3 target;
    [SerializeField]private PlayerAI ai;

    protected List<Vector3> visitedTiles = new List<Vector3>();

    [SerializeField] private Material tileMaterial;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if(startingPoint != null)
        {
            this.gameObject.transform.position = startingPoint.transform.position 
                                                    + new Vector3(0.0f, this.transform.localScale.y / 2, 0.0f);
            target = startingPoint.transform.position;
        }

        visitedTiles = new List<Vector3>();
        visitedTiles.Add(startingPoint.transform.position);

        headVec = (head.transform.position - this.transform.position).normalized;
        dir = headVec;
        startAngle = this.transform.rotation;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        float dist = Vector3.Distance(this.transform.position, target);
        if (dist > 0.05f)
        {

            target = new Vector3(target.x, this.transform.position.y, target.z);
            dir = (target - this.transform.position);


            float angle = Quaternion.FromToRotation(headVec, dir).eulerAngles.y;
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir),
                Time.fixedDeltaTime * 10.0f);
            transform.position = Vector3.MoveTowards(transform.position, target, speed);
            transform.GetComponent<Animator>().SetBool("Run", true);

            dist = Vector3.Distance(this.transform.position, target);
            if (dist < 0.05f)
            {
                if (ai != null)
                {
                    ai.SetRandomTile();
                    transform.position = target;
                }
            }
        }

        else
            transform.GetComponent<Animator>().SetBool("Run", false);

    }

    public Vector3 GetDir()
    {
        return dir;
    }

    public void SetTarget(Vector3 target)
    {
        if (!visitedTiles.Contains(target))
        {
            this.target = target;
            visitedTiles.Add(target);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Player>())
        {
            Debug.Log("çarptı");
            transform.GetComponent<Animator>().SetBool("Carpisma", true);
            GameObject.Find("Main Camera").GetComponent<VoiceController>().playVoice(4);
            stars.SetActive(true);
            GameObject.Find("Main Camera").GetComponent<CameraShake>().Shake();
            StartCoroutine(RestartLevel());
            /*Player otherCar = collision.gameObject.GetComponent<Player>();
            Vector3 otherDir = otherCar.GetDir();
            Vector3 myInvDir = -dir;

            Vector3 powerDir = (otherDir + myInvDir) + Vector3.up;
            this.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(powerDir * impulsePower, collision.transform.position, ForceMode.Impulse); */
        }
        else if(collision.gameObject.tag == "Ground")
        {
            MeshRenderer mr = collision.transform.gameObject.GetComponent<MeshRenderer>();
            if(mr != null && mr.material != tileMaterial)
                mr.material = tileMaterial;
        }
        else
        {
            Vector3 myInvDir = -dir;

            Vector3 powerDir =  myInvDir + Vector3.up;
            this.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(powerDir * impulsePower, collision.transform.position, ForceMode.Impulse);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
            collision.gameObject.GetComponent<Tile>().occupied = false;
        
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
