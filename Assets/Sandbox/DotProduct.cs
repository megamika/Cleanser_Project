using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotProduct : MonoBehaviour
{
    [SerializeField] Vector3 vectorA;
    [SerializeField] Vector3 vectorB;
    public float dotProduct;
    public bool reverseOrder;
    void Start()
    {
        
    }


    void Update()
    {
        if (!reverseOrder)
        {
            dotProduct = Vector3.Dot(vectorA.normalized, vectorB.normalized);
        }
        else
        {
            dotProduct = Vector3.Dot(vectorB.normalized, vectorA.normalized);
        }

        Debug.DrawLine(transform.position, transform.position + vectorA.normalized, Color.red);
        Debug.DrawLine(transform.position, transform.position + vectorB.normalized, Color.green);
    }
}
