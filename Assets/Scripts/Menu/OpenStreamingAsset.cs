using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class OpenStreamingAsset : MonoBehaviour
{
    public string streamingAssetName;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OpenAsset);
    }

    void OpenAsset()
    {
        Application.OpenURL("file:///" + Application.streamingAssetsPath + "/" + streamingAssetName);
    }
}
