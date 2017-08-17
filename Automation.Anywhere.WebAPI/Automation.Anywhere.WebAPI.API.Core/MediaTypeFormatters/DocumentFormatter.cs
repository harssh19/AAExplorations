#region Using
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Automation.Anywhere.WebAPI.Domain;
#endregion

namespace Automation.Anywhere.WebAPI.API.Core.MediaTypeFormatters
{
    public class DocumentFormatter : BufferedMediaTypeFormatter
    {
        public DocumentFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/document"));
        }

        public override bool CanReadType(Type type)
        {
            return false;
        }

        public override bool CanWriteType(Type type)
        {
            //for single document object
            if (type == typeof(Document))
                return true;
            else
            {
                // for multiple document objects
                Type _type = typeof(IEnumerable<Document>);
                return _type.IsAssignableFrom(type);
            }
        }

        public override void WriteToStream(Type type,
                                           object value,
                                           Stream writeStream,
                                           HttpContent content)
        {
            using (StreamWriter writer = new StreamWriter(writeStream))
            {
                var documents = value as IEnumerable<Document>;
                if (documents != null)
                {
                    foreach (var document in documents)
                    {
                        writer.Write(String.Format("[{0},\"{1}\",\"{2}\",\"{3}\",\"{4}\"]",
                                                    document.ID,
                                                    document.Title,
                                                    document.Author,
                                                    document.OnlineURL,
                                                    document.Contents));
                    }
                }
                else
                {
                    var _document = value as Document;
                    if (_document == null)
                    {
                        throw new InvalidOperationException("Cannot serialize type");
                    }
                    writer.Write(String.Format("[{0},\"{1}\",\"{2}\",\"{3}\",\"{4}\"]",
                                                    _document.ID,
                                                    _document.Title,
                                                    _document.Author,
                                                    _document.OnlineURL,
                                                    _document.Contents));
                }
            }
        }
    }
}
