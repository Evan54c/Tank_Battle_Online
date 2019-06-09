 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;
using Common.Tools;
using System.Linq;
public class PlayerManager : MonoBehaviour {

    public int lifevalue=3;
    public int scores = 0;
    public bool Isdead;
    public GameObject born;

    public GameObject Enemy_player;
    public bool Isdefeat;
    public Text scorsText;
    public Text lifeText;
    public GameObject UIIsdefeat;

    public Queue<string> queue;

    public string username;
    private SyncActionRequest syncActionRequest;
    private GetinRequest getinRequest;
    private RecoverRequest recoverRequest;
    public Dictionary<string, GameObject> playerDict; 


    public float Timeal;  //攻击CD
    private float WaitTime;

    private float Recover_time;

    private static PlayerManager instance;

    //test 
     int i = 0;
    float time = 0f;

public static PlayerManager Instance
    {
        get
        {
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    public void Awake()
    {
        Instance = this;
        queue =new Queue<string>();
        playerDict = new Dictionary<string, GameObject>();
        
    }
    // Use this for initialization
    void Start () {
        lifevalue = 3;
        scores = 0;
        Recover_time=0;
        username=Random.Range(1,1000).ToString();  //随机名字
        UIIsdefeat.SetActive(false);
        syncActionRequest = GetComponent<SyncActionRequest>();
        getinRequest = GetComponent<GetinRequest>();
        getinRequest.username = username;
        Vector3 position = new Vector3(UnityEngine.Random.Range(-9, 10), UnityEngine.Random.Range(-7, 8), 0);
        GameObject play = Instantiate(born, position, Quaternion.identity);
        play.GetComponent<Born>().Isplayer = true;
        getinRequest.pos =  position;
        getinRequest.direction = DirectiontCode.up;
        getinRequest.DefaultRequest();

        recoverRequest = GetComponent<RecoverRequest>();
        recoverRequest.username = username;
    }
	
	// Update is called once per frame
	void Update () {
        if(Isdefeat)
        {
            UIIsdefeat.SetActive(true);
        }
		if(Isdead==true)
        {
            Recover();
        }
        scorsText.text = scores.ToString();
        lifeText.text = lifevalue.ToString();
	}
    
    void FixedUpdate()
    {
        i ++;
        time = Time.deltaTime;
      // Debug.Log("这是第" + i + "帧");
      //  Debug.Log("时间为:" + time);
        if(Isdead==true)   {
            while(queue.Count!=0)
            {
                queue.Dequeue();
            }
            WaitTime=0;
            return;
        }
        if(WaitTime<1f)
        {
            WaitTime+=Time.fixedDeltaTime;
            return ;
        }

        Move();
       
        if (Timeal >= 0.3f)
        {
            Attack();
        }
        if(Timeal<1f)
        {
            Timeal += Time.fixedDeltaTime;  
        }


         UploadAction();

    }

     void UploadAction()
    {
        //这里得到当前客户端物体的移动信息，填入syncActionRequest中
         //syncActionRequest.act.Enqueue()
        //调用同步动作请求
        Queue q=new Queue(queue);
        while(q.Count!=0)
        {
            string s=(string)q.Dequeue();
            Debug.Log(s);
            if(s[0]=='d') syncActionRequest.act.Enqueue(Actions.Downward);
            else if(s[0]=='u') syncActionRequest.act.Enqueue(Actions.Upward);
            else if(s[0]=='l') syncActionRequest.act.Enqueue(Actions.Toleft);
            else if(s[0]=='r') syncActionRequest.act.Enqueue(Actions.Toright);
            else if(s[0]=='a') syncActionRequest.act.Enqueue(Actions.Fire);
        }
        syncActionRequest.DefaultRequest();
    }

    private void Move()
    {
     //  if(Isdead) return;
         float h = Input.GetAxisRaw("Horizontal");
        //transform.Translate(Vector3.right * h * speed * Time.fixedDeltaTime, Space.World);
        if (h < 0)
        {
         string s="down "+h.ToString();
         queue.Enqueue(s);
         Debug.Log(s);
        }
        if (h > 0)
        {
         string s="up   "+h.ToString();
         queue.Enqueue(s);
         Debug.Log(s);
        }
        
        if (h != 0)
            return;

        float v = Input.GetAxisRaw("Vertical");
       // transform.Translate(Vector3.up * v * speed * Time.fixedDeltaTime, Space.World);
        if (v > 0)
        {
         string s="right"+v.ToString();
         queue.Enqueue(s);
         Debug.Log(s);
        }
        if (v < 0)
        {
         string s="left "+v.ToString();
         queue.Enqueue(s);
         Debug.Log(s);
        }
    }

    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            string s="attck";
            queue.Enqueue(s);
            Debug.Log("attck-----------");
           // CreatBullet(BulletFabs, transform.position, Quaternion.Euler(transform.eulerAngles + BulletEulerAngles));
            Timeal = 0;
        }
    }

