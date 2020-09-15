using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerAI : Player
{
    [SerializeField] private Tile[] tiles;

    protected override void Start()
    {
        base.Start();

    }

    public void SetRandomTile()
    {
        foreach (var t in tiles)
        {
            bool isPossibleToMove = Vector3.Distance(t.transform.position, this.transform.position) < tiles[0].transform.lossyScale.x + 0.1f;
            if (isPossibleToMove && !visitedTiles.Contains(t.transform.position) && t.occupied == false)
            {
                SetTarget(t.transform.position);
                t.occupied = true;
                break;
            }

        }
    }
}
