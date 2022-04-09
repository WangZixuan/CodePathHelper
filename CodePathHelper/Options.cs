using System.ComponentModel;
using Community.VisualStudio.Toolkit;

namespace CodePathHelper
{
    internal partial class OptionsProvider
    {
        public class GeneralOptions : BaseOptionPage<Options> { }
    }

    public class Options : BaseOptionModel<Options>
    {
        // Share Code Path

        [Category("Share Code Path")]
        [DisplayName("Use current branch")]
        [Description("If using current branch or default branch to generate the Url")]
        [DefaultValue(true)]
        public bool UseCurrentBranch { get; set; } = true;

        [Category("Share Code Path")]
        [DisplayName("Extract git info every time")]
        [Description("Extract git info every time or on first use. It will be faster if turned off, but git info would be wrong if you switch branch.")]
        [DefaultValue(true)]
        public bool ExtractGitInfoEveryTime { get; set; } = true;

        [Category("Share Code Path")]
        [DisplayName("Default branch name")]
        [Description("Default branch name of your repository.")]
        [DefaultValue(NotificationStyle.StatusBar)]
        public string DefaultBranchName { get; set; } = "master";

        [Category("Share Code Path")]
        [DisplayName("Customized copy content")]
        [Description("Use {code}, {url} and {newline} as placeholder to customize your message. Ex: \"Here it is:{newline}{url}{newline}{code}\"")]
        [DefaultValue("{url}")]
        public string CustomizedCopyContent { get; set; } = "{url}";

        [Category("Share Code Path")]
        [DisplayName("Show notification in")]
        [Description("Show notification in status bar(default), or message box, or do not show.")]
        [DefaultValue(NotificationStyle.StatusBar)]
        public NotificationStyle NotificationStyle { get; set; } = NotificationStyle.StatusBar;

        [Category("Share Code Path")]
        [DisplayName("Background git job")]
        [Description("You may use this command to make sure of code consistency between local and remote branch, but git command is NOT guaranteed to run successfully. Please pay close attention to enable it as it may commit more files than you expect.")]
        [DefaultValue(BackgroundGitJob.None)]
        public BackgroundGitJob BackgroundGitJob { get; set; } = BackgroundGitJob.None;

        // Goto Code Path

        [Category("Goto Code Path")]
        [DisplayName("Read clipboard first")]
        [Description("Read clipboard first to get the Url")]
        [DefaultValue(false)]
        public bool ReadClipboardFirst { get; set; } = false;

        [Category("Goto Code Path")]
        [DisplayName("Select code snippet")]
        [Description("If select the code snippet from url automatically.")]
        [DefaultValue(true)]
        public bool SelectCodeSnippet { get; set; } = true;

        [Category("Goto Code Path")]
        [DisplayName("Checkout the branch in Url")]
        [Description("If checking out to the targeted branch in url.")]
        [DefaultValue(false)]
        public CheckingoutBranchOption CheckoutBranchOption { get; set; } = CheckingoutBranchOption.CurrentBranch;
    }

    public enum BranchSelection
    {
        Current,
        Default
    }

    public enum NotificationStyle
    {
        None,
        StatusBar,
        MessageBox
    }

    public enum BackgroundGitJob
    {
        None,
        Push,
        CommitAndPush
    }

    public enum CheckingoutBranchOption
    {
        CurrentBranch,
        DefaultBranch,
        UrlBranch
    }
}
