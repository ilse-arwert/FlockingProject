using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class App : MonoBehaviour
{
    //-----------------------------------------------------------------------------
    // Const Data
    //-----------------------------------------------------------------------------
    private static readonly int mNumberOfEntities = 200;
    
    //-----------------------------------------------------------------------------
    // Data
    //-----------------------------------------------------------------------------
    public Entity templatePrefab = null;

    public float speedWeight = 1.0f;
    public float separationWeight = 0.2f;
    public float alignmentWeight = 0.95f;
    public float cohesionWeight = 0.7f;

    public UISlidersWidget sliderWidget = null;

    public static App instance = null;

    private List<Entity> mTheFlock = new List<Entity>();

    //-----------------------------------------------------------------------------
    // Functions
    //-----------------------------------------------------------------------------
    void Start ()
    {
        instance = this;

        sliderWidget.Setup();

        InstantiateFlock();
    }

    //-----------------------------------------------------------------------------
    private void InstantiateFlock()
    {
        for ( int i = 0; i < mNumberOfEntities; i++ )
        {
            Entity flockEntity = Instantiate( templatePrefab );

            flockEntity.transform.rotation = Random.rotation;

            var startPos = new Vector3(
                Random.Range(-20, 20),
                Random.Range(-10, 10),
                Random.Range(-10, 10)
            );

            flockEntity.transform.position = startPos;

            flockEntity.SetID( i );

            mTheFlock.Add( flockEntity );
        }
    }

    //-----------------------------------------------------------------------------
    public List<Entity> theFlock
    {
        get { return mTheFlock; }
    }
}
