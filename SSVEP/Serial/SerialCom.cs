﻿using System;
using System.IO.Ports;
using System.Management;
using System.Threading;

namespace WpfApplication1
{
	class SerialCom
	{
		private SerialPort port1;
		private int bRate = 9600;
		private LED[] leds;
		private int lednum = 8;
		private int[] ledstatus;

		Random rnd = new Random();
		public event EventHandler<LED_StatusEventArgs> LEDUpdate;
		public event EventHandler<LED_FinishEventArgs> LEDFinish;
		public SerialCom()
		{
			leds = new LED[lednum];
			ledstatus = new int[lednum];

			Console.WriteLine(AutodetectArduinoPort());
			port1 = new SerialPort();
			port1.PortName = AutodetectArduinoPort();
			port1.BaudRate = bRate;
			port1.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
			port1.Open();
			Thread.Sleep(10);
			setupColour(colourset.GREEN);
			Thread.Sleep(10);
			all_blink(0);
			Thread.Sleep(10);
			all_dim();
			Thread.Sleep(10);

		}

		public void setupColour(int setcolour)
		{
			for (int i = 0; i < lednum; i++)
			{
				leds[i] = new LED(i, setcolour, ref port1);
				leds[i].setupLED();
				Thread.Sleep(10);
			}
		}

		public void changeColour(int led, int setcolour)
		{
			leds[led].changecolour(setcolour);
			Thread.Sleep(10);
		}

		public void changeColourall(int setcolour)
		{
			for (int i = 0; i < lednum; i++)
			{
				leds[i].changecolour(setcolour);
				Thread.Sleep(10);
			}
		}

		public void all_on()
		{
			for (int i = 0; i < lednum; i++)
			{
				leds[i].turnon();
				Thread.Sleep(10);
			}
		}

		public void all_off()
		{
			for (int i = 0; i < lednum; i++)
			{
				leds[i].blackout();
				Thread.Sleep(10);
			}
		}

		public void all_dim()
		{
			for (int i = 0; i < lednum; i++)
			{
				leds[i].turnoff();
				Thread.Sleep(10);
			}
		}

		public void all_blink(int freq)
		{
			for (int i = 0; i < lednum; i++)
			{
				leds[i].turnoff();
				Thread.Sleep(10);
				leds[i].blink(freq);
				Thread.Sleep(10);
			}
		}
		public void blinking(int led, int freq)
		{
			leds[led].turnoff();
			Thread.Sleep(10);
			leds[led].blink(freq);
			Thread.Sleep(10);
		}


		public void OnLEDStatusUpdate(int i, int led)
		{
			if (LEDUpdate != null)
				LEDUpdate(this, new LED_StatusEventArgs(led, i));
		}

		public void OnLEDFinish(bool fin)
		{
			if (LEDFinish != null)
				LEDFinish(this, new LED_FinishEventArgs(fin));
		}

		private string AutodetectArduinoPort()
		{
			ManagementScope connectionScope = new ManagementScope();
			SelectQuery serialQuery = new SelectQuery("SELECT * FROM Win32_PnPEntity WHERE Caption like '%(COM%'");
			ManagementObjectSearcher searcher = new ManagementObjectSearcher(connectionScope, serialQuery);
			string[] ArrayComPortsNames = SerialPort.GetPortNames();



			try
			{
				foreach (ManagementObject item in searcher.Get())
				{
					string desc = item["Description"].ToString();
					string deviceId = item["Caption"].ToString();

					if (desc.Contains("CH340")||desc.Contains("Arduino"))
					{						
						return deviceId.Split('(')[1].TrimEnd(')');
					}
				}
			}
			catch (ManagementException e)
			{
				throw new NotConnectException();
			}

			return null;
		}
		private void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
		{
			//Write the serial port data to the console.
			Console.WriteLine(port1.ReadExisting());

		}

		public void turnonEquipment()
		{
			port1.Write("E:" + 1 + "&" + "1" + "#");
		}

		public void turnoffEquipment()
		{
			port1.Write("E:" + 0 + "&" + "0" + "#");
		}
	}



	public class LED_StatusEventArgs : EventArgs
	{
		public int status;
		public int cycle;

		public LED_StatusEventArgs(int ledstatus, int cycle)
		{
			this.cycle = cycle;
			status = ledstatus;
		}
	}
	public class LED_FinishEventArgs : EventArgs
	{
		public bool finish;

		public LED_FinishEventArgs(bool fin)
		{
			finish = fin;
		}
	}


}
