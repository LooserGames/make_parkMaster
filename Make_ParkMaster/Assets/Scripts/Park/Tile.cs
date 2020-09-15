using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class Tile : MonoBehaviour
{
    public Player player1;

    public GameObject linePrefab;
    public GameObject line;
    public LineRenderer lineRenderer;
    
    private Vector3 mousePos;
    private RaycastHit hit;
    private Ray clickRay;

    [HideInInspector] public bool occupied = false;

    // Start is called before the first frame update
    private void Start()
    {
        CreateLine();
    }

    private void OnMouseDown()
    {
        lineRenderer.positionCount = 1;
        
        clickRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layMask = 1 << LayerMask.NameToLayer("Ground");

        void Move(Player player)
        {
            bool isPossibleToMove = Vector3.Distance(this.transform.position, player.transform.position) <
                                    this.transform.lossyScale.x + 0.1f;
            if (isPossibleToMove && Physics.Raycast(clickRay, out hit, layMask) && !occupied)
            {
                if (hit.transform == this.transform)
                {
                    occupied = true;

                    mousePos = new Vector3(hit.point.x, this.transform.position.y, hit.point.z);
                    player.SetTarget(this.transform.position);
                }
            }
        }

        Move(player1);
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