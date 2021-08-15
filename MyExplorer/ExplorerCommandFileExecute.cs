namespace MyExplorer
{
    public class ExplorerCommandFileExecute : ExplorerCommand
    {
        public ExplorerCommandFileExecute(Explorer explorer) : base(explorer) { }

        private ExplorerFileInfo ExplorerFileInfo { get; set; }

        public void Init(ExplorerFileInfo efi)
        {
            ExplorerFileInfo = efi;
        }

        public override void Execute()
        {
            new FileExecuteOpen(1, "開く", ExplorerFileInfo.FullPath).Execute();
        }
    }
}