using System;
using System.Threading.Tasks;
using TS3AudioBot;
using TS3AudioBot.Audio;
using TS3AudioBot.CommandSystem;
using TS3AudioBot.Plugins;
using TSLib.Full.Book;
using TSLib;
using TSLib.Full;
using TSLib.Messages;
using System.Collections.Generic;
//using System.Threading;

namespace AfkChecker
{
	public class CheckAfk : IBotPlugin
	{
		private TsFullClient tsFullClient;
		//private PlayManager playManager;
		private Ts3Client ts3Client;
		private Connection serverView;
		private bool TimerOn = false;
		private int AFKTime = 60; // in Minutes
		private int AmongAFKTime = 120; // in Minutes
		private int AFKNotice; // in Minutes
		private int AmongAFKNotice; // in Minutes
							   // replace with the IDs of the excluded groups
							   //ivate int BotServerGroup = 11;
							   //private static Timer timer;

		//public static Dictionary<ClientId, Client> Clients { get; private set; }

		// Your dependencies will be injected into the constructor of your class.
		public CheckAfk(Ts3Client ts3Client, Connection serverView, TsFullClient tsFull)
		{
			//this.playManager = playManager;
			this.ts3Client = ts3Client;
			this.tsFullClient = tsFull;
			this.serverView = serverView;
		}

		// The Initialize method will be called when all modules were successfully injected.
		public void Initialize()
		{
			// (ChannelId)18
			AFKNotice = AFKTime - 1;
			AmongAFKNotice = AmongAFKTime - 1;
			Console.WriteLine("Starting AFK Service - AFK Time: " + AFKTime + " AFK Notice: " + AFKNotice);

			//timer2 = new Timer(TimerCallback, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
			//Timer timer = new Timer();
			//timer.Interval = 10000; // 10 seconds
			//timer.Elapsed += Timer_Elapsed;
			//timer.Start();
			//playManager.AfterResourceStarted += Start;
			//playManager.PlaybackStopped += Stop;
			//setChannelCommander();

			//tsFullClient.OnClientEnterView += OnUserConnected;
			//tsFullClient.OnClientLeftView += OnUserDisconnected;
			//tsFullClient.OnClientMoved += OnUserMoved;
			TimerOn = true;
			StartTimerLoop();
			//UserIdleCheck();

		}

		private async void StartTimerLoop()
		{
			while (TimerOn)
			{
				UserIdleCheck();
				//Console.WriteLine("Executed method - " + DateTime.Now);
				await Task.Delay(30000); // Wait for 10 seconds before executing again
			}
		}

