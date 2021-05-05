using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Vnpt.Framework.MessageBox;
using Vnpt.Framework.Report;
using Vnpt.iLIS.Tax.Controls.ApiModels;

namespace Vnpt.iLIS.SignThueXMLApp
{
    public class ClassPhieuChuyenXML
    {
        /// <summary>
        /// hiển thị xml
        /// </summary>
        /// <param name="xmlPhieu"></param>
        /// <returns></returns>
        public static string convertxmlPhieu2Html(string xmlPhieu)
        {
            var result = string.Empty;
            if (!string.IsNullOrEmpty(xmlPhieu))
            {
                var template = new GuiPhieuChuyenDataViewModel();
                var guiPhieuChuyenDataVM = Vnpt.iLIS.Tax.Controls.Services.OauthService.DeserializeXml(template, xmlPhieu);
                var danhMucThues = Vnpt.iLIS.Tax.Controls.Main.GetMaDanhMucThueFromXmlPhieuChuyenViewModel(guiPhieuChuyenDataVM);

                var phieuChuyenPDFVm = Vnpt.iLIS.Tax.Controls.Main.ConvertXmlPhieuChuyenVm2PhieuChuyenThuePdfViewModel(guiPhieuChuyenDataVM, danhMucThues);


                phieuChuyenPDFVm.NgayLap = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
                phieuChuyenPDFVm.NguoiLap = "Test";


                phieuChuyenPDFVm.NguoiDangNhap_TenDangNhap = "Test";
                phieuChuyenPDFVm.NguoiDangNhap_TenDayDu = "Test";



                var templateFile = Vnpt.iLIS.Common.Controls.CommonMain.GetTemplateFile("Template\\Workflow", "PhieuCNVTC.html");
                var templateFolder = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "Vnpt", "iLIS");
                if (!Directory.Exists(templateFolder)) Directory.CreateDirectory(templateFolder);

                if (System.IO.File.Exists(templateFile))
                {

                    using (HtmlWriter exDoc = new HtmlWriter(templateFile, templateFolder, "noname.html"))
                    {
                        DataSet dsTemplate = new DataSet("DataSet_Template");

                        var tableGiaDatChiTiet = Vnpt.iLIS.Tax.Controls.Main.ConvertXmlPhieuChuyenVm2DataTable(guiPhieuChuyenDataVM, danhMucThues);
                        dsTemplate.Tables.Add(tableGiaDatChiTiet);

                        exDoc.SetDataSet(dsTemplate);

                        exDoc.SetItems(phieuChuyenPDFVm);
                        result = exDoc.GetContent();
                    }
                }
                else
                {
                    CMessageBox.Show("Không tìm thấy file biểu mẫu\r\n" + templateFile, "Thiếu file biểu mẫu", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            return result;
        }
    }
}
