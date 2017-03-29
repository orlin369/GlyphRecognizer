/*

Copyright (c) [2016] [Orlin Dimitrov]

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

*/

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Xml;
using System.Text;

using AForge.Imaging.Filters;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Vision.GlyphRecognition;
using AForge.Vision.GlyphRecognition.Data;
using AForge.Vision.GlyphRecognition.Utils;

using DiO_CS_GliphRecognizer.SettingsForms;
using DiO_CS_GliphRecognizer.Connectors;
using DiO_CS_GliphRecognizer.Adapters;
using DiO_CS_GliphRecognizer.Data;

namespace DiO_CS_GliphRecognizer
{
    public partial class MainForm : Form
    {

        #region Variables

        /// <summary>
        /// Collection of glyph databases.
        /// </summary>
        private GlyphDatabases glyphDatabases = new GlyphDatabases();

        /// <summary>
        /// Glyph recognizer to use for glyph recognition in video.
        /// </summary>
        private GlyphRecognizer recognizer;

        /// <summary>
        /// Recognized database.
        /// </summary>
        private List<ExtractedGlyphData> recognisedGlyphs = new List<ExtractedGlyphData>();

        /// <summary>
        /// Video source.
        /// </summary>
        private VideoCaptureDevice videoSource;

        /// <summary>
        /// Captured image.
        /// </summary>
        private Bitmap capturedImage;

        /// <summary>
        /// Sync object.
        /// </summary>
        private object syncLock = new object();

        /// <summary>
        /// Image point of the object to estimate pose for.
        /// </summary>
        private AForge.Point[] imagePoints = new AForge.Point[4];

        /// <summary>
        /// Colors used to highlight points on image.
        /// </summary>
        private Color[] pointsColors = new Color[4]
            {
                Color.Yellow,
                Color.Blue,
                Color.Red,
                Color.Lime
            };

        /// <summary>
        /// Video capture devices.
        /// </summary>
        private VideoDevice[] videoDevices;

        /// <summary>
        /// Connector
        /// </summary>
        private DataConnector connector;

        #endregion

        #region Configuration Option Names

        private const string activeDatabaseOption = "ActiveDatabase";
        private const string mainFormXOption = "MainFormX";
        private const string mainFormYOption = "MainFormY";
        private const string mainFormWidthOption = "MainFormWidth";
        private const string mainFormHeightOption = "MainFormHeight";
        private const string mainFormStateOption = "MainFormState";
        private const string mainSplitterOption = "MainSplitter";
        private const string glyphSizeOption = "GlyphSize";
        private const string focalLengthOption = "FocalLength";
        private const string detectFocalLengthOption = "DetectFocalLength";
        private const string autoDetectFocalLengthOption = "AutoDetectFocalLength";

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            // Check to see what video inputs we have available.
            this.videoDevices = this.GetDevices();

            if (videoDevices.Length == 0)
            {
                DialogResult res = MessageBox.Show("A camera device was not detected. Do you want to exit?", "",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == System.Windows.Forms.DialogResult.Yes)
                {
                    Application.Exit();
                }
            }

            // Add cameras to the menus.
            this.AddCameras(this.videoDevices, this.captureDeviceToolStripMenuItem, this.camerasToolStripMenuItem_Click);
        }

        #endregion

        #region Private

        /// <summary>
        /// Get list of all available devices on the PC.
        /// </summary>
        /// <returns></returns>
        private VideoDevice[] GetDevices()
        {
            //Set up the capture method 
            //-> Find systems cameras with DirectShow.Net dll, thanks to Carles Lloret.
            //DsDevice[] systemCamereas = DsDevice.GetDevicesOfCat(AForge.Video.DirectShow.FilterCategory.VideoInputDevice);

            // Enumerate video devices
            FilterInfoCollection systemCamereas = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            VideoDevice[] videoDevices = new VideoDevice[systemCamereas.Count];

            for (int index = 0; index < systemCamereas.Count; index++)
            {
                videoDevices[index] = new VideoDevice(index, systemCamereas[index].Name, systemCamereas[index].MonikerString);
            }

            return videoDevices;
        }

        /// <summary>
        /// Add video devices to the tool stript menu.
        /// </summary>
        /// <param name="videoDevices">List of camears.</param>
        /// <param name="menu">Menu item.</param>
        /// <param name="callback">Callback</param>
        private void AddCameras(VideoDevice[] videoDevices, ToolStripMenuItem menu, EventHandler callback)
        {
            if (videoDevices.Length == 0)
            {
                return;
            }

            menu.DropDown.Items.Clear();

            foreach (VideoDevice device in videoDevices)
            {
                // Store the each retrieved available capture device into the MenuItems.
                ToolStripMenuItem mItem = new ToolStripMenuItem();

                mItem.Text = String.Format("{0:D2} / {1}", device.Index, device.Name);
                mItem.Tag = device;
                mItem.Enabled = true;
                mItem.Checked = false;
                mItem.Click += callback;

                menu.DropDown.Items.Add(mItem);
            }
        }

