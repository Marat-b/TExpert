using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace iTextSharpWebAPI.Classes
{
    public class PDFDocument
    {
        //private string _FIO;
       // private string _Position;
        private string _Reason;
        private string _Conclusion;
        private int? _NumberExp;
        private string _RequestId;
        private string _Name;
        private string _InventoryNumber;
        private string _SerialNumber;
        private string _StartupDate;
        private string _DateExp;
        private bool? _IsServiceableEquipment ;
        private bool? _IsPartsSupply;
        private bool? _IsServiceable;
        private bool? _IsForWriteoff;
        private bool? _IsWarrantyRepair;
        private bool? _IsOrganizationRepair;
        private string _Staff;
        private string _Staff2;
        private byte[] _ImgData;

        public PDFDocument()
        {
            //_FIO = "Буранбаев М.Г.";
            //_Position = "Главный инженер Стерлитамакского СТП";
             _Reason = "";
             _Conclusion = "";
            _NumberExp = 0;
            _RequestId = "";
            _Name = "";
            _InventoryNumber = "";
            _SerialNumber = "";
            _StartupDate = "";
            _DateExp = "";
            _IsServiceableEquipment = false;
            _IsPartsSupply = false;
            _IsServiceable = false;
            _IsForWriteoff = false;
            _IsWarrantyRepair = false;
            _IsOrganizationRepair = false;
            _Staff = "";
            _Staff2 = "";
            _ImgData = null;

        }

        //public string FIO { get { return _FIO; } set { _FIO = value; } }
        //public string Position { get { return _Position;} set { _Position = value; } }
        public string Reason
        {
            get
            {
                return _Reason;
            }
            set {
                _Reason = value ?? "";
            }
        }

        public string Conclusion { get { return _Conclusion;} set { _Conclusion = value ?? ""; } }
        public int? NumberExp { get { return _NumberExp; } set { _NumberExp = value; } }
        public string RequestId { get { return _RequestId;} set { _RequestId = value ?? ""; } }
        public string Name { get { return _Name;} set { _Name = value ?? ""; } }
        public string InventoryNumber { get { return _InventoryNumber; } set { _InventoryNumber = value ?? ""; } }
        public string SerialNumber { get { return _SerialNumber; } set { _SerialNumber = value ?? ""; } }
        public string StartupDate { get { return _StartupDate; } set { _StartupDate = value ?? ""; } }
        public string DateExp { get { return _DateExp; } set { _DateExp = value ?? ""; } }
        public bool? IsServiceableEquipment { get { return IsServiceableEquipment; } set
        {
            _IsServiceableEquipment = value;
        } }
        public bool? IsPartsSupply { get { return _IsPartsSupply; } set { _IsPartsSupply = value; } }

        public bool? IsServiceable
        {
            get { return _IsServiceable; }
            set { _IsServiceable = value; }
        }

        public bool? IsForWriteoff { get { return _IsForWriteoff; } set { _IsForWriteoff = value; } }
        public bool? IsWarrantyRepair { get { return _IsWarrantyRepair; } set { _IsWarrantyRepair = value; } }
        public bool? IsOrganizationRepair { get { return _IsOrganizationRepair; } set { _IsOrganizationRepair = value; } }
        public string Staff { get { return _Staff; } set { _Staff = value ?? ""; } }
        public string Staff2 { get { return _Staff2; } set { _Staff2 = value ?? ""; } }
        public byte[] ImgData { get { return _ImgData; } set { _ImgData = value; } }

        public string GetFileName()
        {
            string FileName ="Act" + _NumberExp.ToString() + "_"  + _InventoryNumber + "_" + _SerialNumber + "_" + _RequestId;
            return FileName;
        }

        public byte[] GetPDFDocument()
        {
            byte[] stream;
            using (MemoryStream output = new MemoryStream())
            {
                BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\times.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font font = new iTextSharp.text.Font(baseFont, 12);
                Font fontCapital = new iTextSharp.text.Font(baseFont, 16, Font.BOLD);

                Document doc = new Document();
                PdfWriter writer = PdfWriter.GetInstance(doc,output);
                doc.Open();
                doc.Add(new Paragraph("\n", font));
                PdfPTable table = new PdfPTable(1);
                table.DefaultCell.Border = PdfPCell.NO_BORDER;
                //table.DefaultCell.VerticalAlignment = Element.ALIGN_RIGHT;
                table.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                table.AddCell(new Phrase("АКТ № " + _NumberExp.ToString(), fontCapital));
                table.AddCell(new Phrase("ТЕХНИЧЕСКОЙ ЭКСПЕРТИЗЫ", fontCapital));
                table.AddCell(new Phrase("к обращению ID " + _RequestId, fontCapital));

                doc.Add(table);
                doc.Add(new Chunk("\n"));
                //doc.Add(new Chunk("Настоящий акт составил - ",font));
                doc.Add(new Chunk("\n"));
                PdfPTable tableDate = new PdfPTable(1);
                tableDate.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                tableDate.DefaultCell.Border = PdfPCell.NO_BORDER;
                tableDate.AddCell(new Phrase("Дата: " + _DateExp, font));
                doc.Add(tableDate);

                doc.Add(new Chunk("\n"));
                doc.Add(new Chunk("Представителем ООО «Сбербанк-Сервис» прозведены работы по технической экспертизе следующего технического средства:", font));
                doc.Add(new Chunk("\n"));


                // Таблица с данными по оборудованию
                PdfPTable mainTable = new PdfPTable(5);
                mainTable.WidthPercentage = 100;
                mainTable.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                mainTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;



                PdfPHeaderCell headerCell0 = new PdfPHeaderCell();
                headerCell0.AddElement(new Phrase("Наименование", font));
                mainTable.AddCell(headerCell0);

                PdfPHeaderCell headerCell1 = new PdfPHeaderCell();
                headerCell1.AddElement(new Phrase("Инвентарный номер", font));
                mainTable.AddCell(headerCell1);

                PdfPHeaderCell headerCell2 = new PdfPHeaderCell();
                headerCell2.AddElement(new Phrase("Серийный номер", font));
                mainTable.AddCell(headerCell2);

                PdfPHeaderCell headerCell3 = new PdfPHeaderCell();
                headerCell3.AddElement(new Phrase("Год ввода в экспл.", font));
                mainTable.AddCell(headerCell3);

                PdfPHeaderCell headerCell4 = new PdfPHeaderCell();
                headerCell4.AddElement(new Phrase("Техническое состояние устройства (описание неисправности)", font));

                mainTable.AddCell(headerCell4);

                mainTable.AddCell(new Phrase(_Name, font));
                mainTable.AddCell(new Phrase(_InventoryNumber, font));
                mainTable.AddCell(new Phrase(_SerialNumber, font));
                mainTable.AddCell(new Phrase(_StartupDate, font));
                mainTable.AddCell(new Phrase(_Reason, font));

                doc.Add(mainTable);

                doc.Add(new Chunk("\n"));

                // Таблица о результате с решением
                PdfPTable conclusionTable = new PdfPTable(3);
                conclusionTable.WidthPercentage = 100;
                conclusionTable.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                conclusionTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;

                PdfPCell headerCell10 = new PdfPCell();
                headerCell10.AddElement(new Phrase("Заключение:", font));
                conclusionTable.AddCell(headerCell10);

                PdfPCell headerCell11 = new PdfPCell(new Phrase("Да", font));
                headerCell11.HorizontalAlignment = Element.ALIGN_CENTER;
                conclusionTable.AddCell(headerCell11);

                PdfPCell headerCell12 = new PdfPCell(new Phrase("Нет", font));
                headerCell12.HorizontalAlignment = Element.ALIGN_CENTER;
                conclusionTable.AddCell(headerCell12);

                conclusionTable.AddCell(new Phrase("Оборудование исправно", font));

                PdfPCell cellYesNo = new PdfPCell(new Phrase("+", font));
                cellYesNo.HorizontalAlignment = Element.ALIGN_CENTER;

                if ((_IsServiceableEquipment ?? false) )
                {
                    conclusionTable.AddCell(cellYesNo);
                    conclusionTable.AddCell(new Phrase("", font));
                }
                else
                {
                    conclusionTable.AddCell(new Phrase("", font));
                    conclusionTable.AddCell(cellYesNo);
                }

                conclusionTable.AddCell(new Phrase("Необходим ремонт по гарантии", font));
                if (_IsWarrantyRepair ?? false)
                {
                    conclusionTable.AddCell(cellYesNo);
                    conclusionTable.AddCell(new Phrase("", font));
                }
                else
                {
                    conclusionTable.AddCell(new Phrase("", font));
                    conclusionTable.AddCell(cellYesNo);
                }

                conclusionTable.AddCell(new Phrase("Возможен ремонт ООО «Сбербанк-Сервис»", font));
                if (_IsOrganizationRepair ?? false)
                {
                    conclusionTable.AddCell(cellYesNo);
                    conclusionTable.AddCell(new Phrase("", font));
                }
                else
                {
                    conclusionTable.AddCell(new Phrase("", font));
                    conclusionTable.AddCell(cellYesNo);
                }

                conclusionTable.AddCell(new Phrase("Требуется поставка запчастей", font));
                if (_IsPartsSupply ?? false)
                {
                    conclusionTable.AddCell(cellYesNo);
                    conclusionTable.AddCell(new Phrase("", font));
                }
                else
                {
                    conclusionTable.AddCell(new Phrase("", font));
                    conclusionTable.AddCell(cellYesNo);
                }

                conclusionTable.AddCell(new Phrase("Пригодно к эксплуатации", font));
                if (_IsServiceable ?? false)
                {
                    conclusionTable.AddCell(cellYesNo);
                    conclusionTable.AddCell(new Phrase("", font));
                }
                else
                {
                    conclusionTable.AddCell(new Phrase("", font));
                    conclusionTable.AddCell(cellYesNo);
                }

                conclusionTable.AddCell(new Phrase("Рекомендуется к списанию", font));
                if (_IsForWriteoff ?? false)
                {
                    conclusionTable.AddCell(cellYesNo);
                    conclusionTable.AddCell(new Phrase("", font));
                }
                else
                {
                    conclusionTable.AddCell(new Phrase("", font));
                    conclusionTable.AddCell(cellYesNo);
                }


                doc.Add(conclusionTable);

                //picture.ScaleToFit(50f, 50f);
                //picture.Alignment = Image.ALIGN_RIGHT | Image.TEXTWRAP;
                //picture.IndentationLeft = 3f;
                //picture.SpacingAfter = 3f;
                //picture.BorderWidthTop = 6f;
                //doc.Add(picture);

                doc.Add(new Chunk("\n"));
                doc.Add(new Paragraph("Дополнительные рекомендации: " + _Conclusion, font));


                doc.Add(new Paragraph("Представитель ООО «Сбербанк-Сервис»:", font));
                //doc.Add(new Chunk("\n"));

                // Таблица для выполнившего акт и его подписи
                // Ячейка для подписи
                PdfPTable tableSign = new PdfPTable(2);
                tableSign.WidthPercentage = 100;
                tableSign.DefaultCell.Border = PdfPCell.NO_BORDER;
                tableSign.DefaultCell.VerticalAlignment = Element.ALIGN_BOTTOM;
                tableSign.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
                // Ячейка для ФИО и должности
                PdfPCell cellText = new PdfPCell(new Phrase(_Staff, font));
                cellText.NoWrap = true;
                cellText.BorderColor = BaseColor.WHITE;
                cellText.HorizontalAlignment = Element.ALIGN_LEFT;
                cellText.VerticalAlignment = Element.ALIGN_BOTTOM;
                tableSign.AddCell(cellText);
                PdfPCell cellSign;
                if (_ImgData != null)
                {
                    Image picture = Image.GetInstance(_ImgData);
                    picture.ScaleToFit(100f, 100f);
                    picture.BorderWidthTop = 0f;
                    //picture.Alignment = Image.ALIGN_RIGHT | Image.TEXTWRAP;
                    cellSign = new PdfPCell(picture);
                }
                else
                {
                     cellSign = new PdfPCell();
                }

                
                cellSign.BorderColor = BaseColor.WHITE;
                cellSign.HorizontalAlignment = Element.ALIGN_CENTER;
                tableSign.AddCell(new PdfPCell(cellSign));
                doc.Add(tableSign);

                doc.Add(new Paragraph("Управления по сервису Уральского Банка ООО «Сбербанк-Сервис»", font));
                doc.Add(new Paragraph("Работы приняты:", font));
                if (!string.IsNullOrWhiteSpace(_Staff2))
                {
                    doc.Add(new Paragraph(_Staff2, font));
                }
                else
                {
                    doc.Add(new Paragraph("_____________________________________________________________________ "));
                }
                


                doc.Close();
                stream = output.ToArray();
            }

            return stream;
        }
    }
}