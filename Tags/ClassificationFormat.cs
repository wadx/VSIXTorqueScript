
using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace TorqueScriptLanguage
{
	#region Format definition

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = "function")]
	[Name("function")]
	[UserVisible(false)]
	[Order(Before = Priority.Default)]
	internal sealed class TorqueScriptF : ClassificationFormatDefinition
	{
		public TorqueScriptF()
		{
			DisplayName = "function"; //human readable version of the name
			ForegroundColor = Colors.Orange;
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = "new")]
	[Name("new")]
	[UserVisible(false)]
	[Order(Before = Priority.Default)]
	internal sealed class TorqueScriptN : ClassificationFormatDefinition
	{
		public TorqueScriptN()
		{
			DisplayName = "new"; //human readable version of the name
			ForegroundColor = Colors.Orange;
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = "singleton")]
	[Name("singleton")]
	[UserVisible(false)]
	[Order(Before = Priority.Default)]
	internal sealed class TorqueScriptS : ClassificationFormatDefinition
	{
		public TorqueScriptS()
		{
			DisplayName = "singleton"; //human readable version of the name
			ForegroundColor = Colors.Orange;
		}
	}

	[Export(typeof(EditorFormatDefinition))]
	[ClassificationType(ClassificationTypeNames = "datablock")]
	[Name("datablock")]
	[UserVisible(false)]
	[Order(Before = Priority.Default)]
	internal sealed class TorqueScriptD : ClassificationFormatDefinition
	{
		public TorqueScriptD()
		{
			DisplayName = "datablock"; //human readable version of the name
			ForegroundColor = Colors.Orange;
		}
	}

	#endregion //Format definition
}

