using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coffee.UIExtensions;

public class Tutorial : MonoBehaviour
{
    protected Unmask unmask;

    protected virtual void Start()
    {
        unmask = GetComponentInChildren<Unmask>();
    }
}
