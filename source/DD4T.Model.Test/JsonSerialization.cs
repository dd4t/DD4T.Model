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
        private static IComponentPresentation testComponentPresentation = null;
        private static string testComponentJson = null;
        private static string testComponentJsonCompressed = null;
        private static string testComponentPresentationJson = null;
        private static string testComponentPresentationJsonCompressed = null;

        [ClassInitialize]
        public static void SetupTest(TestContext context)
        {
            testComponent = GenerateTestComponent();
            testComponentPresentation = GenerateTestComponentPresentation();
            testComponentJson = GetService(false).Serialize<IComponent>(testComponent);
            testComponentJsonCompressed = GetService(true).Serialize<IComponent>(testComponent);
            testComponentPresentationJson = GetService(false).Serialize<IComponentPresentation>(testComponentPresentation);
            testComponentPresentationJsonCompressed = GetService(true).Serialize<IComponentPresentation>(testComponentPresentation);
        }


        [TestMethod]
        public void SerializeComponentJson()
        {
            SerializeJson<Component>(false);
        }

        [TestMethod]
        public void DeserializeComponentJson()
        {
            DeserializeJson<Component>(false);
        }

        [TestMethod]
        public void DeserializeComponentAutodetectedJson()
        {
            DeserializeAutodetectedJson<Component>(false);
        }

        [TestMethod]
        public void SerializeAndCompressComponentJson()
        {
            SerializeJson<Component>(true);
        }

        [TestMethod]
        public void DeserializeAndDecompressComponentJson()
        {
            DeserializeJson<Component>(true);
        }

        [TestMethod]
        public void DeserializeAndDecompressComponentAutodetectedJson()
        {
            DeserializeAutodetectedJson<Component>(true);
        }

        [TestMethod]
        public void SerializeComponentPresentationJson()
        {
            SerializeJson<ComponentPresentation>(false);
        }

        [TestMethod]
        public void DeserializeComponentPresentationJson()
        {
            DeserializeJson<ComponentPresentation>(false);
        }

        [TestMethod]
        public void DeserializeComponentPresentationAutodetectedJson()
        {
            DeserializeAutodetectedJson<ComponentPresentation>(false);
        }

        [TestMethod]
        public void SerializeAndCompressComponentPresentationJson()
        {
            SerializeJson<ComponentPresentation>(true);
        }

        [TestMethod]
        public void DeserializeAndDecompressComponentPresentationJson()
        {
            DeserializeJson<ComponentPresentation>(true);
        }

        [TestMethod]
        public void DeserializeAndDecompressComponentPresentationAutodetectedJson()
        {
            DeserializeAutodetectedJson<ComponentPresentation>(true);
        }

        [TestMethod]
        public void DeserializeComponentPresentationFromComponentJson()
        {
            ISerializerService service = GetService(false);
 
            for (int i = 0; i < loop; i++)
            {
                ComponentPresentation cp = service.Deserialize<ComponentPresentation>(GetTestString<Component>(false));
                Assert.IsNotNull(cp);
                Assert.IsTrue(cp.Component.Title == "Test - component.title");
            }
         }


        private T GetTestModel<T>() where T : IModel
        {
            if (typeof(T) == typeof(Component))
            {
                return (T)testComponent;
            }
            if (typeof(T) == typeof(ComponentPresentation))
            {
                return (T)testComponentPresentation;
            }
            return default(T);
        }

        private string GetTestString<T>(bool isCompressed) where T : IModel
        {
            if (typeof(T) == typeof(Component))
            {
                return isCompressed ? testComponentJsonCompressed : testComponentJson;
            }
            if (typeof(T) == typeof(ComponentPresentation))
            {
                return isCompressed ? testComponentPresentationJsonCompressed : testComponentPresentationJson;
            }
            return String.Empty;
        }

        private void SerializeJson<T>(bool isCompressed) where T : IModel
        {
            ISerializerService service = GetService(isCompressed);
            for (int i = 0; i < loop; i++)
            {
                T model = GetTestModel<T>();
                Assert.IsNotNull(model, "error retrieving test model");
                string _serializedString = service.Serialize<T>(model);
                Assert.IsNotNull(_serializedString);
                if (!isCompressed)
                {
                    Assert.IsTrue(_serializedString.Contains("Test - component.title"));
                }
            }
        }

        private void DeserializeJson<T>(bool isCompressed) where T : IModel
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            ISerializerService service = GetService(isCompressed);
            System.Diagnostics.Trace.WriteLine(string.Format("[{0}] {1}", stopwatch.Elapsed, "instantiated service"));

            for (int i = 0; i < loop; i++)
            {
                T c = service.Deserialize<T>(GetTestString<T>(isCompressed));
                Assert.IsNotNull(c);
                if (c is Component)
                {
                    Assert.IsTrue(((IComponent)c).Title == "Test - component.title");
                }
                else if (c is ComponentPresentation)
                {
                    Assert.IsTrue(((IComponentPresentation)c).Component.Title == "Test - component.title");
                }

            }
            System.Diagnostics.Trace.WriteLine(string.Format("[{0}] deserialized {1} objects of type {2}", stopwatch.Elapsed, loop, typeof(T).Name));
            stopwatch.Stop();
        }

        private void DeserializeAutodetectedJson<T>(bool isCompressed) where T : IModel
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            ISerializerService service = SerializerServiceFactory.FindSerializerServiceForContent(GetTestString<T>(isCompressed));
            System.Diagnostics.Trace.WriteLine(string.Format("[{0}] {1}", stopwatch.Elapsed, "detected service"));
            Assert.IsInstanceOfType(service, typeof(JSONSerializerService), "Incorrect Service detected");
            for (int i = 0; i < loop; i++)
            {
                T c = service.Deserialize<T>(GetTestString<T>(isCompressed));
                Assert.IsNotNull(c);
                if (c is Component)
                {
                    Assert.IsTrue(((IComponent)c).Title == "Test - component.title");
                }
                else if (c is ComponentPresentation)
                {
                    Assert.IsTrue(((IComponentPresentation)c).Component.Title == "Test - component.title");
                }
            }
            System.Diagnostics.Trace.WriteLine(string.Format("[{0}] deserialized {1} objects of type {2}", stopwatch.Elapsed, loop, typeof(T).Name));
            stopwatch.Stop();
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
