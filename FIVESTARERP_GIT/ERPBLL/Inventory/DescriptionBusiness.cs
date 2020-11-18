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
        private readonly IModelColorBusiness _modelColorBusiness;

        public DescriptionBusiness(IInventoryUnitOfWork inventoryDb, IProductionStockInfoBusiness productionStockInfoBusiness, IItemTypeBusiness itemTypeBusiness, IUnitBusiness unitBusiness, IItemBusiness itemBusiness, IColorBusiness colorBusiness, IModelColorBusiness modelColorBusiness)
        {
            this._inventoryDb = inventoryDb;
            descriptionRepository = new DescriptionRepository(this._inventoryDb);
            this._productionStockInfoBusiness = productionStockInfoBusiness;
            this._itemTypeBusiness = itemTypeBusiness;
            itemRepository = new ItemRepository(this._inventoryDb);
            this._unitBusiness = unitBusiness;
            this._itemBusiness = itemBusiness;
            this._colorBusiness = colorBusiness;
            this._modelColorBusiness = modelColorBusiness;
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

        public List<ModelColor> GetModelColors(long modelId, long orgId)
        {
            List<ModelColor> colors = _inventoryDb.Db.Database.SqlQuery<ModelColor>(string.Format(@"Select C.ColorId,C.ColorName From tblModelColors mc 
Inner Join tblColors c on mc.ColorId = c.ColorId
Where mc.DescriptionId = {0} and mc.OrganizationId={1}", modelId, orgId)).ToList();
            return colors;
        }

        public bool SaveDescription(DescriptionDTO model, long userId, long orgId)
        {
            Description description = null;
            // For Inserting Item Color //
            if (model.DescriptionId == 0)
            {
                // Insert
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
                description.Flag = model.Flag;
                description.SalePrice = model.SalePrice;
                description.CostPrice = model.CostPrice;
                descriptionRepository.Insert(description);
                
            }
            else if (model.DescriptionId > 0)
            {
                // Update
                description = GetDescriptionOneByOrdId(model.DescriptionId, orgId);
                description.DescriptionName = model.DescriptionName;
                description.IsActive = model.IsActive;
                description.Remarks = model.Remarks;
                description.TAC = model.TAC;
                description.StartPoint = model.StartPoint;
                description.EndPoint = model.EndPoint;
                description.UpUserId = userId;
                description.UpdateDate = DateTime.Now;
                description.Flag = model.Flag;
                description.SalePrice = model.SalePrice;
                description.CostPrice = model.CostPrice;

                if (description.CategoryId == null || description.CategoryId == 0)
                {
                    description.CategoryId = model.CategoryId;
                }
                if (description.BrandId == null || description.BrandId == 0)
                {
                    description.BrandId = model.BrandId;
                }
                descriptionRepository.Update(description);
            }
            if (descriptionRepository.Save())
            {
                //colors = model.Color.Count() > 0 ? null : new long[model.Color.Count()];
                //for (int i = 0; i < model.Color.Count(); i++)
                //{
                //    colors[i] = model.Color[i];
                //}
                if (model.Color.Count > 0)
                {
                  return IssertModelItemColors(model.Color, description, userId, orgId);
                }
                else
                {
                    return true;
                }
            }
            return false;
        }

        private bool IssertModelItemColors(List<long> colors, Description description, long userId, long orgId)
        {
            if (colors.Count > 0)
            {
                Item item = new Item();
                List<Item> itemList = new List<Item>();
                var GetHandSet = _itemTypeBusiness.GetAllItemTypeByOrgId(orgId).Where(s => s.ItemName == "Handset").FirstOrDefault();
               
                List<long> insertedColor = null;
                if (_modelColorBusiness.SaveModelColors(description.DescriptionId, colors, userId, orgId, out insertedColor))
                {
                    if (GetHandSet != null && insertedColor.Count > 0 && description.Flag =="Internal")
                    {
                        var unit = _unitBusiness.GetAllUnitByOrgId(orgId).Where(s => s.UnitName == "Piece").FirstOrDefault();
                        if(unit != null) {
                            string strColors = string.Join(",", colors);
                            List<ModelColorDTO> modelColors = new List<ModelColorDTO>();
                            modelColors = _inventoryDb.Db.Database.SqlQuery<ModelColorDTO>(
                                string.Format(@"Select ColorId,ColorName From tblColors
                                Where ColorId IN ({0}) and OrganizationId={1}", strColors, orgId)).ToList();
                            if (modelColors.Count > 0)
                            {
                                foreach (var itemColor in modelColors)
                                {
                                    var itemInDb = _itemBusiness.GetItemsByQuery(null, null, null, null, (description.DescriptionName + " " + itemColor.ColorName), null, orgId).FirstOrDefault();
                                    if (itemInDb == null)
                                    {
                                        Item newItem = new Item
                                        {
                                            IsActive = description.IsActive,
                                            ItemName = description.DescriptionName + " " + itemColor.ColorName,
                                            ItemTypeId = GetHandSet.ItemId,
                                            ItemCode = GenerateItemCode(orgId, GetHandSet.ItemId),
                                            Remarks = description.Remarks,
                                            OrganizationId = orgId,
                                            UnitId = unit.UnitId,
                                            ColorId = Convert.ToInt64(itemColor.ColorId),
                                            DescriptionId = description.DescriptionId,
                                        };
                                        itemList.Add(newItem);
                                    }
                                }
                                itemRepository.InsertAll(itemList);
                                return itemRepository.Save();
                            }
                            else
                            {
                                return true;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return true;
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

                if (model.Color.Count > 0)
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
                    if (model.Color.Count > 0 && itemTypeId > 0)
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
                    List<long> unqColor = new List<long>();
                    if (model.Color.Count > 0)
                    {
                        string colorInDb = !string.IsNullOrEmpty(descriptionInDb.ColorId) ? descriptionInDb.ColorId : string.Empty;
                        string[] colorsDb = !string.IsNullOrEmpty(descriptionInDb.ColorId) ? colorInDb.Split(',') : null;
                        unqColor = (from c in model.Color
                                    where colorsDb == null || !colorsDb.Any(x => Convert.ToInt64(x) == c)
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

        public IEnumerable<Dropdown> GetModelsByBrand(long brandId, long orgId)
        {
            return descriptionRepository.GetAll(s => s.BrandId == brandId && s.OrganizationId == orgId).Select(s => new Dropdown() { value = s.DescriptionId.ToString(), text = s.DescriptionName }).ToList();
        }

        public IEnumerable<Dropdown> GetModelsByBrandAndCategory(long brandId, long categoryId, long orgId)
        {
            return descriptionRepository.GetAll(s => s.BrandId == brandId && s.CategoryId == categoryId && s.OrganizationId == orgId).Select(s => new Dropdown() { value = s.DescriptionId.ToString(), text = s.DescriptionName }).ToList();
        }
    }
}
