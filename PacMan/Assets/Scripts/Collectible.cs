using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameplayManager.instance.CreatePellet();
        GameplayManager.instance.resetGame.AddListener(Reset);
    }

    private void Reset()
    {
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D co)
    {
        GameplayManager.instance.CollectPellet();
        gameObject.SetActive(false);
    }
}
