using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ratchase : MonoBehaviour
{
    public float speed;
    public GameObject attractor;
    public GameObject rat;
    public Rigidbody body;
    public Vector3 storage;
    public Vector3 directionWander;
    public float timer;
    public float timerReset;

    // Start is called before the first frame update
    void Start()
    {
        
        timer = timerReset;
        rat = this.gameObject;
        body = this.GetComponent<Rigidbody>();
        directionWander =  new Vector3(Random.Range(0.5f, 360), 0, Random.Range(0.5f, 360));
        storage = directionWander;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(rat.transform.position, attractor.transform.position) > 8)
        {
            if (timer > 0)
            {
                directionWander = directionWander.normalized;
                body.velocity = directionWander * speed;
                timer -= Time.deltaTime;
            }
            else
            {
                directionWander = new Vector3(Mathf.Clamp(Random.Range(0.5f, 360), storage.x - 250, storage.x + 250), 0, Mathf.Clamp(Random.Range(0.5f, 360), storage.z - 250, storage.z + 250));
                transform.rotation = Quaternion.LookRotation(directionWander);
                storage = directionWander;
                timer = timerReset;
            }
        }
        else
        {
            Debug.Log(Vector3.Distance(rat.transform.position, body.transform.position));
            Vector3 direction = attractor.transform.position - rat.transform.position;
            direction = direction.normalized;
            body.velocity = direction * speed;
            return;
        }
    }
}
