using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool canTripleShot = false;
    public bool canSpeedBoost = false;
    public bool shieldsActive = false;
    public int lives = 3;

    [SerializeField]
    private GameObject _explosionPrefab;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _tripleShotPrefab;

    [SerializeField]
    private GameObject _shieldGameObject;

    [SerializeField]
    private GameObject[] _engines;

    [SerializeField]
    private float _fireRate = 0.25F;
    private float _canFire = 0.0F; // canFire is the next fire and it's used to determine how much has passed

    [SerializeField] // any variable underneath it, instead of private, can be appeared in the inspector
    private float _speed = 5.0F;

    private UIManager _uiManager;
    private GameManager _gameManager;
    private SpawnManager _spawnManager;
    private AudioSource _audioSource;

    private int hitCount = 0;
    // Start is called before the first frame update
    private void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_uiManager != null)
        {
            _uiManager.UpdateLives(lives);
        }

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager != null)
        {
            _spawnManager.StartSpawnRoutines();
        }

        _audioSource = GetComponent<AudioSource>();

        hitCount = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        Movement();

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0))
        {
            Shoot();            
        }

    }

    private void Shoot()
    {
        if(Time.time > _canFire)
        {
            _audioSource.Play();
            if(canTripleShot == true)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.92F, 0), Quaternion.identity);
            }
            
            _canFire = Time.time + _fireRate;   
        }
    }

    private void Movement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if(canSpeedBoost == true)
        { 
            transform.Translate(Vector3.right * Time.deltaTime * _speed * 3.5F * horizontalInput);
            transform.Translate(Vector3.up * Time.deltaTime * _speed * 3.5F * verticalInput);
        }
        else
        {
            transform.Translate(Vector3.right * Time.deltaTime * _speed * horizontalInput);
            transform.Translate(Vector3.up * Time.deltaTime * _speed * verticalInput);
        }
        

        if (transform.position.y > 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y < -4.26F)
        {
            transform.position = new Vector3(transform.position.x, -4.26F,0);
        }
        else if (transform.position.x > 8.28F)
        {
            transform.position = new Vector3(-8.28F, transform.position.y, 0);
        }
        else if (transform.position.x < -8.27F)
        {
            transform.position = new Vector3(8.27F, transform.position.y, 0);
        }        
    }
    

    public void Damage()
    {
        if (shieldsActive == true)
        {
            shieldsActive = false; // do this because we've 1 free hit
            _shieldGameObject.SetActive(false);
            // break the function because we don't want to continue with damage function
            return; 
        }

        hitCount++;
        if (hitCount == 1)
        {
            // turn left engine failure on
            _engines[0].SetActive(true);
        }
        else if (hitCount == 2)
        {
            // turn right engine failure on
            _engines[1].SetActive(true);
        }

        // subtract one life from the player
        lives--;
        _uiManager.UpdateLives(lives);

        // if lives < 1(meaning zero)
        // destroy this object
        if (lives < 1)
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            _gameManager.gameOver = true;
            _uiManager.ShowTitleScreen();
            Destroy(this.gameObject);
        }


    }

    public void TripleShotPowerUpOn()
    {
        canTripleShot = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    
    public void SpeedBoostPowerUpOn()
    {
        canSpeedBoost = true;
        StartCoroutine(SpeedBoostDownRoutine());
    }

    public void EnableShields()
    {
        shieldsActive = true;
        _shieldGameObject.SetActive(true); // turning the gameObject on, in this case, gameObject is shield
    }

    public IEnumerator TripleShotPowerDownRoutine()
    {
        // delay tripleshot for 5 seconds and after that we disable it
        yield return new WaitForSeconds(5.0F);
        canTripleShot = false;
    }

    public IEnumerator SpeedBoostDownRoutine()
    {
        // delay tripleshot for 5 seconds and after that we disable it
        yield return new WaitForSeconds(5.0F);
        canSpeedBoost = false;
    }
}
