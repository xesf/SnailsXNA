using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Debugging;

namespace TwoBrainsGames.BrainEngine.Beta
{
	public class DataDispatcher
	{
		const int DISPATCH_POOL_TIME = 1000;
		List<IDataDispatcherItem> Items { get; set; }
		Thread DispatcherThread { get; set; }
		bool Active { get; set; }
        AutoResetEvent EvtTerminate { get; set; }

		public DataDispatcher()
		{
			this.Items = new List<IDataDispatcherItem>();
			this.DispatcherThread = new Thread(new ParameterizedThreadStart(DataDispatcher.DispatcherThreadEntry));
            this.DispatcherThread.IsBackground = true;
            this.EvtTerminate = new AutoResetEvent(false);
		}

		/// <summary>
		/// 
		/// </summary>
		public void Start()
		{
			if (this.Active)
			{
				return;
			}
			this.DispatcherThread.Start(this);
		}

		/// <summary>
		/// 
		/// </summary>
		public void Terminate()
		{
			this.Active = false;
			if (this.DispatcherThread != null)
			{
                this.EvtTerminate.WaitOne(5000);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void AddItem(IDataDispatcherItem item)
		{
			Monitor.Enter(this.Items);
			this.Items.Add(item);
			Monitor.Exit(this.Items);
		}

		/// <summary>
		/// 
		/// </summary>
		private static void DispatcherThreadEntry(object obj)
		{
			int ellapsedSeconds = 0;
			bool timeEllapsed = true;
			DataDispatcher dispatcher = (DataDispatcher)obj;
            dispatcher.EvtTerminate.Reset();
			try
			{
				dispatcher.Active = true;
                BETrace.WriteLine("Webserver message dispather started.");
				do
				{
					Thread.Sleep(1000);
					if (timeEllapsed)
					{
						Monitor.Enter(dispatcher.Items);
						if (dispatcher.Items.Count > 0)
						{
							try
							{
                                BETrace.WriteLine("Dispatching message.");
                                dispatcher.Items[0].Dispatch();
							}
							catch (System.Exception)
							{

							}
							dispatcher.Items.RemoveAt(0);
						}
						Monitor.Exit(dispatcher.Items);
					}
					ellapsedSeconds++;
					if (ellapsedSeconds >= DataDispatcher.DISPATCH_POOL_TIME)
					{
						ellapsedSeconds = 0;
						timeEllapsed = true;
					}
 
				}
                while (dispatcher.Active || dispatcher.Items.Count > 0);
			}
			finally
			{
				dispatcher.Active = false;
                dispatcher.EvtTerminate.Set();
                BETrace.WriteLine("Webserver message dispather ended.");
            }
		}
	}
}
