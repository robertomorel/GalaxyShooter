using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{

    [SerializeField]
    private float _speed = 3.0f;

    /*
        0 = triple shot
        1 = Speed boost
        2 = Shields
        */
    [SerializeField]
    private int _powerUpID;

    [SerializeField]
    private AudioClip _audioClip;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -7)
        {
            Destroy(this.gameObject);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        /*
            "other" vai guardar o objeto que colidiu com o powerup
            Obs.: Para uma colisão:
                1. Pelo menos um dos objetos devem ter um RigidBody;
                    1.1. Se a escala de gravidade > 0, o objeto cai
                2. Ambos devem ter um 'collider', box ou circle
                    2.1. Se Is Trigger está marcado, os objetos colidem, porém sem física.
                         Ou seja, não rebatem
            */
        Debug.Log("Colidiu com: " + other.name);

        if (other.tag == "Player")
        {
            // -- Pegar componente do tipo Player(classe)
            Player player = other.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_audioClip, Camera.main.transform.position);

            if (player != null)
            {

                if (_powerUpID == 0)
                {
                    //player.canTripleShot = true;
                    player.TripleShotPowerOn();
                }
                else if (_powerUpID == 1)
                {
                    player.SpeedBoosterPowerOn();
                }
                else if (_powerUpID == 2)
                {
                    player.EnableShield();
                }

            }

            // -- Chama a corotina para desligar o tiro triplo
            //StartCoroutine(player.TripleShotPowerDownRoutine());

            Destroy(this.gameObject);

        }

    }
}
