using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //gun fire variables///////////////////////////
    [SerializeField] GameObject projectile;
    [SerializeField] float _fireRate = 0.1F;

    [SerializeField] float nextFire = 0.5F;
    GameObject newProjectile;
    float _timeTilNextFire = 0.0F;
    [SerializeField] float _bulletSpeed = 25f;
    ///////////////////////////////////////////////

    //sprite variables
    [SerializeField] Sprite front;
    [SerializeField] Sprite back;
    [SerializeField] Sprite left;
    [SerializeField] Sprite right;


    public static Player Instance;
    [SerializeField] float MaxSpeed = 10f;
    float MoveHorizontal;
    float MoveVertical;
    bool facingRight;
    SpriteRenderer _playerSprite;
    AudioSource _shoot;
    float idleTimer;

    [SerializeField] float _currentSpeed;

    Rigidbody2D _rb;

    SpriteRenderer _mySprite;

    void Start()
    {
        Instance = this;
        _playerSprite = GetComponent<SpriteRenderer>();
        _shoot = GetComponent<AudioSource>();
        _rb = GetComponent<Rigidbody2D>();
        _mySprite = GetComponent<SpriteRenderer>();

        //_deathAnim = _deathLines.GetComponent<Animator>(); []nice to have
        //_deathSprite = _deathLines.GetComponent<SpriteRenderer>();

    }

    void Update()
    {       

        //if (SceneMgr.Instance.GameOver)
        //    return;

        _mySprite.sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1; //this means the dog will be drawn in fron of or behind items if it is higher/lower than them

        _timeTilNextFire = _timeTilNextFire + Time.deltaTime;

        //Mouse stuff//
        if (Input.GetButton("Fire1") && _timeTilNextFire > nextFire)
        {
            nextFire = _timeTilNextFire + _fireRate;
            newProjectile = Instantiate(projectile, transform.position, transform.rotation) as GameObject;

            // create code here that animates the newProjectile
            //if (!_shoot.isPlaying)
                _shoot.Play();
            //test

            Vector2 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
            Vector2 direction = target - myPos;
            direction.Normalize();
            newProjectile.GetComponent<Rigidbody2D>().velocity = direction * _bulletSpeed;
            //

            nextFire = nextFire - _timeTilNextFire;
            _timeTilNextFire = 0.0F;
        }
        // Controller Stuff
        /*
        if (_timeTilNextFire > nextFire)
        {         
            var direction = new Vector2(Input.GetAxis("Right_Horizontal"), Input.GetAxis("Right_Vertical"));

            if (direction.magnitude < 0.1f)
            {
                print("shouldn't execute any more code");
                return;
            }

            var rot = Quaternion.LookRotation(direction, Vector3.up);
            nextFire = _timeTilNextFire + _fireRate;
            newProjectile = Instantiate(projectile, transform.position, rot) as GameObject;
            print(direction.magnitude);
            print("is executing code!");            

                // create code here that animates the newProjectile
                if (!_shoot.isPlaying)
                    _shoot.Play();
            //test


            newProjectile.GetComponent<Rigidbody2D>().AddForce(transform.right);
                //

                nextFire = nextFire - _timeTilNextFire;
                _timeTilNextFire = 0.0F; 
                */
        //}                    

        if (facingRight)
        {
            if (!_playerSprite.flipX)            
                _playerSprite.flipX = true;            
        }
        else
        {
            if (_playerSprite.flipX)            
                _playerSprite.flipX = false;            
        }

        if (MoveHorizontal > 0 && !facingRight)
            facingRight = true;
        else if (MoveHorizontal < 0 && facingRight)
            facingRight = false;

        if (MoveHorizontal > 0)
        {
            if(_playerSprite != left)
                _playerSprite.sprite = left;
        }
        if(MoveVertical > 0)
        {
            if (_playerSprite != back)
                _playerSprite.sprite = back;
        }
        else if (MoveVertical < 0)
        {
            if (_playerSprite != front)
                _playerSprite.sprite = front;
        }

    }

    void FixedUpdate()
    {
        MoveHorizontal = Input.GetAxis("Horizontal");
        MoveVertical = Input.GetAxis("Vertical");

        _rb.velocity = new Vector2(MoveHorizontal * MaxSpeed, MoveVertical * MaxSpeed);
        _currentSpeed = MoveVertical * MaxSpeed;
    }   

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Animal")
        {
            if (SceneMgr.Instance.AnimalsAreAggressive)
            {
                SceneMgr.Instance.SetGameOver();
            }  
        }

        else if(other.gameObject.tag == "Food")
        {
            SceneMgr.Instance.AddFood(0);
        }

        else if (other.gameObject.tag == "Animal Food")
        {
            SceneMgr.Instance.AddFood(1);
            Destroy(other.gameObject);
        }
    }
}
