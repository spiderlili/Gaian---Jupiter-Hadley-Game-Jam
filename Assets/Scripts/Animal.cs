using UnityEngine;

public class Animal : MonoBehaviour {

    Transform _player;
    Rigidbody2D _rb;
    [SerializeField] GameObject _foodDrop;
    [SerializeField] GameObject _blood;

    [SerializeField] float _distBuffer = 15f;
    [SerializeField] float _minMoveSpeed;
    [SerializeField] float _maxMoveSpeed;
    [SerializeField] float _myMoveSpeed;
    [SerializeField] bool _varyMoveSpeed = true;
    [SerializeField] int _animalSadNo = 20;


    //wandering variables
    [SerializeField] float _timeToSpendMoving;
    [SerializeField] float _timeToSpendIdle = 0f;
    float _timeSpentWalking = 0f;
    float _timeSpentIdle = 0f;

    //sprite variables
    SpriteRenderer _sprite;
    [SerializeField] Sprite _happy;
    [SerializeField] Sprite _neutral;
    [SerializeField] Sprite _sad;
    [SerializeField] Sprite _angry;

    bool _isHappy;
    bool _isNeutral;
    bool _isSad;
    bool _isAngry;

    Vector2 _curDirection;
    float _randomX;  
    float _randomY;  

    bool _isMoving = true;

    void Start ()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _player = GameObject.Find("Player").GetComponent<Transform>();

        SceneMgr.Instance.AdjustPopulation(1);

        if (_varyMoveSpeed)
        {
            _myMoveSpeed = Random.Range(_minMoveSpeed, _maxMoveSpeed);
            _timeToSpendMoving = Random.Range(2f, 4f);
            _timeToSpendIdle = Random.Range(3f, 6f);
        }

        _randomX = Random.Range(-3, 3);
        _randomY = Random.Range(-3, 3);
        _isMoving = true;
    }

	void Update ()
    {        

        var distFromPlayer = Vector2.Distance(transform.position, _player.transform.position);

        if (distFromPlayer >= 20f && _isSad)
        {
            //transform.position = Vector2.MoveTowards(transform.position, _player.position, _myMoveSpeed * Time.deltaTime);
            Vector3 dir = _player.transform.position - transform.position;
            dir = -dir.normalized;
            _rb.AddForce(dir * _myMoveSpeed);
        }

        if (SceneMgr.Instance.AnimalsAreAggressive)
        {
            if (!_isAngry)
            {
                SetToAngry();
            }

            if(gameObject.tag == "Animal")
                transform.position = Vector2.MoveTowards(transform.position, _player.position, _myMoveSpeed * Time.deltaTime);
            //else if (gameObject.tag == "Animal Idle")
                //transform.position = Vector2.MoveTowards(transform.position, _player.position, -_myMoveSpeed * Time.deltaTime);



        }

        else
        {     
            if (_isAngry ) //we houldnt ever be angry if this code gets read so we'll turn it off
            {
                SetToNeutral();
            }
            if (_happy && SceneMgr.Instance._timeSinceLastKill >= SceneMgr.Instance._timeUntilHappy)
            {
                SetToHappy();
            }

            if (SceneMgr.Instance._animalsInWorld < _animalSadNo)
            {
                SetToSad();
            }
        }

        if (_isMoving)
        {
            _timeSpentWalking += Time.deltaTime;

            if (_timeSpentWalking < _timeToSpendMoving) //if we haven't been walking too long
            {
                transform.Translate(new Vector2(_curDirection.x, _curDirection.y) * Time.deltaTime); //walk in the current directions
                _timeSpentWalking += Time.deltaTime;

            }
            else
            {
                _timeSpentWalking = 0f;
                _isMoving = false;
            }
        }
        else
        {
            _timeSpentIdle += Time.deltaTime;

            if (_timeSpentIdle < _timeToSpendIdle) //if we haven't been idle too long            
                return;

            _timeSpentIdle = 0f;
            _randomX = Random.Range(-_myMoveSpeed, _myMoveSpeed);
            _randomY = Random.Range(-_myMoveSpeed, _myMoveSpeed);
            _curDirection = new Vector2(_randomX, _randomY);
            _isMoving = true;
        }
    }
    
    void SetToHappy()
    {

        if (SceneMgr.Instance._animalsInWorld <= _animalSadNo)
            return;

        _isHappy = true;
        _isNeutral = false;
        _isSad = false;
        _isAngry = false;

        _sprite.sprite = _happy;

    }

    void SetToNeutral()
    {

        if (SceneMgr.Instance._animalsInWorld <= _animalSadNo)
            return;

        _isHappy = false;
        _isNeutral = true;
        _isSad = false;
        _isAngry = false;

        _sprite.sprite = _neutral;

    }

    void SetToSad()
    {  
        _isHappy = false;
        _isNeutral = false;
        _isSad = true;
        _isAngry = false;

        _sprite.sprite = _sad;

    }

    void SetToAngry()
    {

        if (SceneMgr.Instance._animalsInWorld <= _animalSadNo)
            return;

        _isHappy = false;
        _isNeutral = false;
        _isSad = false;
        _isAngry = true;

        _sprite.sprite = _angry;

    }



    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Bullet")
        {
            Destroy(other.gameObject);

            SceneMgr.Instance.AddKillCount();
            Instantiate(_foodDrop, transform.position, transform.rotation);
            Instantiate(_blood, transform.position, transform.rotation);

            Destroy(gameObject);
        
        }
    }
}
    

