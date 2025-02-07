using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public Vector3 _target;
    public float _angle;
    public Vector3 _direction;
    private UnityEngine.AI.NavMeshAgent _agent;
    public float _time;
    UnityEngine.AI.NavMeshPath _path;
    public float _limitAngle;
    public float _distance;
    [SerializeField] private bool _isChasing = false;
    public Transform _cheese;
    public Transform _water;
    public Transform _rat;
    public float _hunger;
    public float _hungerMax;
    public float _thirst;
    public float _thirstMax;
    public float _reproduction;
    public float _reproductionMax;
    private float _timer;
    public float _timerSet;
    public bool _timerStart;
    public bool _beingChased = false;
    public bool _breedable = false;
    public bool _thirsty = false;
    public Transform _cat;
    public GameObject _ratObject;
    Rigidbody _rb;
    public bool _nothing = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        _path = new UnityEngine.AI.NavMeshPath();
        _hunger = _hungerMax; _thirst = _thirstMax;
        _timer = _timerSet;
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
        _reproduction += Time.deltaTime;
        _hunger = Mathf.Clamp(_hunger, 0f, _hungerMax);
        _thirst = Mathf.Clamp(_thirst, 0f, _thirstMax);
        _reproduction = Mathf.Clamp(_reproduction, 0f, _reproductionMax);
        if (_hunger == 0 || _thirst == 0)
        {
            Destroy(this.gameObject);
        }
        if (_rat == null && _water == null && _cheese == null && _nothing)
        {
            StartCoroutine(Randomize());
            _nothing = false;
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Cheese") && _hunger <= _thirst)
        {
            _isChasing = true;
            _water = null;
            _cheese = other.transform;
            _target = new Vector3(_cheese.transform.position.x, 0, _cheese.transform.position.z);
        }
        if (other.gameObject.CompareTag("Water") && _thirst <= _hunger)
        {
            _thirsty = true;
            _cheese = null;
            _isChasing = true;
            _water = other.transform;
            _target = new Vector3(_water.transform.position.x, 0, _water.transform.position.z);
        }
        if (other.gameObject.CompareTag("Rat") && _reproduction > 40)
        {
            _breedable = true;
            _water = null;
            _cheese = null;
            _rat = other.transform;
            _target = new Vector3(_rat.transform.position.x, 0, _rat.transform.position.z);
            _agent.destination = _target;
            StartCoroutine(Breed());
        }
        else if (other.gameObject.CompareTag("Cat"))
        {
            _beingChased = true;
            _cat = other.transform;
            _target = new Vector3(transform.position.x - _cat.transform.position.x, 0, transform.position.z - _cat.transform.position.z);
        }
    }
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Cat"))
        {
            _beingChased = true;
            _cat = other.transform;
            _target = new Vector3(transform.position.x - _cat.transform.position.x, 0, transform.position.z - _cat.transform.position.z);
            StartCoroutine(Randomize());
        }
        if (other.gameObject.CompareTag("Cheese") && _hunger <= _thirst)
        {
            _isChasing = true;
            _cheese = other.transform;
            _target = new Vector3(_cheese.transform.position.x, 0, _cheese.transform.position.z);
        }
        if (other.gameObject.CompareTag("Rat") && _reproduction > 40)
        {
            _breedable = true;
            _water = null;
            _cheese = null;
            _rat = other.transform;
            _target = new Vector3(_rat.transform.position.x, 0, _rat.transform.position.z);
            _agent.destination = _target;
            StartCoroutine(Breed());

        }
        if (other.gameObject.CompareTag("Water") && _thirst <= _hunger)
        {
            _thirsty = true;
            _cheese = null;
            _isChasing = true;
            _water = other.transform;
            _target = new Vector3(_water.transform.position.x, 0, _water.transform.position.z);
            _agent.destination = _target;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Cat"))
        {
            _beingChased = false;
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cheese"))
        {
            GameObject cheese = collision.gameObject;
            Destroy(cheese.gameObject);
            _hunger += 18f;
            _isChasing = false;
            _timerStart = true;
            StartCoroutine(Randomize());
        }
        if (collision.gameObject.CompareTag("Water") && _thirsty)
        {
            StartCoroutine(Drink());
        }
        if (collision.gameObject.CompareTag("Rat") && _breedable)
        {
            StartCoroutine(Breed0());
        }
    }
    private IEnumerator Randomize()
    {
        while (_isChasing == false && _beingChased == false)
        {
            _agent.speed = 3.5f;
            if (_agent.CalculatePath(_target, _path) && _path.status == UnityEngine.AI.NavMeshPathStatus.PathComplete && _thirsty == false)
            {
                _direction = transform.forward;
                _angle = (Random.Range(-_angle - _limitAngle, _angle + _limitAngle) % 360);
                Quaternion rotation = Quaternion.AngleAxis(_angle, Vector3.up);
                _direction = rotation * _direction;
                _target = transform.position + _direction * _distance;
                _agent.destination = _target;
                yield return new WaitForSeconds(_time);
            }
            else if (_agent.CalculatePath(_target, _path) && _path.status == UnityEngine.AI.NavMeshPathStatus.PathComplete && _thirsty == true)
            {
                _direction = transform.forward;
                _angle = (Random.Range(-_angle - _limitAngle, _angle + _limitAngle) % 360);
                Quaternion rotation = Quaternion.AngleAxis(_angle, Vector3.up);
                _direction = rotation * _direction;
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
                yield return new WaitForSeconds(_time * 3);
            }
        }
       
        while (_isChasing == true && _beingChased == false)
        {
            _agent.speed = 4f;
            _agent.destination = _target;
            yield return new WaitForSeconds(_time);
        }
        while (_beingChased == true)
        {
            _agent.speed = 4f;
            _agent.destination = _target;
            yield return new WaitForSeconds(_time);
        }
        _nothing = true;
        yield return new WaitForSeconds(_time);
    }
    public IEnumerator Breed()
    {
        _agent.destination = _target;
        yield return new WaitForSeconds(_time);
    }
    public IEnumerator Breed0()
    {
        _reproduction = 0f;
        _breedable = false;
        _isChasing = false;
        float _rBaby = Random.Range(0, 2);
        for (var i = 1; i == (int)_rBaby; i++)
        {
            Instantiate(_ratObject, this.transform.position, Quaternion.identity);
        }
        StartCoroutine(Randomize());
        yield return new WaitForSeconds(_time);
    }
    public IEnumerator Drink()
    {
        _thirsty = false;
        _thirst = 30f;
        _water = null;
        _isChasing = false;
        StartCoroutine(Randomize());
        yield return new WaitForSeconds(_time);
    }
}
