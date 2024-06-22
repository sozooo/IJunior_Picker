using System;
using UnityEngine;

public class Stone : Purpose
{
    [SerializeField] private float _resourceCount;

    public event Action<Stone> OnDead; 

    public float GiveResources()
    {
        OnDead?.Invoke(this);

        return _resourceCount;
    }
}
