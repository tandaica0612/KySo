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
        private string _bearerToken = "Bearer eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYmYiOjE2MjAyMDUzOTUsImV4cCI6MTYyMDIwODk5NSwiaXNzIjoiaHR0cDovLzEwLjE1OS4yMS4yMTE6NTAwNiIsImF1ZCI6WyJodHRwOi8vMTAuMTU5LjIxLjIxMTo1MDA2L3Jlc291cmNlcyIsImF1dGhvcml6ZS12aWxpcy1hcGkiXSwiY2xpZW50X2lkIjoidmlsaXMtY2xpZW50LWNvZGUiLCJzdWIiOiIyYTU3M2Q5Yi1mYTJkLTQ0MTAtOGY5NS1iYjIxN2I0YzgzNDQiLCJhdXRoX3RpbWUiOjE2MjAyMDUzOTQsImlkcCI6ImxvY2FsIiwiQXNwTmV0LklkZW50aXR5LlNlY3VyaXR5U3RhbXAiOiJEWFhWWEpMVlo2REdCNk5ZWEdLQkYyQlFFQ0JCQ01RSyIsInByb2ZpbGUiOiJ1c2VyIiwicm9sZSI6WyJVc2VyIiwidXNlciIsImRhdGFFdmVudFJlY29yZHMudXNlciIsImRhdGFFdmVudFJlY29yZHMiLCJzZWN1cmVkRmlsZXMudXNlciIsInNlY3VyZWRGaWxlcyJdLCJwcmVmZXJyZWRfdXNlcm5hbWUiOiJ1c2VyIiwibmFtZSI6InVzZXIiLCJlbWFpbCI6ImFiYzEyM0BnbWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsInBob25lX251bWJlciI6IjExMTExMTExMTEyIiwicGhvbmVfbnVtYmVyX3ZlcmlmaWVkIjpmYWxzZSwiZ2l2ZW5fbmFtZSI6InVzZXIiLCJzY29wZSI6WyJvcGVuaWQiLCJwcm9maWxlIiwiYXV0aG9yaXplLXZpbGlzLWFwaSIsIm9mZmxpbmVfYWNjZXNzIl0sImFtciI6WyJwd2QiXX0.duHBZSYbSoHsowRhdhLqxIGyz0Uhw06NO3WQRD5ryN0cx7KEq5ohr8IHtzY4ppgwrtY5jVrXLzSCs3kQPZKFWnJj2UxkJYIu7ouzlQvGBhqqm4bdkWLfoq8DN03MPAinXrwh6suv6kcPzOct9y-fxBU3L8jcn-taq9BFjQvqMHW7L1JLW-Jl5tCQX-leQ9M96TxyzZ7XIeJNjVGw-5dqlb6iEeyxaKDr2ZrcMyQ_G9zxpZVlXROCoqAFDdFbWqsp9mHmCBAemlcnJJAh8zWpN5XWqks_C7Qf5JhX6MGc6p6PpvbFbf9tfks3i4y40_wDVatiFnjvu9YoipieNpq3Nw";
        private string _id = "36E47CEEF6374238B94FECC2B79729E9";
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
                    if (MessageBox.Show(string.Format("Ký số thành công, bạn có muốn xem lại chữ ký không. Nếu có vui lòng ấn YES và ký lại một lần nữa?"), "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
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
