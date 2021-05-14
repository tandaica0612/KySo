using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Vnpt.iLIS.SignThueXMLApp
{
    public partial class FormSignXmlThue : Form
    {
        private string _bearerToken = "Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2MjA5NjI2NzIsImV4cCI6MTYyMDk2NjI3MiwiaXNzIjoiaHR0cHM6Ly9pbGlzLXNzby52bnB0LnZuIiwiYXVkIjpbImh0dHBzOi8vaWxpcy1zc28udm5wdC52bi9yZXNvdXJjZXMiLCJhdXRob3JpemUtdmlsaXMtYXBpIl0sImNsaWVudF9pZCI6InZpbGlzLWNsaWVudC1jb2RlIiwic3ViIjoiNDEyZWI5YjQtNzczNS00OGQ1LTg1Y2QtNTQ1MjdkMWM3Zjg3IiwiYXV0aF90aW1lIjoxNjIwOTU1NTQxLCJpZHAiOiJsb2NhbCIsIkFzcE5ldC5JZGVudGl0eS5TZWN1cml0eVN0YW1wIjoiTE5aR0tDUkNKU0lGVkFEVU1OR0xDSTc2WFJESTI2T1MiLCJwcm9maWxlIjoidXNlciIsInJvbGUiOlsiVXNlciIsInVzZXIiLCJkYXRhRXZlbnRSZWNvcmRzLnVzZXIiLCJkYXRhRXZlbnRSZWNvcmRzIiwic2VjdXJlZEZpbGVzLnVzZXIiLCJzZWN1cmVkRmlsZXMiXSwicHJlZmVycmVkX3VzZXJuYW1lIjoidXNlciIsIm5hbWUiOiJ1c2VyIiwiZW1haWwiOiJ1c2VyQGdtYWlsLmNvbSIsImVtYWlsX3ZlcmlmaWVkIjpmYWxzZSwicGhvbmVfbnVtYmVyIjoiMDY4NzkxMzM1NDUiLCJwaG9uZV9udW1iZXJfdmVyaWZpZWQiOmZhbHNlLCJnaXZlbl9uYW1lIjoiVHLhuqduIFbEg24gw5p0Iiwic2NvcGUiOlsib3BlbmlkIiwicHJvZmlsZSIsImF1dGhvcml6ZS12aWxpcy1hcGkiLCJvZmZsaW5lX2FjY2VzcyJdLCJhbXIiOlsicHdkIl19.nPxSUJUBrknXZ6DCDcq0XqzRM1smhZK2axYLF1F2upVSGVSxnCt4NoxsAcFWGyWzrCZEzbfLz_YfZMaIGQ3ilXt9mg5-LVND7MY22k0lOSDLMXPizCgJiBhx_dtAaAf5YCRepzk8hcUCEd_Q05JXM__7mvPzqRVEudVJZ2V262stl21a5E1coJnSpyfXw5m8jOyc2DMeK7Ir9OrhMLeX5Utxz4FmXm-e8LWEyHtP8PxdLZLAfzTQupX2j6qWI1DztvMUzeMysBz-_za3QVsvSPZybXhlr2YyvrcT5Ld9QIpt5vLKiwexkGkDbFWjFkMuFSK9u9DFkr33_KmSXjcWdQ";
        private string _id = "0814B2D1FE18435590FE47587CBA4775";
        private string _xmlThue;
        public FormSignXmlThue()
        {
            InitializeComponent();
        }

        public FormSignXmlThue(string token, string id) : this()
        {
            _bearerToken = token;
            _id = id;
        }

        private void btnSignXml_Click(object sender, EventArgs e)
        {
            // Ký xml
            var binaryXmlPhieuSigned = DigitalSign.SignBodyContent(Encoding.UTF8.GetBytes(_xmlThue));
            XmlDocument xDocument = new XmlDocument();
            if (binaryXmlPhieuSigned != null)
            {
                xDocument.LoadXml(Encoding.UTF8.GetString(binaryXmlPhieuSigned));
                var stringXmlPhieuChuyenSigned = xDocument.OuterXml;
                // lưu kết quả ký về db
                bool resultSign = WebService.SignLienThongThueXml(_bearerToken, _id, stringXmlPhieuChuyenSigned);
                if (resultSign)
                {
                    // kiểm tra xem có xem lại chữ ký hay không
                    //if (MessageBox.Show(string.Format("Ký số"), "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                    if (MessageBox.Show(string.Format("Ký số thành công! Xem lại tài liệu đã ký?"), "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        var pdfFilePath = System.IO.Path.Combine(Vnpt.iLIS.Common.Controls.CommonMain.CommonGlobalVariables.RecentFolder.TemporaryFolderRoot, "pdfThue.pdf");
                        var pdfSignedFilePath = System.IO.Path.Combine(Vnpt.iLIS.Common.Controls.CommonMain.CommonGlobalVariables.RecentFolder.TemporaryFolderRoot, "SignPdfThue.pdf");
                        var html = wbThongTinChiTiet.DocumentText;
                        DigitalSign.SaveHtmlToPDF(pdfFilePath, html);
                        //Tọa dộ chữ ký trên pdf
                        var viTri = new iTextSharp.text.Rectangle(460, 780, 575, 830);
                        //font chữ ký
                        var fontFile = Application.StartupPath + "\\Resources\\times.ttf";
                        // ký pdf và hiển thị chữ ký cho người dùng
                        DigitalSign.SignWithThisCert(pdfFilePath, fontFile, viTri, pdfSignedFilePath,
                            () => { MessageBox.Show("Không có Certificate!"); },
                            () => { wbThongTinChiTiet.Url = new Uri(pdfSignedFilePath); });
                        
                    }
                    else
                    {
                        System.Windows.Forms.Application.Exit();
                    }
                }
                else
                {
                    MessageBox.Show("Có lỗi khi lưu vào cơ sở dữ liệu, vui lòng kiểm tra lại");
                }
            }
            else
            {
                MessageBox.Show("Có lỗi khi ký số vui lòng thử lại sau");
            }

        }

        private void FormSignXmlThue_Load(object sender, EventArgs e)
        {
            // gọi API lấy xml
            _bearerToken = _bearerToken.Replace("%20", " ");
            _xmlThue = WebService.GetLienThongThueXml(_bearerToken, _id);
            if (_xmlThue != null)
            {
                // hiển thị xml
                wbThongTinChiTiet.DocumentText = ClassPhieuChuyenXML.convertxmlPhieu2Html(_xmlThue);
            }
            else
            {
                MessageBox.Show("Có lỗi với token khi lấy XML, vui lòng kiểm tra lại");
            }


        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
