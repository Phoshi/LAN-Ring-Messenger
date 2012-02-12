using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;

namespace ToastNotifier {
    public partial class Notifier : Form {
        private string title = null;
        private string description = null;
        private Image image = null;
        private bool goingUp = true;
        private string category = null;
        private int heightToRiseTo;
        public Form parentForm = null;
        private int startTickCount;
        private int newHeightToRiseTo = -1;
        private NotifierOptions options;

        public Notifier(NotifierOptions newOptions, string notifierTitle, string notifierDescription, string notifierCategory, Image notifierImage = null, int notifierheightToRiseTo = -1) {
            InitializeComponent();
            startTickCount = Environment.TickCount;
            title = notifierTitle;
            description = notifierDescription;
            image = notifierImage;
            category = notifierCategory;
            options = newOptions;
            if (!options.enabled) {
                this.Close();
            }
            this.Top = Screen.PrimaryScreen.WorkingArea.Bottom;
            this.Left = Screen.PrimaryScreen.WorkingArea.Right - this.Width;
            Color magicPink = Color.FromArgb(255, 0, 255);
            this.BackColor = magicPink;
            this.TransparencyKey = magicPink;
            if (notifierheightToRiseTo == -1) {
                heightToRiseTo = Screen.PrimaryScreen.WorkingArea.Bottom;
            }
            else {
                heightToRiseTo = notifierheightToRiseTo;
            }

            int textStartingPoint = (image == null || !options.showImage) ? 10 : 300;
            int descriptionStartingHeight = title == null ? 10 : TextRenderer.MeasureText(title, options.titleFont).Height + 10;

            int previousHeight = 0, previousWidth = 0;
            while (TextRenderer.MeasureText(title, options.titleFont).Width + textStartingPoint > this.Width || this.Width - textStartingPoint < 200) {
                this.Width += 10;
                this.Left -= 10;
                if (this.Width == previousWidth) { //We've hit the maximum allowed size for a window!
                    break;
                }
                previousWidth = this.Width;
            }
            int textWidth = this.Width - textStartingPoint;
            string wrappedDescrption = WordWrap(description, textWidth, options.descriptionFont);
            while (TextRenderer.MeasureText(wrappedDescrption, options.descriptionFont).Height + TextRenderer.MeasureText(category, options.descriptionFont).Height + descriptionStartingHeight > this.Height) {
                this.Height += 10;
                if (this.Height == previousHeight) {
                    break;
                }
                previousHeight = this.Height;
            }
        }

        private void Notifier_Paint(object sender, PaintEventArgs e) {


            int textStartingPoint = (image == null || !options.showImage) ? 10 : 300;
            int descriptionStartingHeight = title == null ? 10 : TextRenderer.MeasureText(title, options.titleFont).Height + 10;

            int textWidth = this.Width - textStartingPoint;
            string wrappedDescrption = WordWrap(description, textWidth, options.descriptionFont);


            Rectangle fullFormRect = new Rectangle(0, 0, this.Width - 1, this.Height - 1);
            //Create the background
            e.Graphics.FillRectangle(options.backgroundBrush, fullFormRect);

            if (image != null && options.showImage) {
                //h/w = nh/nw
                //(h*nw)/w = nh
                //h*nw = nh * w
                //nw = (nh * w)/h
                int newWidth, newHeight, startingHeight;
                startingHeight = 0;
                newWidth = 300;
                newHeight = (int)((double)(image.Height * newWidth) / image.Width);
                if (newHeight > this.Height) {
                    int tHeight = newHeight;
                    int tWidth = newWidth;
                    newHeight = this.Height;
                    newWidth = (int)((double)(newHeight * tWidth) / tHeight);
                }
                else {
                    startingHeight = (int)((this.Height / 2.0f) - newHeight / 2.0f);
                }
                e.Graphics.DrawImage(image, 0, startingHeight, newWidth, newHeight);
                textStartingPoint = newWidth + 10;
            }

            if (title != null) {
                e.Graphics.DrawString(title, options.titleFont, options.foregroundBrush, textStartingPoint, 10);
            }
                e.Graphics.DrawString(wrappedDescrption, options.descriptionFont, options.foregroundBrush, textStartingPoint,
                                      descriptionStartingHeight);

            if (category != null) {
                List<string> catArray = category.Split(',').ToList();
                string newCategory = String.Join("   ", catArray.Select(item => String.Format("[{0}]", item.Trim())));

                Size measureText = TextRenderer.MeasureText(newCategory, options.descriptionFont);
                int categoryX = this.Width - measureText.Width;
                int categoryY = this.Height - measureText.Height;
                e.Graphics.FillRectangle(options.backgroundBrush, categoryX, categoryY, measureText.Width, measureText.Height);
                e.Graphics.DrawString(newCategory, options.descriptionFont, options.foregroundBrush, categoryX, categoryY);
            }


            //Draw the final bits, like borders
            e.Graphics.DrawRectangle(options.borderPen, fullFormRect);
        }

        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
        [StructLayout(LayoutKind.Sequential)]
        struct LASTINPUTINFO {
            public static readonly int SizeOf = Marshal.SizeOf(typeof(LASTINPUTINFO));

