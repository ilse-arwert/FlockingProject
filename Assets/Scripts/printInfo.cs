using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class printInfo : MonoBehaviour
{
    public Entity target;
    void LateUpdate() {
        if (!target) return;

        target.printVectors();
    }
}

