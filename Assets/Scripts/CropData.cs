using UnityEngine;

[CreateAssetMenu(
    fileName = "New Crop Data",
    menuName = "Game/Crop Data"
)]
public class CropData : ScriptableObject
{
    [Header("Basic Info")]
    [SerializeField] private string cropName = "´ç±Ù";

    [Header("Eat")]
    [SerializeField] private float eatTime = 2f;
    [SerializeField] private float fullnessAmount = 10f;

    [Header("Regrow")]
    [SerializeField] private float regrowTime = 5f;

    public string CropName => cropName;
    public float EatTime => eatTime;
    public float FullnessAmount => fullnessAmount;
    public float RegrowTime => regrowTime;
}