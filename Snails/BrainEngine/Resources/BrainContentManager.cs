using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace TwoBrainsGames.BrainEngine.Resources
{
    public class BrainContentManager : ContentManager
    {
        public string Id { get; private set; }
        ResourceManager.ResourceManagerCacheType _cacheType;

        public Dictionary<string, object> Resources { get; private set; }

        public object this[string key]
        {
            get
            {
                return this.Resources[key];
            }
        }


        public BrainContentManager(IServiceProvider serviceProvider, string rootDirectory, ResourceManager.ResourceManagerCacheType cacheType, string id) :
            base(serviceProvider, rootDirectory)
        {
            this.Id = id;
            this._cacheType = cacheType; // This helps debugging
            this.Resources = new Dictionary<string, object>();
        }

        public override T Load<T>(string assetName)
        {
            assetName = this.NormalizeAssetName(assetName);
            if (this.ContainsAsset(assetName))
            {
                return (T)Resources[assetName];
            }
   
            T asset = base.Load<T>(assetName);
            this.Resources.Add(assetName, asset);

            return asset;
        }

        public override void Unload()
        {
            base.Unload();
            this.Resources.Clear();
        }

        public bool ContainsAsset(string assetName)
        {
            return this.Resources.ContainsKey(this.NormalizeAssetName(assetName));
        }

        public bool ContainsAssetNormalized(string normalizedAssetName)
        {
            return this.Resources.ContainsKey(normalizedAssetName);
        }

        public string NormalizeAssetName(string assetName)
        {
            assetName = assetName.Replace("/", BrainPath.DirectorySeparatorChar.ToString()).ToLower();
            return assetName.Replace("\\", BrainPath.DirectorySeparatorChar.ToString()).ToLower();
        }
    }
}
