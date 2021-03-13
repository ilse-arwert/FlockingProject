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
    private Vector3 mVelocity = new Vector3();

    //-----------------------------------------------------------------------------
    // Functions
    //-----------------------------------------------------------------------------
    void Start()
    {
        mVelocity = transform.forward;
        mVelocity = Vector3.ClampMagnitude( mVelocity, mMaxVelocity );
    }

    //-----------------------------------------------------------------------------
    void Update()
    {
        mVelocity += FlockingBehaviour();

        mVelocity = Vector3.ClampMagnitude( mVelocity, mMaxVelocity );

        transform.position += mVelocity * Time.deltaTime;

        transform.forward = mVelocity.normalized;

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position));
    }

    //-----------------------------------------------------------------------------
    public void Collide(Vector3 normal)
    {
        for(int i = 0; i < 3; i++)
        {
            if(normal[i] != 0)
            {
                mVelocity[i] *= -1;
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
