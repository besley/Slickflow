using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SpaServices;
using System;

namespace VueCliMiddleware
{
    /// <summary>
    /// Extension methods for enabling Vue development server middleware support.
    /// </summary>
    public static class VueCliMiddlewareExtensions
    {
        /// <summary>
        /// Handles requests by passing them through to an instance of the vue-cli server.
        /// This means you can always serve up-to-date CLI-built resources without having
        /// to run the vue-cli server manually.
        ///
        /// This feature should only be used in development. For production deployments, be
        /// sure not to enable the vue-cli server.
        /// </summary>
        /// <param name="spaBuilder">The <see cref="ISpaBuilder"/>.</param>
        /// <param name="npmScript">The name of the script in your package.json file that launches the vue-cli server.</param>
        /// <param name="port">Specify vue cli server port number. If &lt; 80, uses random port. </param>
        /// <param name="https">Specify vue cli server schema </param>
        /// <param name="runner">Specify the runner, Npm and Yarn are valid options. Yarn support is HIGHLY experimental.</param>
        /// <param name="regex">Specify a custom regex string to search for in the log indicating proxied server is running. VueCli: "running at", QuasarCli: "Compiled successfully"</param>
        /// <param name="forceKill"></param>
        /// <param name="wsl"></param>
        public static void UseVueCli(
            this ISpaBuilder spaBuilder,
            string npmScript = "serve",
            int port = 8080,
            bool https = false,
            ScriptRunnerType runner = ScriptRunnerType.Npm,
            string regex = VueCliMiddleware.DefaultRegex,
            bool forceKill = false,
            bool wsl = false)
        {
            if (spaBuilder == null)
            {
                throw new ArgumentNullException(nameof(spaBuilder));
            }

            var spaOptions = spaBuilder.Options;

            if (string.IsNullOrEmpty(spaOptions.SourcePath))
            {
                throw new InvalidOperationException($"To use {nameof(UseVueCli)}, you must supply a non-empty value for the {nameof(SpaOptions.SourcePath)} property of {nameof(SpaOptions)} when calling {nameof(SpaApplicationBuilderExtensions.UseSpa)}.");
            }

            VueCliMiddleware.Attach(spaBuilder, npmScript, port, https: https, runner: runner, regex: regex, forceKill: forceKill, wsl: wsl);
        }


        public static IEndpointConventionBuilder MapToVueCliProxy(
            this IEndpointRouteBuilder endpoints,
            string pattern,
            SpaOptions options,
            string npmScript = "serve",
            int port = 8080,
            bool https = false,
            ScriptRunnerType runner = ScriptRunnerType.Npm,
            string regex = VueCliMiddleware.DefaultRegex,
            bool forceKill = false,
            bool wsl = false)
        {
            if (pattern == null) { throw new ArgumentNullException(nameof(pattern)); }
            return endpoints.MapFallback(pattern, CreateProxyRequestDelegate(endpoints, options, npmScript, port, https, runner, regex, forceKill, wsl));
        }

        public static IEndpointConventionBuilder MapToVueCliProxy(
            this IEndpointRouteBuilder endpoints,
            string pattern,
            string sourcePath,
            string npmScript = "serve",
            int port = 8080,
            bool https = false,
            ScriptRunnerType runner = ScriptRunnerType.Npm,
            string regex = VueCliMiddleware.DefaultRegex,
            bool forceKill = false,
            bool wsl = false)
        {
            if (pattern == null) { throw new ArgumentNullException(nameof(pattern)); }
            if (sourcePath == null) { throw new ArgumentNullException(nameof(sourcePath)); }
            return endpoints.MapFallback(pattern, CreateProxyRequestDelegate(endpoints, new SpaOptions { SourcePath = sourcePath }, npmScript, port, https, runner, regex, forceKill, wsl));
        }

        public static IEndpointConventionBuilder MapToVueCliProxy(
            this IEndpointRouteBuilder endpoints,
            SpaOptions options,
            string npmScript = "serve",
            int port = 8080,
            bool https = false,
            ScriptRunnerType runner = ScriptRunnerType.Npm,
            string regex = VueCliMiddleware.DefaultRegex,
            bool forceKill = false,
            bool wsl = false)
        {
            return endpoints.MapFallback("{*path}", CreateProxyRequestDelegate(endpoints, options, npmScript, port, https, runner, regex, forceKill, wsl));
        }

        public static IEndpointConventionBuilder MapToVueCliProxy(
            this IEndpointRouteBuilder endpoints,
            string sourcePath,
            string npmScript = "serve",
            int port = 8080,
            bool https = false,
            ScriptRunnerType runner = ScriptRunnerType.Npm,
            string regex = VueCliMiddleware.DefaultRegex,
            bool forceKill = false,
            bool wsl = false)
        {
            if (sourcePath == null) { throw new ArgumentNullException(nameof(sourcePath)); }
            return endpoints.MapFallback("{*path}", CreateProxyRequestDelegate(endpoints, new SpaOptions { SourcePath = sourcePath }, npmScript, port, https, runner, regex, forceKill, wsl));
        }


        
        private static RequestDelegate CreateProxyRequestDelegate(
            IEndpointRouteBuilder endpoints,
            SpaOptions options,
            string npmScript = "serve",
            int port = 8080,
            bool https = false,
            ScriptRunnerType runner = ScriptRunnerType.Npm,
            string regex = VueCliMiddleware.DefaultRegex,
            bool forceKill = false,
            bool wsl = false)
        {
            // based on CreateRequestDelegate() https://github.com/aspnet/AspNetCore/blob/master/src/Middleware/StaticFiles/src/StaticFilesEndpointRouteBuilderExtensions.cs#L194

            if (endpoints == null) { throw new ArgumentNullException(nameof(endpoints)); }
            if (options == null) { throw new ArgumentNullException(nameof(options)); }
            //if (npmScript == null) { throw new ArgumentNullException(nameof(npmScript)); }

            var app = endpoints.CreateApplicationBuilder();
            app.Use(next => context =>
            {
                // Set endpoint to null so the SPA middleware will handle the request.
                context.SetEndpoint(null);
                return next(context);
            });

            app.UseSpa(opt =>
            {
                if (options != null)
                {
                    opt.Options.DefaultPage = options.DefaultPage;
                    opt.Options.DefaultPageStaticFileOptions = options.DefaultPageStaticFileOptions;
                    opt.Options.SourcePath = options.SourcePath;
                    opt.Options.StartupTimeout = options.StartupTimeout;
                }

                if (!string.IsNullOrWhiteSpace(npmScript))
                {
                    opt.UseVueCli(npmScript, port, https, runner, regex, forceKill, wsl);
                }
            });

            return app.Build();
        }
    }
}
