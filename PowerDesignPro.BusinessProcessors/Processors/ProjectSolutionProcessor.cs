using PowerDesignPro.BusinessProcessors.Interface;
using PowerDesignPro.BusinessProcessors.Dtos;
using PowerDesignPro.Data.Framework.Interface;
using PowerDesignPro.Data.Models;
using PowerDesignPro.Common.Mapper;
using System.Linq;
using System;
using PowerDesignPro.Common.CustomException;
using PowerDesignPro.Common.Constant;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Processors
{
    /// <summary>
    /// Processor class to handle all the required operations on Project Solution Entity.
    /// </summary>
    /// <seealso cref="PowerDesignPro.BusinessProcessors.Interface.IProjectSolution" />
    public class ProjectSolutionProcessor : IProjectSolution
    {
        private readonly IEntityBaseRepository<Project> _projectRepository;
        /// <summary>
        /// The solution repository
        /// </summary>
        private readonly IEntityBaseRepository<Solution> _solutionRepository;
        /// <summary>
        /// The solution setup repository
        /// </summary>
        private readonly IEntityBaseRepository<SolutionSetup> _solutionSetupRepository;
        /// <summary>
        /// The user default solution setup repository
        /// </summary>
        private readonly IEntityBaseRepository<UserDefaultSolutionSetup> _userDefaultSolutionSetupRepository;
        /// <summary>
        /// Shared Project Repository
        /// </summary>
        private readonly IEntityBaseRepository<SharedProject> _sharedProjectRepository;
        /// <summary>
        /// The base solution entity to project solution dto mapper
        /// </summary>
        private readonly IMapper<BaseSolutionSetupEntity, ProjectSolutionDto> _baseSolutionEntityToProjectSolutionDtoMapper;
        /// <summary>
        /// The user default solution setup to base solution setup mapping values dto mapper
        /// </summary>
        private readonly IMapper<UserDefaultSolutionSetup, BaseSolutionSetupMappingValuesDto> _userDefaultSolutionSetupToBaseSolutionSetupMappingValuesDtoMapper;
        /// <summary>
        /// The user default solution setup to global default solution setup dto mapper
        /// </summary>
        private readonly IMapper<UserDefaultSolutionSetup, GlobalDefaultSolutionSetupDto> _userDefaultSolutionSetupToGlobalDefaultSolutionSetupDtoMapper;
        /// <summary>
        /// The solution setup to project solution setup response dto mapper
        /// </summary>
        private readonly IMapper<SolutionSetup, BaseSolutionSetupMappingValuesDto> _solutionSetupToProjectSolutionSetupResponseDtoMapper;
        /// <summary>
        /// The solution setup request dto to solution setup entity mapper
        /// </summary>
        private readonly IMapper<BaseSolutionSetupMappingValuesDto, SolutionSetup> _solutionSetupRequestDtoToSolutionSetupEntityMapper;
        /// <summary>
        /// The solution request dto to solution entity mapper
        /// </summary>
        private readonly IMapper<ProjectSolutionDto, Solution> _solutionRequestDtoToSolutionEntityMapper;
        /// <summary>
        /// User Default Solution Setup Dto To Entity mapper
        /// </summary>
        private readonly IMapper<UserDefaultSolutionSetupDto, UserDefaultSolutionSetup> _userDefaultSolutionSetupDtoToUserDefaultSolutionSetupEntityMapper;


        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectSolutionProcessor"/> class.
        /// </summary>
        /// <param name="solutionRepository">The solution repository.</param>
        /// <param name="solutionSetupRepository">The solution setup repository.</param>
        /// <param name="userDefaultSolutionSetupRepository">The user default solution setup repository.</param>
        /// <param name="baseSolutionEntityToProjectSolutionDtoMapper">The base solution entity to project solution dto mapper.</param>
        /// <param name="userDefaultSolutionSetupToBaseSolutionSetupMappingValuesDtoMapper">The user default solution setup to base solution setup mapping values dto mapper.</param>
        /// <param name="userDefaultSolutionSetupToGlobalDefaultSolutionSetupDtoMapper">The user default solution setup to global default solution setup dto mapper.</param>
        /// <param name="solutionSetupToProjectSolutionSetupResponseDtoMapper">The solution setup to project solution setup response dto mapper.</param>
        /// <param name="solutionSetupRequestDtoToSolutionSetupEntityMapper">The solution setup request dto to solution setup entity mapper.</param>
        /// <param name="solutionRequestDtoToSolutionEntityMapper">The solution request dto to solution entity mapper.</param>
        public ProjectSolutionProcessor(
            IEntityBaseRepository<Solution> solutionRepository,
            IEntityBaseRepository<SolutionSetup> solutionSetupRepository,
            IEntityBaseRepository<UserDefaultSolutionSetup> userDefaultSolutionSetupRepository,
            IEntityBaseRepository<SharedProject> sharedProjectRepository,
            IMapper<BaseSolutionSetupEntity, ProjectSolutionDto> baseSolutionEntityToProjectSolutionDtoMapper,
            IMapper<UserDefaultSolutionSetup, BaseSolutionSetupMappingValuesDto> userDefaultSolutionSetupToBaseSolutionSetupMappingValuesDtoMapper,
            IMapper<UserDefaultSolutionSetup, GlobalDefaultSolutionSetupDto> userDefaultSolutionSetupToGlobalDefaultSolutionSetupDtoMapper,
            IMapper<SolutionSetup, BaseSolutionSetupMappingValuesDto> solutionSetupToProjectSolutionSetupResponseDtoMapper,
            IMapper<BaseSolutionSetupMappingValuesDto, SolutionSetup> solutionSetupRequestDtoToSolutionSetupEntityMapper,
            IMapper<ProjectSolutionDto, Solution> solutionRequestDtoToSolutionEntityMapper,
            IMapper<UserDefaultSolutionSetupDto, UserDefaultSolutionSetup> userDefaultSolutionSetupDtoToUserDefaultSolutionSetupEntityMapper,
            IEntityBaseRepository<Project> projectRepository)
        {
            _solutionRepository = solutionRepository;
            _solutionSetupRepository = solutionSetupRepository;
            _userDefaultSolutionSetupRepository = userDefaultSolutionSetupRepository;
            _sharedProjectRepository = sharedProjectRepository;
            _baseSolutionEntityToProjectSolutionDtoMapper = baseSolutionEntityToProjectSolutionDtoMapper;
            _userDefaultSolutionSetupToBaseSolutionSetupMappingValuesDtoMapper = userDefaultSolutionSetupToBaseSolutionSetupMappingValuesDtoMapper;
            _userDefaultSolutionSetupToGlobalDefaultSolutionSetupDtoMapper = userDefaultSolutionSetupToGlobalDefaultSolutionSetupDtoMapper;
            _solutionSetupToProjectSolutionSetupResponseDtoMapper = solutionSetupToProjectSolutionSetupResponseDtoMapper;
            _solutionSetupRequestDtoToSolutionSetupEntityMapper = solutionSetupRequestDtoToSolutionSetupEntityMapper;
            _solutionRequestDtoToSolutionEntityMapper = solutionRequestDtoToSolutionEntityMapper;
            _userDefaultSolutionSetupDtoToUserDefaultSolutionSetupEntityMapper = userDefaultSolutionSetupDtoToUserDefaultSolutionSetupEntityMapper;
            _projectRepository = projectRepository;
        }

        /// <summary>
        /// Gets header level details for the solution
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="projectID"></param>
        /// <param name="solutionID"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public SolutionHeaderDetailDto GetSolutionHeaderDetails(string userID, int projectID, int solutionID, string userName)
        {
            var projectDetail = _projectRepository.GetSingle(p => p.Active && p.UserID == userID && p.ID == projectID);

            if (projectDetail == null)
            {
                return SharedProjectSolutionDetail(projectID, solutionID, userName);
                throw new PowerDesignProException("ProjectNotFound", Message.ProjectDashboard);
            }

            var projectSolutionDetail = projectDetail.Solutions.Where(x => x.Active && x.ID == solutionID)
                .Select(solution =>
                {
                    var solutionDetail = new SolutionHeaderDetailDto
                    {
                        ProjectID = projectID,
                        SolutionID = solution.ID,
                        SolutionName = solution.SolutionName,
                        IsReadOnlyAccess = false,
                        ShowGrantAccess = !solution.CreatedBy.Equals(solution.Project.CreatedBy, StringComparison.InvariantCultureIgnoreCase)
                    };
                    return solutionDetail;
                }).FirstOrDefault();

            if (projectSolutionDetail == null)
            {
                throw new PowerDesignProException("SolutionNotFound", Message.ProjectSolution);
            }

            return projectSolutionDetail;

            throw new PowerDesignProException("ProjectNotFound", Message.ProjectDashboard);
        }

        /// <summary>
        /// Get default solution setup when user is creating a new solution
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="projectID"></param>
        /// <returns></returns>
        public BaseSolutionSetupDto LoadDefaultSolutionSetupForNewSolution(string userID, int projectID)
        {
            var userDefaultSettings = _userDefaultSolutionSetupRepository.GetSingle(x => x.UserID == userID);
            var defaultSolutionSetupMappingForNewSolution = _userDefaultSolutionSetupToBaseSolutionSetupMappingValuesDtoMapper.AddMap(userDefaultSettings);
            if (!string.IsNullOrEmpty(userDefaultSettings.RegulatoryFilter))
            {
                defaultSolutionSetupMappingForNewSolution.SelectedRegulatoryFilterList = userDefaultSettings
                   .RegulatoryFilter.Split(';').Select(x => new RegulatoryFilterDto
                   {
                       Id = Convert.ToInt32(x.Split(':')[0]),
                       ItemName = x.Split(':').Length > 1 ? x.Split(':')[1] : "",
                       LanguageKey= x.Split(':').Length > 2 ? x.Split(':')[2] : string.Concat("tregulatoryFilterList.", x.Split(':')[1].Replace(" ", ""))
                   });
            }

            return new BaseSolutionSetupDto
            {
                SolutionSetupMappingValuesDto = defaultSolutionSetupMappingForNewSolution
            };
        }

        /// <summary>
        /// Get user default solution setup
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public BaseSolutionSetupDto LoadUserDefaultSolutionSetup(string userID)
        {
            var userDefaultSettings = _userDefaultSolutionSetupRepository.GetSingle(x => x.UserID == userID);
            var userDefaultSolutionSetupMappingValues = _userDefaultSolutionSetupToBaseSolutionSetupMappingValuesDtoMapper.AddMap(userDefaultSettings);

            if (!string.IsNullOrEmpty(userDefaultSettings.RegulatoryFilter))
            {
                userDefaultSolutionSetupMappingValues.SelectedRegulatoryFilterList = userDefaultSettings
                   .RegulatoryFilter.Split(';').Select(x => new RegulatoryFilterDto
                   {
                       Id = Convert.ToInt32(x.Split(':')[0]),
                       ItemName = x.Split(':').Length > 1 ? x.Split(':')[1] : "",
                       LanguageKey = x.Split(':').Length > 2 ? x.Split(':')[2] : string.Concat("tregulatoryFilterList.", x.Split(':')[1].Replace(" ", ""))
                   });
            }

            return new BaseSolutionSetupDto
            {
                SolutionSetupMappingValuesDto = userDefaultSolutionSetupMappingValues
            };
        }

        /// <summary>
        /// Global settings to override the user default settings
        /// </summary>
        /// <returns></returns>
        public GlobalDefaultSolutionSetupDto LoadGlobalDefaultSolutionSetup()
        {
            var globalDefaultSolutionSetupMappingValues = _userDefaultSolutionSetupToBaseSolutionSetupMappingValuesDtoMapper.AddMap(
                _userDefaultSolutionSetupRepository.GetSingle(x => x.IsGlobalDefaults));

            return new GlobalDefaultSolutionSetupDto
            {
                SolutionSetupMappingValuesDto = globalDefaultSolutionSetupMappingValues
            };
        }

        /// <summary>
        /// Get solution setup for existing solution
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="projectID"></param>
        /// <param name="solutionID"></param>
        /// <returns></returns>
        public ProjectSolutionDto LoadSolutionSetupForExistingSolution(string userID, int projectID, int solutionID, string userName = "")
        {
            var solution = _solutionRepository.GetSingle(x => x.ID == solutionID && x.ProjectID == projectID && x.Active);

            var readOnlyAccess = false;
            if (!string.IsNullOrEmpty(solution.OwnedBy))
            {
                if (!solution.OwnedBy.Equals(userName, StringComparison.InvariantCultureIgnoreCase))
                    readOnlyAccess = true;
                else
                    readOnlyAccess = false;
            }
            else
            {
                readOnlyAccess = true;
            }

            var response = new ProjectSolutionDto
            {
                ProjectID = solution.ProjectID,
                SolutionID = solution.ID,
                SolutionName = solution.SolutionName,
                SpecRefNumber = solution.SpecRefNumber,
                Description = solution.Description,
                IsReadOnlyAccess = readOnlyAccess,
                BaseSolutionSetupDto = new BaseSolutionSetupDto()
            };

            var solutionSetup = solution.SolutionSetup.FirstOrDefault();
            if (solutionSetup != null)
            {
                var solutionDetail = _solutionSetupToProjectSolutionSetupResponseDtoMapper.AddMap(solution.SolutionSetup.FirstOrDefault());
                response.BaseSolutionSetupDto = new BaseSolutionSetupDto { SolutionSetupMappingValuesDto = solutionDetail };

                if (!string.IsNullOrEmpty(solutionSetup.RegulatoryFilter))
                    response.BaseSolutionSetupDto.SolutionSetupMappingValuesDto.SelectedRegulatoryFilterList = solutionSetup
                    .RegulatoryFilter.Split(';').Select(x => new RegulatoryFilterDto
                    {
                        Id = Convert.ToInt32(x.Split(':')[0]),
                        ItemName = x.Split(':').Length > 1 ? x.Split(':')[1]:"",
                        LanguageKey = x.Split(':').Length >2 ? x.Split(':')[2]: string.Concat("tregulatoryFilterList.", x.Split(':')[1].Replace(" ", ""))
                    });
            }

            return response;
        }
        public Solution GetSolution(string userID, int projectID, int solutionID, string userName = "")
        {
          return _solutionRepository.GetSingle(x => x.ID == solutionID && x.ProjectID == projectID && x.Active);

        }
        /// <summary>
        /// Save solution details
        /// </summary>
        /// <param name="solutionSetupDto"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ProjectSolutionDto SaveSolutionDetail(ProjectSolutionDto projectSolutionResponseDto, string userID, string userName)
        {
            var solutions = _solutionRepository.GetAll(s => s.ProjectID == projectSolutionResponseDto.ProjectID && s.Active);

            if (projectSolutionResponseDto.SolutionID == 0)
            {
                return AddSolutionDetail(projectSolutionResponseDto, solutions, userName);
            }
            else
            {
                return UpdateSolutionDetail(projectSolutionResponseDto, solutions, userName);
            }
        }

        /// <summary>
        /// Save User Default Solution Setup details
        /// </summary>
        /// <param name="userDefaultSolutionSetupDto"></param>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public Boolean SaveUserDefaultSolutionSetup(UserDefaultSolutionSetupDto userDefaultSolutionSetupDto, string userID, string userName)
        {
            var userDefaultSetupCount = _userDefaultSolutionSetupRepository.GetAll(u => !u.IsGlobalDefaults && u.UserID == userID).Count();

            if (userDefaultSetupCount == 0)
            {
                var newUserDefaultSolutionSetup = _userDefaultSolutionSetupDtoToUserDefaultSolutionSetupEntityMapper.AddMap(userDefaultSolutionSetupDto);

                newUserDefaultSolutionSetup.UserID = userID;
                newUserDefaultSolutionSetup.CreatedDateTime = DateTime.UtcNow;
                newUserDefaultSolutionSetup.CreatedBy = userName;
                newUserDefaultSolutionSetup.ModifiedDateTime = DateTime.UtcNow;
                newUserDefaultSolutionSetup.ModifiedBy = userName;
                newUserDefaultSolutionSetup.RegulatoryFilter = string.Join(";",
                    userDefaultSolutionSetupDto.SelectedRegulatoryFilterList.Select(x => x.Id + ":" + x.ItemName + ":" + x.LanguageKey).ToArray());

                var newAddResult = _userDefaultSolutionSetupRepository.Add(newUserDefaultSolutionSetup);
                _userDefaultSolutionSetupRepository.Commit();

                return true;
            }

            var userDefaultSolutionSetup = _userDefaultSolutionSetupRepository.GetSingle(u => !u.IsGlobalDefaults && u.UserID == userID);

            _userDefaultSolutionSetupDtoToUserDefaultSolutionSetupEntityMapper.UpdateMap(userDefaultSolutionSetupDto, userDefaultSolutionSetup);

            userDefaultSolutionSetup.ModifiedDateTime = DateTime.UtcNow;
            userDefaultSolutionSetup.ModifiedBy = userName;
            userDefaultSolutionSetup.RegulatoryFilter = string.Join(";",
                    userDefaultSolutionSetupDto.SelectedRegulatoryFilterList.Select(x => x.Id + ":" + x.ItemName + ":" + x.LanguageKey).ToArray());

            var result = _userDefaultSolutionSetupRepository.Update(userDefaultSolutionSetup);
            _userDefaultSolutionSetupRepository.Commit();

            return true;
        }

        public int CheckUserDefaultSetup(string UserID)
        {
            return _userDefaultSolutionSetupRepository.GetAll(x => x.UserID == UserID && !x.IsGlobalDefaults).Count();
        }

        public Dictionary<string, int> CopySolution(SolutionRequestDto solutionRequestDto, string userID, string userName)
        {
            var solution = new Solution();
            var sharedSolution = _solutionRepository.GetSingle(x => x.ID == solutionRequestDto.SolutionID && x.ProjectID == solutionRequestDto.ProjectID);
            if (sharedSolution != null)
            {
                var dateTime = DateTime.UtcNow;
                MapSolution(userName, solution, sharedSolution, dateTime);

                var result = _solutionRepository.Add(solution);
                _solutionRepository.Commit();

                return new Dictionary<string, int>
                {
                    { "ProjectId",result.ProjectID},
                    { "SolutionId",result.ID}
                };
            }

            throw new PowerDesignProException("", "");
        }

        public GrantEditAccessResponseDto GrantEditAccess(SolutionRequestDto solutionRequestDto, string userID, string userName)
        {
            var project = _projectRepository.GetSingle(p => p.ID == solutionRequestDto.ProjectID);
            var solution = _solutionRepository.GetSingle(x => x.ID == solutionRequestDto.SolutionID
                                                            && x.ProjectID == solutionRequestDto.ProjectID
                                                            && userName.Equals(x.OwnedBy, StringComparison.InvariantCultureIgnoreCase));
            if (solution.OwnedBy.Equals(solution.CreatedBy, StringComparison.InvariantCultureIgnoreCase))
            {
                solution.OwnedBy = solution.Project.CreatedBy;
                solution.ModifiedBy = userName;
                solution.ModifiedDateTime = DateTime.UtcNow;
                solution.EditAccessNotes = solutionRequestDto.Notes;
            }
            else
            {
                if (solution.OwnedBy.Equals(solution.Project.CreatedBy, StringComparison.InvariantCultureIgnoreCase))
                {
                    solution.OwnedBy = solution.CreatedBy;
                    solution.ModifiedBy = userName;
                    solution.ModifiedDateTime = DateTime.UtcNow;
                    solution.EditAccessNotes = solutionRequestDto.Notes;
                }
            }

            var result = _solutionRepository.Update(solution);
            _solutionRepository.Commit();

            //return new Dictionary<string, string>
            //    {
            //        { "ProjectId",Convert.ToString(result.ProjectID)},
            //        { "SolutionId",Convert.ToString(result.ID)},
            //        { "Email",result.OwnedBy}
            //    };

            return new GrantEditAccessResponseDto
            {
                ProjectID = result.ProjectID,
                SolutionID = result.ID,
                ProjectName = project.ProjectName,
                SolutionName = solution.SolutionName,
                SolutionComments = solution.EditAccessNotes,
                RecipientEmail = result.OwnedBy
            };
        }


        #region Private Methods

        private void MapSolution(string userName, Solution solution, Solution sharedSolution, DateTime dateTime)
        {
            IMapper<SolutionSetup, SolutionSetup> _solutionSetupEntityMapper = new AutoMapper<SolutionSetup, SolutionSetup>();
            IMapper<GasPipingSolution, GasPipingSolution> _gasPipingSolutionEntityMapper = new AutoMapper<GasPipingSolution, GasPipingSolution>();
            IMapper<ExhaustPipingSolution, ExhaustPipingSolution> _exhaustPipingSolutionEntityMapper = new AutoMapper<ExhaustPipingSolution, ExhaustPipingSolution>();
            IMapper<RecommendedProduct, RecommendedProduct> _recommendedProductEntityMapper = new AutoMapper<RecommendedProduct, RecommendedProduct>();

            var solutionName = $"{sharedSolution.SolutionName} shared v1";
            var solutionNameExist = true;

            while (solutionNameExist)
            {
                solutionNameExist = _solutionRepository.GetAll(x => solutionName.Equals(x.SolutionName, StringComparison.InvariantCultureIgnoreCase)).Any();
                if (solutionNameExist)
                {
                    var updatedSolutionName = solutionName.Substring(0, solutionName.Length - 1);
                    var solutionVersion = solutionName[solutionName.Length - 1];
                    solutionName = $"{updatedSolutionName}{Convert.ToInt32(solutionVersion.ToString()) + 1}";
                }
            }

            solution.ID = 0;
            solution.ProjectID = sharedSolution.ProjectID;
            solution.SolutionName = solutionName;
            solution.CreatedBy = userName;
            solution.ModifiedBy = userName;
            solution.OwnedBy = userName;
            solution.CreatedDateTime = dateTime;
            solution.ModifiedDateTime = dateTime;
            solution.Active = true;
            solution.Description = sharedSolution.Description;
            solution.SpecRefNumber = sharedSolution.SpecRefNumber;

            if (sharedSolution.SolutionSetup.FirstOrDefault() != null)
            {
                solution.SolutionSetup = new List<SolutionSetup> { _solutionSetupEntityMapper.AddMap(sharedSolution.SolutionSetup.FirstOrDefault()) };
                solution.SolutionSetup.FirstOrDefault().Solution = null;
                solution.SolutionSetup.FirstOrDefault().ID = 0;
            }

            MapSolutionLoadEntities(userName, solution, sharedSolution, dateTime);

            solution.SharedProjectSolution = null;

            var gaspiping = solution.GasPipingSolutions.FirstOrDefault();
            if (gaspiping != null)
            {
                gaspiping.SolutionID = 0;
                solution.GasPipingSolutions = new List<GasPipingSolution> { gaspiping };
            }

            var exhaustpiping = solution.ExhaustPipingSolutions.FirstOrDefault();
            if (exhaustpiping != null)
            {
                exhaustpiping.SolutionID = 0;
                solution.ExhaustPipingSolutions = new List<ExhaustPipingSolution> { exhaustpiping };
            }

            var recommendedProduct = sharedSolution.RecommendedProduct.FirstOrDefault();
            if (recommendedProduct != null)
            {
                solution.RecommendedProduct = new List<RecommendedProduct> { _recommendedProductEntityMapper.AddMap(sharedSolution.RecommendedProduct.FirstOrDefault())};
                solution.RecommendedProduct.FirstOrDefault().Solution = null;
                solution.RecommendedProduct.FirstOrDefault().ID = 0;
            }
        }

        private static void MapSolutionLoadEntities(string userName, Solution solution, Solution sharedSolution, DateTime dateTime)
        {
            IMapper<BasicLoad, BasicLoad> _basicLoadEntityMapper = new AutoMapper<BasicLoad, BasicLoad>();
            IMapper<ACLoad, ACLoad> _acLoadEntityMapper = new AutoMapper<ACLoad, ACLoad>();
            IMapper<UPSLoad, UPSLoad> _upsLoadEntityMapper = new AutoMapper<UPSLoad, UPSLoad>();
            IMapper<LightingLoad, LightingLoad> _lightingLoadEntityMapper = new AutoMapper<LightingLoad, LightingLoad>();
            IMapper<MotorLoad, MotorLoad> _motorLoadEntityMapper = new AutoMapper<MotorLoad, MotorLoad>();
            IMapper<WelderLoad, WelderLoad> _welderLoadEntityMapper = new AutoMapper<WelderLoad, WelderLoad>();

            Parallel.ForEach(sharedSolution.BasicLoadList, (load) =>
            {
                var loadEntity = _basicLoadEntityMapper.AddMap(load);
                loadEntity.SolutionID = 0;
                loadEntity.ID = 0;
                loadEntity.CreatedBy = userName;
                loadEntity.ModifiedBy = userName;
                loadEntity.CreatedDateTime = dateTime;
                loadEntity.ModifiedDateTime = dateTime;
                loadEntity.Solution = null;
                solution.BasicLoadList.Add(loadEntity);
            });

            Parallel.ForEach(sharedSolution.ACLoadList, (load) =>
            {
                var loadEntity = _acLoadEntityMapper.AddMap(load);
                loadEntity.SolutionID = 0;
                loadEntity.ID = 0;
                loadEntity.CreatedBy = userName;
                loadEntity.ModifiedBy = userName;
                loadEntity.CreatedDateTime = dateTime;
                loadEntity.ModifiedDateTime = dateTime;
                loadEntity.Solution = null;
                solution.ACLoadList.Add(loadEntity);
            });

            Parallel.ForEach(sharedSolution.LightingLoadList, (load) =>
            {
                var loadEntity = _lightingLoadEntityMapper.AddMap(load);
                loadEntity.SolutionID = 0;
                loadEntity.ID = 0;
                loadEntity.CreatedBy = userName;
                loadEntity.ModifiedBy = userName;
                loadEntity.CreatedDateTime = dateTime;
                loadEntity.ModifiedDateTime = dateTime;
                loadEntity.Solution = null;
                solution.LightingLoadList.Add(loadEntity);
            });

            Parallel.ForEach(sharedSolution.UPSLoadList, (load) =>
            {
                var loadEntity = _upsLoadEntityMapper.AddMap(load);
                loadEntity.SolutionID = 0;
                loadEntity.ID = 0;
                loadEntity.CreatedBy = userName;
                loadEntity.ModifiedBy = userName;
                loadEntity.CreatedDateTime = dateTime;
                loadEntity.ModifiedDateTime = dateTime;
                loadEntity.Solution = null;
                solution.UPSLoadList.Add(loadEntity);
            });

            Parallel.ForEach(sharedSolution.WelderLoadList, (load) =>
            {
                var loadEntity = _welderLoadEntityMapper.AddMap(load);
                loadEntity.SolutionID = 0;
                loadEntity.ID = 0;
                loadEntity.CreatedBy = userName;
                loadEntity.ModifiedBy = userName;
                loadEntity.CreatedDateTime = dateTime;
                loadEntity.ModifiedDateTime = dateTime;
                loadEntity.Solution = null;
                solution.WelderLoadList.Add(loadEntity);
            });

            Parallel.ForEach(sharedSolution.MotorLoadList, (load) =>
            {
                var loadEntity = _motorLoadEntityMapper.AddMap(load);
                loadEntity.SolutionID = 0;
                loadEntity.ID = 0;
                loadEntity.CreatedBy = userName;
                loadEntity.ModifiedBy = userName;
                loadEntity.CreatedDateTime = dateTime;
                loadEntity.ModifiedDateTime = dateTime;
                loadEntity.Solution = null;
                solution.MotorLoadList.Add(loadEntity);
            });
        }

        private SolutionHeaderDetailDto SharedProjectSolutionDetail(int projectID, int solutionID, string userName)
        {
            var sharedProject = _sharedProjectRepository.GetSingle(sp => sp.ProjectID == projectID && sp.RecipientEmail.Equals(userName));
            var project = _projectRepository.GetSingle(p => p.Active && p.ID == projectID);

            if (sharedProject == null)
            {
                throw new PowerDesignProException("ProjectNotFound", Message.ProjectDashboard);
            }

            var sharedProjectSolution = project.Solutions.Where(x => x.Active && x.ID == solutionID)
                    .Select(solution =>
                    {
                        var readOnlyAccess = false;

                        if (!string.IsNullOrEmpty(solution.OwnedBy))
                        {
                            if (!solution.OwnedBy.Equals(userName, StringComparison.InvariantCultureIgnoreCase))
                                readOnlyAccess = true;
                            else
                                readOnlyAccess = false;
                        }
                        else
                        {
                            readOnlyAccess = true;
                        }

                        var solutionDetail = new SolutionHeaderDetailDto
                        {
                            ProjectID = projectID,
                            SolutionID = solution.ID,
                            SolutionName = solution.SolutionName,
                            IsReadOnlyAccess = readOnlyAccess,
                            ShowGrantAccess = !readOnlyAccess && !solution.CreatedBy.Equals(solution.Project.CreatedBy, StringComparison.InvariantCultureIgnoreCase)
                        };

                        return solutionDetail;

                    }).FirstOrDefault();

            if (sharedProjectSolution == null)
            {
                throw new PowerDesignProException("SolutionNotFound", Message.ProjectSolution);
            }

            return sharedProjectSolution;
        }

        /// <summary>
        /// Adds the solution detail.
        /// </summary>
        /// <param name="solutionRequestDto">The solution request dto.</param>
        /// <param name="solutions">The solutions.</param>
        /// <returns></returns>
        /// <exception cref="PowerDesignProException">SolutionNameExist</exception>
        private ProjectSolutionDto AddSolutionDetail(ProjectSolutionDto solutionRequestDto, IQueryable<Solution> solutions, string userName)
        {
            var solutionCount = solutions.Count(s => s.SolutionName.Equals(solutionRequestDto.SolutionName, StringComparison.InvariantCultureIgnoreCase) && s.Active);
            if (solutionCount > 0)
            {
                throw new PowerDesignProException("SolutionNameExist", Message.ProjectSolution);
            }

            // mapping Solution child Entity (SolutionSetup)
            var solutionSetupEntity = _solutionSetupRequestDtoToSolutionSetupEntityMapper.AddMap(solutionRequestDto.BaseSolutionSetupDto.SolutionSetupMappingValuesDto);
            solutionSetupEntity.RegulatoryFilter = string.Join(";",
                    solutionRequestDto.BaseSolutionSetupDto.SolutionSetupMappingValuesDto.SelectedRegulatoryFilterList.Select(x => x.Id + ":" + x.ItemName + ":" + x.LanguageKey).ToArray());

            // mapping solution entity
            var solutionEntity = _solutionRequestDtoToSolutionEntityMapper.AddMap(solutionRequestDto, null, userName);

            solutionEntity.SolutionSetup.Add(solutionSetupEntity);

            // update Project when add solution
            var Project = _projectRepository.Find(solutionRequestDto.ProjectID);
            Project.ModifiedBy = solutionEntity.ModifiedBy;
            Project.ModifiedDateTime = solutionEntity.ModifiedDateTime;

            var result = _solutionRepository.Add(solutionEntity);
            var updatedProject = _projectRepository.Update(Project);

            _solutionRepository.Commit();
            _projectRepository.Commit();

            return new ProjectSolutionDto
            {
                SolutionID = result.ID
            };
        }

        /// <summary>
        /// Updates the solution detail.
        /// </summary>
        /// <param name="solutionRequestDto">The solution request dto.</param>
        /// <param name="solutions">The solutions.</param>
        /// <returns></returns>
        /// <exception cref="PowerDesignProException">SolutionNameExist</exception>
        private ProjectSolutionDto UpdateSolutionDetail(ProjectSolutionDto solutionRequestDto, IQueryable<Solution> solutions, string userName)
        {
            var solutionCount = solutions.Count(
                s => s.ID != solutionRequestDto.SolutionID
                && s.SolutionName.Equals(solutionRequestDto.SolutionName, StringComparison.InvariantCultureIgnoreCase) && s.Active);

            if (solutionCount > 0)
            {
                throw new PowerDesignProException("SolutionNameExist", Message.ProjectSolution);
            }

            var solution = _solutionRepository.Find(solutionRequestDto.SolutionID);

            // mapping solution entity
            _solutionRequestDtoToSolutionEntityMapper.UpdateMap(solutionRequestDto, solution, null, userName);

            // mapping Solution child Entity (SolutionSetup)
            _solutionSetupRequestDtoToSolutionSetupEntityMapper.UpdateMap(solutionRequestDto.BaseSolutionSetupDto.SolutionSetupMappingValuesDto, solution.SolutionSetup.FirstOrDefault());

            solution.SolutionSetup.FirstOrDefault().RegulatoryFilter = string.Join(";",
                    solutionRequestDto.BaseSolutionSetupDto.SolutionSetupMappingValuesDto.SelectedRegulatoryFilterList.Select(x => x.Id + ":" + x.ItemName + ":" + x.LanguageKey).ToArray());
            solution.Project.ModifiedBy = solution.ModifiedBy;
            solution.Project.ModifiedDateTime = solution.ModifiedDateTime;

            var result = _solutionRepository.Update(solution);
            _solutionRepository.Commit();

            return new ProjectSolutionDto
            {
                SolutionID = result.ID
            };
        }

        #endregion

    }
}