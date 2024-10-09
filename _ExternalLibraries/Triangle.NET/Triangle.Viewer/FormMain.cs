using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MeshExplorer.Controls;
using MeshExplorer.IO;
using TriangleNet;
using TriangleNet.Geometry;
using TriangleNet.IO;
using TriangleNet.Meshing;
using TriangleNet.Meshing.Algorithm;
using TriangleNet.Rendering;
using TriangleNet.Smoothing;
using TriangleNet.Voronoi;

namespace MeshExplorer
{
    public partial class FormMain : Form

    {
        
        //контекст привязки данных
        FormDataContext formDataContext;

        Settings settings;

        //Mesh mesh;
        //IPolygon input; //заменен на this.formDataContext.Input
        VoronoiBase voronoi;

        FormLog frmLog;
        FormGenerator frmGenerator;

        RenderManager renderManager;

        public FormMain()
        {
            InitializeComponent();
            //this.tabPage1.BindingContext = new Binding()
            ToolStripManager.Renderer = new DarkToolStripRenderer();
            
            formDataContext = new FormDataContext();
            this.DataContext = formDataContext;
            this.label1.DataBindings.Add(new Binding("Text", this.DataContext, "AmountPoints"));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            oldClientSize = this.ClientSize;

            settings = new Settings();

            renderManager = new RenderManager();

            IRenderControl control = new TriangleNet.Rendering.GDI.RenderControl();

            /*
            if (!renderManager.TryCreateControl("Triangle.Rendering.SharpGL.dll",
                new string[] { "SharpGL.dll" }, out control))
            {
                control = new TriangleNet.Rendering.GDI.RenderControl();

                if (frmLog == null)
                {
                    frmLog = new FormLog();
                }

                frmLog.AddItem("Failed to initialize OpenGL.", true);
            }
            //*/

            if (control != null)
            {
                InitializeRenderControl((Control)control);
                renderManager.Initialize(control, new TriangleNet.Rendering.GDI.LayerRenderer());
            }
            else
            {
                DarkMessageBox.Show("Ooops ...", "Failed to initialize renderer.");
            }
        }

        private void InitializeRenderControl(Control control)
        {
            this.splitContainer.SuspendLayout();
            this.splitContainer.Panel2.Controls.Add(control);

            var size = this.splitContainer.Panel2.ClientRectangle;

            // Initialize control
            control.BackColor = Color.Black;
            control.Dock = DockStyle.Fill;
            control.Font = new Font("Consolas", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            control.Location = new System.Drawing.Point(0, 0);
            control.Name = "renderControl1";
            control.Size = new Size(size.Width, size.Height);
            control.TabIndex = 0;
            control.Text = "renderControl1";

            this.splitContainer.ResumeLayout();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (splitContainer.Panel2.Bounds.Contains(e.Location))
            {
                var control = renderManager.Control as Control;

                // Set focus on the render control.
                if (control != null && !control.Focused)
                {
                    control.Focus();
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F3:
                    OpenWithDialog();
                    break;
                case Keys.F4:
                    Save();
                    break;
                case Keys.F5:
                    Reload();
                    break;
                case Keys.F8:
                    TriangulateOrRefine();
                    break;
                case Keys.F9:
                    Smooth();
                    break;
                case Keys.F12:
                    ShowLog();
                    break;
            }
        }

        void frmGenerator_InputGenerated(object sender, EventArgs e)
        {
            this.formDataContext.Input = sender as IPolygon;

            if (formDataContext.Input != null)
            {
                settings.CurrentFile = "tmp-" + DateTime.Now.ToString("HH-mm-ss");
                HandleNewInput();
            }
        }

        private void btnMesh_Click(object sender, EventArgs e)
        {
            TriangulateOrRefine();
        }

        private void btnSmooth_Click(object sender, EventArgs e)
        {
            Smooth();
        }

        #region Drag and drop

        private void frmDragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] args = (string[])e.Data.GetData(DataFormats.FileDrop);

                Open(args[0]);
            }
        }

