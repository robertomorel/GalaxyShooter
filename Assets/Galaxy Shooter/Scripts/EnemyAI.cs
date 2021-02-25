using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    [SerializeField]
    private float _speed = 5.0f;

    [SerializeField]
    private GameObject _explodePrefabe;

    private UIManager _uiManager;

    //private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _audioClip;

    // Use this for initialization
    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        //_audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        /*
            O inimigo será gerado sempre na posição Y com 6.6f
            e na posição X entre -7.7f e 7.7f
            */
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -6.6f)
        {
            float randomX = Random.Range(-7.7f, 7.7f);
            transform.position = new Vector3(randomX, 6.6f, 0);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        Debug.Log("Colidiu com: " + other.name);

        if (other.tag == "Player")
        {
            // -- Pegar componente do tipo Player(classe)
            Player player = other.GetComponent<Player>();

            if (player != null)
            {

                player.DamageControl();
                DestroyObject();

            }

        }
        else if (other.tag == "Laser")
        {
            DestroyObject();
            Destroy(other.gameObject);
        }

    }

    private void DestroyObject()
    {

        Instantiate(_explodePrefabe, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
        _uiManager.UpDateScore();
        //_audioSource.Play();
        /*
            Código para rodar um clipe de som independente do objeto 
            estar destruído ou não.
            Irá rodar o som na câmera (2D)
            Apenas 'transform.position' seria um som em 3D 
            */
        AudioSource.PlayClipAtPoint(_audioClip, Camera.main.transform.position);

    }

}