        /// <summary>
        /// Create glyph data.
        /// This method is example.
        /// </summary>
        private void CreateGlyph()
        {
            // Glyph name.
            string glyphName = "Test1";
            // Glyph size.
            const int glyphSize = 5;
            // Glyph data.
            byte[,] glyphData = new byte[glyphSize, glyphSize]
            {
                {0, 0, 0, 0, 0},
                {0, 0, 1, 0, 0},
                {0, 1, 1, 1, 0},
                {0, 1, 0, 0, 0},
                {0, 0, 0, 0, 0}
            };

            // Create glyph.
            Glyph testGlyph = new Glyph(glyphName, glyphData);

            // Glyph user data.
            testGlyph.UserData = new GlyphVisualizationData(Color.Blue);

            // Generate image from the glyph.
            Bitmap image = testGlyph.CreateGlyphImage(500);

            string fileName = String.Format("Glyph_{0}_{1}x{1}.PNG", glyphName, glyphSize);

            // Save it to file.
            image.Save(fileName);
        }

        /// <summary>
        /// Refresh the list displaying available databases of glyphs.
        /// </summary>
        private void LoadGlyphDatabases5()
        {
            try
            {
                using (XmlTextReader xmlOut = new XmlTextReader(Properties.Settings.Default.LastDatabasePath))
                {
                    this.glyphDatabases.Load(xmlOut);
                }

                this.recognizer = new GlyphRecognizer(glyphDatabases["ExampleSize5"].Size);

                // set the database to image processor ...
                this.recognizer.GlyphDatabase = this.glyphDatabases["ExampleSize5"];
            }
            catch (Exception exception)
            {

            }
        }