            [MarshalAs(UnmanagedType.U4)]
            public int cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dwTime;
        }

        private void movementTimer_Tick(object sender, EventArgs e) {
            if (goingUp) {
                for (int i = 0; i < 10; i++) {
                    if (this.Bottom < heightToRiseTo && this.Top > 0) {
                        movementTimer.Interval = (int)options.timeout + 1;
                        goingUp = false;
                        break;
                    }
                    if (this.Top > 0 && newHeightToRiseTo==-1) {
                        newHeightToRiseTo = heightToRiseTo + Screen.PrimaryScreen.WorkingArea.Height;
                    }
                    this.Top -= 1;
                    if (this.Top < 1-this.Height) {
                        this.Left -= this.Width;
                        this.Top = Screen.PrimaryScreen.Bounds.Bottom;
                        heightToRiseTo = newHeightToRiseTo;
                        newHeightToRiseTo = -1;
                    }
                }
            }
            else {
                LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
                lastInputInfo.cbSize = Marshal.SizeOf(lastInputInfo);
                lastInputInfo.dwTime = 0;
                if (GetLastInputInfo(ref lastInputInfo)) {
                    if (lastInputInfo.dwTime < startTickCount) {
                        movementTimer.Interval = (int)((options.timeout + 1) * 2);
                        return;
                    }
                }
                movementTimer.Interval = 1;
                this.Top += 10;
                if (this.Top > Screen.PrimaryScreen.Bounds.Bottom) {
                    this.Close();
                }
            }
        }

        static char[] splitChars = new char[] { ' ', '-', '\t' };

        private static string WordWrap(string wrapString, int width, Font font) {
            string[] words = Explode(wrapString, splitChars); //Like String.Split, but handles dashes a bit more fancy

            int currentLineLength = 0;
            StringBuilder stringBuilder = new StringBuilder(); //Needs more StringBuilding

            for (int i = 0; i < words.Length; i++) { //Iterate through each exploded word
                string word = words[i]; //Copy the current word into a local variable
                if (currentLineLength + TextRenderer.MeasureText(word, font).Width > width) { //If the new width is going to be longer than the allowable line length
                    if (currentLineLength > 0) { //If this isn't at the start of a new line
                        stringBuilder.Append(Environment.NewLine); //add a linebreak
                        currentLineLength = 0; //and go down one line!
                    }
                    while (word.Length > width) { //If this is at the start of a new line, we're in trouble - the word is too long to display
                        stringBuilder.Append(word.Substring(0, width - 1) + "-"); //So cut it down to size a little
                        word = word.Substring(width - 1);

                        stringBuilder.Append(Environment.NewLine);
                    }

                    word = word.TrimStart(); //And chop off any spaces at the start
                }
                stringBuilder.Append(word); //Now whatever's happened, add the word to our builder
                currentLineLength += TextRenderer.MeasureText(word, font).Width;
                if (word.Contains("\n")) {
                    currentLineLength = 0;
                }
            }

            return stringBuilder.ToString();
        }

