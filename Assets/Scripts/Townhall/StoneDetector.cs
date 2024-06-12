using System;
using UnityEngine;

public class StoneDetector : MonoBehaviour
{
    public event Action<Stone> OnDetect;

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.TryGetComponent<Stone>(out Stone stone))
        {
            OnDetect?.Invoke(stone);
        }
    }
}
