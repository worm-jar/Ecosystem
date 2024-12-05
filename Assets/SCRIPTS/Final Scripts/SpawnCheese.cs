using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCheese : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject _cheese;
    public Vector3 _location;
    public float _time;
    void Start()
    {
        StartCoroutine(Spawn());
    }
    public void OnCollisionEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            Destroy(this);
        }
    }
    // Update is called once per frame
    public IEnumerator Spawn()
    {
        while (true)
        {
            float RandomX = Random.Range(-36f, 36f);
            float RandomZ = Random.Range(-45f, 45f);
            _location = new Vector3(RandomX, 0.5f, RandomZ);
            Instantiate(_cheese, _location, Quaternion.identity);
            yield return new WaitForSeconds(_time);
        }
    }
}
