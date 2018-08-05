using PowerDesignPro.BusinessProcessors.Dtos;
using PowerDesignPro.BusinessProcessors.Interface;
using System.Net.Http;
using System.Web.Http;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using PowerDesignPro.Common;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace PowerDesignProAPI.Controllers
{
    [Authorize]
    [RoutePrefix("Project/Solution")]
    public class SolutionController : BaseController
    {
        private IPickList _pickListProcessor;
        private IProjectSolution _projectSolutionProcessor;

        public SolutionController(
            IPickList pickListProcessor,
            IProjectSolution projectSolutionProcessor)
        {
            _pickListProcessor = pickListProcessor;
            _projectSolutionProcessor = projectSolutionProcessor;
        }


        [HttpGet]
        [Route("GetSolutionHeaderDetails")]
        public HttpResponseMessage GetSolutionHeaderDetails(int projectID, int solutionID)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_projectSolutionProcessor.GetSolutionHeaderDetails(UserID, projectID, solutionID, UserName));
            });
        }


        [HttpGet]
        [Route("GetDefaultSolutionSetupForNewSolution")]
        public HttpResponseMessage GetDefaultSolutionSetupForNewSolution(int projectId)
        {
            return CreateHttpResponse(() =>
            {
                var projectSolutionDto = _projectSolutionProcessor.LoadDefaultSolutionSetupForNewSolution(UserID, projectId);
                LoadPickListDetailForSolution(projectSolutionDto.ProjectSolutionPickListDto);
                return Request.CreateResponse(projectSolutionDto);
            });
        }

        [HttpGet]
        [Route("GetUserDefaultSolutionSetup")]
        public HttpResponseMessage GetUserDefaultSolutionSetup()
        {
            return CreateHttpResponse(() =>
            {
                var projectSolutionDto = _projectSolutionProcessor.LoadUserDefaultSolutionSetup(UserID);
                LoadPickListDetailForSolution(projectSolutionDto.ProjectSolutionPickListDto);
                return Request.CreateResponse(projectSolutionDto);
            });
        }

        [HttpGet]
        [Route("CheckUserDefaultSetup")]
        public HttpResponseMessage CheckUserDefaultSetup()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_projectSolutionProcessor.CheckUserDefaultSetup(UserID));
            });
        }

        [HttpGet]
        [Route("GetGlobalDefaultSolutionSetup")]
        public HttpResponseMessage GetGlobalDefaultSolutionSetup()
        {
            return CreateHttpResponse(() =>
            {
                var projectSolutionDto = _projectSolutionProcessor.LoadGlobalDefaultSolutionSetup();
                LoadPickListDetailForSolution(projectSolutionDto.ProjectSolutionPickListDto);
                return Request.CreateResponse(projectSolutionDto);
            });
        }

        [HttpGet]
        [Route("GetSolutionSetupForExistingSolution")]
        public HttpResponseMessage GetSolutionSetupForExistingSolution(int projectID, int solutionID)
        {
            return CreateHttpResponse(() =>
            {
                var projectSolutionDto = _projectSolutionProcessor.LoadSolutionSetupForExistingSolution(UserID, projectID, solutionID, UserName);
                LoadPickListDetailForSolution(projectSolutionDto.BaseSolutionSetupDto.ProjectSolutionPickListDto);
                return Request.CreateResponse(projectSolutionDto);
            });
        }

        [HttpPost]
        [Route("SaveSolutionDetail")]
        public HttpResponseMessage SaveSolutionDetail(ProjectSolutionDto projectSolutionResponseDto)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_projectSolutionProcessor.SaveSolutionDetail(projectSolutionResponseDto, UserID, UserName));
            });
        }

        [HttpPost]
        [Route("SaveUserDefaultSolutionDetail")]
        public HttpResponseMessage SaveUserDefaultSolutionDetail(UserDefaultSolutionSetupDto userDefaultSolutionSetupDto)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_projectSolutionProcessor.SaveUserDefaultSolutionSetup(userDefaultSolutionSetupDto, UserID, UserName));
            });
        }

        private void LoadPickListDetailForSolution(ProjectSolutionPickListDto projectSolutionPickListDto)
        {
            projectSolutionPickListDto.AmbientTemperatureList = _pickListProcessor.GetAmbientTemperature();
            projectSolutionPickListDto.ContinuousAllowableVoltageDistortionList = _pickListProcessor.GetContinuousAllowableVoltageDistortion();
            projectSolutionPickListDto.DesiredRunTimeList = _pickListProcessor.GetDesiredRunTime();
            projectSolutionPickListDto.DesiredSoundList = _pickListProcessor.GetDesiredSound();
            projectSolutionPickListDto.ElevationList = _pickListProcessor.GetElevation();
            projectSolutionPickListDto.EnclosureTypeList = _pickListProcessor.GetEnclosureType();
            projectSolutionPickListDto.EngineDutyList = _pickListProcessor.GetEngineDuty();
            projectSolutionPickListDto.FrequencyList = _pickListProcessor.GetFrequency();
            projectSolutionPickListDto.FrequencyDipList = _pickListProcessor.GetFrequencyDip();
            projectSolutionPickListDto.FrequencyDipUnitList = _pickListProcessor.GetFrequencyDipUnits();
            projectSolutionPickListDto.FuelTankList = _pickListProcessor.GetFuelTank();
            projectSolutionPickListDto.FuelTypeList = _pickListProcessor.GetFuelType();
            projectSolutionPickListDto.LoadSequenceCyclic = _pickListProcessor.GetLoadSequenceCyclic();
            projectSolutionPickListDto.MaxRunningLoadList = _pickListProcessor.GetMaxRunningLoad();
            projectSolutionPickListDto.MomentaryAllowableVoltageDistortionList = _pickListProcessor.GetMomentaryAllowableVoltageDistortion();
            projectSolutionPickListDto.SolutionApplicationList = _pickListProcessor.GetSolutionApplication();
            projectSolutionPickListDto.RegulatoryFilterList = _pickListProcessor.GetRegulatoryFilter();
            projectSolutionPickListDto.UnitsList = _pickListProcessor.GetUnits();
            projectSolutionPickListDto.VoltageDipList = _pickListProcessor.GetVoltageDip();
            projectSolutionPickListDto.VoltageDipUnitList = _pickListProcessor.GetVoltageDipUnits();
            projectSolutionPickListDto.VoltageNominalList = _pickListProcessor.GetVoltageNominal(false);
            projectSolutionPickListDto.VoltagePhaseList = _pickListProcessor.GetVoltagePhase();
            projectSolutionPickListDto.VoltageSpecificList = _pickListProcessor.GetVoltageSpecific();
        }

        [HttpPost]
        [Route("CopySolution")]
        public HttpResponseMessage CopySolution(SolutionRequestDto solutionRequestDto)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_projectSolutionProcessor.CopySolution(solutionRequestDto, UserID, UserName));
            });
        }

        //[HttpPost]
        //[Route("GrantEditAccess")]
        //public async Task<HttpResponseMessage> GrantEditAccess(SolutionRequestDto solutionRequestDto)
        //{
        //    var result = _projectSolutionProcessor.GrantEditAccess(solutionRequestDto, UserID, UserName);
        //    var clientURL = RequestContext.Url.Request.Headers.GetValues("Origin").FirstOrDefault();
        //    var callbackURL = $"{clientURL}/project/{solutionRequestDto.ProjectID}/solution/{solutionRequestDto.SolutionID}";

        //    MailMessage msg = new MailMessage();
        //    msg.From = new MailAddress("PowerDesignProTest@generac.com");
        //    msg.To.Add(new MailAddress(result["Email"]));
        //    msg.Subject = $"Project grant access provide to you";
        //    msg.Body = $"Project grant access provide to you by {UserName}. <br />Notes: {solutionRequestDto.Notes}.<br /><br />";
        //    msg.Body += "Please access the Project <a href=\"" + callbackURL + "\">here</a>.";
        //    //msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
        //    msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(msg.Body, null, MediaTypeNames.Text.Html));

        //    //await EmailHelper.ConfigSendGridasync(msg);
        //    await EmailHelper.SendGridasync(msg);

        //    return CreateHttpResponse(() =>
        //    {
        //        return Request.CreateResponse(result);
        //    });
        //}

        [HttpPost]
        [Route("GrantEditAccess")]
        public async Task<HttpResponseMessage> GrantEditAccess(SolutionRequestDto solutionRequestDto)
        {
            var clientURL = RequestContext.Url.Request.Headers.GetValues("Origin").FirstOrDefault();
            var urlBrand = "generac";
            if (clientURL.ToLower().Contains("pramac"))
            {
                urlBrand = "pramac";
            }

            if (String.IsNullOrEmpty(solutionRequestDto.Language) || solutionRequestDto.Language != "en" || solutionRequestDto.Language != "en")
                solutionRequestDto.Language = "en";

            var result = _projectSolutionProcessor.GrantEditAccess(solutionRequestDto, UserID, UserName);
            //var clientURL = RequestContext.Url.Request.Headers.GetValues("Origin").FirstOrDefault();
            var callbackURL = $"{clientURL}/project/{solutionRequestDto.ProjectID}/solution/{solutionRequestDto.SolutionID}";
            var projectOwnerUserDetails = GetUserDetailsByUserName(result.RecipientEmail);
            var shareeUserDetails = GetUserDetailsByUserName(UserName);

            var emaildata = new SendGridEmailData
            {
                template_id = SectionHandler.GetSectionValue($"{solutionRequestDto.Language.ToLower()}/emailTemplates/grantEditAccess", "TemplateID", ""),
                personalizations = new List<Personalization>()
                {
                    new Personalization
                    {
                        to = new List<To>()
                        {
                            new To
                            {
                                email = result.RecipientEmail,
                                name = projectOwnerUserDetails.FirstName + " " + projectOwnerUserDetails.LastName
                            }
                        },
                        substitutions = new Dictionary<string, string>()
                        {
                            { "%FirstName%", projectOwnerUserDetails.FirstName},
                            { "%LastName%", projectOwnerUserDetails.LastName},
                            { "%ShareeFirstName%", shareeUserDetails.LastName},
                            { "%ShareeLastName%", shareeUserDetails.LastName},
                            { "%ShareeEmail%", shareeUserDetails.Email},
                            { "%SolutionName%", result.SolutionName},
                            { "%ProjectName%", result.ProjectName },
                            { "%SolutionComments%", result.SolutionComments },
                            { "%SolutionLink%", callbackURL },
                            { "%CompanyName%", EmailHelper.CompanyName(solutionRequestDto.Language, urlBrand) },
                            { "%CompanyAddress%", EmailHelper.CompanyAddress(solutionRequestDto.Language, urlBrand) }
                        }
                    }
                }
            };

            var emailResponse = await EmailHelper.SendGridAsyncWithTemplate(emaildata);

            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(result);
            });
        }
    }
}
