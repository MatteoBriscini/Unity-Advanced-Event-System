# Unity Advanced Event System

Tags: `#videogame`, `#CS`, `#Unity`, `#URP`,  `#polimi`. <br>
>**Note:** this repo contains some script extracted from a bigger projects [BeliEve's](https://github.com/MatteoBriscini/BeliEves-videogameDesingAndProgramming-Polimi)

This repository contains the source code for building a complex Event Architecture. The Event System guarantees strong decoupling between components, enabling method invocations via message-based communication. <br> 
The proposed system is a hybrid solution combining [Unity's official Event Architecture](https://unity.com/how-to/architect-game-code-scriptable-objects#architect--for-events) (based on ScriptableObject) with a classic pub-sub system that utilizes a central broker.<br> <br>
In particular, the centralized broker is used solely to create and retrieve Event channels, while the ScriptableObject enables direct multicast communication between components, eliminating the need to pass through the broker:
- `EventBoker`**:** Provide standardized way to create/retrieve event channels directly from code.
- `BasicEventChannels`**:** Allow components to subscribe to event callbacks triggered when an event is raised.
<br><br>

> **note:** Each event callback is executed on an independent Coroutine to improve parallelism.

> **note:** `BasicEventChannels` can be extended to implement more complex logic.

## How to use?
 ### 0~ Preparation
 Place the `CallbacksHandler` MonoBehaviour somewhere in the scene.
 ### 1~ Channel Type Set Up
 Extend `BasicEventChannels` to customize the Event Channel based on your needs. <br> *example:*
    
```csharp 
/// <summary>
/// An event channel that carries a boolean value.
/// </summary>
public class EventWithBool : BasicEventChannel{
    /// <summary>
    /// The current boolean value associated with this event.
    /// </summary>
    public bool boolValue;

    /// <summary>
    /// Updates the boolean value and triggers the event.
    /// </summary>
    /// <param name="value">The new boolean value.</param>
    public void UpdateValue(bool value){
        boolValue = value;
        RaiseEvent();
    }
}
```

### 3~ Channel Creation
This is the only phase when we need to invoke the centralized `EventBoker`, The current implementation provide multicast options: <br>
- **AddEventChannel:** Simply create a new Event Channel for future usage. <br>
    Throw exception if the eventName is already registered. <br>
    *syntax:* `EventBroker.AddEventChannel(<eventName>, <BasicEventChannel>)`<br>
    *example:*    
    ```csharp 
    EventBroker.AddEventChannel("Name", ScriptableObject.CreateInstance<EventWithBool>());
    ```
- **GetEventChannel:** Retrieve the event channel from its name.<br>
    Throw exception if the eventName isn't registered. <br>
    *syntax:* `EventBroker.GetEventChannel(<eventName>)`<br>
    *example:*    
    ```csharp 
    EventWithBool event = (EventWithBool)EventBroker.AddEventChannel("Name");
    ```
- **TryToAddEventChannel:** Retrieve the event channel from its name if already registered, create a new channel if not.<br>
    *syntax:* `EventBroker.TryToAddEventChannel(<eventName>, <BasicEventChannel>)`<br>
    *example:*    
    ```csharp 
    eventWithBool = (EventWithBool)EventBroker.TryToAddEventChannel("exampleEvent",
        ScriptableObject.CreateInstance<EventWithBool>()
    );
    ```
> **Note:** From a single channel type multiple events with different names can be created.<br>

### 4~ Subscribe a callback
If you already have the reference to the event, you can subscribe a callback directly. <br>
*syntax:* `event.Subscribe(<Action>);` <br>
*example:* 
```csharp 
eventWithBool.Subscribe(new Action(() =>
{
    EventCallback(eventWithBool.boolValue);
    Debug.Log("trigger event callback of:" + this.GetHashCode());
}));
```
Alternatively, you can still subscribe a callback through the `EventBroker` using the event name. <br>
*syntax:* `EventBroker.SubsToEventChannel(<eventName>, <Action>);` <br>
*example:* 
```csharp 
EventBroker.SubsToEventChannel("name", new Action(() =>
{
    EventCallback(eventWithBool.boolValue);
    Debug.Log("trigger event callback of:" + this.GetHashCode());
}));
```