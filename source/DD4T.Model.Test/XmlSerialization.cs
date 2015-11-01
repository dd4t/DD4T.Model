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
        private static IPage testPage = null;
        private static string testComponentXml = null;
        private static string testComponentXmlCompressed = null;
        private static string testComponentPresentationXml = null;
        private static string testComponentPresentationXmlCompressed = null;
        private static string testPageXml = null;
        private static string testPageXmlCompressed = null;

        [ClassInitialize]
        public static void SetupTest(TestContext context)
        {
            testComponent = GenerateTestComponent();
            testComponentPresentation = GenerateTestComponentPresentation();
            testPage = GenerateTestPage();

            // Method GetService() is a (virtual) instance method now, so we need a temp instance.
            XmlSerialization testInstance = new XmlSerialization();

            testComponentXml = testInstance.GetService(false).Serialize<IComponent>(testComponent);
            testComponentXmlCompressed = testInstance.GetService(true).Serialize<IComponent>(testComponent);
            testComponentPresentationXml = testInstance.GetService(false).Serialize<IComponentPresentation>(testComponentPresentation);
            testComponentPresentationXmlCompressed = testInstance.GetService(true).Serialize<IComponentPresentation>(testComponentPresentation);
            testPageXml = testInstance.GetService(false).Serialize<IPage>(testPage);
            testPageXmlCompressed = testInstance.GetService(true).Serialize<IPage>(testPage);
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

        [TestMethod]
        public void SerializePageXml()
        {
            SerializeXml<Page>(false);
        }

        [TestMethod]
        public void DeserializePageXml()
        {
            DeserializeXml<Page>(false);
        }



        /// <summary>
        /// This method uses the XML representation of a page as published by DXA 1.0 (.NET)
        /// Use this test to validate compatibility
        /// </summary>
        [TestMethod]
        public void DeserializePageXmlDXA()
        {
            string test = "<Page>  <Id>tcm:4-310-64</Id>  <Title>010 All Articles</Title>  <Publication>    <Id>tcm:0-4-1</Id>    <Title>400 Example Site</Title>  </Publication>  <OwningPublication>    <Id>tcm:0-4-1</Id>    <Title>400 Example Site</Title>  </OwningPublication>  <RevisionDate>2015-08-04T16:37:41.913</RevisionDate>  <Filename>all-articles</Filename>  <LastPublishedDate>0001-01-01T00:00:00</LastPublishedDate>  <PageTemplate>    <Id>tcm:4-193-128</Id>    <Title>Content Page</Title>    <Publication>      <Id>tcm:0-4-1</Id>      <Title>400 Example Site</Title>    </Publication>    <FileExtension>html</FileExtension>    <RevisionDate>2015-08-04T16:39:41.683</RevisionDate>    <MetadataFields>      <item>        <key>          <string>includes</string>        </key>        <value>          <Field xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" FieldType=\"Text\">            <Name>includes</Name>            <Values>              <string>system/include/header</string>              <string>system/include/footer</string>              <string>system/include/content-tools</string>              <string>system/include/left-navigation</string>            </Values>            <NumericValues />            <DateTimeValues />            <LinkedComponentValues />            <EmbeddedValues />            <Keywords />          </Field>        </value>      </item>      <item>        <key>          <string>view</string>        </key>        <value>          <Field xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" FieldType=\"Text\">            <Name>view</Name>            <Values>              <string>GeneralPage</string>            </Values>            <NumericValues />            <DateTimeValues />            <LinkedComponentValues />            <EmbeddedValues />            <Keywords />          </Field>        </value>      </item>    </MetadataFields>    <Folder>      <Id>tcm:4-9-2</Id>      <Title>Templates</Title>      <PublicationId>tcm:0-4-1</PublicationId>    </Folder>  </PageTemplate>  <MetadataFields />  <ComponentPresentations>    <ComponentPresentation>      <Component>        <Id>tcm:4-272</Id>        <Title>All Articles Intro</Title>        <Publication>          <Id>tcm:0-4-1</Id>          <Title>400 Example Site</Title>        </Publication>        <OwningPublication>          <Id>tcm:0-4-1</Id>          <Title>400 Example Site</Title>        </OwningPublication>        <LastPublishedDate>0001-01-01T00:00:00</LastPublishedDate>        <RevisionDate>2015-07-27T14:41:33.937</RevisionDate>        <Schema>          <Id>tcm:4-82-8</Id>          <Title>Article</Title>          <Publication>            <Id>tcm:0-4-1</Id>            <Title>400 Example Site</Title>          </Publication>          <Folder>            <Id>tcm:4-17-2</Id>            <Title>Schemas</Title>            <PublicationId>tcm:0-4-1</PublicationId>          </Folder>          <RootElementName>Article</RootElementName>        </Schema>        <Fields>          <item>            <key>              <string>headline</string>            </key>            <value>              <Field xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" FieldType=\"Text\" XPath=\"tcm:Content/custom:Article/custom:headline\">                <Name>headline</Name>                <Values>                  <string>All Articles</string>                </Values>                <NumericValues />                <DateTimeValues />                <LinkedComponentValues />                <EmbeddedValues />                <Keywords />              </Field>            </value>          </item>          <item>            <key>              <string>image</string>            </key>            <value>              <Field xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" FieldType=\"MultiMediaLink\" XPath=\"tcm:Content/custom:Article/custom:image\">                <Name>image</Name>                <Values>                  <string>tcm:4-217</string>                </Values>                <NumericValues />                <DateTimeValues />                <LinkedComponentValues>                  <Component>                    <Id>tcm:4-217</Id>                    <Title>blueprint</Title>                    <Publication>                      <Id>tcm:0-4-1</Id>                      <Title>400 Example Site</Title>                    </Publication>                    <OwningPublication>                      <Id>tcm:0-4-1</Id>                      <Title>400 Example Site</Title>                    </OwningPublication>                    <LastPublishedDate>0001-01-01T00:00:00</LastPublishedDate>                    <RevisionDate>2015-07-27T14:41:23.187</RevisionDate>                    <Schema>                      <Id>tcm:4-81-8</Id>                      <Title>Image</Title>                      <Publication>                        <Id>tcm:0-4-1</Id>                        <Title>400 Example Site</Title>                      </Publication>                      <Folder>                        <Id>tcm:4-17-2</Id>                        <Title>Schemas</Title>                        <PublicationId>tcm:0-4-1</PublicationId>                      </Folder>                      <RootElementName>undefined</RootElementName>                    </Schema>                    <Fields />                    <MetadataFields />                    <ComponentType>Multimedia</ComponentType>                    <Multimedia>                      <Url>/Preview/media/blueprint_tcm4-217.jpg</Url>                      <MimeType>image/jpeg</MimeType>                      <FileName>blueprint.jpg</FileName>                      <FileExtension>jpg</FileExtension>                      <Size>1200817</Size>                      <Width>0</Width>                      <Height>0</Height>                    </Multimedia>                    <Folder>                      <Id>tcm:4-47-2</Id>                      <Title>Large</Title>                      <PublicationId>tcm:0-4-1</PublicationId>                    </Folder>                    <Categories />                    <Version>1</Version>                  </Component>                </LinkedComponentValues>                <EmbeddedValues />                <Keywords />              </Field>            </value>          </item>          <item>            <key>              <string>articleBody</string>            </key>            <value>              <Field xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" FieldType=\"Embedded\" XPath=\"tcm:Content/custom:Article/custom:articleBody\">                <Name>articleBody</Name>                <Values />                <NumericValues />                <DateTimeValues />                <LinkedComponentValues />                <EmbeddedValues>                  <FieldSet>                    <item>                      <key>                        <string>content</string>                      </key>                      <value>                        <Field FieldType=\"Xhtml\" XPath=\"tcm:Content/custom:Article/custom:articleBody[1]/custom:content\">                          <Name>content</Name>                          <Values>                            <string>&lt;span&gt;Sed dapibus tempus nunc, id sollicitudin ligula mattis in. Pellentesque quis purus lobortis, porttitor felis a, auctor felis. Phasellus posuere aliquet libero et vestibulum. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; In at felis vel risus facilisis ultrices tincidunt nec mauris. Praesent porttitor mi vitae lacus tristique, et hendrerit ante scelerisque. Phasellus sit amet venenatis lacus. Nunc vehicula ullamcorper pharetra. Proin hendrerit enim sed condimentum scelerisque. Maecenas accumsan orci id nisl congue consectetur. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque tristique quam tempus ligula iaculis malesuada. Quisque non lectus dui. Nunc luctus faucibus nulla quis pellentesque. Sed leo quam, auctor sit amet diam et, ultricies dignissim metus. Cras dignissim porttitor nisi sodales facilisis.&lt;/span&gt;</string>                          </Values>                          <NumericValues />                          <DateTimeValues />                          <LinkedComponentValues />                          <EmbeddedValues />                          <Keywords />                        </Field>                      </value>                    </item>                  </FieldSet>                </EmbeddedValues>                <EmbeddedSchema>                  <Id>tcm:4-80-8</Id>                  <Title>Paragraph</Title>                  <Publication>                    <Id>tcm:0-4-1</Id>                    <Title>400 Example Site</Title>                  </Publication>                  <Folder>                    <Id>tcm:4-18-2</Id>                    <Title>Embedded</Title>                    <PublicationId>tcm:0-4-1</PublicationId>                  </Folder>                  <RootElementName>Paragraph</RootElementName>                </EmbeddedSchema>                <Keywords />              </Field>            </value>          </item>        </Fields>        <MetadataFields>          <item>            <key>              <string>standardMeta</string>            </key>            <value>              <Field xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" FieldType=\"Embedded\" XPath=\"tcm:Metadata/custom:Metadata/custom:standardMeta\">                <Name>standardMeta</Name>                <Values />                <NumericValues />                <DateTimeValues />                <LinkedComponentValues />                <EmbeddedValues>                  <FieldSet>                    <item>                      <key>                        <string>description</string>                      </key>                      <value>                        <Field FieldType=\"MultiLineText\" XPath=\"tcm:Metadata/custom:Metadata/custom:standardMeta[1]/custom:description\">                          <Name>description</Name>                          <Values>                            <string>Cras vel justo semper, bibendum odio in, fringilla elit. Cras at nunc fringilla, porttitor mauris eget, pharetra purus. Sed elementum consectetur massa id auctor.</string>                          </Values>                          <NumericValues />                          <DateTimeValues />                          <LinkedComponentValues />                          <EmbeddedValues />                          <Keywords />                        </Field>                      </value>                    </item>                    <item>                      <key>                        <string>name</string>                      </key>                      <value>                        <Field FieldType=\"Text\" XPath=\"tcm:Metadata/custom:Metadata/custom:standardMeta[1]/custom:name\">                          <Name>name</Name>                          <Values>                            <string>View all articles</string>                          </Values>                          <NumericValues />                          <DateTimeValues />                          <LinkedComponentValues />                          <EmbeddedValues />                          <Keywords />                        </Field>                      </value>                    </item>                    <item>                      <key>                        <string>introText</string>                      </key>                      <value>                        <Field FieldType=\"Text\" XPath=\"tcm:Metadata/custom:Metadata/custom:standardMeta[1]/custom:introText\">                          <Name>introText</Name>                          <Values>                            <string>See the latest content published</string>                          </Values>                          <NumericValues />                          <DateTimeValues />                          <LinkedComponentValues />                          <EmbeddedValues />                          <Keywords />                        </Field>                      </value>                    </item>                  </FieldSet>                </EmbeddedValues>                <EmbeddedSchema>                  <Id>tcm:4-79-8</Id>                  <Title>Standard Metadata</Title>                  <Publication>                    <Id>tcm:0-4-1</Id>                    <Title>400 Example Site</Title>                  </Publication>                  <Folder>                    <Id>tcm:4-18-2</Id>                    <Title>Embedded</Title>                    <PublicationId>tcm:0-4-1</PublicationId>                  </Folder>                  <RootElementName>StandardMetadata</RootElementName>                </EmbeddedSchema>                <Keywords />              </Field>            </value>          </item>        </MetadataFields>        <ComponentType>Normal</ComponentType>        <Folder>          <Id>tcm:4-54-2</Id>          <Title>Articles</Title>          <PublicationId>tcm:0-4-1</PublicationId>        </Folder>        <Categories />        <Version>1</Version>      </Component>      <ComponentTemplate>        <Id>tcm:4-191-32</Id>        <Title>Article</Title>        <Publication>          <Id>tcm:0-4-1</Id>          <Title>400 Example Site</Title>        </Publication>        <OutputFormat>HTML Fragment</OutputFormat>        <RevisionDate>2015-07-27T14:41:18.743</RevisionDate>        <MetadataFields>          <item>            <key>              <string>view</string>            </key>            <value>              <Field xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" FieldType=\"Text\">                <Name>view</Name>                <Values>                  <string>Article</string>                </Values>                <NumericValues />                <DateTimeValues />                <LinkedComponentValues />                <EmbeddedValues />                <Keywords />              </Field>            </value>          </item>        </MetadataFields>        <Folder>          <Id>tcm:4-9-2</Id>          <Title>Templates</Title>          <PublicationId>tcm:0-4-1</PublicationId>        </Folder>      </ComponentTemplate>      <IsDynamic>false</IsDynamic>      <Conditions>        <Condition d5p1:type=\"CustomerCharacteristicCondition\" xmlns:d5p1=\"http://www.w3.org/2001/XMLSchema-instance\">          <Negate>false</Negate>          <Name>is_customer</Name>          <Operator>StringEquals</Operator>          <Value xmlns:q1=\"http://www.w3.org/2001/XMLSchema\" d5p1:type=\"q1:string\">true</Value>        </Condition>      </Conditions>    </ComponentPresentation>    <ComponentPresentation>      <Component>        <Id>tcm:4-308</Id>        <Title>All Articles Index</Title>        <Publication>          <Id>tcm:0-4-1</Id>          <Title>400 Example Site</Title>        </Publication>        <OwningPublication>          <Id>tcm:0-4-1</Id>          <Title>400 Example Site</Title>        </OwningPublication>        <LastPublishedDate>0001-01-01T00:00:00</LastPublishedDate>        <RevisionDate>2015-07-27T14:41:39.297</RevisionDate>        <Schema>          <Id>tcm:4-151-8</Id>          <Title>Content Query</Title>          <Publication>            <Id>tcm:0-4-1</Id>            <Title>400 Example Site</Title>          </Publication>          <Folder>            <Id>tcm:4-17-2</Id>            <Title>Schemas</Title>            <PublicationId>tcm:0-4-1</PublicationId>          </Folder>          <RootElementName>ContentQuery</RootElementName>        </Schema>        <Fields>          <item>            <key>              <string>headline</string>            </key>            <value>              <Field xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" FieldType=\"Text\" XPath=\"tcm:Content/custom:ContentQuery/custom:headline\">                <Name>headline</Name>                <Values>                  <string>All Articles</string>                </Values>                <NumericValues />                <DateTimeValues />                <LinkedComponentValues />                <EmbeddedValues />                <Keywords />              </Field>            </value>          </item>        </Fields>        <MetadataFields>          <item>            <key>              <string>contentType</string>            </key>            <value>              <Field xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" FieldType=\"Keyword\" CategoryName=\"Content Type\" CategoryId=\"tcm:4-33-512\" XPath=\"tcm:Metadata/custom:Metadata/custom:contentType\">                <Name>contentType</Name>                <Values>                  <string>Article</string>                </Values>                <NumericValues />                <DateTimeValues />                <LinkedComponentValues />                <EmbeddedValues />                <Keywords>                  <Keyword Description=\"\" Key=\"core.article\" TaxonomyId=\"tcm:4-33-512\" Path=\"\\Content Type\\Article\">                    <Id>tcm:4-204-1024</Id>                    <Title>Article</Title>                    <MetadataFields />                  </Keyword>                </Keywords>              </Field>            </value>          </item>          <item>            <key>              <string>pageSize</string>            </key>            <value>              <Field xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" FieldType=\"Number\" XPath=\"tcm:Metadata/custom:Metadata/custom:pageSize\">                <Name>pageSize</Name>                <Values>                  <string>5</string>                </Values>                <NumericValues>                  <double>5</double>                </NumericValues>                <DateTimeValues />                <LinkedComponentValues />                <EmbeddedValues />                <Keywords />              </Field>            </value>          </item>          <item>            <key>              <string>sort</string>            </key>            <value>              <Field xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" FieldType=\"Keyword\" CategoryName=\"List Sort Type\" CategoryId=\"tcm:4-27-512\" XPath=\"tcm:Metadata/custom:Metadata/custom:sort\">                <Name>sort</Name>                <Values>                  <string>Last Published Date</string>                </Values>                <NumericValues />                <DateTimeValues />                <LinkedComponentValues />                <EmbeddedValues />                <Keywords>                  <Keyword Description=\"\" Key=\"pubdate\" TaxonomyId=\"tcm:4-27-512\" Path=\"\\List Sort Type\\Last Published Date\">                    <Id>tcm:4-171-1024</Id>                    <Title>Last Published Date</Title>                    <MetadataFields />                  </Keyword>                </Keywords>              </Field>            </value>          </item>        </MetadataFields>        <ComponentType>Normal</ComponentType>        <Folder>          <Id>tcm:4-54-2</Id>          <Title>Articles</Title>          <PublicationId>tcm:0-4-1</PublicationId>        </Folder>        <Categories>          <Category>            <Id>tcm:4-33-512</Id>            <Title>Content Type</Title>            <Keywords>              <Keyword Description=\"\" Key=\"core.article\" TaxonomyId=\"tcm:4-33-512\" Path=\"\\Content Type\\Article\">                <Id>tcm:4-204-1024</Id>                <Title>Article</Title>                <MetadataFields />              </Keyword>            </Keywords>          </Category>          <Category>            <Id>tcm:4-27-512</Id>            <Title>List Sort Type</Title>            <Keywords>              <Keyword Description=\"\" Key=\"pubdate\" TaxonomyId=\"tcm:4-27-512\" Path=\"\\List Sort Type\\Last Published Date\">                <Id>tcm:4-171-1024</Id>                <Title>Last Published Date</Title>                <MetadataFields />              </Keyword>            </Keywords>          </Category>        </Categories>        <Version>1</Version>      </Component>      <ComponentTemplate>        <Id>tcm:4-173-32</Id>        <Title>Paged List</Title>        <Publication>          <Id>tcm:0-4-1</Id>          <Title>400 Example Site</Title>        </Publication>        <OutputFormat>HTML Fragment</OutputFormat>        <RevisionDate>2015-07-27T14:41:16.927</RevisionDate>        <MetadataFields>          <item>            <key>              <string>controller</string>            </key>            <value>              <Field xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" FieldType=\"Text\">                <Name>controller</Name>                <Values>                  <string>List</string>                </Values>                <NumericValues />                <DateTimeValues />                <LinkedComponentValues />                <EmbeddedValues />                <Keywords />              </Field>            </value>          </item>          <item>            <key>              <string>action</string>            </key>            <value>              <Field xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" FieldType=\"Text\">                <Name>action</Name>                <Values>                  <string>List</string>                </Values>                <NumericValues />                <DateTimeValues />                <LinkedComponentValues />                <EmbeddedValues />                <Keywords />              </Field>            </value>          </item>          <item>            <key>              <string>view</string>            </key>            <value>              <Field xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" FieldType=\"Text\">                <Name>view</Name>                <Values>                  <string>PagedList</string>                </Values>                <NumericValues />                <DateTimeValues />                <LinkedComponentValues />                <EmbeddedValues />                <Keywords />              </Field>            </value>          </item>          <item>            <key>              <string>regionView</string>            </key>            <value>              <Field xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" FieldType=\"Text\">                <Name>regionView</Name>                <Values>                  <string>Main</string>                </Values>                <NumericValues />                <DateTimeValues />                <LinkedComponentValues />                <EmbeddedValues />                <Keywords />              </Field>            </value>          </item>        </MetadataFields>        <Folder>          <Id>tcm:4-9-2</Id>          <Title>Templates</Title>          <PublicationId>tcm:0-4-1</PublicationId>        </Folder>      </ComponentTemplate>      <IsDynamic>false</IsDynamic>      <Conditions />    </ComponentPresentation>  </ComponentPresentations>  <StructureGroup>    <Id>tcm:4-56-4</Id>    <Title>010 Articles</Title>    <PublicationId>tcm:0-4-1</PublicationId>  </StructureGroup>  <Categories />  <Version>2</Version></Page>";
            ISerializerService service = GetService(false);
            Page page = service.Deserialize<Page>(test);
            Assert.IsNotNull(page.ComponentPresentations[0].Conditions);
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
            if (typeof(T) == typeof(Page))
            {
                return (T)testPage;
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
            if (typeof(T) == typeof(Page))
            {
                return isCompressed ? testPageXmlCompressed : testPageXml;
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
                    if (typeof(T) == typeof(ComponentPresentation))
                    {
                        Assert.IsTrue(_serializedString.Contains(GetTestTitle<Component>()));
                    }
                    else
                    {
                        Assert.IsTrue(_serializedString.Contains(GetTestTitle<T>()));
                    }
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
                    Assert.IsTrue(((IComponent)c).Title == GetTestTitle<Component>());
                }
                else if (c is ComponentPresentation)
                {
                    Assert.IsTrue(((IComponentPresentation)c).Component.Title == GetTestTitle<Component>());
                }
                else if (c is Page)
                {
                    Assert.IsTrue(((IPage)c).Title == GetTestTitle<Page>());
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
                    Assert.IsTrue(((IComponent)c).Title == GetTestTitle<Component>());
                }
                else if (c is ComponentPresentation)
                {
                    Assert.IsTrue(((IComponentPresentation)c).Component.Title == GetTestTitle<Component>());
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
