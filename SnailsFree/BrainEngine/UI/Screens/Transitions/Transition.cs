using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoBrainsGames.BrainEngine;

namespace TwoBrainsGames.BrainEngine.UI.Screens.Transitions
{
    public abstract class Transition
    {
        public event EventHandler OnTransitionEnded;
        public bool _ended;

        public Transition()
        {
	    }

        public virtual void Initialize()
        {
            this._ended = false;
        }

        public virtual void LoadContent()
        {
        }

        public virtual void Update(BrainGameTime gameTime) 
        {          
        }

        public virtual void Draw() 
        {
        }

        public virtual void OnStart()
        {
        }

        public virtual void Reset()
        {
        }

        public virtual void TransitionOut()
        {
        }

        protected void InvokeTransitonEnded()
        {
            if (this.OnTransitionEnded != null)
            {
                this.OnTransitionEnded(this, new EventArgs());
            }
        }
    }
}
