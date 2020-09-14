using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Tile : MonoBehaviour
{
    public Player myPlayer;
    public GameObject goal;

    public GameObject linePrefab;
    public GameObject line;
    public LineRenderer lineRenderer;
    
    private Vector3 mousePos;
    private RaycastHit hit;
    private Ray clickRay;

    [SerializeField] private Material tileColor;


    // Start is called before the first frame update
    void Start()
    {
        if (goal != null)
        {
            goal.GetComponent<Goal>().myCar = myPlayer.gameObject;
        }

        CreateLine();
    }

    private void OnMouseDown()
    {
        lineRenderer.positionCount = 1;
        
        clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layMask = 1 << LayerMask.NameToLayer("Ground");
        bool isPossibleToMove = Vector3.Distance(this.transform.position, myPlayer.transform.position) < this.transform.lossyScale.x + 0.1f;
        if (isPossibleToMove && Physics.Raycast(clickRay, out hit, layMask))
        {
            Debug.Log("hello");
            if(hit.transform.name == this.transform.name)
            {
                GetComponent<MeshRenderer>().material = tileColor;
                mousePos = new Vector3(hit.point.x, this.transform.position.y, hit.point.z);
                myPlayer.SetTarget(this.transform.position);
            }
        }
    }

    private bool isNearTheGoal(Vector3 pos)
    {
        float dist = Vector3.Distance(pos, this.transform.position);
        if(dist < 0.4f)
        {
            return true;
        }
        return false;
    }
    private void CreateLine()
    {
        if(line == null)
        {
            line = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
        }
        lineRenderer = line.GetComponent<LineRenderer>();
        if(lineRenderer == null)
        {
            line.AddComponent<LineRenderer>();
        }
    }
}