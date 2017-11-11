using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SharePoint
{
	public static class SpFile
	{
		public const string WebFilesPath = @"Technology\WebsiteFiles\";
		public const string FileMask = "000000";
		public const string FileDelimiter = "___";

		public static int preCharsLength = FileMask.Length + FileDelimiter.Length;

		public static int PreCharsLength
		{
			get
			{
				return preCharsLength;
			}

			set
			{
				preCharsLength = value;
			}
		}
	}
}