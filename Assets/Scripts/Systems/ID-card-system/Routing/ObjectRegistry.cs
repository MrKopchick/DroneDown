using System.Collections.Generic;

public static class ObjectRegistry
{
    private static Dictionary<string, List<IDCardBase>> objectRegistry = new Dictionary<string, List<IDCardBase>>();

    public static void RegisterObject(IDCardBase obj)
    {
        if (obj == null) return;

        string objectType = obj.ObjectType;
        if (!objectRegistry.ContainsKey(objectType))
        {
            objectRegistry[objectType] = new List<IDCardBase>();
        }

        objectRegistry[objectType].Add(obj);
    }

    public static void DeregisterObject(IDCardBase obj)
    {
        if (obj == null) return;

        string objectType = obj.ObjectType;
        if (objectRegistry.ContainsKey(objectType))
        {
            objectRegistry[objectType].Remove(obj);
        }
    }

    public static int GetObjectCount(string type)
    {
        return objectRegistry.ContainsKey(type) ? objectRegistry[type].Count : 0;
    }

    public static List<IDCardBase> GetObjects(string type)
    {
        return objectRegistry.ContainsKey(type) ? new List<IDCardBase>(objectRegistry[type]) : new List<IDCardBase>();
    }
}