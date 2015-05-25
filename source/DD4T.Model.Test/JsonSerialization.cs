using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD4T.ContentModel;
using DD4T.Serialization;
using DD4T.ContentModel.Contracts.Serializing;
using System.Timers;
using System.Diagnostics;

namespace DD4T.Model.Test
{
    [TestClass]
    public class JsonSerialization : BaseSerialization
    {
        private static int loop = 100;
        private static System.Collections.Generic.Dictionary<bool, ISerializerService> services = new System.Collections.Generic.Dictionary<bool, ISerializerService>();

        private static IComponent testComponent = null;
        private static string testJson = null;
        private static string testJsonCompressed = null;

        [ClassInitialize]
        public static void SetupTest(TestContext context)
        {
            testComponent = GenerateTestComponent();
            testJson = GetService(false).Serialize<IComponent>(testComponent);
            testJsonCompressed = GetService(true).Serialize<IComponent>(testComponent);
        }


        [TestMethod]
        public void SerializeJson()
        {
            ISerializerService service = GetService(false);
            for (int i = 0; i < loop; i++)
            {
                string _serializedString = service.Serialize<IComponent>(testComponent);
                Assert.IsNotNull(_serializedString);
            }
        }


        [TestMethod]
        public void DeserializeJson()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            ISerializerService service = GetService(false);
            System.Diagnostics.Trace.WriteLine(string.Format("[{0}] {1}", stopwatch.Elapsed, "instantiated service"));
         
            for (int i = 0; i < loop; i++)
            {
                IComponent c = service.Deserialize<Component>(testJson);
                Assert.IsNotNull(c);
            }
            System.Diagnostics.Trace.WriteLine(string.Format("[{0}] deserialized {1} objects", stopwatch.Elapsed, loop));
            stopwatch.Stop();
        }

        [TestMethod]
        public void DeserializeAutodetectedJson()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            ISerializerService service = SerializerServiceFactory.FindSerializerServiceForContent(testJson);
            System.Diagnostics.Trace.WriteLine(string.Format("[{0}] {1}", stopwatch.Elapsed, "detected service"));
            Assert.IsInstanceOfType(service, typeof(JSONSerializerService), "Incorrect Service detected");
            for (int i = 0; i < loop; i++)
            {
                IComponent c = service.Deserialize<Component>(testJson);
                Assert.IsNotNull(c);
            }
            System.Diagnostics.Trace.WriteLine(string.Format("[{0}] deserialized {1} objects", stopwatch.Elapsed, loop));
            stopwatch.Stop();
        }

        [TestMethod]
        public void SerializeAndCompressJson()
        {
            ISerializerService service = GetService(true);
            for (int i = 0; i < loop; i++)
            {
                string serializedString = service.Serialize<IComponent>(testComponent);
                Assert.IsNotNull(serializedString);
            }
        }

        [TestMethod]
        public void DeserializeAndDecompressJson()
        {           
            ISerializerService service = GetService(true);
            for (int i = 0; i < loop; i++)
            {
                IComponent c = service.Deserialize<Component>(testJsonCompressed);
                Assert.IsNotNull(c);
            }
        }

        [TestMethod]
        public void DeserializeAndDecompressAutodetectedJson()
        {
            ISerializerService service = SerializerServiceFactory.FindSerializerServiceForContent(testJsonCompressed);
            Assert.IsInstanceOfType(service, typeof(JSONSerializerService), "Incorrect Service detected");
            Assert.IsTrue(((BaseSerializerService)service).SerializationProperties.CompressionEnabled, "compression is not enabled on the serializerservice");
            for (int i = 0; i < loop; i++)
            {
                IComponent c = service.Deserialize<Component>(testJsonCompressed);
                Assert.IsNotNull(c);
            }
        }

        
        private static ISerializerService GetService(bool compressionEnabled)
        {
            if (!services.ContainsKey(compressionEnabled))
            {
                ISerializerService s;
                if (compressionEnabled)
                {
                    s = new JSONSerializerService()
                    {
                        SerializationProperties = new SerializationProperties()
                        {
                            CompressionEnabled = true
                        }
                    };

                }
                else
                {
                    s = new JSONSerializerService();
                }
                services.Add(compressionEnabled, s);
            }
            return services[compressionEnabled];
        }
        private string SerializeJson(IComponent c)
        {
            return GetService(false).Serialize<IComponent>(c);
        }
        private string SerializeAndCompressJson(IComponent c)
        {
            return GetService(true).Serialize<IComponent>(c);
        }



    }
}
