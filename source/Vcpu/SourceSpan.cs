namespace Vcpu
{
    public readonly struct SourceSpan
    {
        public SourceSpan(int startLine, int lineCount)
        {
            StartLine = startLine;
            LineCount = lineCount;
        }

        public int StartLine { get; }
        public int LineCount { get; }

        public int EndLine => StartLine + LineCount - 1;
    }
}
