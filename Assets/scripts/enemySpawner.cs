using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class enemySpawner : MonoBehaviour {

    public int maxEnemies = 5;
    List<GameObject> children = new List<GameObject>();
    public GameObject enemy;

    public GameObject idle, summomning;

    public Image comboCircle;
    public Text comboText;
    public score theScore;

    bool spawning = false;
    public bool leftRight;
	// Use this for initialization
	void Start () {
	}

    IEnumerator spawnEnemies(float interval)
    {
        idle.SetActive(false);
        summomning.SetActive(true);
        while (children.Count < maxEnemies)
        {
            spawning = true;
            GameObject newGuy;
            if (leftRight)
                newGuy = Instantiate(enemy, transform.position + (Vector3.right * 4), transform.rotation) as GameObject;
            else
                newGuy = Instantiate(enemy, transform.position + (Vector3.left * 4), transform.rotation) as GameObject;

            newGuy.GetComponent<enemy>().theScore = theScore;
            newGuy.GetComponent<enemy>().comboText = comboText;
            newGuy.GetComponent<enemy>().comboCircle = comboCircle;
            newGuy.GetComponent<enemy>().Mum = gameObject.GetComponent<enemySpawner>();
            children.Add(newGuy);
            yield return new WaitForSeconds(interval);
        }
        idle.SetActive(true);
        summomning.SetActive(false);
        spawning = false;
    }

	// Update is called once per frame
	void Update () {
        if (children.Count < maxEnemies && spawning == false)
            StartCoroutine(spawnEnemies(0.3f));
	}

    public void disown(GameObject baby)
    {
        children.Remove(baby);
    }
}
