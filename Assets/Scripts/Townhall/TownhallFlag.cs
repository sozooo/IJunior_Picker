using System;
using UnityEngine;

public class TownhallFlag : Purpose
{
    [SerializeField] private Townhall _prefab;

    public event Action OnBuilded;

    private void OnDestroy()
    {
        OnBuilded?.Invoke();
    }

    public Townhall CreateTownhall()
    {
        Townhall townhall = Instantiate(_prefab, transform.position, Quaternion.identity);

        Destroy(gameObject);

        return townhall;
    }
}
