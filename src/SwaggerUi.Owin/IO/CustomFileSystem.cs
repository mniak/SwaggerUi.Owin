using Microsoft.Owin.FileSystems;
using System.Collections.Generic;
using System.Reflection;

namespace SwaggerUi.Owin.IO
{
    internal class CustomFileSystem : IFileSystem
    {
        private EmbeddedResourceFileSystem inner;
        private readonly IDictionary<string, string> replaceDict;

        public CustomFileSystem(IDictionary<string, string> replaceDictionary)
        {
            this.inner = new EmbeddedResourceFileSystem(this.GetType().Assembly.GetName().Name + ".Content");
            this.replaceDict = replaceDictionary;
        }
        public bool TryGetDirectoryContents(string subpath, out IEnumerable<IFileInfo> contents)
        {
            var result = inner.TryGetDirectoryContents(subpath, out contents);
            return result;
        }

        public bool TryGetFileInfo(string subpath, out IFileInfo fileInfo)
        {
            var result = inner.TryGetFileInfo(subpath, out IFileInfo tempFileInfo);
            if (tempFileInfo == null)
                fileInfo = null;
            else
                fileInfo = new ReplaceableFileInfo(tempFileInfo, replaceDict);
            return result;
        }
    }
}
