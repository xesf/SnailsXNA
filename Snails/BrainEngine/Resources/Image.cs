using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TwoBrainsGames.BrainEngine.Resources
{
    public class Image : Resource
    {
        #region Member
        private Texture2D _texture;
        #endregion

        #region Properties
        public Texture2D Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }
        #endregion

        public override bool Load(ContentManager contentManager)
        {
            IsLoaded = false;

            _texture = contentManager.Load<Texture2D>(Path);
            if (_texture != null)
            {
                IsLoaded = true;
            }

            return IsLoaded;
        }

        public override bool Release(ContentManager contentManager)
        {
            if (_texture != null)
                _texture.Dispose();
            IsLoaded = false;
            return true;
        }
    }
}
