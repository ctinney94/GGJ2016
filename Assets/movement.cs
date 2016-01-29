using UnityEngine;
using System.Collections;

public class movement : MonoBehaviour {

    public float moveSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        inputThings();
	}

    void inputThings()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //make a rocket at pos
        }

        if (Input.GetKey(KeyCode.D))
            transform.Translate(new Vector3(1 * moveSpeed, 0, 0));

        if (Input.GetKey(KeyCode.A))
            transform.Translate(new Vector3(-1 * moveSpeed, 0, 0));

        if (Input.GetKey(KeyCode.W))
            transform.Translate(new Vector3(0, 1 * moveSpeed, 0));

        if (Input.GetKey(KeyCode.S))
            transform.Translate(new Vector3(0, -1 * moveSpeed, 0));
    }
}
