using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
public class PlayerAI : Player
{
    [SerializeField] private Tile[] tiles;
    [SerializeField] private Tile[] secondTiles;
    protected override void Start()
    {
        base.Start();
    }
    private void SwitchTiles()
    {
        tiles = secondTiles;
    }
    public void SetRandomTile()
    {
        foreach (var t in tiles)
        {
            bool isPossibleToMove = Vector3.Distance(t.transform.position, this.transform.position) < tiles[0].transform.lossyScale.x + 0.1f;
            if (isPossibleToMove && t.occupied == false && !visitedTiles.Contains(t.transform.position))
            {
                SetTarget(t.transform.position);
                t.occupied = true;
                break;
            }
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.gameObject.layer == 10)
        {

            if (transform.name == "AI")
            {
                SwitchTiles();
            }
        }
    }
}