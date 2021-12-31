using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using TwoBrainsGames.BrainEngine.Data.DataFiles;

namespace TwoBrainsGames.BrainEngine.Resources
{
    public class Resource : IDataFileSerializable
    {
        public enum ResourceType
        {
            None = 0,
            Image,
            Sprite,
            Font,
            Sample,
            Music
        }

        #region Members
        private string _id;
        private string _gid;
        private string _desc;
        private string _path;
        private string _asset;
        private bool _isLoaded;
        private bool _preLoad;
        private ResourceType _type;
        #endregion

        #region Properties
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string GroupId
        {
            get { return _gid; }
            set { _gid = value; }
        }

        public string Description
        {
            get { return _desc; }
            set { _desc = value; }
        }

        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        public string Asset
        {
            get { return _asset; }
            set { _asset = value; }
        }

        public bool IsLoaded
        {
            get { return _isLoaded; }
            set { _isLoaded = value; }
        }

        public bool Preload
        {
            get { return _preLoad; }
            set { _preLoad = value; }
        }

        public ResourceType Type
        {
            get { return _type; }
            set { _type = value; }
        }
        #endregion

        public Resource()
        {
            _isLoaded = false;
            _preLoad = false;
            _type = ResourceType.None;
            _id = string.Empty;
            _path = string.Empty;
            _asset = string.Empty;
            _desc = string.Empty;
        }

        public virtual void LoadContent(ContentManager contentManager)
        {
            throw new NotImplementedException();
        }

        public virtual bool Load(ContentManager contentManager) 
        {
            throw new NotImplementedException();
        }

        public virtual bool Release(ContentManager contentManager)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Create Resource instance by type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Resource CreateResourceByType(Resource.ResourceType type)
        {
            Resource res;
            switch (type)
            {
                case Resource.ResourceType.Image: res = new Image(); break;
                case Resource.ResourceType.Sprite: res = new SpriteSet(); break;
                case Resource.ResourceType.Font: res = new TextFont(); break;
                case Resource.ResourceType.Sample: res = new Sample(); break;
                case Resource.ResourceType.Music: res = new Music(); break;
                case Resource.ResourceType.None:
                default: res = new Resource(); break;
            }
            return res;
        }

        /// <summary>
        /// Create Resource instance by type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetResourceObjectType(Resource.ResourceType type)
        {
            Type t;

            switch (type)
            {
                case Resource.ResourceType.Image: t = typeof(Image); break;
                case Resource.ResourceType.Sprite: t = typeof(SpriteSet); break;
                case Resource.ResourceType.Font: t = typeof(TextFont); break;
                case Resource.ResourceType.Sample: t = typeof(Sample); break;
                case Resource.ResourceType.Music: t = typeof(Music); break;
                case Resource.ResourceType.None:
                default: t = typeof(Resource); break;
            }
            return t;
        }

        /// <summary>
        /// Only base class need this method
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public static Resource FromDataFileRecordBase(DataFileRecord record)
        {
            Resource.ResourceType type = (Resource.ResourceType)Enum.Parse(typeof(Resource.ResourceType), record.GetFieldValue<string>("type"), false);

            Resource res = Resource.CreateResourceByType(type);
            res.Type = type;
            res.InitFromDataFileRecordBase(record);
            return res;
        }

        /// <summary>
        ///  Init From DataFile - We don't use the interface in this case because of each derived type that make use of it.
        /// </summary>
        /// <param name="record"></param>
        public void InitFromDataFileRecordBase(DataFileRecord record)
        {
            //this._type = (ResourceType)Enum.Parse(typeof(ResourceType), record.GetFieldValue<string>("type"), false);
            this._id = record.GetFieldValue<string>("id");
            this._desc = record.GetFieldValue<string>("desc");
            this._path = record.GetFieldValue<string>("path");
            this._asset = record.GetFieldValue<string>("asset");
            this._preLoad = record.GetFieldValue<bool>("preload");
        }

        public void InitFromResource(Resource res)
        {
            this._type = res.Type;
            this._id = res.Id;
            this._desc = res.Description;
            this._path = res.Path;
            this._asset = res.Asset;
            this._preLoad = res.Preload;
        }

        #region IDataFileSerializable Members

        public virtual void InitFromDataFileRecord(DataFileRecord record)
        {
            throw new NotImplementedException();
        }

        public virtual DataFileRecord ToDataFileRecord()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
