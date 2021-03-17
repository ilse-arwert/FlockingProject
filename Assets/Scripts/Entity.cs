using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Entity : MonoBehaviour
{
    //-----------------------------------------------------------------------------
    // Const Data
    //-----------------------------------------------------------------------------
    private static readonly float mRadiusSquaredDistance = 50.0f;
    private static readonly float mMaxVelocity = 3.0f;

    //-----------------------------------------------------------------------------
    // Data
    //-----------------------------------------------------------------------------
    private int mID = 0;
    private Vector3 goalVelocity = new Vector3();
    private Vector3 currentVelocity = new Vector3();
    //-----------------------------------------------------------------------------
    // Functions
    //-----------------------------------------------------------------------------
    void Start()
    {
        goalVelocity = transform.forward;
        goalVelocity = Vector3.ClampMagnitude( goalVelocity, mMaxVelocity );
        currentVelocity = goalVelocity;
    }

    //-----------------------------------------------------------------------------
    void Update()
    {
        //are we done turning away from our collision? then consider the flock again
        //note: i'm rounding to two decimals here bc the entities kept getting stuck at 0.9999...
        if ((Mathf.Round(Vector3.Dot(goalVelocity.normalized, currentVelocity.normalized) * 100) / 100) == 1) {
            currentVelocity = goalVelocity;
            goalVelocity += FlockingBehaviour();
            goalVelocity = Vector3.ClampMagnitude( goalVelocity, mMaxVelocity );
        }
        
        currentVelocity = Vector3.RotateTowards(currentVelocity, goalVelocity, mMaxVelocity/2 * Time.deltaTime, 1.0f);
        currentVelocity = Vector3.ClampMagnitude(currentVelocity, mMaxVelocity);            
        
        transform.rotation = Quaternion.LookRotation(currentVelocity);
        transform.position += currentVelocity * Time.deltaTime;
        transform.forward = currentVelocity.normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position));
    }

    public void printVectors() {
        print("goal vector: " + goalVelocity.ToString() );
        print("current vector: " + currentVelocity.ToString() );
        print(Vector3.Dot(goalVelocity.normalized, currentVelocity.normalized));
    }

    //-----------------------------------------------------------------------------
    public void Collide(Vector3 normal)
    {
        for(int i = 0; i < 3; i++)
        {
            if(normal[i] != 0)
            {
                goalVelocity[i] *= -1;
            }
        }
    }

    //-----------------------------------------------------------------------------
    public void SetID( int ID )
    {
        mID = ID;
    }

    //-----------------------------------------------------------------------------
    public int ID
    {
        get { return mID; }
    }

    //-----------------------------------------------------------------------------
    // Flocking Behavior
    //-----------------------------------------------------------------------------
    private Vector3 FlockingBehaviour()
    {
        List<Entity> theFlock = App.instance.theFlock;

        Vector3 cohesionVector = new Vector3();
        Vector3 separateVector = new Vector3();
        Vector3 alignmentVector = new Vector3();

        int count = 0;

        for( int index = 0; index < theFlock.Count; index++ )
        {
            if( mID != theFlock[ index ].ID )
            {
                float distance = ( transform.position - theFlock[ index ].transform.position ).sqrMagnitude;

                if( distance > 0 && distance < mRadiusSquaredDistance )
                {
                    cohesionVector += theFlock[ index ].transform.position;
                    separateVector += theFlock[ index ].transform.position - transform.position;
                    alignmentVector += theFlock[ index ].transform.forward;

                    count++;
                }
            }
        }

        if( count == 0 )
        {
            return Vector3.zero;
        }

        // revert vector
        // separation step
        separateVector /= count;
        separateVector *= -1;

        // forward step
        alignmentVector /= count;

        // cohesione step
        cohesionVector /= count;
        cohesionVector = ( cohesionVector - transform.position );

        // Add All vectors together to get flocking
        Vector3 flockingVector = ( ( separateVector.normalized * App.instance.separationWeight ) +
                                    ( cohesionVector.normalized * App.instance.cohesionWeight ) +
                                    ( alignmentVector.normalized * App.instance.alignmentWeight ) );

        return flockingVector;
    }
}
