using UnityEngine;
using System.Collections;

public class exploder : MonoBehaviour {

    public AudioClip sound;
    AudioSource audio;

	// Use this for initialization
	void Start () {
        Camera.main.GetComponent<CameraShake>().shake = true;
        Camera.main.GetComponent<CameraShake>().shakeRange = transform.localScale / 60;

        audio = GetComponent<AudioSource>();
        audio.clip = sound;
        audio.Play();
        StartCoroutine(changCol());
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<movement>().HP -= Random.Range(40, 60);
        }

        if (col.gameObject.tag == "enemy")
            col.gameObject.GetComponent<enemy>().HP -= Random.Range(60, 80);
    }

    IEnumerator changCol()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().material.color = Color.black;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject,0.5f);
    }
}