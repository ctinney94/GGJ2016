using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class movement : MonoBehaviour {

    public float moveSpeed, fireSpeedA, fireSpeedB;
    public GameObject missiles, bullet;
    bool allowMissile = true, allowShoot = true;
    float jetpackJuice = 250;
    public Slider jetpackBar;
	
    void Update () {
        inputThings();
        //GetComponent<Rigidbody2D>().angularVelocity = 0;
    }

    void missileCD() { allowMissile = true; }

    void shootCD() { allowShoot = true; }

    void inputThings()
    {
        if (Input.GetMouseButton(0))
        {
            if (allowShoot)
            {
                var pos = Input.mousePosition;
                pos.z = transform.position.z - Camera.main.transform.position.z;
                pos = Camera.main.ScreenToWorldPoint(pos);

                var q = Quaternion.FromToRotation(Vector3.up, pos - transform.position);
                GameObject go = Instantiate(bullet, transform.position + (pos * 0.1f), q) as GameObject;
                go.GetComponent<Rigidbody2D>().AddForce(go.transform.up *4000);

                allowShoot = false;
                Invoke("shootCD", fireSpeedA);
            }
        }

        if (Input.GetKey(KeyCode.D))
            GetComponent<Rigidbody2D>().AddForce(new Vector2(1 * moveSpeed, 0), ForceMode2D.Impulse);


        if (Input.GetKey(KeyCode.A))
            GetComponent<Rigidbody2D>().AddForce(new Vector2(-1 * moveSpeed, 0), ForceMode2D.Impulse);


        if (Input.GetKey(KeyCode.W))
        {
            Debug.Log(jetpackJuice);
            if (jetpackJuice > 0)
            {
                jetpackBar.value = jetpackJuice;
                jetpackBar.GetComponentsInChildren<Image>()[1].color = Color.Lerp(Color.red, Color.green, jetpackJuice/250);
                jetpackJuice--;
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1 * moveSpeed), ForceMode2D.Impulse);

            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            if (allowMissile)
            {
                GameObject rocket = Instantiate(missiles) as GameObject;
                rocket.transform.position = transform.position + new Vector3(0, 3f, 0);
                rocket.GetComponent<rocket>().target = GameObject.Find("target").transform;
                rocket.GetComponent<BoxCollider2D>().enabled = true;
                allowMissile = false;
                Invoke("missileCD", fireSpeedB);
            }
        }
    }
}