        private void frmDragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] args = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (args.Length > 1)
                {
                    e.Effect = DragDropEffects.None;
                    return;
                }

                string file = args[0].ToLower();

                // Check if file extension is known
                if (FileProcessor.IsSupported(file))
                {
                    e.Effect = DragDropEffects.Copy;
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        #endregion

        #region Resize event handler

        bool isResizing = false;
        Size oldClientSize;

        private void ResizeHandler(object sender, EventArgs e)
        {
            // Handle window minimize and maximize
            if (!isResizing)
            {
                renderManager.Resize();
            }
        }

        private void ResizeEndHandler(object sender, EventArgs e)
        {
            isResizing = false;

            if (this.ClientSize != this.oldClientSize)
            {
                this.oldClientSize = this.ClientSize;
                renderManager.Resize();
            }
        }

        private void ResizeBeginHandler(object sender, EventArgs e)
        {
            isResizing = true;
        }

        #endregion

        #region State changes

        private void LockOnException()
        {
            btnMesh.Enabled = false;
            btnSmooth.Enabled = false;

            //menuFileSave.Enabled = false;
            //menuFileExport.Enabled = false;
            menuViewVoronoi.Enabled = false;
            menuToolsCheck.Enabled = false;
            menuToolsRcm.Enabled = false;

            settings.ExceptionThrown = true;
        }

        private void HandleNewInput()
        {
            // Reset formDataContext.Mesh
            formDataContext.Mesh = null;
            voronoi = null;

            // Reset state
            settings.RefineMode = false;
            settings.ExceptionThrown = false;

            // Reset buttons
            btnMesh.Enabled = true;
            btnMesh.Text = "Triangulate";
            btnSmooth.Enabled = false;

            // Update Statistic view
            statisticView.HandleNewInput(formDataContext.Input);

            // Clear voronoi
            menuViewVoronoi.Checked = false;

            // Disable menu items
            menuFileSave.Enabled = false;
            menuFileExport.Enabled = false;
            menuViewVoronoi.Enabled = false;
            menuToolsCheck.Enabled = false;
            menuToolsRcm.Enabled = false;

            // Render formDataContext.Input
            renderManager.Set(formDataContext.Input);

            // Update window caption
            this.Text = "Triangle.NET - Mesh Explorer - " + settings.CurrentFile;
        }

        private void HandleMeshImport()
        {
            voronoi = null;

            // Render formDataContext.Mesh
            renderManager.Set(formDataContext.Mesh, true);

            // Update window caption
            this.Text = "Triangle.NET - Mesh Explorer - " + settings.CurrentFile;

            // Update Statistic view
            statisticView.HandleMeshImport(formDataContext.Input, formDataContext.Mesh);

            // Set refine mode
            btnMesh.Enabled = true;
            btnMesh.Text = "Refine";

            settings.RefineMode = true;

            HandleMeshChange();
        }

        private void HandleMeshUpdate()
        {
            // Render formDataContext.Mesh
            renderManager.Set(formDataContext.Mesh, false);

            // Update Statistic view
            statisticView.HandleMeshUpdate(formDataContext.Mesh);

            HandleMeshChange();
        }

        private void HandleMeshChange()
        {
            // Update Statistic view
            statisticView.HandleMeshChange(formDataContext.Mesh);

            // TODO: Should the Voronoi diagram automatically update?
            voronoi = null;
            menuViewVoronoi.Checked = false;

            // Enable menu items
            menuFileSave.Enabled = true;
            menuFileExport.Enabled = true;
            menuViewVoronoi.Enabled = true;
            menuToolsCheck.Enabled = true;
            menuToolsRcm.Enabled = true;
        }

        #endregion

        #region Commands

        private void OpenWithDialog()
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.Filter = settings.OfdFilter;
            ofd.FilterIndex = settings.OfdFilterIndex;
            ofd.InitialDirectory = settings.OfdDirectory;
            ofd.FileName = "";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (Open(ofd.FileName))
                {
                    // Update folder settings
                    settings.OfdFilterIndex = ofd.FilterIndex;
                    settings.OfdDirectory = Path.GetDirectoryName(ofd.FileName);
                }
            }
        }

        private bool MeshDataExists(string filename)
        {
            string ext = Path.GetExtension(filename);

            if (ext == ".node" || ext == ".poly")
            {
                if (File.Exists(Path.ChangeExtension(filename, ".ele")))
                {
                    return true;
                }
            }

            return ext == ".ele";
        }

        private bool Open(string filename)
        {
            if (!FileProcessor.IsSupported(filename))
            {
                // TODO: show message.
            }
            else
            {
                if (MeshDataExists(filename))
                {
                    if (filename.EndsWith(".ele") || DarkMessageBox.Show("Import formDataContext.Mesh", Settings.ImportString,
                        "Do you want to import the formDataContext.Mesh?", MessageBoxButtons.YesNo) == DialogResult.OK)
                    {
                        formDataContext.Input = null;

                        try
                        {
                            formDataContext.Mesh = (Mesh)FileProcessor.Import(filename);
                        }
                        catch (Exception e)
                        {
                            DarkMessageBox.Show("Import formDataContext.Mesh error", e.Message, MessageBoxButtons.OK);
                            return false;
                        }

                        if (formDataContext.Mesh != null)
                        {
                            statisticView.UpdateStatistic(formDataContext.Mesh);

                            // Update settings
                            settings.CurrentFile = Path.GetFileName(filename);

                            HandleMeshImport();
                            btnSmooth.Enabled = true; // TODO: Remove
                        }
                        // else Message

                        return true;
                    }
                }

                formDataContext.Input = FileProcessor.Read(filename);
            }

            if (formDataContext.Input != null)
            {
                // Update settings
                settings.CurrentFile = Path.GetFileName(filename);

                HandleNewInput();
            }
            // else Message

            return true;
        }

        private void Save()
        {
            SaveFileDialog sfd = new SaveFileDialog();

            sfd.Filter = settings.SfdFilter;
            sfd.FilterIndex = settings.SfdFilterIndex;
            sfd.InitialDirectory = settings.SfdDirectory;
            sfd.FileName = "";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                FileProcessor.Write(formDataContext.Mesh, sfd.FileName);
            }
        }

        private void Reload()
        {
            if (formDataContext.Input != null)
            {
                formDataContext.Mesh = null;
                settings.RefineMode = false;
                settings.ExceptionThrown = false;

                HandleNewInput();
            }
        }

        private void TriangulateOrRefine()
        {
            if ((formDataContext.Input == null && !settings.RefineMode) || settings.ExceptionThrown)
            {
                return;
            }

            if (settings.RefineMode == false)
            {
                Triangulate();

                if (meshControlView.ParamQualityChecked)
                {
                    btnMesh.Text = "Refine";
                    btnSmooth.Enabled = formDataContext.Mesh.IsPolygon;
                }
            }
            else if (meshControlView.ParamQualityChecked)
            {
                Refine();
            }
        }

        private void Triangulate()
        {
            if (formDataContext.Input == null) return;

            var options = new ConstraintOptions();
            var quality = new QualityOptions();

            if (meshControlView.ParamConformDelChecked)
            {
                options.ConformingDelaunay = true;
            }

            if (meshControlView.ParamQualityChecked)
            {
                quality.MinimumAngle = meshControlView.ParamMinAngleValue;

                double maxAngle = meshControlView.ParamMaxAngleValue;

                if (maxAngle < 180)
                {
                    quality.MaximumAngle = maxAngle;
                }

                // Ignore area constraints on initial triangulation.

                //double area = slMaxArea.Value * 0.01;
                //if (area > 0 && area < 1)
                //{
                //    var size = formDataContext.Input.Bounds;
                //    double min = Math.Min(size.Width, size.Height);
                //    formDataContext.Mesh.SetOption(Options.MaxArea, area * min);
                //}
            }

            if (meshControlView.ParamConvexChecked)
            {
                options.Convex = true;
            }

            try
            {
                if (meshControlView.ParamSweeplineChecked)
                {
                    formDataContext.Mesh = (Mesh)formDataContext.Input.Triangulate(options, quality, new SweepLine());
                }
                else
                {
                    formDataContext.Mesh = (Mesh)formDataContext.Input.Triangulate(options, quality);
                }

                statisticView.UpdateStatistic(formDataContext.Mesh);

                HandleMeshUpdate();

                if (meshControlView.ParamQualityChecked)
                {
                    settings.RefineMode = true;
                }
            }
            catch (Exception ex)
            {
                LockOnException();
                DarkMessageBox.Show("Exception - Triangulate", ex.Message, MessageBoxButtons.OK);
            }

            UpdateLog();
        }

        private void Refine()
        {
            if (formDataContext.Mesh == null) return;

            double area = meshControlView.ParamMaxAreaValue;

            var quality = new QualityOptions();

            if (area > 0 && area < 1)
            {
                quality.MaximumArea = area * statisticView.Statistic.LargestArea;
            }

            quality.MinimumAngle = meshControlView.ParamMinAngleValue;

            double maxAngle = meshControlView.ParamMaxAngleValue;

            if (maxAngle < 180)
            {
                quality.MaximumAngle = maxAngle;
            }

            try
            {
                formDataContext.Mesh.Refine(quality, meshControlView.ParamConformDelChecked);

                statisticView.UpdateStatistic(formDataContext.Mesh);

                HandleMeshUpdate();
            }
            catch (Exception ex)
            {
                LockOnException();
                DarkMessageBox.Show("Exception - Refine", ex.Message, MessageBoxButtons.OK);
            }

            UpdateLog();
        }

        private void Renumber()
        {
            if (formDataContext.Mesh == null || settings.ExceptionThrown) return;

            bool tmp = Log.Verbose;
            Log.Verbose = true;

            formDataContext.Mesh.Renumber(NodeNumbering.CuthillMcKee);
            ShowLog();

            Log.Verbose = tmp;
        }

        private void Smooth()
        {
            if (formDataContext.Mesh == null || settings.ExceptionThrown) return;

            if (!formDataContext.Mesh.IsPolygon)
            {
                return;
            }

            var smoother = new SimpleSmoother();

            try
            {
                smoother.Smooth(this.formDataContext.Mesh);

                statisticView.UpdateStatistic(formDataContext.Mesh);

                HandleMeshUpdate();
            }
            catch (Exception ex)
            {
                LockOnException();
                DarkMessageBox.Show("Exception - Smooth", ex.Message, MessageBoxButtons.OK);
            }

            UpdateLog();
        }

        private bool CreateVoronoi()
        {
            if (formDataContext.Mesh == null)
            {
                return false;
            }

            if (formDataContext.Mesh.IsPolygon)
            {
                try
                {
                    this.voronoi = new BoundedVoronoi(formDataContext.Mesh);
                }
                catch (Exception ex)
                {
                    if (!meshControlView.ParamConformDelChecked)
                    {
                        DarkMessageBox.Show("Exception - Bounded Voronoi", Settings.VoronoiString, MessageBoxButtons.OK);
                    }
                    else
                    {
                        DarkMessageBox.Show("Exception - Bounded Voronoi", ex.Message, MessageBoxButtons.OK);
                    }

                    return false;
                }
            }
            else
            {
                this.voronoi = new StandardVoronoi(formDataContext.Mesh);
            }

            // HACK: List<Vertex> -> ICollection<Point> ? Nope, no way.
            //           Vertex[] -> ICollection<Point> ? Well, ok.
            renderManager.Set(voronoi.Vertices.ToArray(), voronoi.Edges, false);

            return true;
        }

        private void ShowLog()
        {
            if (frmLog == null)
            {
                frmLog = new FormLog();
            }

            UpdateLog();

            if (!frmLog.Visible)
            {
                frmLog.Show(this);
            }
        }

        private void UpdateLog()
        {
            if (frmLog != null)
            {
                frmLog.UpdateItems();
            }
        }

        #endregion

        #region Menu Handler

        private void menuFileOpen_Click(object sender, EventArgs e)
        {
            OpenWithDialog();
        }

        private void menuFileSave_Click(object sender, EventArgs ev)
        {
            if (formDataContext.Mesh != null)
            {
                Save();
            }
        }

        private void menuFileExport_Click(object sender, EventArgs e)
        {
            if (formDataContext.Mesh != null)
            {
                FormExport export = new FormExport();

                string file = settings.OfdDirectory;

                if (!file.EndsWith("\\"))
                {
                    file += "\\";
                }

                file += settings.CurrentFile;

                export.ImageName = Path.ChangeExtension(file, ".png");

                if (export.ShowDialog() == DialogResult.OK)
                {
                    int format = export.ImageFormat;
                    int size = export.ImageSize;
                    bool compress = export.UseCompression;

                    var writer = new ImageWriter();

                    writer.Export(this.formDataContext.Mesh, export.ImageName, format, size, compress);
                }
            }
        }

        private void menuFileQuit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void menuViewVoronoi_Click(object sender, EventArgs e)
        {
            if (this.voronoi == null)
            {
                menuViewVoronoi.Checked = CreateVoronoi();
            }
            else
            {
                bool visible = menuViewVoronoi.Checked;

                renderManager.Enable(4, !visible);
                menuViewVoronoi.Checked = !visible;
            }
        }

        private void menuViewLog_Click(object sender, EventArgs e)
        {
            ShowLog();
        }

        private void menuToolsGenerator_Click(object sender, EventArgs e)
        {
            if (frmGenerator == null || frmGenerator.IsDisposed)
            {
                frmGenerator = new FormGenerator();
                frmGenerator.InputGenerated += new EventHandler(frmGenerator_InputGenerated);
            }

            if (!frmGenerator.Visible)
            {
                frmGenerator.Show();
            }
            else
            {
                frmGenerator.Activate();
            }
        }

        private void menuToolsCheck_Click(object sender, EventArgs e)
        {
            if (formDataContext.Mesh != null)
            {
                bool save = Log.Verbose;

                Log.Verbose = true;

                bool isConsistent = MeshValidator.IsConsistent(formDataContext.Mesh);
                bool isDelaunay = MeshValidator.IsDelaunay(formDataContext.Mesh);

                Log.Verbose = save;

                if (isConsistent)
                {
                    Log.Instance.Info("Mesh topology appears to be consistent.");
                }

                if (isDelaunay)
                {
                    Log.Instance.Info("Mesh is (conforming) Delaunay.");
                }

                ShowLog();
            }
        }

        private void menuToolsTopology_Click(object sender, EventArgs e)
        {
            (new FormTopology()).ShowDialog(this);
        }

        private void menuToolsRcm_Click(object sender, EventArgs e)
        {
            Renumber();
        }

        #endregion
    }
}
