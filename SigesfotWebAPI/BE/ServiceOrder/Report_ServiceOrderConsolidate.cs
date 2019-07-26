using BE.Sigesoft;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NetPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.ServiceOrder
{
    public class Report_ServiceOrderConsolidate
    {
        public static bool GenerateOrderService(List<ServiceOrderDetailCustom> ListServiceOrder, OrganizationDto infoEmpresaPropietaria, ServiceOrderCustom dataServiceOrder, string filePDF)
        {
            Document document = new Document(PageSize.A4, 30f, 30f, 45f, 41f);
            document.SetPageSize(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePDF, FileMode.Create));
            try
            {
                
                pdfPage page = new pdfPage();
                writer.PageEvent = page;
                document.Open();

                #region Declaration Tables
                var subTitleBackGroundColor = new BaseColor(System.Drawing.Color.Gray);
                string include = string.Empty;
                List<PdfPCell> cells = null;
                List<PdfPCell> cells2 = null;
                float[] columnWidths = null;
                string[] columnValues = null;
                string[] columnHeaders = null;
                PdfPTable header2 = new PdfPTable(6);
                header2.HorizontalAlignment = Element.ALIGN_CENTER;
                header2.WidthPercentage = 100;
                float[] widths1 = new float[] { 16.6f, 18.6f, 16.6f, 16.6f, 16.6f, 16.6f };
                header2.SetWidths(widths1);
                PdfPTable companyData = new PdfPTable(6);
                companyData.HorizontalAlignment = Element.ALIGN_CENTER;
                companyData.WidthPercentage = 100;
                float[] widthscolumnsCompanyData = new float[] { 16.6f, 16.6f, 16.6f, 16.6f, 16.6f, 16.6f };
                companyData.SetWidths(widthscolumnsCompanyData);
                PdfPTable filiationWorker = new PdfPTable(4);
                PdfPTable table = null;
                PdfPCell cell = null;
                PdfPCell cell2 = null;
                document.Add(new Paragraph("\r\n"));
                float HeightTitle = 40f;
                float SingleHeight = 25f;
                #endregion

                #region Fonts
                Font fontTitle1 = FontFactory.GetFont("Calibri", 10, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.Black));
                Font fontTitle1_1 = FontFactory.GetFont("Calibri", 8, iTextSharp.text.Font.NORMAL, new BaseColor(System.Drawing.Color.Black));
                Font fontTitle2 = FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, new BaseColor(System.Drawing.Color.Black));
                Font fontTitleTable = FontFactory.GetFont("Calibri", 6, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.Black));
                Font fontTitleTableNegro = FontFactory.GetFont("Calibri", 6, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.Black));
                Font fontSubTitle = FontFactory.GetFont("Calibri", 6, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.White));
                Font fontSubTitleNegroNegrita = FontFactory.GetFont("Calibri", 6, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.Black));

                Font fontColumnValue = FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.NORMAL, new BaseColor(System.Drawing.Color.Black));
                Font fontColumnValueBold = FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.Black));
                Font fontColumnValueBold1 = FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.White));

                Font fontColumnValueApendice = FontFactory.GetFont("Calibri", 5, iTextSharp.text.Font.BOLD, new BaseColor(System.Drawing.Color.Black));
                #endregion




                PdfPCell cellLogo = new PdfPCell(new Phrase("SIN LOGO", fontTitle1)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = HeightTitle };
                if (infoEmpresaPropietaria != null)
                {
                    if (infoEmpresaPropietaria.b_Image != null)
                    {
                        Image imagenEmpresa = Image.GetInstance(HandlingItextSharp.GetImage(infoEmpresaPropietaria.b_Image));
                        imagenEmpresa.ScalePercent(25);
                        cellLogo = new PdfPCell(imagenEmpresa) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = HeightTitle };
                    }
                }
                var cellsTit = new List<PdfPCell>()
                {
                    cellLogo,
                    new PdfPCell(new Phrase("ORDEN DE SERVICIO \n" + dataServiceOrder.v_CustomServiceOrderId, fontTitle1)) {HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = HeightTitle},
                    new PdfPCell(new Phrase("", fontTitle1)) {HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = HeightTitle},
                };
                columnWidths = new float[] { 33f, 34f, 33f };
                table = HandlingItextSharp.GenerateTableFromCells(cellsTit, columnWidths, Rectangle.NO_BORDER, null, fontTitleTable);
                document.Add(table);

                #region CABECERA
                cellsTit = new List<PdfPCell>()
                {

                    new PdfPCell(new Phrase("Empresa Cliente :", fontColumnValueBold)) { HorizontalAlignment = Element.ALIGN_RIGHT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },
                    new PdfPCell(new Phrase(dataServiceOrder.v_OrganizationName, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },
                    new PdfPCell(new Phrase("Fecha Emisión :", fontColumnValueBold)) { HorizontalAlignment = Element.ALIGN_RIGHT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },
                    new PdfPCell(new Phrase(dataServiceOrder.d_InsertDate.ToString(), fontColumnValue)) { HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },

                    new PdfPCell(new Phrase("Dirección :", fontColumnValueBold)) { HorizontalAlignment = Element.ALIGN_RIGHT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight },
                    new PdfPCell(new Phrase(dataServiceOrder.v_OrganizationAdress, fontColumnValue)) { Colspan = 3, HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },

                    new PdfPCell(new Phrase("Representante :", fontColumnValueBold)) { HorizontalAlignment = Element.ALIGN_RIGHT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight },
                    new PdfPCell(new Phrase(dataServiceOrder.v_ContacName, fontColumnValue)) {  Colspan = 3, HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },

                    new PdfPCell(new Phrase("MEDIANTE LA PRESENTE SOLICITO EL SERVICIO DE EXAMENES MEDICOS OCUPACIONALES AL PERSONAL DE MI REPRESENTADA, EN SU CLINICA ESPECIALIZADA EN SALUD OCUPACIONAL.", fontColumnValue)) { Colspan=4, HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },

                };

                columnWidths = new float[] { 20f, 45f, 15f, 20f };
                table = HandlingItextSharp.GenerateTableFromCells(cellsTit, columnWidths, Rectangle.NO_BORDER, null, fontTitleTable);
                document.Add(table);
                #endregion




                #region DETALLES
                

                foreach (var detail in ListServiceOrder)
                {

                    cellsTit = new List<PdfPCell>()
                    {
                        new PdfPCell(new Phrase("PARA UN NÚMERO DE " + detail.i_NumberOfWorkerProtocol + " TRABAJADORES CON UN COSTO APROXIMADO DE S/"+ detail.r_Total +" NUEVOS SOLES, DEPENDIENDO DE LA FACTURACIÓN FINAL DEL NÚMERO DE ATENCIONES REALIZADAS EN LA CLÍNICA.", fontColumnValue))
                        { Colspan=4, HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },

                        new PdfPCell(new Phrase("", fontColumnValue)){ HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },
                        new PdfPCell(new Phrase("Exámenes Ocupacionales(" + detail.v_ProtocolTypeName + ")", fontColumnValue)){ HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },
                        new PdfPCell(new Phrase("Precio", fontColumnValue)){ HorizontalAlignment = Element.ALIGN_RIGHT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },
                        new PdfPCell(new Phrase("", fontColumnValue)){ HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },

                    };

                    columnWidths = new float[] { 20f, 40f, 20f, 20f };
                    table = HandlingItextSharp.GenerateTableFromCells(cellsTit, columnWidths, Rectangle.NO_BORDER, null, fontTitleTable);
                    document.Add(table);

                    float totalPrice = 0;
                    foreach (var comp in detail.ProtocolComponents)
                    {
                        totalPrice += comp.r_Price;
                        cellsTit = new List<PdfPCell>()
                        {
                            new PdfPCell(new Phrase("", fontColumnValue)){ HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },
                            new PdfPCell(new Phrase(comp.v_Name, fontColumnValue)){ HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight, UseVariableBorders = true, BorderColorLeft=BaseColor.BLACK,  BorderColorRight=BaseColor.BLACK,  BorderColorBottom=BaseColor.BLACK, BorderColorTop=BaseColor.BLACK },
                            new PdfPCell(new Phrase(comp.r_Price.ToString("N2"), fontColumnValue)){ HorizontalAlignment = Element.ALIGN_RIGHT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight, UseVariableBorders = true,  BorderColorLeft=BaseColor.BLACK,  BorderColorRight=BaseColor.BLACK,  BorderColorBottom=BaseColor.BLACK, BorderColorTop=BaseColor.BLACK },
                            new PdfPCell(new Phrase("", fontColumnValue)){ HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },
                        };

                        columnWidths = new float[] { 20f, 40f, 20f, 20f };
                        table = HandlingItextSharp.GenerateTableFromCells(cellsTit, columnWidths, Rectangle.NO_BORDER, null, fontTitleTable);
                        document.Add(table);
                    }
                    cellsTit = new List<PdfPCell>()
                    {
                        new PdfPCell(new Phrase("", fontColumnValue)){ HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },
                        new PdfPCell(new Phrase("Total ", fontColumnValue)){ HorizontalAlignment = Element.ALIGN_RIGHT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight },
                        new PdfPCell(new Phrase(totalPrice.ToString("N2"), fontColumnValue)){ HorizontalAlignment = Element.ALIGN_RIGHT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight, UseVariableBorders = true,  BorderColorLeft=BaseColor.BLACK,  BorderColorRight=BaseColor.BLACK,  BorderColorBottom=BaseColor.BLACK, BorderColorTop=BaseColor.BLACK },
                        new PdfPCell(new Phrase("", fontColumnValue)){ HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },
                    };

                    columnWidths = new float[] { 20f, 40f, 20f, 20f };
                    table = HandlingItextSharp.GenerateTableFromCells(cellsTit, columnWidths, Rectangle.NO_BORDER, null, fontTitleTable);
                    document.Add(table);
                }

                cellsTit = new List<PdfPCell>()
                {
                    new PdfPCell(new Phrase("", fontColumnValue)){ HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },
                    new PdfPCell(new Phrase("APROBADO: ", fontColumnValue)){ HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight },
                    new PdfPCell(new Phrase("_____________________________________________________", fontColumnValue)){ HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_BOTTOM, MinimumHeight = SingleHeight },
                    new PdfPCell(new Phrase("", fontColumnValue)){ HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },
                };

                columnWidths = new float[] { 20f, 10f, 40f, 30f };
                table = HandlingItextSharp.GenerateTableFromCells(cellsTit, columnWidths, Rectangle.NO_BORDER, null, fontTitleTable);
                document.Add(table);
                #endregion
                document.Close();
                writer.Close();
                writer.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                document.Close();
                writer.Close();
                writer.Dispose();
                return false;
            }
        }

        public static bool GenerateCotizacion(List<ServiceOrderDetailCustom> ListServiceOrder, OrganizationDto infoEmpresaPropietaria, ServiceOrderCustom dataServiceOrder, string filePDF)
        {
            Document document = new Document(PageSize.A4, 30f, 30f, 45f, 41f);
            document.SetPageSize(PageSize.A4);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePDF, FileMode.Create));
            try
            {

                pdfPage page = new pdfPage();
                writer.PageEvent = page;
                document.Open();

                #region Declaration Tables
                var subTitleBackGroundColor = new BaseColor(System.Drawing.Color.Gray);
                string include = string.Empty;
                List<PdfPCell> cells = null;
                List<PdfPCell> cells2 = null;
                float[] columnWidths = null;
                string[] columnValues = null;
                string[] columnHeaders = null;
                PdfPTable header2 = new PdfPTable(6);
                header2.HorizontalAlignment = Element.ALIGN_CENTER;
                header2.WidthPercentage = 100;
                float[] widths1 = new float[] { 16.6f, 18.6f, 16.6f, 16.6f, 16.6f, 16.6f };
                header2.SetWidths(widths1);
                PdfPTable companyData = new PdfPTable(6);
                companyData.HorizontalAlignment = Element.ALIGN_CENTER;
                companyData.WidthPercentage = 100;
                float[] widthscolumnsCompanyData = new float[] { 16.6f, 16.6f, 16.6f, 16.6f, 16.6f, 16.6f };
                companyData.SetWidths(widthscolumnsCompanyData);
                PdfPTable filiationWorker = new PdfPTable(4);
                PdfPTable table = null;
                PdfPCell cell = null;
                PdfPCell cell2 = null;
                document.Add(new Paragraph("\r\n"));
                float HeightTitle = 40f;
                float SingleHeight = 25f;
                #endregion

                #region Fonts
                Font fontTitle1 = FontFactory.GetFont("Calibri", 10, Font.BOLD, new BaseColor(System.Drawing.Color.Black));
                Font fontTitle1_1 = FontFactory.GetFont("Calibri", 8, Font.NORMAL, new BaseColor(System.Drawing.Color.Black));
                Font fontTitle2 = FontFactory.GetFont("Calibri", 7, Font.NORMAL, new BaseColor(System.Drawing.Color.Black));
                Font fontTitleTable = FontFactory.GetFont("Calibri", 6, Font.BOLD, new BaseColor(System.Drawing.Color.Black));
                Font fontTitleTableNegro = FontFactory.GetFont("Calibri", 6, Font.BOLD, new BaseColor(System.Drawing.Color.Black));
                Font fontSubTitle = FontFactory.GetFont("Calibri", 6, Font.BOLD, new BaseColor(System.Drawing.Color.White));
                Font fontSubTitleNegroNegrita = FontFactory.GetFont("Calibri", 6, Font.BOLD, new BaseColor(System.Drawing.Color.Black));

                Font fontColumnValue = FontFactory.GetFont("Calibri", 7, Font.NORMAL, new BaseColor(System.Drawing.Color.Black));
                Font fontColumnValueBold = FontFactory.GetFont("Calibri", 7, Font.BOLD, new BaseColor(System.Drawing.Color.Black));
                Font fontColumnValueBold1 = FontFactory.GetFont("Calibri", 7, Font.BOLD, new BaseColor(System.Drawing.Color.White));

                Font fontColumnValueApendice = FontFactory.GetFont("Calibri", 5, Font.BOLD, new BaseColor(System.Drawing.Color.Black));
                #endregion




                PdfPCell cellLogo = new PdfPCell(new Phrase("SIN LOGO", fontTitle1)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = HeightTitle };
                if (infoEmpresaPropietaria != null)
                {
                    if (infoEmpresaPropietaria.b_Image != null)
                    {
                        Image imagenEmpresa = Image.GetInstance(HandlingItextSharp.GetImage(infoEmpresaPropietaria.b_Image));
                        imagenEmpresa.ScalePercent(25);
                        cellLogo = new PdfPCell(imagenEmpresa) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = HeightTitle };
                    }
                }
                var cellsTit = new List<PdfPCell>()
                {
                    cellLogo,
                    new PdfPCell(new Phrase("PROPUESTA ECONÓMICA DE \nEXAMEN MEDICO OCUPACIONAL (EMO)", fontTitle1)) {HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = HeightTitle},
                    new PdfPCell(new Phrase("", fontTitle1)) {HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = HeightTitle},
                };
                columnWidths = new float[] { 33f, 34f, 33f };
                table = HandlingItextSharp.GenerateTableFromCells(cellsTit, columnWidths, Rectangle.NO_BORDER, null, fontTitleTable);
                document.Add(table);

                #region CABECERA
                cellsTit = new List<PdfPCell>()
                {

                    new PdfPCell(new Phrase("Empresa Cliente :", fontColumnValueBold)) { HorizontalAlignment = Element.ALIGN_RIGHT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },
                    new PdfPCell(new Phrase(dataServiceOrder.v_OrganizationName, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },
                    new PdfPCell(new Phrase("Fecha Emisión :", fontColumnValueBold)) { HorizontalAlignment = Element.ALIGN_RIGHT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },
                    new PdfPCell(new Phrase(dataServiceOrder.d_InsertDate.ToString(), fontColumnValue)) { HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },

                    new PdfPCell(new Phrase("Dirección :", fontColumnValueBold)) { HorizontalAlignment = Element.ALIGN_RIGHT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight },
                    new PdfPCell(new Phrase(dataServiceOrder.v_OrganizationAdress, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },

                    new PdfPCell(new Phrase("RUC :", fontColumnValueBold)) { HorizontalAlignment = Element.ALIGN_RIGHT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight },
                    new PdfPCell(new Phrase(dataServiceOrder.v_IdentificationNumber, fontColumnValue)) { HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },

                    new PdfPCell(new Phrase("Representante :", fontColumnValueBold)) { HorizontalAlignment = Element.ALIGN_RIGHT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight },
                    new PdfPCell(new Phrase(dataServiceOrder.v_ContacName, fontColumnValue)) {  Colspan = 3, HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },

                    new PdfPCell(new Phrase("Para la propuesta económica del EMO, se ha tomado en cuenta la Normativa Legal Nacional vigente establecida mediante la  Ley de Seguridad y Salud en el Trabajo Nº 29783, la Resolución Ministerial RM 312 - 2011 - MINSA , con sus respectivas  modificatorias y las exigencias mismas de la Empresa solicitante.", fontColumnValue)) { Colspan=4, HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },

                    new PdfPCell(new Phrase(infoEmpresaPropietaria.v_Name + ",Empresa especializada en Servicios  Medico Ocupacionales, cuenta con las licencias, autorizaciones  legales y la acreditación por DIGESA para la realización de los EMO. El Staff de Profesionales de Salud, cuenta con  Diplomados, Maestría en Salud Ocupacional, así como Certificaciones Internacionales (OIT, ALAT, CAOH) para la lectura e  interpretación de espirometría, audiometría y placas radiográficas.", fontColumnValue)) { Colspan=4, HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },


                };

                columnWidths = new float[] { 20f, 45f, 15f, 20f };
                table = HandlingItextSharp.GenerateTableFromCells(cellsTit, columnWidths, Rectangle.NO_BORDER, null, fontTitleTable);
                document.Add(table);
                #endregion




                #region DETALLES


                foreach (var detail in ListServiceOrder)
                {

                    cellsTit = new List<PdfPCell>()
                    {
                        new PdfPCell(new Phrase("Protocolo de atenciòn o Perfil:" + detail.v_GesoName + "-" + detail.v_ProtocolTypeName, fontColumnValue))
                        { Colspan=4, HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },

                        new PdfPCell(new Phrase("", fontColumnValue)){ HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },
                        new PdfPCell(new Phrase("Exámenes Ocupacionales", fontColumnValue)){ HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },
                        new PdfPCell(new Phrase("Precio", fontColumnValue)){ HorizontalAlignment = Element.ALIGN_RIGHT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },
                        new PdfPCell(new Phrase("", fontColumnValue)){ HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },

                    };

                    columnWidths = new float[] { 20f, 40f, 20f, 20f };
                    table = HandlingItextSharp.GenerateTableFromCells(cellsTit, columnWidths, Rectangle.NO_BORDER, null, fontTitleTable);
                    document.Add(table);

                    float totalPrice = 0;
                    foreach (var comp in detail.ProtocolComponents)
                    {
                        totalPrice += comp.r_Price;
                        cellsTit = new List<PdfPCell>()
                        {
                            new PdfPCell(new Phrase("", fontColumnValue)){ HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },
                            new PdfPCell(new Phrase(comp.v_Name, fontColumnValue)){ HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight, UseVariableBorders = true, BorderColorLeft=BaseColor.BLACK,  BorderColorRight=BaseColor.BLACK,  BorderColorBottom=BaseColor.BLACK, BorderColorTop=BaseColor.BLACK },
                            new PdfPCell(new Phrase(comp.r_Price.ToString("N2"), fontColumnValue)){ HorizontalAlignment = Element.ALIGN_RIGHT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight, UseVariableBorders = true,  BorderColorLeft=BaseColor.BLACK,  BorderColorRight=BaseColor.BLACK,  BorderColorBottom=BaseColor.BLACK, BorderColorTop=BaseColor.BLACK },
                            new PdfPCell(new Phrase("", fontColumnValue)){ HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },
                        };

                        columnWidths = new float[] { 20f, 40f, 20f, 20f };
                        table = HandlingItextSharp.GenerateTableFromCells(cellsTit, columnWidths, Rectangle.NO_BORDER, null, fontTitleTable);
                        document.Add(table);
                    }
                    cellsTit = new List<PdfPCell>()
                    {
                        new PdfPCell(new Phrase("", fontColumnValue)){ HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },
                        new PdfPCell(new Phrase("Total ", fontColumnValue)){ HorizontalAlignment = Element.ALIGN_RIGHT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight },
                        new PdfPCell(new Phrase(totalPrice.ToString("N2"), fontColumnValue)){ HorizontalAlignment = Element.ALIGN_RIGHT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight, UseVariableBorders = true,  BorderColorLeft=BaseColor.BLACK,  BorderColorRight=BaseColor.BLACK,  BorderColorBottom=BaseColor.BLACK, BorderColorTop=BaseColor.BLACK },
                        new PdfPCell(new Phrase("", fontColumnValue)){ HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },
                    };

                    columnWidths = new float[] { 20f, 40f, 20f, 20f };
                    table = HandlingItextSharp.GenerateTableFromCells(cellsTit, columnWidths, Rectangle.NO_BORDER, null, fontTitleTable);
                    document.Add(table);
                }
                PdfPCell cellFirma = new PdfPCell(new Phrase("SIN FIRMA", fontTitle1)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = HeightTitle };
                if (dataServiceOrder != null)
                {
                    if (dataServiceOrder.b_SignatureImage != null)
                    {
                        Image firmaMedico = Image.GetInstance(HandlingItextSharp.GetImage(dataServiceOrder.b_SignatureImage));
                        firmaMedico.ScalePercent(20);
                        cellFirma = new PdfPCell(firmaMedico) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = HeightTitle };
                    }
                }
                cellsTit = new List<PdfPCell>()
                {
                    new PdfPCell(new Phrase("", fontColumnValue)){ HorizontalAlignment = Element.ALIGN_RIGHT,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },
                    cellFirma,
                    new PdfPCell(new Phrase("", fontColumnValue)){ HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_BOTTOM, MinimumHeight = SingleHeight },

                    new PdfPCell(new Phrase("", fontColumnValue)){ HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_BOTTOM, MinimumHeight = SingleHeight },
                    new PdfPCell(new Phrase("_____________________________________________________", fontColumnValue)){ HorizontalAlignment = Element.ALIGN_CENTER,VerticalAlignment = Element.ALIGN_BOTTOM, MinimumHeight = SingleHeight },
                    new PdfPCell(new Phrase("", fontColumnValue)){ HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_BOTTOM, MinimumHeight = SingleHeight },

                    new PdfPCell(new Phrase("", fontColumnValue)){ HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_BOTTOM, MinimumHeight = SingleHeight },
                    new PdfPCell(new Phrase("Firma y Sello", fontColumnValue)){ HorizontalAlignment = Element.ALIGN_CENTER,VerticalAlignment = Element.ALIGN_MIDDLE, MinimumHeight = SingleHeight  },
                    new PdfPCell(new Phrase("", fontColumnValue)){ HorizontalAlignment = Element.ALIGN_LEFT,VerticalAlignment = Element.ALIGN_BOTTOM, MinimumHeight = SingleHeight },
                };

                columnWidths = new float[] { 50f, 30f, 20f };
                table = HandlingItextSharp.GenerateTableFromCells(cellsTit, columnWidths, Rectangle.NO_BORDER, null, fontTitleTable);
                document.Add(table);
                #endregion
                document.Close();
                writer.Close();
                writer.Dispose();
                return true;
            }
            catch (Exception ex)
            {
                document.Close();
                writer.Close();
                writer.Dispose();
                return false;
            }
        }




    }
}
