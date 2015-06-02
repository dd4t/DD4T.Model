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
    public class XmlSerialization : BaseSerialization
    {
        private static int loop = 100;
        private static System.Collections.Generic.Dictionary<bool, ISerializerService> services = new System.Collections.Generic.Dictionary<bool, ISerializerService>();

        private static IComponent testComponent = null;
        private static IComponentPresentation testComponentPresentation = null;
        private static string testComponentXml = null;
        private static string testComponentXmlCompressed = null;
        private static string testComponentPresentationXml = null;
        private static string testComponentPresentationXmlCompressed = null;

        [ClassInitialize]
        public static void SetupTest(TestContext context)
        {
            testComponent = GenerateTestComponent();
            testComponentPresentation = GenerateTestComponentPresentation();
            testComponentXml = GetService(false).Serialize<IComponent>(testComponent);
            testComponentXmlCompressed = GetService(true).Serialize<IComponent>(testComponent);
            testComponentPresentationXml = GetService(false).Serialize<IComponentPresentation>(testComponentPresentation);
            testComponentPresentationXmlCompressed = GetService(true).Serialize<IComponentPresentation>(testComponentPresentation);
        }


        [TestMethod]
        public void SerializeComponentXml()
        {
            SerializeXml<Component>(false);
        }

        [TestMethod]
        public void DeserializeComponentXml()
        {
            DeserializeXml<Component>(false);
        }

        [TestMethod]
        public void DeserializeComponentAutodetectedXml()
        {
            DeserializeAutodetectedXml<Component>(false);
        }

        [TestMethod]
        public void SerializeAndCompressComponentXml()
        {
            SerializeXml<Component>(true);
        }

        [TestMethod]
        public void DeserializeAndDecompressComponentXml()
        {
            DeserializeXml<Component>(true);
        }

        [TestMethod]
        public void DeserializeAndDecompressComponentAutodetectedXml()
        {
            DeserializeAutodetectedXml<Component>(true);
        }

        [TestMethod]
        public void SerializeComponentPresentationXml()
        {
            SerializeXml<ComponentPresentation>(false);
        }

        [TestMethod]
        public void DeserializeComponentPresentationXml()
        {
            DeserializeXml<ComponentPresentation>(false);
        }

        [TestMethod]
        public void DeserializeComponentPresentationAutodetectedXml()
        {
            DeserializeAutodetectedXml<ComponentPresentation>(false);
        }

        [TestMethod]
        public void SerializeAndCompressComponentPresentationXml()
        {
            SerializeXml<ComponentPresentation>(true);
        }

        [TestMethod]
        public void DeserializeAndDecompressComponentPresentationXml()
        {
            DeserializeXml<ComponentPresentation>(true);
        }

        [TestMethod]
        public void DeserializeAndDecompressComponentPresentationAutodetectedXml()
        {
            DeserializeAutodetectedXml<ComponentPresentation>(true);
        }

        [TestMethod]
        public void DeserializeComponentPresentationFromComponentXml()
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
                return isCompressed ? testComponentXmlCompressed : testComponentXml;
            }
            if (typeof(T) == typeof(ComponentPresentation))
            {
                return isCompressed ? testComponentPresentationXmlCompressed : testComponentPresentationXml;
            }
            return String.Empty;
        }

        private void SerializeXml<T>(bool isCompressed) where T : IModel
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

        private void DeserializeXml<T>(bool isCompressed) where T : IModel
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

        private void DeserializeAutodetectedXml<T>(bool isCompressed) where T : IModel
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            ISerializerService service = SerializerServiceFactory.FindSerializerServiceForContent(GetTestString<T>(isCompressed));
            System.Diagnostics.Trace.WriteLine(string.Format("[{0}] {1}", stopwatch.Elapsed, "detected service"));
            Assert.IsInstanceOfType(service, typeof(XmlSerializerService), "Incorrect Service detected");
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
                    s = new XmlSerializerService()
                    {
                        SerializationProperties = new SerializationProperties()
                        {
                            CompressionEnabled = true
                        }
                    };

                }
                else
                {
                    s = new XmlSerializerService();
                }
                services.Add(compressionEnabled, s);
            }
            return services[compressionEnabled];
        }
        private string SerializeXml(IComponent c)
        {
            return GetService(false).Serialize<IComponent>(c);
        }
        private string SerializeAndCompressXml(IComponent c)
        {
            return GetService(true).Serialize<IComponent>(c);
        }
    }
}
