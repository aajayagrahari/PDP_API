using PowerDesignPro.BusinessProcessors.Dtos;
using PowerDesignPro.BusinessProcessors.Dtos.Admin;
using PowerDesignPro.BusinessProcessors.Interface;
using PowerDesignPro.Common;
using PowerDesignPro.Common.CustomException;
using PowerDesignPro.Common.MessageConfig;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

namespace PowerDesignProAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("Admin")]
    public class AdminController : BaseController
    {
        private readonly IAdmin _adminProcessor;
        private IPickList _pickListProcessor;
        private ITraceMessage _traceMessageProcessor;

        public AdminController(
            IAdmin adminProcessor,
            IPickList pickListProcessor,
            ITraceMessage traceMessageProcessor)
        {
            _adminProcessor = adminProcessor;
            _pickListProcessor = pickListProcessor;
            _traceMessageProcessor = traceMessageProcessor;
        }


        [HttpGet]
        [Route("GetGeneratorDetails")]
        public HttpResponseMessage GetGeneratorDetails()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_adminProcessor.GetGeneratorDetails());
            });
        }

        [HttpGet]
        [Route("GetGenerator")]
        public HttpResponseMessage GetGenerator(int GeneratorID)
        {
            return CreateHttpResponse(() =>
            {
                var requestDto = new SearchAdminRequestDto
                {
                    ID = GeneratorID
                };

                var generatorDto = _adminProcessor.GetGenerator(requestDto);
                PickListDetailForGenerator(generatorDto);
                return Request.CreateResponse(generatorDto);
            });
        }

        [HttpGet]
        [Route("GetGeneratorByProductFamily")]
        public HttpResponseMessage GetGeneratorByProductFamily(int productFamilyId)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_adminProcessor.GetGenerator(productFamilyId));
            });
        }


        [HttpPost]
        [Route("SaveGeneratorDetail")]
        public HttpResponseMessage SaveGeneratorDetail(GeneratorDto generatorResponseDto)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_adminProcessor.SaveGeneratorDetail(generatorResponseDto, generatorResponseDto.UserName));
            });
        }

        [HttpDelete]
        [Route("DeleteGenerator")]
        public HttpResponseMessage DeleteGenerator(int generatorID, string userName)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_adminProcessor.DeleteGenerator(generatorID, userName));
            });
        }

        [HttpGet]
        [Route("GetAlternatorDetails")]
        public HttpResponseMessage GetAlternatorDetails()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_adminProcessor.GetAlternatorDetails());
            });
        }

        [HttpGet]
        [Route("GetAlternator")]
        public HttpResponseMessage GetAlternator(int AlternatorID)
        {
            return CreateHttpResponse(() =>
            {
                if (AlternatorID > 0)
                {
                    var requestDto = new SearchAdminRequestDto
                    {
                        ID = AlternatorID
                    };

                    var alternatorDto = _adminProcessor.GetAlternator(requestDto);
                    PickListDetailForAlternator(alternatorDto);
                    return Request.CreateResponse(alternatorDto);
                }
                else
                {
                    var alternatorDto = new AlternatorDto();
                    PickListDetailForAlternator(alternatorDto);
                    return Request.CreateResponse(alternatorDto);
                }
            });
        }

        [HttpPost]
        [Route("SaveAlternatorDetail")]
        public HttpResponseMessage SaveAlternatorDetail(AlternatorDto alternatorResponseDto)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_adminProcessor.SaveAlternatorDetail(alternatorResponseDto));
            });
        }

        [HttpDelete]
        [Route("DeleteAlternator")]
        public HttpResponseMessage DeleteAlternator(int alternatorID, string userName)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_adminProcessor.DeleteAlternator(alternatorID, userName));
            });
        }

        [HttpGet]
        [Route("GetAlternatorFamilies")]
        public HttpResponseMessage GetAlternatorFamilies()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_adminProcessor.GetAlternatorFamilies());
            });
        }

        [HttpGet]
        [Route("GetProductFamilies")]
        public HttpResponseMessage GetProductFamilies()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_adminProcessor.GetProductFamilies());
            });
        }


        [HttpGet]
        [Route("GetProductFamily")]
        public HttpResponseMessage GetProductFamily(int ProductFamilyID)
        {
            return CreateHttpResponse(() =>
            {
                var requestDto = new SearchAdminRequestDto
                {
                    ID = ProductFamilyID
                };

                var productFamilyDto = _adminProcessor.GetProductFamily(requestDto);
                return Request.CreateResponse(productFamilyDto);
            });
        }

        [HttpPost]
        [Route("SaveProductFamily")]
        public HttpResponseMessage SaveProductFamily(ProductFamilyDto productFamilyResponseDto)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_adminProcessor.SaveProductFamily(productFamilyResponseDto));
            });
        }
        
        [HttpDelete]
        [Route("DeleteProductFamily")]
        public HttpResponseMessage DeleteProductFamily(int productFamilyID, string userName)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_adminProcessor.DeleteProductFamily(productFamilyID, userName));
            });
        }

        [HttpPost]
        [Route("SaveAlternatorFamily")]
        public HttpResponseMessage SaveAlternatorFamily(AlternatorFamilyDto alternatorFamilyResponseDto)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_adminProcessor.SaveAlternatorFamily(alternatorFamilyResponseDto));
            });
        }

        [HttpDelete]
        [Route("DeleteAlternatorFamily")]
        public HttpResponseMessage DeleteAlternatorFamily(int alternatorFamilyID,string userName)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_adminProcessor.DeleteAlternatorFamily(alternatorFamilyID, userName));
            });
        }

        [HttpGet]
        [Route("GetMaintainmarketVertical")]
        public HttpResponseMessage GetMaintainmarketVertical()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_adminProcessor.GetMaintainMarketVertical());
            });
        }

        [HttpPost]
        [Route("SaveMaintainMarketVertical")]
        public HttpResponseMessage SaveMaintainMarketVertical(SolutionApplicationDto maintainmarketVerticalResponseDto)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_adminProcessor.SaveMaintainMarketVertical(maintainmarketVerticalResponseDto));
            });
        }

        [HttpDelete]
        [Route("DeleteMaintainMarketVertical")]
        public HttpResponseMessage DeleteMaintainMarketVertical(int maintainMarketVerticalID, string userName)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_adminProcessor.DeleteMaintainMarketVerticals(maintainMarketVerticalID, userName));
            });
        }

        /// <summary>
        /// Imports the genset data.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("ImportGensetData")]
        public HttpResponseMessage ImportGensetData()
        {
            return CreateHttpResponse(() =>
            {
                if (HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    string fileSavePath, fileName = "";
                    try
                    {
                        var httpPostedFile = HttpContext.Current.Request.Files["UploadedFile"];

                        var userName = HttpContext.Current.Request.Form["UserName"].ToString();

                        if (httpPostedFile != null)
                        {
                            // Validate the uploaded file(optional)

                            fileName = new FileInfo(httpPostedFile.FileName.Trim(' ')).Name;
                            fileSavePath = Path.Combine(HostingEnvironment.MapPath("~/App_Data"), fileName);

                            httpPostedFile.SaveAs(fileSavePath);

                            var connString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileSavePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                            var command = "SELECT * FROM [Generator$]";
                            var gensetData = GetImportDataRequestDto<ImportGensetDataRequestDto>(fileSavePath, connString, command);

                            // call processor method to process and save ImportGensetRequestDto...
                            return Request.CreateResponse(_adminProcessor.ImportGensetData(gensetData, userName));
                        }
                    }
                    catch (PowerDesignProException ex)
                    {
                        var errorMessage = string.Empty;
                        if (!string.IsNullOrEmpty(ex.ErrorMessage))
                            errorMessage = ex.ErrorMessage;

                        else
                        {
                            errorMessage = MessageCollection.Instance.GetMessage(ex.Code, ex.Category);
                        }

                        var messageLength = errorMessage.Length > 255 ? 255 : errorMessage.Length;
                        _traceMessageProcessor.WriteTrace(TraceMessaging.EventSource, TraceLevel.Error, "ImportGensetData", HostingEnvironment.MapPath("~/App_Data") + "---" + ex.TargetSite.Name + "---" + fileName
                            , errorMessage.Substring(0, messageLength), ex.StackTrace);

                        throw;
                    }
                    catch (Exception ex)
                    {
                        var messageLength = ex.Message.Length > 255 ? 255 : ex.Message.Length;
                        _traceMessageProcessor.WriteTrace(TraceMessaging.EventSource, TraceLevel.Error, "ImportGensetData", HostingEnvironment.MapPath("~/App_Data") + "---" + ex.TargetSite.Name + "---" + fileName
                            , ex.Message.Substring(0, messageLength), ex.StackTrace);
                        throw;
                    }

                }
                return null;
            });
        }

        /// <summary>
        /// Imports the alternator data.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("ImportAlternatorData")]
        public HttpResponseMessage ImportAlternatorData()
        {
            return CreateHttpResponse(() =>
            {
                if (HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    string fileSavePath, fileName = "";
                    try
                    {
                        var httpPostedFile = HttpContext.Current.Request.Files["UploadedFile"];
                        var userName = HttpContext.Current.Request.Form["UserName"].ToString();

                        if (httpPostedFile != null)
                        {
                            // Validate the uploaded file(optional)

                            fileName = new FileInfo(httpPostedFile.FileName.Trim(' ')).Name;
                            fileSavePath = Path.Combine(HostingEnvironment.MapPath("~/App_Data"), fileName);

                            httpPostedFile.SaveAs(fileSavePath);

                            var connString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileSavePath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                            var command = "SELECT * FROM [Alternator$]";
                            var alternatorData = GetImportDataRequestDto<ImportAlternatorDataRequestDto>(fileSavePath, connString, command);

                            // call processor method to process and save ImportAlternatorRequestDto...
                            return Request.CreateResponse(_adminProcessor.ImportAlternatorData(alternatorData, userName));
                        }
                    }
                    catch (PowerDesignProException ex)
                    {
                        var errorMessage = string.Empty;
                        if (!string.IsNullOrEmpty(ex.ErrorMessage))
                            errorMessage = ex.ErrorMessage;

                        else
                        {
                            errorMessage = MessageCollection.Instance.GetMessage(ex.Code, ex.Category);
                        }

                        var messageLength = errorMessage.Length > 255 ? 255 : errorMessage.Length;
                        _traceMessageProcessor.WriteTrace(TraceMessaging.EventSource, TraceLevel.Error, "ImportAlternatorData", HostingEnvironment.MapPath("~/App_Data") + "---" + ex.TargetSite.Name + "---" + fileName
                            , errorMessage.Substring(0, messageLength), ex.StackTrace);

                        throw;
                    }
                    catch (Exception ex)
                    {
                        var messageLength = ex.Message.Length > 255 ? 255 : ex.Message.Length;
                        _traceMessageProcessor.WriteTrace(TraceMessaging.EventSource, TraceLevel.Error, "ImportAlternatorData", HostingEnvironment.MapPath("~/App_Data") + "---" + ex.TargetSite.Name + "---" + fileName
                            , ex.Message.Substring(0, messageLength), ex.StackTrace);
                        throw;
                    }
                }

                return null;
            });
        }

        private IEnumerable<T> GetImportDataRequestDto<T>(string strFilePath, string connString, string command)
        {
            var oledbConn = new OleDbConnection(connString);
            var dt = new DataTable();
            try
            {
                oledbConn.Open();
                using (var cmd = new OleDbCommand(command, oledbConn))
                {
                    var oleda = new OleDbDataAdapter
                    {
                        SelectCommand = cmd
                    };

                    var ds = new DataSet();
                    oleda.Fill(ds);

                    dt = ds.Tables[0];
                    var filteredRows = dt.Rows.Cast<DataRow>()
                                                .Where(row => !row.ItemArray.All(field => field is DBNull))
                                                .CopyToDataTable();
                    var gensetData = ConvertDataTable<T>(filteredRows);
                    return gensetData;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                oledbConn.Close();
            }
        }

        private static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            int rowNum = 1;
            var errorMessage = string.Empty;

            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row, rowNum, out string errorStr);
                errorMessage += errorStr;
                data.Add(item);
                rowNum++;
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                throw new PowerDesignProException("", "", errorMessage);
            }

            return data;
        }

        private static T GetItem<T>(DataRow dr, int rowNum, out string errorStr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();
            errorStr = string.Empty;
            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName && dr[column.ColumnName] != null && dr[column.ColumnName] != DBNull.Value)
                    {
                        try
                        {
                            dynamic columnData;
                            if (pro.PropertyType == typeof(Decimal?))
                            {
                                columnData = Convert.ToDecimal(dr[column.ColumnName]);
                            }
                            else if (pro.PropertyType == typeof(Decimal))
                            {
                                columnData = Convert.ToDecimal(dr[column.ColumnName]);
                            }
                            else if (pro.PropertyType == typeof(Int32?))
                            {
                                columnData = Convert.ToInt32(dr[column.ColumnName]);
                            }
                            else if (pro.PropertyType == typeof(Int32))
                            {
                                columnData = Convert.ToInt32(dr[column.ColumnName]);
                            }
                            else
                            {
                                columnData = dr[column.ColumnName];
                            }

                            pro.SetValue(obj, columnData, null);
                        }
                        catch (Exception ex)
                        {
                            if (string.IsNullOrEmpty(errorStr))
                            {
                                errorStr = $"<p> <b>Template data parse errors in Excel Data Row {rowNum} : </b> </p> <ul>";
                            }

                            var error = $"<li> Invalid data in COLUMN : '{column.ColumnName}'. Error : {ex.Message} </li>";
                            errorStr += error;
                            continue;
                        }
                    }
                    else
                        continue;
                }
            }

            if (!string.IsNullOrEmpty(errorStr))
            {
                errorStr += "</ul>";
            }

            return obj;
        }

        private void PickListDetailForGenerator(GeneratorDto generatorPickListDto)
        {
            generatorPickListDto.FrequencyList = _pickListProcessor.GetFrequency();
            generatorPickListDto.VoltageNominalList = _pickListProcessor.GetVoltageNominal(false);
            generatorPickListDto.ProductFamilyList = _pickListProcessor.GetProductFamily();
            generatorPickListDto.RegulatoryFilterList = _pickListProcessor.GetRegulatoryFilter();
            generatorPickListDto.FuelTypeList = _pickListProcessor.GetFuelTypeCode();
        }

        private void PickListDetailForAlternator(AlternatorDto alternatorPickListDto)
        {
            alternatorPickListDto.AlternatorFamilyList = _pickListProcessor.GetAlternatorFamily();
            alternatorPickListDto.VoltageNominalList = _pickListProcessor.GetVoltageNominal(false);
            alternatorPickListDto.FrequencyList = _pickListProcessor.GetFrequency();
            alternatorPickListDto.VoltagePhaseList = _pickListProcessor.GetVoltagePhase();
        }

        [HttpGet]
        [Route("GetDocumentationsByGenerator")]
        public HttpResponseMessage GetDocumentationsByGenerator(int generatorID)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_adminProcessor.GetDocumentationsByGenerator(generatorID));
            });
        }

        [HttpGet]
        [Route("GetDocumentation")]
        public HttpResponseMessage GetDocumentation(int documentationID)
        {
            return CreateHttpResponse(() =>
            {
                var requestDto = new SearchAdminRequestDto
                {
                    ID = documentationID
                };

                var documentationDto = _adminProcessor.GetDocumentation(requestDto);
                return Request.CreateResponse(documentationDto);
            });
        }

        [HttpPost]
        [Route("SaveDocumentation")]
        public HttpResponseMessage SaveDocumentation(DocumentationDto documentationResponseDto)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_adminProcessor.SaveDocumentation(documentationResponseDto));
            });
        }

        [HttpDelete]
        [Route("DeleteDocumentation")]
        public HttpResponseMessage DeleteDocumentation(int documentationID, int generatorID , string userName)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_adminProcessor.DeleteDocumentation(documentationID, generatorID, userName));
            });
        }

        [HttpGet]
        [Route("GetHarmonicDeviceTypeDetails")]
        public HttpResponseMessage GetHarmonicDeviceTypeDetails()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_adminProcessor.GetHarmonicDeviceTypeDetail());
            });
        }

        [HttpGet]
        [Route("GetHarmonicDeviceType")]
        public HttpResponseMessage GetHarmonicDeviceType(int ID)
        {
            return CreateHttpResponse(() =>
            {
                if (ID > 0)
                {
                    var requestDto = new SearchAdminRequestDto
                    {
                        ID = ID
                    };

                    var harmonicDeviceTypeDto = _adminProcessor.GetHarmonicDeviceType(requestDto);
                    PickListDetailForHarmonicDeviceType(harmonicDeviceTypeDto);
                    return Request.CreateResponse(harmonicDeviceTypeDto);
                }
                else
                {
                    var harmonicDeviceTypeDto = new HarmonicDeviceTypeDto();
                    PickListDetailForHarmonicDeviceType(harmonicDeviceTypeDto);
                    return Request.CreateResponse(harmonicDeviceTypeDto);
                }
            });
        }

        [HttpPost]
        [Route("SaveHarmonicDeviceType")]
        public HttpResponseMessage SaveHarmonicDeviceType(HarmonicDeviceTypeDto harmonicDeviceTypeResponseDto)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_adminProcessor.SaveHarmonicDeviceTypeDetail(harmonicDeviceTypeResponseDto));
            });
        }

        [HttpDelete]
        [Route("DeleteHarmonicDeviceType")]
        public HttpResponseMessage DeleteHarmonicDeviceType(int ID, string userName)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_adminProcessor.DeleteHarmonicDeviceType(ID, userName));
            });
        }

        private void PickListDetailForHarmonicDeviceType(HarmonicDeviceTypeDto harmonicDevicePickListDto)
        {
            harmonicDevicePickListDto.StartingMethodList = _pickListProcessor.GetStartingMethod();
            harmonicDevicePickListDto.HarmonicContentList = _pickListProcessor.GetHarmonicContentStartingMethod();
        }

    }



}
