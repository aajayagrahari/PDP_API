using PowerDesignPro.BusinessProcessors.Dtos;
using PowerDesignPro.BusinessProcessors.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PowerDesignProAPI.Controllers
{
    [Authorize]
    [RoutePrefix("User/Project")]
    public class ProjectController : BaseController
    {
        private IProject _project;

        public ProjectController(IProject project)
        {
            _project = project;
        }

        [HttpGet]
        [Route("GetProjectDetail")]
        public HttpResponseMessage GetProjectDetail(int ProjectID)
        {
            return CreateHttpResponse(() =>
            {
                var requestDto = new SearchProjectRequestDto
                {
                    UserID = UserID,
                    ID = ProjectID
                };

                return Request.CreateResponse(_project.GetProjectDetail(requestDto, UserName));
            });
        }

        [HttpGet]
        [Route("GetSolutionListByProjectID")]
        public HttpResponseMessage GetSolutionListByProjectID(int ProjectID)
        {
            return CreateHttpResponse(() =>
            {
                var requestDto = new SearchProjectRequestDto
                {
                    UserID = UserID,
                    ID = ProjectID
                };

                return Request.CreateResponse(_project.GetSolutionHeaderDetailsInProject(requestDto, UserName));
            });
        }

        [HttpDelete]
        [Route("DeleteSolution")]
        public HttpResponseMessage DeleteSolution(int projectID, int solutionID)
        {
            return CreateHttpResponse(() =>
            {
                return Request.CreateResponse(_project.DeleteSolution(projectID, solutionID, UserName));
            });
        }

    } 
}
