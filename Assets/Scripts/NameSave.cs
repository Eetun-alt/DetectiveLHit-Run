using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NameSave : MonoBehaviour
{
    public TMP_InputField inputField;

    public void SaveName()
    {
        PlayerPrefs.SetString("PlayerName", inputField.text);
    }

    public void LoadName()
    {
        inputField.text = PlayerPrefs.GetString("PlayerName", "No Name");
    }

    public void ClearName()
    {
        PlayerPrefs.DeleteKey("PlayerName");
        PlayerPrefs.DeleteAll();
    }
}