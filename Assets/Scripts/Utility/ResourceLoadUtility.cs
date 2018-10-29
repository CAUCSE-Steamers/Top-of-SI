using UnityEngine;
using System.Collections;

public static class ResourceLoadUtility
{
    public static Sprite LoadSprite(string spriteName)
    {
        return Resources.Load<Sprite>("Icons/" + spriteName);
    }
}
