using DD4T.ContentModel.Contracts;
using DD4T.ContentModel.Contracts.Serializing;

namespace DD4T.Serialization
{
    public abstract class BaseSerializerService : ISerializerService
    {
        public abstract string Serialize<T>(T input) where T : IModel;

        public abstract T Deserialize<T>(string input) where T : IModel;

        public abstract bool IsAvailable();



        private SerializationProperties _serializationProperties = null;
        public SerializationProperties SerializationProperties
        {
            get
            {
                if (_serializationProperties == null)
                    _serializationProperties = new Serialization.SerializationProperties() { CompressionEnabled = false };
                return _serializationProperties;
            }
            set
            {
                _serializationProperties = value;
            }
        }
    }
}
