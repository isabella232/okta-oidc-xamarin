﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Okta.Xamarin
{
	/// <summary>
	/// Stores configuration for the Okta Android OIDC client
	/// </summary>
	public partial class OktaConfig : IOktaConfig
	{
		// There might be some Android-specific config in the future.  That would go here.

		/// <summary>
		/// Instantiates a <see cref="OktaConfig"/> from an XML string and validates it.  Throws an exception if required fields are missing or invalid.
		/// </summary>
		/// <param name="xml">The xml string to parse.  Please refer to the documentation or samples for examples.</param>
		/// <returns>Returns the <see cref="OktaConfig"/> with fields filled from <paramref name="xml"/>.</returns>
		public static OktaConfig ParseXml(string xml)
		{
			XDocument doc = XDocument.Parse(xml);

			OktaConfig config = new OktaConfig();

			if (doc.Element("Okta").Element("ClientId") != null)
			{
				config.ClientId = doc.Element("Okta").Element("ClientId").Value;
			}

			if (doc.Element("Okta").Element("Scope") != null)
			{
				config.Scope = doc.Element("Okta").Element("Scope").Value;
			}

			if (doc.Element("Okta").Element("OktaDomain") != null)
			{
				config.OktaDomain = doc.Element("Okta").Element("OktaDomain").Value;
			}

			if (doc.Element("Okta").Element("AuthorizationServerId") != null)
			{
				config.AuthorizationServerId = doc.Element("Okta").Element("AuthorizationServerId").Value;
			}

			if (doc.Element("Okta").Element("RedirectUri") != null)
			{
				config.RedirectUri = doc.Element("Okta").Element("RedirectUri").Value;
			}

			if (doc.Element("Okta").Element("PostLogoutRedirectUri") != null)
			{
				config.PostLogoutRedirectUri = doc.Element("Okta").Element("PostLogoutRedirectUri").Value;
			}

			if (doc.Element("Okta").Element("ClockSkew") != null &&
				int.TryParse(doc.Element("Okta").Element("ClockSkew").Value, out int clockSkewSeconds))
			{
				config.ClockSkew = TimeSpan.FromSeconds(clockSkewSeconds);
			}

			OktaConfigValidator<OktaConfig> validator = new OktaConfigValidator<OktaConfig>();
			validator.Validate(config);

			return config;
		}

		/// <summary>
		/// Instantiates a <see cref="OktaConfig"/> from an xml resource asynchronously and validates it.  Throws an exception if required fields are missing or invalid.
		/// </summary>
		/// <param name="resourceName">The xml resource filename</param>
		/// <returns>Returns a Task which returns the <see cref="OktaConfig"/> with fields filled from <paramref name="resourceName"/>.</returns>
		public static async Task<OktaConfig> LoadFromXmlStreamAsync(Stream xmlStream)
		{
		//	var allre = Assembly.GetExecutingAssembly().GetManifestResourceNames();

			using (StreamReader reader = new StreamReader(xmlStream))
			{
				return ParseXml(await reader.ReadToEndAsync());
			}
		}

		/// <summary>
		/// Instantiates a <see cref="OktaConfig"/> from an xml file asynchronously and validates it.  Throws an exception if required fields are missing or invalid.
		/// </summary>
		/// <param name="filename">The file containing xml to parse.  This is treated as a simple file path.  If you are attempting to load an embedded resource you should use <see cref="LoadFromXmlResourceAsync(string)"/> instead.</param>
		/// <returns>Returns a Task which returns the <see cref="OktaConfig"/> with fields filled from <paramref name="filename"/>.</returns>
		public static async Task<OktaConfig> LoadFromXmlFileAsync(string filename)
		{
			using (StreamReader reader = File.OpenText(filename))
			{
				return ParseXml(await reader.ReadToEndAsync());
			}
		}
	}
}