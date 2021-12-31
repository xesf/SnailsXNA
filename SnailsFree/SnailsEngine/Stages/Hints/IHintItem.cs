using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine;

namespace TwoBrainsGames.Snails.Stages.Hints
{
    public interface IHintItem : IDataFileSerializable
    {
        void Initialize();
        void Draw(SpriteBatch spriteBatch, bool useColorEffect);
        object ItemObject { get; }
        DataFileRecord ToDataFileRecord(ToDataFileRecordContext context);
        Color BlendColorStageEditor { get; }
    }
}
