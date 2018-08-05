using PowerDesignPro.BusinessProcessors.Dtos;
using PowerDesignPro.BusinessProcessors.Interface;
using PowerDesignPro.Common.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PowerDesignProAPI.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("AdminLoad")]
    public class AdminLoadController : BaseController
    {
        private readonly IAdminLoad _adminLoadProcessor;
        private IPickList _pickListProcessor;
        private ITraceMessage _traceMessageProcessor;

        public AdminLoadController(
            IAdminLoad adminLoadProcessor,
            IPickList pickListProcessor,
            ITraceMessage traceMessageProcessor)
        {
            _adminLoadProcessor = adminLoadProcessor;
            _pickListProcessor = pickListProcessor;
            _traceMessageProcessor = traceMessageProcessor;
        }

        [HttpGet]
        [Route("GetAllLoads")]
        public HttpResponseMessage GetGeneratorDetails()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_adminLoadProcessor.GetLoads());
            });
        }

        [HttpPost]
        [Route("GetDefaultLoadDetails")]
        public HttpResponseMessage GetDefaultLoadDetails(SearchBaseLoadRequestDto searchBasicLoadDto)
        {
            return CreateHttpResponse(() =>
            {
                var adminLoadDefaultDto = _adminLoadProcessor.GetDefaultLoadDetails(searchBasicLoadDto);
                PickListDetailForDefaultLoad(adminLoadDefaultDto, searchBasicLoadDto.LoadFamilyID);
                return Request.CreateResponse(adminLoadDefaultDto);
            });
        }

        [HttpPost]
        [Route("SaveLoadDefault")]
        public HttpResponseMessage SaveLoadDefault(AdminLoadDefaultDto adminLoadDefaultDto)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_adminLoadProcessor.SaveLoadDetail(adminLoadDefaultDto, adminLoadDefaultDto.UserName));
            });
        }

        [HttpDelete]
        [Route("DeleteLoadDefault")]
        public HttpResponseMessage DeleteLoadDefault(int loadID, string userName)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_adminLoadProcessor.DeleteLoadDefault(loadID, userName));
            });
        }

        //[AllowAnonymous]
        [HttpPost]
        [Route("UpdateLoadDetails")]
        public HttpResponseMessage UpdateLoadDetails(List<string> userEmailList)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_adminLoadProcessor.UpdateLoadDetails(userEmailList));
            });
        }

        private void PickListDetailForDefaultLoad(AdminLoadDefaultDto adminLoadDefaultDto, int loadFamilyID)
        {
            adminLoadDefaultDto.SequenceList = _pickListProcessor.GetSequence();
            adminLoadDefaultDto.VoltageDipList = _pickListProcessor.GetVoltageDip();
            adminLoadDefaultDto.VoltageDipUnitsList = _pickListProcessor.GetVoltageDipUnits();
            adminLoadDefaultDto.FrequencyDipList = _pickListProcessor.GetFrequencyDip();
            adminLoadDefaultDto.FrequencyDipUnitsList = _pickListProcessor.GetFrequencyDipUnits();
            var sizingUnitList = _pickListProcessor.GetSizeUnits();

            if ((int)SolutionLoadFamilyEnum.AC == loadFamilyID)
            {
                adminLoadDefaultDto.CoolingUnitsList = sizingUnitList.Where(x => x.LoadFamilyID == loadFamilyID);
                adminLoadDefaultDto.CompressorsList = _pickListProcessor.GetCompressors();
                adminLoadDefaultDto.CoolingLoadList = _pickListProcessor.GetCoolingLoad();
                adminLoadDefaultDto.ReheatLoadList = _pickListProcessor.GetReheatLoad();
            }
            if ((int)SolutionLoadFamilyEnum.Lighting == loadFamilyID || (int)SolutionLoadFamilyEnum.Basic == loadFamilyID || (int)SolutionLoadFamilyEnum.UPS == loadFamilyID || (int)SolutionLoadFamilyEnum.Welder == loadFamilyID)
            {
                adminLoadDefaultDto.PFList = _pickListProcessor.GetPF();
            }
            if ((int)SolutionLoadFamilyEnum.Lighting == loadFamilyID || (int)SolutionLoadFamilyEnum.Basic == loadFamilyID || (int)SolutionLoadFamilyEnum.Motor == loadFamilyID || (int)SolutionLoadFamilyEnum.Welder == loadFamilyID)
            {
                adminLoadDefaultDto.VoltagePhaseList = _pickListProcessor.GetVoltagePhase();
                adminLoadDefaultDto.VoltageNominalList = _pickListProcessor.GetVoltageNominal(true);
                adminLoadDefaultDto.VoltageSpecificList = _pickListProcessor.GetVoltageSpecific();
            }
            if ((int)SolutionLoadFamilyEnum.Lighting == loadFamilyID)
            {
                adminLoadDefaultDto.LightingTypeList = _pickListProcessor.GetLightingType();
            }
            if ((int)SolutionLoadFamilyEnum.Motor == loadFamilyID)
            {
                adminLoadDefaultDto.MotorLoadLevelList = _pickListProcessor.GetMotorLoadLevel();
                adminLoadDefaultDto.MotorLoadTypeList = _pickListProcessor.GetMotorLoadType();
                adminLoadDefaultDto.MotorTypeList = _pickListProcessor.GetMotorType();
                adminLoadDefaultDto.StartingCodeList = _pickListProcessor.GetStartingCode();
                adminLoadDefaultDto.ConfigurationInputList = _pickListProcessor.GetConfigurationInput();
                adminLoadDefaultDto.StartingMethodList = _pickListProcessor.GetStartingMethod();
                adminLoadDefaultDto.PowerFactorList = _pickListProcessor.GetPowerFactor();

            }
            if ((int)SolutionLoadFamilyEnum.UPS == loadFamilyID)
            {
                adminLoadDefaultDto.PhaseList = _pickListProcessor.GetPhase();
                adminLoadDefaultDto.EfficiencyList = _pickListProcessor.GetEfficiency();
                adminLoadDefaultDto.ChargeRateList = _pickListProcessor.GetChargeRate();
                adminLoadDefaultDto.PowerFactorList = _pickListProcessor.GetPowerFactor();
                adminLoadDefaultDto.LoadLevelList = _pickListProcessor.GetLoadLevel();
                adminLoadDefaultDto.UPSTypeList = _pickListProcessor.GetUPSType();
                adminLoadDefaultDto.SizeKVAUnitsList = sizingUnitList.Where(x => x.LoadFamilyID == loadFamilyID);
            }
            if ((int)SolutionLoadFamilyEnum.UPS == loadFamilyID || (int)SolutionLoadFamilyEnum.Lighting == loadFamilyID || (int)SolutionLoadFamilyEnum.Basic == loadFamilyID || (int)SolutionLoadFamilyEnum.Motor == loadFamilyID || (int)SolutionLoadFamilyEnum.Welder == loadFamilyID)
            {
                adminLoadDefaultDto.SizeUnitsList = sizingUnitList.Where(x => x.LoadFamilyID == loadFamilyID);
            }
            if ((int)SolutionLoadFamilyEnum.Welder == loadFamilyID)
            {
                adminLoadDefaultDto.WelderTypeList = _pickListProcessor.GetWelderType();
            }
            if ((int)SolutionLoadFamilyEnum.Welder == loadFamilyID || (int)SolutionLoadFamilyEnum.UPS == loadFamilyID || (int)SolutionLoadFamilyEnum.Lighting == loadFamilyID || (int)SolutionLoadFamilyEnum.Basic == loadFamilyID || (int)SolutionLoadFamilyEnum.Motor == loadFamilyID)
            {
                adminLoadDefaultDto.HarmonicDeviceTypeList = _pickListProcessor.GetHarmonicDeviceType();
                adminLoadDefaultDto.HarmonicContentList = _pickListProcessor.GetHarmonicContent();
            }


        }
    }
}
