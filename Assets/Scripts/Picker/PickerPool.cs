using System.Collections.Generic;
using UnityEngine;

public class PickerPool : MonoBehaviour
{
    [SerializeField] private Picker _pickerPrefab;

    private Queue<Picker> _pickers = new Queue<Picker>();

    public void Add(Picker picker)
    {
        _pickers.Enqueue(picker);
    }

    public Picker Take()
    {
        if(_pickers.Count == 0)
        {
            Add(Instantiate(_pickerPrefab, transform));
        }

        return _pickers.Dequeue();
    }
}