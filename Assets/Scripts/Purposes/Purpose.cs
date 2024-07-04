using UnityEngine;

public class Purpose : MonoBehaviour 
{
    private const float OffsetCoefficient = 0.1f;

    public Vector3 GetOffset(Vector3 destination)
    {
        return Vector3.Lerp(transform.position, destination, OffsetCoefficient);
    } 
}
