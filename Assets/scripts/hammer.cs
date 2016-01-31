using UnityEngine;
using System.Collections;

public class hammer : MonoBehaviour {

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
            col.gameObject.GetComponent<enemy>().HP -= Random.Range(85, 150);
            col.gameObject.GetComponent<enemy>().PS.Emit(20);
        }
    }
}
