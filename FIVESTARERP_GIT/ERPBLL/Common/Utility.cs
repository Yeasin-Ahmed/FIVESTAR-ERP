using ERPBO.Common;
using QRCoder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Drawing;
using System.Drawing.Imaging;
using ERPBO.Production.ViewModels;

namespace ERPBLL.Common
{
    public class Utility
    {

        private readonly static string _imagByte = string.Format(@"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAIYAAAAMCAYAAACnUsUvAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAIhSURBVFhHfYsBimUxDMP+/S89u4YnEK5bQUkiu7///OUFZsC3M+vG9T8/YF/erjPT2e1+ZcH3KwvuNatr57ky9rBysOvOzZnVYX7vDAO+nVk3rv/5Afvydp2Zzm73Kwu+X1lwr1ldO8+VsYeVg113bs6sDvN7Zxjw7cy6cf3PD9iXt+vMdHa7X1nw/cqCe83q2nmujD2sHOy6c3NmdZjfO8OAb2fWjet/fsC+vF1nprPb/cqC71cW3GtW185zZexh5WDXnZszq8P83hkGfDuzblz/8wP25e06M53d7lcWfL+y4F6zunaeK2MPKwe77tycWR3m984w4NuZdeP6nx+wL2/Xmensdr+y4PuVBfea1bXzXBl7WDnYdefmzOowv3eGAd/OrBvX//yAfXm7zkxnt/uVBd+vLLjXrK6d58rYw8rBrjs3Z1aH+b0zDPh2Zt24/ucH7MvbdWY6u92vLPh+ZcG9ZnXtPFfGHlYOdt25ObM6zO+dYcC3M+vG9T8/YF/erjPT2e1+ZcH3KwvuNatr57ky9rBysOvOzZnVYX7vDAO+nVk3rv/5Afvydp2Zzm73Kwu+X1lwr1ldO8+VsYeVg113bs6sDvN7Zxjw7cy6cf3PD9iXt+vMdHa7X1nw/cqCe83q2nmujD2sHOy6c3NmdZjfO8OAb2fWjet/fsC+vF1nprPb/cqC71cW3GtW185zZexh5WDXnZszq8P8/X5//wC5QfHT67yLDAAAAABJRU5ErkJggg==");

