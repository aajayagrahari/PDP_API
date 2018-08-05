using PowerDesignPro.BusinessProcessors.Interface;
using System.Net.Http;
using System.Web.Http;

namespace PowerDesignProAPI.Controllers
{
    [Authorize]
    [RoutePrefix("PickList")]
    public class PickListController : BaseController
    {
        private IPickList _pickListProcessor;

        public PickListController(IPickList pickListProcessor)
        {
            _pickListProcessor = pickListProcessor;
        }

        [HttpGet]
        [Route("GetVoltageNominalByFrequency")]
        public HttpResponseMessage GetVoltageNominalForFrequency(int frequencyId)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetVoltageNominal(frequencyId));
            });
        }

     
        #region Solution Setup Picklists

        [HttpGet]
        [Route("GetFrequencyDipUnits")]
        public HttpResponseMessage GetFrequencyDipUnits()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetFrequencyDipUnits());
            });
        }

        [HttpGet]
        [Route("GetAmbientTemperature")]
        public HttpResponseMessage GetAmbientTemperature()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetAmbientTemperature());
            });
        }

        [HttpGet]
        [Route("GetElevation")]
        public HttpResponseMessage GetElevation()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetElevation());
            });
        }

        [HttpGet]
        [Route("GetEngineDuty")]
        public HttpResponseMessage GetEngineDuty()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetEngineDuty());
            });
        }

        [HttpGet]
        [Route("GetFrequency")]
        public HttpResponseMessage GetFrequency()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetFrequency());
            });
        }

        [HttpGet]
        [Route("GetFrequencyDip")]
        public HttpResponseMessage GetFrequencyDip()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetFrequencyDip());
            });
        }

        [HttpGet]
        [Route("GetFuelType")]
        public HttpResponseMessage GetFuelType()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetFuelType());
            });
        }

        [HttpGet]
        [Route("GetMaxRunningLoad")]
        public HttpResponseMessage GetMaxRunningLoad()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetMaxRunningLoad());
            });
        }

        [HttpGet]
        [Route("GetUnits")]
        public HttpResponseMessage GetUnits()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetUnits());
            });
        }

        [HttpGet]
        [Route("GetVoltageDip")]
        public HttpResponseMessage GetVoltageDip()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetVoltageDip());
            });
        }

        [HttpGet]
        [Route("GetVoltageDipUnits")]
        public HttpResponseMessage GetVoltageDipUnits()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetVoltageDipUnits());
            });
        }

        [HttpGet]
        [Route("GetVoltageNominal")]
        public HttpResponseMessage GetVoltageNominal(bool IsForLoads)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetVoltageNominal(IsForLoads));
            });
        }

        [HttpGet]
        [Route("GetVoltagePhase")]
        public HttpResponseMessage GetVoltagePhase()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetVoltagePhase());
            });
        }

        [HttpGet]
        [Route("GetVoltageSpecific")]
        public HttpResponseMessage GetVoltageSpecific()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetVoltageSpecific());
            });
        }

        [HttpGet]
        [Route("GetLoadFamily")]
        public HttpResponseMessage GetLoadFamily()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetLoadFamily());
            });
        }

        [HttpGet]
        [Route("GetContinuousAllowableVoltageDistortion")]
        public HttpResponseMessage GetContinuousAllowableVoltageDistortion()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetContinuousAllowableVoltageDistortion());
            });
        }

        [HttpGet]
        [Route("GetDesiredRunTime")]
        public HttpResponseMessage GetDesiredRunTime()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetDesiredRunTime());
            });
        }

        [HttpGet]
        [Route("GetDesiredSound")]
        public HttpResponseMessage GetDesiredSound()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetDesiredSound());
            });
        }

        [HttpGet]
        [Route("GetEnclosureType")]
        public HttpResponseMessage GetEnclosureType()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetEnclosureType());
            });
        }

        [HttpGet]
        [Route("GetFuelTank")]
        public HttpResponseMessage GetFuelTank()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetFuelTank());
            });
        }

        [HttpGet]
        [Route("GetLoadSequenceCyclic")]
        public HttpResponseMessage GetLoadSequenceCyclic()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetLoadSequenceCyclic());
            });
        }

        [HttpGet]
        [Route("GetMomentaryAllowableVoltageDistortion")]
        public HttpResponseMessage GetMomentaryAllowableVoltageDistortion()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetMomentaryAllowableVoltageDistortion());
            });
        }

        [HttpGet]
        [Route("GetSolutionApplication")]
        public HttpResponseMessage GetSolutionApplication()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetSolutionApplication());
            });
        }

        #endregion

        #region Registration Picklists

        [HttpGet]
        [Route("State")]
        public HttpResponseMessage GetState()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetState());
            });
        }

        [HttpGet]
        [Route("Country")]
        public HttpResponseMessage GetCountry()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetCountry());
            });
        }

        [HttpGet]
        [Route("Brand")]
        public HttpResponseMessage GetBrand()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetBrand());
            });
        }

        [HttpGet]
        [Route("UserCategory")]
        public HttpResponseMessage GetUserCategory()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetUserCategory());
            });
        }

        #endregion

        #region Loads Picklists

        [HttpGet]
        [Route("Loads")]
        public HttpResponseMessage GetLoads()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetLoads());
            });
        }

        [HttpGet]
        [Route("Sequence")]
        public HttpResponseMessage GetSequence()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetSequence());
            });
        }

        [HttpGet]
        [Route("PF")]
        public HttpResponseMessage GetPF()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetPF());
            });
        }

        [HttpGet]
        [Route("SizeUnits")]
        public HttpResponseMessage GetSizeUnits()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetSizeUnits());
            });
        }

        [HttpGet]
        [Route("HarmonicDeviceType")]
        public HttpResponseMessage GetHarmonicDeviceType()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetHarmonicDeviceType());
            });
        }

        [HttpGet]
        [Route("HarmonicContent")]
        public HttpResponseMessage GetHarmonicContent()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetHarmonicContent());
            });
        }

        #endregion

        #region AC Load PickList

        [HttpGet]
        [Route("Compressors")]
        public HttpResponseMessage GetCompressors()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetCompressors());
            });
        }

        [HttpGet]
        [Route("CoolingLoad")]
        public HttpResponseMessage GetCoolingLoad()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetCoolingLoad());
            });
        }

        [HttpGet]
        [Route("ReheatLoad")]
        public HttpResponseMessage GetReheatLoad()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetReheatLoad());
            });
        }
        #endregion

        #region Lighting Load PickList
        [HttpGet]
        [Route("LightingType")]
        public HttpResponseMessage GetLightingType()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetLightingType());
            });
        }
        #endregion

        #region UPS Load PickList
        [HttpGet]
        [Route("Phase")]
        public HttpResponseMessage GetPhase()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetPhase());
            });
        }

        [HttpGet]
        [Route("Efficiency")]
        public HttpResponseMessage GetEfficiency()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetEfficiency());
            });
        }

        [HttpGet]
        [Route("ChargeRate")]
        public HttpResponseMessage GetChargeRate()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetChargeRate());
            });
        }

        [HttpGet]
        [Route("PowerFactor")]
        public HttpResponseMessage GetPowerFactor()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetPowerFactor());
            });
        }


        [HttpGet]
        [Route("UpsType")]
        public HttpResponseMessage GetUpsType()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetUPSType());
            });
        }


        [HttpGet]
        [Route("LoadLevel")]
        public HttpResponseMessage GetLoadLevel()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetLoadLevel());
            });
        }
        #endregion

        #region Welder Load PickList
        [HttpGet]
        [Route("WelderType")]
        public HttpResponseMessage GetWelderType()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetWelderType());
            });
        }
        #endregion

    }
}
