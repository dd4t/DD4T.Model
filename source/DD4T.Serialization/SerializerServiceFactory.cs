using DD4T.ContentModel.Contracts.Serializing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DD4T.Serialization
{
    public class SerializerServiceFactory
    {
        private static Dictionary<Regex, string> serializersByPattern = new Dictionary<Regex, string>()
        {
            { new Regex ("^<"), "DD4T.Serialization.XmlSerializerService" }
        };
        private static string defaultSerializerServerType = "DD4T.Serialization.JSONSerializerService";

        public static ISerializerService FindSerializerServiceForContent(string content)
        {
            // first trim leading and trailing whitespace 
            string contentToCheck = content.Trim();

            // first, try to decompress (this will fail if the content is uncompressed, of course)
            bool isCompressed = false;
            try
            {
                content = Compressor.Decompress(content);
                isCompressed = true;
            }
            catch
            {

            }
            foreach (Regex re in serializersByPattern.Keys)
            {
                if (re.IsMatch(contentToCheck))
                {
                    return Initialize(serializersByPattern[re], isCompressed);
                }
            }
            return Initialize(defaultSerializerServerType, isCompressed);
        }

        private static BaseSerializerService Initialize(string typeName, bool isCompressed)
        {
            Type t = Type.GetType(typeName);
            BaseSerializerService service = Activator.CreateInstance(t) as BaseSerializerService;
            if (isCompressed)
            {
                service.SerializationProperties = new SerializationProperties() { CompressionEnabled = true };
            }
            return service;
        }
    }
}
