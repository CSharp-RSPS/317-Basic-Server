using RSPS.src.game.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RSPS.src.Data
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

    }
}
