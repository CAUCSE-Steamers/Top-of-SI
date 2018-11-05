using UnityEngine;
using System.Collections;

public static class ResourceLoadUtility
{
    public static Sprite LoadIcon(string spriteName)
    {
        return Resources.Load<Sprite>("Icons/" + spriteName);
    }

    public static Sprite LoadPortrait(string spriteName)
    {
        return Resources.Load<Sprite>("Portraits/" + spriteName);
    }

    public static AudioClip LoadEffectClip(string effectName)
    {
        return Resources.Load<AudioClip>("Sounds/" + effectName);
    }
}
