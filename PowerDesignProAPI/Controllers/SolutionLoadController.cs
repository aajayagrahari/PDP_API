using System.Net.Http;
using System.Web.Http;
using PowerDesignPro.BusinessProcessors.Dtos;
using PowerDesignPro.BusinessProcessors.Interface;
using PowerDesignPro.Common.Constant;

namespace PowerDesignProAPI.Controllers
{
    [Authorize]
    [RoutePrefix("Solution/Load")]
    public class SolutionLoadController : BaseController
    {
        private readonly ISolutionLoad _solutionLoadProcessor;
        private IPickList _pickListProcessor;

        public SolutionLoadController(
            ISolutionLoad solutionLoadProcessor,
            IPickList pickListProcessor)
        {
            _solutionLoadProcessor = solutionLoadProcessor;
            _pickListProcessor = pickListProcessor;
        }

        [HttpGet]
        [Route("CheckLoadExistForSolution")]
        public HttpResponseMessage CheckLoadExistForSolution(int solutionId)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_solutionLoadProcessor.CheckLoadExistForSolution(solutionId));
            });
        }

        [HttpGet]
        [Route("GetLoadDetails")]
        public HttpResponseMessage GetLoadDetails(int solutionId)
        {
            return CreateHttpResponse(() =>
            {

                var solutionDetail = _solutionLoadProcessor.GetSolutionDetail(solutionId);

                return Request.CreateResponse(new BaseLoadDto
                {
                    SolutionId = solutionId,
                    SolutionName = solutionDetail.SolutionName,
                    Loads = _pickListProcessor.GetLoads()
                });
            });
        }

        [HttpGet]
        [Route("GetSolutionLoadDetails")]
        public HttpResponseMessage GetSolutionLoadDetails(int solutionId, int loadId, int loadFamilyId, int? solutionLoadId = 0)
        {
            return CreateHttpResponse(() =>
            {
                var requestDto = new SearchBaseLoadRequestDto
                {
                    ID = solutionLoadId,
                    LoadID = loadId,
                    SolutionID = solutionId,
                    LoadFamilyID = loadFamilyId
                };

                switch (loadFamilyId)
                {
                    case (int)SolutionLoadFamilyEnum.Basic:
                        {
                            var basicLoadDto = _solutionLoadProcessor.GetLoadDetailsForBasicLoad(requestDto, UserName);
                            LoadPickListDetailForBasicLoad(basicLoadDto.BasicLoadPickListDto);

                            return Request.CreateResponse(basicLoadDto);
                        }
                    case (int)SolutionLoadFamilyEnum.AC:
                        {
                            var acLoadDto = _solutionLoadProcessor.GetLoadDetailsForACLoad(requestDto, UserName);
                            LoadPickListDetailForACLoad(acLoadDto.ACLoadPickListDto);

                            return Request.CreateResponse(acLoadDto);
                        }
                    case (int)SolutionLoadFamilyEnum.Lighting:
                        {
                            var acLoadDto = _solutionLoadProcessor.GetLoadDetailsForLightingLoad(requestDto, UserName);
                            LoadPickListDetailForLightingLoad(acLoadDto.LightingLoadPickListDto);

                            return Request.CreateResponse(acLoadDto);
                        }
                    case (int)SolutionLoadFamilyEnum.UPS:
                        {
                            var upsLoadDto = _solutionLoadProcessor.GetLoadDetailsForUpsLoad(requestDto, UserName);
                            LoadPickListDetailForUpsLoad(upsLoadDto.UPSLoadPickListDto);

                            return Request.CreateResponse(upsLoadDto);
                        }
                    case (int)SolutionLoadFamilyEnum.Welder:
                        {
                            var welderLoadDto = _solutionLoadProcessor.GetLoadDetailsForWelderLoad(requestDto, UserName);
                            LoadPickListDetailForWelderLoad(welderLoadDto.WelderLoadPickListDto);

                            return Request.CreateResponse(welderLoadDto);
                        }
                    case (int)SolutionLoadFamilyEnum.Motor:
                        {
                            var motorLoadDto = _solutionLoadProcessor.GetLoadDetailsForMotorLoad(requestDto, UserName);
                            LoadPickListDetailForMotorLoad(motorLoadDto.MotorLoadPickListDto);
                            return Request.CreateResponse(motorLoadDto);
                        }
                    default:
                        return null;
                }

            });
        }

        [HttpPost]
        [Route("SaveSolutionLoadDetail")]
        public HttpResponseMessage SaveSolutionLoadDetail(BasicLoadDto basicLoadDto)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_solutionLoadProcessor.SaveSolutionBasicLoad(basicLoadDto, UserID, UserName));
            });
        }

        [HttpDelete]
        [Route("DeleteSolutionLoad")]
        public HttpResponseMessage DeleteSolutionLoad(int solutionID, int solutionLoadID, int loadFamilyID)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_solutionLoadProcessor.DeleteSolutionLoad(solutionID, solutionLoadID, loadFamilyID, UserID));
            });
        }

        [HttpPost]
        [Route("SaveSolutionAcLoadDetail")]
        public HttpResponseMessage SaveSolutionAcLoadDetail(ACLoadDto acLoadDto)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_solutionLoadProcessor.SaveSolutionACLoad(acLoadDto, UserID, UserName));
            });
        }

        [HttpPost]
        [Route("SaveSolutionWelderLoadDetail")]
        public HttpResponseMessage SaveSolutionWelderLoadDetail(WelderLoadDto welderLoadDto)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_solutionLoadProcessor.SaveSolutionWelderLoad(welderLoadDto, UserID, UserName));
            });
        }

        [HttpPost]
        [Route("SaveSolutionLightingLoadDetail")]
        public HttpResponseMessage SaveSolutionLightingLoadDetail(LightingLoadDto lightingLoadDto)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_solutionLoadProcessor.SaveSolutionLightingLoad(lightingLoadDto, UserID, UserName));
            });
        }

        [HttpPost]
        [Route("SaveSolutionUPSLoadDetail")]
        public HttpResponseMessage SaveSolutionUPSLoadDetail(UPSLoadDto upsLoadDto)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_solutionLoadProcessor.SaveSolutionUpsLoad(upsLoadDto, UserID, UserName));
            });
        }

        [HttpPost]
        [Route("SaveSolutionMotorLoadDetail")]
        public HttpResponseMessage SaveSolutionMotorLoadDetail(MotorLoadDto motorLoadDto)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_solutionLoadProcessor.SaveSolutionMotorLoad(motorLoadDto, UserID, UserName));
            });
        }

        private void LoadPickListDetailForBasicLoad(BasicLoadPickListDto basicLoadPickListDto)
        {
            basicLoadPickListDto.FrequencyDipList = _pickListProcessor.GetFrequencyDip();
            basicLoadPickListDto.FrequencyDipUnitsList = _pickListProcessor.GetFrequencyDipUnits();
            basicLoadPickListDto.HarmonicContentList = _pickListProcessor.GetHarmonicContent();
            basicLoadPickListDto.HarmonicDeviceTypeList = _pickListProcessor.GetHarmonicDeviceType();
            basicLoadPickListDto.PFList = _pickListProcessor.GetPF();
            basicLoadPickListDto.SequenceList = _pickListProcessor.GetSequence();
            basicLoadPickListDto.SizeUnitsList = _pickListProcessor.GetSizeUnits();
            basicLoadPickListDto.VoltageDipList = _pickListProcessor.GetVoltageDip();
            basicLoadPickListDto.VoltageDipUnitsList = _pickListProcessor.GetVoltageDipUnits();
            basicLoadPickListDto.VoltageNominalList = _pickListProcessor.GetVoltageNominal(true);
            basicLoadPickListDto.VoltagePhaseList = _pickListProcessor.GetVoltagePhase();
            basicLoadPickListDto.VoltageSpecificList = _pickListProcessor.GetVoltageSpecific();
        }

        private void LoadPickListDetailForACLoad(ACLoadPickListDto acLoadPickListDto)
        {
            acLoadPickListDto.SequenceList = _pickListProcessor.GetSequence();
            acLoadPickListDto.FrequencyDipList = _pickListProcessor.GetFrequencyDip();
            acLoadPickListDto.FrequencyDipUnitsList = _pickListProcessor.GetFrequencyDipUnits();
            acLoadPickListDto.VoltageDipList = _pickListProcessor.GetVoltageDip();
            acLoadPickListDto.VoltageDipUnitsList = _pickListProcessor.GetVoltageDipUnits();
            acLoadPickListDto.CoolingUnitsList = _pickListProcessor.GetSizeUnits();
            acLoadPickListDto.CoolingLoadList = _pickListProcessor.GetCoolingLoad();
            acLoadPickListDto.CompressorsList = _pickListProcessor.GetCompressors();
            acLoadPickListDto.ReheatLoadList = _pickListProcessor.GetReheatLoad();
        }

        private void LoadPickListDetailForLightingLoad(LightingLoadPickListDto lightingLoadPickListDto)
        {
            lightingLoadPickListDto.FrequencyDipList = _pickListProcessor.GetFrequencyDip();
            lightingLoadPickListDto.FrequencyDipUnitsList = _pickListProcessor.GetFrequencyDipUnits();
            lightingLoadPickListDto.HarmonicContentList = _pickListProcessor.GetHarmonicContent();
            lightingLoadPickListDto.HarmonicDeviceTypeList = _pickListProcessor.GetHarmonicDeviceType();
            lightingLoadPickListDto.PFList = _pickListProcessor.GetPF();
            lightingLoadPickListDto.SequenceList = _pickListProcessor.GetSequence();
            lightingLoadPickListDto.SizeUnitsList = _pickListProcessor.GetSizeUnits();
            lightingLoadPickListDto.VoltageDipList = _pickListProcessor.GetVoltageDip();
            lightingLoadPickListDto.VoltageDipUnitsList = _pickListProcessor.GetVoltageDipUnits();
            lightingLoadPickListDto.VoltageNominalList = _pickListProcessor.GetVoltageNominal(true);
            lightingLoadPickListDto.VoltagePhaseList = _pickListProcessor.GetVoltagePhase();
            lightingLoadPickListDto.VoltageSpecificList = _pickListProcessor.GetVoltageSpecific();
            lightingLoadPickListDto.LightingTypeList = _pickListProcessor.GetLightingType();
        }

        private void LoadPickListDetailForUpsLoad(UPSLoadPickListDto upsLoadPickListDto)
        {
            upsLoadPickListDto.FrequencyDipList = _pickListProcessor.GetFrequencyDip();
            upsLoadPickListDto.FrequencyDipUnitsList = _pickListProcessor.GetFrequencyDipUnits();
            upsLoadPickListDto.HarmonicContentList = _pickListProcessor.GetHarmonicContent();
            upsLoadPickListDto.HarmonicDeviceTypeList = _pickListProcessor.GetHarmonicDeviceType();
            upsLoadPickListDto.SequenceList = _pickListProcessor.GetSequence();
            upsLoadPickListDto.VoltageDipList = _pickListProcessor.GetVoltageDip();
            upsLoadPickListDto.VoltageDipUnitsList = _pickListProcessor.GetVoltageDipUnits();
            upsLoadPickListDto.PhaseList = _pickListProcessor.GetPhase();
            upsLoadPickListDto.EfficiencyList = _pickListProcessor.GetEfficiency();
            upsLoadPickListDto.ChargeRateList = _pickListProcessor.GetChargeRate();
            upsLoadPickListDto.PowerFactorList = _pickListProcessor.GetPowerFactor();
            upsLoadPickListDto.LoadLevelList = _pickListProcessor.GetLoadLevel();
            upsLoadPickListDto.UPSTypeList = _pickListProcessor.GetUPSType();
            upsLoadPickListDto.SizeKVAUnitsList = _pickListProcessor.GetSizeUnits();
        }

        private void LoadPickListDetailForMotorLoad(MotorLoadPickListDto motorLoadPickListDto)
        {
            motorLoadPickListDto.VoltageNominalList = _pickListProcessor.GetVoltageNominal(true);
            motorLoadPickListDto.VoltagePhaseList = _pickListProcessor.GetVoltagePhase();
            motorLoadPickListDto.VoltageSpecificList = _pickListProcessor.GetVoltageSpecific();
            motorLoadPickListDto.SizeUnitsList = _pickListProcessor.GetSizeUnits();
            motorLoadPickListDto.HarmonicContentList = _pickListProcessor.GetMotorHarmonicContent();
            motorLoadPickListDto.HarmonicDeviceTypeList = _pickListProcessor.GetHarmonicDeviceType();
            motorLoadPickListDto.MotorLoadLevelList = _pickListProcessor.GetMotorLoadLevel();
            motorLoadPickListDto.MotorLoadTypeList = _pickListProcessor.GetMotorLoadType();
            motorLoadPickListDto.MotorTypeList = _pickListProcessor.GetMotorType();
            motorLoadPickListDto.StartingCodeList = _pickListProcessor.GetStartingCode();
            motorLoadPickListDto.ConfigurationInputList = _pickListProcessor.GetConfigurationInput();
            motorLoadPickListDto.SequenceList = _pickListProcessor.GetSequence();
            motorLoadPickListDto.StartingMethodList = _pickListProcessor.GetStartingMethod();
            motorLoadPickListDto.VoltageDipList = _pickListProcessor.GetVoltageDip();
            motorLoadPickListDto.VoltageDipUnitsList = _pickListProcessor.GetVoltageDipUnits();
            motorLoadPickListDto.FrequencyDipList = _pickListProcessor.GetFrequencyDip();
            motorLoadPickListDto.FrequencyDipUnitsList = _pickListProcessor.GetFrequencyDipUnits();
            motorLoadPickListDto.MotorCalculationList = _pickListProcessor.GetMotorCalculation();
        }

        private void LoadPickListDetailForWelderLoad(WelderLoadPickListDto welderLoadPickListDto)
        {
            welderLoadPickListDto.FrequencyDipList = _pickListProcessor.GetFrequencyDip();
            welderLoadPickListDto.FrequencyDipUnitsList = _pickListProcessor.GetFrequencyDipUnits();
            welderLoadPickListDto.HarmonicContentList = _pickListProcessor.GetHarmonicContent();
            welderLoadPickListDto.HarmonicDeviceTypeList = _pickListProcessor.GetHarmonicDeviceType();
            welderLoadPickListDto.PFList = _pickListProcessor.GetPF();
            welderLoadPickListDto.SequenceList = _pickListProcessor.GetSequence();
            welderLoadPickListDto.SizeUnitsList = _pickListProcessor.GetSizeUnits();
            welderLoadPickListDto.VoltageDipList = _pickListProcessor.GetVoltageDip();
            welderLoadPickListDto.VoltageDipUnitsList = _pickListProcessor.GetVoltageDipUnits();
            welderLoadPickListDto.VoltageNominalList = _pickListProcessor.GetVoltageNominal(true);
            welderLoadPickListDto.VoltagePhaseList = _pickListProcessor.GetVoltagePhase();
            welderLoadPickListDto.VoltageSpecificList = _pickListProcessor.GetVoltageSpecific();
            welderLoadPickListDto.WelderTypeList = _pickListProcessor.GetWelderType();
        }
    }
}
