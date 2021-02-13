using System;
using UnityEngine;
using UnityEngine.Events;
namespace MyMarmot.Tools
{
     class EventWrapper { }

    [Serializable]
    public class EventVector2 : UnityEvent<Vector2> { }
    [Serializable]
    public class EventVector3 : UnityEvent<Vector3> { }
    [Serializable]
    public class EventVector3Float : UnityEvent<Vector3,float> { }
    [Serializable]
    public class EventGameObj : UnityEvent<GameObject> { }
    [Serializable]
    public class EventTransform : UnityEvent<Transform> { }
    [Serializable]
    public class EventBool : UnityEvent<bool> { }
    [Serializable]
    public class EventString : UnityEvent<string> { }

}
