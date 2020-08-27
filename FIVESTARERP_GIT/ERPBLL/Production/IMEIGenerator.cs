using ERPBLL.Common;
using ERPBLL.Inventory.Interface;
using ERPBLL.Production.Interface;
using ERPBO.Inventory.DTOModel;
using ERPBO.Production.DTOModel;
using ERPDAL.InventoryDAL;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Production
{
    public class IMEIGenerator : IIMEIGenerator
    {
        // Database
        private readonly IProductionUnitOfWork _productionDb;
        private readonly IInventoryUnitOfWork _inventoryDb;
        // Business
        private readonly IDescriptionBusiness _descriptionBusiness;
        private readonly ITempQRCodeTraceBusiness _tempQRCodeTraceBusiness;

        private readonly IGeneratedIMEIBusiness _generatedIMEIBusiness;
        //Repository //
        private readonly DescriptionRepository _descriptionRepository;
        private readonly TempQRCodeTraceRepository _tempQRCodeTraceRepository;

        public IMEIGenerator(IProductionUnitOfWork productionDb, IDescriptionBusiness descriptionBusiness, ITempQRCodeTraceBusiness tempQRCodeTraceBusiness, DescriptionRepository descriptionRepository, IInventoryUnitOfWork inventoryDb, IGeneratedIMEIBusiness generatedIMEIBusiness, TempQRCodeTraceRepository tempQRCodeTraceRepository)
        {
            // Database
            this._productionDb = productionDb;
            this._inventoryDb = inventoryDb;
            // Business
            this._descriptionBusiness = descriptionBusiness;
            this._tempQRCodeTraceBusiness = tempQRCodeTraceBusiness;
            this._generatedIMEIBusiness = generatedIMEIBusiness;
            
            //Repository //
            this._descriptionRepository = new DescriptionRepository(this._inventoryDb);
            this._tempQRCodeTraceRepository = new TempQRCodeTraceRepository(this._productionDb);
        }

        // Demon //
        public IMEIListWithSerial IMEIGeneratedList(long modelId, long tac, long serial, int noOfSim, int noOfHandset,long userId, long orgId)
        {
            IMEIListWithSerial iMEIListWithSerial = new IMEIListWithSerial();
            List<HandSetIMEI> IMEIs = new List<HandSetIMEI>();

            long startingNumber = 0;
            string allNewImei = string.Empty;
            long newSl = serial;
            startingNumber = serial; //+ 1;
            
            long s5 = serial;
            long stser = 0;
            for (int k = 0; k < noOfHandset; k++)
            {
                string value1 = "";
                HandSetIMEI handSet = new HandSetIMEI();
                handSet.SetIMEI = new List<string>();
                for (int m = 0; m < noOfSim * 1; m++)
                {
                    stser = s5 + m;
                    value1 = tac + "" + stser.ToString().PadLeft(6, '0');

                    if (value1.Length == 14)
                    {
                        string newImei = IMEIGenerate(value1);
                        handSet.SetIMEI.Add(newImei);
                        newSl++;
                        allNewImei += newImei + ",";
                    }
                }
                s5 = stser;
                IMEIs.Add(handSet);
                s5++;
            }
            iMEIListWithSerial.HandSetIMEIs = IMEIs;
            iMEIListWithSerial.Serial = newSl;
            return iMEIListWithSerial;
        }
        // Main //
        public List<string> IMEIGenerateByQRCode(string qrCode, int noOfSim, long userId, long orgId)
        {
            List<string> IMEIs = new List<string>();
            long startingNumber = 0;
            var qrCodeInfo = _tempQRCodeTraceBusiness.GetTempQRCodeTraceByCode(qrCode, orgId);
            if(qrCodeInfo != null)
            {
                string allNewImei = string.Empty;
                var model =_descriptionBusiness.GetDescriptionOneByOrdId(qrCodeInfo.DescriptionId.Value, orgId);
                long newSl = Utility.TryParseInt(model.EndPoint.ToString());
                if(model != null && !string.IsNullOrEmpty(model.TAC) && model.TAC.Trim() != "")
                {
                    startingNumber = Utility.TryParseInt(model.EndPoint.ToString()) ;
                    long s5 = startingNumber;
                    long stser = 0;
                    for (int k = 0; k < 1; k++)
                    {
                        string value1 = "";
                        for (int m = 0; m < noOfSim*1; m++)
                        {

                            stser = s5 + m;
                            value1 = model.TAC + "" + stser.ToString().PadLeft(6, '0');

                            if(value1.Length == 14)
                            {
                                string newImei = IMEIGenerate(value1);
                                IMEIs.Add(newImei);
                                newSl++;
                                allNewImei += newImei + ",";
                            }
                        }
                        s5 = stser;
                        s5++;
                    }
                    if (IMEIs.Count > 0)
                    {
                        model.EndPoint = newSl;
                        _descriptionRepository.Update(model);
                        _descriptionRepository.Save();
                        allNewImei = allNewImei.Substring(0, allNewImei.Length - 1);
                        List<GeneratedIMEIDTO> imeiList = new List<GeneratedIMEIDTO>() {
                            new GeneratedIMEIDTO(){
                              FloorId=qrCodeInfo.ProductionFloorId,ProductionFloorName=qrCodeInfo.ProductionFloorName,AssemblyLineId=qrCodeInfo.AssemblyId,AssemblyLineName=qrCodeInfo.AssemblyLineName,QRCode=qrCodeInfo.CodeNo,DescriptionId=qrCodeInfo.DescriptionId.Value,CodeId=qrCodeInfo.CodeId,DescriptionName=qrCodeInfo.ModelName,IMEI=allNewImei,OrganizationId=orgId,EUserId=userId }
                        };

                        _generatedIMEIBusiness.SaveGeneratedIMEI(imeiList, userId, orgId);
                        qrCodeInfo.IMEI = allNewImei;
                        _tempQRCodeTraceRepository.Update(qrCodeInfo);
                        _tempQRCodeTraceRepository.Save();
                    }
                }
            }
            return IMEIs;
        }

        // Generator Main Method//
        private string IMEIGenerate(string value1)
        {
            string imei;
            char[] characters = value1.ToCharArray();
            int a1, a3, a5, a7, a9, a11, a13, block;
            int a0, a2, a4, a6, a8, a10, a12, a14, a19, a20;

            int a111, a112, a113;
            int b111, b112, b113;
            int c111, c112, c113;
            int d111, d112, d113;
            int e111, e112, e113;
            int f111, f112, f113;
            int g111, g112, g113;
            int final1, final2, final3;
            //a0 = Convert.ToInt32(characters[0]);
            //a1 = Convert.ToInt32(characters[1]) * 2;
            //a2 = Convert.ToInt32(characters[2]);
            //a3 = Convert.ToInt32(characters[3]) * 2;
            //a4 = Convert.ToInt32(characters[4]);
            //a5 = Convert.ToInt32(characters[5]) * 2;
            //a6 = Convert.ToInt32(characters[6]);
            //a7 = Convert.ToInt32(characters[7]) * 2;
            //a8 = Convert.ToInt32(characters[8]);
            //a9 = Convert.ToInt32(characters[9]) * 2;
            //a10 = Convert.ToInt32(characters[10]);
            //a11 = Convert.ToInt32(characters[11]) * 2;
            //a12 = Convert.ToInt32(characters[12]);
            //a13 = Convert.ToInt32(characters[13]) * 2;

            a0 = int.Parse(characters[0].ToString());
            //block = (int.Parse(characters[1].ToString()));
            //double a50 = Char.GetNumericValue(characters[1]);

            //int cd = Convert.ToInt32(characters[1]);
            //a19 = cd * 2;
            //double a60 = (int)a50 * 2;
            //if (int.TryParse(characters[1].ToString(), out a19)) { }
            //a1 = (int)a50 * 2;
            a1 = int.Parse(characters[1].ToString()) * 2;
            a2 = int.Parse(characters[2].ToString());
            a3 = int.Parse(characters[3].ToString()) * 2;
            a4 = int.Parse(characters[4].ToString());
            a5 = int.Parse(characters[5].ToString()) * 2;
            a6 = int.Parse(characters[6].ToString());
            a7 = int.Parse(characters[7].ToString()) * 2;
            a8 = int.Parse(characters[8].ToString());
            a9 = int.Parse(characters[9].ToString()) * 2;
            a10 = int.Parse(characters[10].ToString());
            a11 = int.Parse(characters[11].ToString()) * 2;
            a12 = int.Parse(characters[12].ToString());
            a13 = int.Parse(characters[13].ToString()) * 2;

            if (a1 >= 10)
            {
                char[] characters2 = a1.ToString().ToCharArray();
                a111 = Convert.ToInt32(characters2[0].ToString());
                a112 = Convert.ToInt32(characters2[1].ToString());
                a113 = a111 + a112;
            }
            else
            {
                a113 = a1;
            }
            if (a3 >= 10)
            {
                char[] characters2 = a3.ToString().ToCharArray();
                b111 = Convert.ToInt32(characters2[0].ToString());
                b112 = Convert.ToInt32(characters2[1].ToString());
                b113 = b111 + b112;
            }
            else
            {
                b113 = a3;
            }
            if (a5 >= 10)
            {
                char[] characters2 = a5.ToString().ToCharArray();
                c111 = Convert.ToInt32(characters2[0].ToString());
                c112 = Convert.ToInt32(characters2[1].ToString());
                c113 = c111 + c112;
            }
            else
            {
                c113 = a5;
            }
            if (a7 >= 10)
            {
                char[] characters2 = a7.ToString().ToCharArray();
                d111 = Convert.ToInt32(characters2[0].ToString());
                d112 = Convert.ToInt32(characters2[1].ToString());
                d113 = d111 + d112;
            }
            else
            {
                d113 = a7;
            }
            if (a9 >= 10)
            {
                char[] characters2 = a9.ToString().ToCharArray();
                e111 = Convert.ToInt32(characters2[0].ToString());
                e112 = Convert.ToInt32(characters2[1].ToString());
                e113 = e111 + e112;
            }
            else
            {
                e113 = a9;
            }
            if (a11 >= 10)
            {
                char[] characters2 = a11.ToString().ToCharArray();
                f111 = Convert.ToInt32(characters2[0].ToString());
                f112 = Convert.ToInt32(characters2[1].ToString());
                f113 = f111 + f112;
            }
            else
            {
                f113 = a11;
            }
            if (a13 >= 10)
            {
                char[] characters2 = a13.ToString().ToCharArray();
                g111 = Convert.ToInt32(characters2[0].ToString());
                g112 = Convert.ToInt32(characters2[1].ToString());
                g113 = g111 + g112;
            }
            else
            {
                g113 = a13;
            }
            final1 = a0 + a113 + a2 + b113 + a4 + c113 + a6 + d113 + a8 + e113 + a10 + f113 + a12 + g113;

            int mod = final1 % 10;

            if (mod > 0)
            {
                mod = 10 - mod;
            }
            a14 = mod;
            imei = value1.ToString() + a14;
            return imei;
        }

        List<string> IIMEIGenerator.IMEIGenerate(long modelId, long tac, long serial, int noOfSim, int noOfHandset, long userId, long orgId)
        {
            throw new NotImplementedException();
        }
    }

}
