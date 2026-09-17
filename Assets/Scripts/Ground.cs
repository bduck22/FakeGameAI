using UnityEngine;

public class Ground : MonoBehaviour
{
    public bool scarecrow;

    public Transform ScareCrow;

    private void Update()
    {
        ScareCrow.gameObject.SetActive(scarecrow);
    }
}
