using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace XWidget.SwaggerUI {
    /// <summary>
    /// 內嵌資源檔案提供者
    /// </summary>
    internal class ManifestResourceFileProvider : IFileProvider {
        static Dictionary<string, byte[]> _resourceFiles = new Dictionary<string, byte[]>();

        static ManifestResourceFileProvider() {
            var assembly = Assembly.GetExecutingAssembly();

            var files = assembly.GetManifestResourceNames();

            var prefix = "XWidget.SwaggerUI.SwaggerUI_Files.";

            foreach (var file in files) {
                _resourceFiles["/" + file.Replace(prefix, string.Empty).ToLower()] = assembly.GetManifestResourceStream(file).ToBytes();
            }
        }

        /// <summary>
        /// 取得目錄內容
        /// </summary>
        /// <param name="subpath"></param>
        /// <returns></returns>
        public IDirectoryContents GetDirectoryContents(string subpath) {
            if (!_resourceFiles.Any(x => x.Key.StartsWith(subpath.ToLower()))) {
                return new NotFoundDirectoryContents();
            }

            var files = _resourceFiles.Where(x => x.Key.StartsWith(subpath.ToLower()));

            return new ManifestResourceDirectoryContents() {
                Files = files.Select(x => GetFileInfo(x.Key)).ToList()
            };
        }

        /// <summary>
        /// 取得檔案資訊
        /// </summary>
        /// <param name="subpath"></param>
        /// <returns></returns>
        public IFileInfo GetFileInfo(string subpath) {
            if (!_resourceFiles.ContainsKey(subpath.ToLower())) {
                return new NotFoundFileInfo(Path.GetFileName(subpath));
            }
            return new ManifestResourceFileInfo() {
                Name = Path.GetFileName(subpath),
                Stream = _resourceFiles[subpath].ToStream()
            };
        }

        public IChangeToken Watch(string filter) {
            return NullChangeToken.Singleton;
        }
    }
}
