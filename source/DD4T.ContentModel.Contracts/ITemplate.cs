﻿using System;

namespace DD4T.ContentModel.Contracts
{
    public interface ITemplate : IRepositoryLocal
    {
        IOrganizationalItem Folder { get; }
        IFieldSet MetadataFields { get; }
        DateTime RevisionDate { get; }
    }
}
