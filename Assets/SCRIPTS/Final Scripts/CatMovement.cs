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
    public GameObject _catObject;
    private bool _ratNull;
    private float _spawnTimer = 0.1f;
    public bool _nothing = false;

    private void Start()
    {
        _agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        _path = new NavMeshPath();
        _hunger = _hungerMax; _thirst = _thirstMax;
        StartCoroutine(Randomize());
    }
    private void Awake()
    {
        _thirst = _thirstMax;
        _hunger = _hungerMax;
    }

    // Update is called once per frame
    void Update()
    {
        _reproduction += Time.deltaTime;
        _reproduction = Mathf.Clamp(_reproduction, 0f, _reproductionMax);
        _hunger -= Time.deltaTime;
        _thirst -= Time.deltaTime;
        _hunger = Mathf.Clamp(_hunger, 0f, _hungerMax);
        _thirst = Mathf.Clamp(_thirst, 0f, _thirstMax);
        if (_rat == null && _ratNull == true)
        {
            _isChasing = false;
            StartCoroutine(Randomize());
        }
        if (_hunger == 0 || _thirst == 0)
        {
            Destroy(this.gameObject);
            if (_rat == null && _water == null && _cat == null && _nothing)
            {
                StartCoroutine(Randomize());
                _nothing = false;
            }
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Rat") && _hunger <= _thirst)
        {
            _ratNull = true;
            _isChasing = true;
            _water = null;
            _rat = other.transform;
            _target = new Vector3(_rat.transform.position.x, 0, _rat.transform.position.z);
        }
        if (other.gameObject.CompareTag("Water") && _thirst <= _hunger)
        {
            _thirsty = true;
            _isChasing = true;
            _rat = null;
            _water = other.transform;
            _target = new Vector3(_water.transform.position.x, 0, _water.transform.position.z);
            _agent.destination = _target;
        }
        if (other.gameObject.CompareTag("Cat") && _reproduction > 40)
        {
            _breedable = true;
            _water = null;
            _rat = null;
            _cat = other.transform;
            _target = new Vector3(_cat.transform.position.x, 0, _cat.transform.position.z);
            _agent.destination = _target;
            StartCoroutine(Breed());
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Rat") == true && _thirst >= _hunger)
        {
            _isChasing = true;
            _rat = other.transform;
            _water = null;
            _target = new Vector3(_rat.transform.position.x, 0, _rat.transform.position.z);
        }
        if (other.gameObject.CompareTag("Cat") && _reproduction > 40)
        {
            _breedable = true;
            _water = null;
            _rat = null;
            _cat = other.transform;
            _target = new Vector3(_cat.transform.position.x, 0, _cat.transform.position.z);
            _agent.destination = _target;
            StartCoroutine(Breed());
        }
        if (other.gameObject.CompareTag("Water") && _thirst <= _hunger)
        {
            _thirsty = true;
            _isChasing = true;
            _rat = null;
            _water = other.transform;
            _target = new Vector3(_water.transform.position.x, 0, _water.transform.position.z);
            _agent.destination = _target;
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
            StartCoroutine(Randomize());
        }
        if (collision.gameObject.CompareTag("Water") && _thirsty)
        {
            StartCoroutine(Drink());
        }
        if (collision.gameObject.CompareTag("Cat") && _breedable)
        {
            StartCoroutine(Breed0());
        }
    }
   //void OnTriggerExit(Collider other)
   //{   
   //     if ((other.gameObject.CompareTag("Rat")||other.gameObject.CompareTag("Cat") || other.gameObject.CompareTag("Water")) && _thirsty)
   //     {
   //         _target = new Vector3(_water.transform.position.x, 0, _water.transform.position.z);
   //         _agent.destination = _target;
   //     } 
   // }

    private IEnumerator Randomize()
    {
        _ratNull = false;
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
            else if (_agent.CalculatePath(_target, _path) && _path.status == NavMeshPathStatus.PathComplete && _thirsty == true)
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
        _nothing = true;
    }
    public IEnumerator Breed()
    {
        _ratNull = false;
        _agent.speed = 6.5f;
        _agent.destination = _target;        
        yield return new WaitForSeconds(_time);
    }
    public IEnumerator Breed0()
    {
        float _rBaby = Random.Range(0, 2);
        _reproduction = 0f;
        _breedable = false;
        _isChasing = false;
        for (var i = 1; i == (int)_rBaby; i++)
        {
            Instantiate(_catObject, this.transform.position, Quaternion.identity);
        }
        StartCoroutine(Randomize());
        yield return new WaitForSeconds(_time);
    }
    public IEnumerator Drink()
    {
        _thirsty = false;
        _thirst = 30f;
        _isChasing = false;
        _water = null;
        StartCoroutine(Randomize());
        yield return new WaitForSeconds(_time);
    }
}
