using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class targetFinder : MonoBehaviour {

    public List<GameObject> targets = new List<GameObject>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "enemy")
        {
            targets.Add(col.gameObject);
            col.gameObject.GetComponent<enemy>().targetThing = gameObject;
        }
    }
}