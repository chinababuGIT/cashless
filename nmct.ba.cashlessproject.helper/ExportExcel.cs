using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using nmct.ba.cashlessproject.model.Kassa;

namespace nmct.ba.cashlessproject.helper
{
    public class ExportExcel
    {
        
                //yield return c;//je moet dan geen aparte ienumaberlle van char aanmaken
        enum Alphabets { A = 1, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z }

        //private static void ExportToExcel()
        //{
        //    SpreadsheetDocument doc = SpreadsheetDocument.Create(file, SpreadsheetDocumentType.Workbook);

        //    WorkbookPart wbp = doc.AddWorkbookPart();
        //    wbp.Workbook = new Workbook();

        //    WorksheetPart wsp = wbp.AddNewPart<WorksheetPart>();
        //    SheetData data = new SheetData();
        //    wsp.Worksheet = new Worksheet(data);

        //    Sheets sheets = doc.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

        //    // Append a new worksheet and associate it with the workbook.
        //    Sheet sheet = new Sheet()
        //    {
        //        Id = doc.WorkbookPart.GetIdOfPart(wsp),
        //        SheetId = 1,
        //        Name = "results"
        //    };
        //    sheets.Append(sheet);

        //    Row header = new Row() { RowIndex = 1 };
        //    Cell name = new Cell() { CellReference = "A1", DataType = CellValues.String, CellValue = new CellValue("Naam") };
        //    Cell firstname = new Cell() { CellReference = "B1", DataType = CellValues.String, CellValue = new CellValue("Voornaam") };
        //    Cell group = new Cell() { CellReference = "C1", DataType = CellValues.String, CellValue = new CellValue("Groep") };
        //    Cell score = new Cell() { CellReference = "D1", DataType = CellValues.String, CellValue = new CellValue("Score") };

        //    header.Append(name, firstname, group, score);
        //    data.Append(header);

        //    wbp.Workbook.Save();
        //    doc.Close();
        //}
        public static void SaveFile(string path,List<Sale> sales)
        {
           // ExportToExcel();
            try
            {
                SpreadsheetDocument doc = SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook);

                WorkbookPart wbp = doc.AddWorkbookPart();
                wbp.Workbook = new Workbook();

                WorksheetPart wsp = wbp.AddNewPart<WorksheetPart>();
                SheetData data = new SheetData();
                wsp.Worksheet = new Worksheet(data);


                Sheets sheets = doc.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

                //nieuwe sheet aanmaken en connecten tot het workbook
                Sheet sheet = new Sheet()
                {
                    Id = doc.WorkbookPart.GetIdOfPart(wsp),
                    SheetId = 1,
                    Name = "Statistieken"
                };
                //sheet, blad toevoegen aan alle sheets
                sheets.Append(sheet);

                #region header aanmaken

                List<string> headerLijst = new List<string>();
                headerLijst.Add("SaleID");
                headerLijst.Add("ProductID");
                headerLijst.Add("Productnaam");
                headerLijst.Add("Productprijs");
                headerLijst.Add("KlantID");
                headerLijst.Add("Klantnaam");
                headerLijst.Add("Klantbalans");
                headerLijst.Add("RegisterID");
                headerLijst.Add("Registernaam");
                headerLijst.Add("Register device id");
                headerLijst.Add("Hoeveelheid");
                headerLijst.Add("Totale prijs");
                headerLijst.Add("Tijdstip aankoop");


                AddRowWith(headerLijst, 1, data);

                #endregion

                for (int i = 0; i < sales.Count; i++)
                {
                    UInt32 index = UInt32.Parse(i.ToString());
                    AddRowWith(sales[i], index, data);

                }





                //opslaan en sluiten
                wbp.Workbook.Save();
                doc.Close();
            }
            catch (Exception ex)
            {

                return;
            }
            
        }

