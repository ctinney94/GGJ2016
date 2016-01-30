using UnityEngine;
using System.Collections;

public class enemy : MonoBehaviour {

    public int HP = 100;
    ParticleSystem PS;

    // Use this for initialization
    void Start()
    {
        PS = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "bullet")
        {
            Camera.main.GetComponent<CameraShake>().shake = true;
            Camera.main.GetComponent<CameraShake>().shakeRange = new Vector3(.4f, 0.4f, 0);
            HP -= Random.Range(15, 30);
            PS.Emit(((100 - HP) / 5));
        }
        else if (col.gameObject.tag == "rocket")
        {
            HP -= Random.Range(85, 100);
            PS.Emit(((100 - HP) / 5));
        }
        if (HP < 1)
        {
            //ded
            PS.transform.parent = null;
            PS.Stop();
            Destroy(PS.gameObject, 30f);
            Destroy(gameObject);
        }
    }
}
