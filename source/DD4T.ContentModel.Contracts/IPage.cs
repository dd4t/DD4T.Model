﻿    using System.Collections.Generic;
    using System;

namespace DD4T.ContentModel.Contracts
{
    public interface IPage : IRepositoryLocal, IViewable
    {
        [Obsolete]
        IList<ICategory> Categories { get; }
        IList<IComponentPresentation> ComponentPresentations { get; }
        DateTime RevisionDate { get; }
        string Filename { get; }
        IFieldSet MetadataFields { get; }
        IPageTemplate PageTemplate { get; }
        ISchema Schema { get; }
        IOrganizationalItem StructureGroup { get; }
        int Version { get; }
        DateTime LastPublishedDate { get; }
    }
}
