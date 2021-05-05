using ExpertPdf.HtmlToPdf;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Vnpt.iLIS.SignThueXMLApp
{
    public class DigitalSign
    {
        /// <summary>
        /// ký xml
        /// </summary>
        /// <param name="inputXmlData"></param>
        /// <returns></returns>
        public static byte[] SignBodyContent(byte[] inputXmlData)
        {
            try
            {
                if (inputXmlData == null)
                    throw new Exception("Dữ liệu cần ký số không hợp lệ");

                string signPath = "/DATA/BODY";
                //string storePath = "/DATA/SECURITY/SIGNATURE";

                //Tao xmlDoc va load xml document
                var xmlDoc = new XmlDocument { PreserveWhitespace = false };
                Stream stream = new MemoryStream(inputXmlData);
                xmlDoc.Load(stream);
                XmlNode nodeToSign = xmlDoc.SelectSingleNode(signPath);
                if (nodeToSign == null) throw new Exception("Node dữ liệu cần ký số không hợp lệ");
                var IdBody = "body";
                if (nodeToSign.Attributes != null && nodeToSign.Attributes["id"] != null)
                {
                    IdBody = nodeToSign.Attributes["id"].Value;
                }
                else
                {
                    XmlAttribute idAttr = xmlDoc.CreateAttribute("id");
                    idAttr.InnerText = IdBody;
                    nodeToSign.Attributes.Append(idAttr);
                }




                X509Certificate2 cert = GetCertificate();
                if (cert == null) throw new Exception("Không tìm thấy thông tin chữ ký nào được cài đặt");

                //Tao xml sign va add PrivateKey cho Object SignedXml
                SignedXml signxml = null;
                try
                {
                    signxml = new SignedXml(xmlDoc) { SigningKey = cert.PrivateKey };
                }
                catch (Exception ex)
                {
                    throw new Exception("Không tìm thấy token, vui lòng cắm token và thực hiện lại");
                }

                //Thiet lap phuong thuc SerializationC14
                signxml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigC14NWithCommentsTransformUrl; // Serialization ca comments.
                signxml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA1Url;

                //Thiet lap referent
                var reference = new Reference();
                reference.Uri = "#" + IdBody; //idAttr.InnerText;
                reference.DigestMethod = SignedXml.XmlDsigSHA1Url;
                XmlDsigEnvelopedSignatureTransform transform = new XmlDsigEnvelopedSignatureTransform();
                reference.AddTransform(transform);
                signxml.AddReference(reference);

                // Thuc hien signing
                signxml.ComputeSignature();

                // Tao va hiet lap thong tin cho the KeyInfo
                var keyInfo = new KeyInfo();

                //Them public key
                var rsaprovider = (RSACryptoServiceProvider)cert.PublicKey.Key;
                var rkv = new RSAKeyValue(rsaprovider);
                keyInfo.AddClause(rkv);

                //Lay va dien thong tin ve cert 
                var keyX509 = new KeyInfoX509Data(cert);
                keyX509.AddSubjectName(cert.SubjectName.Name);
                keyInfo.AddClause(keyX509);

                //Them ten signature
                //var keyNme = new KeyInfoName { Value = "LPTB Sign" };
                //keyInfo.AddClause(keyNme);

                //Gan thong tin keyinfo cho object xmlSigned
                signxml.KeyInfo = keyInfo;

                // Lay Node <Signature> tuw xmlSigned objec de gan vao xmlDocument
                XmlElement sig = signxml.GetXml();
                //var nodeSignature = xmlDoc.SelectSingleNode(storePath);
                //if (nodeSignature == null) throw new Exception("Node để lưu dữ liệu ký số không hợp lệ");
                //nodeSignature.AppendChild(sig);

                byte[] outData = Encoding.UTF8.GetBytes(sig.OuterXml);
                stream.Close();
                return outData;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return null;
        }
        private static X509Certificate2 GetCertificate()
        {
            var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certificates = store.Certificates;
            X509Certificate2 current = null;
            X509Certificate2Collection certificates2 = X509Certificate2UI.SelectFromCollection(certificates, "Danh sách chữ ký số", "Hãy chọn một chữ ký số để thực hiện ký", X509SelectionFlag.SingleSelection);
            if (certificates2.Count > 0)
            {
                X509Certificate2Enumerator enumerator = certificates2.GetEnumerator();
                enumerator.MoveNext();
                current = enumerator.Current;
            }
            store.Close();
            return current;
        }
        /// <summary>
        /// lưu convert html sang pdf
        /// </summary>
        /// <param name="strFileName"></param>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string SaveHtmlToPDF(string strFileName, string html)
        {
            try
            {
                PdfConverter pdfConverter = new PdfConverter();
                pdfConverter.PdfDocumentOptions.PdfPageSize = ExpertPdf.HtmlToPdf.PdfPageSize.A4;
                pdfConverter.PdfDocumentOptions.PdfCompressionLevel = ExpertPdf.HtmlToPdf.PdfCompressionLevel.Normal;
                pdfConverter.PdfDocumentOptions.LeftMargin = 0;
                pdfConverter.PdfDocumentOptions.RightMargin = 0;
                pdfConverter.PdfDocumentOptions.TopMargin = 0;
                pdfConverter.PdfDocumentOptions.BottomMargin = 0;
                pdfConverter.PdfDocumentOptions.GenerateSelectablePdf = true;
                pdfConverter.PdfFooterOptions.PageNumberText = "Trang";
                pdfConverter.PdfFooterOptions.ShowPageNumber = true;
                pdfConverter.LicenseKey = "eVJIWUFZSkBLWUBXSVlKSFdIS1dAQEBA";
                pdfConverter.SavePdfFromHtmlStringToFile(html, strFileName);
                return strFileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
        }

        /// <summary>
        /// Truyền vào 1 file PDF sau khi chọn chữ ký số mềm, nó sẽ ký vào file PDF kết quả
        /// </summary>
        /// <param name="SourcePdfFileName"></param>
        /// <param name="fontPath"></param>
        /// <param name="viTri"></param>
        /// <param name="DestPdfFileName"></param>
        /// <param name="failCallBack">Khi không có chữ ký số được chọn.</param>
        /// <param name="successCallBack">Khi ký xong.</param>
        public static void SignWithThisCert(string SourcePdfFileName, string fontPath, iTextSharp.text.Rectangle viTri, string DestPdfFileName,
            Action failCallBack, Action successCallBack)
        {
            System.Security.Cryptography.X509Certificates.X509Store store
                = new System.Security.Cryptography.X509Certificates.X509Store(System.Security.Cryptography.X509Certificates.StoreLocation.CurrentUser);
            store.Open(System.Security.Cryptography.X509Certificates.OpenFlags.ReadOnly);
            System.Security.Cryptography.X509Certificates.X509Certificate2 cert;
            System.Security.Cryptography.X509Certificates.X509Certificate2Collection sel =
                System.Security.Cryptography.X509Certificates.X509Certificate2UI.SelectFromCollection(store.Certificates, null, null,
                System.Security.Cryptography.X509Certificates.X509SelectionFlag.SingleSelection);
            if (sel.Count > 0)
                cert = sel[0];
            else
            {
                failCallBack();
                return;
            }
            string signName = cert.Subject.ToString();


            //signName += "_PL";

            Org.BouncyCastle.X509.X509CertificateParser cp = new Org.BouncyCastle.X509.X509CertificateParser();
            Org.BouncyCastle.X509.X509Certificate[] chain = new Org.BouncyCastle.X509.X509Certificate[] { cp.ReadCertificate(cert.RawData) };
            iTextSharp.text.pdf.security.IExternalSignature externalSignature = new iTextSharp.text.pdf.security.X509Certificate2Signature(cert, "SHA-1");
            iTextSharp.text.pdf.PdfReader pdfReader = new iTextSharp.text.pdf.PdfReader(SourcePdfFileName);
            System.IO.FileStream signedPdf = new System.IO.FileStream(DestPdfFileName, System.IO.FileMode.Create);
            iTextSharp.text.pdf.PdfStamper pdfStamper = iTextSharp.text.pdf.PdfStamper.CreateSignature(pdfReader, signedPdf, '\0', null, true);
            iTextSharp.text.pdf.PdfSignatureAppearance signatureAppearance = pdfStamper.SignatureAppearance;
            iTextSharp.text.Rectangle rect = viTri;
            iTextSharp.text.Font fnt = new iTextSharp.text.Font(iTextSharp.text.pdf.BaseFont.CreateFont(fontPath, iTextSharp.text.pdf.BaseFont.IDENTITY_H, iTextSharp.text.pdf.BaseFont.EMBEDDED), 5, 1, null);
            fnt.SetColor(255, 0, 0);
            signatureAppearance.Layer2Font = fnt;
            signatureAppearance.SetVisibleSignature(rect, 1, signName);
            signatureAppearance.SignatureRenderingMode = iTextSharp.text.pdf.PdfSignatureAppearance.RenderingMode.DESCRIPTION;
            iTextSharp.text.pdf.security.MakeSignature.SignDetached(signatureAppearance, externalSignature, chain, null, null, null, 0, iTextSharp.text.pdf.security.CryptoStandard.CMS);
            successCallBack();

        }
    }
}
