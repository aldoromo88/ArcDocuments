using Nest;
using System;

namespace ArcDocuments.Core.Applications.Common
{
    public class BaseApp
    {
        readonly IElasticClient _storage;

        public BaseApp(string index)
        {
            //TODO Get this path from configs
            var node = new Uri("http://localhost:9200");
            var settings = new ConnectionSettings(node);
            settings.DefaultIndex(index);
            settings.DisableDirectStreaming(true);

            _storage = new ElasticClient(settings);
        }

        public  IElasticClient Storage => _storage;
    }
}