        private static void AddRowWith(Sale sale,UInt32 index,SheetData data)
        {
            try
            {
                //header skippen
                index++;
                //index laten starten op 1 ipv 0

                index++;
                //rijen aanmaken
                Row header = new Row() { RowIndex = index };

                #region cellen

                Cell id = new Cell()
                {
                    CellReference = "A" + index.ToString(),
                    DataType = CellValues.String,
                    CellValue = new CellValue(sale.ID.ToString())
                };
                Cell productID = new Cell()
                {

                    CellReference = "B" + index.ToString(),
                    DataType = CellValues.String,
                    CellValue = new CellValue(sale.ProductID.ToString())
                };
                Cell productName = new Cell()
                {

                    CellReference = "C" + index.ToString(),
                    DataType = CellValues.String,
                    CellValue = new CellValue(sale.ProductName.ToString())
                };
                Cell productPrice = new Cell()
                {

                    CellReference = "D" + index.ToString(),
                    DataType = CellValues.String,
                    CellValue = new CellValue(sale.ProductPrice.ToString())
                };
                Cell customerID = new Cell()
                {

                    CellReference = "E" + index.ToString(),
                    DataType = CellValues.String,
                    CellValue = new CellValue(sale.CustomerID.ToString())
                };
                Cell customerName = new Cell()
                {

                    CellReference = "F" + index.ToString(),
                    DataType = CellValues.String,
                    CellValue = new CellValue(sale.CustomerName.ToString())
                };
                Cell customerBalance = new Cell()
                {

                    CellReference = "G" + index.ToString(),
                    DataType = CellValues.String,
                    CellValue = new CellValue(sale.CustomerBalance.ToString())
                };


                Cell registerID = new Cell()
                {

                    CellReference = "H" + index.ToString(),
                    DataType = CellValues.String,
                    CellValue = new CellValue(sale.RegisterID.ToString())
                };
                Cell registerName = new Cell()
                {

                    CellReference = "I" + index.ToString(),
                    DataType = CellValues.String,
                    CellValue = new CellValue(sale.RegisterName.ToString())
                };
                Cell registerDevice = new Cell()
                {

                    CellReference = "J" + index.ToString(),
                    DataType = CellValues.String,
                    CellValue = new CellValue(sale.RegisterDevice.ToString())
                };
                Cell amount = new Cell()
                {

                    CellReference = "K" + index.ToString(),
                    DataType = CellValues.String,
                    CellValue = new CellValue(sale.Amount.ToString())
                };
                Cell totalPrice = new Cell()
                {

                    CellReference = "L" + index.ToString(),
                    DataType = CellValues.String,
                    CellValue = new CellValue(sale.TotalPrice.ToString())
                };
                Cell timestamp = new Cell()
                {

                    CellReference = "M" + index.ToString(),
                    DataType = CellValues.String,
                    CellValue = new CellValue(sale.Timestamp.ToString())
                };

                #endregion


                //cellen aan rij toevoegen
                header.Append(id, productID, productName, productPrice, customerID, customerName, customerBalance, registerID, registerName, registerDevice, amount, totalPrice, timestamp);

                //rijen aan sheetdata toevoegen
                data.Append(header);
            }
            catch (Exception ex)
            {

                return;
            }
            
        }

        private static void AddRowWith(List<string> stringHeader, UInt32 index, SheetData data)
        {
            try
            {
                Row header = new Row() { RowIndex = index };

                #region cellen
                Cell id = new Cell()
                {
                    CellReference = "A" + index.ToString(),
                    DataType = CellValues.String,
                    CellValue = new CellValue(stringHeader[0])
                };
                Cell productID = new Cell()
                {

                    CellReference = "B" + index.ToString(),
                    DataType = CellValues.String,
                    CellValue = new CellValue(stringHeader[1])
                };
                Cell productName = new Cell()
                {

                    CellReference = "C" + index.ToString(),
                    DataType = CellValues.String,
                    CellValue = new CellValue(stringHeader[2])
                };
                Cell productPrice = new Cell()
                {

                    CellReference = "D" + index.ToString(),
                    DataType = CellValues.String,
                    CellValue = new CellValue(stringHeader[3])
                };
                Cell customerID = new Cell()
                {

                    CellReference = "E" + index.ToString(),
                    DataType = CellValues.String,
                    CellValue = new CellValue(stringHeader[4])
                };
                Cell customerName = new Cell()
                {

                    CellReference = "F" + index.ToString(),
                    DataType = CellValues.String,
                    CellValue = new CellValue(stringHeader[5])
                };
                Cell customerBalance = new Cell()
                {

                    CellReference = "G" + index.ToString(),
                    DataType = CellValues.String,
                    CellValue = new CellValue(stringHeader[6])
                };


                Cell registerID = new Cell()
                {

                    CellReference = "H" + index.ToString(),
                    DataType = CellValues.String,
                    CellValue = new CellValue(stringHeader[7])
                };
                Cell registerName = new Cell()
                {

                    CellReference = "I" + index.ToString(),
                    DataType = CellValues.String,
                    CellValue = new CellValue(stringHeader[8])
                };
                Cell registerDevice = new Cell()
                {

                    CellReference = "J" + index.ToString(),
                    DataType = CellValues.String,
                    CellValue = new CellValue(stringHeader[9])
                };
                Cell amount = new Cell()
                {

                    CellReference = "K" + index.ToString(),
                    DataType = CellValues.String,
                    CellValue = new CellValue(stringHeader[10])
                };
                Cell totalPrice = new Cell()
                {

                    CellReference = "L" + index.ToString(),
                    DataType = CellValues.String,
                    CellValue = new CellValue(stringHeader[11])
                };
                Cell timestamp = new Cell()
                {

                    CellReference = "M" + index.ToString(),
                    DataType = CellValues.String,
                    CellValue = new CellValue(stringHeader[12])
                };

                #endregion


                //cellen aan rij toevoegen
                header.Append(id, productID, productName, productPrice, customerID, customerName, customerBalance, registerID, registerName, registerDevice, amount, totalPrice, timestamp);

                //rijen aan sheetdata toevoegen
                data.Append(header);
            }
            catch (Exception ex)
            {

                return;
            }
           
        }

    }
}
