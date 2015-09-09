using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Threading;

namespace HotKey79
{
    /// <summary>
    /// Main program hidden form.
    /// </summary>
    public class HotKey79 : Form
    {
        /// <summary>
        /// Default configuration file name.
        /// </summary>
        private const string DEFAULT_CONFIGURATION_FILE = "hotkey79.conf";

        /// <summary>
        /// Hot key commands.
        /// </summary>
        Dictionary<int, HotKey> commands;

        /// <summary>
        /// Keyboard hook.
        /// </summary>
        KeyboardHook keyboardHook;

        /// <summary>
        /// Mutex for only one instance testing.
        /// </summary>
        private static Mutex oneInstanceMutex;

        /// <summary>
        /// Main method.
        /// </summary>
        /// <param name="args">Command line arguments</param>
        [STAThread]
        static void Main(string[] args)
        {
            string configurationFile = Application.StartupPath + "\\" + DEFAULT_CONFIGURATION_FILE;

            if (args.Length >= 1)
            {
                configurationFile = args[0];
            }

            HotKey79 hotkey79 = null;

            try
            {
                hotkey79 = new HotKey79();
                hotkey79.Initialize(new ConfigurationReader(configurationFile));
            }
            catch (ProgramException e)
            {
                ShowError("Inicialization error", e.Message);
                return;
            }
            catch (Exception e)
            {
                ShowError("Unhandled inicialization exception", "Message: " + e.Message + "\nSource: " + e.Source + "\nStack trace: " + e.StackTrace);
                return;
            }

            Application.Run(hotkey79);
        }

        
        /// <summary>
        /// Initialize HotKey79.
        /// </summary>
        /// <param name="configurationReader">Configuration read from file.</param>
        public void Initialize(ConfigurationReader configurationReader)
        {
            if (IsAlreadyRunning())
            {
                throw new ProgramException("Only one instance of HotKey79 is allowed.");
            }

            List<HotKey> hotkeys = configurationReader.GetHotKeys();

            if (hotkeys.Count == 0)
            {
                throw new ProgramException("No command is defined in \"" + configurationReader.GetConfigurationFile() + "\".");
            }
            else
            {
                keyboardHook = new KeyboardHook();
                keyboardHook.KeyPressed += new EventHandler<KeyPressedEventArgs>(KeyboardHookKeyPressed);

                commands = new Dictionary<int, HotKey>();

                foreach (HotKey hotkey in hotkeys)
                {
                    int hotkeyId = keyboardHook.RegisterHotKey(hotkey.GetModifiers(), hotkey.GetKey());
                    commands.Add(hotkeyId, hotkey);
                }
            }
        }

        /// <summary>
        /// Run command for hotkey.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void KeyboardHookKeyPressed(object sender, KeyPressedEventArgs e)
        {
            commands[e.Id].KeyboardHookKeyPressed(sender, e);
        }

        /// <summary>
        /// Set form properties.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            Visible = false; // Hide form window.
            ShowInTaskbar = false; // Remove from taskbar.

            base.OnLoad(e);
        }

        /// <summary>
        /// Dispose form.
        /// </summary>
        /// <param name="isDisposing"></param>
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                keyboardHook.Dispose();
            }

            base.Dispose(isDisposing);
        }

        /// <summary>
        /// Initialize form component.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Devel79Tray
            // 
            this.ClientSize = new System.Drawing.Size(100, 100);
            this.Name = "HotKey79";
            this.ResumeLayout(false);

        }

        /// <summary>
        /// Check if another instance of application is already running.
        /// </summary>
        /// <returns>True if another instance is running, false otherwise.</returns>
        private static bool IsAlreadyRunning()
        {
            string location = Assembly.GetExecutingAssembly().Location;

            FileSystemInfo fileInfo = new FileInfo(location);
            string sExeName = fileInfo.Name;
            oneInstanceMutex = new Mutex(true, sExeName);

            if (oneInstanceMutex.WaitOne(0, false))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Show error message box.
        /// </summary>
        /// <param name="caption">Message box caption.</param>
        /// <param name="text">Message box body.</param>
        public static void ShowError(string caption, string text)
        {
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Program exception.
    /// </summary>
    public class ProgramException : Exception
    {

        /// <summary>
        /// Blank initialization.
        /// </summary>
        public ProgramException()
        {
        }

        /// <summary>
        /// Initialization with message.
        /// </summary>
        /// <param name="message"></param>
        public ProgramException(string message)
            : base(message)
        {
        }
    }
}
