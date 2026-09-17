using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Crop : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private CropData cropData;

    [Header("State")]
    [SerializeField] private bool isScarecrowApplied;

    [Header("Visual")]
    [SerializeField] private GameObject visualObject;
    public GameObject saaSok;

    private bool canEat = true;

    public CropData Data => cropData;

    public string CropName => cropData != null ? cropData.CropName : "None";
    public float EatTime => cropData != null ? cropData.EatTime : 0f;
    public float FullnessAmount => cropData != null ? cropData.FullnessAmount : 0f;
    public float RegrowTime => cropData != null ? cropData.RegrowTime : 0f;

    public bool IsScarecrowApplied => ground.scarecrow;

    Text TimerUi;

    float growtime;

    public Ground ground;

    private void Start()
    {
        ground = transform.parent.GetComponent<Ground>();
        TimerUi = GameManager.instance.GetTimerUI();
    }

    private void Update()
    {
        if (!canEat)
        {
            TimerUi.gameObject.SetActive(true);
            TimerUi.transform.position = transform.position + new Vector3(0, 1, 0);
            growtime += Time.deltaTime;
            TimerUi.text = $"{(RegrowTime - growtime).ToString("#,##0.0")}s";
            if(growtime >= RegrowTime)
            {
                canEat = true;
            }
            saaSok.gameObject.SetActive(true);
            visualObject.gameObject.SetActive(false);
            saaSok.transform.localScale = new Vector3((growtime / RegrowTime) * 1.5f, (growtime / RegrowTime) * 1.5f, (growtime / RegrowTime) * 1.5f);
        }
        else
        {
            visualObject.gameObject.SetActive(true);
            saaSok.gameObject.SetActive(false);
            TimerUi.gameObject.SetActive(false);
        }
    }

    public bool IsEat()
    {
        if (canEat)
        {
            return true;
        }

        return canEat;
    }

    public void Eat()
    {
        canEat = false;
        growtime = 0;
    }
}