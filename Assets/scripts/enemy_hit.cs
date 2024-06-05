using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class enemy_hit : MonoBehaviour
{
    public Action onDeath;
    public Action onSpawn;

    // Start is called before the first frame update
    void Start()
    {
        onSpawn?.Invoke();
    }

    // Update is called once per frame
    public void Death()
    {
        onDeath?.Invoke();
        Destroy(gameObject);   
    }
}