        public static IEnumerable<Dropdown> ListOfReqStatus()
        {
            IEnumerable<Dropdown> dropdowns = new List<Dropdown>() {
                new Dropdown{text=RequisitionStatus.Pending,value=RequisitionStatus.Pending },
                new Dropdown{text=RequisitionStatus.Rechecked,value=RequisitionStatus.Rechecked },
                new Dropdown{text=RequisitionStatus.Approved,value=RequisitionStatus.Approved },
                new Dropdown{text=RequisitionStatus.Accepted,value=RequisitionStatus.Accepted },
                new Dropdown{text=RequisitionStatus.Rejected,value=RequisitionStatus.Rejected },
                new Dropdown{text=RequisitionStatus.Canceled,value=RequisitionStatus.Canceled },
                new Dropdown{text=RequisitionStatus.Waiting,value=RequisitionStatus.Waiting },
                new Dropdown{text=RequisitionStatus.Current,value=RequisitionStatus.Current },
                new Dropdown{text=RequisitionStatus.Void,value=RequisitionStatus.Void }
            };
            return dropdowns;
        }
        public static IEnumerable<Dropdown> ListOfReturnType()
        {
            IEnumerable<Dropdown> dropdowns = new List<Dropdown>()
            {
                new Dropdown{text=ReturnType.RepairFaultyReturn,value=ReturnType.RepairFaultyReturn },
                new Dropdown{text=ReturnType.RepairGoodsReturn,value=ReturnType.RepairGoodsReturn },
                new Dropdown{text=ReturnType.ProductionFaultyReturn,value=ReturnType.ProductionFaultyReturn},
                new Dropdown{text=ReturnType.ProductionGoodsReturn,value=ReturnType.ProductionGoodsReturn },
            };
            return dropdowns;
        }
        public static IEnumerable<Dropdown> ListOfFaultyCase()
        {
            IEnumerable<Dropdown> dropdowns = new List<Dropdown>
            {
                new Dropdown(){text=FaultyCase.ManMade,value=FaultyCase.ManMade},
                new Dropdown(){text=FaultyCase.ChinaMade,value=FaultyCase.ChinaMade}
            };
            return dropdowns;
        }
        public static IEnumerable<Dropdown> ListOfJournalType()
        {
            IEnumerable<Dropdown> dropdowns = new List<Dropdown>
            {
                new Dropdown(){text=JournalType.Dr,value=JournalType.Dr},
                new Dropdown(){text=JournalType.Cr,value=JournalType.Cr}
            };
            return dropdowns;
        }
        public static IEnumerable<Dropdown> ListOfAccountsType()
        {
            IEnumerable<Dropdown> dropdowns = new List<Dropdown>
            {
                new Dropdown(){text=AccountsType.BalanceSheed,value=AccountsType.BalanceSheed},
                new Dropdown(){text=AccountsType.PNL,value=AccountsType.PNL}
            };
            return dropdowns;
        }
        public static IEnumerable<Dropdown> ListOfTransferStatus()
        {
            IEnumerable<Dropdown> dropdowns = new List<Dropdown>
            {
                new Dropdown(){text=JobOrderTransferStatus.Received,value=JobOrderTransferStatus.Received},
                new Dropdown(){text=JobOrderTransferStatus.Pending,value=JobOrderTransferStatus.Pending}
            };
            return dropdowns;
        }
        public static IEnumerable<Dropdown> ListOfInvoiceTypeStatus()
        {
            IEnumerable<Dropdown> dropdowns = new List<Dropdown>
            {
                new Dropdown(){text=InvoiceTypeStatus.JobOrder,value=InvoiceTypeStatus.JobOrder},
                new Dropdown(){text=InvoiceTypeStatus.Sells,value=InvoiceTypeStatus.Sells}
            };
            return dropdowns;
        }
        public static IEnumerable<Dropdown> ListOfTransferCondition()
        {
            IEnumerable<Dropdown> dropdowns = new List<Dropdown>
            {
                new Dropdown(){text=JobOrderTransferCondition.Transfer,value=JobOrderTransferCondition.Transfer},
                new Dropdown(){text=JobOrderTransferCondition.Return,value=JobOrderTransferCondition.Return}
            };
            return dropdowns;
        }
        public static IEnumerable<Dropdown> ListOfReceiveCondition()
        {
            IEnumerable<Dropdown> dropdowns = new List<Dropdown>
            {
                new Dropdown(){text=ReceiveCondition.ReceiveJob,value=ReceiveCondition.ReceiveJob},
                new Dropdown(){text=ReceiveCondition.ReceiveReturnJob,value=ReceiveCondition.ReceiveReturnJob}
            };
            return dropdowns;
        }

