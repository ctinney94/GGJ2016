using UnityEngine;
using System.Collections;

public class explosion : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Invoke("changCol", 0.1f);
	}

    void changCol()
    {
        GetComponent<SpriteRenderer>().material.color = Color.black;
        Debug.Log("doop!");
        Destroy(gameObject, 0.1f);
    }
}
