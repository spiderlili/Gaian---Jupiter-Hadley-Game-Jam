using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneMgr : MonoBehaviour {

    public static SceneMgr Instance;

    //player variables
    public float _totalHealth = 100f;
    [SerializeField] float _healthFromKill = 5f;
    [SerializeField] float _healthFromFood = 3f;

    [SerializeField] int _foodCollected = 0;
    [SerializeField] int _animalsKilled = 0;
    public int _animalsInWorld = 0;
    public float _timeSinceLastKill;
    public float _timeUntilHappy = 3f;
    [SerializeField] GameObject _animalObj;
    [SerializeField] Transform[] _spawnPoint;

    //animal spawn variables
    [SerializeField] float _baseSpawnFrequency;
    [SerializeField] float _curSpawnFrequency;
    [SerializeField] float _populationSpawnMultiplier = 15f;
    [SerializeField] int _maxAnimalNumber = 200;

    bool _bigSpawn;

    float _timeTilNextSpawn;

    //animal aggression variables
    public bool AnimalsAreAggressive;
    [SerializeField] float _aggressionDuration;
    float _aggressiveTimeLeft;

    //scene variables
    public bool GameStarted = false;
    public bool GameOver = false;
    float _healthLeft;
    [SerializeField] float TimeElapsed = 0f;

    //UI things
    [SerializeField] Text TimeElapsedTx;
    public Image HealthBar;

    public Text AnimalsKilledTx;

    public GameObject GameOverScreen;
    public GameObject StartSequenceScreen;
    [SerializeField] Text _finishText;
    [SerializeField] Text _summaryText;

    public Text StartSequenceCountdown;
    int StartSeqTime = 1;

    //Audio things
    public AudioClip failSound;
    public AudioClip winSound;
    private AudioSource source;
    private bool playAudio = false; //fix audio update bug

    private void Awake()
    {
        Instance = this;
        _curSpawnFrequency = _baseSpawnFrequency;
        source = GetComponent<AudioSource>();
        Time.timeScale = 1f;
    }

    void Start ()
    {
        _healthLeft = 50f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
            StartCoroutine("StartGameSequence");
    }

    IEnumerator StartGameSequence()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        StartSequenceScreen.SetActive(true);
        
        StartSequenceCountdown.text = "Go!";
        yield return new WaitForSeconds(1f);
        StartSequenceScreen.SetActive(false);
        GameStarted = true;
    }

    void Update ()
    {
        //if (GameOver || !GameStarted)
        //    return;

        TimeElapsed += Time.deltaTime;
        TimeElapsedTx.text = TimeElapsed.ToString("F0");

        if(!AnimalsAreAggressive)
            _timeSinceLastKill += Time.deltaTime;

        if (_healthLeft > _totalHealth)        
            _healthLeft = _totalHealth;
        
        _healthLeft -= Time.deltaTime * 1.66666666667f;
        HealthBar.fillAmount = _healthLeft / 100;

        _timeTilNextSpawn += Time.deltaTime;

        if(_timeTilNextSpawn >= _curSpawnFrequency)
        {
            Debug.LogWarning("SPAWN");
            SpawnCreature();
            _timeTilNextSpawn = 0;
        }

        if (_aggressiveTimeLeft > 0)       
            _aggressiveTimeLeft -= Time.deltaTime;
        
        else        
            AnimalsAreAggressive = false;        

        if (_animalsInWorld < 20 && !_bigSpawn)
            _baseSpawnFrequency /= 2;
        
        if (_healthLeft <= 0)
            SetGameOver();
	}

    public void AddScore()
    {
        _foodCollected++;
        //play a noise (:
    }

    public void AddKillCount()
    {
        AdjustPopulation(-1);
        _animalsKilled++;
        _timeSinceLastKill = 0f;
        AnimalsAreAggressive = true;
        _aggressiveTimeLeft = _aggressionDuration;
    }

    public void AddFood(int foodType)
    {
        if(foodType == 0)        
            _healthLeft += _healthFromFood;        
        else
            _healthLeft += _healthFromKill;

        _healthLeft += _healthFromFood;
        _foodCollected++;
        //play noise
    }

    public void SpawnCreature()
    {
        if (_animalsInWorld > _maxAnimalNumber)        
            return;
        
        else if (_animalsInWorld < 20)
        {
            Debug.LogError("Not spawning animals any more");
            return;
        }
                
        var selectedSpawnPoint = Random.Range(0, _spawnPoint.Length);
        var newAnimal = Instantiate(_animalObj, _spawnPoint[selectedSpawnPoint].position, _spawnPoint[selectedSpawnPoint].rotation); //pool these later[]
    }

    public void AdjustPopulation(int animalsLeft)
    {
        _animalsInWorld += animalsLeft;
        _curSpawnFrequency = _baseSpawnFrequency / (_animalsInWorld * _populationSpawnMultiplier);
        print("animals in world is now " + _animalsInWorld);
    }

    public void SetGameOver()
    {
        Time.timeScale = 0f;

        GameOver = true;
        
        if (_animalsInWorld <= 0)
        {
            _finishText.text = "Oops";
            _summaryText.text = "You killed every last one..";

            if (!playAudio)
            {
                source.PlayOneShot(failSound);
                playAudio = true;
            }
        }
        else if (_animalsKilled > 0)
        {
            _finishText.text = "You Died";
            _summaryText.text = "You survived for " + TimeElapsed.ToString("F0") + " Seconds! You killed " + _animalsKilled + " animals and collected " + _foodCollected + " food.";

            if (!playAudio)
            {
                source.PlayOneShot(failSound);
                playAudio = true;
            }
        }
        else if (_animalsKilled <= 0 && _healthLeft >= _totalHealth)
        {
            _finishText.text = "You Win!";
            _summaryText.text = "You found a sustainable way to survive.";

            if (!playAudio)
            {
                source.PlayOneShot(winSound);
                playAudio = true;
            }
        }
        else
        {
            _finishText.text = "You starved!";
            _summaryText.text = "You survived for " + TimeElapsed.ToString("F0") + " Seconds! You killed " + _animalsKilled + " animals and collected " + _foodCollected + " food.";

            if (!playAudio)
            {
                source.PlayOneShot(failSound);
                playAudio = true;
            }
        }

        GameOverScreen.SetActive(true);
    }
}
