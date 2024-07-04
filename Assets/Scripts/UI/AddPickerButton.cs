using UnityEngine;
using UnityEngine.UI;

public class AddPickerButton : MonoBehaviour
{
    [SerializeField] private Townhall _townhall;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(BuyPicker);
    }

    private void BuyPicker()
    {
        _townhall.BuyPicker();
    }
}
