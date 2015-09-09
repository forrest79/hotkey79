using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace HotKey79
{
    /// <summary>
    /// Windows global keyboard hook definition (http://stackoverflow.com/questions/482729/c-sharp-iterating-through-an-enum-indexing-a-system-array)
    /// </summary>
    public sealed class KeyboardHook : IDisposable
    {
        // Registers a hot key with Windows.
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        // Unregisters the hot key with Windows.
        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        /// <summary>
        /// Represents the window that is used internally to get the messages.
        /// </summary>
        private class Window : NativeWindow, IDisposable
        {
            private static int WM_HOTKEY = 0x0312;

            public Window()
            {
                // create the handle for the window.
                this.CreateHandle(new CreateParams());
            }

            /// <summary>
            /// Overridden to get the notifications.
            /// </summary>
            /// <param name="m"></param>
            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);

                // check if we got a hot key pressed.
                if (m.Msg == WM_HOTKEY)
                {
                    int id = m.WParam.ToInt32();
                    Keys key = (Keys)(((int)m.LParam >> 16) & 0xFFFF);
                    KeyModifiers modifier = (KeyModifiers)((int)m.LParam & 0xFFFF);

                    // invoke the event to notify the parent.
                    if (KeyPressed != null)
                    {
                        KeyPressed(this, new KeyPressedEventArgs(id, modifier, key));
                    }
                }
            }

            public event EventHandler<KeyPressedEventArgs> KeyPressed;

            #region IDisposable Members

            public void Dispose()
            {
                this.DestroyHandle();
            }

            #endregion
        }

        private Window window = new Window();
        private int currentId = 0;

        public KeyboardHook()
        {
            // register the event of the inner native window.
            window.KeyPressed += delegate(object sender, KeyPressedEventArgs args)
            {
                if (KeyPressed != null)
                {
                    KeyPressed(this, args);
                }
            };
        }

        /// <summary>
        /// Registers a hot key in the system.
        /// </summary>
        /// <param name="modifier">The modifiers that are associated with the hot key.</param>
        /// <param name="key">The key itself that is associated with the hot key.</param>
        /// <returns>Hot key ID</returns>
        public int RegisterHotKey(KeyModifiers modifier, Keys key)
        {
            // increment the counter.
            currentId = currentId + 1;

            // register the hot key.
            if (!RegisterHotKey(window.Handle, currentId, (uint)modifier, (uint)key))
            {
                throw new InvalidOperationException("Couldn’t register the hot key.");
            }

            return currentId;
        }

        /// <summary>
        /// A hot key has been pressed.
        /// </summary>
        public event EventHandler<KeyPressedEventArgs> KeyPressed;

        #region IDisposable Members

        public void Dispose()
        {
            // unregister all the registered hot keys.
            for (int i = currentId; i > 0; i--)
            {
                UnregisterHotKey(window.Handle, i);
            }

            // dispose the inner native window.
            window.Dispose();
        }

        #endregion
    }

    /// <summary>
    /// Event Args for the event that is fired after the hot key has been pressed.
    /// </summary>
    public class KeyPressedEventArgs : EventArgs
    {
        private int id;
        private KeyModifiers modifier;
        private Keys key;

        internal KeyPressedEventArgs(int id, KeyModifiers modifier, Keys key)
        {
            this.id = id;
            this.modifier = modifier;
            this.key = key;
        }

        public int Id
        {
            get { return id; }
        }

        public KeyModifiers Modifier
        {
            get { return modifier; }
        }

        public Keys Key
        {
            get { return key; }
        }
    }

    /// <summary>
    /// The enumeration of possible modifiers.
    /// </summary>
    [Flags]
    public enum KeyModifiers : uint
    {
        Alt = 1,
        Control = 2,
        Shift = 4,
        Win = 8
    }
}
