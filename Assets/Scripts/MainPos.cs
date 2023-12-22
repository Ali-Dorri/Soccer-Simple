using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPos : MonoBehaviour {
    public float Radius = 1f;
    public int ID;
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Radius);
    }

}
