﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MyLogger
{
	public sealed class LoggerService
	{
		private static readonly LoggerService Instance = new LoggerService();

		LoggerProvider loggerProvider = new LoggerProvider();

		private LoggerService()	{ }

		public static LoggerService GetInstance
		{
			get { return Instance; }
		}

		//-------------------------------------------------------------------------------------------------------------------------------------------------------------------

		public void RunLogger()
		{
			Console.Clear();
			Console.WriteLine("Gdzie chcesz dokonac wpisu: \nRegistry && EventViewer && File.txt -> wybierz '3' \nFile.txt -> wybierz '1'");
			char LogDestination = char.ToLower(Console.ReadKey().KeyChar);

			if (LogDestination.Equals('1') || LogDestination.Equals('3'))
			{
				Console.Clear();
				Console.WriteLine("Podaj Nazwę/Id wpisu");
				string GetLogName = Console.ReadLine();
				Console.WriteLine("Podaj treść wpisu");
				string GetText = Console.ReadLine();

				try
				{
					switch (LogDestination)
					{
						case '1':
							FileLogger(GetLogName, GetText);
							break;
						case '3':
							MultiLogger(GetLogName, GetText);
							break;
						default:
							Console.WriteLine($"Wybrano niepoprawne zrodlo zapisu: {LogDestination}");
							break;
					}
					Console.ReadKey();
					NewAction();
				}
				catch (Exception e)
				{
					Console.WriteLine("Nie udalo sie uruchomic Loggera");
					Console.WriteLine(e.Message);
				}
			}
			else
			{
				Console.Clear();
				Console.WriteLine("Nie ma takiej opcji");
				Thread.Sleep(1000);
				NewAction();
			}
				
		}

		//-------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public void FileLogger(string GetLogName, string GetText)
		{
			LoggerFactory FileFactory = loggerProvider.LoggerFactoryList.Where(z => z is TxtLoggerFactory).Select(x => x as TxtLoggerFactory).FirstOrDefault();
			ILogger FileLogger = FileFactory.CreateLogger();
			FileLogger.LogTo(GetLogName,GetText);
		}


		public void MultiLogger(string GetLogName, string GetText)
		{
			foreach (var Factory in loggerProvider.LoggerFactoryList)
			{
				ILogger ThisLogger = Factory.CreateLogger();
				ThisLogger.LogTo(GetLogName, GetText);
			}
		}

		//-------------------------------------------------------------------------------------------------------------------------------------------------------------------

		public void NewAction()
		{
			try
			{
				Console.Clear();
				Console.WriteLine("Aby zapisać kolejny log (wciśnij 'Z') \n\nAby wyświetlić wszystkie zapisane logi (wciśnij 'W')");
				char a = char.ToLower(Console.ReadKey().KeyChar);
				Console.Clear();
				switch (a)
				{
					case 'z': RunLogger(); break;
					case 'w':
						TxtLogger txtLogger = new TxtLogger();
						txtLogger.ReadFrom(); break;
					default: CloseLogger(); break;
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		//-------------------------------------------------------------------------------------------------------------------------------------------------------------------

		public void CloseLogger()
		{
			Console.Clear();
			Console.WriteLine("WYBRANO  NIEPOPRAWNA  OPCJE - NASTAPI ZAMKNIECIE");
			Thread.Sleep(1000);
			Environment.Exit(0);
		}

	}
}