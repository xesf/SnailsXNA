using TwoBrainsGames.BrainEngine.Web.Types;
using System.Net.NetworkInformation;
using System;
using System.ServiceModel;
using TwoBrainsGames.BrainEngine.ClosedBetaWS;
using TwoBrainsGames.BrainEngine.Debugging;

namespace TwoBrainsGames.BrainEngine.Web.Services.ClosedBeta
{
	public class ClosedBetaService
	{
		
		ClosedBetaPortTypeClient Client { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ClosedBetaService()
        {
            this.ConfigureEndPoint();
        }
        
        /// <summary>
        /// Endpoint configuration - hardcode address for now
        /// </summary>
        private void ConfigureEndPoint()
        {
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.CloseTimeout = new System.TimeSpan(0, 0, 30);
            binding.OpenTimeout = new System.TimeSpan(0, 1, 0);
            binding.ReceiveTimeout = new System.TimeSpan(0, 1, 0);

            EndpointAddress endpoint =  new EndpointAddress("http://twobrainsgames.xesf.net/snails/services/server-closedbeta.php");
            this.Client = new ClosedBetaPortTypeClient(binding, endpoint);
        }

		#region Login method
		/// <summary>
		/// 
		/// </summary>
		public LoginResponse Login(LoginRequest request)
		{
			// Get Macs
			string mac = this.GetMacAddress();
			string statusMessage;
			string sid;

            BETrace.WriteLine("ClosedBetaWS.Login() request: Mac[{0}], Country[{1}], GameVersion[{2}], OSVersion[{3}]",
                                mac, request.Country, request.GameVersion, request.OsVersion);

            int rc = this.Client.Login(Translator.DateTimeToString(DateTime.Now), mac, request.BetaKey, request.Country, request.GameVersion, request.OsVersion, out statusMessage, out sid);
            BETrace.WriteLine("ClosedBetaWS.Login() response: rc[{0}] message[{1}]", rc, statusMessage);
            
			return new LoginResponse(this.ConvertStatus(rc), statusMessage, sid);
		}

		/// <summary>
		/// 
		/// </summary>
		private string GetMacAddress()
		{
			NetworkInterface [] adapters = NetworkInterface.GetAllNetworkInterfaces();
			if (adapters.Length == 0)
			{
				throw new BrainEngine.Web.BEWebException("No network adapter was found.");
			}
            foreach (NetworkInterface adapter in adapters)
            {
                PhysicalAddress addr = adapter.GetPhysicalAddress();
                if (addr != null &&
                    !string.IsNullOrEmpty(addr.ToString()))
                {
                    return addr.ToString();
                }
            }
			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		private LoginStatusType ConvertStatus(int rc)
		{
			switch (rc)
			{
				case 0:
					return LoginStatusType.Ok;
				case 1:
					return LoginStatusType.OkUserNeeded;
				case 2:
					return LoginStatusType.NotAuthorized;
                case 3:
                    return LoginStatusType.InvalidParams;
            }

			return LoginStatusType.Unknown;
		}
		#endregion

        #region RegisterUser method
        /// <summary>
        /// 
        /// </summary>
        public RegisterUserResponse RegisterUser(RegisterUserRequest request)
        {
            string statusMessage;
            BETrace.WriteLine("ClosedBetaWS.RegisterUser() request: Username[{0}]", request.Username);
            int rc = this.Client.RegisterUser(request.Username, out statusMessage);
            BETrace.WriteLine("ClosedBetaWS.RegisterUser() response: rc[{0}] StatusMessage[{1}]", rc, statusMessage);

            LoginStatusType status = this.ConvertStatus(rc);
            if (status != LoginStatusType.Ok)
            {
                throw new BEWebException(statusMessage);
            }
            return new RegisterUserResponse();
        }

        #endregion

        #region Log method
        /// <summary>
		/// 
		/// </summary>
		public LogRegisterResponse Log(LogRegisterRequest request)
		{
			string msgStatus;
            BETrace.WriteLine("ClosedBetaWS.RegisterLog() request");
        	int rc = this.Client.RegisterLog(request.Sid, Translator.DateTimeToString(request.Timestamp),(int)request.Severity, request.Message, out msgStatus);
            BETrace.WriteLine("ClosedBetaWS.RegisterLog() response: rc[{0}] StatusMessage[{1}]", rc, msgStatus);

			return new LogRegisterResponse(this.ConvertLogRegisterStatus(rc), msgStatus);
		}


		/// <summary>
		/// 
		/// </summary>
		private LogRegisterStatusTypes ConvertLogRegisterStatus(int rc)
		{
			switch (rc)
			{
				case 0:
					return LogRegisterStatusTypes.Ok;
			}

			return LogRegisterStatusTypes.Error;
		}
		#endregion

		#region RegisterStageStats method
		/// <summary>
		/// 
		/// </summary>
		public RegisterStageStatsResponse RegisterStageStats(RegisterStageStatsRequest request)
		{
			string statusMsg;
            BETrace.WriteLine("ClosedBetaWS.RegisterStageStats() request");
        	int rc = this.Client.RegisterStageStats(request.SID, 
													request.Theme.ToString(), 
													request.StageNumber,
													Translator.DateTimeToString(request.TimeStarted),
													request.Status,
													request.Goldcoins,
													request.Silvercoins,
													request.Bronzecoins,
													request.Score,
													request.Unusedtools,
													Translator.DateTimeToString(request.TimeTaken),
													request.IsNewHighScore,
													request.Medal,
													request.StageBuildNr,
													out statusMsg);

			BETrace.WriteLine("ClosedBetaWS.RegisterStageStats() response: rc[{0}] StatusMessage[{1}]", rc, statusMsg);
        	return new RegisterStageStatsResponse(this.ConvertRegisterStageStatsStatus(rc), statusMsg);
		}

		/// <summary>
		/// 
		/// </summary>
		private RegisterStageStatsStatusType ConvertRegisterStageStatsStatus(int rc)
		{
			switch (rc)
			{
				case 0:
					return RegisterStageStatsStatusType.Ok;
			}

			return RegisterStageStatsStatusType.Error;
		}
		#endregion
	}
}
