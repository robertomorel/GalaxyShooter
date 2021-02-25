using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public bool gameOver = true;

    [SerializeField]
    private GameObject _playerPrefabe;

    private UIManager _uiManager;

    // Use this for initialization
    void Start () {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();        
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(_playerPrefabe, Vector3.zero, Quaternion.identity);
            gameOver = false;
            _uiManager.HideTitleScreen();
        }

    }

}
