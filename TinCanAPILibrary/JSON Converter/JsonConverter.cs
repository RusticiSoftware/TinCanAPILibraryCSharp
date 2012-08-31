using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Converters;
using System.Collections;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace RusticiSoftware.TinCanAPILibrary.Json
{
    public class JsonConverter
    {
        private Hashtable typeConverters = new Hashtable();
        public void RegisterTypeConverter(Type objectType, JsonTypeConverter typeConverter)
        {
            typeConverters[objectType] = typeConverter;
        }

        public void RegisterDefaultConverter(Type objectType)
        {
            //Do nothing, this method is needed on Java side for concrete subclasses of 
            //interfaces and abstract classes registered with a custom converter
        }

        protected JsonSerializer getCustomSerializer()
        {
            JsonSerializer jsonSer = new JsonSerializer();

            //Don't serialize stuff that is null
            jsonSer.NullValueHandling = NullValueHandling.Ignore;

            //Don't serialize any properties that start with two underscores
            PrefixBasedPropertyExcludingContractResolver contractResolver = new PrefixBasedPropertyExcludingContractResolver("__");
            jsonSer.ContractResolver = contractResolver;
            
            //Register our custom type converters
            foreach (Type objectType in typeConverters.Keys) {
                jsonSer.Converters.Add(
                    new JsonTypeConverterWrapper(
                        (JsonTypeConverter)typeConverters[objectType], this));
            }

            //Add a built in converter to do the right thing with dates and enums
            jsonSer.Converters.Add(new IsoDateTimeConverter());
            jsonSer.Converters.Add(new StringEnumConverter());

            return jsonSer;
        }

        public String SerializeToJSON(Object obj)
        {
            return SerializeToJSON(obj, false);
        }

        public String SerializeToJSON(Object obj, bool prettyPrint)
        {
            JsonSerializer jsonSer = getCustomSerializer();
            using (StringWriter sw = new StringWriter())
            {
                JsonWriter writer = new CamelCasingJsonTextWriter(sw);

                if (prettyPrint)
                {
                    writer.Formatting = Formatting.Indented;
                }
                jsonSer.Serialize(writer, obj);

                return sw.ToString();
            }
        }

        public Object DeserializeJSON(String jsonString, Type objectType)
        {
            string typeName = objectType.Name;
            JsonSerializer jsonSer = getCustomSerializer();
            StringReader sr = new StringReader(jsonString);
            JsonReader jsonReader = new JsonTextReader(sr);
            Object obj = jsonSer.Deserialize(jsonReader, objectType);

            return PostDeserialize(obj);
        }

        public IDictionary DeserializeJSONToMap(String jsonString)
        {
            return (IDictionary)DeserializeJSON(jsonString, typeof(Hashtable));
        }

        protected virtual Object PostDeserialize(Object obj)
        {
            return obj;
        }

    }

    class JsonTypeConverterWrapper : Newtonsoft.Json.JsonConverter
    {
        private JsonTypeConverter typeConverter;
        private JsonConverter jsonConverter;

        public JsonTypeConverterWrapper(JsonTypeConverter typeConverter, JsonConverter jsonConverter)
        {
            this.typeConverter = typeConverter;
            this.jsonConverter = jsonConverter;
        }
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeConverter.GetTargetClass());
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            //HttpUtil.WriteToWebLog("INFO", "reader.TokenType = " + reader.TokenType.ToString() + ", reader.Value = " + reader.Value + ", reader.ValueType = " + reader.ValueType + ", objectType = " + objectType);

            String valueAsString = null;
            if (reader.TokenType.Equals(JsonToken.StartObject)) {
                valueAsString = jsonConverter.SerializeToJSON(serializer.Deserialize(reader, typeof(JObject)));
            }
            else if (reader.TokenType.Equals(JsonToken.StartArray)) {
                valueAsString = jsonConverter.SerializeToJSON(serializer.Deserialize(reader, typeof(JObject)));
            }
            else if (reader.TokenType.Equals(JsonToken.Null)) {
                return null;
            }
            else {
                valueAsString = reader.Value.ToString();
            }
            //HttpUtil.WriteToWebLog("INFO", "valueAsString = " + valueAsString);
            return typeConverter.Deserialize(valueAsString, jsonConverter);
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null) {
                writer.WriteNull();
                return;
            }
            value = typeConverter.Reduce(value, jsonConverter);
            if (value != null)
            {
                serializer.Serialize(writer, value);
            }
        }
    }

    class CamelCasingJsonTextWriter : JsonTextWriter
    {
        public CamelCasingJsonTextWriter(TextWriter tw) : base(tw)
        {
        }

        public override void WritePropertyName(string name)
        {
            String newName = name[0].ToString().ToLower() + name.Substring(1);
            base.WritePropertyName(newName);
        }
    }

    public class PrefixBasedPropertyExcludingContractResolver : DefaultContractResolver
    {
        private readonly String prefixToExclude;
        public PrefixBasedPropertyExcludingContractResolver(String prefixToExclude)
        {
            this.prefixToExclude = prefixToExclude;
        }

        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);

            // only serializer properties that start with the specified character
            /*properties = 
              properties.Where(p => p.PropertyName.StartsWith(_startingWithChar.ToString())).ToList();*/
            IList<JsonProperty> filtered = new List<JsonProperty>();
            foreach (JsonProperty prop in properties) {
                if (!prop.PropertyName.StartsWith(prefixToExclude)) {
                    filtered.Add(prop);
                }
            }
            return filtered;
        }
    }
}
