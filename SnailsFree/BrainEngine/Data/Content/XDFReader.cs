using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Data.DataFiles.BinaryDataFile;

using TRead = TwoBrainsGames.BrainEngine.Data.DataFiles.DataFileRecord;

namespace TwoBrainsGames.BrainEngine.Data.Content
{

	public class XDFReader : ContentTypeReader<TRead>
	{
		static string[] supportedExtensions = new string[] { ".xdf" };

		internal static string Normalize(string fileName)
		{
			return Normalize(fileName, supportedExtensions);
		}

		protected override TRead Read(ContentReader input, TRead existingInstance)
		{

			BinaryDataFileReader reader = new BinaryDataFileReader();
			DataFile dataFile = reader.Read(input.BaseStream, BrainGame.Ek);
			return dataFile.RootRecord;
		}
	}
}
