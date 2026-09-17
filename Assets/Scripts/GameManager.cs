using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Transform Canvas;

    public Text Timer;

    public Text GameTimer;

    public Slider HpBar;

    public Slider WhiteBar;

    public float hp;

    public float time;

    public Vector2 Rangemin;
    public Vector2 Rangemax;

    public GameObject slowOb;

    public float Nexthuman = 0;

    public Text warnning;

    public Text humaninfo;

    public float middlehuman;

    public GameObject crack;

    public PlayerController playerController;

    public int EatCount;
    public int humanCount;

    public bool gameover;

    public GameObject Endimage;
    public GameObject ClearImage;

    public Text log;

    public Text Tear;

    public Text result;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gameover = true;
        SpawnHuman();
    }

    public Text GetTimerUI()
    {
        return Instantiate(Timer.gameObject, Canvas).GetComponent<Text>();
    }

    public void GameStart()
    {
        Cursor.lockState = CursorLockMode.Locked;
        gameover = false;
        playerController.enabled = true;
    }

    public void GameReStart()
    {
        SceneManager.LoadScene(0);
    }

    public void GameQuit()
    {
        Application.Quit();
    }

    public void Clear()
    {
        Cursor.lockState = CursorLockMode.None;
        gameover = true;
        playerController.enabled = false;
        GameOverUi.gameObject.SetActive(true);
        ClearImage.gameObject.SetActive(true);
        log.text = $"먹은 작물 수  {EatCount}\n" +
            $"주인 등장 수  {humanCount-1}\n" +
            $"클리어 타임  {(Mathf.Floor(time / 60)).ToString("#,##0")} : {(time % 60).ToString("#,#00")}";

        string t="F";
        if(time > 480)
        {
            t = "F";
        }
        else if(time > 420)
        {
            t = "E";
        }
        else if(time > 360)
        {
            t = "D";
        }
        else if(time > 330)
        {
            t = "C";
        }
        else if(time > 300)           
        {
            t = "B";
        }
        else if(time > 270)
        {
            t = "A";
        }
        else
        {
            t = "S";
        }
        Tear.text = "등급 : " + t;

        result.text = "잠입\n성공";
    }

    public Ground[] grounds;

    private void Update()
    {
        if (gameover)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.I)){
            hp -= 100;
            whitehp = true;
        }

        if(hp <= 0)
        {
            hp = 0;
        }

        humaninfo.text = $"주인 도착 예정\n" +
            $"{(Mathf.Floor(middlehuman / 60)).ToString("#,##0")} : {(middlehuman % 60).ToString("#,#00")} ~ {(Mathf.Floor((middlehuman+20) / 60)).ToString("#,##0")} : {((middlehuman + 20) % 60).ToString("#,#00")}";
        HpBar.value = hp / 500f;

        if (whitehp)
        {
            if(WhiteBar.value > HpBar.value)
            {
                WhiteBar.value = WhiteBar.value - 0.5f * Time.deltaTime;
            }
            else
            {
                WhiteBar.value = HpBar.value;
                whitehp = false;
            }
        }

        crack.transform.localScale = new Vector3(0.1f, (1 - hp / 500f) * 0.5f, (1 - hp / 500f) * 0.5f);

        time += Time.deltaTime;

        GameTimer.text = $"현재 시간\n{(Mathf.Floor(time / 60)).ToString("#,##0")} : {(time%60).ToString("#,#00")}";

        if(Nexthuman-time <= 20f)
        {
            humaninfo.transform.parent.gameObject.SetActive(false);
            warnning.transform.parent.gameObject.SetActive(true);
            warnning.text = $"농장 주인 도착까지 {(Nexthuman - time).ToString("#,##0.0")}s";
        }
        else
        {
            warnning.transform.parent.gameObject.SetActive(false);
        }

        if(Nexthuman <= time)
        {
            if (playerController.hide)
            {
                humaninfo.transform.parent.gameObject.SetActive(true);
                SpawnHuman();
                SpawnD();
            }
            else
            {
                GameOver();
            }

        }
    }

    public bool whitehp=false;

    public Transform GameOverUi;

    public void GameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        gameover = true;
        playerController.enabled = false;
        GameOverUi.gameObject.SetActive(true);
        Endimage.gameObject.SetActive(true);
        log.text = $"먹은 작물 수  {EatCount}\n" +
            $"주인 등장 수  {humanCount-1}\n" +
            $"클리어 타임  {(Mathf.Floor(time / 60)).ToString("#,##0")} : {(time % 60).ToString("#,#00")}";

        string t = "F";
        Tear.text = "등급 : " + t;

        result.text = "잠입\n실패";

        //Plus.text = "";
    }

    public Text Plus;

    void SpawnHuman()
    {
        middlehuman = Nexthuman + 60;
        Nexthuman = Random.Range(Nexthuman+60, Nexthuman+80);
        humanCount++;
    }

    public Transform dinfo;

    void SpawnD()
    {
        dinfo.gameObject.SetActive(true);
        for(int i=0;i<Random.Range(1, 4);i++ ){
            Vector3 wid;
            wid.x = Random.Range(Rangemin.x, Rangemax.x);
            wid.z = Random.Range(Rangemin.y, Rangemax.y);
            wid.y = 0.55f;
            Instantiate(slowOb.gameObject, wid, Quaternion.identity);
        }

        bool yes = true;
        for (int i = 0; i < grounds.Length; i++)
        {
            if (!grounds[i].scarecrow)
            {
                yes = false;
            }
        }

        while (!yes)
        {
            int num = Random.Range(0, grounds.Length);
            if (!grounds[num].scarecrow)
            {
                grounds[num].scarecrow = true;
                yes = true;
            }
        }
    }
}
