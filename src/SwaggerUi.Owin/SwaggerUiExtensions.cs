using Microsoft.Owin;
using Microsoft.Owin.StaticFiles;
using SwaggerUi.Owin.IO;
using System.Collections.Generic;

namespace Owin
{
    public static class SwaggerUiExtensions
    {
        private const string REPLACEMENT_KEY = "###***start_url***###";
        public static IAppBuilder UseSwaggerUi(this IAppBuilder app, string specificationPath)
        {
            var dictionary = new Dictionary<string, string>()
            {
                { REPLACEMENT_KEY, specificationPath },
            };
            var staticOptions = new FileServerOptions()
            {
                FileSystem = new CustomFileSystem(dictionary),
                RequestPath = new PathString(""),
            };
            app.UseFileServer(staticOptions);

            return app;
        }
    }
}
