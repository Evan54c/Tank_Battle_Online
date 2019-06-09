using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Common.Tools;

public class Player : MonoBehaviour {

    public float speed = 3;
    public Vector3 BulletEulerAngles;
    public float Timeal;     //攻击CD
    public SpriteRenderer sr;
    public GameObject BulletFabs;
    public GameObject ExplosionPrefab;
    public  bool IsDefended ;
    public GameObject DefendEffecPrefab;
    

    public float DefendTime=4f;

    public Sprite[] tankerSprite;//上右下左
    public void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start () {
        DefendTime = 3f;
        IsDefended = true;
        Timeal = 0;
        Instantiate(BulletFabs, transform.position, Quaternion.Euler(transform.eulerAngles + BulletEulerAngles));
    }

// Update is called once per frame
void Update () {

        if(IsDefended)
        {
            DefendEffecPrefab.SetActive(true);
            DefendTime -= Time.deltaTime;
            if(DefendTime<0)
            {
                IsDefended = false;
                DefendEffecPrefab.SetActive(false);
            }
        }

	}

     private void FixedUpdate()
    {
        if(PlayerManager.Instance.Isdefeat==true)
        {
            return;
        }
        while(PlayerManager.Instance.queue.Count!=0)
        {
            string s=(string)PlayerManager.Instance.queue.Dequeue();
            if(s=="attck")  Attack();
            else 
            {
                string s0=s.Substring(0,5);
                string s1=s.Substring(5,s.Length-5);
                float t=float.Parse(s1);
                float h=0,v=0;
                if(s0=="up   "||s0=="down ") h=t;
                else if(s0=="left "||s0=="right") v=t;
                Move(h,v);
            }
        }
/* 
        Move();
       
        if (Timeal >= 0.3f)
        {
            Attack();
        }
        if(Timeal<1f)
        {
            Timeal += Time.fixedDeltaTime;  
        }
*/
    }
//

    //攻击方法
    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreatBullet(BulletFabs, transform.position, Quaternion.Euler(transform.eulerAngles + BulletEulerAngles));
            Timeal = 0;
        }
    }

   private void CreatBullet(GameObject gameObject,Vector3 creatposition,Quaternion creatrotation)
    {
        GameObject item = Instantiate(gameObject, creatposition, creatrotation);
        //item.transform.SetParent(gameObject.transform);
    }
    

    //移动方法
    private void Move (float h,float v)
    {
       // float h = Input.GetAxisRaw("Horizontal");
        transform.Translate(Vector3.right * h * speed * Time.fixedDeltaTime, Space.World);
        if (h < 0)
        {
            sr.sprite = tankerSprite[3];
            BulletEulerAngles = new Vector3(0, 0, 90);
        }
        if (h > 0)
        {
            sr.sprite = tankerSprite[1];
            BulletEulerAngles = new Vector3(0, 0, -90);
        }
        if (h != 0)
            return;

     //   float v = Input.GetAxisRaw("Vertical");
        transform.Translate(Vector3.up * v * speed * Time.fixedDeltaTime, Space.World);
        if (v > 0)
        {
            sr.sprite = tankerSprite[0];
            BulletEulerAngles = new Vector3(0, 0, 0);
        }
        if (v < 0)
        {
            sr.sprite = tankerSprite[2];
            BulletEulerAngles = new Vector3(0, 0, 180);
        }
    }

    //死亡方法
    private void Die()
    {

        if (IsDefended)
        {
            return;
        }
        while(PlayerManager.Instance.queue.Count!=0)
        {
            PlayerManager.Instance.queue.Dequeue();
        }

        PlayerManager.Instance.Isdead = true;
        //触发爆炸特效
        Instantiate(ExplosionPrefab, transform.position,transform.rotation);
        //销毁对象
        Destroy(gameObject);
    }
}
