using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MannequinManager : Singleton<MannequinManager>
{
    public UnityEvent<float> onDamaged;
    public UnityEvent<int> onKilled;
}
