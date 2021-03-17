using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell.Interop;
using System.Windows.Forms;

namespace Analyzer
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [Guid(CodeAnalyzer.PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(CodeAnalyzerWindow))]

    public sealed class CodeAnalyzer : Package
    {
        public const string PackageGuidString = "2b716baa-3dc4-49d2-ab99-1e46232b68da";
        public CodeAnalyzer() {   }

        #region Package Members
        protected override void Initialize()
        {
            base.Initialize();
            CodeAnalyzerCommand.Initialize(this);
        }

        #endregion
    }

    internal sealed class CodeAnalyzerCommand
    {
        private readonly Package package;
       
        public const int CommandID = 0x0601;
        public static readonly Guid CommandSet = new Guid("e73a1752-b81d-4d17-945f-a2a13c25cf4c");

        public static void Initialize(Package package)
        {
            Instance = new CodeAnalyzerCommand(package);
        }

        public static CodeAnalyzerCommand Instance
        {
            get;
            private set;
        }

        private CodeAnalyzerCommand(Package package)
        {

            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.package = package;

            OleMenuCommandService mcs = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            // Create the command for the menu item.
            CommandID menuCommandID = new CommandID(CommandSet, CommandID);
            MenuCommand menuItem = new MenuCommand(this.ShowWindow, menuCommandID);
            mcs.AddCommand(menuItem);
        }

        private IServiceProvider ServiceProvider
        {
            get { return this.package; }
        }

        private void ShowWindow(object sender, EventArgs e)
        {
            ToolWindowPane MyWindow = this.package.FindToolWindow(typeof(CodeAnalyzerWindow), 0, true);
            if ((null == MyWindow) || (null == MyWindow.Frame))
            {
                throw new NotSupportedException("Cannot create tool window");
            }
            IVsWindowFrame windowFrame = (IVsWindowFrame)MyWindow.Frame;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
        }

    }

    public class CodeAnalyzerWindow : ToolWindowPane
    {
        public CodeAnalyzerWindow() : base(null)
        {
            this.Caption = "CodeAnalyzer";
            this.Content = new CodeAnalyzerControl();
        }
    }
}