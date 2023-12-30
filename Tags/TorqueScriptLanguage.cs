
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Text.Tagging;
using System.Windows.Media;
using System;
using System.Diagnostics;


namespace TorqueScriptLanguage
{
	class TorqueScriptLanguage
	{
		#region Member Variables

		private List<Regex> _comments = new List<Regex>();
		private List<Regex> _quoted = new List<Regex>();
		private List<Regex> _keywords = new List<Regex>();
		private List<Regex> _operators = new List<Regex>();

		#endregion // Member Variables

		#region ctor
		public TorqueScriptLanguage()
		{
			Initialize();
		}
		#endregion // ctor

		#region Methods

		private void Initialize()
		{
			_comments.Add(new Regex(@"\/\/[^\n\r]+?(?:\*\)|[\n\r])", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_comments.Add(new Regex(@"(?s)/\*(.*?)\*/", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));

			_quoted.Add(new Regex(@"([""'])(?:\\\1|.)*?\1", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));

			_keywords.Add(new Regex(@"\bbreak\b", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_keywords.Add(new Regex(@"\bcase\b", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_keywords.Add(new Regex(@"\bcontinue\b", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_keywords.Add(new Regex(@"\bdatablock\b", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_keywords.Add(new Regex(@"\bdefault\b", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_keywords.Add(new Regex(@"\belse\b", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_keywords.Add(new Regex(@"\bfalse\b", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_keywords.Add(new Regex(@"\bfor\b", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_keywords.Add(new Regex(@"\bfunction\b", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_keywords.Add(new Regex(@"\bif\b", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_keywords.Add(new Regex(@"\blocal\b", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_keywords.Add(new Regex(@"\bnew\b", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_keywords.Add(new Regex(@"\bor\b", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_keywords.Add(new Regex(@"\bpackage\b", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_keywords.Add(new Regex(@"\breturn\b", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_keywords.Add(new Regex(@"\bsingleton\b", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_keywords.Add(new Regex(@"\bswitch\b", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_keywords.Add(new Regex(@"\bswitch$\b", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_keywords.Add(new Regex(@"\btrue\b", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_keywords.Add(new Regex(@"\bwhile\b", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));

			_operators.Add(new Regex(@"(\b|^|\s)\!(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)\!\$\=(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)\!\=(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)\-(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)\-\-(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)\$\=(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)%(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)%\=(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)&(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)&&(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)&\=(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)\*(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)\*\=(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)\/(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)\/\=(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)@(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)\^(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)\^\=(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)\|(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)\|\|(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)\|\=(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)\+(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)\+\+(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)\+\=(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)<(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)<<(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)<<\=(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)<\=(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)=(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled)); // [^=!<>]=[^=]
			_operators.Add(new Regex(@"(\b|^|\s)\-\=(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)\=\=(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)>(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)>\=(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)>>(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			_operators.Add(new Regex(@"(\b|^|\s)>>\=(\b|$|\s)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));

			//_operators.Add(new Regex(@"^[-+(]*[[:digit:]]+[)]*([-+*/][-+(]*[[:digit:]]+[)]*)*$", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			//_operators.Add(new Regex(@"(?<=[-+*\/^()])|(?=[-+*\/^()])", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled));
			
		}
		public IEnumerable<ITagSpan<TorqueScriptTokenTag>> findComments(SnapshotSpan span)
		{
			string str = span.GetText();
			foreach (var item in _comments)
			{
				var matches = item.Matches(str);
				for (int i = 0; i < matches.Count; i++)
				{
					Match match = matches[i];
					Span new_span = new Span(span.Start.Position + match.Index, match.Length);
					SnapshotSpan tokenSpan = new SnapshotSpan(span.Snapshot, new_span);
					yield return new TagSpan<TorqueScriptTokenTag>(tokenSpan, new TorqueScriptTokenTag(TorqueScriptTokenTypes.TorqueScriptComment));
				}
			}
		}
		public IEnumerable<ITagSpan<TorqueScriptTokenTag>> findQuoted(SnapshotSpan span)
		{
			string str = span.GetText();
			foreach (var item in _quoted)
			{
				var matches = item.Matches(str);
				for (int i = 0; i < matches.Count; i++)
				{
					Match match = matches[i];
					Span new_span = new Span(span.Start.Position + match.Index, match.Length);
					SnapshotSpan tokenSpan = new SnapshotSpan(span.Snapshot, new_span);
					yield return new TagSpan<TorqueScriptTokenTag>(tokenSpan, new TorqueScriptTokenTag(TorqueScriptTokenTypes.TorqueScriptQuoted));
				}
			}
		}

		public IEnumerable<ITagSpan<TorqueScriptTokenTag>> findKeywords(SnapshotSpan span)
		{
			string str = span.GetText();
			foreach (var item in _keywords)
			{
				var matches = item.Matches(str);
				for (int i = 0; i < matches.Count; i++)
				{
					Match match = matches[i];
					Span new_span = new Span(span.Start.Position + match.Index, match.Length);
					SnapshotSpan tokenSpan = new SnapshotSpan(span.Snapshot, new_span);
					yield return new TagSpan<TorqueScriptTokenTag>(tokenSpan, new TorqueScriptTokenTag(TorqueScriptTokenTypes.TorqueScriptKeyword));
				}
			}
		}

		public IEnumerable<ITagSpan<TorqueScriptTokenTag>> findOperators(SnapshotSpan span)
		{
			string str = span.GetText();
			foreach (var item in _operators)
			{
				var matches = item.Matches(str);
				for (int i = 0; i < matches.Count; i++)
				{
					Match match = matches[i];
					Span new_span = new Span(span.Start.Position + match.Index, match.Length);
					SnapshotSpan tokenSpan = new SnapshotSpan(span.Snapshot, new_span);
					yield return new TagSpan<TorqueScriptTokenTag>(tokenSpan, new TorqueScriptTokenTag(TorqueScriptTokenTypes.TorqueScriptOperator));
				}
			}
		}

		#endregion // Methods
	};
}

