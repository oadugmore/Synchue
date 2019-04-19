using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnPointerDownTest : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Image pointerDownTest;

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDownTest.color = Color.red;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerDownTest.color = Color.white;
    }

    // Start is called before the first frame update
    void Start()
    {
        //pointerDownTest = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
