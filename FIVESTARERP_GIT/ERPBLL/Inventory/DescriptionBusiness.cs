using ERPBLL.Inventory.Interface;
using ERPBLL.Production.Interface;
using ERPBO.Common;
using ERPBO.Inventory.DomainModels;
using ERPBO.Inventory.DTOModel;
using ERPBO.Production.DomainModels;
using ERPDAL.InventoryDAL;
using ERPDAL.ProductionDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPBLL.Inventory
{
    public class DescriptionBusiness : IDescriptionBusiness
    {
        private readonly DescriptionRepository descriptionRepository; // table
        private readonly IInventoryUnitOfWork _inventoryDb;
        private readonly IProductionStockInfoBusiness _productionStockInfoBusiness;
        private readonly IItemTypeBusiness _itemTypeBusiness;
        private readonly ItemRepository itemRepository;
        private readonly IUnitBusiness _unitBusiness;
        private readonly IItemBusiness _itemBusiness;
        private readonly IColorBusiness _colorBusiness;

        public DescriptionBusiness(IInventoryUnitOfWork inventoryDb, IProductionStockInfoBusiness productionStockInfoBusiness, IItemTypeBusiness itemTypeBusiness, IUnitBusiness unitBusiness, IItemBusiness itemBusiness, IColorBusiness colorBusiness)
        {
            this._inventoryDb = inventoryDb;
            descriptionRepository = new DescriptionRepository(this._inventoryDb);
            this._productionStockInfoBusiness = productionStockInfoBusiness;
            this._itemTypeBusiness = itemTypeBusiness;
            itemRepository = new ItemRepository(this._inventoryDb);
            this._unitBusiness = unitBusiness;
            this._itemBusiness = itemBusiness;
            this._colorBusiness = colorBusiness;
        }

        public IEnumerable<Dropdown> GetAllDescriptionsInProductionStock(long orgId)
        {
            var modelInPDN = _productionStockInfoBusiness.GetAllProductionStockInfoByOrgId(orgId).Select(s => s.DescriptionId).Distinct().ToList();
            return GetDescriptionByOrgId(orgId).Where(d => modelInPDN.Contains(d.DescriptionId)).OrderBy(d => d.DescriptionName).Select(s => new Dropdown { text = s.DescriptionName, value = s.DescriptionId.ToString() }).ToList();
        }

        public IEnumerable<Description> GetDescriptionByOrgId(long orgId)
        {
            return descriptionRepository.GetAll(des => des.OrganizationId == orgId).ToList();
        }

        public Description GetDescriptionOneByOrdId(long id, long orgId)
        {
            return descriptionRepository.GetOneByOrg(des => des.DescriptionId == id && des.OrganizationId == orgId);
        }

        public List<ModelColors> GetModelColors(long modelId, long orgId)
        {
            List<ModelColors> colors = new List<ModelColors>();
            var colorsInDb = this.GetDescriptionOneByOrdId(modelId, orgId);
            if (colorsInDb != null && colorsInDb.ColorId != null && colorsInDb.ColorId.Trim() != "")
            {
                string[] colorIds = null;
                if (colorsInDb.ColorId.Contains(","))
                {
                    colorIds = colorsInDb.ColorId.Split(',');
                }
                else
                {
                    colorIds = new string[1];
                    colorIds[0] = colorsInDb.ColorId;
                }

                if (colorIds.Length > 0)
                {
                    var colorName = string.Join(",", colorIds);
                    colors = _inventoryDb.Db.Database.SqlQuery<ModelColors>(string.Format(@"Select ColorName From tblColors Where ColorId In ({0}) and OrganizationId = {1}", colorName, orgId)).ToList();
                }

            }
            return colors;
        }

        public bool UpdateDescriptionTAC(DescriptionDTO model, long userId, long orgId)
        {
            Description description = new Description();
            List<Description> descriptionList = new List<Description>();
            Item item = new Item();
            List<Item> itemList = new List<Item>();

            long itemTypeId = 0;
            var GetHandSet = _itemTypeBusiness.GetAllItemTypeByOrgId(orgId).Where(s => s.ItemName == "Handset").FirstOrDefault();
            itemTypeId = GetHandSet.ItemId;

            if (model.DescriptionId == 0)
            {
                string allColor = string.Empty;
                description = new Description();
                description.DescriptionName = model.DescriptionName;
                description.TAC = model.TAC;
                description.StartPoint = model.StartPoint;
                description.EndPoint = model.EndPoint;
                description.Remarks = model.Remarks;
                description.IsActive = model.IsActive;
                description.EUserId = userId;
                description.EntryDate = DateTime.Now;
                description.OrganizationId = orgId;
                description.CategoryId = model.CategoryId;
                description.BrandId = model.BrandId;

                if(model.Color.Length > 0)
                {
                    foreach (var items in model.Color)
                    {
                        allColor += items + ",";
                    }
                    allColor = allColor.Substring(0, allColor.Length - 1);
                    description.ColorId = allColor;
                }
                descriptionRepository.Insert(description);
                //itemRepository.InsertAll(itemList);
                if (descriptionRepository.Save())
                {
                    if(model.Color.Length > 0 && itemTypeId > 0)
                    {
                        string[] colorSplit = description.ColorId.Split(',');
                        foreach (var i in colorSplit)
                        {
                            long iConvertedValue = Convert.ToInt64(i);
                            item = new Item()
                            {
                                EntryDate = DateTime.Now,
                                EUserId = userId,
                                IsActive = description.IsActive,
                                ItemName = description.DescriptionName + " " + _colorBusiness.GetColorOneByOrgId(iConvertedValue, orgId).ColorName,
                                //item.ItemName = description.DescriptionName + " " + items;
                                ItemTypeId = itemTypeId,
                                ItemCode = GenerateItemCode(orgId, itemTypeId),
                                Remarks = description.Remarks,
                                OrganizationId = orgId,
                                UnitId = 4,
                                ColorId = iConvertedValue,
                                DescriptionId = description.DescriptionId,
                            };
                            //itemList.Add(item);
                            itemRepository.Insert(item);
                            itemRepository.Save();
                        }
                    }
                    else
                    {
                        return true;
                    }                   
                }
            }
            else
            {
                var descriptionInDb = GetDescriptionOneByOrdId(model.DescriptionId, orgId);
                if (descriptionInDb != null)
                {
                    string allColor = string.Empty;
                    descriptionInDb.DescriptionName = model.DescriptionName;
                    descriptionInDb.IsActive = model.IsActive;
                    descriptionInDb.Remarks = model.Remarks;
                    descriptionInDb.TAC = model.TAC;
                    descriptionInDb.StartPoint = model.StartPoint;
                    descriptionInDb.EndPoint = model.EndPoint;
                    descriptionInDb.UpUserId = userId;
                    descriptionInDb.UpdateDate = DateTime.Now;

                    if (descriptionInDb.CategoryId == null || descriptionInDb.CategoryId == 0)
                    {
                        descriptionInDb.CategoryId = model.CategoryId;
                    }
                    if (descriptionInDb.BrandId == null || descriptionInDb.BrandId == 0)
                    {
                        descriptionInDb.BrandId = model.BrandId;
                    }
                    List<int> unqColor = new List<int>();
                    if (model.Color.Length > 0)
                    {
                        string colorInDb = !string.IsNullOrEmpty(descriptionInDb.ColorId) ? descriptionInDb.ColorId : string.Empty;
                        string[] colorsDb = !string.IsNullOrEmpty(descriptionInDb.ColorId) ? colorInDb.Split(',') : null;
                        unqColor = (from c in model.Color
                                        where colorsDb == null || !colorsDb.Any(x => Convert.ToInt32(x) == c)
                                        select c).ToList();
                        allColor = !string.IsNullOrEmpty(descriptionInDb.ColorId) ? descriptionInDb.ColorId + "," : "";
                        foreach (var items in unqColor)
                        {
                            allColor += items + ",";
                        }

                        allColor = allColor.Substring(0, allColor.Length - 1);
                        descriptionInDb.ColorId = unqColor.Count > 0 ? allColor : colorInDb;
                    }
                    

                    descriptionRepository.Update(descriptionInDb);
                    if (descriptionRepository.Save())
                    {
                        if (itemTypeId > 0 && unqColor.Count > 0)
                        {
                            //string[] colorSplit = allColor.Split(',');
                            foreach (var i in unqColor)
                            {
                                long iConvertedValue = Convert.ToInt64(i);
                                item = new Item()
                                {
                                    EntryDate = DateTime.Now,
                                    EUserId = userId,
                                    IsActive = descriptionInDb.IsActive,
                                    ItemName = descriptionInDb.DescriptionName + " " + _colorBusiness.GetColorOneByOrgId(iConvertedValue, orgId).ColorName,
                                    //item.ItemName = model.DescriptionName + " " + items;
                                    ItemTypeId = itemTypeId,
                                    ItemCode = GenerateItemCode(orgId, itemTypeId),
                                    Remarks = descriptionInDb.Remarks,
                                    OrganizationId = orgId,
                                    UnitId = 2,
                                    ColorId = iConvertedValue,
                                    DescriptionId = descriptionInDb.DescriptionId,
                                };
                                //itemList.Add(item);
                                itemRepository.Insert(item);
                                itemRepository.Save();
                            }
                            return true;
                        }
                        else
                        {
                            return true;
                        }

                    }
                    //itemRepository.UpdateAll(itemList);
                }
            }
            return false;
        }

        private string GenerateItemCode(long OrgId, long itemTypeId)
        {
            string code = string.Empty;
            string newCode = string.Empty;
            string shortName = _itemTypeBusiness.GetItemType(itemTypeId, OrgId).ShortName;

            var lastItem = itemRepository.GetAll(i => i.ItemTypeId == itemTypeId && i.OrganizationId == OrgId).OrderByDescending(i => i.ItemId).FirstOrDefault();
            if (lastItem == null)
            {
                newCode = shortName + "00001";
            }
            else
            {
                code = lastItem.ItemCode.Substring(3);
                code = (Convert.ToInt32(code) + 1).ToString();
                newCode = shortName + code.PadLeft(5, '0');
            }
            return newCode;
        }
    }
}
