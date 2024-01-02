
namespace TorqueScriptLanguage
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.Composition;
	using System.Linq;
	using System.Diagnostics;
	using Microsoft.VisualStudio.Text;
	using Microsoft.VisualStudio.Text.Classification;
	using Microsoft.VisualStudio.Text.Editor;
	using Microsoft.VisualStudio.Text.Tagging;
	using Microsoft.VisualStudio.Utilities;

	[Export(typeof(ITaggerProvider))]
	[ContentType("TorqueScript")]
	[TagType(typeof(TorqueScriptTokenTag))]
	internal sealed class TorqueScriptTokenTagProvider : ITaggerProvider
	{

		public ITagger<T> CreateTagger<T>(ITextBuffer buffer) where T : ITag
		{
			return new TorqueScriptTokenTagger(buffer) as ITagger<T>;
		}
	}
	public class TorqueScriptTokenTag : ITag 
	{
		public TorqueScriptTokenTypes type { get; private set; }

		public TorqueScriptTokenTag(TorqueScriptTokenTypes type)
		{
			this.type = type;
		}
	}
	internal sealed class TorqueScriptTokenTagger : ITagger<TorqueScriptTokenTag>
	{
		TorqueScriptLanguage _language = new TorqueScriptLanguage();
		int                  _lastSnapshotVersion = -1;
		List<SnapshotSpan>   _snapshotCommentsSpans = new List<SnapshotSpan>();

		internal TorqueScriptTokenTagger(ITextBuffer buffer)
		{
		}

		public event EventHandler<SnapshotSpanEventArgs> TagsChanged
		{
			add { }
			remove { }
		}
		private bool IsIntersectsWithComments(ITagSpan<TorqueScriptTokenTag> tag)
		{
			foreach (SnapshotSpan test in _snapshotCommentsSpans)
			{
				if(test.IntersectsWith(tag.Span))
				{
					return true;
				}
			}
			return false;
		}
		public IEnumerable<ITagSpan<TorqueScriptTokenTag>> GetTags(NormalizedSnapshotSpanCollection spans)
		{
			foreach (SnapshotSpan span in spans)
			{
				//int lineNumber = span.Snapshot.GetLineNumberFromPosition(span.Start.Position);
				//Debug.WriteLine(spans.ToString() + " " + lineNumber + " " + span.ToString());

				int version = span.Snapshot.Version.VersionNumber;
				if(version != _lastSnapshotVersion)
				{
					SnapshotSpan fullSnapshotSpan = new SnapshotSpan(span.Snapshot, new Span(0, span.Snapshot.Length));
					var allComments = _language.findComments(fullSnapshotSpan);
					_snapshotCommentsSpans.Clear();
					foreach (ITagSpan<TorqueScriptTokenTag> comment in allComments)
					{
						_snapshotCommentsSpans.Add(comment.Span);
					}
					_lastSnapshotVersion = version;
				}

				foreach (SnapshotSpan test in _snapshotCommentsSpans)
				{
					SnapshotSpan? intersect = span.Intersection(test);
					if(intersect.HasValue)
					{
						yield return new TagSpan<TorqueScriptTokenTag>(intersect.Value, new TorqueScriptTokenTag(TorqueScriptTokenTypes.TorqueScriptComment));
					}
				}

				var tags = _language.findQuoted(span);
				foreach (ITagSpan<TorqueScriptTokenTag> tag in tags)
				{
					if (!IsIntersectsWithComments(tag))
					{
						yield return tag;
					}
				}
				tags = _language.findKeywords(span);
				foreach (ITagSpan<TorqueScriptTokenTag> tag in tags)
				{
					if (!IsIntersectsWithComments(tag))
					{
						yield return tag;
					}
				}
				tags = _language.findOperators(span);
				foreach (ITagSpan<TorqueScriptTokenTag> tag in tags)
				{
					if (!IsIntersectsWithComments(tag))
					{
						yield return tag;
					}
				}
				tags = _language.findVariables(span);
				foreach (ITagSpan<TorqueScriptTokenTag> tag in tags)
				{
					if (!IsIntersectsWithComments(tag))
					{
						yield return tag;
					}
				}
			}
		}
	}
}
