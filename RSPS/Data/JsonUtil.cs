using RSPS.Game.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RSPS.Data
{
    /// <summary>
    /// Contains json related utilities
    /// </summary>
    public static class JsonUtil
    {


        /// <summary>
        /// Deserializes a list of objects from a json file
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <param name="filePath">The file path</param>
        /// <returns>The list of deserialized objects</returns>
        public static List<T>? DeserializeListFromFile<T>(string filePath) where T : class
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            return DeserializeList<T>(File.ReadAllText(filePath));
        }

        /// <summary>
        /// Deserializes an object from a json file
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <param name="filePath">The file path</param>
        /// <returns>The list of deserialized objects</returns>
        public static T? DeserializeFromFile<T>(string filePath) where T : class
        {
            if (!File.Exists(filePath))
            {
                return null;
            }
            return Deserialize<T>(File.ReadAllText(filePath));
        }

        /// <summary>
        /// Deserializes a list of objects from a json string
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <param name="jsonData">The json string</param>
        /// <returns>The list of deserialized objects</returns>
        public static List<T>? DeserializeList<T>(string jsonData) where T : class
        {
            return JsonSerializer.Deserialize<List<T>>(jsonData);
        }

        /// <summary>
        /// Deserializes an object from a json string
        /// </summary>
        /// <typeparam name="T">The object type</typeparam>
        /// <param name="jsonData">The json string</param>
        /// <returns>The deserialized object</returns>
        public static T? Deserialize<T>(string jsonData) where T : class
        {
            return JsonSerializer.Deserialize<T>(jsonData);
        }

        /// <summary>
        /// Imports data in the form of a list to the application
        /// </summary>
        /// <typeparam name="T">The data type</typeparam>
        /// <param name="filePath">The file path</param>
        /// <param name="handle">The data handler</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="InvalidDataException"></exception>
        public static void DataImport<T>(string filePath, Action<List<T>> handle) where T : class
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentNullException(nameof(filePath));
            }
            List<T>? elements = DeserializeListFromFile<T>(filePath);

            if (elements == null)
            {
                throw new InvalidDataException(nameof(elements));
            }
            handle(elements);
        }

        /// <summary>
        /// Serializes an element to a json string
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="element">The element</param>
        /// <param name="indented">Whether to use indented (pretty) formatting</param>
        /// <returns>The json string</returns>
        public static string Serialize<T>(T element, bool indented = true) where T : class
        {
            return JsonSerializer.Serialize(element, new JsonSerializerOptions
            {
                WriteIndented = indented
            });
        }

        /// <summary>
        /// Serializes an list of elements to a json string
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="elements">The elements</param>
        /// <param name="indented">Whether to use indented (pretty) formatting</param>
        /// <returns>The json string</returns>
        public static string SerializeList<T>(List<T> elements, bool indented = true) where T : class
        {
            return JsonSerializer.Serialize(elements, new JsonSerializerOptions
            {
                WriteIndented = indented
            });
        }

        /// <summary>
        /// Serializes an element to a file
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="filePath">The file path</param>
        /// <param name="element">the element</param>
        /// <param name="indented">Whether to use indented (pretty) formatting</param>
        public static void SerializeToFile<T>(string filePath, T element, bool indented = true) where T : class
        {
            File.WriteAllText(filePath, Serialize(element, indented));
        }

        /// <summary>
        /// Serializes a list of elements to a file
        /// </summary>
        /// <typeparam name="T">The element type</typeparam>
        /// <param name="filePath">The file path</param>
        /// <param name="elements">the elements</param>
        /// <param name="indented">Whether to use indented (pretty) formatting</param>
        public static void SerializeListToFile<T>(string filePath, List<T> elements, bool indented = true) where T : class
        {
            File.WriteAllText(filePath, SerializeList(elements, indented));
        }

    }
}
