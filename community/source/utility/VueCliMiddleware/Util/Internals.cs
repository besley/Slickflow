// Copyright (c) .NET Foundation Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Original Source: https://github.com/aspnet/JavaScriptServices

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;


[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("VueCliMiddleware.Tests")]
namespace VueCliMiddleware
{
    internal static class LoggerFinder
    {
        public static ILogger GetOrCreateLogger(
            IApplicationBuilder appBuilder,
            string logCategoryName)
        {
            // If the DI system gives us a logger, use it. Otherwise, set up a default one.
            var loggerFactory = appBuilder.ApplicationServices.GetService<ILoggerFactory>();
            if (loggerFactory == null)
            {
                loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder.AddConsole();
                });
            }

            return loggerFactory.CreateLogger(logCategoryName);
        }
    }

    internal static class TaskTimeoutExtensions
    {
        public static async Task WithTimeout(this Task task, TimeSpan timeoutDelay, string message)
        {
            if (task == await Task.WhenAny(task, Task.Delay(timeoutDelay)))
            {
                task.Wait(); // Allow any errors to propagate
            }
            else
            {
                throw new TimeoutException(message);
            }
        }

        public static async Task<T> WithTimeout<T>(this Task<T> task, TimeSpan timeoutDelay, string message)
        {
            if (task == await Task.WhenAny(task, Task.Delay(timeoutDelay)))
            {
                return task.Result;
            }
            else
            {
                throw new TimeoutException(message);
            }
        }
    }

    internal static class TcpPortFinder
    {
        public static int FindAvailablePort()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            try
            {
                return ((IPEndPoint)listener.LocalEndpoint).Port;
            }
            finally
            {
                listener.Stop();
            }
        }
    }

    /// <summary>
    /// Wraps a <see cref="StreamReader"/> to expose an evented API, issuing notifications
    /// when the stream emits partial lines, completed lines, or finally closes.
    /// </summary>
    internal class EventedStreamReader
    {
        public delegate void OnReceivedChunkHandler(ArraySegment<char> chunk);
        public delegate void OnReceivedLineHandler(string line);
        public delegate void OnStreamClosedHandler();

        public event OnReceivedChunkHandler OnReceivedChunk;
        public event OnReceivedLineHandler OnReceivedLine;
        public event OnStreamClosedHandler OnStreamClosed;

        private readonly StreamReader _streamReader;
        private readonly StringBuilder _linesBuffer;

        public EventedStreamReader(StreamReader streamReader)
        {
            _streamReader = streamReader ?? throw new ArgumentNullException(nameof(streamReader));
            _linesBuffer = new StringBuilder();
            Task.Factory.StartNew(Run);
        }

        public Task<Match> WaitForMatch(Regex regex)
        {
            var tcs = new TaskCompletionSource<Match>();
            var completionLock = new object();

            OnReceivedLineHandler onReceivedLineHandler = null;
            OnStreamClosedHandler onStreamClosedHandler = null;

            void ResolveIfStillPending(Action applyResolution)
            {
                lock (completionLock)
                {
                    if (!tcs.Task.IsCompleted)
                    {
                        OnReceivedLine -= onReceivedLineHandler;
                        OnStreamClosed -= onStreamClosedHandler;
                        applyResolution();
                    }
                }
            }

            onReceivedLineHandler = line =>
            {
                var match = regex.Match(line);
                if (match.Success)
                {
                    ResolveIfStillPending(() => tcs.SetResult(match));
                }
            };

            onStreamClosedHandler = () =>
            {
                ResolveIfStillPending(() => tcs.SetException(new EndOfStreamException()));
            };

            OnReceivedLine += onReceivedLineHandler;
            OnStreamClosed += onStreamClosedHandler;

            return tcs.Task;
        }

        private async Task Run()
        {
            var buf = new char[8 * 1024];
            while (true)
            {
                var chunkLength = await _streamReader.ReadAsync(buf, 0, buf.Length);
                if (chunkLength == 0)
                {
                    OnClosed();
                    break;
                }

                OnChunk(new ArraySegment<char>(buf, 0, chunkLength));

                int lineBreakPos = -1;
                int startPos = 0;

                // get all the newlines
                while ((lineBreakPos = Array.IndexOf(buf, '\n', startPos, chunkLength - startPos)) >= 0 && startPos < chunkLength)
                {
                    var length = lineBreakPos - startPos;
                    _linesBuffer.Append(buf, startPos, length);
                    OnCompleteLine(_linesBuffer.ToString());
                    _linesBuffer.Clear();
                    startPos = lineBreakPos + 1;
                }

                // get the rest
                if (lineBreakPos < 0 && startPos < chunkLength)
                {
                    _linesBuffer.Append(buf, startPos, chunkLength - startPos);
                }
            }
        }

        private void OnChunk(ArraySegment<char> chunk)
        {
            var dlg = OnReceivedChunk;
            dlg?.Invoke(chunk);
        }

        private void OnCompleteLine(string line)
        {
            var dlg = OnReceivedLine;
            dlg?.Invoke(line);
        }

        private void OnClosed()
        {
            var dlg = OnStreamClosed;
            dlg?.Invoke();
        }
    }

    /// <summary>
    /// Captures the completed-line notifications from a <see cref="EventedStreamReader"/>,
    /// combining the data into a single <see cref="string"/>.
    /// </summary>
    internal class EventedStreamStringReader : IDisposable
    {
        private EventedStreamReader _eventedStreamReader;
        private bool _isDisposed;
        private StringBuilder _stringBuilder = new StringBuilder();

        public EventedStreamStringReader(EventedStreamReader eventedStreamReader)
        {
            _eventedStreamReader = eventedStreamReader
                ?? throw new ArgumentNullException(nameof(eventedStreamReader));
            _eventedStreamReader.OnReceivedLine += OnReceivedLine;
        }

        public string ReadAsString() => _stringBuilder.ToString();

        private void OnReceivedLine(string line) => _stringBuilder.AppendLine(line);

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _eventedStreamReader.OnReceivedLine -= OnReceivedLine;
                _isDisposed = true;
            }
        }
    }
}
