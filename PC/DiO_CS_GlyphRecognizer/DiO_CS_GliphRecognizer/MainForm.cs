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

using AForge.Imaging.Filters;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Vision.GlyphRecognition;
using AForge.Vision.GlyphRecognition.Data;
using AForge.Vision.GlyphRecognition.Utils;

namespace DiO_CS_GliphRecognizer
{
    public partial class MainForm : Form
    {

        #region Variables

        /// <summary>
        /// Collection of glyph databases.
        /// </summary>
        private GlyphDatabases glyphDatabases;

        /// <summary>
        /// Glyph recognizer to use for glyph recognition in video.
        /// </summary>
        private GlyphRecognizer recognizer;

        /// <summary>
        /// Recognized database.
        /// </summary>
        private List<ExtractedGlyphData> recognisedGlyphs;

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
        private object syncLock;

        /// <summary>
        /// Image point of the object to estimate pose for.
        /// </summary>
        private AForge.Point[] imagePoints;

        /// <summary>
        /// Colors used to highlight points on image.
        /// </summary>
        private Color[] pointsColors;

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

            this.glyphDatabases = new GlyphDatabases();
            this.syncLock = new object();
            this.recognisedGlyphs = new List<ExtractedGlyphData>();
            this.imagePoints = new AForge.Point[4];
            this.pointsColors = new Color[4]
            {
                Color.Yellow,
                Color.Blue,
                Color.Red,
                Color.Lime
            };
        }

        #endregion

        #region Private

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
            const string dbName = "ExampleSize5";
            const int glyphSize = 5;
            
            // Create glyph.
            string glyphName1 = "Test1";
            byte[,] glyphData1 = new byte[glyphSize, glyphSize]
            {
                {0, 0, 0, 0, 0},
                {0, 0, 1, 0, 0},
                {0, 1, 1, 1, 0},
                {0, 1, 0, 0, 0},
                {0, 0, 0, 0, 0}
            };

            Glyph testGlyph1 = new Glyph(glyphName1, glyphData1);
            testGlyph1.UserData = new GlyphVisualizationData(Color.Purple);

            // Create glyph.
            string glyphName2 = "Test2";
            byte[,] glyphData2 = new byte[glyphSize, glyphSize]
            {
                {0, 0, 0, 0, 0},
                {0, 1, 0, 1, 0},
                {0, 0, 1, 0, 0},
                {0, 1, 0, 0, 0},
                {0, 0, 0, 0, 0}
            };

            Glyph testGlyph2 = new Glyph(glyphName2, glyphData2);
            testGlyph2.UserData = new GlyphVisualizationData(Color.Blue);

            // Create glyph.
            string glyphName3 = "Test3";
            byte[,] glyphData3 = new byte[glyphSize, glyphSize]
            {
                {0, 0, 0, 0, 0},
                {0, 1, 0, 1, 0},
                {0, 0, 1, 0, 0},
                {0, 0, 1, 1, 0},
                {0, 0, 0, 0, 0}
            };

            Glyph testGlyph3 = new Glyph(glyphName3, glyphData3);
            testGlyph3.UserData = new GlyphVisualizationData(Color.Green);

            // Create database.
            GlyphDatabase lGlyphDatabase = new GlyphDatabase(glyphSize);

            // Add glyph to database.
            lGlyphDatabase.Add(testGlyph1);
            lGlyphDatabase.Add(testGlyph2);
            lGlyphDatabase.Add(testGlyph3);

            // Add database.
            this.glyphDatabases.AddGlyphDatabase(dbName, lGlyphDatabase);

            this.recognizer = new GlyphRecognizer(glyphSize);

