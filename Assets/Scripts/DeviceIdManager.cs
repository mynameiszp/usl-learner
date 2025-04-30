using UnityEngine;
using System;

public static class DeviceIdManager
{
    private const string Key = "user_id";

    public static string GetUserId()
    {
        if (!PlayerPrefs.HasKey(Key))
        {
            string newId = Guid.NewGuid().ToString();
            PlayerPrefs.SetString(Key, newId);
            PlayerPrefs.Save();
        }

        return PlayerPrefs.GetString(Key);
    }
}