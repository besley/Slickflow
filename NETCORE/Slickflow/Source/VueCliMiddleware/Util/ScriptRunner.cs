// Copyright (c) .NET Foundation Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Original Source: https://github.com/aspnet/JavaScriptServices

using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;

namespace VueCliMiddleware
{
    /// <summary>
    /// Executes the <c>script</c> entries defined in a <c>package.json</c> file,
    /// capturing any output written to stdio.
    /// </summary>
    internal class ScriptRunner
    {
        public EventedStreamReader StdOut { get; }
        public EventedStreamReader StdErr { get; }
        public Process RunnerProcess { get; }

        public ScriptRunnerType Runner { get; }

        private string GetExeName()
        {
            switch (Runner)
            {
                case ScriptRunnerType.Npm:
                    return "npm";
                case ScriptRunnerType.Yarn:
                    return "yarn";
                case ScriptRunnerType.Npx:
                    return "npx";
                case ScriptRunnerType.Pnpm:
                    return "pnpm";
                default:
                    return "npm";
            }
        }

        private static string BuildCommand(ScriptRunnerType runner, string scriptName, string arguments)
        {
            var command = new StringBuilder();

            if (runner == ScriptRunnerType.Npm) { command.Append("run "); }

            command.Append(scriptName);
            command.Append(' ');

            if (runner == ScriptRunnerType.Npm) { command.Append("-- "); }

            if (!string.IsNullOrWhiteSpace(arguments)) { command.Append(arguments); }
            return command.ToString();
        }

        private static Regex AnsiColorRegex = new Regex("\x001b\\[[0-9;]*m", RegexOptions.None, TimeSpan.FromSeconds(1));



        public void Kill()
        {
            try { RunnerProcess?.Kill(); } catch { }
            try { RunnerProcess?.WaitForExit(); } catch { }
        }

        public ScriptRunner(string workingDirectory, string scriptName, string arguments, IDictionary<string, string> envVars, ScriptRunnerType runner, bool wsl)
        {
            if (string.IsNullOrEmpty(workingDirectory))
            {
                throw new ArgumentException("Cannot be null or empty.", nameof(workingDirectory));
            }

            if (string.IsNullOrEmpty(scriptName))
            {
                throw new ArgumentException("Cannot be null or empty.", nameof(scriptName));
            }

            Runner = runner;

            var exeName = GetExeName();
            var completeArguments = BuildCommand(runner, scriptName, arguments);

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                if (wsl)
                {
                    completeArguments = $"{exeName} {completeArguments}";
                    exeName = "wsl";
                }
                else
                {
                    // On Windows, the NPM executable is a .cmd file, so it can't be executed
                    // directly (except with UseShellExecute=true, but that's no good, because
                    // it prevents capturing stdio). So we need to invoke it via "cmd /c".
                    completeArguments = $"/c {exeName} {completeArguments}";
                    exeName = "cmd";
                }
            }

            var processStartInfo = new ProcessStartInfo(exeName)
            {
                Arguments = completeArguments,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = workingDirectory
            };

            if (envVars != null)
            {
                foreach (var keyValuePair in envVars)
                {
                    processStartInfo.Environment[keyValuePair.Key] = keyValuePair.Value;
                }
            }

            RunnerProcess = LaunchNodeProcess(processStartInfo);

            StdOut = new EventedStreamReader(RunnerProcess.StandardOutput);
            StdErr = new EventedStreamReader(RunnerProcess.StandardError);
        }

        public void AttachToLogger(ILogger logger)
        {
            // When the NPM task emits complete lines, pass them through to the real logger
            StdOut.OnReceivedLine += line =>
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    // NPM tasks commonly emit ANSI colors, but it wouldn't make sense to forward
                    // those to loggers (because a logger isn't necessarily any kind of terminal)
                    //logger.LogInformation(StripAnsiColors(line).TrimEnd('\n'));
                    // making this console for debug purpose
                    if (line.StartsWith("<s>"))
                    {
                        Console.Error.WriteLine(line.Substring(3));
                    }
                    else
                    {
                        Console.Error.WriteLine(line);
                    }
                }
            };

            StdErr.OnReceivedLine += line =>
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    //logger.LogError(StripAnsiColors(line).TrimEnd('\n'));
                    // making this console for debug purpose
                    if (line.StartsWith("<s>"))
                    {
                        Console.Error.WriteLine(line.Substring(3));
                    }
                    else
                    {
                        Console.Error.WriteLine(line);
                    }
                }
            };

            // But when it emits incomplete lines, assume this is progress information and
            // hence just pass it through to StdOut regardless of logger config.
            StdErr.OnReceivedChunk += chunk =>
            {
                var containsNewline = Array.IndexOf(chunk.Array, '\n', chunk.Offset, chunk.Count) >= 0;

                if (!containsNewline)
                {
                    Console.Write(chunk.Array, chunk.Offset, chunk.Count);
                }
            };
        }

        private static string StripAnsiColors(string line)
            => AnsiColorRegex.Replace(line, string.Empty);

        private static Process LaunchNodeProcess(ProcessStartInfo startInfo)
        {
            try
            {
                var process = Process.Start(startInfo);

                // See equivalent comment in OutOfProcessNodeInstance.cs for why
                process.EnableRaisingEvents = true;

                return process;
            }
            catch (Exception ex)
            {
                var message = $"Failed to start '{startInfo.FileName}'. To resolve this:.\n\n"
                            + $"[1] Ensure that '{startInfo.FileName}' is installed and can be found in one of the PATH directories.\n"
                            + $"    Current PATH enviroment variable is: { Environment.GetEnvironmentVariable("PATH") }\n"
                            + "    Make sure the executable is in one of those directories, or update your PATH.\n\n"
                            + "[2] See the InnerException for further details of the cause.";
                throw new InvalidOperationException(message, ex);
            }
        }
    }
}
