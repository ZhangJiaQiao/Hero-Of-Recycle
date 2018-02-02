using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class PlayerManager : PunBehaviour {
    #region public var
    public float PlayerHp = 100;//生命值
    public float PlayerMp = 100;//魔法值
    public AudioClip footSound;//脚步声
    public AudioClip reloadSound;//装弹声
    public float spread;
    public Texture2D defaultTex;
    public GameObject muzzleFlash;
    public ParticleSystem smoke;
    public float shootColdTime = 0;//射击冷却总时间
    public GameObject Gun;
    public GameObject Mappoint;
    #endregion

    #region private var
    private string bulletName;//子弹名字
    private string tornadoName;
    private string trashcarName;
    private string grenadeName;
    private int currentBulletType;//当前子弹类型
    private List<string> bullet_types;//子弹类型名称列表
    private List<GameObject> BulletList = new List<GameObject>();
    private float jumpColdTime = 0;//跳跃冷却时间
    private float muzzleFlashTime = 0;//火花闪现时间
    private float CurrentColdT;//射击冷却当前时间
    private static float maxVelocity_fb = 5;
    private static float maxVelocity_rl = 5;
    private bool isDied = false;//是否死亡
    private Animator animator;
    private AudioSource _audioSource;
    private Rigidbody rigbody;
    private Role role;
    private int CurrentBullet;//子弹数量
    private bool Reload = false;//是否装弹
    #endregion
    // Use this for initialization
    void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        CurrentBullet = 20;
        bulletName = "NetBullet";
        tornadoName = "Nettornado";
        trashcarName = "NettrashCar";
        grenadeName = "Netgrenade";
        animator = GetComponent<Animator>();
        rigbody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        role = GetComponent<Role>();
        Camera.main.transform.parent = Gun.transform;
        Camera.main.transform.localPosition = Vector3.zero;
        currentBulletType = 0;
        bullet_types = new List<string> { "foodTrash", "recyclableTrash", "otherTrash", "harmfulTrash" };
    }
	
	// Update is called once per frame
	void Update () {
        //if (photonView.isMine && !isDied)
        //{
        //    this.ProcessInputs();
        //}
        this.ProcessInputs();
        PlayerHp = role.hp;
        PlayerMp = role.mp;
    }

    void OnGUI()
    {
        float resFactor = Mathf.Clamp((Screen.height / 7.2f), 50, 150);
        float crossW = Mathf.Clamp(resFactor / 1000, 0.01f, 0.02f);
        float crossH = resFactor / 833.3f;
        GUI.DrawTexture(new Rect((Screen.width / 2 - resFactor * crossH / 2) - (resFactor * spread), Screen.height / 2 - resFactor * crossW / 2, resFactor * crossH, resFactor * crossW), defaultTex);
        GUI.DrawTexture(new Rect((Screen.width / 2 - resFactor * crossH / 2) + (resFactor * spread), Screen.height / 2 - resFactor * crossW / 2, resFactor * crossH, resFactor * crossW), defaultTex);

        //Vertical
        GUI.DrawTexture(new Rect(Screen.width / 2 - resFactor * crossW / 2, (Screen.height / 2 - resFactor * crossH / 2) - (resFactor * spread), resFactor * crossW, resFactor * crossH), defaultTex);
        GUI.DrawTexture(new Rect(Screen.width / 2 - resFactor * crossW / 2, (Screen.height / 2 - resFactor * crossH / 2) + (resFactor * spread), resFactor * crossW, resFactor * crossH), defaultTex);
    }

    private void ProcessInputs()
    {
        setBulletType();
        directionCtrl();
        GunMove();
        move();
        jump();
        shoot();
        useSkill();
    }

    void directionCtrl()
    {
        if (!isDied)
        {
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * 2;
            float rotationY = transform.localEulerAngles.x;
            transform.localEulerAngles = new Vector3(rotationY, rotationX, 0);
        }
    }

    void GunMove()
    {
        if(!isDied)
        {
            float rotationX = Gun.transform.localEulerAngles.y;
            float rotationY = Gun.transform.localEulerAngles.x - Input.GetAxis("Mouse Y");
            if (rotationY < 60 || rotationY > 325)
            {
                Gun.transform.localEulerAngles = new Vector3(rotationY, rotationX, 0);
            }
        }
    }

    void move()
    {
        if (!isDied)
        {
            bool isMoving = false;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                Vector3 v = rigbody.velocity;
                Vector3 tmp = transform.forward.normalized;
                float projection = Vector3.Dot(v, tmp) / tmp.sqrMagnitude;
                if (Mathf.Abs(projection) < maxVelocity_fb)
                {
                    v += transform.forward.normalized;
                }
                isMoving = true;
                if (Input.GetKey(KeyCode.E))
                {
                    rigbody.velocity = v * 0.8f;
                    animator.SetBool("run", true);
                    animator.SetBool("walk", false);
                    animator.Play("Run", 0);
                }
                else
                {
                    rigbody.velocity = v * 0.6f;
                    animator.SetBool("walk", true);
                    animator.SetBool("run", false);
                    animator.Play("Walk", 0);
                }
                _audioSource.clip = footSound;
                if (_audioSource.isPlaying && _audioSource.clip.name == "foot") { }
                else _audioSource.Play();
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                Vector3 v = rigbody.velocity;
                Vector3 tmp = transform.forward.normalized;
                float projection = Vector3.Dot(v, tmp * -1) / tmp.sqrMagnitude;
                if (Mathf.Abs(projection) < maxVelocity_fb)
                {
                    v -= transform.forward.normalized;
                }
                isMoving = true;
                if (Input.GetKey(KeyCode.E))
                {
                    rigbody.velocity = v * 0.8f;
                    animator.SetBool("run", true);
                    animator.SetBool("walk", false);
                    animator.Play("Run", 0);
                }
                else
                {
                    rigbody.velocity = v * 0.6f;
                    animator.SetBool("walk", true);
                    animator.SetBool("run", false);
                    animator.Play("Walk", 0);
                }
                _audioSource.clip = footSound;
                if (_audioSource.isPlaying && _audioSource.clip.name == "foot") { }
                else _audioSource.Play();
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                Vector3 v = rigbody.velocity;
                Vector3 tmp = transform.right.normalized;
                float projection = Vector3.Dot(v, tmp) / tmp.sqrMagnitude;
                if (Mathf.Abs(projection) < maxVelocity_rl)
                {
                    v -= transform.right.normalized;
                }
                isMoving = true;
                if (Input.GetKey(KeyCode.E))
                {
                    rigbody.velocity = v * 0.8f;
                    animator.SetBool("run", true);
                    animator.SetBool("walk", false);
                    animator.Play("Run", 0);
                }
                else
                {
                    rigbody.velocity = v * 0.6f;
                    animator.SetBool("walk", true);
                    animator.SetBool("run", false);
                    animator.Play("Walk", 0);
                }
                _audioSource.clip = footSound;
                if (_audioSource.isPlaying && _audioSource.clip.name == "foot") { }
                else _audioSource.Play();
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                Vector3 v = rigbody.velocity;
                Vector3 tmp = transform.right.normalized;
                float projection = Vector3.Dot(v, tmp * -1) / tmp.sqrMagnitude;
                if (Mathf.Abs(projection) < maxVelocity_rl)
                {
                    v += transform.right.normalized;
                }
                isMoving = true;
                if (Input.GetKey(KeyCode.E))
                {
                    rigbody.velocity = v * 0.8f;
                    animator.SetBool("run", true);
                    animator.SetBool("walk", false);
                    animator.Play("Run", 0);
                }
                else
                {
                    rigbody.velocity = v * 0.6f;
                    animator.SetBool("walk", true);
                    animator.SetBool("run", false);
                    animator.Play("Walk", 0);
                }
                _audioSource.clip = footSound;
                if (_audioSource.isPlaying && _audioSource.clip.name == "foot") { }
                else _audioSource.Play();
            }

            if (!isMoving)
            {
                animator.SetBool("walk", false);
                animator.SetBool("run", false);
            }
        }
    }

    void jump()
    {
        if (!isDied)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (jumpColdTime <= 0)
                {
                    rigbody.AddForce(transform.up.normalized * 4, ForceMode.Impulse);
                    jumpColdTime = 1;
                }
            }
            else
            {
                if (jumpColdTime > 0)
                {
                    jumpColdTime -= Time.deltaTime;
                }
            }
        }
    }

    void shoot()
    {
        if (muzzleFlashTime > 0)
        {
            muzzleFlashTime -= Time.deltaTime;
        }
        else
        {
            muzzleFlash.SetActive(false);
        }

        if (CurrentColdT > 0)
        {
            CurrentColdT -= Time.deltaTime;
            return;
        }
        else
        {
            SetSpread(0.3f);
        }

        if (Input.GetKey(KeyCode.Mouse0) && !Reload)
        {
            if (Cursor.lockState == CursorLockMode.None)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            Camera c = Camera.main;
            GameObject bullet = getBullet();
            Vector3 point = c.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, c.nearClipPlane));
            bullet.transform.right = c.transform.forward * -1;
            bullet.transform.position = point + c.transform.forward.normalized;
            bullet.GetComponent<Rigidbody>().AddForce(c.transform.forward.normalized * 50, ForceMode.Impulse);
            CurrentColdT = shootColdTime;
            muzzleFlash.SetActive(true);
            float size = Random.Range(0.2f, 0.3f);
            muzzleFlash.transform.localScale = new Vector3(size, size, 0);
            muzzleFlash.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, Random.Range(0, 360)));
            smoke.Play();
            float spread = Random.Range(0.4f, 0.6f);
            SetSpread(spread);
            muzzleFlashTime = 0.05f;
            //子弹数量减少并判断是否需加弹
            CurrentBullet--;
            //Debug.Log(CurrentBullet);
            if (CurrentBullet <= 0)
            {
                _audioSource.clip = reloadSound;
                _audioSource.Play();
                Reload = true;
                muzzleFlash.SetActive(false);
                StartCoroutine(GunReload());
            }
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            if (info.IsName("Idle"))
            {
                animator.Play("Stand_Shoot");
            }
            else if(info.IsName("Walk"))
            {
                animator.Play("WalkShoot");
            }
            else if (info.IsName("Run"))
            {
                animator.Play("RunShoot");
            }
            animator.SetTrigger("shoot");
        }
    }

    void useSkill()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            float Mp = role.mp;
            if (Mp >= 100f)
            {
                useTornado();
            }
            else if (Mp >= 67f)
            {
                useTrashCar();
            }
            else if (Mp >= 34f)
            {
                useGrenade();
            }
        }
    }

    void SetSpread(float spread)
    {
        if (this.spread != spread)
            this.spread = Mathf.Clamp(spread * 1.5f, 0.15f, 1.5f);
    }

    private GameObject getBullet()
    {
        for(int i = 0; i < BulletList.Count; i++)
        {
            if(!BulletList[i].gameObject.activeSelf)
            {
                BulletList[i].gameObject.SetActive(true);
                BulletList[i].gameObject.tag = bullet_types[currentBulletType];
                BulletList[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
                return BulletList[i];
            }
        }
        GameObject newBullet = (GameObject)Instantiate(Resources.Load(bulletName));
        newBullet.gameObject.tag = bullet_types[currentBulletType];
        BulletList.Add(newBullet);
        return newBullet;
    }

    private void setBulletType()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentBulletType += 1;
            currentBulletType %= 4;
        }
    }

    IEnumerator GunReload()
    {
        yield return new WaitForSeconds(3);
        Reload = false;
        CurrentBullet = 20;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (!isDied)
        {
            if (collision.gameObject.tag == "otherTrash" || collision.gameObject.tag == "recyclableTrash"
                || collision.gameObject.tag == "foodTrash" || collision.gameObject.tag == "harmfulTrash")
            {
                pain p = Singleton<pain>.Instance;
                p.showPain();
                role.hp -= collision.gameObject.GetComponent<Weapon>().damageValue;
            }
            if (collision.gameObject.tag == "SoS")
            {
                role.hp += 5;
            }
        }
    }

    void useTornado()
    {
        GameObject tornado1 = (GameObject)Instantiate(Resources.Load(tornadoName));
        GameObject tornado2 = (GameObject)Instantiate(Resources.Load(tornadoName));
        tornado1.transform.position = this.transform.position + 0.5f * this.transform.forward.normalized + 0.2f * this.transform.right.normalized;
        tornado2.transform.position = this.transform.position + 0.5f * this.transform.forward.normalized - 0.2f * this.transform.right.normalized;
        Debug.Log(tornado1.transform.position);
        Debug.Log(tornado2.transform.position);
        if (tornado1.transform.position.y != 0.4)
        {
            tornado1.transform.position = new Vector3(tornado1.transform.position.x, 0.4f, tornado1.transform.position.z);
            tornado2.transform.position = new Vector3(tornado2.transform.position.x, 0.4f, tornado2.transform.position.z);
        }
        tornado1.transform.forward = this.transform.forward;
        Vector3 rotation1 = tornado1.transform.localEulerAngles;
        rotation1.x = -90;
        tornado1.transform.localEulerAngles = rotation1;

        tornado2.transform.forward = this.transform.forward;
        Vector3 rotation2 = tornado2.transform.localEulerAngles;
        rotation2.x = -90;
        tornado2.transform.localEulerAngles = rotation2;
        role.mp -= 33;
    }

    void useTrashCar()
    {
        GameObject trashcar = (GameObject)Instantiate(Resources.Load(trashcarName));
        trashcar.transform.position = this.transform.position + 2 * this.transform.forward.normalized;
        if(trashcar.transform.position.y != 0.4)
        {
            trashcar.transform.position = new Vector3(trashcar.transform.position.x, 0.4f, trashcar.transform.position.z);
        }
        trashcar.transform.forward = this.transform.forward;
        Vector3 rotation = trashcar.transform.localEulerAngles;
        rotation.x = -90;
        trashcar.transform.localEulerAngles = rotation;
        role.mp -= 33;

    }

    void useGrenade()
    {
        Camera c = Camera.main;
        GameObject bullet = (GameObject)Instantiate(Resources.Load(grenadeName));
        Vector3 point = c.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, c.nearClipPlane));
        bullet.transform.right = c.transform.forward * -1;
        bullet.transform.position = point + c.transform.forward.normalized;
        bullet.GetComponent<Rigidbody>().AddForce(c.transform.forward.normalized * 20, ForceMode.Impulse);
        role.mp -= 33;
    }

    #region public method
    public string getCurrentBulletType()
    {
        return bullet_types[currentBulletType];
    }
    #endregion
}
