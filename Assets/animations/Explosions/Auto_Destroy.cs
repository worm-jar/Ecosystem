using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Auto_Destroy : MonoBehaviour
{
    public float delay = 2;
    private void Start()
    {
        Destroy(gameObject, delay);
            
    }
}
