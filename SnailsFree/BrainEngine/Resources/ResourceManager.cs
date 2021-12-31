using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using TwoBrainsGames.BrainEngine.Graphics;

namespace TwoBrainsGames.BrainEngine.Resources
{


    public class ResourceManager
    {

        public const string RES_MANAGER_ID_STATIC = "__STATIC__";
        public const string RES_MANAGER_ID_TEMPORARY = "__TEMPORARY__";

        #region Constants
        public enum ResourceManagerCacheType
        {
            Temporary,
            Static,
            UserDefined
        }
        #endregion

        #region Members
        BrainContentManager StaticResManager { get { return this.ResourceManagers[RES_MANAGER_ID_STATIC]; } }
        BrainContentManager TemporaryResManager { get { return this.ResourceManagers[RES_MANAGER_ID_TEMPORARY]; } }
        Dictionary<string, BrainContentManager> ResourceManagers { get; set; }
        IServiceProvider ServiceProvider { get; set; }
        string RootDirectory { get; set; }
        #endregion


        public ResourceManager(IServiceProvider serviceProvider, string rootDirectory)
        {
            this.ServiceProvider = serviceProvider;
            this.RootDirectory = rootDirectory;
            this.ResourceManagers = new Dictionary<string, BrainContentManager>();
            this.ResourceManagers.Add(RES_MANAGER_ID_STATIC, new BrainContentManager(serviceProvider, rootDirectory, ResourceManagerCacheType.Static, RES_MANAGER_ID_STATIC));
            this.ResourceManagers.Add(RES_MANAGER_ID_TEMPORARY, new BrainContentManager(serviceProvider, rootDirectory, ResourceManagerCacheType.Temporary, RES_MANAGER_ID_TEMPORARY));
        }

        /// <summary>
        /// 
        /// </summary>
        public void CreateUserDefinedResourceManager(string resManagerId)
        {
            if (this.ResourceManagers.ContainsKey(resManagerId))
            {
                return;
            }
            this.ResourceManagers.Add(resManagerId, new BrainContentManager(this.ServiceProvider, this.RootDirectory, ResourceManagerCacheType.UserDefined, resManagerId));
        }

        /// <summary>
        /// Loads an asset that has been processed by the Content Pipeline and Cache It.
        /// If we don't pass CacheType than it will be a temporary resource
        /// </summary>
        /// <typeparam name="T">Type T of asset object to load</typeparam>
        /// <param name="assetName">Asset name</param>
        /// <returns>Instance of object type T</returns>
        public T Load<T>(string assetName)
        {
            return Load<T>(assetName, ResourceManagerCacheType.Temporary);
        }
        
        /// <summary>
        /// 
        /// </summary>
        public T Load<T>(string assetName, string resManagerId)
        {
            if (!this.ResourceManagers.ContainsKey(resManagerId))
            {
                throw new BrainException("Resource Manager with id [" + resManagerId + "] does not exist. Use ResourceManager.CreateUserDefinedResourceManager() to create one.");
            }

            return this.ResourceManagers[resManagerId].Load<T>(assetName);
        }

        /// <summary>
        /// 
        /// </summary>
        public T Load<T>(string assetName, ResourceManagerCacheType type)
        {
            T asset = default(T);
            switch (type)
            {
                case ResourceManagerCacheType.Temporary:
                    // If resource already loaded as static, ignore it
               /*     if (this.StaticResManager.ContainsAsset(assetName))
                    {
                        return this.StaticResManager.Load<T>(assetName);
                    }*/
                    asset = this.TemporaryResManager.Load<T>(assetName);
                    break;

                case ResourceManagerCacheType.Static:
                    asset = this.StaticResManager.Load<T>(assetName);
                    break;

                case ResourceManagerCacheType.UserDefined:
                    throw new BrainException("Resource ID must be specified for UserDefined resource managers.");
            }

            return asset;
        }

        /// <summary>
        /// 
        /// </summary>
        public void UnloadTemporary()
        {
            this.Unload(this.TemporaryResManager);
        }

        /// <summary>
        /// 
        /// </summary>
        public void UnloadStatic()
        {
            this.Unload(this.StaticResManager);
        }
       
