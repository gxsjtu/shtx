﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Service.ViewModel;

namespace Service
{
    public class ProductPriceService
    {
        private List<int> GetProductIdsByMobileAndMarketId(string mobile, int marketId)
        {
            using (var ctx = new ShtxSms2008Entities())
            {
                return ctx.Gps.Where(o => o.Tel.Contains(mobile) && o.MarketID == marketId).Select(o => o.ProductID.Value).ToList();
            }
        }

        public List<ProductPriceVM> GetPricesByMobileAndMarketId(string mobile, int marketId)
        {
            var listOrder = new List<ProductPriceVM>();
            using (var ctx = new ShtxSms2008Entities())
            {
                CustomerBase cb = ctx.CustomerBases.FirstOrDefault(o => o.Tel.Contains(mobile) && o.SendInterFace == 102);
                if (cb != null)
                {
                    mobile = cb.Tel;
                    //change://
                    //var products = ctx.SmsProducts.Where(o => o.MarketId == marketId).OrderBy(p => p.DisplayOrder).ToList();
                    var products = CacheService.GetSmsProductByMarketId(marketId);
                    var orderIds = ctx.Gps.Where(o => o.Tel == mobile && o.MarketID == marketId).Select(o => o.ProductID.Value).ToList();
                    foreach (var product in products)
                    {
                        var vm = new ProductPriceVM
                        {
                            ProductId = product.ProductId,
                            ProductName = product.ProductName,
                            ParentName = product.parentName,
                            Comment = product.comment,
                            Spec = product.spec
                        };
                        if (orderIds.Contains(product.ProductId))
                        {
                            vm.IsOrder = "YES";
                        }
                        else
                        {
                            vm.IsOrder = "NO";
                        }
                        //mongo:
                        //Price price = ctx.Prices.OrderByDescending(o => o.AddDate).FirstOrDefault(o => o.ProductID == product.ProductId);
                        Price price = MongoDBService.GetLastPricesByProductId(product.ProductId);
                        if (price != null)
                        {
                            vm.LPrice = price.LPrice;
                            vm.HPrice = string.IsNullOrEmpty(price.HPrice) ? price.LPrice : price.HPrice;
                            vm.Date = price.AddDate.Value.ToString("yyyy-MM-dd");
                            vm.Change = CommonService.ChangePrice(price.PriceChange);
                            vm.AddDate = price.AddDate.Value;
                            listOrder.Add(vm);
                        }
                    }
                }
                return listOrder;
            }
        }

        public List<ProductPriceVM> GetOrderPricesByMobileAndMarketId(string mobile, int marketId)
        {
            var listOrder = new List<ProductPriceVM>();
            using (var ctx = new ShtxSms2008Entities())
            {
                CustomerBase cb = ctx.CustomerBases.FirstOrDefault(o => o.Tel.Contains(mobile) && o.SendInterFace == 102);
                if (cb != null)
                {
                    mobile = cb.Tel;
                    //change://
                    //var products = ctx.SmsProducts.Where(o => o.MarketId == marketId).OrderBy(p => p.DisplayOrder).ToList();
                    var products = CacheService.GetSmsProductByMarketId(marketId);
                    var orderIds = ctx.Gps.Where(o => o.Tel == mobile && o.MarketID == marketId).Select(o => o.ProductID.Value).ToList();
                    foreach (var product in products)
                    {
                        var vm = new ProductPriceVM
                        {
                            ProductId = product.ProductId,
                            ProductName = product.ProductName,
                            ParentName = product.parentName,
                            Comment = product.comment,
                            Spec = product.spec
                        };
                        if (orderIds.Contains(product.ProductId))
                        {
                            vm.IsOrder = "YES";
                        }
                        else
                        {
                            continue;
                        }
                        //mongo:
                       // Price price = ctx.Prices.OrderByDescending(o => o.AddDate).FirstOrDefault(o => o.ProductID == product.ProductId);
                        Price price = MongoDBService.GetLastPricesByProductId(product.ProductId);
                        if (price != null)
                        {
                            vm.LPrice = price.LPrice;
                            vm.HPrice = string.IsNullOrEmpty(price.HPrice) ? price.LPrice : price.HPrice;
                            vm.Date = price.AddDate.Value.ToString("yyyy-MM-dd");
                            vm.Change = CommonService.ChangePrice(price.PriceChange);
                            vm.AddDate = price.AddDate.Value;
                            listOrder.Add(vm);
                        }
                    }
                }
                return listOrder;
            }
        }

        public List<ProductPriceVM> GetHistoryPrices(int productId, DateTime start, DateTime end)
        {
            var list = new List<ProductPriceVM>();

            //mongo:
            //去mongo里取该productId对应的相应日期里的所有价格信息,下边代码为改进的，注释的是原来的代码
            var prices = MongoDBService.GetHostoryPrices(productId, start, end);

            while (start <= end)
            {
                DateTime endTime = start.AddDays(1);

                var price = prices.Where(o => o.ProductID == productId && o.AddDate < endTime && o.AddDate >= start).OrderByDescending(o => o.AddDate).FirstOrDefault();
                if (price != null && (!string.IsNullOrEmpty(price.LPrice) || !string.IsNullOrEmpty(price.HPrice)))
                {
                    ProductPriceVM vm = new ProductPriceVM();
                    vm.Date = start.ToString("yyyy-MM-dd");
                    vm.HPrice = string.IsNullOrEmpty(price.HPrice) ? price.LPrice : price.HPrice;
                    vm.LPrice = string.IsNullOrEmpty(price.LPrice) ? price.HPrice : price.LPrice;
                    vm.Change = CommonService.ChangePrice(price.PriceChange);
                    list.Add(vm);
                }

                start = start.AddDays(1);
            }


            //using (var ctx = new ShtxSms2008Entities())
            //{
            //    while (start <= end)
            //    {
            //        DateTime endTime = start.AddDays(1);

            //        var price = ctx.Prices.Where(o => (o.ProductID ?? 0) == productId && o.AddDate < endTime && o.AddDate >= start).OrderByDescending(o => o.AddDate).FirstOrDefault();
            //        if (price != null && (!string.IsNullOrEmpty(price.LPrice) || !string.IsNullOrEmpty(price.HPrice)))
            //        {
            //            ProductPriceVM vm = new ProductPriceVM();
            //            vm.Date = start.ToString("yyyy-MM-dd");
            //            vm.HPrice = string.IsNullOrEmpty(price.HPrice) ? price.LPrice : price.HPrice;
            //            vm.LPrice = string.IsNullOrEmpty(price.LPrice) ? price.HPrice : price.LPrice;
            //            vm.Change = CommonService.ChangePrice(price.PriceChange);
            //            list.Add(vm);
            //        }
            //        start = start.AddDays(1);
            //    }
            //}
            return list.OrderByDescending(o => o.Date).ToList();
        }
    }
}
