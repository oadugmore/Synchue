using System.IO;
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
        Application.OpenURL(Path.Combine(Application.streamingAssetsPath, streamingAssetName));
    }
}
