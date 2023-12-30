
namespace TorqueScriptLanguage
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.Composition;
	using Microsoft.VisualStudio.Text;
	using Microsoft.VisualStudio.Text.Classification;
	using Microsoft.VisualStudio.Text.Editor;
	using Microsoft.VisualStudio.Text.Tagging;
	using Microsoft.VisualStudio.Utilities;

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
		internal static FileExtensionToContentTypeDefinition TorqueScriptFileTypeCS = null;

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
		ITextBuffer                                              _buffer;
		ITagAggregator<TorqueScriptTokenTag>                     _aggregator;
		IDictionary<TorqueScriptTokenTypes, IClassificationType> _torqueScriptTypes;
		internal TorqueScriptClassifier(ITextBuffer buffer, ITagAggregator<TorqueScriptTokenTag> torqueScriptTagAggregator, IClassificationTypeRegistryService typeService)
		{
			_buffer = buffer;
			_aggregator = torqueScriptTagAggregator;
			_torqueScriptTypes = new Dictionary<TorqueScriptTokenTypes, IClassificationType>();
			_torqueScriptTypes[TorqueScriptTokenTypes.TorqueScriptFunction] = typeService.GetClassificationType("function");
			_torqueScriptTypes[TorqueScriptTokenTypes.TorqueScriptNew] = typeService.GetClassificationType("new");
			_torqueScriptTypes[TorqueScriptTokenTypes.TorqueScriptSingleton] = typeService.GetClassificationType("singleton");
			_torqueScriptTypes[TorqueScriptTokenTypes.TorqueScriptDatablock] = typeService.GetClassificationType("datablock");
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
