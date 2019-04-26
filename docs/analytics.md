# Adding events to our analytics system

Toggl uses the Firebase/AppCenter analytics for events.

When you want to track something new, this is the procedure:

**Preparation.** First consider how to properly structure the event. It is better to have one parameterized event than two events that do the same thing. Bear in mind that (for now) we are unable to correlate multiple parameters from one event, so do proper care to understand what information you want saved and design your event accordingly.

📚 Example: it's better to have one event `PageOpened` with parameter `page` than multiple events such as `MainLogOpened`, `EditViewOpened`, `SettingsViewOpened`, etc. However, if you need some additional data tracked related to one of those operation, for example, source from which the Edit View got opened, use an additional event such as `EditViewOpened(source)`.

**Steps**.

1. Check whether the same data is already tracked in any shape or form.
    1. If yes, consider using the existing event or find a good way to merge the two events
    1. If not, continue
1. Consider how many event parameters you need.
    1. If there are 4 or less primitive parameters, follow `IAnalyticsEvent` chapter
    1. If there are 5 or more parameters or the event requires any additional logic, follow `ITrackableEvent` chapter. This is also the way to do it if you need a platform specific event.

⚠ The examples in the following chapters are showing how to do the same thing different ways. But make sure you use the above decision tree to decide which one you need, so that we maintain consistency.

## `IAnalyticsEvent`

✔ 1. Add your event as a **getter-only** property to `Toggl.Core.Analytics.IAnalyticsService` interface. Make sure your generic type has proper number of arguments and use only primitive types for parameters.

> 📚 Example: `IAnalyticsEvent<string, int> MyEvent { get; }`

✔ 2. Add your event as a ``{ get; protected set; }`` property to `Toggl.Core.Analytics.BaseAnalyticsService` class. Make sure that you have both accessors because otherwise the application will fail to initialize the analytics service on startup.

> 📚 Example: `public IAnalyticsEvent<string, int> MyEvent { get; protected set; }`

✔ 3. Mark the property you've just created in `BaseAnalyticsService` with an `AnalyticsEvent` attribute. Add one string argument for each of the event parameters. These strings are the names for the event parameters as seen from the perspective of Firebase/AppCenter. 

⚠ Name your parameters wisely!

> 📚 Example: `[AnalyticsEvent("Source", "Value")] `

You are now able to track the event by using your property through `IAnalyticsService`:
```cs
analyticsService.MyEvent.Track("SourceExample", 14);
```

## `ITrackableEvent`
For more complex events, or events with additional logic/behavior, these are the steps:

✔ 1. Create a class and implement the `ITrackableEvent` interface.
* If the event is the same for both platforms, put it into the `Toggl.Core.Analytics` folder, otherwise find an appropriate platform-specific location for the class.

✔ 2. Implement the `EventName` property by providing the event name, as seen from Firebase/AppCenter perspective.

⚠ To keep things consistent with `IAnalyticsEvent` events, use `PascalCase` when creating names.

> 📚 Example: `public string EventName => "MyComplexEvent";`

✔ 3. Implement the `ToDictionary` property so that it returns a dictionary of arguments for the event.

In the following example, `source` and `value` variables are local variables of the event class.

```cs
public Dictionary<string, string> ToDictionary()
{
    return new Dictionary<string, string>
    {
        ["Source"] = source,
        ["Value"] = value
    };
}
```

ℹ You are able to use objects of this class as the regular .NET objects. Run their methods, perform calculations on them, do whatever you want. But at the moment you actually want to track it (current state of it, that is), you can do it like this:

```cs
var myEvent = new MyEvent("SourceExample", 14);
analyticsService.Track(myEvent);
```

Make sure your `ToDictionary` always returns a correct set of arguments/values.