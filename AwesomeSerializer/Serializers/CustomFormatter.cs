﻿using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using AwesomeSerializer.ResolverBase;
using Newtonsoft.Json;

namespace AwesomeSerializer.ContractResolvers.Formatters
{
    public class CustomFormatter : JsonMediaTypeFormatter
    {
        private readonly JsonSerializerSettings jsonSerializerSettings;

        private readonly Resolver resolver;

        public CustomFormatter()
            : this(new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, PreserveReferencesHandling = PreserveReferencesHandling.None })
        {

        }

        public CustomFormatter(Type resolver)
            : this()
        {
            this.resolver = Activator.CreateInstance(resolver) as Resolver;
        }

        public CustomFormatter(JsonSerializerSettings settings)
        {
            this.jsonSerializerSettings = settings;

            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));

            this.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/javascript"));
        }

        public override MediaTypeFormatter GetPerRequestFormatterInstance(
           Type type,
           HttpRequestMessage request,
           MediaTypeHeaderValue mediaType)
        {
            var formatter = new JsonMediaTypeFormatter
            {
                SerializerSettings = jsonSerializerSettings
            };

            if (this.resolver != null)
                formatter.SerializerSettings.ContractResolver = resolver;

            return formatter;
        }
    }
}