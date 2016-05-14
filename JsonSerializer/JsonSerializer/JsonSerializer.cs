using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;

namespace JsonSerializer
{
    static class JsonSerializer
    {
        public static void Serialize<T>(this T t, string filename)
        {
            Type objectType = t.GetType();

            SerializableAttribute serializable = new SerializableAttribute();

            if (!objectType.GetCustomAttributes(true).Contains(serializable))
            {
                throw new InvalidOperationException("Sorry, this object is not serializable");
            }

            PropertyInfo[] objectProperties = objectType.GetProperties();
            FieldInfo[] objectFields = objectType.GetFields();

            StringBuilder builder = new StringBuilder();
            StringWriter writer = new StringWriter(builder);

            using (JsonWriter jsonOutput = new JsonTextWriter(writer))
            {
                jsonOutput.Formatting = Formatting.Indented;

                jsonOutput.WriteStartObject();
                jsonOutput.WritePropertyName(objectType.Name);
                jsonOutput.WriteStartObject();

                foreach (var property in objectProperties)
                {
                    jsonOutput.WritePropertyName(property.Name);    
                    jsonOutput.WriteValue(property.GetValue(t, null));
                }

                NonSerializedAttribute nonSerialized = new NonSerializedAttribute();

                foreach (var field in objectFields)
                {
                    if (field.GetCustomAttributes(true).Contains(nonSerialized))
                        continue;
                    jsonOutput.WritePropertyName(field.Name);
                    jsonOutput.WriteValue(field.GetValue(t));
                }

                jsonOutput.WriteEndObject();
                jsonOutput.WriteEndObject();


                using (StreamWriter fileWriter = new StreamWriter(filename))
                {
                    fileWriter.WriteLine(builder);
                }
            }
        }
    }
}
