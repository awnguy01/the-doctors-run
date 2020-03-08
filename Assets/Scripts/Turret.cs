using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject projectile;
    public float upperYBound = 3.14f;
    public float lowerYBound = -3.14f;
    public float baseSpeed = 1f;
    private float currSpeed = 1f;
    private Vector3 originalStartPos;
    private float targetY = 0;
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        transform.position = originalStartPos = GetStartingPosition();
        // StartCoroutine(FireMissiles());
    }

    void OnEnable()
    {
        StartCoroutine(FireMissiles());
    }

    // Update is called once per frame
    void Update()
    {
        ScreenResponsiveTurretAdjust();
        MoveRandomY();
        if (GeneratorScript.score > 0)
        {
            currSpeed = baseSpeed + 1 * Mathf.Log(GeneratorScript.score);
        }
        else
        {
            currSpeed = baseSpeed;
        }
    }

    void ScreenResponsiveTurretAdjust()
    {
        Vector3 currPos = transform.position;

        if (currPos.x != originalStartPos.x)
        {
            transform.position = originalStartPos = new Vector3(GetStartingPosition().x, transform.position.y, 0);
        }
    }

    void MoveRandomY()
    {
        if (Mathf.Abs(transform.position.y - targetY) > 0)
        {
            Vector3 targetPos = new Vector3(transform.position.x, targetY, 0);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, currSpeed * Time.deltaTime);
        }
        else
        {
            targetY = Random.Range(lowerYBound, upperYBound);
        }
    }

    Vector3 GetStartingPosition()
    {
        float cameraWidth = mainCamera.pixelWidth;
        float turretWidth = GetComponent<SpriteRenderer>().bounds.size.x;

        Vector3 currPosition = transform.position;
        Vector3 screenRight = new Vector3(cameraWidth, 0, 0);
        Vector3 worldRight = mainCamera.ScreenToWorldPoint(screenRight);


        return new Vector3(worldRight.x - turretWidth / 2, transform.position.y, 0);
    }

    IEnumerator FireMissiles()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            Instantiate(projectile, transform.position, projectile.transform.rotation);
        }
    }
}
