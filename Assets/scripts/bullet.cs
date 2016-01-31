using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {

    public AudioClip[] fireSounds;
    public AudioClip[] hitSounds;
    AudioSource audio;

    public GameObject explosion;
	// Use this for initialization
    void Update()
    {
    }

    void Start()
    {
        Camera.main.GetComponent<CameraShake>().shake = true;
        Camera.main.GetComponent<CameraShake>().shakeRange = new Vector3(.3f, 0.3f, 0);
        
        Destroy(gameObject, 3);
        audio = GetComponentInChildren<AudioSource>();
        audio.clip = fireSounds[Random.Range(0, fireSounds.Length-1)];
        audio.Play();
	}
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag != "Player" && col.gameObject.tag != "bullet")
        {
            if (col.gameObject.tag != "enemy")
            {
                Camera.main.GetComponent<CameraShake>().shake = true;
                Camera.main.GetComponent<CameraShake>().shakeRange = new Vector3(0.15f, 0.15f, 0);
                
                GameObject explo = Instantiate(explosion) as GameObject;
                explo.GetComponent<exploder>().sound = hitSounds[Random.Range(0, hitSounds.Length - 1)];
                explo.transform.position = transform.position;
                explo.transform.localScale = new Vector3(.75f, .75f, 1);
            }
            Destroy(gameObject);
        }
    }
}
