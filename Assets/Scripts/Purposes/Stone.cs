using System;
using UnityEngine;

public class Stone : Purpose
{
    [SerializeField] private float _resourceCount;

    public event Action OnDead; 

    public float ResourceCount {
        get { return _resourceCount; }
        private set {
            _resourceCount = value;

            if (_resourceCount <= 0)
            {
                Dead();
            }
        }
    }

    private void Dead()
    {
        Destroy(gameObject);

        OnDead?.Invoke();
    }

    public void ChangeResourceCount(float resourcesTaken)
    {
        _resourceCount -= resourcesTaken;
    }
}
