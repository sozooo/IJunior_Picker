using UnityEngine;
using UnityEngine.UI;

public class PlaceFlagButton : MonoBehaviour
{
    [SerializeField] private FlagPlacer _flagPlacer;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Place);
    }

    private void Place()
    {
        _flagPlacer.StartPlacing();
    }
}
