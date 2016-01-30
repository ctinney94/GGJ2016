using UnityEngine;
using System.Collections;

public class rocket : MonoBehaviour
{
    public GameObject explosion;
    public Transform target;
    public float forceX, forceY;
    Rigidbody2D rig;
    public ParticleSystem PS;
    public AudioClip missileHit;

    float aliveTime = 0;

    // Use this for initialization
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        rig.AddForce(Vector2.up * 30, ForceMode2D.Impulse);
    }

    // Update is called once per frame
	void Update () 
    {
        aliveTime += Time.deltaTime;
        Debug.Log(aliveTime);
        forceX = 15;
        forceY = 15;

        float mult = 2f;

        if (target != null)
        {
            if (transform.position.x > target.position.x)
            {
                forceX = 5 + (transform.position.x - target.position.x) * mult;
                rig.AddForce(-Vector2.right * Time.deltaTime * forceX, ForceMode2D.Impulse);
            }

            if (transform.position.x < target.position.x)
            {
                forceX = 5 + (target.position.x - transform.position.x) * mult;
                rig.AddForce(Vector2.right * Time.deltaTime * forceX, ForceMode2D.Impulse);
            }

            if (transform.position.y > target.position.y)
            {
                forceY = 5 + (transform.position.y - target.position.y) * mult;
                rig.AddForce(-Vector2.up * Time.deltaTime * forceY, ForceMode2D.Impulse);
            }

            if (transform.position.y < target.position.y)
            {
                forceY = 5 + (target.position.y - transform.position.y) * mult;
                rig.AddForce(Vector2.up * Time.deltaTime * forceY, ForceMode2D.Impulse);
            }
            Vector3 targetDir = target.position - transform.position;

            float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
            Quaternion targetRot = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * 5);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (aliveTime < 0.2)
        {
            if (col.gameObject.tag == "Player")
                Physics2D.IgnoreCollision(col.collider, GetComponent<Collider2D>());

            Invoke("revert", 0.9f);
        }
        else
        {
            if (col.gameObject.GetComponent<rocket>() == null)
            {
                PS.transform.parent = null;
                PS.Stop();
                Destroy(PS.gameObject, 2.5f);
                GameObject explo = Instantiate(explosion) as GameObject;
                explo.transform.position = transform.position;
                explo.GetComponent<exploder>().sound = missileHit;
                Destroy(gameObject);
            }
        }
    }
    void revert()
    {
        Physics2D.IgnoreCollision(GameObject.Find("mech").GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
    }
}
