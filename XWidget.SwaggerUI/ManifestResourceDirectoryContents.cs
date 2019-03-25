using Microsoft.Extensions.FileProviders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace XWidget.SwaggerUI {
    internal class ManifestResourceDirectoryContents : IDirectoryContents {
        public bool Exists => true;

        public List<IFileInfo> Files = new List<IFileInfo>();

        public IEnumerator<IFileInfo> GetEnumerator() {
            return Files.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return Files.GetEnumerator();
        }
    }
}
