using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.InteropServices;
using System.Threading;
using System.Text;
using System.Text.RegularExpressions;

namespace ProvaDLL
{
	/// <summary>
	/// Descrizione di riepilogo per Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
    {
        private Button button1;
		/// <summary>
		/// Variabile di progettazione necessaria.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public Form1()
		{
			InitializeComponent();
        }

		/// <summary>
		/// Pulire le risorse in uso.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Codice generato da Progettazione Windows Form
		/// <summary>
		/// Metodo necessario per il supporto della finestra di progettazione. Non modificare
		/// il contenuto del metodo con l'editor di codice.
		/// </summary>
		private void InitializeComponent()
		{
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(208, 47);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Read";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(295, 82);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// Il punto di ingresso principale dell'applicazione.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Dll version: " + KDLL.CSReqDllVersion();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            KDLL.CSSetChannel(1, ",,,,,,,192.168.4.100,3000,,,1,1");
            if (KDLL.CSOpenChannel(1) == 0)
            {
                String strData = KDLL.CSReqDateTime(1, 1);
                                
                KDLL.CSCloseChannel(1);
                MessageBox.Show("Data:" + strData);
            }

            if (KDLL.CSOpenChannel(1) == 0)
            {
                String strFW = KDLL.CSReqFWVersion(1, 1);

                KDLL.CSCloseChannel(1);

                string FW = KDLL.HexAsciiConvert(strFW);

                MessageBox.Show("Versione Fw:" + FW);
            }

            if (KDLL.CSOpenChannel(1) == 0)
            {
                String strFW = KDLL.ReqTransactNum(1, 1);

                KDLL.CSCloseChannel(1);

                string FW = KDLL.HexAsciiConvert(strFW);

                MessageBox.Show(FW);
            }
        }

        
	}


    static class KDLL
    {
        // C# to convert a string to a byte array
        private static byte[] StrToByteArray(string str)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            return encoding.GetBytes(str);
        }


        
        // import DLL functions from unsafe C++ dll
        [DllImport("kdll.dll", EntryPoint = "ReqDllVersion", SetLastError = true)]
        static private unsafe extern void ReqDllVersion(byte* pbAnswer);
        [DllImport("kdll.dll", EntryPoint = "ReqFWVersion", SetLastError = true)]
        static private unsafe extern void ReqFWVersion(int iChn, int iDev, byte* pbAnswer);
        [DllImport("kdll.dll", EntryPoint = "ReqDateTime", SetLastError = true)]
        static private unsafe extern void ReqDateTime(int iChn, int iDev, byte* pbAnswer);
        [DllImport("kdll.dll", EntryPoint = "ReqTransactNum", SetLastError = true)]
        static private unsafe extern void ReqTransactNum(int iChn, int iDev, byte* pbAnswer);
        [DllImport("kdll.dll", EntryPoint = "SetChannel", SetLastError = true)]
        static private unsafe extern int SetChannel(int iChn, byte* pbParms);
        [DllImport("kdll.dll", EntryPoint = "OpenChannel", SetLastError = true)]
        static private unsafe extern int OpenChannel(int iChn);
        [DllImport("kdll.dll", EntryPoint = "CloseChannel", SetLastError = true)]
        static private unsafe extern int CloseChannel(int iChn);

        // C# expose class methods (i.e. dll functions)
        static public unsafe string CSReqDllVersion()
        {
            byte[] abChars = new byte[256];

            fixed (byte* pbChars = abChars)
            {
                ReqDllVersion(pbChars);
            }

            return System.Text.Encoding.ASCII.GetString(abChars, 0, abChars.Length);
        }
        static public unsafe string CSReqFWVersion(int iChn, int iDev)
        {
            byte[] abChars = new byte[256];

            fixed (byte* pbChars = abChars)
            {
                ReqFWVersion(iChn, iDev, pbChars);
            }

            return System.Text.Encoding.ASCII.GetString(abChars, 0, abChars.Length);
        }

        static public unsafe string ReqTransactNum(int iChn, int iDev)
        {
            byte[] abChars = new byte[256];

            fixed (byte* pbChars = abChars)
            {
                ReqTransactNum(iChn, iDev, pbChars);
            }

            return System.Text.Encoding.ASCII.GetString(abChars, 0, abChars.Length);
        }

        static public unsafe string HexAsciiConvert(string hex)
        {

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i <= hex.Length - 2; i += 2)
            {
                try
                {
                    sb.Append(Convert.ToString(Convert.ToChar(Int32.Parse(hex.Substring(i, 2), System.Globalization.NumberStyles.HexNumber))));
                }
                catch { };

            }

            return sb.ToString();

        }

        static public unsafe string CSReqDateTime(int iChn, int iDev)
        {
            byte[] abChars = new byte[256];

            fixed (byte* pbChars = abChars)
            {
                ReqDateTime(iChn, iDev, pbChars);
            }

            return System.Text.Encoding.ASCII.GetString(abChars, 0, abChars.Length);
        }

        static public unsafe int CSSetChannel(int iChn, String strParms)
        {
            int iAns;
            byte[] abChars = new byte[256];

            abChars = StrToByteArray(strParms);
            fixed (byte* pbChars = abChars)
            {
                iAns = SetChannel(iChn, pbChars);
            }

            return iAns;
        }
        static public unsafe int CSOpenChannel(int iChn)
        {
            return OpenChannel(iChn);
        }
        static public unsafe int CSCloseChannel(int iChn)
        {
            return CloseChannel(iChn);
        }

        
    }


}