        private static string[] Explode(string toExplode, char[] explodeOn) {
            List<string> parts = new List<string>();
            int startIndex = 0;
            while (true) {
                int index = toExplode.IndexOfAny(explodeOn, startIndex);

                if (index == -1) {
                    parts.Add(toExplode.Substring(startIndex));
                    return parts.ToArray();
                }

                string word = toExplode.Substring(startIndex, index - startIndex);
                char nextChar = toExplode.Substring(index, 1)[0];
                if (char.IsWhiteSpace(nextChar)) {
                    parts.Add(word);
                    parts.Add(nextChar.ToString());
                }
                else {
                    parts.Add(word + nextChar);
                }

                startIndex = index + 1;
            }
        }


        protected override bool ShowWithoutActivation {
            get {
                return true;
            }
        }

        private void Notifier_Click(object sender, EventArgs e) {
            if (parentForm != null) {
                if (parentForm.WindowState == FormWindowState.Minimized) {
                    parentForm.WindowState = FormWindowState.Normal;
                }
                parentForm.Select();
            }
            goingUp = false;
            movementTimer.Interval = 1;
            movementTimer.Start();
        }

        private void Notifier_MouseEnter(object sender, EventArgs e) {
            movementTimer.Stop();
        }

        private void Notifier_MouseLeave(object sender, EventArgs e) {
            movementTimer.Start();
        }

    }
    [Serializable()]
    public class NotifierOptions : ISerializable {
        public bool enabled;
        public bool showDescription;
        public bool showImage;
        public double timeout;
        public Font titleFont;
        public Font descriptionFont;
        public bool showNewChapters;
        public Pen borderPen;
        public SolidBrush backgroundBrush;
        public SolidBrush foregroundBrush;
        public bool previewMode = false;

        public NotifierOptions(bool newEnabled = true, int newtimeout = 5, bool newShowDescription = true,
            bool newShowImage = true, bool newShowNewChapters = true,
            Font newDescriptionFont = null, Font newTitleFont = null, Pen newBorderPen = null,
            SolidBrush newBackgroundBrush = null, SolidBrush newForegroundBrush = null) {
            enabled = newEnabled;
            showNewChapters = newShowNewChapters;
            showDescription = newShowDescription;
            showImage = newShowImage;
            descriptionFont = newDescriptionFont ?? new Font("Segoe UI", 10.0f);
            titleFont = newTitleFont ?? new Font("Segoe UI", 16.0f, FontStyle.Bold);
            timeout = TimeSpan.FromSeconds(newtimeout).TotalMilliseconds;
            borderPen = newBorderPen ?? new Pen(Color.Black);
            backgroundBrush = newBackgroundBrush ?? new SolidBrush(Color.White);
            foregroundBrush = newForegroundBrush ?? new SolidBrush(Color.Black);
        }

        public NotifierOptions(SerializationInfo info, StreamingContext ctxt) {
            enabled = info.GetBoolean("enabled");
            showDescription = info.GetBoolean("showDescription");
            showImage = info.GetBoolean("showImage");
            showNewChapters = info.GetBoolean("showNewChapters");
            timeout = info.GetDouble("timeout");
            titleFont = (Font) info.GetValue("titleFont", typeof (Font));
            descriptionFont = (Font) info.GetValue("descriptionFont", typeof (Font));
            borderPen = new Pen((Color) info.GetValue("borderPen", typeof (Color)));
            backgroundBrush = new SolidBrush((Color) info.GetValue("backgroundBrush", typeof (Color)));
            foregroundBrush = new SolidBrush((Color) info.GetValue("foregroundBrush", typeof (Color)));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt) {
            info.AddValue("enabled", enabled);
            info.AddValue("showDescription", showDescription);
            info.AddValue("showImage", showImage);
            info.AddValue("timeout", timeout);
            info.AddValue("titleFont", titleFont);
            info.AddValue("descriptionFont", descriptionFont);
            info.AddValue("showNewChapters", showNewChapters);
            info.AddValue("borderPen", borderPen.Color);
            info.AddValue("backgroundBrush", backgroundBrush.Color);
            info.AddValue("foregroundBrush", foregroundBrush.Color);
        }
    }
}
