using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class StoneCounter : MonoBehaviour
{
    [SerializeField] private Townhall _townhall;

    private TextMeshProUGUI _text;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        _townhall.OnResourseCountChange += DisplayCount;
    }

    private void OnDisable()
    {
        _townhall.OnResourseCountChange -= DisplayCount;
    }

    private void DisplayCount(float currentValue) 
    {
        _text.text = currentValue.ToString();
    }
}