		public async void UserIdleCheck()
		{
			try { 
				var allConnectedClients = serverView.Clients;
				foreach (var client in allConnectedClients)
				{
					// If is in Bot Server Group Ignore
					ServerGroupId serverGroupId = (ServerGroupId)11;
					ServerGroupId serverGroupId2 = (ServerGroupId)69;
					if (client.Value.ServerGroups.Contains(serverGroupId) || client.Value.ServerGroups.Contains(serverGroupId2))
					{
						//Console.WriteLine("User is Bot: " + client.Value.Name);
					}
					else
					{
						// If Channel has more that 1 user
						if (GetUserCountFromChannelId(client.Value.Channel) > 1)
						{
							//Console.WriteLine("Usercount in channel: " + GetUserCountFromChannelId(client.Value.Channel));

							// Check if already in AFK Channel
							if (client.Value.Channel.ToString() == "18")
							{
								//Console.WriteLine("Already in Afk channel");
							}
							else
							{
								var ci = await ts3Client.GetClientInfoById(client.Value.Id);
								//var ci = await tsFullClient.ClientInfo(client.Value.Id);
								if (ci.ClientIdleTime == null)
								{
									//Console.WriteLine("Full Client Empty " + client.Value.Name);
								}
								else
								{
									TimeSpan ts = ci.ClientIdleTime;
									double totalMinutesAfk = Math.Round(ts.TotalMinutes);
									//Console.WriteLine("Real Client and not in AFK Channel " + client.Value.Name + " Timespan: " + ts.ToString());
									// Check if in Among us channel
									//if (client.Value.Channel.ToString() == "460" || client.Value.Channel.ToString() == "461")
									//{
									// In Among us channels AmongAFKTime
									//if (ts.Minutes >= AmongAFKNotice && ts.Minutes <= AmongAFKTime)
									//{
									//Console.WriteLine(client.Value.Name + " is " + ts.Minutes + " minutes Idle");
									//Console.WriteLine("Sending "+ client.Value.Name+" a Notice");
									// move to afk Channel
									//ts3Client.
									//await tsFullClient.ClientMove(client.Value.Id, (ChannelId)18);
									//await tsFullClient.SendPrivateMessage("[b][color=red]🚨 Attention![/color][/b] Please note, if there's no activity in the next minute, you'll be moved to the AFK channel. Feel free to type or type to stay active!", client.Value.Id);

									//client.Value.Channel = (ChannelId)18;
									//}
									//if (ts.Minutes >= AmongAFKTime)
									//{
									//Console.WriteLine(client.Value.Name + " is " + ts.Minutes + " minutes Idle");
									//Console.WriteLine("Sending " + client.Value.Name + " to AFK Channel");
									// move to afk Channel
									//ts3Client.
									//await tsFullClient.ClientMove(client.Value.Id, (ChannelId)18);
									//await tsFullClient.PokeClient("Moved to AFK: No activity for 2 hours. Join when ready!", client.Value.Id);

									//client.Value.Channel = (ChannelId)18;
									//}
									//Console.WriteLine("Already in Afk channel");
									//}
									//else
									//{
									//Console.WriteLine(client.Value.Name + " is " + totalMinutesAfk + " minutes Idle - Notice at: " + AFKNotice+" and AFK at: "+ AFKTime);
									if (totalMinutesAfk >= AFKNotice && totalMinutesAfk <= AFKTime)
									{
										//Console.WriteLine(client.Value.Name + " is " + totalMinutesAfk + " minutes Idle");
										//Console.WriteLine("Sending "+ client.Value.Name+" a Notice");
										// move to afk Channel
										//ts3Client.
										//await tsFullClient.ClientMove(client.Value.Id, (ChannelId)18);
										await tsFullClient.SendPrivateMessage("[b][color=red]🚨 Attention![/color][/b] Please note, if there's no activity in the next minute, you'll be moved to the AFK channel. Feel free to talk or type to stay active!", client.Value.Id);

										//client.Value.Channel = (ChannelId)18;
									}
									if (totalMinutesAfk >= AFKTime)
									{
										//Console.WriteLine(client.Value.Name + " is " + totalMinutesAfk + " minutes Idle");
										Console.WriteLine("Sending " + client.Value.Name + " to AFK Channel");
										// move to afk Channel
										//ts3Client.
										await tsFullClient.ClientMove(client.Value.Id, (ChannelId)18);
										await tsFullClient.PokeClient("Moved to AFK: No activity for 1 hour. Join when ready!", client.Value.Id);

										//client.Value.Channel = (ChannelId)18;
									//}
									}
								}
							}
						}
					}
				}
			}
			catch(Exception e)
			{
				Console.WriteLine("Error: "+e.Message);
			}
		}

		private async Task<List<int>> GetUsersInChannel(int ChanId)
		{
			var list = new List<int>();
			string usrId;
			foreach (var client in serverView.Clients)
			{
				if (((int)client.Value.Channel.Value) == ChanId)
				{
					usrId = client.Value.Id.ToString();
					list.Add(Convert.ToInt32(usrId));
				}
				//Console.WriteLine(client.Value.Id + " is in channel: " + client.Value.Channel.Value.ToString());
			}
			return list;
		}

		private int GetUserCountFromChannelId(ChannelId ChanId)
		{
			int count = 0;

			var allConnectedClients = serverView.Clients;

			foreach (var client in allConnectedClients)
			{
				ServerGroupId serverGroupId = (ServerGroupId)11;
				if (client.Value.ServerGroups.Contains(serverGroupId))
				{
					//Console.WriteLine("User is Bot: " + client.Value.Name);
				}
				else
				{
					if (client.Value.Channel == ChanId)
					{
						count++;
					}
				}

				//Console.WriteLine(client.Value.Id + " is in channel: " + client.Value.Channel.Value.ToString());
			}

			return count;
		}


		private void OnUserConnected(object sender, IEnumerable<ClientEnterView> clients)
		{

		}

		private void OnUserDisconnected(object sender, IEnumerable<ClientLeftView> clients)
		{

		}

		private void OnUserMoved(object sender, IEnumerable<ClientMoved> clients)
		{
			foreach (var client in clients)
			{
				Console.WriteLine("Client "+client.InvokerName+" moved to: "+ client.TargetChannelId);
			}
		}

		[Command("afk")]
		public static string CommandAFK()
		{
			string outputString = "empty";
			//foreach (var client in Clients)
			//{
			//	outputString = outputString + "Client: " + client.Value.Name + " Idle time: " + client.Value.ConnectionData.IdleTime;
			//	Console.WriteLine("Client: " + client.Value.Name + " Idle time: " + client.Value.ConnectionData.IdleTime);
			//}

			return outputString;
		}

		public void Dispose()
		{
			TimerOn = false;
			// Don't forget to unregister everything you have subscribed to,
			// otherwise your plugin will remain in a zombie state
			//playManager.AfterResourceStarted -= Start;
			//playManager.PlaybackStopped -= Stop;
		}
	}
}