    private void Recover()
    {
        if(lifevalue<0)
        {
            Isdefeat = true;
            return ;
        }

        if(Recover_time<1f)
        {
            Recover_time+=Time.fixedDeltaTime;
            return ;
        }
        Recover_time=0;

        
            lifevalue--;
           // GameObject go = Instantiate(born, new Vector3(-2,-8,0), Quaternion.identity);
           //随机产生位置
            Vector3 position = new Vector3(UnityEngine.Random.Range(-9, 10), UnityEngine.Random.Range(-7, 8), 0);
            GameObject go = Instantiate(born, position, Quaternion.identity);
            go.GetComponent<Born>().Isplayer = true;
            Isdead = false;
            /*上传到服务器*/
            recoverRequest.pos=position;
            recoverRequest.direction=DirectiontCode.up;
           // playerDict.Add() 
            recoverRequest.DefaultRequest();
       // syncActionRequest.act.Enqueue(Actions);
       // syncActionRequest.DefaultRequest();
    }


      public void OnGetinResponse(List<PlayerData> PlayerDataList, List<Queue<Actions>> ActionQueueList)
    {
        int i = 0;
        Vector3Data Pos = new Vector3Data();
        string Username;
        Queue<Actions> ActionQueue = new Queue<Actions>();
        //得到其他客户端的Player角色
        foreach (PlayerData playerData in PlayerDataList)
        {
            //初始位置
            Pos = playerData.Pos;
            //用户名
            Username = playerData.Username;
            //动作队列
            ActionQueue = ActionQueueList[i];
            i++;
            /*
             * 
             * 下面实例化其他客户端的角色当前方位
             * 
            */
            OnNewPlayerEvent(Username,Pos);
            OnMove(Username,ActionQueue);


        }
    }
    

    //新用户登录事件
     public void OnNewPlayerEvent(string username, Vector3Data Pos)
    {
        Vector3 pos=new Vector3(Pos.x,Pos.y,Pos.z);
        //Quaternion qu=Pos.direction;
        GameObject go = Instantiate(Enemy_player, pos, Quaternion.identity);
        go.GetComponent<Enemy_player>().Enemy_name=username;
       // Enemy_player en=go.GetComponent<Enemy_player> ();
        playerDict.Add(username,go);
    
    }

    public void OnSyncActionEvent(Dictionary<string, Queue<Actions>> actionDictionary)
    {
        string username;
        Queue<Actions> actions = new Queue<Actions>();
        //更新所有玩家位置
        foreach (KeyValuePair<string, Queue<Actions>> ac in actionDictionary)
        {
            username = ac.Key;
            actions = ac.Value;
            if(username!=getinRequest.username)
                OnMove(username, actions);
        }
    }

    //根据用户名，同步动作
    public void OnMove(string name, Queue<Actions> actions)
    {
        //查找到username对应的物体
        GameObject go = DicTool.GetValue<string, GameObject>(playerDict, name);
       // go.GetComponent<Enemy_player>.queue
       if(go==default(GameObject)) return ;
        while(actions.Count!=0)
        {
            string s="";
           // Debug.Log(go.name+"???");
            Actions a=(Actions)actions.Dequeue();
            /* 
            if(a==Actions.Recover)
            {
                //重新初始化

            }
            */
            if(a==Actions.Upward) s="u";
            else if(a==Actions.Downward) s="d";
            else if(a==Actions.Toleft) s="l";
            else if(a==Actions.Toright) s="r";
            else if(a==Actions.Fire) s="a";
            //go.GetComponent<Enemy_player>.queue=new Queue();
            if(s!="")
            go.GetComponent<Enemy_player>().queue.Enqueue(s);
        }
    }

    //其他用户复活事件
    public void OnRecoverEvent(string username, Vector3Data Pos)
    {
        /*
         * 基本上和OnNewPlayerEvent差不多
         */
       // GameObject g = DicTool.GetValue<string, GameObject>(playerDict, name);
        //GameObject g = playerDict.Where(p=>p.Key==username).Select(p=>p.Value);
        playerDict.Remove(username);
        Vector3 pos=new Vector3(Pos.x,Pos.y,Pos.z);
        //Quaternion qu=Pos.direction;
        GameObject go = Instantiate(Enemy_player, pos, Quaternion.identity);
        playerDict.Add(username,go);
        
    }

    
}
