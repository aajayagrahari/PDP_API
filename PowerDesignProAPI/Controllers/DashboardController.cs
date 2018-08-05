using PowerDesignPro.BusinessProcessors;
using PowerDesignPro.BusinessProcessors.Dtos;
using PowerDesignPro.BusinessProcessors.Interface;
using PowerDesignPro.Common;
using PowerDesignPro.Common.Constant;
using PowerDesignPro.Common.MessageConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web.Http;

namespace PowerDesignProAPI.Controllers
{
    [Authorize]
    [RoutePrefix("User/Dashboard")]
    public class DashboardController : BaseController
    {
        private IUserDashboard _userDashboard;
        private IPickList _pickListProcessor;

        public DashboardController(IUserDashboard userDashboard, IPickList pickListProcessor)
        {
            _userDashboard = userDashboard;
            _pickListProcessor = pickListProcessor;
        }

        [HttpGet]
        [Route("RecentProjects")]
        public HttpResponseMessage GetRecentProjects(int daysOld)
        {
            return CreateHttpResponse(() =>
            {
                var requestDto = new SearchProjectRequestDto
                {
                    UserID = UserID,
                    DaysOld = daysOld
                };

                return Request.CreateResponse(_userDashboard.GetProjects(requestDto));
            });
        }

        [HttpGet]
        [Route("GetProjectsByName")]
        public HttpResponseMessage GetProjectsByName(string projectName)
        {
            return CreateHttpResponse(() =>
            {
                var requestDto = new SearchProjectRequestDto
                {
                    ProjectName = projectName,
                    UserID = UserID,
                    SolutionName = string.Empty,
                    CreateDate = string.Empty,
                    ModifyDate = string.Empty
                };

                return Request.CreateResponse(_userDashboard.GetProjects(requestDto));
            });
        }


        [HttpGet]
        [Route("GetProjectsBySolutionName")]
        public HttpResponseMessage GetProjectsBySolutionName(string solutionName)
        {
            return CreateHttpResponse(() =>
            {
                var requestDto = new SearchProjectRequestDto
                {
                    ProjectName = string.Empty,
                    UserID = UserID,
                    SolutionName = solutionName,
                    CreateDate = string.Empty,
                    ModifyDate = string.Empty
                };

                return Request.CreateResponse(_userDashboard.GetProjects(requestDto));
            });
        }

        [HttpGet]
        [Route("GetProjectsByCreateDate")]
        public HttpResponseMessage GetProjectsByCreateDate(string createDate)
        {
            return CreateHttpResponse(() =>
            {
                var requestDto = new SearchProjectRequestDto
                {
                    ProjectName = string.Empty,
                    UserID = UserID,
                    SolutionName = string.Empty,
                    CreateDate  = createDate,
                    ModifyDate = string.Empty
                };

                return Request.CreateResponse(_userDashboard.GetProjects(requestDto));
            });
        }

        [HttpGet]
        [Route("GetProjectsByModifyDate")]
        public HttpResponseMessage GetProjectsByModifyDate(string modifyDate)
        {
            return CreateHttpResponse(() =>
            {
                var requestDto = new SearchProjectRequestDto
                {
                    ProjectName = string.Empty,
                    UserID = UserID,
                    SolutionName = string.Empty,
                    CreateDate = string.Empty,
                    ModifyDate = modifyDate
                };

                return Request.CreateResponse(_userDashboard.GetProjects(requestDto));
            });
        }

