using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public bool canTripleShot = false;
    public bool canSpeedBooster = false;
    public bool shieldOn = false;

    public int _lifes = 3;
    /*
        [SerializeField] 
        --> Usado para que variáveis privadas apareçam no controle do Unity
        */

    [SerializeField]
    private float _speed = 5.0f;

    [SerializeField]
    private GameObject _laserPrefab;

    [SerializeField]
    private GameObject _explodePrefabe;

    [SerializeField]
    private GameObject _shieldGameObject;

    /*
        [SerializeField]
        private GameObject _tripleLaserPrefab;
        --> Opção de uso para o laser triplo com empty object   
        */

    /*
        --> Sistema CoolDown:
                -- Taxa de Tiro seguido: 0.25f segundos
        --> Time.time: retorna o tempo de jogo
         */
    [SerializeField]
    private float _fireRate = 0.25f;

    private float _canFire = 0.0f;

    private UIManager _uiManager;

    private GameManager _gameManager;

    private SpawnManage _spawnManager;

    private AudioSource _audioSource;

    [SerializeField]
    private GameObject[] _engines;

    private int hitCount = 0;

    // Use this for initialization
    private void Start()
    {

        //Debug.Log( "Start foi chamado no "+ name +"!" );
        //Debug.Log( "x pos: " + transform.position.x );
        //Debug.Log( "3D pos:" + transform.position );
        transform.position = new Vector3(0, 0, 0);

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_uiManager)
        {
            _uiManager.UpDateLives(_lifes);
        }

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _spawnManager = GameObject.Find("SpawnManage").GetComponent<SpawnManage>();

        if (_spawnManager)
        {
            _spawnManager.StarSpawnRoutines();
        }

        _audioSource = GetComponent<AudioSource>();

        hitCount = 0;

    }

    // Update is called once per frame
    private void Update()
    {

        Movimento();
        Shoot();

    }

    private void Shoot()
    {

        //-- Criar laser usando espaço ou left click
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButton(0))
        {
            /* --> Instanciar o prefab laser...
                ... na posição do player...
                ... na rotação padrão ou sem rotação 
                Instantiate(laserPrefab, transform.position, Quaternion.identity);  
                */
            if (Time.time > _canFire)
            { // -- Compara se pode atirar considerando o tempo _fireRate segundos por tiro

                /*
                    Outra opção seria criar o componente AudioSource
                    para o prefabe 'Laser' e settar o som preciso.
                    Marcando o ckeckbox 'Play On Awake'
                */
                _audioSource.Play();

                if (canTripleShot)
                {

                    // -- Esquerda    
                    Instantiate(_laserPrefab, transform.position + new Vector3(-0.55f, 0.06f, 0), Quaternion.identity);
                    // -- Centro
                    Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.88f, 0), Quaternion.identity);
                    // -- Direira
                    Instantiate(_laserPrefab, transform.position + new Vector3(0.55f, 0.06f, 0), Quaternion.identity);

                    /*
                        Instantiate(_tripleLaserPrefab, transform.position, Quaternion.identity);
                        --> Opção de uso para o laser triplo com empty object
                        */

                }
                else
                {
                    Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.88f, 0), Quaternion.identity);
                }

                // -- _canFire é settado para o momento atual + 0.25 segundos
                _canFire = Time.time + _fireRate;
                // -- Só pode atirar a cada 0.25 segundos

            }

        }

    }

    private void Movimento()
    {

        //transform.Translate( new Vector3(1, 0, 0) * 1 * speed );
        //transform.Translate( Vector3.right * Time.deltaTime * speed );

        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");

        float speedBooster = canSpeedBooster ? 1.5f : 1;

        transform.Translate(Vector3.right * Time.deltaTime * _speed * inputHorizontal * speedBooster);
        transform.Translate(Vector3.up * Time.deltaTime * _speed * inputVertical * speedBooster);

        // -- Lógica para que o player não saia da tela
        /*
        if (transform.position.y < -4.2f) {
            transform.position = new Vector3(transform.position.x, -4.2f, 0);
        } else if (transform.position.y > 4.2f) {
            transform.position = new Vector3(transform.position.x, 4.2f, 0);
        } else if (transform.position.x < -8.3f) {
            transform.position = new Vector3(-8.3f, transform.position.y, 0);
        } else if (transform.position.x > 8.3f) {
            transform.position = new Vector3(8.3f, transform.position.y, 0);
        }
        */
        transform.position = (transform.position.y < -4.2f) ?
            new Vector3(transform.position.x, -4.2f, 0) :
            (transform.position.y > 4.2f) ?
            new Vector3(transform.position.x, 4.2f, 0) :
            (transform.position.x < -9.5f) ?
            new Vector3(9.5f, transform.position.y, 0) :
            (transform.position.x > 9.5f) ?
            new Vector3(-9.5f, transform.position.y, 0) :
            new Vector3(transform.position.x, transform.position.y, 0);

    }

    public void DamageControl()
    {

        if (shieldOn)
        {
            shieldOn = false;
            _shieldGameObject.SetActive(false);
            return;
        }
        else
        {
            _lifes--;
            _uiManager.UpDateLives(_lifes);
        }

        hitCount++;

        // -- Controlar as animações de motor pegando fogo
        if (hitCount == 1)
        {
            _engines[0].SetActive(true);
        }
        else if (hitCount == 2)
        {
            _engines[1].SetActive(true);
        }

        if (_lifes == 0)
        {
            Destroy(this.gameObject);
            Instantiate(_explodePrefabe, transform.position, Quaternion.identity);
            _gameManager.gameOver = true;
            _uiManager.ShowTitleScreen();
        }

    }

    public void TripleShotPowerOn()
    {
        canTripleShot = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    /*
        Criando corotina para desligar o tiro triplo.
        A corotina permite a variável 'yield'.
        Muito úteis para ações que necessitem de delays
        */
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        canTripleShot = false;
    }

    public void SpeedBoosterPowerOn()
    {
        canSpeedBooster = true;
        StartCoroutine(SpeedBoosterPowerDown());
    }

    IEnumerator SpeedBoosterPowerDown()
    {
        yield return new WaitForSeconds(5.0f);
        canSpeedBooster = false;
    }

    public void EnableShield()
    {
        Debug.Log("Entrou no EnableShield()");
        shieldOn = true;
        _shieldGameObject.SetActive(true);
    }

}
