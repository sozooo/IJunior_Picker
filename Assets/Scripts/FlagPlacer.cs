using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlagPlacer : MonoBehaviour
{
    [SerializeField] private TownhallFlag _flagPrefab;

    private Camera _camera;
    private TownhallFlag _flag;
    private Coroutine _coroutine;

    public event Action<TownhallFlag> OnPLace;

    private void Awake()
    {
        _camera = Camera.main;
        _flag = new TownhallFlag();
    }

    public void StartPlacing()
    {
        if(_coroutine == null)
        {
            _coroutine = StartCoroutine(PlaceFlag());
        }
    }

    private void Place()
    {
        Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            _flag = Instantiate(_flagPrefab, hit.point, Quaternion.identity);
        }

        OnPLace?.Invoke(_flag);
    }

    private IEnumerator PlaceFlag()
    {
        yield return new WaitUntil(() => Mouse.current.leftButton.wasPressedThisFrame);

        Place();
    }
}
