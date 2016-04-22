using System;
using DD4T.ContentModel;
using DD4T.Serialization;
using DD4T.ContentModel.Contracts.Serializing;
using Xunit;
using System.Diagnostics;

namespace DD4T.Model.Test
{

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


        //public static void SetupTest(TestContext context)
        //{
        //    testComponent = GenerateTestComponent();
        //    testComponentPresentation = GenerateTestComponentPresentation();

        //    // Method GetService() is a (virtual) instance method now, so we need a temp instance.
        //    JsonSerialization testInstance = new JsonSerialization();

        //    testComponentJson = testInstance.GetService(false).Serialize<IComponent>(testComponent);
        //    testComponentJsonCompressed = testInstance.GetService(true).Serialize<IComponent>(testComponent);
        //    testComponentPresentationJson = testInstance.GetService(false).Serialize<IComponentPresentation>(testComponentPresentation);
        //    testComponentPresentationJsonCompressed = testInstance.GetService(true).Serialize<IComponentPresentation>(testComponentPresentation);
        //}

        static JsonSerialization()
        {
            testComponent = GenerateTestComponent();
            testComponentPresentation = GenerateTestComponentPresentation();

            // Method GetService() is a (virtual) instance method now, so we need a temp instance.
            JsonSerialization testInstance = new JsonSerialization();

            testComponentJson = testInstance.GetService(false).Serialize<IComponent>(testComponent);
            testComponentJsonCompressed = testInstance.GetService(true).Serialize<IComponent>(testComponent);
            testComponentPresentationJson = testInstance.GetService(false).Serialize<IComponentPresentation>(testComponentPresentation);
            testComponentPresentationJsonCompressed = testInstance.GetService(true).Serialize<IComponentPresentation>(testComponentPresentation);
        }

        [Fact]
        public void SerializeComponentJson()
        {

            SerializeJson<Component>(false);
        }

        [Fact]
        public void DeserializeComponentJson()
        {
            DeserializeJson<Component>(false);
        }

        [Fact]
        public void DeserializeComponentAutodetectedJson()
        {
            DeserializeAutodetectedJson<Component>(false);
        }

        [Fact]
        public void SerializeAndCompressComponentJson()
        {
            SerializeJson<Component>(true);
        }

        [Fact]
        public void DeserializeAndDecompressComponentJson()
        {
            DeserializeJson<Component>(true);
        }

        [Fact]
        public void DeserializeAndDecompressComponentAutodetectedJson()
        {
            DeserializeAutodetectedJson<Component>(true);
        }

        [Fact]
        public void SerializeComponentPresentationJson()
        {
            SerializeJson<ComponentPresentation>(false);
        }

        [Fact]
        public void DeserializeComponentPresentationJson()
        {
            DeserializeJson<ComponentPresentation>(false);
        }

        [Fact]
        public void DeserializeComponentPresentationAutodetectedJson()
        {
            DeserializeAutodetectedJson<ComponentPresentation>(false);
        }

        [Fact]
        public void SerializeAndCompressComponentPresentationJson()
        {
            SerializeJson<ComponentPresentation>(true);
        }

        [Fact]
        public void DeserializeAndDecompressComponentPresentationJson()
        {
            DeserializeJson<ComponentPresentation>(true);
        }

        [Fact]
        public void DeserializeAndDecompressComponentPresentationAutodetectedJson()
        {
            DeserializeAutodetectedJson<ComponentPresentation>(true);
        }

        [Fact]
        public void DeserializeComponentPresentationFromComponentJson()
        {
            ISerializerService service = GetService(false);

            for (int i = 0; i < loop; i++)
            {
                ComponentPresentation cp = service.Deserialize<ComponentPresentation>(GetTestString<Component>(false));
                Assert.NotNull(cp);
                Assert.True(cp.Component.Title == "Test - component.title");
            }
        }


        [Fact]
        public void DuplicateKeywordsIssue()
        {
            Component c = GetTestModel<Component>();
            int nrOfKeywords = c.Fields["test"].KeywordValues.Count;
            ISerializerService service = GetService(false);
            string s = service.Serialize<Component>(c);
            Component c2 = service.Deserialize<Component>(s);
            string s2 = service.Serialize<Component>(c2);
            Component c3 = service.Deserialize<Component>(s2);
            Assert.True(c3.Fields["test"].KeywordValues.Count == nrOfKeywords);
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

                //Assert.NotNull(model, "error retrieving test model");
                Assert.NotNull(model);
                string _serializedString = service.Serialize<T>(model);
                Assert.NotNull(_serializedString);
                if (!isCompressed)
                {
                    Assert.True(_serializedString.Contains("Test - component.title"));
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
                Assert.NotNull(c);
                if (c is Component)
                {
                    Assert.True(((IComponent)c).Title == "Test - component.title");
                }
                else if (c is ComponentPresentation)
                {
                    Assert.True(((IComponentPresentation)c).Component.Title == "Test - component.title");
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
            //Assert.InstanceOfType(service, typeof(JSONSerializerService), "Incorrect Service detected");
            Assert.IsType<JSONSerializerService>(service);
            for (int i = 0; i < loop; i++)
            {
                T c = service.Deserialize<T>(GetTestString<T>(isCompressed));
                Assert.NotNull(c);
                if (c is Component)
                {
                    Assert.True(((IComponent)c).Title == "Test - component.title");
                }
                else if (c is ComponentPresentation)
                {
                    Assert.True(((IComponentPresentation)c).Component.Title == "Test - component.title");
                }
            }
            System.Diagnostics.Trace.WriteLine(string.Format("[{0}] deserialized {1} objects of type {2}", stopwatch.Elapsed, loop, typeof(T).Name));
            stopwatch.Stop();
        }
        
        protected override ISerializerService GetService(bool compressionEnabled)
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
