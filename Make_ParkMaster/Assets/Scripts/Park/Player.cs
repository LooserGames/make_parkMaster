using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
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

    [SerializeField] private ParticleSystem particle;

    [SerializeField] private GameObject stars;
    [SerializeField] private MenuController menuController;

    [SerializeField] private List<GameObject> breakList;
   

 

    private Vector3 target;
    [SerializeField]private PlayerAI ai;
    private Vector3 playerPosition;
    private Vector3 aiPos;
    
    protected List<Vector3> visitedTiles = new List<Vector3>();
    private bool isMoving = false;
    public bool IsMoving
    {
        set { isMoving = value;}
        get { return isMoving; }
    }

 

    [SerializeField] private Material tileMaterial;

 

    // Start is called before the first frame update
    protected virtual void Start()
    {
       
        if(startingPoint != null)
        {
            SetTarget(startingPoint.transform.position);
            this.gameObject.transform.position  = new Vector3(target.x,transform.position.y,target.z);
            
        }

        StartCoroutine(setPos());
        

        //visitedTiles = new List<Vector3>();
        //visitedTiles.Add(startingPoint.transform.position);

 

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
            isMoving = true;

 

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
                isMoving = false;
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
        //if (!visitedTiles.Contains(target))
        //{
            this.target = new Vector3(target.x,transform.position.y,target.z);
            visitedTiles.Add(target);
        //}
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
            //this.gameObject.GetComponent<Rigidbody>().AddForceAtPosition(powerDir * impulsePower, collision.transform.position, ForceMode.Impulse);
        }

       if (!isActive)
        {
            if (transform.name == "Player")
            {
                collision.gameObject.SetActive(false);
                isActive = true;
                
               
                
            }
            if (transform.name == "AI")
            {
                collision.gameObject.SetActive(false);
                isActive = true;
                
               
                
            }
        }

        
    }
    
    

 

 

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Tile tile = collision.gameObject.GetComponent<Tile>();
            tile.occupied = false;
            tile.SetMaterialToDefaultMaterial();
            print("exit");
        }
        
        

        
    }
    IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator CoinFalse(GameObject coin)
    {
        yield return new WaitForSeconds(1);
        coin.SetActive(false);
    }
   

    private bool isActive = true;
    protected virtual void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger= " + other.tag);
        if (other.transform.tag == "Diamond")
        {
            other.transform.GetChild(0).GetComponent<Animator>().enabled = true;
            StartCoroutine(DiamondAnim(other));
            voiceController.playVoice(3);
            StartCoroutine(CoinFalse(other.gameObject));
        }
        
        if (other.gameObject.layer == 10)
        {
            if (other.gameObject.tag == "break")
            {
                breakList[0].gameObject.SetActive(true);
                isActive = false;
            }
            
            if (other.gameObject.tag == "break2")
            {
                breakList[1].gameObject.SetActive(true);
                isActive = false;
            }

           
            if (transform.name == "Player")
            {
               
                StartCoroutine(playerPos());
          
                
            }

            if (transform.name == "AI")
            {
               
                StartCoroutine(AIPos());
               
                visitedTiles=new List<Vector3>();
            }
        }

        if (other.gameObject.tag == "canta")
        {
            other.gameObject.SetActive(false);
            
            if (transform.name == "Player")
            {
                transform.GetChild(7).gameObject.SetActive(true);
            }
            
            if (transform.name == "AI")
            {
                transform.GetChild(7).gameObject.SetActive(true);
            }
            
            
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

    IEnumerator setPos()
    {
        yield return new WaitForEndOfFrame();
        playerPosition = GameObject.Find("Player").transform.position;
        aiPos = GameObject.Find("AI").transform.position;
    }
    IEnumerator AIPos()
    {
        yield return new WaitForSeconds(0.7f);
        //particle.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        GameObject.Find("AI").transform.position = startingPoint.transform.position;
        GameObject.Find("AI").GetComponent<Collider>().enabled = true;
        SetTarget(startingPoint.transform.position);
        particle.gameObject.SetActive(false);
    }
    IEnumerator playerPos()
    {
        yield return new WaitForSeconds(0.7f);
        //particle.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        GameObject.Find("Player").transform.position = startingPoint.transform.position;
        GameObject.Find("Player").GetComponent<Collider>().enabled = true;
        SetTarget(startingPoint.transform.position);
        particle.gameObject.SetActive(false);
    }

   

  
}