using TwoBrainsGames.BrainEngine.Web.Services.ClosedBeta;
using TwoBrainsGames.BrainEngine.Web.Types;
using System.Windows.Forms;
using TwoBrainsGames.BrainEngine.Web.Services;
using Microsoft.Win32;
using System.ServiceModel;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Debugging;

namespace TwoBrainsGames.BrainEngine.Beta
{
	public class ClosedBeta
	{
        enum LoginResult
        {
            Ok,
            Error,
            BetaKeyInputCanceled
        }

        enum InitializeState
        {
            Startup,
            QueryBetaOrNormalPlay,
            QueryKey,
            Login,
            ExitGame,
            QueryLoginErrorPlayNormaly,
            StartGame
        }

		public const string ClosedBetaName = "Closed Beta";
		public static string SID { get; private set; }

		private static ClosedBetaService _wsService;
		private static DataDispatcher _dispatcher;
		
		private static DataDispatcher Dispatcher 
		{
			get 
			{
				if (ClosedBeta._dispatcher == null)
				{
					ClosedBeta._dispatcher = new DataDispatcher();
				}
				return ClosedBeta._dispatcher; 
			}
		}

		public static ClosedBetaService WSService
		{
			get
			{
				if (ClosedBeta._wsService == null)
				{
					ClosedBeta._wsService = new ClosedBetaService();
				}
				return ClosedBeta._wsService;
			}
		}

        private static bool ShouldSendData
        {
            get
            {
                return !(string.IsNullOrEmpty(ClosedBeta.SID));
            }
        }

        /// <summary>
        /// Queries the user if he wants to be part of the beta
        /// Returns true or false
        /// </summary>
        private static DialogResult QueryBeta()
        {
            BetaQueryForm form = new BetaQueryForm();
            DialogResult dr = form.ShowDialog();

            if (dr == DialogResult.OK)
            {
                BetaSettings.ShouldShowQueryBetaForm = !form.DontShowAgainChecked;
                BetaSettings.IsBetaTester = form.BetaTesterChecked;
            }
            return dr;
        }

