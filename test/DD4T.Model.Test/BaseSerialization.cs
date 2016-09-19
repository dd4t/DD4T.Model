using System;
using DD4T.ContentModel;
using DD4T.ContentModel.Contracts.Serializing;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DD4T.Model.Test
{
    public abstract class BaseSerialization
    {
        public static string GetTestTitle<T>() where T : IModel
        {
            if (typeof(T) == typeof(Component))
            {
                return "Test - component.title";
            }
            if (typeof(T) == typeof(ComponentTemplate))
            {
                return "Test - componentTemplate.title";
            }
            if (typeof(T) == typeof(Page))
            {
                return "Test - page.title";
            }
            throw new Exception("unexpected type " + typeof(T).Name);
        }
        internal static IComponentPresentation GenerateTestComponentPresentation()
        {
            ComponentTemplate ct = new ComponentTemplate()
            {
                Id = "tcm:1-2",
                Title = GetTestTitle<ComponentTemplate>(),
                Folder = new OrganizationalItem()
                {
                    Id = "tcm:1-2-2",
                    Title = "Test - folder.title"
                },
                Publication = new Publication()
                {
                    Id = "tcm:0-2-1",
                    Title = "Test - publication.title"
                },
                OwningPublication = new Publication()
                {
                    Id = "tcm:0-2-1",
                    Title = "Test - owningpublication.title"
                }
            };
            IComponentPresentation cp = new ComponentPresentation()
            {
                ComponentTemplate = ct,
                Component = (Component) GenerateTestComponent()
            };
            return cp;
        }
        internal static IComponent GenerateTestComponent()
        {
            IComponent c = new Component()
            {
                Id = "tcm:1-2",
                Title = GetTestTitle<Component>(),
                Folder = new OrganizationalItem()
                {
                    Id = "tcm:1-2-2",
                    Title = "Test - folder.title"
                },
                Publication = new Publication()
                {
                    Id = "tcm:0-2-1",
                    Title = "Test - publication.title"
                },
                OwningPublication = new Publication()
                {
                    Id = "tcm:0-2-1",
                    Title = "Test - owningpublication.title"
                },
                Fields = new FieldSet() 
            };

            c.Fields.Add(new KeyValuePair<string, IField>("test", new Field()
                {
                    Name = "test",
                    KeywordValues = new List<Keyword>() { new Keyword()
                    {
                        Title = "My keyword",
                        Key = "My key",
                        Description = "My description",
                        Id = "tcm:1-2-1024"
                    }
                }
                }
                ));
            return c;
        }

        internal static IPage GenerateTestPage()
        {
            Page p = new Page()
            {
                
                Id = "tcm:1-2-64",
                Title = GetTestTitle<Page>(),
                StructureGroup = new OrganizationalItem()
                {
                    Id = "tcm:1-2-2",
                    Title = "Test - structuregroup.title"
                },
                Publication = new Publication()
                {
                    Id = "tcm:0-2-1",
                    Title = "Test - publication.title"
                },
                OwningPublication = new Publication()
                {
                    Id = "tcm:0-2-1",
                    Title = "Test - owningpublication.title"
                },
                ComponentPresentations = new System.Collections.Generic.List<ComponentPresentation>()
            };
            List<Condition> conditions = new List<Condition>();
            conditions.Add(new CustomerCharacteristicCondition()
            {
                Name = "CustomersOnly",
                Negate = false,
                Operator = ConditionOperator.Equals,
                Value = "ID"
            }
                );
            ComponentPresentation cp = (ComponentPresentation) GenerateTestComponentPresentation();
            cp.Conditions = conditions;
            p.ComponentPresentations.Add(cp);
            return p;
        }

        protected static void SetTestExtensionData(IModel model)
        {
            ContentModel.Model modelImpl = (ContentModel.Model) model;
            modelImpl.AddExtensionProperty("test1", "testProperty1a", new [] { "this", "is", "a", "test" });
            modelImpl.AddExtensionProperty("test1", "testProperty1b", 3.1415);
            modelImpl.AddExtensionProperty("test2", "testProperty2a", new DateTime(1970, 12, 16));
            modelImpl.AddExtensionProperty("test3", "dummyProperty", null); // This should not do anything

            Assert.False(modelImpl.ExtensionData.ContainsKey("test3"), "Adding a null value should not do anything.");
        }

        protected abstract ISerializerService GetService(bool compressionEnabled);


        protected T SerializeDeserializeModel<T>(IModel inputModel) where T: ContentModel.Model
        {
            ISerializerService serializer = GetService(compressionEnabled: false);

            string serializedData = serializer.Serialize(inputModel);
            //Assert.NotNull(serializedData, "Serialized data");
            Assert.NotNull(serializedData);
            //Console.WriteLine(serializedData);

            T deserializedModel = serializer.Deserialize<T>(serializedData);
            //Assert.NotNull(deserializedModel, "Deserialized Model");
            Assert.NotNull(deserializedModel);

            return deserializedModel;
        }


        protected void AssertEqualFieldSets(IFieldSet expected, IFieldSet actual)
        {
            Assert.True(actual.Keys.SequenceEqual(expected.Keys), "FieldSets have different Keys.");

            foreach (IField field in expected.Values)
            {
                IField deserializedField = actual[field.Name];
                Assert.Equal(field.FieldType, deserializedField.FieldType);
                //Assert.AreEqual(field.FieldType, deserializedField.FieldType, "FieldType");
                switch (field.FieldType)
                {
                    case FieldType.Number:
                        //Assert.NotNull(deserializedField.NumericValues, "NumericValues");
                        Assert.NotNull(deserializedField.NumericValues);
                        Assert.True(deserializedField.NumericValues.SequenceEqual(field.NumericValues), "NumericValues");
                        break;

                    case FieldType.Date:
                        //Assert.NotNull(deserializedField.DateTimeValues, "DateTimeValues");
                        Assert.NotNull(deserializedField.DateTimeValues);
                        Assert.True(deserializedField.DateTimeValues.SequenceEqual(field.DateTimeValues), "DateTimeValues");
                        break;

                    case FieldType.Embedded:
                        //Assert.NotNull(deserializedField.EmbeddedValues, "EmbeddedValues");
                        Assert.NotNull(deserializedField.EmbeddedValues);
                        //Assert.AreEqual(deserializedField.EmbeddedValues.Count, field.EmbeddedValues.Count, "EmbeddedValues");
                        Assert.Equal(deserializedField.EmbeddedValues.Count, field.EmbeddedValues.Count);
                        break;

                    default:
                        //Assert.NotNull(deserializedField.Values, "Values");
                        Assert.NotNull(deserializedField.Values);
                        Assert.True(deserializedField.Values.SequenceEqual(field.Values), "Values");
                        break;
                }
            }
        }


        [Fact]
        public void SerializeDeserializeComponentWithExtensionData()
        {
            IComponent testComponent = GenerateTestComponent();
            SetTestExtensionData(testComponent);

            IComponent deserializedComponent = SerializeDeserializeModel<Component>(testComponent);

            //Assert.NotNull(deserializedComponent.ExtensionData, "ExtensionData");
            Assert.NotNull(deserializedComponent.ExtensionData);
            //Assert.AreEqual(testComponent.ExtensionData.Count, deserializedComponent.ExtensionData.Count, "#ExtensionData");
            Assert.Equal(testComponent.ExtensionData.Count, deserializedComponent.ExtensionData.Count);
            Assert.True(deserializedComponent.ExtensionData.Keys.SequenceEqual(testComponent.ExtensionData.Keys), "ExtensionData.Keys");

            foreach (KeyValuePair<string, IFieldSet> extensionDataSection in testComponent.ExtensionData)
            {
                IFieldSet deserializedFieldSet = deserializedComponent.ExtensionData[extensionDataSection.Key];
                AssertEqualFieldSets(extensionDataSection.Value, deserializedFieldSet);
            }
        }

        [Fact]
        public void SerializeDeserializeComponentWithEclId()
        {
            Component testComponent = (Component)GenerateTestComponent();
            testComponent.EclId = "ecl:666-911";

            IComponent deserializedComponent = SerializeDeserializeModel<Component>(testComponent);

            //Assert.AreEqual(testComponent.EclId, deserializedComponent.EclId);
            Assert.Equal(testComponent.EclId, deserializedComponent.EclId);

        }

    }
}
