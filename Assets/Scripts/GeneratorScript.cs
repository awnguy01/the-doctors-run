/*
* Copyright (c) 2018 Razeware LLC
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* Notwithstanding the foregoing, you may not use, copy, modify, merge, publish,
* distribute, sublicense, create a derivative work, and/or sell copies of the
* Software in any work that is designed, intended, or marketed for pedagogical or
* instructional purposes related to programming, coding, application development,
* or information technology.  Permission for such use, copying, modification,
* merger, publication, distribution, sublicensing, creation of derivative works,
* or sale is expressly withheld.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorScript : MonoBehaviour
{
    public GameObject[] availableRooms;
    public List<GameObject> currentRooms;
    private float screenWidthInPoints;

    public GameObject[] availableObjects;
    public List<GameObject> objects;

    public GameObject[] availableProjectiles;
    public List<GameObject> projectiles;

    public GameObject healthIcon;
    private List<GameObject> healthArray;
    public float objectsMinDistance = 5.0f;
    public float objectsMaxDistance = 10.0f;

    public float objectsMinY = -1.4f;
    public float objectsMaxY = 1.4f;

    public float objectsMinRotation = -45.0f;
    public float objectsMaxRotation = 45.0f;

    public static int score = 0;
    public static int lives = 3;
    private Text scoreText;
    private GameObject turret;
    private GameObject player;
    private GameObject gameOverUI;

    // Use this for initialization
    private void Start()
    {
        turret = GameObject.Find("Turret");
        player = GameObject.FindGameObjectWithTag("Player");

        float height = 2.0f * Camera.main.orthographicSize;
        screenWidthInPoints = height * Camera.main.aspect;
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        gameOverUI = GameObject.Find("GameOverUI");
        FillHearts();
        GameObject.Find("RestartButton").GetComponent<Button>().onClick.AddListener(RestartGame);
        gameOverUI.SetActive(false);

        StartCoroutine(GeneratorCheck());
    }

    // Update is called once per frame
    private void Update()
    {
        scoreText.text = "Score " + score;
        if (healthArray.Count > 0 && healthArray.Count > lives)
        {
            for (int i = lives; i < healthArray.Count; i++)
            {
                healthArray[i].SetActive(false);
            }
            healthArray.RemoveRange(lives, healthArray.Count - lives);
        }
        else if (healthArray.Count < lives)
        {
            FillHearts();
        }
        else if (healthArray.Count <= 0)
        {
            if (turret)
            {
                turret.SetActive(false);
            }
            if (player)
            {
                player.SetActive(false);
            }
            gameOverUI.SetActive(true);
        }
    }

    void RestartGame()
    {
        score = 0;
        lives = 3;
        FillHearts();
        gameOverUI.SetActive(false);
        turret.SetActive(true);
        player.SetActive(true);
    }

    private void FillHearts()
    {
        if (healthArray == null)
        {
            healthArray = new List<GameObject>();
        }
        for (int i = 0; i < lives; i++)
        {
            if (healthArray.Count < i + 1)
            {
                Vector3 originalPos = healthIcon.GetComponent<RectTransform>().transform.position;
                GameObject newHeart = (GameObject)Instantiate(healthIcon, originalPos, Quaternion.identity);
                Vector3 rectPos = newHeart.GetComponent<RectTransform>().position;
                newHeart.GetComponent<RectTransform>().position = new Vector3(rectPos.x + 55f * i, rectPos.y, 0);
                newHeart.transform.SetParent(GameObject.Find("Canvas").transform, false);
                if (newHeart == null)
                {
                    print(newHeart);
                }
                healthArray.Add(newHeart);
            }
        }
    }

    private void AddHeart(int index)
    {

    }
    private void AddRoom(float farhtestRoomEndX)
    {
        //1
        int randomRoomIndex = Random.Range(0, availableRooms.Length);

        //2
        GameObject room = (GameObject)Instantiate(availableRooms[randomRoomIndex]);

        //3
        float roomWidth = room.transform.Find("floor").localScale.x;

        //4
        float roomCenter = farhtestRoomEndX + roomWidth * 0.5f;

        //5
        room.transform.position = new Vector3(roomCenter, 0, 0);

        //6
        currentRooms.Add(room);
    }

    private void GenerateRoomIfRequired()
    {
        //1
        List<GameObject> roomsToRemove = new List<GameObject>();
        //2
        bool addRooms = true;
        //3
        float playerX = transform.position.x;
        //4
        float removeRoomX = playerX - screenWidthInPoints;
        //5
        float addRoomX = playerX + screenWidthInPoints;
        //6
        float farthestRoomEndX = 0;
        foreach (var room in currentRooms)
        {
            //7
            float roomWidth = room.transform.Find("floor").localScale.x;
            float roomStartX = room.transform.position.x - (roomWidth * 0.5f);
            float roomEndX = roomStartX + roomWidth;
            //8
            if (roomStartX > addRoomX)
            {
                addRooms = false;
            }
            //9
            if (roomEndX < removeRoomX)
            {
                roomsToRemove.Add(room);
            }
            //10
            farthestRoomEndX = Mathf.Max(farthestRoomEndX, roomEndX);
        }
        //11
        foreach (var room in roomsToRemove)
        {
            currentRooms.Remove(room);
            Destroy(room);
        }
        //12
        if (addRooms)
        {
            AddRoom(farthestRoomEndX);
        }
    }

    private void GenerateProjectilesIfRequired()
    {
        ; ;
    }

    private IEnumerator GeneratorCheck()
    {
        while (true)
        {
            GenerateRoomIfRequired();
            GenerateObjectsIfRequired();
            GenerateProjectilesIfRequired();
            yield return new WaitForSeconds(0.25f);
        }
    }

    private void AddObject(float lastObjectX)
    {
        //1
        int randomIndex = Random.Range(0, availableObjects.Length);

        //2
        GameObject obj = (GameObject)Instantiate(availableObjects[randomIndex]);

        //3
        float objectPositionX = lastObjectX + Random.Range(objectsMinDistance, objectsMaxDistance);
        float randomY = Random.Range(objectsMinY, objectsMaxY);
        obj.transform.position = new Vector3(objectPositionX, randomY, 0);

        //4
        // float rotation = Random.Range(objectsMinRotation, objectsMaxRotation);
        // obj.transform.rotation = Quaternion.Euler(Vector3.forward * rotation);

        //5
        objects.Add(obj);
    }

    private void GenerateObjectsIfRequired()
    {
        //1
        float playerX = transform.position.x;
        float removeObjectsX = playerX - screenWidthInPoints;
        float addObjectX = playerX + screenWidthInPoints;
        float farthestObjectX = 0;

        //2
        List<GameObject> objectsToRemove = new List<GameObject>();

        foreach (var obj in objects)
        {
            //3
            float objX = obj.transform.position.x;

            //4
            farthestObjectX = Mathf.Max(farthestObjectX, objX);

            //5
            if (objX < removeObjectsX)
            {
                objectsToRemove.Add(obj);
            }
        }

        //6
        foreach (var obj in objectsToRemove)
        {
            objects.Remove(obj);
            Destroy(obj);
        }

        //7
        if (farthestObjectX < addObjectX)
        {
            AddObject(farthestObjectX);
        }
    }
}