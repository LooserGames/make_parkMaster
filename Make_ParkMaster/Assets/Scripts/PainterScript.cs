using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Generate paint decals
/// </summary>
public class PainterScript : MonoBehaviour
{
    public static PainterScript Instance;

    /// <summary>
    /// A single paint decal to instantiate
    /// </summary>
    public Transform PaintPrefab;

    private int MinSplashs = 5;
    private int MaxSplashs = 15;
    private float SplashRange = 2f;

    private float MinScale = 0.25f;
    private float MaxScale = 2.5f;

    // DEBUG
    private bool mDrawDebug;
    private Vector3 mHitPoint;
    private List<Ray> mRaysDebug = new List<Ray>();

    void Awake()
    {
        if (Instance != null) Debug.LogError("More than one Painter has been instanciated in this scene!");
        Instance = this;

        if (PaintPrefab == null) Debug.LogError("Missing Paint decal prefab!");
    }

    private int instTime=0;
    void OnParticleCollision(GameObject other)
    {

        while (instTime<2)
        {
            Paint(other.transform.position  * (SplashRange / 4f));
            instTime++;
            print(instTime);
        }
        
    }

    public void Paint(Vector3 location)
    {
        //DEBUG
        mHitPoint = location;
        mRaysDebug.Clear();
        mDrawDebug = true;

        int n = -1;

        int drops = Random.Range(MinSplashs, MaxSplashs);
        RaycastHit hit;

        // Generate multiple decals in once
        while (n <= drops)
        {
            n++;

            // Get a random direction (beween -n and n for each vector component)
            var fwd = transform.TransformDirection(Random.onUnitSphere * SplashRange);

            mRaysDebug.Add(new Ray(location, fwd));

            // Raycast around the position to splash everwhere we can
            if (Physics.Raycast(location, fwd, out hit, SplashRange))
            {
                // Create a splash if we found a surface
                var paintSplatter = GameObject.Instantiate(PaintPrefab,
                                                           new Vector3(transform.position.x,transform.position.y+0.4f,transform.position.z),

                                                           // Rotation from the original sprite to the normal
                    // Prefab are currently oriented to z+ so we use the opposite
                                                           Quaternion.FromToRotation(new Vector3(180,0,0),hit.normal)
                                                           ) as Transform;

                // Random scale
                var scaler = Random.Range(MinScale, MaxScale);

                paintSplatter.localScale = new Vector3(
                    paintSplatter.localScale.x * scaler,
                    paintSplatter.localScale.y * scaler,
                    paintSplatter.localScale.z
                );

                // Random rotation effect
                var rater = Random.Range(0, 0);
                paintSplatter.transform.RotateAround(transform.position, new Vector3(180,0,0), rater);


                // TODO: What do we do here? We kill them after some sec?
                Destroy(paintSplatter.gameObject, 25);
            }

        }
    }

    void OnDrawGizmos()
    {
        // DEBUG
        if (mDrawDebug)
        {
            Gizmos.DrawSphere(mHitPoint, 0.2f);
            foreach (var r in mRaysDebug)
            {
                Gizmos.DrawRay(r);
            }
        }
    }
}