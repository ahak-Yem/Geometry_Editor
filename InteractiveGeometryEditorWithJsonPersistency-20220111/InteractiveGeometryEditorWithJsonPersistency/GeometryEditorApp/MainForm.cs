using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GeometryLibrary;
using MathLibrary;
using static GeometryLibrary.Curve;
using Point = MathLibrary.Point;

namespace GeometryEditorApp
{
    public partial class MainForm : Form
    {
        private Graphics _pictureBoxGraphics = null;
        private Drawing _drawing = new Drawing(new Curve[0]);

        private CurveClickHandler _clickHandler = null;
        private Curve _currentCurve = null;

        public MainForm()
        {
            InitializeComponent();
            _drawing.Redraw += OnRedraw;
            StatusManager.Instance.StatusMessageChanged += (s, e) => toolStripStatusLabel.Text = e.Message;
        }

        private void SetGraphicsTransformToWorld(Graphics g)
        {
            g.ResetTransform();
            g.ScaleTransform(1f, -1f);
            g.TranslateTransform(0f, -pictureBox.Height);
        }

        private void toolStripButtonLine_Click(object sender, EventArgs e)
        {
            _currentCurve = null;
            _clickHandler = Line.LineClickHandler;
            StatusManager.Instance.SetStatus(Line.StartMessage);
        }

        private void toolStripButtonCircle_Click(object sender, EventArgs e)
        {
            _currentCurve = null;
            _clickHandler = Circle.CircleClickHandler;
            StatusManager.Instance.SetStatus(Circle.StartMessage);
        }

        private void toolStripButtonPolyline_Click(object sender, EventArgs e)
        {
            _currentCurve = null;
            _clickHandler = Polyline.PolylineClickHandler;
            StatusManager.Instance.SetStatus(Polyline.StartMessage);
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            SetGraphicsTransformToWorld(e.Graphics);

            if (showLinesButton.Checked)
            {
                _drawing.GetLines().DrawElements(e.Graphics);
            }

            if (showCirclesButton.Checked)
            {
                _drawing.GetCircles().DrawElements(e.Graphics);
            }

            if (showPolylinesButton.Checked)
            {
                _drawing.GetPolylines().DrawElements(e.Graphics);
            }
        }

        private void OnRedraw(Object sender, EventArgs e)
        {
            pictureBox.Invalidate();
        }

        private void showLinesButton_Click(object sender, EventArgs e)
        {
            pictureBox.Invalidate();
        }

        private void showCirclesButton_Click(object sender, EventArgs e)
        {
            pictureBox.Invalidate();
        }

        private void showPolylinesButton_Click(object sender, EventArgs e)
        {
            pictureBox.Invalidate();
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (_clickHandler != null)
            {
                string statusMessage;
                ClickResult result = _clickHandler(e.Location, e.Button, pictureBox.Height, ref _currentCurve, out statusMessage);
                StatusManager.Instance.SetStatus(statusMessage);
                if (result == ClickResult.Canceled)
                {
                    _currentCurve = null;
                }
                else if (result == ClickResult.Finished)
                {
                    _drawing.AddCurve(_currentCurve);
                    _currentCurve = null;
                }
            }
        }

        private void pictureBox_Resize(object sender, EventArgs e)
        {
            pictureBox.Invalidate();
        }

        private void toolStripButtonNew_Click(object sender, EventArgs e)
        {
            _drawing.RemoveAllCurves();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButtonNew_Click(sender, e);
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                AddExtension = true,
                DefaultExt = ".drw",
                CheckPathExists = true,
                Filter = "Drawing files (*.drw)|",
                InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Title = "Please select a drawing file to save."
            };
            sfd.ShowDialog();
            if (sfd.CheckPathExists)
                _drawing.Save(sfd.FileName);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButtonSave_Click(sender, e);
        }

        private void toolStripButtonOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                AddExtension = true,
                DefaultExt = ".drw",
                CheckFileExists = true,
                Filter = "Drawing files (*.drw)|",
                InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Title = "Please select a drawing file to open."
            };
            ofd.ShowDialog();
            _drawing.Load(ofd.FileName);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButtonOpen_Click(sender, e);
        }
    }
}
