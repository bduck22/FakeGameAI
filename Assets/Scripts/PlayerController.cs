using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Fullness")]
    [SerializeField] private float maxFullness = 100f;
    [SerializeField] private float currentFullness = 0f;

    [Header("Warehouse Attack")]
    [SerializeField] private float warehouseFullnessCostPerSecond = 5f;
    [SerializeField] private float warehouseDamagePerSecond = 5f;

    public bool IsSticky { get; private set; }

    public float CurrentFullness => currentFullness;
    public float MaxFullness => maxFullness;

    public bool stop;

    public Crop currentcrop;

    public void ChangeFull(float value)
    {
        currentFullness = Mathf.Clamp(currentFullness+value, 0, maxFullness);
    }

    float Eatingtime;

    public Text StateUi;

    public GameObject Cropinfo;
    public Text Cropname;
    public Text Cropdescription;


    public Transform ScareUI;

    public bool doorin;

    public Image fullslide;
    public Text fulltext;

    public bool hide;

    public GameObject Black;

    public ParticleSystem effect;
    private void Update()
    {
        fullslide.fillAmount = (currentFullness / maxFullness);
        fulltext.text = $"포만감\n" +
            $"{((currentFullness / maxFullness)*100f).ToString("#,##0")}%";

        if (currentcrop)
        {
            if (!Cropinfo.activeSelf)
            {
                Cropname.text = currentcrop.CropName;
                Cropdescription.text = $"먹는 시간 : {currentcrop.EatTime}\n" +
                    $"포만감 : {currentcrop.FullnessAmount}\n" +
                    $"재생 시간 : {currentcrop.RegrowTime}";
                Cropinfo.SetActive(true);
            }
            ScareUI.gameObject.SetActive(currentcrop.IsScarecrowApplied);
        }
        else
        {
            if (Cropinfo.activeSelf)
            {
                Cropinfo.gameObject.SetActive(false);
            }
            ScareUI.gameObject.SetActive(false);
        }

        if (hide)
        {
            StateUi.text = "숨어 있는 중 (아무키나 눌러 해제)";
            if (Input.anyKeyDown)
            {
                hide = false;
                Black.gameObject.SetActive(false);
                StateUi.transform.parent.gameObject.SetActive(false);
                stop = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (currentcrop)
            {
                if (currentcrop.IsEat())
                {
                    stop = true;
                    StateUi.transform.parent.gameObject.SetActive(true);
                }
            }

            if (Canhide)
            {
                hide = true;
                Black.gameObject.SetActive(true);
                StateUi.transform.parent.gameObject.SetActive(true);
                stop = true;
            }

            if (doorin)
            {
                if (GameManager.instance.hp <= 0)
                {
                    GameManager.instance.Clear();
                    enabled = false;
                }
            }
        }

        if (Input.GetKey(KeyCode.F))
        {
            if(GameManager.instance.hp > 0) {
                if (doorin)
                {
                    if (CurrentFullness > 0)
                    {
                        GameManager.instance.whitehp = false;   
                        stop = true;
                        GameManager.instance.hp -= warehouseDamagePerSecond * Time.deltaTime;
                        currentFullness -= warehouseFullnessCostPerSecond * Time.deltaTime;
                        currentFullness = Mathf.Clamp(currentFullness, 0, maxFullness);
                        StateUi.transform.parent.gameObject.SetActive(true);
                        StateUi.text = $"포만감을 소모하여 문 갉아 먹는 중... \n(F키 유지하기)";
                    }
                    else
                    {
                        stop = false; StateUi.transform.parent.gameObject.SetActive(false);
                        GameManager.instance.whitehp = true;
                    }
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            if (doorin)
            {
                stop = false; StateUi.transform.parent.gameObject.SetActive(false);
                GameManager.instance.whitehp = true;
            }
        }

        if (stop)
        {
            if (!effect.isPlaying)
            {
                effect.Play();
            }

            if (currentcrop)
            {
                Eatingtime += Time.deltaTime;
                StateUi.text = $"작물 먹는 중... {((currentcrop.EatTime * (currentcrop.IsScarecrowApplied ? 1.5f : 1)) - Eatingtime).ToString("#,##0.0")}s\n" +
                    $"(취소 불가)";
                if(Eatingtime >= (currentcrop.EatTime*(currentcrop.IsScarecrowApplied?1.5f:1)))
                {
                    ChangeFull(currentcrop.FullnessAmount);
                    StateUi.transform.parent.gameObject.SetActive(false);
                    Eatingtime = 0;
                    stop = false;
                    currentcrop.Eat();
                    GameManager.instance.EatCount++;
                }
            }
        }
        else
        {
            if (effect.isPlaying)
            {
                effect.Stop();
            }
        }
    }

    public bool Canhide;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("slow"))
        {
            GetComponent<PlayeryMovement>().slow = true;
        }

        if (other.CompareTag("Hide"))
        {
            Canhide = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("crop"))
        {
            Crop crop = other.GetComponent<Crop>();
            if (crop.IsEat())
            {
                currentcrop = crop;
            }
            else
            {
                currentcrop = null;
            }
        }

        if (other.CompareTag("door"))
        {
            doorin = true;
            if (GameManager.instance.hp <= 0)
            {
                StateUi.transform.parent.gameObject.SetActive(true);
                StateUi.text = $"F키를 눌러 창고 진입";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("door"))
        {
            doorin = false;
        }
        if (other.CompareTag("slow"))
        {
            GetComponent<PlayeryMovement>().slow = false;
        }
        if (other.CompareTag("Hide"))
        {
            Canhide = false;
        }
        if (other.CompareTag("crop"))
        {
            currentcrop = null;
        }
    }
}