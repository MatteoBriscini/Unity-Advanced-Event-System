namespace Script.Events.EventsLayout
{
    /// <summary>
    /// An event channel that carries a boolean value.
    /// </summary>
    public class EventWithBool : BasicEventChannel
    {
        /// <summary>
        /// The current boolean value associated with this event.
        /// </summary>
        public bool boolValue;

        /// <summary>
        /// Updates the boolean value and triggers the event.
        /// </summary>
        /// <param name="value">The new boolean value.</param>
        public void UpdateValue(bool value)
        {
            boolValue = value;
            RaiseEvent();
        }
    }
}