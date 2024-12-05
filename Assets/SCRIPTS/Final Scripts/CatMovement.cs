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
    public Transform _cat;
    public Transform _water;
    public float _hunger;
    public float _hungerMax;
    public float _thirst;
    public float _thirstMax;
    public bool _thirsty = false;
    public float _reproduction;
    public float _reproductionMax;
    public float _timerSet;
    public bool _timerStart;
    public bool _breedable = false;

    private void Start()
    {
        _agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        _path = new NavMeshPath();
        _hunger = _hungerMax; _thirst = _thirstMax;
        StartCoroutine(Randomize());

    }

    // Update is called once per frame
    void Update()
    {
        _hunger -= Time.deltaTime;
        _thirst -= Time.deltaTime;
        _hunger = Mathf.Clamp(_hunger, 0f, _hungerMax);
        _thirst = Mathf.Clamp(_thirst, 0f, _thirstMax);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Rat") && _hunger >= _thirst)
        {
            _isChasing = true;
            _rat = other.transform;
            _target = new Vector3(_rat.transform.position.x, 0, _rat.transform.position.z);
        }
        if (other.gameObject.CompareTag("Water") && _thirst >= _hunger)
        {
            _thirsty = true;
            _isChasing = true;
            _water = other.transform;
            _target = new Vector3(_water.transform.position.x, 0, _water.transform.position.z);
        }
        if (other.gameObject.CompareTag("Cat") && _reproduction > 40)
        {
            _breedable = true;
            _cat = other.transform;
            _target = new Vector3(_cat.transform.position.x, 0, _cat.transform.position.z);
            StartCoroutine(Breed());
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Rat"))
        {
            _isChasing = true;
            _rat = other.transform;
            _target = new Vector3(_rat.transform.position.x, 0, _rat.transform.position.z);
        }
        if (other.gameObject.CompareTag("Cat") && _reproduction > 40)
        {
            _breedable = true;
            _cat = other.transform;
            _target = new Vector3(_cat.transform.position.x, 0, _cat.transform.position.z);
            StartCoroutine(Breed());
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
        }
        if (collision.gameObject.CompareTag("Water") && _thirsty)
        {
            StartCoroutine(Drink());
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Rat"))
        {
            _isChasing = false;
        }
    }
    private IEnumerator Randomize()
    {
        while (_isChasing == false)
        {
            _agent.speed = 3.5f;
            if (_agent.CalculatePath(_target, _path) && _path.status == NavMeshPathStatus.PathComplete && _thirsty == false)
            {
                _direction = transform.forward;
                _angle = (Random.Range(-_angle - _limitAngle, _angle + _limitAngle)%360);
                Quaternion rotation = Quaternion.AngleAxis(_angle, Vector3.up);
                _direction = rotation*_direction;
                _target = transform.position + _direction * _distance;
                _agent.destination = _target;
                yield return new WaitForSeconds(_time);
            }
            if (_agent.CalculatePath(_target, _path) && _path.status == NavMeshPathStatus.PathComplete && _thirsty == true)
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
        while (_isChasing == true)
        {
            _agent.speed = 7.5f;
            _agent.destination = _target;
            yield return new WaitForSeconds(_time/20);
        }
            _agent.velocity = new Vector3(0, 0, 0);
            _isChasing = false;
            yield return new WaitForSeconds(_time);
    }
    public IEnumerator Breed()
    {
        _breedable = false;
        _reproduction = 0f;
        _agent.destination = _target;
        yield return new WaitForSeconds(_time);
    }
    public IEnumerator Drink()
    {
        _thirst = 30f;
        yield return new WaitForSeconds(_time);
    }
}
