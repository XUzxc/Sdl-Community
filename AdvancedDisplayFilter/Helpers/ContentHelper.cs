﻿using System;
using System.Text.RegularExpressions;

namespace Sdl.Community.Plugins.AdvancedDisplayFilter.Helpers
{
	public static class ContentHelper
	{
		public static bool SearchContentRegularExpression(string text, string regexRule, RegexOptions regexOptions)
		{
			try
			{
				var regex = new Regex(regexRule, regexOptions);
				var match = regex.Match(text);

				//if the words matching the rule is not found return the segment
				if (match.Success)
				{
					return true;
				}
			}
			catch (Exception e)
			{
				// ignored
			}
			return false;
		}
	}
}
