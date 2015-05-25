using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD4T.ContentModel;
using DD4T.Serialization;
using DD4T.ContentModel.Contracts.Serializing;

namespace DD4T.Model.Test
{
    [TestClass]
    public class XmlSerialization : BaseSerialization
    {  
        private static int loop = 50;

        [TestMethod]
        public void SerializeXml()
        {
            IComponent c = GenerateTestComponent();
            ISerializerService service = new XmlSerializerService();
            for (int i = 0; i < loop; i++)
            {
                string _serializedString = service.Serialize<IComponent>(c);
                Assert.IsNotNull(_serializedString);
            }
        }

        private string SerializeXml(IComponent c)
        {
            ISerializerService service = new XmlSerializerService();
            return service.Serialize<IComponent>(c);
        }

        [TestMethod]
        public void DeserializeXml()
        {
            IComponent c = GenerateTestComponent();
            //string serializedString = SerializeXml(c);
            //ISerializerService service = new XmlSerializerService();
            //for (int i = 0; i < loop; i++)
            //{
            //    c = service.Deserialize<IComponent>(serializedString);
            //    Assert.IsNotNull(c);
            //}
        }

        [TestMethod]
        public void DeserializeAutodetectedXml()
        {
            IComponent c = GenerateTestComponent();
            string serializedString = SerializeXml(c);
            ISerializerService service = SerializerServiceFactory.FindSerializerServiceForContent(serializedString);
            Assert.IsInstanceOfType(service, typeof(XmlSerializerService), "Incorrect Service detected");
            for (int i = 0; i < loop; i++)
            {
                c = service.Deserialize<IComponent>(serializedString);
                Assert.IsNotNull(c);
            }
        }

        [TestMethod]
        public void SerializeAndCompressXml()
        {
            IComponent c = GenerateTestComponent();
            ISerializerService service = new XmlSerializerService()
            {
                SerializationProperties = new SerializationProperties()
                {
                    CompressionEnabled = true
                }
            };
            for (int i = 0; i < loop; i++)
            {
                string serializedString = service.Serialize<IComponent>(c);
                Assert.IsNotNull(serializedString);
            }
        }

        private string SerializeAndCompressXml(IComponent c)
        {
            ISerializerService service = new XmlSerializerService()
            {
                SerializationProperties = new SerializationProperties()
                {
                    CompressionEnabled = true
                }
            };
            return service.Serialize<IComponent>(c);
        }

        [TestMethod]
        public void DeserializeAndDecompressXml()
        {
            IComponent c = GenerateTestComponent();
            string serializedString = SerializeAndCompressXml(c);
            ISerializerService service = new XmlSerializerService()
            {
                SerializationProperties = new SerializationProperties()
                {
                    CompressionEnabled = true
                }
            };
            for (int i = 0; i < loop; i++)
            {
                c = service.Deserialize<IComponent>(serializedString);
                Assert.IsNotNull(c);
            }
        }

        [TestMethod]
        public void DeserializeAndDecompressAutodetectedXml()
        {
            IComponent c = GenerateTestComponent();
            string serializedString = SerializeAndCompressXml(c);
            ISerializerService service = SerializerServiceFactory.FindSerializerServiceForContent(serializedString);
            Assert.IsInstanceOfType(service, typeof(XmlSerializerService), "Incorrect Service detected");
            Assert.IsTrue(((BaseSerializerService)service).SerializationProperties.CompressionEnabled, "compression is not enabled on the serializerservice");
            for (int i = 0; i < loop; i++)
            {
                c = service.Deserialize<IComponent>(serializedString);
                Assert.IsNotNull(c);
            }
        }
    }
}
