using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FinanceApp.UI.Converters
{
    public class TransactionJsonConverter : JsonConverter<AbstractTransaction>
    {
        public override AbstractTransaction? ReadJson(JsonReader reader, Type objectType, AbstractTransaction? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jsonObj = JObject.Load(reader);
            var type = jsonObj["$type"]?.ToString();


            var safeSerializer = new JsonSerializer();
            foreach (var conv in serializer.Converters)
            {
                if (conv is not TransactionJsonConverter)
                    safeSerializer.Converters.Add(conv);
            }

            return type switch
            {
                "Income" => jsonObj.ToObject<IncomeTransaction>(safeSerializer),
                "Expense" => jsonObj.ToObject<ExpenseTransaction>(safeSerializer),
                _ => throw new InvalidOperationException($"Unknown transaction type: {type}")
            };
        }

        public override void WriteJson(JsonWriter writer, AbstractTransaction? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            var json = JObject.FromObject(value, serializer);

            string typeStr = value switch
            {
                IncomeTransaction => "Income",
                ExpenseTransaction => "Expense",
                _ => throw new InvalidOperationException("Unsupported transaction type")
            };

            json.AddFirst(new JProperty("$type", typeStr));
            json.WriteTo(writer);
        }
    }
}