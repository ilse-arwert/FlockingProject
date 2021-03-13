using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class envCollision : MonoBehaviour
{
    void OnCollisionEnter(Collision collisionInfo) {
        Entity e = collisionInfo.gameObject.GetComponent(typeof(Entity)) as Entity;
        ContactPoint c = collisionInfo.GetContact(0);
        e.Collide(c.normal);
    }
}

