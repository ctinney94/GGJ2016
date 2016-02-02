using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class movement : MonoBehaviour {

    public float HP = 100;

    AudioSource audio;
    public AudioClip missileSound, dead;
    bool alive=true;
    public float moveSpeed, fireSpeedA, fireSpeedB;
    public GameObject missiles, bullet, epxlosion, comboUI, shootingArm, bulletOrigin;
    int missileCount = 25;
    bool allowMissile = true, allowShoot = true;
    float jetpackJuice = 250;
    public GameObject hammer;

    public Text gameOverText, gameOverSubText;

    public SpriteRenderer redBarrel;
    public float hotness;
    public Slider jetpackBar, healthBar;
    public Text missileCountText;
    public Animator anim, HAMMERanim;

    public int combo = 3;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (GetComponent<Rigidbody2D>().velocity.x < 0.2f && GetComponent<Rigidbody2D>().velocity.x > -0.2f)
            anim.SetBool("running", false);
        if (GetComponent<Rigidbody2D>().velocity.y < 0.05f && GetComponent<Rigidbody2D>().velocity.y > -0.05f)
            anim.SetBool("grounded", true);

        jetpackBar.value = jetpackJuice;
        jetpackBar.GetComponentsInChildren<Image>()[1].color = Color.Lerp(Color.red, Color.green, jetpackJuice / 250);
        healthBar.value = HP;
        healthBar.GetComponentsInChildren<Image>()[1].color = Color.Lerp(Color.red, Color.green, HP / 100);
        if (jetpackJuice != jetpackBar.maxValue)
            jetpackJuice++;

        #region combo
        if (alive)
        {
            comboUI.GetComponentsInChildren<Image>()[1].fillAmount -= 0.005f;
            if (comboUI.GetComponentsInChildren<Image>()[1].fillAmount == 0 && combo > 1)
            {
                combo--;
                comboUI.GetComponentsInChildren<Image>()[1].fillAmount = 1;
                comboUI.GetComponentInChildren<Text>().text = "x" + combo;
            }
        }
        #endregion

        if (Input.GetKeyDown(KeyCode.R))
        {
            Application.LoadLevel("main");
        }


        inputThings();

        //Keep upright at all times
        if (transform.rotation.eulerAngles.z != 0)
        {
            Quaternion rot = transform.rotation;
            rot.eulerAngles = new Vector3(0, rot.eulerAngles.y,0);
            transform.rotation = rot;
        }

        if (hotness > 0)
            hotness--;
        else
            allowShoot = true;

        redBarrel.color = Color.Lerp(Color.white, Color.red, hotness / 150);

        if (HP < 1 && alive)
        {
            alive = false;
            //we're dead :(
            GameObject.Find("score").GetComponent<score>().saveScore();
            GameObject explo = Instantiate(epxlosion) as GameObject;
            explo.transform.position = transform.position;
            explo.GetComponent<exploder>().sound = dead;
            SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();

            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].color = Color.Lerp(Color.white, Color.black, 0.8f);
            }

            StartCoroutine(gameOver());
        }
    }

    IEnumerator gameOver()
    {
        yield return new WaitForSeconds(1);
        gameOverText.enabled = true;
        yield return new WaitForSeconds(2);
        gameOverSubText.enabled = true;
    }

    void missileCD() { allowMissile = true; }

    void shootCD() { allowShoot = true; }

    void stopHammerDMG()
    {
        hammer.GetComponent<SpriteRenderer>().material.color = Color.white;
        hammer.GetComponent<Collider2D>().enabled = false;
        hammer.GetComponent<AudioSource>().Play();
        swinging = false;
    }
    IEnumerator swingHammer()
    {
        HAMMERanim.Play("Mech_HAMMER");
        yield return new WaitForSeconds(0.2f);
        hammer.GetComponent<Collider2D>().enabled = true;
        yield return new WaitForSeconds(0.3f);
        
        hammer.GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.35f);
        hammer.GetComponent<Collider2D>().enabled = false;
        swinging = false;
    }

    bool swinging = false;
    void inputThings()
    {
        if (alive)
        {
            #region point the gun
            Vector3 pos = Input.mousePosition;
            pos.z = transform.position.z - Camera.main.transform.position.z;
            pos = Camera.main.ScreenToWorldPoint(pos);

            #region flip
            if (pos.x > transform.position.x && transform.rotation.eulerAngles.y != 0)
            {
                transform.rotation = Quaternion.Euler(Vector3.zero);
                GetComponentsInChildren<ParticleSystem>()[0].startRotation = 1000;
            }

            if (pos.x < transform.position.x && transform.rotation.eulerAngles.y == 0)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                GetComponentsInChildren<ParticleSystem>()[0].startRotation = 1000;
            }
            #endregion

            Vector3 shootDir = pos - transform.position;
            Quaternion q = Quaternion.FromToRotation(Vector3.right, pos - transform.position);

            if (transform.rotation.eulerAngles.y != 0)
                q.eulerAngles = new Vector3(180, q.eulerAngles.y, -q.eulerAngles.z);
            shootingArm.transform.rotation = q;
            #endregion

            #region LMB fire
            if (Input.GetMouseButton(0))
            {
                if (allowShoot)
                {
                    q = Quaternion.FromToRotation(Vector3.up, pos - transform.position);
                    var dir = Quaternion.AngleAxis(q.eulerAngles.z, Vector3.forward) * Vector3.up * 2;

                    dir *= 2f;
                    //GameObject go = Instantiate(bullet, transform.position + dir, q) as GameObject;
                    GameObject go = Instantiate(bullet, bulletOrigin.transform.position, q) as GameObject;

                    go.GetComponent<Rigidbody2D>().AddForce(go.transform.up * 400);

                    allowShoot = false;

                    hotness += 10;
                    if (hotness < 150)
                    {
                        Invoke("shootCD", fireSpeedA);
                    }

                    //Shells
                    GetComponentsInChildren<ParticleSystem>()[1].Emit(1);
                }
            }
            #endregion

            if (Input.GetMouseButtonDown(1))
            {
                if (!swinging)
                {
                    swinging = true;
                    StartCoroutine(swingHammer());
                }
            }
            if (Input.GetKey(KeyCode.D))
            {
                anim.SetBool("running", true);
                GetComponent<Rigidbody2D>().AddForce(new Vector2(1 * moveSpeed, 0), ForceMode2D.Impulse);
            }

            if (Input.GetKey(KeyCode.A))
            {
                anim.SetBool("running", true);
                GetComponent<Rigidbody2D>().AddForce(new Vector2(-1 * moveSpeed, 0), ForceMode2D.Impulse);
            }

            #region jetpack
            if (Input.GetKey(KeyCode.W))
            {
                if (jetpackJuice > 0)
                {
                    anim.SetBool("grounded", false);
                    jetpackJuice -= 3;
                    GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 1 * moveSpeed), ForceMode2D.Impulse);

                    //Fire
                    GetComponentsInChildren<ParticleSystem>()[0].Emit(40);
                }
            }
            #endregion
            #region missiles
            if (Input.GetKey(KeyCode.Space))
            {
                if (allowMissile)
                {
                    allowMissile = false;
                    Invoke("missileCD", fireSpeedB);
                    if (missileCount > 0)
                    {
                        missileCount--;
                        missileCountText.text = "x" + missileCount;
                        audio.clip = missileSound;
                        audio.Play();
                        GameObject rocket = Instantiate(missiles) as GameObject;
                        if (transform.rotation.eulerAngles.y == 0)
                            rocket.transform.position = transform.position + new Vector3(-0.6f, 5f, 0);
                        else
                            rocket.transform.position = transform.position + new Vector3(0.6f, 5f, 0);

                        if (GetComponentInChildren<targetFinder>().targets.Count > 0)
                        {
                            rocket.GetComponent<rocket>().target = GetComponentInChildren<targetFinder>().targets[0].transform;
                        }
                        rocket.GetComponent<BoxCollider2D>().enabled = true;
                    }
                }
            }
            #endregion
        }
    }
}