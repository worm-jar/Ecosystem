using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class CatMovement : MonoBehaviour
{
    // Start is called before the first frame update


    public Vector3 _target;
    public float _angle;
    public Vector3 _direction;
    private UnityEngine.AI.NavMeshAgent _agent;
    public float _time;
    NavMeshPath _path;
    public float _limitAngle;
    public float _distance;
    [SerializeField] private bool _isChasing = false;
    public Transform _rat;
    public float _hunger;
    public float _hungerMax;
    public float _thirst;
    public float _thirstMax;
    private float _timer;
    public float _timerSet;
    public bool _timerStart;

    private void Start()
    {
        _agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        _path = new NavMeshPath();
        _hunger = _hungerMax; _thirst = _thirstMax;
        _timer = _timerSet;
        StartCoroutine(Randomize());

    }

    // Update is called once per frame
    void Update()
    {
        if (_timer > 0 && _timerStart)
        {
            _timer -= Time.deltaTime;
        }
        else if (_timer <= 0)
        {
            StartCoroutine(Randomize());
            _timerStart = false;
            _timer = _timerSet;
        }
        _hunger -= Time.deltaTime;
        _thirst -= Time.deltaTime;
        _hunger = Mathf.Clamp(_hunger, 0f, _hungerMax);
        _thirst = Mathf.Clamp(_thirst, 0f, _thirstMax);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Rat"))
        {
            _isChasing = true;
            _rat = other.transform;
            _target = new Vector3(_rat.transform.position.x, 0, _rat.transform.position.z);
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Rat"))
        {
            GameObject rat = collision.gameObject;
            Destroy(rat.gameObject);
            _hunger += 18f;
            _isChasing = false;
            _timerStart = true;
        }
    }
    private IEnumerator Randomize()
    {
        while (_isChasing == false)
        {
            _agent.speed = 3.5f;
            if (_agent.CalculatePath(_target, _path) && _path.status == NavMeshPathStatus.PathComplete)
            {
                _direction = transform.forward;
                _angle = (Random.Range(-_angle - _limitAngle, _angle + _limitAngle)%360);
                Quaternion rotation = Quaternion.AngleAxis(_angle, Vector3.up);
                _direction = rotation*_direction;
                _target = transform.position + _direction * _distance;
                _agent.destination = _target;
                yield return new WaitForSeconds(_time);
            }
            else
            {
                Quaternion rotation = Quaternion.AngleAxis(_angle, Vector3.up);
                _direction = -(rotation * _direction);
                _target = transform.position + _direction * _distance;
                _agent.destination = _target;
                yield return new WaitForSeconds(_time*3);
            }
        }
        while ( _isChasing == true)
        {
            _agent.speed = 7.5f;
            _agent.destination = _target;
            yield return new WaitForSeconds(_time);
        }
    }
}
