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

    IEnumerator changCol()
    {
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().material.color = Color.black;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().enabled = false;
        Destroy(gameObject,0.2f);
    }
}