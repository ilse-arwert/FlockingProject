﻿//script by carmine - https://answers.unity.com/users/29803/carmine.html
//https://answers.unity.com/questions/213427/sphere-collider-invert.html?childToView=369468#answer-369468
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshFilter))]
public class ReverseNormals : MonoBehaviour
{
    void Start()
    {
        MeshFilter filter = GetComponent(typeof(MeshFilter)) as MeshFilter;
        if (filter != null)
        {
            Mesh mesh = filter.mesh;

            Vector3[] normals = mesh.normals;
            for (int i = 0; i < normals.Length; i++)
                normals[i] = -normals[i];
            mesh.normals = normals;

            for (int m = 0; m < mesh.subMeshCount; m++)
            {
                int[] triangles = mesh.GetTriangles(m);
                for (int i = 0; i < triangles.Length; i += 3)
                {
                    int temp = triangles[i + 0];
                    triangles[i + 0] = triangles[i + 1];
                    triangles[i + 1] = temp;
                }
                mesh.SetTriangles(triangles, m);
            }
        }

        this.GetComponent<MeshCollider>().sharedMesh = filter.mesh;
    }
}