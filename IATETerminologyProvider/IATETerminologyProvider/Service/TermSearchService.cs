﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IATETerminologyProvider.Helpers;
using IATETerminologyProvider.Model;
using IATETerminologyProvider.Model.ResponseModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Sdl.Core.Globalization;
using Sdl.Terminology.TerminologyProvider.Core;

namespace IATETerminologyProvider.Service
{
	public class TermSearchService
	{
		private ProviderSettings _providerSettings;
		public TermSearchService(ProviderSettings providerSettings)
		{
			_providerSettings = providerSettings;
		}
	
		public IList<ISearchResult> GetTerms(string text, ILanguage source, ILanguage destination, int maxResultsCount)
		{
			// maxResults (the number of returned words) value is set from the Termbase -> Search Settings
			var client = new RestClient(ApiUrls.BaseUri("true", _providerSettings.Offset.ToString(), maxResultsCount.ToString()));

			// _providerSettings.Offset (the number of returned words) is set from the Provider Settings grid
			//var client = new RestClient(ApiUrls.BaseUri("true", _providerSettings.Offset.ToString(), _providerSettings.Limit.ToString()));
			var request = new RestRequest("", Method.POST);
			request.AddHeader("Connection", "Keep-Alive");
			request.AddHeader("Cache-Control", "no-cache");
			request.AddHeader("Pragma", "no-cache");
			request.AddHeader("Accept", "application/json");
			request.AddHeader("Accept-Encoding", "gzip, deflate, br");
			request.AddHeader("Content-Type", "application/json");
			request.AddHeader("Origin", "https://iate.europa.eu");
			request.AddHeader("Host", "iate.europa.eu");			
			request.AddHeader("Access-Control-Allow-Origin", "*");

			var bodyModel = SetApiRequestBodyValues(destination, source, text);
			request.AddJsonBody(bodyModel);

			var response = client.Execute(request);
			
			var result = MapResponseValues(response);
			return result;
		}

		// Set the needed fields for the API search request
		private object SetApiRequestBodyValues(ILanguage destination, ILanguage source, string text)
		{
			var targetLanguges = new List<string>();
			targetLanguges.Add(destination.Locale.TwoLetterISOLanguageName);

			var bodyModel = new
			{
				query = text,
				source = source.Locale.TwoLetterISOLanguageName,
				targets = targetLanguges,
				include_subdomains = true
			};
			return bodyModel;
		}

		private IList<ISearchResult> MapResponseValues(IRestResponse response)
		{
			var termsList = new List<ISearchResult>();

			var jObject = JObject.Parse(response.Content);
			var itemTokens = (JArray)jObject.SelectToken("items");
			if (itemTokens != null)
			{
				foreach (var item in itemTokens)
				{
					// get language childrens (source + target languages)
					var languageTokens = item.SelectToken("language").Children().ToList();
					if(languageTokens.Any())
					{
						//remove the first token(which corresponds to the Source Language)
						languageTokens.Remove(languageTokens[0]);

						// foreach language token remained(which represents the target languages) get the terms
						foreach(JProperty languageToken in languageTokens)
						{
							var termEntry = languageToken.FirstOrDefault().SelectToken("term_entries").Last;
							var termValue = termEntry.SelectToken("term_value").ToString();
							var termId = termEntry.SelectToken("id").ToString();
							var langTwoLetters = languageToken.Name;
							var languageModel = new LanguageModel
							{
								Name = new Language(langTwoLetters).DisplayName,
								Locale = new Language(langTwoLetters).CultureInfo
							};		

							int result;
							var termResult = new SearchResult
							{
								Text = termValue,
								Id = int.TryParse(termId, out result) ? int.Parse(termId) : 0,
								Score = 100,
								Language = languageModel
							};
							termsList.Add(termResult);
						}
					}
				}
			}

			return termsList;
		}
	}
}