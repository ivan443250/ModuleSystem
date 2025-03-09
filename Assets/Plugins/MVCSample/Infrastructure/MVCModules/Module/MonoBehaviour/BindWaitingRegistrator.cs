using MVCSample.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MVCSample.Infrastructure
{
    [RequireComponent(typeof(IModule))]
    public class BindWaitingRegistrator : MonoBehaviour
    {
        [SerializeField] private SerializableType[] _bindWaitings;

        public HashSet<Type> GetBindWaitings()
        {
            return new(_bindWaitings.Select(b => b.GetValue()));
        }
    }
}
