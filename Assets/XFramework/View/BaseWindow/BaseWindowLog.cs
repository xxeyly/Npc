using System.Runtime.CompilerServices;
using UnityEngine;

namespace XFramework
{
    partial class BaseWindow
    {
        protected void Log(string message)
        {
            if (isLog)
            {
                Debug.Log(message);
            }
        }

        protected void LogError(object message)
        {
            if (isLog)
            {
                Debug.LogError(message);
            }
        }
    }
}