using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityUtils
{
    public static class ActionExtension
    {
        public static void RemoveAllEvents(this Action parent)
        {
            foreach (Delegate deg in parent.GetInvocationList())
            {
                parent -= (Action) deg;
            }
        }
    }
}
