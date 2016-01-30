using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class enemy : MonoBehaviour {

    public int HP = 100;
    ParticleSystem PS;
    public GameObject targetThing;
    GameObject player;
    Rigidbody2D rig;
    float maxVelocity = 10;
    SpriteRenderer SR;
    bool alive = true;
    public Image comboCircle;
    public Text comboText;
    // Use this for initialization
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        PS = GetComponentInChildren<ParticleSystem>();
        SR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            if (transform.position.x > player.transform.position.x)
            {
                if (rig.velocity.x > -maxVelocity)
                    rig.AddForce(-Vector2.right, ForceMode2D.Impulse);
            }

            if (transform.position.x < player.transform.position.x)
            {
                if (rig.velocity.x < maxVelocity)
                    rig.AddForce(Vector2.right, ForceMode2D.Impulse);
            }
        }
        else
        {
            Color tempCol = SR.color;
            tempCol.a = Mathf.Lerp(SR.color.a, 0, Time.deltaTime);
            SR.color = tempCol;
            if (SR.color.a == 0)
                Destroy(gameObject);
        }
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

            rig.AddForce(new Vector2(col.transform.rotation.z, col.transform.rotation.w), ForceMode2D.Impulse);

            PS.Emit(((100 - HP) / 5));
        }
        else if (col.gameObject.tag == "enemy")
        {
            Physics2D.IgnoreCollision(col.collider, GetComponent<Collider2D>());
        }
        else if (col.gameObject.tag == "Player")
            col.gameObject.GetComponent<movement>().HP -= Random.Range(8, 15);


        if (HP < 1 && alive)
        {
            if (comboCircle.fillAmount > 0.7f)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<movement>().combo += 1;
                comboText.text = "x" + GameObject.FindGameObjectWithTag("Player").GetComponent<movement>().combo;
                comboCircle.fillAmount = comboCircle.fillAmount + 0.3f - 1;
            }
            else
                comboCircle.fillAmount += 0.3f;

            //ded
            alive = false;
            rig.velocity = Vector3.zero;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<AudioSource>().Play();
            PS.transform.parent = null;
            PS.Stop();
            Destroy(PS.gameObject, 30f);

            if (targetThing != null)
            {
                targetThing.GetComponent<targetFinder>().targets.Remove(gameObject);
            }
        }
    }
}
