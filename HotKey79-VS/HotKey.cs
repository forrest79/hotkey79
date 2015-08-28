using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace HotKey79
{
    /// <summary>
    /// Hot key definition.
    /// </summary>
    public class HotKey
    {
        /// <summary>
        /// Modifiers definition (Alt, Control, Shitf and Win key).
        /// </summary>
        private KeyModifiers modifiers;

        /// <summary>
        /// Key code definition.
        /// </summary>
        private Keys key;

        /// <summary>
        /// Command to execute.
        /// </summary>
        private string command;

        /// <summary>
        /// Create new hot key with definition.
        /// </summary>
        /// <param name="modifiers">Modifier keys</param>
        /// <param name="key">Key code</param>
        /// <param name="command">Command to execute</param>
        public HotKey(KeyModifiers modifiers, Keys key, string command)
        {
            this.modifiers = modifiers;
            this.key = key;
            this.command = command;
        }

        /// <summary>
        /// Get modifier keys.
        /// </summary>
        /// <returns>Modifier keys</returns>
        public KeyModifiers GetModifiers()
        {
            return modifiers;
        }

        /// <summary>
        /// Get key code.
        /// </summary>
        /// <returns>Key code</returns>
        public Keys GetKey()
        {
            return key;
        }

        /// <summary>
        /// Execute command.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void KeyboardHookKeyPressed(object sender, KeyPressedEventArgs e)
        {
            Process process = new System.Diagnostics.Process();

            try
            {
                string[] commandParts = command.Split(new Char[] { ' ', '\t' }, 2);

                switch (commandParts.Length)
                {
                    case 1:
                        process.StartInfo.FileName = commandParts[0];
                        break;
                    case 2:
                        process.StartInfo.Arguments = commandParts[1];
                        goto case 1;
                    default:
                        return;
                }

                process.Start();
            }
            catch (System.ComponentModel.Win32Exception)
            {
                MessageBox.Show("Can\'t run command \"" + command + "\"", "HotKey79", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
