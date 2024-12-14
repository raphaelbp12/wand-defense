using UnityEngine;

public class OpenURLButton : MonoBehaviour
{
    public string url = "https://www.unity3d.com";

    public void OpenURL()
    {
        Application.OpenURL(url);
    }
}
