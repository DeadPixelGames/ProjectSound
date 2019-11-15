using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ObjectType  {
    Pentagrgamable, Splashable, Boomable, Bzzable, Popable
}

[CreateAssetMenu (fileName ="New Object", menuName = "InteractableObject")]
public class Object : ScriptableObject
{
    public ObjectType type;

    public UnityEvent onOnomatopeiaSound;
}
