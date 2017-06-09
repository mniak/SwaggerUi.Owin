using Microsoft.Owin.FileSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SwaggerUi.Owin.IO
{
    internal class ReplaceableFileInfo : IFileInfo
    {
        private readonly IFileInfo inner;
        private readonly IDictionary<string, string> replaceDict;

        public ReplaceableFileInfo(IFileInfo inner, IDictionary<string, string> replaceDictionary)
        {
            this.inner = inner;
            this.replaceDict = replaceDictionary;
        }

        public long Length => GenerateReplacedContent().LongLength;

        public string PhysicalPath => inner.PhysicalPath;

        public string Name => inner.Name;

        public DateTime LastModified => inner.LastModified;

        public bool IsDirectory => inner.IsDirectory;

        byte[] _replacedContent = null;
        public Stream CreateReadStream()
        {
            var cont = GenerateReplacedContent();
            return new MemoryStream(cont);
        }

        private byte[] GenerateReplacedContent()
        {
            if (_replacedContent != null)
                return _replacedContent;

            byte[] bytes = null;
            using (var ms = new MemoryStream())
            {
                using (var stream = inner.CreateReadStream())
                {
                    stream.CopyTo(ms);
                }
                bytes = ms.ToArray();
            }
            var str = Encoding.UTF8.GetString(bytes);
            foreach (var kv in replaceDict)
            {
                str = str.Replace(kv.Key, kv.Value);
            }
            _replacedContent = Encoding.UTF8.GetBytes(str);
            return _replacedContent;
        }
    }
}
