﻿using System;
using System.Threading;
using Frequencytest.Logger;
using Frequencytest.Serial;
using System.Threading.Tasks;
using System.IO;

namespace Frequencytest
{
	class Program
	{
		private static EEG_Logger p;
		private static SerialCom s;

		private bool connect = false;
		private Thread loggerThread;
		private Thread LEDThread;



		private static int lednum = 8;

		static System.Media.SoundPlayer finishSound = new System.Media.SoundPlayer(@"c:\\Windows\\media\\Windows Proximity Notification.wav");
		static void ClearLine()
		{
			Console.SetCursorPosition(0, Console.CursorTop);
			Console.Write(new string(' ', Console.WindowWidth));
			Console.SetCursorPosition(0, Console.CursorTop - 1);
		}

		static void countDown(int time)
		{
			for (int i = time; i >= 0; --i)
			{
				Console.Write("Time: {0}", i);
				Thread.Sleep(1000);
				ClearLine();
			}
		}

		void connectEngin()
		{
			if (!connect)
			{
				for (int i = 0; i < 2; i++)
				{
					Console.WriteLine(i);
					try
					{
						if (loggerThread == null)
						{
							loggerThread = new Thread(p.Run);
							loggerThread.Start();
						}
						Thread.Sleep(100);
					}
					catch (NotConnectException error)
					{
						continue;
					}
					catch (TaskCanceledException error)
					{
						return;
					}
				}
			}
		}
		static void Main(string[] args)
		{

			#region strip
			//String folder = ".\\" + "data" + "\\";

			//string[] fileEntries = Directory.GetFiles(folder);
			//foreach (string fileName in fileEntries)
			//{
			//	String filename = Path.GetFileName(fileName);
			//	if (filename.EndsWith(".csv"))
			//	{
			//		strip s = new strip(folder, filename.TrimEnd(".csv".ToCharArray()));
			//	}
			//}

			//Console.ReadKey();

			#endregion

			#region time and space
			s = new SerialCom();
			// LD,D , RD , R , UR , U , LU , L
			int[] freqset = new int[] { 0, 13, 0, 15, 0, 17, 0, 14 };



			int starttime = 6;

			int time = 15;
			int freq = 16;


			String Prefix = "WinT";

			#region init
			try
			{
				p = new EEG_Logger(1 + starttime + time, freq, Prefix);

				for (int i = 0; i < 2; i++)
				{
					p.connect();
					Thread.Sleep(500);
				}
				#endregion

				#region Pre
				Thread.Sleep(2);

				p.setMarker(1);
				Thread.Sleep(starttime * 1000);
				p.setMarker(1);
				#endregion

				//#region Dual colour
				//	//s.changeColour(0, colourset.GREEN);
				//	//s.blinking(0, freq);
				//	//p.setMarker(freq);
				//	//Thread.Sleep((time / 2) * 1000);

				//	//s.changeColour(0, colourset.RED);
				//	//s.blinking(0, freq);
				//	//p.setMarker(freq);
				//	//Thread.Sleep((time / 2) * 1000);

				//	//p.setMarker(freq);
				//	#endregion

				#region Single colour
				//s.changeColour(0, colourset.RED);
				s.changeColour(0, colourset.GREEN);
				for (int i = 0; i < lednum; i++)
				{
					s.blinking(i, freqset[i]);
					Thread.Sleep(3);
				}

				p.setMarker(freq);

				Thread.Sleep(time * 1000);
				p.setMarker(freq);

				#endregion
				Thread.Sleep(100);
				s.all_blink(0);
				Thread.Sleep(10);
				s.all_off();
				p.Run();
				//s.changeColour(0, colourset.GREEN);



				Thread.Sleep(500);
			}
			catch (NotConnectException e)
			{
				Console.WriteLine("not connect");
			}

		//}
		Thread.Sleep(5);
					s.all_off();
				#endregion
		//		//Console.WriteLine("End");
		//Console.ReadLine();
	}
}

	}