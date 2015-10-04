using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AForge;
using AForge.Imaging;
using AForge.Math;
using AForge.Vision;
using AForge.Vision.GlyphRecognition;
using DiO_CS_GliphRecognizer;
using System.Drawing.Imaging;
using AForge.Imaging.Filters;
using AForge.Video.DirectShow;
using AForge.Video;
using AForge.Math.Geometry;

namespace DiO_CS_GliphRecognizer
{
    public partial class MainForm : Form
    {
        #region Variables

        /// <summary>
        /// Input image name + path.
        /// </summary>
        private string imageFileName = null;

        /// <summary>
        /// Input image.
        /// </summary>
        private Bitmap loadedImage = null;

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

        private void DisplayData(List<ExtractedGlyphData> glyphs, string name)
        {
            foreach (ExtractedGlyphData gdata in glyphs)
            {
                if (gdata.RecognizedGlyph.Name == name)
                {
                    float yaw = 0.0f;
                    float pitch = 0.0f;
                    float roll = 0.0f;

                    gdata.Estimate(this.capturedImage.Size, true, out yaw, out pitch, out roll);

                    PointF pc = gdata.Centroid();
                    string gData = string.Format("Name: {0};\r\nAngles[deg]: (Y:{1:F3}, P{2:F3}, R:{3:F3} \r\nPosition[pix]: X:{4:F3} Y:{5:F3})", gdata.RecognizedGlyph.Name, yaw, pitch, roll, pc.X, pc.Y);
                    
                    // Display image.
                    if (this.lblGlyphData.InvokeRequired)
                    {
                        this.lblGlyphData.BeginInvoke((MethodInvoker)delegate()
                        {
                            this.lblGlyphData.Text = gData;
                        });
                    }
                    else
                    {
                        this.lblGlyphData.Text = gData;
                    }


                    //Console.WriteLine("{0}", estimationLabel);
                    //Application.DoEvents();
                }
            }
        }

        /// <summary>
        /// Display mage.
        /// </summary>
        /// <param name="image"></param>
        private void DisplayImage(Bitmap image)
        {
            // Display image.
            if (this.pbMain.InvokeRequired)
            {
                this.pbMain.BeginInvoke((MethodInvoker)delegate()
                {
                    this.pbMain.Image = image;
                    this.pbMain.Refresh();
                });
            }
            else
            {
                this.pbMain.Image = image;
                this.pbMain.Refresh();
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

                this.DisplayData(this.recognisedGlyphs, "Test1");

                this.DisplayImage(this.capturedImage);
            }
        }

        #endregion

        #region Buttons


        #endregion

        #region Picture Box

        private void pbMain_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.Clear(Color.White);
            if ((this.loadedImage != null || this.capturedImage != null) && this.imageProcessor.VisualizationType == VisualizationType.Image)
            {
                foreach (ExtractedGlyphData egd in this.recognisedGlyphs)
                {
                    //egd.DrawCentroid(e.Graphics);
                    //egd.DrawContour(e.Graphics);
                    egd.DrawPoints(e.Graphics);
                    egd.DrawCoordinates(e.Graphics);
                    //Console.WriteLine("{0}", egd.RecognizedGlyph.Name);
                }
            }            
        }

        #endregion
        
    }
}
