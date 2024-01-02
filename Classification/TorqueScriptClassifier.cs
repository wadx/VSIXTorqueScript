
namespace TorqueScriptLanguage
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.Composition;
	using System.Diagnostics;
	using Microsoft.VisualStudio.Text;
	using Microsoft.VisualStudio.Text.Classification;
	using Microsoft.VisualStudio.Text.Editor;
	using Microsoft.VisualStudio.Text.Tagging;
	using Microsoft.VisualStudio.Utilities;
	using Microsoft.VisualStudio.Language.StandardClassification;

	[Export(typeof(ITaggerProvider))]
	[ContentType("TorqueScript")]
	[TagType(typeof(ClassificationTag))]
	internal sealed class TorqueScriptClassifierProvider : ITaggerProvider
	{
		[Export]
		[Name("TorqueScript")]
		[BaseDefinition("code")]
		internal static ContentTypeDefinition TorqueScriptContentType = null;

		[Export]
		[FileExtension(".tscript")]
		[ContentType("TorqueScript")]
		internal static FileExtensionToContentTypeDefinition TorqueScriptFileTypeTScript = null;

		[Export]
		[FileExtension(".gui")]
		[ContentType("TorqueScript")]
		internal static FileExtensionToContentTypeDefinition TorqueScriptFileTypeGUI = null;

		[Import]
		internal IClassificationTypeRegistryService classificationTypeRegistry = null;

		[Import]
		internal IBufferTagAggregatorFactoryService aggregatorFactory = null;

		public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
		{
			ITagAggregator<TorqueScriptTokenTag> torqueScriptTagAggregator = aggregatorFactory.CreateTagAggregator<TorqueScriptTokenTag>(buffer);
			return new TorqueScriptClassifier(buffer, torqueScriptTagAggregator, classificationTypeRegistry) as ITagger<T>;
		}
	}
	internal sealed class TorqueScriptClassifier : ITagger<ClassificationTag>
	{
		ITagAggregator<TorqueScriptTokenTag>                     _aggregator;
		IDictionary<TorqueScriptTokenTypes, IClassificationType> _torqueScriptTypes;
		internal TorqueScriptClassifier(ITextBuffer buffer, ITagAggregator<TorqueScriptTokenTag> torqueScriptTagAggregator, IClassificationTypeRegistryService typeService)
		{
			_aggregator = torqueScriptTagAggregator;
			_torqueScriptTypes = new Dictionary<TorqueScriptTokenTypes, IClassificationType>();
			_torqueScriptTypes[TorqueScriptTokenTypes.TorqueScriptOperator] = typeService.GetClassificationType(PredefinedClassificationTypeNames.Operator);
			_torqueScriptTypes[TorqueScriptTokenTypes.TorqueScriptComment] = typeService.GetClassificationType(PredefinedClassificationTypeNames.Comment);
			_torqueScriptTypes[TorqueScriptTokenTypes.TorqueScriptQuoted] = typeService.GetClassificationType(PredefinedClassificationTypeNames.String);
			_torqueScriptTypes[TorqueScriptTokenTypes.TorqueScriptVariable] = typeService.GetClassificationType(PredefinedClassificationTypeNames.Identifier);
			_torqueScriptTypes[TorqueScriptTokenTypes.TorqueScriptKeyword] = typeService.GetClassificationType(PredefinedClassificationTypeNames.Keyword);
			_torqueScriptTypes[TorqueScriptTokenTypes.TorqueScriptValue] = typeService.GetClassificationType(PredefinedClassificationTypeNames.Number);
		}

		public event EventHandler<SnapshotSpanEventArgs> TagsChanged
		{
			add { }
			remove { }
		}
		public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
		{
			foreach (var tagSpan in _aggregator.GetTags(spans))
			{
				var tagSpans = tagSpan.Span.GetSpans(spans[0].Snapshot);
				yield return new TagSpan<ClassificationTag>(tagSpans[0], new ClassificationTag(_torqueScriptTypes[tagSpan.Tag.type]));
			}
		}
	}
}
