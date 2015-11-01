using DD4T.ContentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.IO;
using DD4T.ContentModel.Contracts;

namespace DD4T.Serialization
{
    public class JSONSerializerService : BaseSerializerService
    {
        private JsonSerializer _serializer = null;
        public JsonSerializer Serializer
        {
            get
            {
                if (_serializer == null)
                {  
                    _serializer = new JsonSerializer();
                    _serializer.NullValueHandling = NullValueHandling.Ignore;
                    _serializer.Converters.Add(new FieldConverter());
                }

                return _serializer;
            }
        }

        public override string Serialize<T>(T input)
        {
            string result = string.Empty;
            using (StringWriter stringWriter = new StringWriter())
            {
                JsonWriter jsonWriter = new JsonTextWriter(stringWriter);
                Serializer.Serialize(jsonWriter, input);
                result = stringWriter.ToString();
                result = ((SerializationProperties)SerializationProperties).CompressionEnabled ? Compressor.Compress(result) : result;
            }
            return result;
        }

        public override T Deserialize<T>(string input) 
        {
            // NOTE: important exception situation!!
            // if the requested type is IComponentPresentation, there is a possiblity that the data 
            // provided to us actually contains a Component instead. In that case we need to add a 
            // dummy CT / CP around the Component and return that!

            if (((SerializationProperties)SerializationProperties).CompressionEnabled)
            {
                input = Compressor.Decompress(input);
            }

            using (var inputValueReader = new StringReader(input))
            {
                JsonTextReader reader = new JsonTextReader(inputValueReader);
                if (typeof(T).Name.Contains("ComponentPresentation") 
                    && !input.Contains("ComponentTemplate"))
                {
                    // handle the exception situation where we are asked to deserialize into a CP but the data is actually a Component
                    Component component = Serializer.Deserialize<Component>(reader);
                    IComponentPresentation componentPresentation = new ComponentPresentation()
                    {
                        Component = component,
                        ComponentTemplate = new ComponentTemplate()
                    };
                    return (T)componentPresentation;
                }
                return (T)Serializer.Deserialize<T>(reader);
            }
        }

        public override bool IsAvailable()
        {
            return Type.GetType("Newtonsoft.Json.JsonSerializer") != null;
        }
    }

    public class FieldConverter : CustomCreationConverter<IField>
    {
        public override IField Create(Type objectType)
        {
            return new Field();
        }
    }
}