		#region Login
		/// <summary>
		/// 
		/// </summary>
        private static LoginResult Login()
        {
			ClosedBeta.SID = null;

			bool retryLogin;
			bool askUser;
			string betaKey = null;
			do
			{
				retryLogin = false;
				askUser = false;
				LoginResponse response = null;
				try
				{
					response = (LoginResponse)AsyncServiceForm.Execute(new LoginRequest(betaKey, BrainGame.GameVersion), ClosedBeta.CallLoginService);
				}
				catch(System.Exception ex)
				{
					DialogResult dr = MessageBox.Show(null, "An error ocurred while trying to login.\nThe error was:.\n" + ex.Message + "\nRetry?", 
														ClosedBeta.ClosedBetaName, MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
					if (dr == DialogResult.Cancel)
					{
                        return LoginResult.Error;
					}
					retryLogin = true;
				}
				if (response != null)
				{
					switch (response.Status)
					{
						case LoginStatusType.Ok:
							ClosedBeta.SID = response.SID;
                            return LoginResult.Ok;

						case LoginStatusType.OkUserNeeded:
							askUser = true;
							break;
						case LoginStatusType.NotAuthorized:
							MessageBox.Show(null, "Invalid Beta Key.\nPlease check if you typed the Beta Key correctly.\nPlease note, Beta Keys are sent only by envitaion.", ClosedBeta.ClosedBetaName,
										MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							askUser = true;
							break;

						case LoginStatusType.Unknown:
							break;
					}
				}

				if (askUser)
				{
                    betaKey = ClosedBeta.AskForBetaKey();
                    if (betaKey != null)
                    {
                        retryLogin = true;
                    }
                    else
                    {
                        return LoginResult.BetaKeyInputCanceled;
                    }
				}
			}
			while(retryLogin);

			return LoginResult.Error;
		}

		/// <summary>
		/// 
		/// </summary>
		private static IResponse CallLoginService(IRequest request)
		{
			return ClosedBeta.WSService.Login((LoginRequest)request);
		}

		/// <summary>
		/// 
		/// </summary>
		private static string AskForBetaKey()
		{
			BetaKeyForm form = new BetaKeyForm();
			if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				return form.BetaKey;
			}

			return null;
		}
		#endregion

        #region Register user
        /// <summary>
        /// 
        /// </summary>
        public static void RegisterUser(string username)
        {
            RegisterUserRequest request = new RegisterUserRequest(username);
            ClosedBeta.WSService.RegisterUser(request);
        }
        

        #endregion

        #region Log


        /// <summary>
		/// 
		/// </summary>
		public static void LogInfo(string message)
		{
            if (ClosedBeta.ShouldSendData)
            {
                ClosedBeta.Dispatcher.AddItem(new LogDispatcherItem(message, LogMessageSeverityTypes.Info));
            }
		}

		/// <summary>
		/// If immediate = false, message will be sent by the dispatcher in background
		/// </summary>
		public static void LogException(System.Exception ex)
		{
            BETrace.Write(ex);
            if (ClosedBeta.ShouldSendData)
            {
                LogDispatcherItem item = new LogDispatcherItem(ex.ToString(), LogMessageSeverityTypes.Exception);
                ClosedBeta.Dispatcher.AddItem(item);
            }
		}
		#endregion

		#region Register stage stats
		/// <summary>
		/// 
		/// </summary>
		public static void RegisterStageStats(SnailsStageStats stats)
		{
            if (ClosedBeta.ShouldSendData)
            {
                ClosedBeta.Dispatcher.AddItem(new RegisterStageStatsDispatcherItem(stats));
            }
		}
		#endregion

		#region Data dispatcher
		/// <summary>
		/// 
		/// </summary>
		private static void StartDataDispatcher()
		{
			ClosedBeta.Dispatcher.Start();
		}

        /// <summary>
        /// 
        /// </summary>
        public static void EndLogging()
        {
            if (ClosedBeta.Dispatcher != null)
            {
                ClosedBeta.Dispatcher.Terminate();
            }
        }
		#endregion


        /// <summary>
        /// Who wants a nice old school state machine spagetthi code?
        /// </summary>
        public static bool Initialize()
        {
            InitializeState state = InitializeState.Startup;
            BetaSettings.Load();
            do
            {
                switch(state)
                {
                    case InitializeState.Startup:
                        if (BetaSettings.ShouldShowQueryBetaForm)
                        {
                            state = InitializeState.QueryBetaOrNormalPlay;
                        }
                        else
                        {
                            if (BetaSettings.IsBetaTester)
                            {
                                state = InitializeState.Login;
                            }
                            else
                            {
                                state = InitializeState.StartGame;
                            }
                        }
                        break;

                    case InitializeState.QueryBetaOrNormalPlay:
                        if (ClosedBeta.QueryBeta() != System.Windows.Forms.DialogResult.OK)
                        {
                            state = InitializeState.ExitGame;
                        }
                        else
                        {
                            if (BetaSettings.IsBetaTester)
                            {
                                state = InitializeState.Login;
                            }
                            else
                            {
                                state = InitializeState.StartGame;
                            }
                        }
                        break;

                    case InitializeState.Login:
                        LoginResult res = ClosedBeta.Login();
                        switch(res)
                        {
                            case LoginResult.Ok:
                                state = InitializeState.StartGame;
                                break;

                            default:
                                state = InitializeState.QueryLoginErrorPlayNormaly;
                                // Login could not be possible, make the beta form show up again
                                // user might have entered an incorrect email, give him a chance to correct that
                                BetaSettings.ShouldShowQueryBetaForm = true;
                                break;
                        }
                        break;

                    case InitializeState.QueryLoginErrorPlayNormaly:
                        LoginErrorForm errForm = new LoginErrorForm();
                        LoginErrorForm.LoginErrorFormResult res1 = errForm.ShowDialog();
                        if (errForm.DontWantToBeBetaTester == true)
                        {
                            BetaSettings.IsBetaTester = false;
                        }
                        switch (res1)
                        {
                            case LoginErrorForm.LoginErrorFormResult.Retry:
                                state = InitializeState.Login;
                                break;
                            case LoginErrorForm.LoginErrorFormResult.Quit:
                                state = InitializeState.ExitGame;
                                break;
                            case LoginErrorForm.LoginErrorFormResult.PlayNormaly:
                                state = InitializeState.StartGame;
                                break;
                        }
                        break;

                    case InitializeState.ExitGame:
                        return false;

                    case InitializeState.StartGame:
                        BetaSettings.Save();
                        if (BetaSettings.IsBetaTester)
                        {
                            ClosedBeta.StartDataDispatcher();
                        }
                        return true;
                }
/*                {
                    if (ClosedBeta.QueryBeta() != System.Windows.Forms.DialogResult.OK)
                    {
                        this.Exit();
                        return;
                    }
                }

                if (!ClosedBeta.IsBetaTester)
                {
                    return;
                }

                if (ClosedBeta.Login() == false)
                {
                    this.Exit();
                    return;
                }
                ClosedBeta.StartDataDispatcher();*/
            }
            while (true); // There's nothing like a while(true)
        }
	}
}
