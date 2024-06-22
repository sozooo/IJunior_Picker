using System;
using System.Collections.Generic;
using UnityEngine;

public class StoneDetector : MonoBehaviour
{
    [SerializeField] private Vector3 _center;
    [SerializeField] private Vector3 _halfExtents;

    private Collider[] _scannedObjects;
    private List<Stone> _stones;

    private void OnEnable()
    {
        ScanForStones();
    }

    public List<Stone> ScanForStones()
    {
        _stones = new List<Stone>();

        _scannedObjects = Physics.OverlapBox(_center, _halfExtents, Quaternion.identity);

        foreach (Collider collider in _scannedObjects)
        {
            if(collider.TryGetComponent(out Stone stone))
            {
                _stones.Add(stone);
            }
        }

        return _stones;
    }
}
