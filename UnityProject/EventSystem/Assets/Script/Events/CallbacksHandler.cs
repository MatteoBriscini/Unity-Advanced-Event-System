using System;
using System.Collections;
using System.Collections.Generic;

namespace Script.Events
{
    /// <summary>
    /// Handles execution of callback actions using Unity coroutines.
    /// </summary>
    public class CallbacksHandler : Singleton<CallbacksHandler>
    {
        /// <summary>
        /// Processes a list of callbacks, executing each as a coroutine.
        /// </summary>
        /// <param name="callbacks">List of callback actions to execute.</param>
        public void HandleCallback(List<Action> callbacks)
        {
            for (int i = 0; i < callbacks.Count; i++)
            {
                var callback = callbacks[i];
                StartCoroutine(Callback(callback));
            }
        }

        /// <summary>
        /// Executes callbacks sequentially in a coroutine.
        /// </summary>
        /// <param name="callbacks">List of callback actions.</param>
        private IEnumerator Callback(Action callback)
        {
            callback?.Invoke();
            yield return null;
        }
    }
}