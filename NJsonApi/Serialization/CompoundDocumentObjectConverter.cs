﻿using System;
using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SocialCee.Framework.NJsonApi.Utils;

namespace SocialCee.Framework.NJsonApi.Serialization
{
    public class CompoundDocumentObjectConverter : CustomCreationConverter<CompoundDocument>
    {
        public override CompoundDocument Create(Type objectType)
        {
            return new CompoundDocument();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartObject();

            // Write properties.
            var propertyInfos = value.GetType().GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                // Skip all properties excluding Data & UnmappedAttributes
                if (propertyInfo.Name == "UnmappedAttributes" || propertyInfo.Name == "Data")
                {
                    continue;
                }

                var propertyName = CamelCaseUtil.ToCamelCase(propertyInfo.Name);
                var propertyValue = propertyInfo.GetValue(value);

                var objectDictionary = propertyValue as IDictionary;
                if (objectDictionary == null || objectDictionary.Count > 0)
                {
                    writer.WritePropertyName(propertyName);
                    serializer.Serialize(writer, propertyValue);
                }
            }

            // Write dictionary key-value pairs.
            var compoundDocument = (CompoundDocument)value;
            foreach (var kvp in compoundDocument.Data)
            {
                writer.WritePropertyName(kvp.Key);
                serializer.Serialize(writer, kvp.Value);
            }
            writer.WriteEndObject();
        }

        public override bool CanWrite
        {
            get { return true; }
        }

    }
}