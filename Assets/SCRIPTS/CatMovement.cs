using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatMovement : MonoBehaviour
{
    // Start is called before the first frame update


    public Transform Target;
    private UnityEngine.AI.NavMeshAgent _agent;
    public GameObject cat;
    public float time;
    private float speed = 3.5f;

    private void Start()
    {
        _agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        StartCoroutine(Randomize());
    }

    // Update is called once per frame
    void Update()
    {
        speed = GetComponent<Rigidbody>().velocity.magnitude;
    }
    private IEnumerator Randomize()
    {
        while (true)
        {
            if (speed > 2f)
            {
                float randomPosx = Random.Range(cat.transform.position.x, Mathf.Clamp(cat.transform.position.x + (Target.position.x - cat.transform.position.x) + 10f, cat.transform.position.x + 20f, cat.transform.position.x + 30f));
                float randomPosz = Random.Range(cat.transform.position.z, Mathf.Clamp(cat.transform.position.z + (Target.position.z - cat.transform.position.z) + 10f, cat.transform.position.z + 20f, cat.transform.position.z + 30f));
                Target.position = new Vector3(randomPosx, 0f, randomPosz);
                _agent.destination = Target.position;
                yield return new WaitForSeconds(time);
            }
            else
            {
                Target.position = Target.position*-10;           
                speed = 3.5f;
                _agent.destination = Target.position;
                yield return new WaitForSeconds(time);
            }
        }
    }
}
