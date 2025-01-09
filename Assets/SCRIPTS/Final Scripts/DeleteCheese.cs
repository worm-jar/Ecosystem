using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteCheese : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            Destroy(this.gameObject);
        }
    }
}
