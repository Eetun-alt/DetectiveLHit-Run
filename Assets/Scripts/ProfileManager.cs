using System.Collections.Generic;
using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    public static ProfileManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveProfile(int slot, ProfileData data)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("profile_" + slot, json);
        PlayerPrefs.Save();
    }

    public ProfileData LoadProfile(int slot)
    {
        string key = "profile_" + slot;

        if (!PlayerPrefs.HasKey(key))
            return null;

        string json = PlayerPrefs.GetString(key);
        return JsonUtility.FromJson<ProfileData>(json);
    }

    public void DeleteProfile(int slot)
    {
        PlayerPrefs.DeleteKey("profile_" + slot);
    }

    //Hakee kaikki olemassa olevat profiilit
    public List<ProfileData> GetAllProfiles(int maxSlots = 3)
    {
        List<ProfileData> profiles = new List<ProfileData>();

        for (int slot = 1; slot <= maxSlots; slot++)
        {
            ProfileData data = LoadProfile(slot);
            if (data != null)
            {
                profiles.Add(data);
            }
        }

        return profiles;
    }

    //tiedot sloteista
    public Dictionary<int, ProfileData> GetAllProfilesWithSlot(int maxSlots = 3)
    {
        Dictionary<int, ProfileData> profiles = new Dictionary<int, ProfileData>();

        for (int slot = 1; slot <= maxSlots; slot++)
        {
            ProfileData data = LoadProfile(slot);
            if (data != null)
            {
                profiles.Add(slot, data);
            }
        }

        return profiles;
    }
}
