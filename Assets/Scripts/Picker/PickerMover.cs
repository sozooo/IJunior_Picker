using System.Collections;
using UnityEngine;

public class PickerMover : MonoBehaviour
{
    [SerializeField] private float _speed;

    public IEnumerator Move(Purpose purpose)
    {
        transform.LookAt(purpose.transform);
        Vector3 destination = purpose.GetOffset(transform.position);

        while (transform.position != destination)
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, _speed * Time.deltaTime);

            yield return null;
        }
    }
}