        /// <summary>
        /// Unload all cached content assets
        /// </summary>
        public void Unload()
        {
            foreach(KeyValuePair<string, BrainContentManager> resource in this.ResourceManagers)
            {
                this.Unload(resource.Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Unload(string resManagerId)
        {
            if (!this.ResourceManagers.ContainsKey(resManagerId))
            {
                return;
            }

            this.Unload(ResourceManagers[resManagerId]);
            GC.Collect();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Unload(BrainContentManager resManager)
        {
            resManager.Unload();
            GC.Collect();
        }

        /// <summary>
        /// 
        /// </summary>
        public Sprite GetSpriteTemporary(string resourceName)
        {
            string assetName = BrainPath.GetDirectoryName(resourceName);
            string spriteName = BrainPath.GetFileName(resourceName);

            return this.GetSpriteTemporary(assetName, spriteName);
        }

        /// <summary>
        /// 
        /// </summary>
        public Sprite GetSprite(string resourceName, string resManagerId)
        {
            string assetName = BrainPath.GetDirectoryName(resourceName);
            string spriteName = BrainPath.GetFileName(resourceName);
            SpriteSet ss = this.Load<SpriteSet>(assetName, resManagerId);
            return ss[spriteName];
        }

        /// <summary>
        /// 
        /// </summary>
        public Sprite GetSpriteTemporary(string assetName, string spriteName)
        {
            SpriteSet ss = this.Load<SpriteSet>(assetName, ResourceManager.ResourceManagerCacheType.Temporary);
            return ss[spriteName];
        }

        /// <summary>
        /// 
        /// </summary>
        public Sprite GetSpriteStatic(string assetName, string spriteName)
        {
            SpriteSet ss = this.Load<SpriteSet>(assetName, ResourceManager.ResourceManagerCacheType.Static);
            return ss[spriteName];
        }

        /// <summary>
        /// 
        /// </summary>
        public Sprite GetSpriteStatic(string spriteResourceName)
        {
            string assetName = BrainPath.GetDirectoryName(spriteResourceName);
            string spriteName = BrainPath.GetFileName(spriteResourceName);

            return this.GetSpriteStatic(assetName, spriteName);
        }

        /// <summary>
        /// 
        /// </summary>
        public Sprite GetSprite(string assetName, string spriteName, ResourceManager.ResourceManagerCacheType type)
        {
            SpriteSet ss = this.Load<SpriteSet>(assetName, type);
            return ss[spriteName];
        }

        /// <summary>
        /// 
        /// </summary>
        public Sprite GetSprite(string resName, ResourceManager.ResourceManagerCacheType type)
        {
            string assetName = BrainPath.GetDirectoryName(resName);
            string spriteName = BrainPath.GetFileName(resName);
            SpriteSet ss = this.Load<SpriteSet>(assetName, type);
            return ss[spriteName];
        }

        /// <summary>
        /// 
        /// </summary>
        public Sample GetSampleTemporary(string assetName)
        {
            return GetSampleTemporary(assetName, null);
        }

        /// <summary>
        /// 
        /// </summary>
        public Sample GetSampleStatic(string assetName)
        {
            return GetSampleStatic(assetName, null);
        }

        /// <summary>
        /// 
        /// </summary>
        public Sample GetSampleTemporary(string assetName, Object2D objEmitter)
        {
            return new Sample(this.Load<SoundEffect>(assetName, ResourceManager.ResourceManagerCacheType.Temporary), objEmitter);
        }

        /// <summary>
        /// 
        /// </summary>
        public Sample GetSampleStatic(string assetName, Object2D objEmitter)
        {
            return new Sample(this.Load<SoundEffect>(assetName, ResourceManager.ResourceManagerCacheType.Static), objEmitter);
        }

        /// <summary>
        /// 
        /// </summary>
        public Sample GetSample(string assetName, ResourceManager.ResourceManagerCacheType type)
        {
            return new Sample(this.Load<SoundEffect>(assetName, type));
        }

        /// <summary>
        /// 
        /// </summary>
        public Sample GetSample(string assetName, string resourceManagerId)
        {
            return new Sample(this.Load<SoundEffect>(assetName, resourceManagerId));
        }

        /// <summary>
        /// 
        /// </summary>
        public Sample GetSample(string assetName, string resourceManagerId, Object2D objEmitter)
        {
            return new Sample(this.Load<SoundEffect>(assetName, resourceManagerId), objEmitter);
        }

        /// <summary>
        /// 
        /// </summary>
        public Music GetMusicTemporary(string assetName)
        {
            return new Music(this.Load<Song>(assetName, ResourceManager.ResourceManagerCacheType.Temporary));
        }

        /// <summary>
        /// 
        /// </summary>
        public Music GetMusicStatic(string assetName)
        {
            return new Music(this.Load<Song>(assetName, ResourceManager.ResourceManagerCacheType.Static));
        }

        /// <summary>
        /// 
        /// </summary>
        public Music GetMusic(string assetName, ResourceManager.ResourceManagerCacheType type)
        {
            return new Music(this.Load<Song>(assetName, type));
        }

        /// <summary>
        /// 
        /// </summary>
        public Music GetMusic(string assetName, string resourceManagerId)
        {
            return new Music(this.Load<Song>(assetName, resourceManagerId));
        }

        /// <summary>
        /// 
        /// </summary>
        public bool ContainsSprite(string assetName, string spriteName)
        {
            assetName = this.StaticResManager.NormalizeAssetName(assetName);

            if (this.StaticResManager.ContainsAssetNormalized(assetName))
            {
                SpriteSet ss = (SpriteSet)this.StaticResManager[assetName];
                return ss.ContainsSprite(spriteName);
            }

            if (this.TemporaryResManager.ContainsAssetNormalized(assetName))
            {
                SpriteSet ss = (SpriteSet)this.TemporaryResManager[assetName];
                return ss.ContainsSprite(spriteName);
            }
            
            return false;
        }
        #region Thread sync
        ///---------------------------------------------------------------
        /// Used for thread synchronization
        /// When resource loading is done in a separate thread
        /// This is needed in the BrainGame.Draw() when tracing the ResourceManager
        ///---------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public void Lock()
        {
            Monitor.Enter(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool TryLock(int timeout)
        {
            return Monitor.TryEnter(this, timeout);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Unlock()
        {
            Monitor.Exit(this);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsLocked()
        {
            if (Monitor.TryEnter(this) == false)
            {
                return true;
            }
            Monitor.Exit(this);
            return false;
        }
        #endregion
       
        /// <summary>
        /// 
        /// </summary>
        public void TraceLoadedResources(SpriteBatch spriteBatch, SpriteFont font)
        {
           

            BrainGame.DrawRectangleFilled(spriteBatch, new Rectangle(BrainGame.Viewport.X, BrainGame.Viewport.Y, BrainGame.Viewport.Width, BrainGame.Viewport.Height), new Color(0, 0, 0, 120));
            Vector2 position = new Vector2(20f, 60f);
            foreach (KeyValuePair<string, BrainContentManager> resource in this.ResourceManagers)
            {
                position = this.TraceLoadedResources(resource.Value, spriteBatch, font, position);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private Vector2 TraceLoadedResources(BrainContentManager manager, SpriteBatch spriteBatch, SpriteFont font, Vector2 position)
        {
            spriteBatch.DrawString(font, manager.Id, position, Color.Cyan);
            position += new Vector2(0f, font.MeasureString(manager.Id).Y);
            Color color;
            foreach (KeyValuePair<string, object> resource in manager.Resources)
            {
                //string text = string.Format("{0} {1}", resource.Key.PadRight(40), resource.Value.GetType().Name.PadRight(20));
                string text = string.Format("{0}", resource.Key.PadRight(40));
                color = Color.Yellow;
                if (resource.Value is Texture2D)
                {
                    color = Color.Orange;
                }

                if (this.IsResourceDoubleUsed(manager, resource.Key))
                {
                    color = Color.Red;
                }

                spriteBatch.DrawString(font, text, position, color);
                position += new Vector2(0f, font.MeasureString(text).Y);
                if (position.Y + font.MeasureString(text).Y > BrainGame.ScreenHeight)
                {
                    position = new Vector2(position.X + 250f, 60f);
                }

            }

            return position;
        }

        /// <summary>
        /// 
        /// </summary>
        private bool IsResourceDoubleUsed(BrainContentManager ignoreManager, string resourceId)
        {
            foreach (KeyValuePair<string, BrainContentManager> manager in this.ResourceManagers)
            {
                if (manager.Value == ignoreManager)
                {
                    continue;
                }
                if (manager.Value.Resources.ContainsKey(resourceId))
                {
                    return true;
                }
            }
            return false;
        }

        
    }
}
