namespace CodePathHelper.Commands
{
    using CodePathHelper.Providers;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using System;
    using System.ComponentModel.Design;
    using System.Windows;
    using Task = System.Threading.Tasks.Task;

    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class GotoCodePath
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 256;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("90247df7-8439-4fd8-a1ca-024a94c78a8d");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        /// <summary>
        /// Initializes a new instance of the <see cref="GotoCodePath"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private GotoCodePath(AsyncPackage package, OleMenuCommandService commandService)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.Execute, menuCommandID);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static GotoCodePath Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in GotoCodePath's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)).ConfigureAwait(false) as OleMenuCommandService;
            Instance = new GotoCodePath(package, commandService);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            if (!GitProvider.EnsureGitInstalledAsync().ConfigureAwait(false).GetAwaiter().GetResult())
                return;

            ThreadHelper.ThrowIfNotOnUIThread();
            EnvDTE80.DTE2 dteObject = (EnvDTE80.DTE2)Package.GetGlobalService(typeof(SDTE));
            if (dteObject == null)
            {
                MessageProvider.ShowErrorInMessageBoxAsync("Error loading VS environment, please restart and retry.").ConfigureAwait(false).GetAwaiter().GetResult();
                return;
            }

            // Read Clipboard
            if (Options.Instance.ReadClipboardFirst)
            {
                var text = Clipboard.GetText();
                bool hasUrl = TextProvider.ExtractUrl(text, out string url);

                if (hasUrl)
                {
                    // Extract
                    bool isExtracted = AzureDevOpsCodePathProvider.ExtractInfoFromUrl(url, out string repoUrl, out string filePath, out string branchName, out int line, out int lineEnd, out int lineStartColumn, out int lineEndColumn);
                    
                    if (isExtracted)
                    {
                        DirectlyGotoDialogWindow directlyGotoDialogWindow = new DirectlyGotoDialogWindow(dteObject, url, filePath, branchName, line, lineEnd, lineStartColumn, lineEndColumn)
                        {
                            HasMaximizeButton = false,
                            HasMinimizeButton = false
                        };

                        directlyGotoDialogWindow.ShowDialog();
                        
                        return;
                    }
                }
            }


            InputUrlDialogWindow inputUrlDialogWindow = new InputUrlDialogWindow(dteObject)
            {
                HasMaximizeButton = false,
                HasMinimizeButton = false
            };
            inputUrlDialogWindow.ShowModal();
        }
    }
}
