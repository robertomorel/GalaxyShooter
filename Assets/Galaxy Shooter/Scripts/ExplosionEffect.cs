using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Destroy(this.gameObject, 4.0f);
    }

    // Update is called once per frame
    void Update() { }

}
