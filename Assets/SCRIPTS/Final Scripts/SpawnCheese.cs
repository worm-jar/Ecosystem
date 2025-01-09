using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCheese : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject _cheese;
    public Vector3 _location;
    public Transform _transform;
    public float _time;
    void Start()
    {
        StartCoroutine(Spawn());
    }
    private void OnCollisionEnter(Collision collision)
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
            float RandomX = Random.Range(_transform.position.x - 30f, _transform.position.x + 30f);
            float RandomZ = Random.Range(_transform.position.z - 45f, _transform.position.z + 45f);
            _location = new Vector3(RandomX, 4.5f, RandomZ);
            Instantiate(_cheese, _location, Quaternion.identity);
            yield return new WaitForSeconds(_time);
        }
    }
}