        /// <summary>
        /// Refresh the list displaying available databases of glyphs
        /// </summary>
        private void LoadGlyphDatabases12()
        {

            const int glyphSize = 12;

            // Create glyph.
            string glyphName4 = "Test4";
            byte[,] glyphData4 = new byte[glyphSize, glyphSize]
            {
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0},
                {0, 0, 1, 1, 0, 0, 1, 1, 0, 1, 0, 0},
                {0, 0, 1, 0, 1, 0, 0, 1, 0, 1, 0, 0},
                {0, 0, 1, 1, 0, 1, 1, 1, 1, 1, 0, 0},
                {0, 0, 1, 1, 0, 1, 1, 1, 0, 1, 0, 0},
                {0, 0, 1, 1, 1, 1, 0, 0, 0, 1, 0, 0},
                {0, 0, 1, 0, 0, 0, 1, 1, 0, 1, 0, 0},
                {0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
                {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
            };
            Glyph testGlyph4 = new Glyph(glyphName4, glyphData4);
            testGlyph4.UserData = new GlyphVisualizationData(Color.Purple);

            // Create database.
            const string dbName = "ExampleSize12";

            // Create database.
            GlyphDatabase lGlyphDatabase = new GlyphDatabase(glyphSize);

            // Add glyph to database.
            lGlyphDatabase.Add(testGlyph4);

            // Add database.
            this.glyphDatabases.AddGlyphDatabase(dbName, lGlyphDatabase);

            this.recognizer = new GlyphRecognizer(glyphSize);
            
            // set the database to image processor ...
            this.recognizer.GlyphDatabase = this.glyphDatabases[dbName];
        }

        /// <summary>
        /// Display glyph data.
        /// </summary>
        /// <param name="egd"></param>
        /// <param name="name"></param>
        private void DisplayGlyphData(List<ExtractedGlyphData> egd)
        {
            if (this.dgvGlyphData.InvokeRequired)
            {
                this.dgvGlyphData.BeginInvoke(
                    (MethodInvoker)delegate ()
                    {
                        this.dgvGlyphData.Rows.Clear();
                    });
            }
            else
            {
                this.dgvGlyphData.Rows.Clear();
            }

            int index = 0;
            foreach (ExtractedGlyphData gd in egd)
            {
                if (gd.RecognizedGlyph != null)
                {
                    // Estimate orientation and position.
                    float yaw = 0.0f;
                    float pitch = 0.0f;
                    float roll = 0.0f;
                    gd.EstimateOrientation(true, out yaw, out pitch, out roll);
                    AForge.Point[] pp = gd.PerformProjection();
                    double area = gd.Area();

                    //string textData = string.Format("Name: {0};\r\nAngles[deg]: Y: {1:F3}, P: {2:F3}, R: {3:F3} \r\nPosition[pix]: X: {4:F3} Y: {5:F3}\r\n Size: {5:F3}",
                    //    gd.RecognizedGlyph.Name, yaw, pitch, roll, pp[0].X, pp[0].Y, area);

                    string[] row = new string[]
                    {
                        index.ToString(),
                        gd.RecognizedGlyph.Name,
                        pp[0].X.ToString(),
                        pp[0].Y.ToString(),
                        yaw.ToString(),
                        pitch.ToString(),
                        roll.ToString(),
                        area.ToString(),
                    };

                    if (this.dgvGlyphData.InvokeRequired)
                    {
                        this.dgvGlyphData.BeginInvoke(
                            (MethodInvoker)delegate ()
                            {
                                this.dgvGlyphData.Rows.Add(row);
                            });
                    }
                    else
                    {
                        this.dgvGlyphData.Rows.Add(row);
                    }
                }

                index++;
            }
        }

        /// <summary>
        /// Display image.
        /// </summary>
        /// <param name="image"></param>
        private void DisplayGlyphs(Bitmap image, List<ExtractedGlyphData> egd)
        {
            if (WindowState == FormWindowState.Minimized) return;

            int centerSize = 20;
            Point center = new Point(image.Width / 2 - centerSize / 2, image.Height / 2 - centerSize / 2);

            // Display image.
            if (this.pbMain.InvokeRequired)
            {
                this.pbMain.BeginInvoke((MethodInvoker)delegate()
                {
                    using (Graphics g = Graphics.FromImage((Image)image))
                    {
                        foreach (ExtractedGlyphData gd in egd)
                        {
                            GlyphDrawer.DrawCentroid(gd, g);
                            GlyphDrawer.DrawContour(gd, g);
                            GlyphDrawer.DrawPoints(gd, g);
                            GlyphDrawer.DrawCoordinates(gd, g);
                        }

                        g.DrawEllipse(Pens.Yellow, new Rectangle(center, new Size(centerSize, centerSize)));
                    }

                    if (this.pbMain.Size.Width > 1 && this.pbMain.Size.Height > 1)
                    {
                        Bitmap rszImage = Utils.ResizeImage(image, this.pbMain.Size);

                        this.pbMain.Image = rszImage;
                    }
                });
            }
            else
            {
                using (Graphics g = Graphics.FromImage((Image)image))
                {
                    foreach (ExtractedGlyphData gd in egd)
                    {
                        GlyphDrawer.DrawCentroid(gd, g);
                        GlyphDrawer.DrawContour(gd, g);
                        GlyphDrawer.DrawPoints(gd, g);
                        GlyphDrawer.DrawCoordinates(gd, g);
                    }

                    g.DrawEllipse(Pens.Yellow, new Rectangle(center, new Size(centerSize, centerSize)));
                }

                if (this.pbMain.Size.Width > 1 && this.pbMain.Size.Height > 1)
                {
                    Bitmap rszImage = Utils.ResizeImage(image, this.pbMain.Size);

                    this.pbMain.Image = rszImage;
                }
            }
        }

        #endregion

        #region Data connector

        /// <summary>
        /// Connect vision system.
        /// </summary>
        private void ConnectVisionSystemViaMqtt()
        {
            
            try
            {
                this.connector = new DataConnector(new MqttAdapter(
                    Properties.Settings.Default.BrokerHost,
                    Properties.Settings.Default.BrokerPort,
                    Properties.Settings.Default.MqttInputTopic,
                    Properties.Settings.Default.MqttOutputTopic,
                    Properties.Settings.Default.MqttImageTopic));

                //this.robot.OnMessage += myRobot_OnMessage;
                //this.robot.OnSensors += myRobot_OnSensors;
                //this.robot.OnDistanceSensors += myRobot_OnDistanceSensors;
                //this.robot.OnGreatingsMessage += myRobot_OnGreatingsMessage;
                //this.robot.OnStoped += myRobot_OnStoped;
                //this.robot.OnPosition += myRobot_OnPosition;
                this.connector.Connect();
                //this.robot.Reset();
            }
            catch (Exception exception)
            {
                //this.AddStatus(exception.ToString(), Color.White);
            }
            
        }

        /// <summary>
        /// Disconnect vision system.
        /// </summary>
        private void DisconnectVisionSystemFromMqtt()
        {
            try
            {
                if (this.connector != null && this.connector.IsConnected)
                {
                    this.connector.Disconnect();
                }
            }
            catch (Exception exception)
            {
                //this.AddStatus(exception.ToString(), Color.White);
            }
        }


        private void SendGlyphData(List<ExtractedGlyphData> egd)
        {
            if (this.connector == null || !this.connector.IsConnected) return;

            foreach (ExtractedGlyphData gd in egd)
            {
                SerialExtractedGlyphData sgd = new SerialExtractedGlyphData(gd);

                this.connector.SendGlyph(sgd);
            }
        }

        private void SendImageData(Bitmap image)
        {
            if (this.connector == null || !this.connector.IsConnected) return;
            if (image == null) return;
            this.connector.SendImage(image);
        }

        #endregion

        #region Main Form

        private void MainForm_Load(object sender, EventArgs e)
        {
            //this.LoadGlyphDatabases12();
            this.LoadGlyphDatabases5();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.StopCapture();
            this.DisconnectVisionSystemFromMqtt();
        }

        #endregion

        #region Frame Grabber

        /// <summary>
        /// Start video capture.
        /// </summary>
        /// <param name="monikerString">Moniker string.</param>
        private void StartCapture(string monikerString)
        {
            this.videoSource = new VideoCaptureDevice(monikerString);

            // We will only use 1 frame ready event this is not really safe but it fits the purpose.
            this.videoSource.NewFrame += new NewFrameEventHandler(this.videoSource_NewFrame);

            //_Capture2.Start(); //We make sure we start Capture device 2 first.
            this.videoSource.Start();
        }

        /// <summary>
        /// Stop video capture.
        /// </summary>
        private void StopCapture()
        {
            if (this.videoSource != null)
            {
                // We will only use 1 frame ready event this is not really safe but it fits the purpose.
                this.videoSource.NewFrame -= new NewFrameEventHandler(this.videoSource_NewFrame);

                this.videoSource.Stop();
            }
        }


        private void videoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            lock(this.syncLock)
            {
                // Dispose last frame.
                if (this.capturedImage != null)
                {
                    //this.capturedImage.Dispose();
                }

                // Clone the content.
                this.capturedImage = (Bitmap)eventArgs.Frame.Clone();

                // Exit there is a problem with data cloning.
                if (this.capturedImage == null)
                {
                    return;
                }

                // Convert image to RGB if it is gray scale.
                if (this.capturedImage.PixelFormat == PixelFormat.Format8bppIndexed)
                {
                    GrayscaleToRGB filter = new GrayscaleToRGB();
                    Bitmap temp = filter.Apply(this.capturedImage);
                    this.capturedImage.Dispose();
                    this.capturedImage = temp;
                }

                //TODO: Make preprocessing hear if it is needed.


                // Create temp buffer.
                List<ExtractedGlyphData> tmpGlyps = recognizer.FindGlyphs(this.capturedImage);



                // Rewrite the glyph buffer.
                this.recognisedGlyphs = tmpGlyps;
                // Display image data.

                this.DisplayGlyphData(this.recognisedGlyphs);

                if (this.capturedImage != null)
                {
                    this.DisplayGlyphs(this.capturedImage, this.recognisedGlyphs);

                    Bitmap dump = (Bitmap)this.capturedImage.Clone();
                    this.SendImageData(Utils.ResizeImage(dump, Properties.Settings.Default.ImageSize));
                    dump.Dispose();
                }

                this.SendGlyphData(this.recognisedGlyphs);
            }
        }