        [HttpPost]
        [Route("AddProject")]
        public HttpResponseMessage AddProject(AddProjectDto requestDto)
        {
            if (requestDto == null || !ModelState.IsValid)
            {
                var errorResponse = new
                {
                    ErrorCode = -1,
                    ErrorDescription = MessageCollection.Instance.GetMessage("NameFieldRequired", Message.ProjectDashboard)
                };

                return Request.CreateResponse(HttpStatusCode.OK, errorResponse);
            }
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_userDashboard.AddProject(requestDto, UserID, UserName));
            });

        }

        [HttpPost]
        [Route("UpdateProject")]
        public HttpResponseMessage UpdateProject(AddProjectDto requestDto)
        {
            if (requestDto == null || !ModelState.IsValid)
            {
                var errorResponse = new
                {
                    ErrorCode = -1,
                    ErrorDescription = MessageCollection.Instance.GetMessage("NameFieldRequired", Message.ProjectDashboard)
                };

                return Request.CreateResponse(HttpStatusCode.OK, errorResponse);
            }
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_userDashboard.UpdateProject(requestDto, UserID, UserName));
            });

        }

        [HttpDelete]
        [Route("DeleteProject")]
        public HttpResponseMessage DeleteProject(int projectID)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_userDashboard.DeleteProject(projectID, UserID));
            });
        }

        [HttpDelete]
        [Route("DeleteSharedProject")]
        public HttpResponseMessage DeleteSharedProject(int projectID)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_userDashboard.DeleteSharedProject(projectID, UserID, UserName));
            });
        }

        /// <summary>
        /// Saves shared project high level detail
        /// </summary>
        /// <param name="sharedProjectsDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("SaveSharedProject")]
        public async Task<HttpResponseMessage> SaveSharedProjectAsync(SharedProjectsDto sharedProjectsDto)
        {
            var clientURL = RequestContext.Url.Request.Headers.GetValues("Origin").FirstOrDefault();
            var urlBrand = "generac";
            if(clientURL.ToLower().Contains("pramac"))
            {
                urlBrand = "pramac";
            }
            if (String.IsNullOrEmpty(sharedProjectsDto.Language) || sharedProjectsDto.Language != "en" || sharedProjectsDto.Language != "en")
                sharedProjectsDto.Language = "en";

            var result = _userDashboard.SaveSharedProject(sharedProjectsDto, UserID, UserName);
            //var clientURL = RequestContext.Url.Request.Headers.GetValues("Origin").FirstOrDefault();
            var callbackURL = $"{clientURL}/project/{sharedProjectsDto.ProjectID}";

            var emaildata = new SendGridEmailData
            {
                template_id = SectionHandler.GetSectionValue($"{sharedProjectsDto.Language.ToLower()}/emailTemplates/projectShared", "TemplateID", ""),
                personalizations = new List<Personalization>()
                {
                    new Personalization
                    {
                        to = new List<To>()
                        {
                            new To
                            {
                                email = sharedProjectsDto.RecipientEmail,
                                name = sharedProjectsDto.RecipientEmail
                            }
                        },
                        substitutions = new Dictionary<string, string>()
                        {
                            { "%SharerLoginID%", UserName},
                            { "%ProjectName%", result.SharedProjectName },
                            { "%ProjectComments%", sharedProjectsDto.Notes },
                            { "%ProjectLink%", callbackURL },
                            { "%CompanyName%", EmailHelper.CompanyName(sharedProjectsDto.Language, urlBrand) },
                            { "%CompanyAddress%", EmailHelper.CompanyAddress(sharedProjectsDto.Language, urlBrand) }
                        }
                    }
                }
            };

            var emailResponse = EmailHelper.SendGridAsyncWithTemplate(emaildata);

            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(result);
            });
        }

        [HttpGet]
        [Route("GetSharedProjects")]
        public HttpResponseMessage GetSharedProjects()
        {
            return CreateHttpResponse(() =>
            {
                var requestDto = new SearchShareProjectRequestDto
                {
                    RecipientEmail = UserName
                };

                return Request.CreateResponse(_userDashboard.GetSharedProjects(requestDto));
            });
        }

        [HttpGet]
        [Route("GetSolutionListForSharedProject")]
        public HttpResponseMessage GetSolutionListForSharedProject(int projectID)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_userDashboard.GetSolutionListForSharedProject(projectID));
            });
        }

        [HttpGet]
        [Route("GetSearchFilter")]
        public HttpResponseMessage GetSearchFilter()
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_pickListProcessor.GetSearchFilter());
            });
        }
    }
}
