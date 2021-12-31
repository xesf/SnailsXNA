using System.Collections.Generic;
using System.Threading;
using TwoBrainsGames.BrainEngine;
#if WIN8
using System.Threading.Tasks;
#endif

namespace TwoBrainsGames.Snails
{
    /// <summary>
    /// The async processor is used to run a batch of async operations
    /// Add operations and then call Begin()
    /// When all opertions end, IsLoading returns false
    /// It's used to load a stage and tutorial data in a single call for instance
    /// </summary>
    class AsyncProcessor
    {
#if !WIN8
        Thread _processorThread;
#else
        //Task _processorThread;
#endif
        public List<IAsyncOperation> Operations { get; private set; }
        public bool IsLoading
        {
            get 
            {
#if !WIN8
                return (this._processorThread != null && this._processorThread.IsAlive); 
#else
                return false; // !this._processorThread.IsCompleted;
#endif
            } 
        }
        public System.Exception ExceptionThrown { get; private set; }
        public AsyncProcessor()
        {
            this.Operations = new List<IAsyncOperation>();
        }

        /// <summary>
        /// 
        /// </summary>
        private void ProcessorThreadEntryPoint(object param)
        {
            AsyncProcessor processor = (AsyncProcessor)param;

            try
            {
                foreach (IAsyncOperation operation in this.Operations)
                {
                    operation.BeginLoad();
                }
            }
            catch (System.Exception ex)
            {
                processor.ExceptionThrown = ex;
            }
            finally
            {
                //BrainGame.ResourceManager.Unlock();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Begin()
        {
            this.ExceptionThrown = null;
#if !WIN8
            this._processorThread = new Thread(new ParameterizedThreadStart(this.ProcessorThreadEntryPoint));
            this._processorThread.Start(this);
#else
            //this._processorThread = new Task(ProcessorThreadEntryPoint);
            //this._processorThread.Start();
#endif
        }

    }
}
