using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {

    public GameObject explosion;
	// Use this for initialization
	void Start () {
        Destroy(gameObject, 3);
	}
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag != "Player" && col.gameObject.tag != "bullet")
        {
            GameObject explo = Instantiate(explosion) as GameObject;
            explo.transform.position = transform.position;
            explo.transform.localScale = new Vector3(15, 15, 1);
            Destroy(gameObject);
        }
    }
}
