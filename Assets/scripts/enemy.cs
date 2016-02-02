using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class enemy : MonoBehaviour {

    public enemySpawner Mum;
    public int HP = 100;
    public ParticleSystem PS;
    public GameObject targetThing;
    GameObject player;
    Rigidbody2D rig;
    float maxVelocity = 10;
    SpriteRenderer[] SR;
    bool alive = true;
    public Image comboCircle;
    public Text comboText;

    public score theScore; //If you don't know the score, you don't know the truth.

    // Use this for initialization
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        PS = GetComponentInChildren<ParticleSystem>();
        SR = GetComponentsInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //Keep upright at all times
        if (transform.rotation.eulerAngles.z != 0)
        {
            Quaternion rot = transform.rotation;
            rot.eulerAngles = new Vector3(0, rot.eulerAngles.y, 0);
            transform.rotation = rot;
        }

        if (HP < 1 && alive)
        {
            if (comboCircle.fillAmount > 0.7f)
            {
                int combo = GameObject.FindGameObjectWithTag("Player").GetComponent<movement>().combo += 1;
                comboText.text = "x" + GameObject.FindGameObjectWithTag("Player").GetComponent<movement>().combo;
                comboCircle.fillAmount = comboCircle.fillAmount + 0.3f - 1;
            }
            else
                comboCircle.fillAmount += 0.3f;

            theScore.enemyKilled();

            //ded
            alive = false;
            Mum.disown(gameObject);
            //rig.velocity = Vector3.zero;
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

        if (alive)
        {
            if (transform.position.x > player.transform.position.x)
            {
                if (transform.rotation.eulerAngles.y == 0)
                    transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));

                if (rig.velocity.x > -maxVelocity)
                    rig.AddForce(-Vector2.right, ForceMode2D.Impulse);
            }

            if (transform.position.x < player.transform.position.x)
            {
                if (transform.rotation.eulerAngles.y != 0)
                    transform.rotation = Quaternion.Euler(Vector3.zero);

                if (rig.velocity.x < maxVelocity)
                    rig.AddForce(Vector2.right, ForceMode2D.Impulse);
            }
        }
        else
        {
            for (int i = 0; i < SR.Length; i++)
            {
                Color tempCol = SR[i].color;
                tempCol.a = Mathf.Lerp(SR[i].color.a, 0, Time.deltaTime);
                SR[i].color = tempCol;
                if (SR[i].color.a == 0 || transform.position.y < -20)
                {
                    if (targetThing != null)
                    {
                        targetThing.GetComponent<targetFinder>().targets.Remove(gameObject);
                    }
                    Destroy(gameObject);
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "bullet")
        {
            Camera.main.GetComponent<CameraShake>().shake = true;
            Camera.main.GetComponent<CameraShake>().shakeRange = new Vector3(.4f, 0.4f, 0);
            HP -= Random.Range(30, 60);
                 PS.Emit(((100 - HP) / 5));
        }
        else if (col.gameObject.tag == "rocket")
        {
            HP -= 100;

            rig.AddForce(new Vector2(col.transform.rotation.z, col.transform.rotation.w) * 100, ForceMode2D.Impulse);

            PS.Emit(((100 - HP) / 5));
        }
        else if (col.gameObject.tag == "enemy")
        {
            Physics2D.IgnoreCollision(col.collider, GetComponent<Collider2D>());
        }
        else if (col.gameObject.tag == "Player")
            col.gameObject.GetComponent<movement>().HP -= Random.Range(8, 15);
    }
}
