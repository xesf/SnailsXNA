using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace TwoBrainsGames.BrainEngine.Data
{
    /// <summary>
    /// Windows 8 Storage Helper
    /// </summary>
    public class StorageHelper
    {
        public static Stream ReadStream(string filename)
        {
            try
            {
                return Task.Run<Stream>(
                    async () =>
                    {
                        StorageFolder folder = ApplicationData.Current.LocalFolder;
                        return await folder.OpenStreamForReadAsync(filename);
                    }).Result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Stream WriteStream(string filename)
        {
            return Task.Run<Stream>(
                async () =>
                {
                    StorageFolder folder = ApplicationData.Current.LocalFolder;
                    return await folder.OpenStreamForWriteAsync(filename, CreationCollisionOption.OpenIfExists);
                }).Result;
        }
    }
} 
