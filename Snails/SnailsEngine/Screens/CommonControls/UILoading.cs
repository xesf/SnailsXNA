using System;
using System.Collections.Generic;
using System.Text;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using System.Threading;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    /// Control used for assynchronous operations
    /// The control shows the loading icon while the assynchronous operation is running
    /// All SnailScreen operations are disabled while the loading is running
    class UILoading : UIControl
    {
        #region Events
        public event UIEvent OnLoadEnded; 
        #endregion

        #region Vars
        private SpriteAnimation _loadingAnim;
        private Thread _loadThread;
		//private bool _ended;
        #endregion

        #region Properties
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public UILoading(UIScreen ownerScreen) :
            base(ownerScreen)
        {
            this._loadingAnim = new SpriteAnimation(BrainGame.ResourceManager.GetSpriteTemporary("spriteset/anim-snails", "SnailWalk"));
            this.Visible = false;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainEngine.BrainGameTime gameTime)
        {
            this._loadingAnim.Update(gameTime);
            if (this._loadThread != null)
	    {
		if (this._loadThread.IsAlive == false)
                {
                   this._loadThread = null;
                   this.Visible = false;
                   if (this.OnLoadEnded != null)
                   {
                      this.OnLoadEnded(this);
                   }
                }
	    }            
        }
	
        /// <summary>
        /// 
        /// </summary>
	public void LoadThreadEntryPoint(object param)
	{
		IAsyncOperation operation = param as IAsyncOperation;
		operation.BeginLoad();
		
	}

        /// <summary>
        /// 
        /// </summary>
        public void BeginLoad(IAsyncOperation operation)
        {
#if DEBUG
           if (this._loadThread != null) 
           { 
             throw new SnailsException("Loading control as already started a load.");
           }
#endif

           this._loadThread = new Thread(new ParameterizedThreadStart(this.LoadThreadEntryPoint));
           this.Visible = true;
	   operation.BeginLoad();
	}
    }
}
