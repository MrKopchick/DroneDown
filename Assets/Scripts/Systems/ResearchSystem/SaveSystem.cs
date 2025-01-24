using System.IO;
using UnityEngine;

public static class SaveSystem
{
    public static void Save<T>(T data, string filename)
    {
        string path = Path.Combine(Application.persistentDataPath, filename);
        File.WriteAllText(path, JsonUtility.ToJson(data, true));
        Debug.Log($"Data saved to {path}");
    }

    public static T Load<T>(string filename)
    {
        string path = Path.Combine(Application.persistentDataPath, filename);
        if (!File.Exists(path))
        {
            Debug.LogWarning($"File not found at {path}");
            return default;
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<T>(json);
    }
}
