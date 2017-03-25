using System;

namespace DiO_CS_GliphRecognizer.Data
{
    /// <summary>
    /// Structure to Store Information about Video Devices
    /// </summary>
    public class VideoDevice
    {

        #region Variables

        /// <summary>
        /// Name of device.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Device index.
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Moniker string.
        /// </summary>
        public string MonikerString { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="index">Device index.</param>
        /// <param name="name">Name</param>
        /// <param name="monikerString">Moniker string.</param>
        public VideoDevice(int index, string name, string monikerString)
        {
            this.Index = index;
            this.Name = name;
            this.MonikerString = monikerString;
        }

        #endregion

        #region ToString()

        /// <summary>
        /// Represent the Device as a String.
        /// </summary>
        /// <returns>
        /// The string representation of this device.
        /// </returns>
        public override string ToString()
        {
            return String.Format("[{0} {1}:{2}]", this.Index, this.Name, this.MonikerString);
        }

        #endregion

    }
}
