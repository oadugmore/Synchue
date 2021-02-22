using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class OpenUrl : MonoBehaviour
{
    public string url;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(ButtonClicked);
    }

    void ButtonClicked()
    {
        Application.OpenURL(url);
    }
}
