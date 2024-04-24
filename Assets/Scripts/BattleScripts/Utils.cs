using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    public static Player GetPlayer()
    {
        return GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public static List<Enemy> GetEnemies()
    {
        GameObject[] enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");
        List<Enemy> enemiesList = new List<Enemy>();
        foreach (GameObject enemyObject in enemyObjects)
        {
            Enemy enemy = enemyObject.GetComponent<Enemy>();
            if (enemy != null) enemiesList.Add(enemy);
        }
        return enemiesList;
    }

}
