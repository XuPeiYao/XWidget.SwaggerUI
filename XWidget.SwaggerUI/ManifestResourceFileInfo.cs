using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace XWidget.SwaggerUI {
    internal class ManifestResourceFileInfo : IFileInfo {
        public Stream Stream { get; set; }
        public bool Exists => true;

        public long Length => Stream.Length;

        public string PhysicalPath => null;

        public string Name { get; set; }

        public DateTimeOffset LastModified => DateTimeOffset.UtcNow;

        public bool IsDirectory => false;

        public Stream CreateReadStream() {
            Stream.Seek(0, SeekOrigin.Begin);
            return Stream;
        }
    }
}
