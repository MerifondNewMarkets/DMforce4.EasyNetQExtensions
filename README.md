# DMforce4.EasyNetQExtensions

Sharing a message (type) between projects which use EasyNetQ should be easy, but it's not. You need to have a concrete implementation of a class lying around in a shared project which is then referenced by project A and B. The nightmare begings when you have to change something on that class implementation. You need to update all projects that use that class as well.

## Alternative Solutions

But what about [Polymorphic Publish and Subscribe](https://github.com/EasyNetQ/EasyNetQ/wiki/Polymorphic-Publish-and-Subscribe), you ask? Sounds great, doesn't work. When the message gets serialized and put on the bus the header type field still contains the message type from the project that published the message. At least when using the [Advanced API](https://github.com/EasyNetQ/EasyNetQ/wiki/The-Advanced-API) with generic interfaces that is.

## Why this project?

I haven't found a way around this limitation, hence I created this project to implement workarounds.

## How does it work?

Basically whatever object is given to the methods `PublishTypeAgnostic` or `PublishTypeAgnosticAsync` is serialized and immediately deserialized so it looses its type (Is there a better way to do this?). The type less `object` is then put on the bus (queue) where it can be fetched by `ConsumeTypeAgnostic`. The method signatures are copied from the implementation of EasyNetQ, so if you are familiar with the Advanced API this should be a no-brainer.

## Usage Eexample

```csharp
using DMforce4.EasyNetQExtensions

// [...]

var bus = RabbitHutch.CreateBus(configurationString).Advanced;
var exchange = _client.ExchangeDeclare("MyExchangeName", ExchangeType.Topic); // Use exchange type as you see fit

var msg = new MyMessage()
{
  MyProperty = "MyValue"
};

bus.PublishTypeAgnostic(exchange, "my.routing.key", false, msg);
```