            // set the database to image processor ...
            this.recognizer.GlyphDatabase = this.glyphDatabases[dbName]; ;
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
        /// <param name="glyphs"></param>
        /// <param name="name"></param>
        private void DisplayGlyphData(List<ExtractedGlyphData> glyphs, string name)
        {
            foreach (ExtractedGlyphData gdata in glyphs)
            {
                if (gdata.RecognizedGlyph != null && gdata.RecognizedGlyph.Name == name)
                {
                    // Estimate orientation and position.
                    float yaw = 0.0f;
                    float pitch = 0.0f;
                    float roll = 0.0f;
                    gdata.EstimateOrientation(true, out yaw, out pitch, out roll);
                    AForge.Point[] pp = gdata.PerformProjection();
                    float area = gdata.Area();

                    string textData = string.Format("Name: {0};\r\nAngles[deg]: Y: {1:F3}, P: {2:F3}, R: {3:F3} \r\nPosition[pix]: X: {4:F3} Y: {5:F3}\r\n Size: {5:F3}", gdata.RecognizedGlyph.Name, yaw, pitch, roll, pp[0].X, pp[0].Y, area);
                    
                    // Display image.
                    if (this.lblGlyphData.InvokeRequired)
                    {
                        this.lblGlyphData.BeginInvoke(
                            (MethodInvoker)delegate()
                            {
                                this.lblGlyphData.Text = textData;
                            });
                    }
                    else
                    {
                        this.lblGlyphData.Text = textData;
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// Display image.
        /// </summary>
        /// <param name="image"></param>
        private void DisplayGlyphs(Bitmap image, List<ExtractedGlyphData> glyphs)
        {
            // Display image.
            if (this.pbMain.InvokeRequired)
            {
                this.pbMain.BeginInvoke((MethodInvoker)delegate()
                {
                    using (Graphics g = Graphics.FromImage((Image)image))
                    {
                        //e.Graphics.Clear(Color.White);
                        if (this.capturedImage != null)
                        {
                            foreach (ExtractedGlyphData egd in glyphs)
                            {
                                //GlyphDrawer.DrawCentroid(e.Graphics);
                                GlyphDrawer.DrawContour(egd, g);
                                GlyphDrawer.DrawPoints(egd, g);
                                GlyphDrawer.DrawCoordinates(egd, g);

                                //Console.WriteLine("{0}", egd.RecognizedGlyph.Name);
                            }
                        }            
                    }

                    Bitmap rszImage = Utils.ResizeImage(image, this.pbMain.Size);

                    this.pbMain.Image = rszImage;
                });
            }
            else
            {
                using (Graphics g = Graphics.FromImage((Image)image))
                {
                    //e.Graphics.Clear(Color.White);
                    if (this.capturedImage != null)
                    {
                        foreach (ExtractedGlyphData egd in this.recognisedGlyphs)
                        {
                            //egd.DrawCentroid(e.Graphics);
                            GlyphDrawer.DrawContour(egd, g);
                            GlyphDrawer.DrawPoints(egd, g);
                            GlyphDrawer.DrawCoordinates(egd, g);
                            //Console.WriteLine("{0}", egd.RecognizedGlyph.Name);
                        }
                    }
                }

                Bitmap rszImage = Utils.ResizeImage(image, this.pbMain.Size);

                this.pbMain.Image = rszImage;
            }
        }

        #endregion

        #region Main Form

        private void MainForm_Load(object sender, EventArgs e)
        {
            //this.LoadGlyphDatabases12();
            this.LoadGlyphDatabases5();

            try
            {
                // Enumerate video devices
                FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                // Create video source
                this.videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
                // Set NewFrame event handler
                this.videoSource.NewFrame += new NewFrameEventHandler(this.video_NewFrame);
                // Start the video source
                this.videoSource.Start();


            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Stop and free the web cam object if application is closing.
            if (this.videoSource != null && this.videoSource.IsRunning)
            {
                // Signal to stop when you no longer need capturing.
                this.videoSource.SignalToStop();
                // Remove event handler
                videoSource.NewFrame -= new NewFrameEventHandler(video_NewFrame);
                // Dispose the camera capture device.
                this.videoSource = null;
            }
        }

        #endregion

        #region Frame Grabber

        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
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
                this.DisplayGlyphData(this.recognisedGlyphs, "Test1");
                // 
                this.DisplayGlyphs(this.capturedImage, this.recognisedGlyphs);

            }
        }

        #endregion

        #region Buttons


        #endregion
        
    }
}
