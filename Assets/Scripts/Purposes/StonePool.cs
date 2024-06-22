using System.Collections.Generic;
using UnityEngine;

public class StonePool : MonoBehaviour 
{
    [SerializeField] private Stone _stonePrefab;

    private Queue<Stone> _stones = new Queue<Stone>();

    public void Add(Stone stone)
    {
        _stones.Enqueue(stone);
    }

    public Stone Take()
    {
        if (_stones.Count == 0)
        {
            Add(Instantiate(_stonePrefab));
        }

        return _stones.Dequeue();
    }
}
