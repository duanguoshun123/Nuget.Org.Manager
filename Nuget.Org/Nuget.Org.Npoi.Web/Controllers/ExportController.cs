﻿using Nuget.Org.Npoi.ExcelExportHelper;
using Nuget.Org.Npoi.ExcelExportHelper.Npoi;
using Nuget.Org.Npoi.Web.Infrastructure;
using Nuget.Org.Npoi.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
namespace Nuget.Org.Npoi.Web.Controllers
{
    public class ExportController : ApiController
    {
        /// <summary>
        ///  导出测试
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult ExportTest()
        {
            List<TestViewModel> report = new List<TestViewModel>
            {
               new TestViewModel{
                   CommodityTypeName ="铜",
                   Details = new List<DetailViewModel>{
                       new DetailViewModel{
                           AccountingDate = new DateTime(2020,11,21),
                           PnL = 86.56m,
                           Position = 50.50m
                       },
                       new DetailViewModel{ AccountingDate = new DateTime(2020,11,22),
                           PnL = 18986.56m,
                           Position = 50.50m },
                       new DetailViewModel{ AccountingDate = new DateTime(2020,11,23),
                           PnL = 86.56m,
                           Position = 50.50m }
                   },

               },
                new TestViewModel{
                   CommodityTypeName ="银",
                   Details = new List<DetailViewModel>{
                       new DetailViewModel{
                           AccountingDate = new DateTime(2020,11,20),
                           PnL = 86.56m,
                           Position = 556540.50m
                       },
                       new DetailViewModel{ AccountingDate = new DateTime(2020,11,22),
                           PnL = 86.56m,
                           Position = 50.50m },
                       new DetailViewModel{ AccountingDate = new DateTime(2020,11,23),
                           PnL = 86.56m,
                           Position = 50.50m }
                   },

               },
                 new TestViewModel{
                   CommodityTypeName ="铝",
                   Details = new List<DetailViewModel>{
                       new DetailViewModel{
                           AccountingDate = new DateTime(2020,11,19),
                           PnL = 86.56m,
                           Position = 50.54001m
                       },
                       new DetailViewModel{ AccountingDate = new DateTime(2020,11,22),
                           PnL = 86.56m,
                           Position = 50.50m },
                       new DetailViewModel{ AccountingDate = new DateTime(2020,11,23),
                           PnL = 86.56m,
                           Position = 50.5001m }
                   },

               },
                  new TestViewModel{
                   CommodityTypeName ="铅",
                   Details = new List<DetailViewModel>{
                       new DetailViewModel{
                           AccountingDate = new DateTime(2020,11,17),
                           PnL = 86.56m,
                           Position = 50.55550m
                       },
                       new DetailViewModel{ AccountingDate = new DateTime(2020,11,18),
                           PnL = 86.56m,
                           Position = 50.50m },
                       new DetailViewModel{ AccountingDate = new DateTime(2020,11,23),
                           PnL = -86.56800m,
                           Position = 50.50m }
                   },

               },
            };
            List<ExcelExportHelper.Model.ExportConfigModel> configModels = new List<ExcelExportHelper.Model.ExportConfigModel>();
            configModels.Add(new ExcelExportHelper.Model.ExportConfigModel
            {
                CellStyle = CellStyleEnums.StyleBoldCenter,
                IsMerge = true,
                MergeRowIndexStart = 0,
                MergeRowIndexEnd = 1,
                MergeColIndexStart = 0,
                CellValue = @"具体合约",
                IsNeedSetBorder = true
            });
            var commodityTypeCols = report.Select(x => x.CommodityTypeName).Distinct().OrderByDescending(x => x).ToList();

            var accountingDataCols = report.SelectMany(x => x.Details.Select(r => r.AccountingDate)).Distinct().OrderBy(x => x).ToList();

            int colIndex = 1;

            Dictionary<string, int> commodityColDic = new Dictionary<string, int>();
            commodityTypeCols.ForEach(x =>
            {
                configModels.Add(new ExcelExportHelper.Model.ExportConfigModel
                {
                    CellStyle = CellStyleEnums.StyleBoldCenter,
                    IsMerge = true,
                    MergeRowIndexStart = 0,
                    MergeColIndexStart = colIndex,
                    MergeColIndexEnd = colIndex + 1,
                    CellValue = x,
                    IsNeedSetBorder = true
                });
                configModels.Add(new ExcelExportHelper.Model.ExportConfigModel
                {
                    CellStyle = CellStyleEnums.StyleBoldCenter,
                    IsMerge = false,
                    MergeRowIndexStart = 1,
                    MergeColIndexStart = colIndex,
                    CellValue = "持仓手数",
                    IsNeedSetBorder = true
                });
                configModels.Add(new ExcelExportHelper.Model.ExportConfigModel
                {
                    CellStyle = CellStyleEnums.StyleBoldCenter,
                    IsMerge = false,
                    MergeRowIndexStart = 1,
                    MergeColIndexStart = colIndex + 1,
                    CellValue = "未结盈亏",
                    IsNeedSetBorder = true
                });
                commodityColDic.Add(x, colIndex);
                colIndex += 2;
            });

            // 盈亏合计
            configModels.Add(new ExcelExportHelper.Model.ExportConfigModel
            {
                CellStyle = CellStyleEnums.StyleBoldCenter,
                IsMerge = false,
                MergeRowIndexStart = 0,
                MergeColIndexStart = colIndex,
                CellValue = @"合计",
                IsNeedSetBorder = true
            });


            int rowIndex = 2;
            Dictionary<DateTime, int> dateRowDic = new Dictionary<DateTime, int>();
            accountingDataCols.ForEach(x =>
            {
                configModels.Add(new ExcelExportHelper.Model.ExportConfigModel
                {
                    CellStyle = CellStyleEnums.StyleNoBoldLeft,
                    IsMerge = false,
                    MergeRowIndexStart = rowIndex,
                    MergeColIndexStart = 0,
                    CellValue = x.ToShortDateString(),
                    IsNeedSetBorder = true
                });
                dateRowDic.Add(x, rowIndex);
                rowIndex++;
            });

            // 重量合计
            configModels.Add(new ExcelExportHelper.Model.ExportConfigModel
            {
                CellStyle = CellStyleEnums.StyleBoldCenter,
                IsMerge = false,
                MergeRowIndexStart = rowIndex,
                MergeColIndexStart = 0,
                CellValue = @"合计",
                IsNeedSetBorder = true
            });

            report.ForEach(x =>
            {
                x.Details.ForEach(p =>
                {
                    configModels.AddRange(new List<ExcelExportHelper.Model.ExportConfigModel> {

                        new ExcelExportHelper.Model.ExportConfigModel
                        {
                             CellStyle = CellStyleEnums.StyleForDecimal0,
                            IsMerge = false,
                            MergeRowIndexStart =  dateRowDic[p.AccountingDate],
                            MergeColIndexStart = commodityColDic[x.CommodityTypeName],
                            CellValue = p.Position,
                            IsNeedNumberFormat = true,
                            FontBackgroundColor = p.Position<0? CellBackgroundColorEnums.LightRed:default(CellBackgroundColorEnums),
                    IsNeedSetBorder = true
                        },
                        new ExcelExportHelper.Model.ExportConfigModel{
                            CellStyle = CellStyleEnums.StyleForDecimal4,
                            IsMerge = false,
                            MergeRowIndexStart = dateRowDic[p.AccountingDate],
                            MergeColIndexStart = commodityColDic[x.CommodityTypeName]+1,
                            CellValue = p.PnL,
                            IsNeedNumberFormat = true,
                            FontBackgroundColor = p.Position<0? CellBackgroundColorEnums.LightRed:default(CellBackgroundColorEnums),
                    IsNeedSetBorder = true
                        }
                    });
                });
                //持仓手数合计
                configModels.Add(new ExcelExportHelper.Model.ExportConfigModel
                {
                    CellStyle = CellStyleEnums.StyleForDecimal0,
                    IsMerge = false,
                    MergeRowIndexStart = rowIndex,
                    MergeColIndexStart = commodityColDic[x.CommodityTypeName],
                    CellValue = x.Details.Sum(p => p.Position ?? 0m),
                    IsNeedNumberFormat = true,
                    FontBackgroundColor = x.Details.Sum(p => p.Position ?? 0m) < 0 ? CellBackgroundColorEnums.LightRed : default(CellBackgroundColorEnums),
                    IsNeedSetBorder = true
                });
            });

            report.SelectMany(x => x.Details)
                .GroupBy(x => x.AccountingDate)
                .ToList()
                .ForEach(x =>
                {
                    // 盈亏合计
                    configModels.Add(new ExcelExportHelper.Model.ExportConfigModel
                    {
                        CellStyle = CellStyleEnums.StyleForDecimal4,
                        IsMerge = false,
                        MergeRowIndexStart = dateRowDic[x.Key],
                        MergeColIndexStart = colIndex,
                        CellValue = x.Sum(p => p.PnL ?? 0m),
                        IsNeedNumberFormat = true,
                        FontBackgroundColor = x.Sum(p => p.PnL ?? 0m) < 0 ? CellBackgroundColorEnums.LightRed : default(CellBackgroundColorEnums?),
                        IsNeedSetBorder = true
                    });
                });


            var stream = NpoiUtils.ExportWithCellFormat(new ExcelExportHelper.Model.ExportConfig
            {
                 CellModelConfigs = configModels,
            }, true, "测试");



            var filename = string.Format("test-{0:yyyyMMddhhmmss}.xlsx", DateTime.Now);
            return new FileActionResult(stream, @"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
        }
    }
}
