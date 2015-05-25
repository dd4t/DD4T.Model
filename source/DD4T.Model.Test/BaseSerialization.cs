using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DD4T.ContentModel;
using DD4T.Serialization;
using DD4T.ContentModel.Contracts.Serializing;

namespace DD4T.Model.Test
{
    public class BaseSerialization
    {
        internal static IComponent GenerateTestComponent()
        {
            IComponent c = new Component()
            {
                Id = "tcm:1-2",
                Title = "Test - component.title",
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
            return c;
        }
    }
}
