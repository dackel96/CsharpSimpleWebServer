namespace CsharpSimpleWebServer.Server.Http
{
    public class HttpRequest
    {
        public HttpMethod Method { get; private set; }

        public string Url { get; private set; }

        public HttpHeaderCollection Headers { get; private set; }

        public string Body { get; private set; }

        public static HttpRequest Parse(string request)
        {
            string[] lines = request.Split(Constants.NewLine);

            string[] startLine = lines.First().Split(" ");

            var method = ParseHttpMethod(startLine[0]);

            var url = startLine[1];

            var headerCollection = ParseHttpHeaderCollection(lines.Skip(1));

            var bodyLines = lines.Skip(headerCollection.Count + 2).ToArray();

            var body = string.Join(Constants.NewLine, bodyLines);

            return new HttpRequest
            {
                Method = method,
                Url = url,
                Headers = headerCollection,
                Body = body
            };
        }

        private static HttpMethod ParseHttpMethod(string method)
        {
            return method.ToUpper() switch
            {
                "GET" => HttpMethod.Get,
                "POST" => HttpMethod.Post,
                "PUT" => HttpMethod.Put,
                "DELETE" => HttpMethod.Delete,
                _ => throw new InvalidOperationException($"Method '{method}' is not supported!")
            };
        }

        private static HttpHeaderCollection ParseHttpHeaderCollection(IEnumerable<string> headerLines)
        {
            var headerCollection = new HttpHeaderCollection();

            foreach (var headerLine in headerLines)
            {
                if (headerLine == String.Empty)
                {
                    break;
                }

                var IndexOfColon = headerLine.IndexOf(":");

                if (IndexOfColon < 0)
                {
                    throw new InvalidOperationException("Request is not valid !");
                }

                var header = new HttpHeader
                {
                    Name = headerLine.Substring(0,IndexOfColon),
                    Value = headerLine[(IndexOfColon + 1)..].Trim()
                };

                headerCollection.Add(header);
            }
            return headerCollection;
        }
        //private static string[] GetStarttLine()
        //{

        //}
    }
}
