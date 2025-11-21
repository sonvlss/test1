using UnityEngine;

public class randomrotation : MonoBehaviour
{
    void Start()
    {
        float[] possibleAngles = { 0f, 90f, 180f, 270f };
        float randomAngle = possibleAngles[Random.Range(0, possibleAngles.Length)];
        Vector3 rotation = transform.localEulerAngles;
        rotation.y = randomAngle;
        transform.localEulerAngles = rotation;
    }
}
