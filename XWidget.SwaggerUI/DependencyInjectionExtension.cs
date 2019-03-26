using HtmlAgilityPack;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using XWidget.Web;

namespace XWidget.SwaggerUI {
    public static class DependencyInjectionExtension {
        private static HttpClient httpClient = new HttpClient();
        /// <summary>
        /// 使用SwaggerUI
        /// </summary>
        /// <param name="app">應用程式建構器</param>
        /// <param name="path">路徑</param>
        /// <param name="swaggerJsonPath">Swagger文件路徑</param>
        /// <param name="generateClient">是否顯示客戶端匯出資訊</param>
        /// <returns>應用程式建構器</returns>
        public static IApplicationBuilder UseSwaggerUI(
            this IApplicationBuilder app,
            PathString path,
            string swaggerJsonPath,
            bool generateClient = true) {

            return app.Map(path + "/export", app2 => {
                app2.Use(async (context, next) => {
                    var lang = (context.Request.PathBase + context.Request.Path).Value.Split('/').Last();
                    var jsonPath = context.Request.Query["path"];

                    var swaggerDoc = await httpClient.GetStringAsync(jsonPath);
                    swaggerDoc = $"{{\"spec\":{swaggerDoc}" + "}";

                    var payload = new StringContent(swaggerDoc, Encoding.UTF8, "application/json"); ;

                    var json = await (await httpClient.PostAsync($"https://generator.swagger.io/api/gen/clients/{lang}", payload)).ToJsonAsync();

                    context.Response.Redirect(json["link"].Value<string>());

                    return;
                });
            }).Map(path, app2 => {
                app2.UseHtmlHandler(async (context, html) => {
                    var jsonPath = swaggerJsonPath;

                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(html);
                    var body = doc.DocumentNode.SelectSingleNode("//body");

                    if (body == null) return html;

                    var script = doc.CreateElement("script");
                    script.InnerHtml = "var swaggerUrl = '" + jsonPath + "';var swaggerPath = '" + path.Value + "';";

                    body.AppendChild(script);

                    if (generateClient) {
                        var assembly = Assembly.GetExecutingAssembly();
                        var injectHtml = Encoding.UTF8.GetString(assembly.GetManifestResourceStream("XWidget.SwaggerUI.ResourceFiles.ClientExport.html").ToBytes());

                        body.InnerHtml += injectHtml;
                    }

                    return doc.DocumentNode.OuterHtml;
                });

                app2.Use(async (context, next) => {
                    await next();
                    if (context.Response.StatusCode != 200) {
                        context.Response.Redirect(path + "/index.html");
                    }
                });

                app2.UseStaticFiles(new StaticFileOptions() {
                    FileProvider = new ManifestResourceFileProvider()
                });
            });
        }
    }
}
