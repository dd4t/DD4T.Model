﻿
namespace DD4T.ContentModel.Contracts
{
    public interface IMultimedia
    {
        string AltText { get; }
        string FileExtension { get; }
        string FileName { get; }
        int Height { get; }
        string MimeType { get; }
        long Size { get; }
        string Url { get; }
        int Width { get; }
    }
}
