using System;
using UnityEngine;

//TODO: example
namespace Script.Events.EventsLayout
{
    /// <summary>
    /// Demonstrates how to use the EventBroker system with an event that carries a boolean value.
    /// </summary>
    public class EventSystemUser : MonoBehaviour
    {
        [SerializeField] private bool b;
        [SerializeField] private Material mat1;
        [SerializeField] private Material mat2;

        /// <summary>
        /// Reference to the event channel that transmits a boolean value.
        /// </summary>
        EventWithBool eventWithBool;

        /// <summary>
        /// Initializes the event channel on Awake.
        /// </summary>
        private void Awake()
        {
            // This line attempts to retrieve an existing event channel with the name "exampleEvent".
            // - If the event already exists, it returns the existing instance.
            // - If the event does not exist, uses the input scriptable object as new instance of EventWithBool and registers it.
            eventWithBool = (EventWithBool)EventBroker.TryToAddEventChannel("exampleEvent",
                ScriptableObject.CreateInstance<EventWithBool>());


            // When the event is triggered, the provided callback function will be executed. \
            // The callback function does the following:
            // 1. It calls the `EventCallback` method with the current value of `boolValue` from the `eventWithBool` object.
            // 2. It logs a message indicating that the event callback was triggered, including the current object's hash code for identification purposes.
            eventWithBool.Subscribe(new Action(() =>
            {
                EventCallback(eventWithBool.boolValue);
                Debug.Log("trigger event callback of:" + this.GetHashCode());
            }));
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("on trigger enter of:" + this.GetHashCode());
            eventWithBool.UpdateValue(b);
        }

        private void EventCallback(bool b)
        {
            this.b = !b;
            Renderer renderer = GetComponent<Renderer>();
            if (b)
            {
                renderer.material = mat1;
            }
            else
            {
                renderer.material = mat2;
            }
        }
    }
}