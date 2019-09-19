using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Slickflow.Scheduler.Web.Utility
{
    /// <summary>
    /// 按xml格式传输
    /// </summary>
    public class XmlContent : HttpContent
    {
        private readonly MemoryStream _stream = new MemoryStream();
        public XmlContent(XmlDocument document)
        {
            document.Save(_stream);
            _stream.Position = 0;
            Headers.ContentType = new MediaTypeHeaderValue("application/xml");
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            _stream.CopyTo(stream);
            var tcs = new TaskCompletionSource<object>();
            tcs.SetResult(null);
            return tcs.Task;
        }

        protected override bool TryComputeLength(out long length)
        {
            length = _stream.Length;
            return true;
        }
    }
}
