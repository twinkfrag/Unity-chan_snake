﻿using System;
using System.Linq;
using System.Xml.Linq;
using SyntaxTree.VisualStudio.Unity.Bridge;
using UnityEditor;
// ReSharper disable PossibleNullReferenceException

namespace Assets.Editor
{
	[InitializeOnLoad]
	public class VstuHook
	{
		static VstuHook()
		{
			ProjectFilesGenerator.ProjectFileGeneration += (filename, content) => ReplaceProject(content);

		}

		private static string ReplaceProject(string content)
		{
			var document = XDocument.Parse(content);
			var ns = document.Root.Name.Namespace;

			document.Root
					.Descendants()
					.Where(x => x.Name.LocalName == "Reference")
					.Where(x => (string)x.Attribute("Include") == "Boo.Lang")
					.Remove();

			var xmllinq = new XElement(ns + "Reference");
			xmllinq.SetAttributeValue("Include", "System.XML.Linq");
			document.Root
			        .Descendants()
			        .Where(x => x.Name.LocalName == "Reference")
			        .First(x => (string)x.Attribute("Include") == "System.XML")
			        .AddAfterSelf(xmllinq);

			document.Root
			        .Descendants()
			        .First(x => x.Name.LocalName == "PropertyGroup")
			        .Add(new XElement(ns + "NoWarn", "649,CSE0001"));

			return document.Declaration + Environment.NewLine + document.Root;
		}
	}
}
