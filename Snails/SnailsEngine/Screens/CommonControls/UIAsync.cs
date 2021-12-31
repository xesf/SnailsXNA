using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using System.Threading;
using TwoBrainsGames.BrainEngine;

namespace TwoBrainsGames.Snails.Screens.CommonControls
{
    /// <summary>
    /// This control may be used in UIScreens to control the process of async operations
    /// This control shows/hides the hdd access indicador Icon when the async operations are running
    /// This class uses the AsyncProcessor tu execute the async operations, pools for the ending of the operations
    /// and the fires the OnAsyncOperationEnded 
    /// </summary>
    class UIAsync : UIControl
    {
        public event UIEvent OnAsyncOperationEnded;

        public AsyncProcessor _asyncProcessor;
        UITimer _minimumTimeTimer;
        private bool _canEnd;

        public double MinimumTime
        {
            get { return this._minimumTimeTimer.Time; }
            set { this._minimumTimeTimer.Time = value; }
        }

        private bool WithMinimumTime { get { return this._minimumTimeTimer.Time > 0; } }
        /// <summary>
        /// 
        /// </summary>
        public UIAsync(UIScreen screenOwner) :
            base(screenOwner)
        {
            this._asyncProcessor = new AsyncProcessor();
            this.Enabled = false;


            this._minimumTimeTimer = new UITimer(screenOwner);
            this._minimumTimeTimer.Enabled = false;
            this._minimumTimeTimer.Time = 0;
            this._minimumTimeTimer.OnTimer += new UIEvent(_minimumTimeTimer_OnTimer);
            this._minimumTimeTimer.Snooze = false;
            this.Controls.Add(this._minimumTimeTimer);
        }

        /// <summary>
        /// 
        /// </summary>
        void _minimumTimeTimer_OnTimer(IUIControl sender)
        {
            this._canEnd = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearOperations()
        {
            this._asyncProcessor.Operations.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        public void AddOperation(IAsyncOperation operation)
        {
            if (operation == null)
            {
                throw new SnailsException("Async operation cannot be null.");
            }
            this._asyncProcessor.Operations.Add(operation);
        }

        /// <summary>
        /// 
        /// </summary>
        public void StartLoad()
        {
            BrainGame.HddAccessIcon.Visible = true;
            this.Enabled = true;
            this._asyncProcessor.Begin();
            this._minimumTimeTimer.Enabled = this.WithMinimumTime;
            this._minimumTimeTimer.Reset();
            this._canEnd = !this.WithMinimumTime;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update(BrainEngine.BrainGameTime gameTime)
        {
            if (this.Enabled == true)
            {
                if (this._asyncProcessor.IsLoading == false)
                {
                    if (this._asyncProcessor.ExceptionThrown != null)
                    {
                        throw new SnailsException(this._asyncProcessor.ExceptionThrown.Message, this._asyncProcessor.ExceptionThrown);
                    }

                    if (this._canEnd)
                    {
                        this.OperationEnded();
                    }
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void OperationEnded()
        {
            this.Enabled = false;
            BrainGame.HddAccessIcon.Visible = false;
            if (this.OnAsyncOperationEnded != null)
            {
                this.OnAsyncOperationEnded(this);
            }
        }
    }
}
