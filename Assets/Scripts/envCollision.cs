using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class envCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collisionInfo) {
        Entity e = collisionInfo.gameObject.GetComponent(typeof(Entity)) as Entity;
        ContactPoint c = collisionInfo.GetContact(0);

        //
        // Debug.Log(c.point);
        e.Reposition(c.normal);

    }
}

