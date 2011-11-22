using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amazon.SimpleDB.Model;
using Rolstad.Extensions;
using Attribute = Amazon.SimpleDB.Model.Attribute;

namespace Domus.Providers.Amazon
{
    /// <summary>
    /// Extension methods for Amazon Attributes
    /// </summary>
    internal static class AmazonAttributeExtensions
    {
        /// <summary>
        /// For a given attribute name, gets the full value
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public static string GetFullValue( this IEnumerable<Attribute> attributes )
        {
            var builder = new StringBuilder();

            attributes.OrderBy(a => a.Name).Each(a => builder.Append(a.Value));

            return builder.ToString();
        }

        /// <summary>
        /// Converts a string into an enumeration of replaceable attributes
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="attributeName">name of the attribute</param>
        /// <returns></returns>
        public static IEnumerable<ReplaceableAttribute> Convert( this string value, string attributeName )
        {
            // Ensure nulls are converted to empty
            var nullSafeValue = value ?? string.Empty;

            // Split up the string and convert into attributse
            var chunkedValues = nullSafeValue.Chunk(500);

            var attributes = new List<ReplaceableAttribute>();
            int attributeCount = 0;
            foreach (var item in chunkedValues)
            {
                attributes.Add(new ReplaceableAttribute {Name = "{0}_{1}".StringFormat(attributeName, attributeCount), Replace = false, Value = item ?? string.Empty});
                attributeCount++;
            }

            // Set the first attribute to replace the others
            attributes.First(a => a.Replace = true);

            return attributes;
        }

        /// <summary>
        /// Chunks a string up into an enumeration of strings
        /// </summary>
        /// <param name="stringToChunk">String being chunked</param>
        /// <param name="chunkSize">Size of the chunks</param>
        /// <returns></returns>
        public static IEnumerable<string> Chunk( this string stringToChunk, int chunkSize )
        {
            if (stringToChunk.IsEmpty())
            {
                yield return string.Empty;
            }
            else
            {
                for (int offset = 0; offset < stringToChunk.Length; offset += chunkSize)
                {
                    int size = Math.Min(chunkSize, stringToChunk.Length - offset);
                    yield return stringToChunk.Substring(offset, size);
                }
            }
        }
    }
}