        #endregion

        #region Menu

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Filter = "XML files (*.XML)|*.XML|All files (*.*)|*.*";
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.LastDatabasePath = sfd.FileName;
                Properties.Settings.Default.Save();

                using (XmlTextWriter xmlOut = new XmlTextWriter(sfd.FileName, Encoding.UTF8))
                {
                    //xmlOut.Settings.Indent = true;

                    this.glyphDatabases.Save(xmlOut);
                }
            }
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = "XML files (*.XML)|*.XML|All files (*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                using (XmlTextReader xmlOut = new XmlTextReader(ofd.FileName))
                {
                    this.glyphDatabases.Load(xmlOut);
                }
            }
        }

        private void camerasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Create instance of caller.
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            // Display text.
            this.pbMain.Tag = item.Text;

            // Get device.
            VideoDevice videoDevice = (VideoDevice)item.Tag;

            // Stop if other stream was displaying.
            this.StopCapture();

            // Start the new stream.
            this.StartCapture(videoDevice.MonikerString);
        }
        
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SettingsForm sf = new SettingsForm())
            {
                sf.ShowDialog();
            }
        }

        private void connectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.ConnectVisionSystemViaMqtt();
        }

        private void dissconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.DisconnectVisionSystemFromMqtt();
        }

        #endregion


    }
}
