using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public int maxLives = 5;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "Coin":
                GeneratorScript.score++;
                break;
            case "Missile":
                GeneratorScript.lives--;
                break;
            case "Potion":
                if (GeneratorScript.lives < maxLives)
                {
                    GeneratorScript.lives++;
                }
                break;
        }
        other.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
