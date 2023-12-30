
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

		ITextBuffer _buffer;
		IDictionary<string, TorqueScriptTokenTypes> _torqueScriptTypes;

		internal TorqueScriptTokenTagger(ITextBuffer buffer)
		{
			_buffer = buffer;
			_torqueScriptTypes = new Dictionary<string, TorqueScriptTokenTypes>();
			_torqueScriptTypes["function"] = TorqueScriptTokenTypes.TorqueScriptFunction;
			_torqueScriptTypes["new"] = TorqueScriptTokenTypes.TorqueScriptNew;
			_torqueScriptTypes["singleton"] = TorqueScriptTokenTypes.TorqueScriptSingleton;
			_torqueScriptTypes["datablock"] = TorqueScriptTokenTypes.TorqueScriptDatablock;
		}

		public event EventHandler<SnapshotSpanEventArgs> TagsChanged
		{
			add { }
			remove { }
		}

		public IEnumerable<ITagSpan<TorqueScriptTokenTag>> GetTags(NormalizedSnapshotSpanCollection spans)
		{
			foreach (SnapshotSpan curSpan in spans)
			{
				ITextSnapshotLine containingLine = curSpan.Start.GetContainingLine();
				int curLoc = containingLine.Start.Position;
				string[] tokens = containingLine.GetText().ToLower().Split(' ', '\t');
				foreach (string torqueScriptToken in tokens)
				{
					if (_torqueScriptTypes.ContainsKey(torqueScriptToken))
					{
						var tokenSpan = new SnapshotSpan(curSpan.Snapshot, new Span(curLoc, torqueScriptToken.Length));
						if (tokenSpan.IntersectsWith(curSpan))
						{
							yield return new TagSpan<TorqueScriptTokenTag>(tokenSpan, new TorqueScriptTokenTag(_torqueScriptTypes[torqueScriptToken]));
						}
					}
					curLoc += torqueScriptToken.Length + 1; //add an extra char location because of the space
				}
			}
		}
	}
}
