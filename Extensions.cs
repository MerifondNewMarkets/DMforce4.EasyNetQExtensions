using EasyNetQ;
using EasyNetQ.Topology;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace DMforce4.EasyNetQExtensions
{
    public static class Extensions
    {
        #region Publishers
        public static void PublishTypeAgnostic<T>(this IAdvancedBus @this, IExchange exchange, string routingKey, bool mandatory, T message) where T : class
        {
            var obj = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(message));
            @this.Publish(exchange, routingKey, mandatory, new Message<object>(obj));
        }

        public static void PublishTypeAgnostic<T>(this IAdvancedBus @this, IExchange exchange, string routingKey, bool mandatory, MessageProperties properties, T message) where T : class
        {
            var body = JsonConvert.SerializeObject(message);
            @this.Publish(exchange, routingKey, mandatory, properties, Encoding.UTF8.GetBytes(body));
        }

        public static Task PublishTypeAgnosticAsync<T>(this IAdvancedBus @this, IExchange exchange, string routingKey, bool mandatory, T message) where T : class
        {
            var obj = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(message));
            return @this.PublishAsync(exchange, routingKey, mandatory, new Message<object>(obj));
        }

        public static void PublishTypeAgnosticAsync<T>(this IAdvancedBus @this, IExchange exchange, string routingKey, bool mandatory, MessageProperties properties, T message) where T : class
        {
            var body = JsonConvert.SerializeObject(message);
            @this.Publish(exchange, routingKey, mandatory, properties, Encoding.UTF8.GetBytes(body));
        }
        #endregion

        #region Consumers
        public static void ConsumeTypeAgnostic<T>(this IAdvancedBus @this, IQueue queue, Action<T, MessageReceivedInfo> action) where T : class
        {
            @this.Consume<object>(queue, (body, properties) => {
                var msg = JsonConvert.DeserializeObject<T>(body.Body.ToString());
                action(msg, properties);
            });
        }
        #endregion
    }
}
