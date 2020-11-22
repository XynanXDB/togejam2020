using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Library.Delegate
{
    public delegate void VoidSignature();
    public delegate void OneParamSignature<T>(T value);
}