        public static IEnumerable<Dropdown> ListOfCustomerType()
        {
            IEnumerable<Dropdown> dropdowns = new List<Dropdown>
            {
                new Dropdown(){text=CustomerType.WalkInCustomer,value=CustomerType.WalkInCustomer},
                new Dropdown(){text=CustomerType.Dealer,value=CustomerType.Dealer}
            };
            return dropdowns;
        }
        public static IEnumerable<Dropdown> ListOfStockType()
        {
            IEnumerable<Dropdown> dropdowns = new List<Dropdown>
            {
                new Dropdown(){text=StockType.FreshStock,value=StockType.FreshStock},
                new Dropdown(){text=StockType.FaultyStock,value=StockType.FaultyStock},
            };
            return dropdowns;
        }
        public static IEnumerable<Dropdown> ListOfStockStatus()
        {
            IEnumerable<Dropdown> dropdowns = new List<Dropdown>
            {
                new Dropdown(){text=StockStatus.StockIn,value=StockStatus.StockIn},
                new Dropdown(){text=StockStatus.StockOut,value=StockStatus.StockOut},
                new Dropdown(){text=StockStatus.StockReturn,value=StockStatus.StockReturn}
            };
            return dropdowns;
        }
        public static IEnumerable<Dropdown> ListOfModelColor()
        {
            IEnumerable<Dropdown> dropdowns = new List<Dropdown>
            {
                new Dropdown(){text=ModelColors.Red,value=ModelColors.Red},
                new Dropdown(){text=ModelColors.Blue,value=ModelColors.Blue},
                new Dropdown(){text=ModelColors.Black,value=ModelColors.Black},
                new Dropdown(){text=ModelColors.White,value=ModelColors.White},
            };
            return dropdowns;
        }
        public static IEnumerable<Dropdown> ListOfPhoneTypes()
        {
            IEnumerable<Dropdown> dropdowns = new List<Dropdown>
            {
                new Dropdown(){text=PhoneTypes.Smartphone,value=PhoneTypes.Smartphone},
                new Dropdown(){text=PhoneTypes.Featurephone,value=PhoneTypes.Featurephone},
                new Dropdown(){text=PhoneTypes.Accessories,value=PhoneTypes.Accessories},
                new Dropdown(){text=PhoneTypes.SpareParts,value=PhoneTypes.SpareParts}
            };
            return dropdowns;
        }
        public static IEnumerable<Dropdown> ListOfProductionType()
        {
            IEnumerable<Dropdown> dropdowns = new List<Dropdown>
            {
                new Dropdown(){text=ProductionTypes.CKD,value=ProductionTypes.CKD},
                new Dropdown(){text=ProductionTypes.SKD,value=ProductionTypes.SKD},
                new Dropdown(){text=ProductionTypes.HandSet,value=ProductionTypes.HandSet}
            };
            return dropdowns;
        }
        public static IEnumerable<Dropdown> ListOfFinishGoodsSendStatus()
        {
            IEnumerable<Dropdown> dropdowns = new List<Dropdown>
            {
                new Dropdown(){text=FinishGoodsSendStatus.Send,value=FinishGoodsSendStatus.Send},
                new Dropdown(){text=FinishGoodsSendStatus.Received,value=FinishGoodsSendStatus.Received}
            };
            return dropdowns;
        }
        public static IEnumerable<Dropdown> ListOfRequisitionType()
        {
            IEnumerable<Dropdown> dropdowns = new List<Dropdown>
            {
                new Dropdown(){text=RequisitionType.CKD,value=RequisitionType.CKD},
                new Dropdown(){text=RequisitionType.SKD,value=RequisitionType.SKD}
            };
            return dropdowns;
        }
        public static IEnumerable<Dropdown> ListOfJobOrderStatus()
        {
            IEnumerable<Dropdown> dropdowns = new List<Dropdown>
            {
                //new Dropdown(){text=JobOrderStatus.PendingJobOrder,value=JobOrderStatus.PendingJobOrder},
                new Dropdown(){text=JobOrderStatus.JobInitiated,value=JobOrderStatus.JobInitiated},
                //new Dropdown(){text=JobOrderStatus.CustomerDisapproved,value=JobOrderStatus.CustomerDisapproved},
                 new Dropdown(){text=JobOrderStatus.AssignToTS,value=JobOrderStatus.AssignToTS},
                 new Dropdown(){text=JobOrderStatus.RepairDone,value=JobOrderStatus.RepairDone},
                 new Dropdown(){text=JobOrderStatus.DeliveryDone,value=JobOrderStatus.DeliveryDone},
            };
            return dropdowns;
        }
        public static IEnumerable<Dropdown> ListOfJobOrderType()
        {
            IEnumerable<Dropdown> dropdowns = new List<Dropdown>
            {
                new Dropdown(){text=JobOrderTypes.Warrenty,value=JobOrderTypes.Warrenty},
                new Dropdown(){text=JobOrderTypes.Billing,value=JobOrderTypes.Billing},
                //new Dropdown(){text=JobOrderTypes.WarrentyBounce,value=JobOrderTypes.WarrentyBounce},
                //new Dropdown(){text=JobOrderTypes.BillingBounce,value=JobOrderTypes.BillingBounce}
            };
            return dropdowns;
        }
        public static IEnumerable<Dropdown> ListOfAppType()
        {
            IEnumerable<Dropdown> dropdowns = new List<Dropdown>
            {
                new Dropdown(){text=ApplicationType.ERP,value=ApplicationType.ERP},
                new Dropdown(){text=ApplicationType.Service,value=ApplicationType.Service}
            };
            return dropdowns;
        }


