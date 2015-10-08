using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

using AForge;
using AForge.Vision.GlyphRecognition;
using AForge.Imaging.Filters;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing.Drawing2D;

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
        /// Image processor.
        /// </summary>
        private GlyphImageProcessor imageProcessor = new GlyphImageProcessor();

        /// <summary>
        /// Sync objec.
        /// </summary>
        private Object syncLock = new Object();

        /// <summary>
        /// Recogniesed database.
        /// </summary>
        private List<ExtractedGlyphData> recognisedGlyphs = new List<ExtractedGlyphData>();

        /// <summary>
        /// Video source.
        /// </summary>
        VideoCaptureDevice videoSource = null;

        /// <summary>
        /// Captured image.
        /// </summary>
        private Bitmap capturedImage = null;

        /// <summary>
        /// Image point of the object to estimate pose for.
        /// </summary>
        private AForge.Point[] imagePoints = new AForge.Point[4];

        /// <summary>
        /// Colors used to highlight points on image.
        /// </summary>
        private Color[] pointsColors = new Color[4]
        {
            Color.Yellow, Color.Blue, Color.Red, Color.Lime
        };

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

        #region Construcotr

        /// <summary>
        /// Constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Private

        /// <summary>
        /// Refresh the list displaying available databases of glyphss
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

            // set the database to image processor ...
            this.imageProcessor.GlyphDatabase = this.glyphDatabases[dbName];

            /*
            // List the database size.
            List<string> dbNames = this.glyphDatabases.GetDatabaseNames();
            
            foreach (string name in dbNames)
            {
                GlyphDatabase db = glyphDatabases[name];
                Console.WriteLine(string.Format("Name:{0}; Size:{1}x{2}", name, db.Size, db.Size));
            }
            */
        }

        /// <summary>
        /// Refresh the list displaying available databases of glyphss
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

            // set the database to image processor ...
            this.imageProcessor.GlyphDatabase = this.glyphDatabases[dbName];
        }

        private List<ExtractedGlyphData> ProcessImage(Bitmap image)
        {
            List<ExtractedGlyphData> tmpBuffer = new List<ExtractedGlyphData>();
            tmpBuffer.Clear();

            if (this.capturedImage.PixelFormat == PixelFormat.Format8bppIndexed)
            {
                // convert image to RGB if it is grayscale
                GrayscaleToRGB filter = new GrayscaleToRGB();
                Bitmap temp = filter.Apply(this.capturedImage);
                this.capturedImage.Dispose();
                this.capturedImage = temp;
            }

            List<ExtractedGlyphData> glyphs = imageProcessor.ProcessImage(image);

            foreach (ExtractedGlyphData glyph in glyphs)
            {
                if ((glyph.RecognizedGlyph != null) &&
                     (glyph.RecognizedGlyph.UserData != null) &&
                     (glyph.RecognizedGlyph.UserData is GlyphVisualizationData) &&
                     (glyph.IsTransformationDetected))
                {

                    tmpBuffer.Add(glyph);

                    //List<System.Drawing.Point> points = this.ConvertToPoint(glyph.Quadrilateral);
                    //this.lblGlyphData.Text = String.Format("Glyph: {0}, Center: {1}", glyph.RecognizedGlyph.Name, glyph.Centroid().ToString());
                }
            }

            return tmpBuffer;
        }

        private void DisplayGlyphData(List<ExtractedGlyphData> glyphs, string name)
        {
            foreach (ExtractedGlyphData gdata in glyphs)
            {
                if (gdata.RecognizedGlyph.Name == name)
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
                }
            }
        }

        /// <summary>
        /// Display mage.
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
                        if (this.capturedImage != null && this.imageProcessor.VisualizationType == VisualizationType.Image)
                        {
                            foreach (ExtractedGlyphData glyph in glyphs)
                            {
                                //egd.DrawCentroid(e.Graphics);
                                glyph.DrawContour(g);
                                glyph.DrawPoints(g);
                                glyph.DrawCoordinates(g);
                                //Console.WriteLine("{0}", egd.RecognizedGlyph.Name);
                            }
                        }            
                    }

                    Bitmap rszImage = this.ResizeImage(image, this.pbMain.Size);

                    this.pbMain.Image = rszImage;
                });
            }
            else
            {
                using (Graphics g = Graphics.FromImage((Image)image))
                {
                    //e.Graphics.Clear(Color.White);
                    if (this.capturedImage != null && this.imageProcessor.VisualizationType == VisualizationType.Image)
                    {
                        foreach (ExtractedGlyphData egd in this.recognisedGlyphs)
                        {
                            //egd.DrawCentroid(e.Graphics);
                            egd.DrawContour(g);
                            egd.DrawPoints(g);
                            egd.DrawCoordinates(g);
                            //Console.WriteLine("{0}", egd.RecognizedGlyph.Name);
                        }
                    }
                }

                Bitmap rszImage = this.ResizeImage(image, this.pbMain.Size);

                this.pbMain.Image = rszImage;
            }
        }

        /// <summary>
        /// Convert AForge integer points to .NET point.
        /// </summary>
        /// <param name="points">AForge points.</param>
        /// <returns>DotNET points.</returns>
        private List<System.Drawing.Point> ConvertToPoint(List<IntPoint> points)
        {
            if (points == null)
            {
                throw new NullReferenceException("Points can not be null.");
            }

            List<System.Drawing.Point> netPoints = new List<System.Drawing.Point>();

            foreach (IntPoint p in points)
            {
                netPoints.Add(new System.Drawing.Point(p.X, p.Y));
            }

            return netPoints;
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
        /// Resize bitmap images.
        /// </summary>
        /// <param name="imgToResize">Source image.</param>
        /// <param name="size">Output size.</param>
        /// <returns>Resized new bitmap.</returns>
        private Bitmap ResizeImage(Bitmap sourceImage, Size size)
        {

            int sourceWidth = sourceImage.Width;
            int sourceHeight = sourceImage.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
            }
            else
            {
                nPercent = nPercentW;
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bitmapImage = new Bitmap(destWidth, destHeight);
            Graphics graphics = Graphics.FromImage((Image)bitmapImage);

            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.DrawImage(sourceImage, 0, 0, destWidth, destHeight);
            graphics.Dispose();

            return bitmapImage;
        }

        #endregion

        #region Main Form

        private void MainForm_Load(object sender, EventArgs e)
        {
            // load configuratio
            Configuration config = Configuration.Instance;

            if (config.Load(this.glyphDatabases))
            {
                //this.LoadGlyphDatabases12();
                this.LoadGlyphDatabases5();
                
                try
                {
                    bool autoDetectFocalLength = bool.Parse(config.GetConfigurationOption(autoDetectFocalLengthOption));
                    this.imageProcessor.GlyphSize = float.Parse(config.GetConfigurationOption(glyphSizeOption));
                    
                    if (!autoDetectFocalLength)
                    {
                        this.imageProcessor.CameraFocalLength = float.Parse(config.GetConfigurationOption(focalLengthOption));
                    }


                    // Enumerate video devices
                    FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                    // Create video source
                    this.videoSource = new VideoCaptureDevice(videoDevices[1].MonikerString);
                    // Set NewFrame event handler
                    this.videoSource.NewFrame += new NewFrameEventHandler(this.video_NewFrame);
                    // Start the video source
                    this.videoSource.Start();


                }
                catch(Exception exception)
                {
                    MessageBox.Show(exception.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }            
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Stop and free the webcam object if application is closing.
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

        #region Frame Graber

        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            lock (this.syncLock)
            {
                // Clone the content.
                this.capturedImage = (Bitmap)eventArgs.Frame.Clone();

                if (this.capturedImage == null)
                {
                    return;
                }

                this.recognisedGlyphs = this.ProcessImage(this.capturedImage);

                this.DisplayGlyphData(this.recognisedGlyphs, "Test1");

                this.DisplayGlyphs(this.capturedImage, this.recognisedGlyphs);
            }
        }

        #endregion

        #region Buttons


        #endregion
        
    }
}
