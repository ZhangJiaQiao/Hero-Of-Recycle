using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss2 : MonoBehaviour {
    public float maxVelocity = 50;//最大速度
    private Transform player;//玩家位置
    private float AIthinkLastTime;//AI上次思考的时间
    private characterProperty CharacterProperty;//角色属性
    private Animator animator;//怪物状态机
    private Rigidbody rigibody;//怪物刚体
    private bool isAlive = true;
    private int CallNum = 0;//呼叫的怪物数量
    private BossState state = 0;//AI状态
    private float TraceTime = 5.0f;//追踪时间
    private float TargetTime = 2.0f;//变换walk和run时间
    private Vector3 TargetPos;//walk和run的目标
    private const float DetectDis = 6.5f;//检测距离
    private Vector3 HitNormal;//障碍物法线
    private float CollisionDis;//障碍物距离
    private bool IScollision = false;//碰撞
    private float RiseTime = 1f;//上升时间
    private float ChangeTagTime = 5f;//改变Tag时间
    private AvoidState avoidState;//障碍状态
    private bool finishAttack = true;
    public delegate void destroy();//死亡毁灭委托
    public event destroy destroyEvent;//事件
    public GameObject label;//垃圾属性字体
    public GameObject texture;//垃圾属性图片
    public UIProgressBar HPBar;//血条
    private string[] trashName = { "battery", "bone", "china", "clothe", "dirtypaper", "dusty","fruit", "glass", "greens", "ink", "leave",
        "light", "medicine", "metal", "milk", "oil", "once", "paper", "pet", "plastic", "rise", "smoke", "tea", "pesticide"};

    private enum AvoidState
    {
        Push,Pull,Parallel,
    };

    private enum BossState
    {
        Idle,Walk,Run,Trace,Attack1,Attack2,Attack3,Died,
    };

    // Use this for initialization
    void Start () {
        CharacterProperty = this.gameObject.GetComponent<characterProperty>();
        animator = GetComponent<Animator>();
        player = SSDirector.getInstance().currentSceneController.getPlayer();
        rigibody = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        HPBar.GetComponent<HpUISlider>().UpdateVal(CharacterProperty.life / 300);
        //Debug.Log(Vector3.Distance(transform.position, player.position));
        if (isAlive)
        {
            if (state != BossState.Trace && state != BossState.Attack1 && state != BossState.Attack2 && state != BossState.Attack3 && IsAIthink())
            {
                SetState();
            }
            else
            {
                UpState();
            }
        }
        UpdateCharacterProperty();
        ChangeTagTime -= Time.deltaTime;
        if (ChangeTagTime <= 0)
        {
            changeTag();
            ChangeTagTime = 5;
        }
    }

    void UpdateCharacterProperty()
    {
        if (CharacterProperty.speed <= 0.0f)
            CharacterProperty.speed = 0.0f;
        if (CharacterProperty.damageValue <= 0.0f)
            CharacterProperty.damageValue = 0.0f;
        if (CharacterProperty.life <= 0)
        {
            animator.SetBool("Dead", true);
            rigibody.useGravity = true;
            isAlive = false;
            if (destroyEvent != null)
            {
                destroyEvent();
            }
            if (CallNum == 0)
            {
                destroyItself();
            }
        }
        if (CharacterProperty.life >= 300)
        {
            CharacterProperty.life = 300;
        }
    }

    void SetState()
    {
        /*
         *通过随机数随机状态 
         */
        float R = UnityEngine.Random.value;
        if(R < 0.5)
        {
            state = BossState.Idle;
        }
        else
        {
            state = BossState.Walk;
        }
    }

    void UpState()
    {
        if(state == BossState.Attack1 || state == BossState.Attack2 || state == BossState.Attack3)
        {
            //Debug.Log("333");
            AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
            if(info.IsName("Attack1") || info.IsName("Attack2") || info.IsName("Attack3"))
            {
                if (info.normalizedTime > 1.0f)
                {
                    //Debug.Log("Finish");
                    finishAttack = true;
                    animator.SetBool("Attack1", false);
                    animator.SetBool("Attack2", false);
                    animator.SetBool("Attack3", false);
                    state = BossState.Trace;
                }
            }
            TraceTime -= Time.deltaTime;
        }
        else if (findPlayer() && !closeToAttack() && state != BossState.Trace)
        {
            ChooseState();
        }
        if (state == BossState.Trace)
        {
            TraceTime -= Time.deltaTime;
            if (TraceTime < 0.0f)
            {
                TraceTime = 5.0f;
                state = BossState.Idle;
            }
        }
        switch(state)
        {
            case BossState.Idle:
                idle();
                break;
            case BossState.Walk:
                walk();
                break;
            case BossState.Run:
                run();
                break;
            case BossState.Trace:
                TracePlayer();
                break;
            case BossState.Attack1:
                attack1();
                break;
            case BossState.Attack2:
                attack2();
                break;
            case BossState.Attack3:
                attack3();
                break;
        }

    }

    void ChooseState()
    {
        //选择状态
        float R = UnityEngine.Random.value;
        if(R < 0.1)
        {
            state = BossState.Idle;
        }
        else if(R < 0.2)
        {
            state = BossState.Walk;
        }
        else if(R < 0.45)
        {
            state = BossState.Run;
        }
        else
        {
            state = BossState.Trace;
        }
    }

    void ChooseAttackState()
    {
        float R = UnityEngine.Random.value;
        if(R < 0.35)
        {
            state = BossState.Attack1;
        }
        else if (R < 0.65)
        {
            state = BossState.Attack2;
        }
        else
        {
            state = BossState.Attack3;
        }
    }

    bool findPlayer()
    {
        return Vector3.Distance(this.transform.position, player.position) < 22;
    }

    bool closeToAttack()
    {
        return Vector3.Distance(transform.position, player.position) < 3.5;
    }

    void idle()
    {
        animator.SetInteger("State",0);
        RaycastHit hit;
        int layerMask = 1 << 13;
        if(Physics.Raycast(this.transform.position,Vector3.down,out hit, 100.0f, layerMask))
        {
            if(hit.distance < 12)
            {
                rigibody.AddForce(Vector3.up * 5f, ForceMode.Force);//模拟上升力
                rigibody.velocity = new Vector3(0, rigibody.velocity.y, 0);
            }
            else if(hit.distance > 13)
            {
                rigibody.AddForce(Vector3.down, ForceMode.Force);//模拟下降力
                rigibody.velocity = new Vector3(0, rigibody.velocity.y, 0);
            }
            else
            {
                rigibody.velocity = Vector3.zero;
            }
        }
    }

    void walk()
    {
        if (TargetTime == 2)
        {
            do
            {
                TargetPos = new Vector3(UnityEngine.Random.Range(-50.0f, -10.0f), this.transform.position.y, UnityEngine.Random.Range(-40.0f, -20.0f));
            }
            while (Vector3.Distance(TargetPos, transform.position) < 5);
        }
        Vector3 targetDir = TargetPos - transform.position;
        float step = 10 * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
        transform.rotation = Quaternion.LookRotation(newDir);//转向
        Vector3 currentV = CharacterProperty.speed * transform.forward.normalized;//向前走
        rigibody.velocity = currentV;
        TargetTime -= Time.deltaTime;
        if (TargetTime <= 0)
        {
            TargetTime = 2;
        }
        animator.SetInteger("State", 1);
    }

    void run()
    {
        if (TargetTime == 2)
        {
            do
            {
                TargetPos = new Vector3(UnityEngine.Random.Range(-50.0f, -10.0f), this.transform.position.y, UnityEngine.Random.Range(-40.0f, -20.0f));
            }
            while (Vector3.Distance(TargetPos, transform.position) < 10);
        }
        Vector3 targetDir = TargetPos - transform.position;
        float step = 10 * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
        transform.rotation = Quaternion.LookRotation(newDir);//转向
        Vector3 currentV = 2 * CharacterProperty.speed * transform.forward.normalized;//向前走
        rigibody.velocity = currentV;
        TargetTime -= Time.deltaTime;
        if (TargetTime <= 0)
        {
            TargetTime = 2;//每两秒钟改变一次位置
        }
        animator.SetInteger("State", 2);
    }

    void TracePlayer()
    {
        Vector3 targetDir = player.position - transform.position;
        Vector3 currentV;
        float step = 10 * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
        transform.rotation = Quaternion.LookRotation(newDir);
        if (closeToAttack())
        {
            ChooseAttackState();
            rigibody.velocity = Vector3.zero;
            //Debug.Log("222");
        }
        else
        {
            if(IScollision)
            {
                RiseTime -= Time.deltaTime;
                if(RiseTime <= 0)
                {
                    IScollision = false;
                    RiseTime = 1;
                }
            }
            else if (DetectCollison())
            {
                AvoidObstacles();
            }
            else
            {
                currentV = 3 * CharacterProperty.speed * transform.forward.normalized;
                rigibody.velocity = currentV;
            }
        }
        animator.SetInteger("State", 2);
    }

    void attack1()
    {
        rigibody.velocity = Vector3.zero;
        animator.SetBool("Attack1", true);
    }

    void attack2()
    {
        rigibody.velocity = Vector3.zero;
        animator.SetBool("Attack2", true);
    }

    void attack3()
    {
        Debug.Log("Attack");
        rigibody.velocity = Vector3.zero;
        animator.SetBool("Attack3", true);
        if(finishAttack)
        {
            creatMonsters();
            finishAttack = false;
        }
    }

    void creatMonsters()
    {
        List<string> types = new List<string> { "foodTrash", "recyclableTrash", "otherTrash", "harmfulTrash" };
        for (int i = 0; i < 3; i++)
        {
            int type = UnityEngine.Random.Range(0, types.Count);
            myFactory mF = Singleton<myFactory>.Instance;
            GameObject monster = mF.getMonster(types[type]);
            switch (types[type])
            {
                case "foodTrash":
                    {
                        ghost g = monster.GetComponent<ghost>();
                        if (g != null)
                        {
                            g.destroyEvent += monsterDestroyHandler;
                        }
                        else
                        {
                            crab c = monster.GetComponent<crab>();
                            c.destroyEvent += monsterDestroyHandler;
                        }
                        break;
                    }
                case "harmfulTrash":
                    {
                        ghost g = monster.GetComponent<ghost>();
                        g.destroyEvent += monsterDestroyHandler;
                        break;
                    }
                case "recyclableTrash":
                    {
                        bone b = monster.GetComponent<bone>();
                        b.destroyEvent += monsterDestroyHandler;
                        break;
                    }
                case "otherTrash":
                    {
                        bone b = monster.GetComponent<bone>();
                        b.destroyEvent += monsterDestroyHandler;
                        break;
                    }
            }
            monster.transform.position = this.transform.position + new Vector3(UnityEngine.Random.Range(1, 4), 0, UnityEngine.Random.Range(1, 4));
        }
        CallNum += 3;
    }

    void monsterDestroyHandler()
    {
        CallNum -= 1;
    }

    public int GetCallNum()
    {
        return CallNum;
    }

    bool DetectCollison()
    {
        RaycastHit hit;
        int layerMask = 1 << 13;
        bool IsDetect = Physics.Raycast(transform.position, rigibody.velocity, out hit, DetectDis, layerMask);
        HitNormal = hit.normal;//碰撞点法线
        HitNormal.y = 0.0f;
        CollisionDis = hit.distance;//碰撞距离
        return IsDetect;
    }

    void AvoidObstacles()
    {
        //Debug.Log("111");
        StateManager();
        switch(avoidState)
        {
            case AvoidState.Push:
                UpdatePush();
                break;
            case AvoidState.Pull:
                UpdatePull();
                break;
            case AvoidState.Parallel:
                UpdateParallel();
                break;
        }
    }

    void StateManager()
    {
        if(CollisionDis > 5)
        {
            avoidState = AvoidState.Push;
        }
        else if (CollisionDis > 2)
        {
            avoidState = AvoidState.Pull;
        }
        else
        {
            avoidState = AvoidState.Parallel;
        }
    }

    void UpdatePush()
    {
        rigibody.AddForce(HitNormal * 10);
    }

    void UpdatePull()
    {
        Vector3 pullForce = Vector3.Cross(HitNormal, Vector3.up).normalized;
        if (Vector3.Dot(rigibody.velocity.normalized, pullForce) > 0)
        {
            rigibody.AddForce(pullForce * 10);
        }
        else
        {
            rigibody.AddForce(-pullForce * 10);
        }
    }

    void UpdateParallel()
    {
        Vector3 paraSpeed = Vector3.Cross(HitNormal, Vector3.up).normalized;
        if(Vector3.Dot(rigibody.velocity.normalized, paraSpeed) > 0)
        {
            rigibody.velocity = paraSpeed * 10;
        }
        else
        {
            rigibody.velocity = -paraSpeed * 10;
        }
    }

    void changeTag()
    {
        int randomNum = UnityEngine.Random.Range(0, 24);                //所有垃圾的种类数目。
        string nameOfTrash = trashName[randomNum];
        string tag = "";
        if (nameOfTrash == "battery" || nameOfTrash == "ink" || nameOfTrash == "light" || nameOfTrash == "medicine" || nameOfTrash == "oil" || nameOfTrash == "pesticide")
            tag = "harmfulTrash";
        else if (nameOfTrash == "clothe" || nameOfTrash == "glass" || nameOfTrash == "metal" || nameOfTrash == "milk" || nameOfTrash == "paper" || nameOfTrash == "plastic")
            tag = "recyclableTrash";
        else if (nameOfTrash == "bone" || nameOfTrash == "fruit" || nameOfTrash == "greens" || nameOfTrash == "leave" || nameOfTrash == "rise" || nameOfTrash == "tea")
            tag = "foodTrash";
        else if (nameOfTrash == "china" || nameOfTrash == "dirtypaper" || nameOfTrash == "dusty" || nameOfTrash == "pet" || nameOfTrash == "smoke" || nameOfTrash == "once")
            tag = "otherTrash";

        //Transform PanelOfBoss = this.transform.Find ("Panel");
        label.GetComponent<UILabel>().text = nameOfTrash;
        UITexture textureComponent = texture.GetComponent<UITexture>();
        textureComponent.mainTexture = Resources.Load("Texture/label/" + nameOfTrash) as Texture2D;
        this.gameObject.tag = tag;
    }

    bool IsAIthink()
    {
        //这里表示AI每3秒进行一次思考
        if (Time.time - AIthinkLastTime >= 3.0f)
        {
            AIthinkLastTime = Time.time;//记录AI上次思考时间
            return true;
        }
        return false;
    }

    void destroyItself()
    {
        StartCoroutine("Dispear");
    }

    IEnumerator Dispear()
    {
        yield return new WaitForSeconds(5);
        this.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 12)
        {
            IScollision = true;
            transform.rotation = Quaternion.Euler(Vector3.zero);
            rigibody.velocity = Vector3.up * 10;
        }
        else if (collision.gameObject.tag == this.gameObject.tag)
        {
            CharacterProperty.speed -= 0.1f;
            CharacterProperty.life -= collision.gameObject.GetComponent<Bullet>().GetDamage();
            CharacterProperty.damageValue -= 2f;
        }
        else if(collision.gameObject.tag != this.gameObject.tag)
        {
            CharacterProperty.speed += 0.2f;
            CharacterProperty.life += collision.gameObject.GetComponent<Bullet>().GetDamage();
            CharacterProperty.damageValue += 2f;
        }
    }
}
