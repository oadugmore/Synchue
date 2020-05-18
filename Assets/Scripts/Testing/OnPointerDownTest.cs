using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnPointerDownTest : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Image pointerDownTest;
    public Text pointerDownTime;

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDownTest.color = Color.red;
        pointerDownTime.text = Time.time.ToString();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerDownTest.color = Color.white;
        pointerDownTime.text = Time.time.ToString();
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
