
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace TorqueScriptLanguage
{
	internal static class OrdinaryClassificationDefinition
	{
		#region Type definition

		[Export(typeof(ClassificationTypeDefinition))]
		[Name("function")]
		internal static ClassificationTypeDefinition torqueScriptFunction = null;

		[Export(typeof(ClassificationTypeDefinition))]
		[Name("new")]
		internal static ClassificationTypeDefinition torqueScriptNew = null;

		[Export(typeof(ClassificationTypeDefinition))]
		[Name("singleton")]
		internal static ClassificationTypeDefinition torqueScriptSingleton = null;

		[Export(typeof(ClassificationTypeDefinition))]
		[Name("datablock")]
		internal static ClassificationTypeDefinition torqueScriptDatablock = null;

		#endregion
	}
}