        public static IEnumerable<SetIMEIDataForHTML> StrickerOne(int stickerCount, int imeiNo)
        {
            List<SetIMEIDataForHTML> data = new List<SetIMEIDataForHTML>();

            for (int i = 1; i <= stickerCount; i++)
            {
                SetIMEIDataForHTML setOne = new SetIMEIDataForHTML();
                setOne.ModelName = "M4";
                setOne.IMEIImages = new List<IMEIImage>();

                for (int j = 1; j <= imeiNo; j++)
                {
                    IMEIImage setOneImg1 = new IMEIImage()
                    {
                        Text = "IMEI " + j.ToString() + ": 123456789012345",
                        Image = _imagByte
                    };
                    setOne.IMEIImages.Add(setOneImg1);
                }
                data.Add(setOne);
            }
            return data;
        }

        public static int TryParseInt(string value)
        {
            int i = 0;
            int.TryParse(value, out i);
            return i;
        }

        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
        public static string GetImage(string URL)
        {
            string base64Img = string.Empty;
            if (System.IO.File.Exists(URL))
            {
                FileStream fs = new FileStream(URL, FileMode.Open, FileAccess.Read);

                BinaryReader br = new BinaryReader(fs);

                Byte[] bytes = br.ReadBytes((Int32)fs.Length);

                br.Close();
                string extension = Path.GetExtension(URL);

                var base64 = Convert.ToBase64String(bytes);
                base64Img = String.Format("data:image/{0};base64,{1}", extension, base64);
            }
            return base64Img;
        }
        public static string SaveImage(Stream stream, string fileName, string driverPath, long OrgId)
        {
            WebImage imgFile = new WebImage(stream);
            string file = fileName.Substring(0, (fileName.LastIndexOf(".")));
            string extension = imgFile.ImageFormat;
            file = file + OrgId.ToString().PadLeft(4, '0') + DateTime.Now.ToString("yymmssff");//+ extension;
            imgFile.Save(Path.Combine(string.Format(@"{0}", driverPath), file));
            return driverPath + file + "." + extension;
        }
        public static string SaveImage(Stream stream, string fileName, string driverPath, long OrgId, int width, int height)
        {
            WebImage imgFile = new WebImage(stream);
            imgFile.Resize(width, height, false, true);
            string file = fileName.Substring(0, (fileName.LastIndexOf(".")));
            string extension = imgFile.ImageFormat;
            file = file + OrgId.ToString().PadLeft(4, '0') + DateTime.Now.ToString("yymmssff");//+ extension;
            imgFile.Save(Path.Combine(string.Format(@"{0}", driverPath), file));
            return driverPath + file + "." + extension;
        }
        public static void DeleteImage(string ImagePath)
        {
            if (!string.IsNullOrEmpty(ImagePath))
            {
                if (File.Exists(ImagePath))
                {
                    File.Delete(ImagePath);
                }
            }
        }
        public static byte[] GetImageBytes(string imagePath)
        {
            byte[] imgByte = null;
            if (System.IO.File.Exists(imagePath))
            {
                FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read);

                BinaryReader br = new BinaryReader(fs);

                Byte[] bytes = br.ReadBytes((Int32)fs.Length);

                br.Close();
                imgByte = bytes;
            }
            return imgByte;
        }
        public static string ParamChecker(string param)
        {
            if (param.ToLower().Contains(";") || param.ToLower().Contains("delete") || param.ToLower().Contains("database") || param.ToLower().Contains("alter") || param.ToLower().Contains("truncate") || param.ToLower().Contains("column") || param.ToLower().Contains("drop"))
            {
                return param = "";
            }
            return param;
        }
        public static string OrgLogoPath
        {
            get
            {
                return @"C:/Z Files/ClientPhotos/FIVESTARERP/Org/";
            }
        }
        public static string ReportLogoPath
        {
            get
            {
                return @"C:/Z Files/ClientPhotos/FIVESTARERP/Report/";//System.Web.HttpServerUtilityBase. //
            }
        }
        public static byte[] GenerateQRCode(string codeText)
        {
            QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
            QRCodeData data = qRCodeGenerator.CreateQrCode(codeText, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(data);
            MemoryStream stream = new MemoryStream();
            qrCode.GetGraphic(5).Save(stream,ImageFormat.Png);
            return stream.ToArray();
        }
    }
}
