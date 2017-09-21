using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagpieIOS
{
	class MagpieUser
	{
		[JsonProperty("User_ID")]
		public string uid { get; set; }

		[JsonProperty("Badge_ID")]
		public int bid { get; set; }

		[JsonProperty("isClaimed")]
		public string isClaimed { get; set; }

		public MagpieUser()
		{
			uid = "";
			bid = 0;
			isClaimed = "";
		}
	}
}
