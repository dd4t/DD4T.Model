using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD4T.ContentModel;
using DD4T.Serialization;
using DD4T.ContentModel.Contracts.Serializing;
using System.Timers;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Collections.Generic;
using Microsoft.Web.Redis;

namespace DD4T.Model.Test
{
    [TestClass]
    public class GeneralTests
    {

        [TestMethod]
        public void FieldSet()
        {
            var fieldSet = new FieldSet();
            Assert.IsTrue(fieldSet != null);
        }

        [TestMethod]
        public void TestBinarySerializer()
        {
            var page = GenerateTestPage();
            BinarySerializer serializer = new BinarySerializer();
            var b = serializer.Serialize(page);
            var p2 = serializer.Deserialize(b) as IPage;
            Assert.IsTrue(page.Title == p2.Title);
            Assert.IsTrue(page.MetadataFields?.FirstOrDefault().Value.Value == p2.MetadataFields?.FirstOrDefault().Value.Value);

        }

        [TestMethod]
        public void TestBinarySerializerWithoutFields()
        {
            var page = GenerateTestPage();
            page.MetadataFields.Clear();
            BinarySerializer serializer = new BinarySerializer();
            var b = serializer.Serialize(page);
            var p2 = serializer.Deserialize(b) as IPage;
            Assert.IsTrue(page.Title == p2.Title);
            Assert.IsTrue(page.MetadataFields.Count() == p2.MetadataFields.Count());

        }


        [TestMethod]
        public void DictionaryTest()
        {
            var tester = new Tester();
            tester.FieldSet = new Dictionary<string, string>();
            tester.FieldSet.Add("testkey", "testvalue");
            BinarySerializer serializer = new BinarySerializer();
            var b = serializer.Serialize(tester);
            var t2 = serializer.Deserialize(b) as Tester;
            Assert.IsTrue(tester.FieldSet.Count() == t2.FieldSet.Count());
            Assert.IsTrue(tester.FieldSet["testkey"] == t2.FieldSet["testkey"]);

        }

        [Serializable]
        public class Tester
        {
            public Dictionary<string,string> FieldSet { get; set; }
        }

        internal static IPage GenerateTestPage()
        {
            Page p = new Page()
            {

                Id = "tcm:1-2-64",
                Title = "Testing 123",
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
                //ComponentPresentations = new System.Collections.Generic.List<ComponentPresentation>(),
                //Regions = new List<Region>
                //{
                //    new Region()
                //    {
                //        Name = "Region A",
                //        ComponentPresentations = new List<ComponentPresentation>
                //        {
                //            GenerateTestComponentPresentation() as ComponentPresentation
                //        }
                //    }
                //}
            };
            //List<Condition> conditions = new List<Condition>();
            //conditions.Add(new CustomerCharacteristicCondition()
            //{
            //    Name = "CustomersOnly",
            //    Negate = false,
            //    Operator = ConditionOperator.Equals,
            //    Value = "ID"
            //}
            //    );
            //var targetGroupConditions = new List<TargetGroupCondition>();
            //targetGroupConditions.Add(new TargetGroupCondition()
            //{
            //    TargetGroup = new TargetGroup() { Title = "CustomersOnly", Conditions = conditions, Description = "A test target group", Id = "tcm:2-1231-256", PublicationId = "tcm:0-2-1" },
            //    Negate = false
            //});
            p.MetadataFields = new FieldSet();
            ((IPage)p).MetadataFields.Add(new KeyValuePair<string, IField>("test", new Field()
            {
                Name = "test",
                Values = new List<string>() { "DefaultPage" }
            }
                ));
            ((IPage)p).MetadataFields.Add(new KeyValuePair<string, IField>("test2", new Field()
            {
                Name = "test2",
                Values = new List<string>() { "DefaultPage" }
            }
                ));
            ((IPage)p).MetadataFields.Add(new KeyValuePair<string, IField>("emb", new Field()
            {
                Name = "emb",
                EmbeddedSchema = new Schema() { Title = "myembschema", Id = "tcm:3-123-8" },
                EmbeddedValues = new List<FieldSet>()
                {
                    new FieldSet()
                    {
                    }
                }
            }
                ));
            p.MetadataFields["emb"].EmbeddedValues.FirstOrDefault().Add("embeddedField1", new Field()
            {
                Name = "embeddedField1", 
                Values = new List<string>() { "testing 123" }
            });

            //ComponentPresentation cp = (ComponentPresentation)GenerateTestComponentPresentation();
            //cp.TargetGroupConditions = targetGroupConditions;
            //p.ComponentPresentations.Add(cp);
            return p;
        }

        internal static IComponentPresentation GenerateTestComponentPresentation()
        {
            ComponentTemplate ct = new ComponentTemplate()
            {
                Id = "tcm:1-2",
                Title = "Component template 123",
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
                Component = (Component)GenerateTestComponent()
            };
            return cp;
        }
        internal static IComponent GenerateTestComponent()
        {
            IComponent c = new Component()
            {
                Id = "tcm:1-2",
                Title = "Component 123",
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
    }
}
