using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{

    public float baseSpeed = 1f;
    private float currSpeed = 1f;
    private float remainingDist;
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        remainingDist = 2f * mainCamera.orthographicSize * mainCamera.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        if (GeneratorScript.score > 0)
        {
            currSpeed = baseSpeed + 1 * Mathf.Log(GeneratorScript.score);
        }
        else
        {
            currSpeed = baseSpeed;
        }
        remainingDist += -1 * currSpeed * Time.deltaTime;
        transform.position += new Vector3(-1, 0, 0) * currSpeed * Time.deltaTime;
        if (remainingDist <= 0)
        {
            Destroy(gameObject);
        }
    }
}
