using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restrict_ClickOnce
{
	class Program
	{
		static void Main(string[] args)
		{
			Microsoft.Win32.RegistryKey key;
			key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey("SOFTWARE\\MICROSOFT\\.NETFramework\\Security\\TrustManager\\PromptingLevel");
			key.SetValue("MyComputer", "AuthenticodeRequired");
			key.SetValue("LocalIntranet", "AuthenticodeRequired");
			key.SetValue("Internet", "AuthenticodeRequired");
			key.SetValue("TrustedSites", "AuthenticodeRequired");
			key.SetValue("UntrustedSites", "Disabled");
			key.Close();
		}
	}
}
