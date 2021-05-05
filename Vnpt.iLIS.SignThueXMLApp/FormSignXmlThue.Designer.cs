
namespace Vnpt.iLIS.SignThueXMLApp
{
    partial class FormSignXmlThue
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSignXmlThue));
            this.btnSignXml = new DevExpress.XtraEditors.SimpleButton();
            this.btnThoat = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.grpThongTinXML = new DevExpress.XtraEditors.GroupControl();
            this.wbThongTinChiTiet = new System.Windows.Forms.WebBrowser();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grpThongTinXML)).BeginInit();
            this.grpThongTinXML.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSignXml
            // 
            this.btnSignXml.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSignXml.Appearance.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSignXml.Appearance.Options.UseFont = true;
            this.btnSignXml.Image = ((System.Drawing.Image)(resources.GetObject("btnSignXml.Image")));
            this.btnSignXml.Location = new System.Drawing.Point(586, 16);
            this.btnSignXml.Margin = new System.Windows.Forms.Padding(2);
            this.btnSignXml.Name = "btnSignXml";
            this.btnSignXml.Size = new System.Drawing.Size(91, 22);
            this.btnSignXml.TabIndex = 5;
            this.btnSignXml.Text = "Ký XML";
            this.btnSignXml.Click += new System.EventHandler(this.btnSignXml_Click);
            // 
            // btnThoat
            // 
            this.btnThoat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnThoat.Appearance.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThoat.Appearance.Options.UseFont = true;
            this.btnThoat.Image = ((System.Drawing.Image)(resources.GetObject("btnThoat.Image")));
            this.btnThoat.Location = new System.Drawing.Point(681, 16);
            this.btnThoat.Margin = new System.Windows.Forms.Padding(2);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(80, 22);
            this.btnThoat.TabIndex = 4;
            this.btnThoat.Text = "Thoát";
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.btnSignXml);
            this.panelControl1.Controls.Add(this.btnThoat);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 401);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(772, 49);
            this.panelControl1.TabIndex = 6;
            // 
            // grpThongTinXML
            // 
            this.grpThongTinXML.AppearanceCaption.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.grpThongTinXML.AppearanceCaption.Options.UseFont = true;
            this.grpThongTinXML.Controls.Add(this.wbThongTinChiTiet);
            this.grpThongTinXML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpThongTinXML.Location = new System.Drawing.Point(0, 0);
            this.grpThongTinXML.Name = "grpThongTinXML";
            this.grpThongTinXML.Size = new System.Drawing.Size(772, 401);
            this.grpThongTinXML.TabIndex = 7;
            this.grpThongTinXML.Text = "Thông tin XML";
            // 
            // wbThongTinChiTiet
            // 
            this.wbThongTinChiTiet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbThongTinChiTiet.Location = new System.Drawing.Point(2, 22);
            this.wbThongTinChiTiet.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbThongTinChiTiet.Name = "wbThongTinChiTiet";
            this.wbThongTinChiTiet.Size = new System.Drawing.Size(768, 377);
            this.wbThongTinChiTiet.TabIndex = 1;
            // 
            // FormSignXmlThue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(772, 450);
            this.Controls.Add(this.grpThongTinXML);
            this.Controls.Add(this.panelControl1);
            this.Name = "FormSignXmlThue";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ký Xml Thuế";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormSignXmlThue_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grpThongTinXML)).EndInit();
            this.grpThongTinXML.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.SimpleButton btnSignXml;
        private DevExpress.XtraEditors.SimpleButton btnThoat;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.GroupControl grpThongTinXML;
        private System.Windows.Forms.WebBrowser wbThongTinChiTiet;
    }
}

