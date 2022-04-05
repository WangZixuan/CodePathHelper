namespace CodePathHelper.Providers
{
    using Microsoft.VisualStudio.Text;

    /// <summary>
    /// SnapshotSpanProvider, will be deprecated.
    /// </summary>
    internal static class SnapshotSpanProvider
    {
        internal static void GetStartAndEndLineNumberAndColumn(in SnapshotSpan selectedSpan, out int line, out int lineEnd, out int lineStartColumn, out int lineEndColumn)
        {
            line = selectedSpan.Start.GetContainingLine().LineNumber;
            int thisLineStartPosition = selectedSpan.Snapshot.GetLineFromLineNumber(line).Start.Position;
            lineStartColumn = selectedSpan.Start.Position - thisLineStartPosition + 1;
            line++; // LineNumber starts from 0!

            lineEnd = selectedSpan.End.GetContainingLine().LineNumber;
            thisLineStartPosition = selectedSpan.Snapshot.GetLineFromLineNumber(lineEnd).Start.Position;
            lineEndColumn = selectedSpan.End.Position - thisLineStartPosition + 1;
            lineEnd++;
        }

        internal static void GetThisLineEndToCursor(in SnapshotSpan selectedSpan, out int line, out int lineEnd, out int lineStartColumn, out int lineEndColumn)
        {
            line = selectedSpan.Start.GetContainingLine().LineNumber;
            lineStartColumn = 1;

            lineEndColumn = selectedSpan.End.Position - selectedSpan.Snapshot.GetLineFromLineNumber(line).Start.Position + 1;

            line++;
            lineEnd = line;
        }
    }
}
