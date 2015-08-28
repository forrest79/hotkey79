using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace HotKey79
{
    /// <summary>
    /// Read and parse configuration file.
    /// </summary>
    public class ConfigurationReader
    {
        /// <summary>
        /// Absolute path to configuration file.
        /// </summary>
        private string configurationFile;

        /// <summary>
        /// Hot keys definition.
        /// </summary>
        private List<HotKey> hotkeys;

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="configurationFile">Absolute path to configuration file.</param>
        public ConfigurationReader(string configurationFile)
        {
            this.configurationFile = configurationFile;
            this.hotkeys = new List<HotKey>();
        }

        /// <summary>
        /// Read configuration file.
        /// </summary>
        private void ReadConfiguration()
        {
            if (File.Exists(configurationFile))
            {
                try
                {
                    StreamReader configuration = File.OpenText(configurationFile);

                    string line = null;
                    while ((line = configuration.ReadLine()) != null)
                    {
                        line = line.Trim();

                        if (line.Equals("") || line.StartsWith("#"))
                        {
                            continue;
                        }

                        string[] settings = line.Split(":".ToCharArray(), 2);
                        string allKeys = settings[0].Trim();
                        string command = settings[1].Trim();

                        string[] keys = allKeys.Split("+".ToCharArray());

                        KeyModifiers modifiers = 0;
                        Keys key = 0;
                        foreach (string item in keys)
                        {
                            string oneKey = item.Trim().ToLower();

                            switch (oneKey)
                            {
                                case "alt":
                                    modifiers = modifiers | KeyModifiers.Alt;
                                    break;
                                case "ctrl":
                                    modifiers = modifiers | KeyModifiers.Control;
                                    break;
                                case "shift":
                                    modifiers = modifiers | KeyModifiers.Shift;
                                    break;
                                case "win":
                                    modifiers = modifiers | KeyModifiers.Win;
                                    break;
                                default:
                                    string[] enumKeys = Enum.GetNames(typeof(Keys));
                                    Array values = Enum.GetValues(typeof(Keys));
                                    foreach(Keys val in values)
                                    {
                                        if (Enum.GetName(typeof(Keys), val).ToLower().Equals(oneKey))
                                        {
                                            key = val;
                                            break;
                                        }
                                    }
                                    break;
                            }

                        }

                        if ((key == 0) || (modifiers == 0))
                        {
                            throw new ConfigurationReaderException("Key and modificators have to be set for command \"" + line + "\".");
                        }

                        hotkeys.Add(new HotKey(modifiers, key, command));
                    }

                    configuration.Close();
                }
                catch (ConfigurationReaderException e)
                {
                    throw e;
                }
                catch
                {
                    throw new ConfigurationReaderException("Can't read from configuration file \"" + configurationFile + "\".");
                }
            }
            else
            {
                throw new ConfigurationReaderException("Configuration file \"" + configurationFile + "\" not exists.");
            }
        }

        /// <summary>
        /// Return configuration file absolute path.
        /// </summary>
        /// <returns>Configuration file absolute path.</returns>
        public string GetConfigurationFile()
        {
            return configurationFile;
        }

        /// <summary>
        /// Get hot keys definition.
        /// </summary>
        /// <returns></returns>
        public List<HotKey> GetHotKeys()
        {
            if (hotkeys.Count == 0)
            {
                ReadConfiguration();
            }

            return hotkeys;
        }
    }

    /// <summary>
    /// Configuration reader exception.
    /// </summary>
    public class ConfigurationReaderException : ProgramException
    {

        /// <summary>
        /// Blank initialization.
        /// </summary>
        public ConfigurationReaderException()
        {
        }

        /// <summary>
        /// Initialization with message.
        /// </summary>
        /// <param name="message"></param>
        public ConfigurationReaderException(string message) : base(message)
        {
        }
    }
}
