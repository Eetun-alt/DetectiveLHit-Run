using TMPro;
using UnityEngine;

public class ProfileButtonUI : MonoBehaviour
{
    public TMP_Text profile1Text;
    public TMP_Text profile2Text;
    public TMP_Text profile3Text;

    void Start()
    {
        profile1Text.text = ProfileManager.Instance.GetProfileName(1);
        profile2Text.text = ProfileManager.Instance.GetProfileName(2);
        profile3Text.text = ProfileManager.Instance.GetProfileName(3);
    }
}
