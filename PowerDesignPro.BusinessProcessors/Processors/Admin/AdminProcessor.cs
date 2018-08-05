using PowerDesignPro.BusinessProcessors.Dtos;
using PowerDesignPro.BusinessProcessors.Dtos.Admin;
using PowerDesignPro.BusinessProcessors.Interface;
using PowerDesignPro.Common.Constant;
using PowerDesignPro.Common.CustomException;
using PowerDesignPro.Common.Mapper;
using PowerDesignPro.Data.Framework.Interface;
using PowerDesignPro.Data.Models;
using PowerDesignPro.Data.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.BusinessProcessors.Processors
{
    public class AdminProcessor : IAdmin
    {
        private readonly IEntityBaseRepository<Generator> _generatorRepository;

        private readonly IEntityBaseRepository<GeneratorAvailableVoltage> _generatorAvailableVoltageRepository;

        private readonly IEntityBaseRepository<GeneratorRegulatoryFilter> _generatorRegulatoryFilterRepository;

        private readonly IEntityBaseRepository<GeneratorAvailableAlternator> _generatorAvailableAlternatorRepository;

        private readonly IEntityBaseRepository<AlternatorFamily> _alternatorFamilyRepository;

        private readonly IMapper<Generator, GeneratorDto> _generatorEntityToGeneratorDtoMapper;

        private readonly IMapper<GeneratorDto, Generator> _addGeneratorDtoToEntityMapper;

        private readonly IEntityBaseRepository<Alternator> _alternatorRepository;

        private readonly IMapper<Alternator, AlternatorDto> _alternatorEntityToAlternatorDtoMapper;

        private readonly IMapper<AlternatorDto, Alternator> _addAlternatorDtoToEntityMapper;

        private readonly IEntityBaseRepository<ProductFamily> _productFamilyRepository;

        private readonly IMapper<ImportGensetDataRequestDto, Generator> _addImportGensetDataRequestDtoToGeneratorEntityMapper;

        private readonly IEntityBaseRepository<RegulatoryFilter> _regulatoryFilterRepository;

        private readonly IEntityBaseRepository<Frequency> _frequencyRepository;

        private readonly IEntityBaseRepository<FuelType> _fuelTypeRepository;

        private readonly IMapper<ImportAlternatorDataRequestDto, Alternator> _addImportAlternatorDataRequestDtoToAlternatorEntityMapper;

        private readonly IEntityBaseRepository<VoltagePhase> _voltagePhaseRepository;

        private readonly IEntityBaseRepository<VoltageNominal> _voltageNominalRepository;

        private readonly IEntityBaseRepository<Brand> _brandRepository;

        private readonly IEntityBaseRepository<SolutionApplication> _solutionApplicationRepository;

        private readonly IEntityBaseRepository<Documentation> _documentationRepository;

        private readonly IEntityBaseRepository<HarmonicDeviceType> _harmonicDeviceTypeRepository;

        private readonly IMapper<HarmonicDeviceType, HarmonicDeviceTypeDto> _harmonicDeviceTypeEntityToHarmonicDeviceTypeDtoMapper;

        private readonly IMapper<HarmonicDeviceTypeDto, HarmonicDeviceType> _addHarmonicDeviceDtoToEntityMapper;
        public AdminProcessor(
           IEntityBaseRepository<Generator> generatorRepository,
           IMapper<Generator, GeneratorDto> generatorEntityToGeneratorDtoMapper,
           IEntityBaseRepository<Alternator> alternatorRepository,
           IEntityBaseRepository<AlternatorFamily> alternatorFamilyRepository,
           IMapper<Alternator, AlternatorDto> alternatorEntityToAlternatorDtoMapper,
           IMapper<AlternatorDto, Alternator> addAlternatorDtoToEntityMapper,
           IMapper<GeneratorDto, Generator> addGeneratorDtoToEntityMapper,
           IEntityBaseRepository<GeneratorAvailableVoltage> generatorAvailableVoltageRepository,
           IEntityBaseRepository<GeneratorRegulatoryFilter> generatorRegulatoryFilterRepository,
           IEntityBaseRepository<ProductFamily> productFamilyRepository,
           IMapper<ImportGensetDataRequestDto, Generator> addImportGensetDataRequestDtoToGeneratorEntityMapper,
           IEntityBaseRepository<GeneratorAvailableAlternator> generatorAvailableAlternatorRepository,
           IEntityBaseRepository<RegulatoryFilter> regulatoryFilterRepository,
           IEntityBaseRepository<Frequency> frequencyRepository,
           IEntityBaseRepository<FuelType> fuelTypeRepository,
           IMapper<ImportAlternatorDataRequestDto, Alternator> addImportAlternatorDataRequestDtoToAlternatorEntityMapper,
           IEntityBaseRepository<VoltagePhase> voltagePhaseRepository,
           IEntityBaseRepository<VoltageNominal> voltageNominalRepository,
           IEntityBaseRepository<Brand> brandRepository,
           IEntityBaseRepository<SolutionApplication> solutionApplicationRepository,
           IEntityBaseRepository<Documentation> documentationRepository,
           IEntityBaseRepository<HarmonicDeviceType> harmonicDeviceTypeRepository,
           IMapper<HarmonicDeviceType, HarmonicDeviceTypeDto> harmonicDeviceTypeEntityToHarmonicDeviceTypeDtoMapper,
           IMapper<HarmonicDeviceTypeDto, HarmonicDeviceType> addHarmonicDeviceDtoToEntityMapper
           )
        {
            _generatorRepository = generatorRepository;
            _generatorEntityToGeneratorDtoMapper = generatorEntityToGeneratorDtoMapper;
            _alternatorRepository = alternatorRepository;
            _alternatorFamilyRepository = alternatorFamilyRepository;
            _alternatorEntityToAlternatorDtoMapper = alternatorEntityToAlternatorDtoMapper;
            _addAlternatorDtoToEntityMapper = addAlternatorDtoToEntityMapper;
            _addGeneratorDtoToEntityMapper = addGeneratorDtoToEntityMapper;
            _generatorAvailableVoltageRepository = generatorAvailableVoltageRepository;
            _generatorRegulatoryFilterRepository = generatorRegulatoryFilterRepository;
            _productFamilyRepository = productFamilyRepository;
            _addImportGensetDataRequestDtoToGeneratorEntityMapper = addImportGensetDataRequestDtoToGeneratorEntityMapper;
            _generatorAvailableAlternatorRepository = generatorAvailableAlternatorRepository;
            _regulatoryFilterRepository = regulatoryFilterRepository;
            _frequencyRepository = frequencyRepository;
            _fuelTypeRepository = fuelTypeRepository;
            _addImportAlternatorDataRequestDtoToAlternatorEntityMapper = addImportAlternatorDataRequestDtoToAlternatorEntityMapper;
            _voltagePhaseRepository = voltagePhaseRepository;
            _voltageNominalRepository = voltageNominalRepository;
            _brandRepository = brandRepository;
            _solutionApplicationRepository = solutionApplicationRepository;
            _documentationRepository = documentationRepository;
            _harmonicDeviceTypeRepository = harmonicDeviceTypeRepository;
            _harmonicDeviceTypeEntityToHarmonicDeviceTypeDtoMapper = harmonicDeviceTypeEntityToHarmonicDeviceTypeDtoMapper;
            _addHarmonicDeviceDtoToEntityMapper = addHarmonicDeviceDtoToEntityMapper;
        }

        #region Generator
        public IEnumerable<GeneratorDto> GetGeneratorDetails()
        {
            return _generatorRepository.AllIncluding(x => x.ProductFamily).Where(x => x.Active).ToList().
                Select(x => new GeneratorDto
                {
                    ID = x.ID,
                    ModelDescription = x.ModelDescription,
                    Liters = x.Liters,
                    Description = x.ProductFamily.Description,
                    ProductFamilyID = x.ProductFamilyID,
                    KwStandby=x.KwStandby,
                });
        }

        public IEnumerable<GeneratorDto> GetGenerator(int productFamilyId)
        {
            return _generatorRepository.GetAll(x => x.ProductFamilyID == productFamilyId && x.Active).ToList().
                Select(x => new GeneratorDto
                {
                    ID = x.ID,
                    ModelDescription = x.ModelDescription,
                    Liters = x.Liters,
                    Description = x.ProductFamily.Description,
                    KwStandby = x.KwStandby,
                });
        }

        public GeneratorDto GetGenerator(SearchAdminRequestDto searchDto)
        {
            if (searchDto.ID > 0)
            {
                var generatorDetail = _generatorRepository.GetSingle(p => p.ID == searchDto.ID && p.Active);

                if (generatorDetail == null)
                {
                    throw new PowerDesignProException("GeneratorNotFound", Message.Admin);
                }

                var result = _generatorEntityToGeneratorDtoMapper.AddMap(generatorDetail);
                result.SelectedRegulatoryFilterList = generatorDetail.GeneratorRegulatoryFilters.Select(x => new PickListDto
                {
                    ID = x.RegulatoryFilterID,
                    Description = x.RegulatoryFilter.Description,
                     LanguageKey=x.RegulatoryFilter.LanguageKey
                });

                result.SelectedVoltageNominalList = generatorDetail.GeneratorAvailableVoltages.Select(x => new PickListDto
                {
                    ID = x.VoltageNominalID,
                    Description = x.VoltageNominal.Description
                });

                result.AlternatorFamilyTreeList = GetAlternatorFamilyTreeList();

                result.SelectedAlternatorList = generatorDetail.GeneratorAvailableAlternators.Select(x => new PickListDto
                {
                    ID = x.AlternatorID,
                    Description = x.Alternator.ModelDescription
                });

                return result;
            }
            else
            {
                return new GeneratorDto
                {
                    AlternatorFamilyTreeList = GetAlternatorFamilyTreeList()
                };
            }
        }

        public AdminResponseDto SaveGeneratorDetail(GeneratorDto generatorResponseDto, string userID)
        {
            if (generatorResponseDto.ID == 0)
            {
                return AddGeneratorDetail(generatorResponseDto, userID);
            }
            else
            {
                return UpdateGeneratorDetail(generatorResponseDto, userID);
            }
        }

        private AdminResponseDto AddGeneratorDetail(GeneratorDto generatorRequestDto, string userID)
        {
            var generator = _addGeneratorDtoToEntityMapper.AddMap(generatorRequestDto);
            generator.GeneratorAvailableVoltages = generatorRequestDto.SelectedVoltageNominalList.ToList().ConvertAll(x => ToAvailableVoltageEntity(x.ID, userID));
            generator.GeneratorRegulatoryFilters = generatorRequestDto.SelectedRegulatoryFilterList.ToList().ConvertAll(x => ToAvailableGeneratorRegulatoryEntity(x.ID, userID));
            generator.GeneratorAvailableAlternators = generatorRequestDto.SelectedAlternatorList.ToList().ConvertAll(x => ToAvailableGeneratorAlternatorEntity(x.ID, userID));

            generator.Active = true;
            generator.CreatedDateTime = DateTime.UtcNow;
            generator.CreatedBy = userID;
            generator.ModifiedDateTime = DateTime.UtcNow;
            generator.ModifiedBy = userID;

            var generatorDetail = _generatorRepository.Add(generator);
            _generatorRepository.Commit();

            return new AdminResponseDto
            {
                ID = generatorDetail.ID
            };
        }

        private AdminResponseDto UpdateGeneratorDetail(GeneratorDto generatorRequestDto, string userID)
        {
            var generator = _generatorRepository.Find(generatorRequestDto.ID);
            if (generator == null)
            {
                throw new PowerDesignProException("GeneratorNotFound", Message.Admin);
            }
            _addGeneratorDtoToEntityMapper.UpdateMap(generatorRequestDto, generator);

            foreach (var item in generator.GeneratorAvailableVoltages.ToList())
            {
                _generatorAvailableVoltageRepository.Delete(item);
            }
            foreach (var item in generator.GeneratorRegulatoryFilters.ToList())
            {
                _generatorRegulatoryFilterRepository.Delete(item);
            }
            foreach (var item in generator.GeneratorAvailableAlternators.ToList())
            {
                _generatorAvailableAlternatorRepository.Delete(item);
            }

            generator.GeneratorAvailableVoltages = generatorRequestDto.SelectedVoltageNominalList.ToList().ConvertAll(x => ToAvailableVoltageEntity(x.ID, userID));
            generator.GeneratorRegulatoryFilters = generatorRequestDto.SelectedRegulatoryFilterList.ToList().ConvertAll(x => ToAvailableGeneratorRegulatoryEntity(x.ID, userID));
            generator.GeneratorAvailableAlternators = generatorRequestDto.SelectedAlternatorList.ToList().ConvertAll(x => ToAvailableGeneratorAlternatorEntity(x.ID, userID));

            generator.Active = true;
            generator.ModifiedDateTime = DateTime.UtcNow;
            generator.ModifiedBy = userID;

            var generatorDetail = _generatorRepository.Update(generator);
            _generatorRepository.Commit();

            return new AdminResponseDto
            {
                ID = generatorDetail.ID
            };
        }

        public bool DeleteGenerator(int generatorID, string userName)
        {
            var generator = _generatorRepository.GetSingle(x => x.ID == generatorID);
            if (generator == null)
            {
                throw new PowerDesignProException("GeneratorNotFound", Message.Admin);
            }

            generator.Active = false;
            generator.ModifiedBy = userName;
            generator.ModifiedDateTime = DateTime.UtcNow;

            _generatorRepository.Update(generator);
            _generatorRepository.Commit();
            return true;
        }

        public bool ImportGensetData(IEnumerable<ImportGensetDataRequestDto> gensetData, string userName)
        {
            var regFilters = _regulatoryFilterRepository.GetAll().ToList();
            var frequencyList = _frequencyRepository.GetAll().ToList();
            var fuelTypeList = _fuelTypeRepository.GetAll().ToList();
            var errorMessage = string.Empty;
            int rowNum = 1;
            foreach (var item in gensetData)
            {
                var error = string.Empty;
                var generator = _addImportGensetDataRequestDtoToGeneratorEntityMapper.AddMap(item);
                var productFamily = _productFamilyRepository.GetSingle(p => p.Description.Contains(item.ProductFamily));
                if (productFamily == null)
                {
                    var errStr = $"<li> No ProductFamily detail exist for 'PRODUCT FAMILY' :  {item.ProductFamily} </li>";
                    error = GetError(rowNum, errStr, error);
                }
                else
                {
                    generator.ProductFamilyID = productFamily.ID;
                }

                var existingGenerator = _generatorRepository.GetSingle(x => x.ProductFamilyID == generator.ProductFamilyID
                                                                && x.ModelDescription.Equals(generator.ModelDescription, StringComparison.InvariantCultureIgnoreCase)
                                                                && x.Liters == generator.Liters);

                if (existingGenerator != null)
                {
                    foreach (var regulatoryFilter in existingGenerator.GeneratorRegulatoryFilters.ToList())
                    {
                        _generatorRegulatoryFilterRepository.Delete(regulatoryFilter);
                    }

                    _addImportGensetDataRequestDtoToGeneratorEntityMapper.UpdateMap(item, existingGenerator);

                    generator = existingGenerator;
                }

                if (item.PrimePowerAvailable.Equals("Yes", StringComparison.InvariantCultureIgnoreCase))
                {
                    generator.PrimePowerAvailable = true;
                }

                var selectedFrequency = frequencyList.FirstOrDefault(f => f.Value.Equals(item.Frequency.ToString(), StringComparison.InvariantCultureIgnoreCase));
                if (selectedFrequency == null)
                {
                    var errStr = $"<li> No Frequency exist for 'Frequency' :  {item.Frequency} </li>";
                    error = GetError(rowNum, errStr, error);
                }
                else
                {
                    generator.FrequencyID = selectedFrequency.ID;
                }

                if(!string.IsNullOrEmpty(item.RegulatoryFilters))
                {
                    var regulatoryFilters = item.RegulatoryFilters.Split(',').ToList();
                    foreach (var regulatoryFilter in regulatoryFilters)
                    {
                        var selectedFilter = regFilters.FirstOrDefault(r => r.Description.Contains(regulatoryFilter));
                        if (selectedFilter == null)
                        {
                            var errStr = $"<li> No RegulatoryFilter exist for 'RegulatoryFilters' :  {item.RegulatoryFilters} </li>";
                            error = GetError(rowNum, errStr, error);
                        }
                        else
                        {
                            generator.GeneratorRegulatoryFilters.Add(
                                new GeneratorRegulatoryFilter
                                {
                                    RegulatoryFilterID = selectedFilter.ID,
                                    CreatedBy = userName,
                                    CreatedDateTime = DateTime.UtcNow,
                                    ModifiedBy = userName,
                                    ModifiedDateTime = DateTime.UtcNow
                                });
                        }
                    }
                }
               

                var availableFuels = item.AvailableFuelCode.Split(',').ToList();
                generator.AvailableFuelCode = string.Empty;
                foreach (var fuel in availableFuels)
                {
                    var selectedFuelType = fuelTypeList.FirstOrDefault(f => f.Description.Contains(fuel.Trim()) || f.Code.Contains(fuel.Trim()));
                    if (selectedFuelType == null)
                    {
                        var errStr = $"<li> No AvailableFuelCode exist for 'AvailableFuelCode' :  {item.AvailableFuelCode} </li>";
                        error = GetError(rowNum, errStr, error);
                    }
                    else
                    {
                        generator.AvailableFuelCode += selectedFuelType.Code;
                    }
                }

                if (item.PMGConfigurable.Equals("Yes", StringComparison.InvariantCultureIgnoreCase))
                {
                    generator.PMGConfigurable = true;
                }
                else
                {
                    generator.PMGConfigurable = false;
                }

                if (item.IsGemini.Equals("Yes", StringComparison.InvariantCultureIgnoreCase))
                {
                    generator.IsGemini = true;
                }
                else
                {
                    generator.IsGemini = false;
                }

                if (item.IsParallelable.Equals("Yes", StringComparison.InvariantCultureIgnoreCase))
                {
                    generator.IsParallelable = true;
                }
                else
                {
                    generator.IsParallelable = false;
                }

                if (item.LPFuelCheck == 1)
                {
                    generator.LPFuelCheck = true;
                }
                else
                {
                    generator.LPFuelCheck = false;
                }

                if (string.IsNullOrEmpty(error))
                {
                    generator.CreatedDateTime = DateTime.UtcNow;
                    generator.ModifiedDateTime = DateTime.UtcNow;
                    generator.CreatedBy = userName;
                    generator.ModifiedBy = userName;
                    generator.Active = true;
                    if (existingGenerator == null)
                    {
                        _generatorRepository.Add(generator);
                    }
                    else
                    {
                        _generatorRepository.Update(generator);
                    }
                }
                else
                {
                    errorMessage += $"{error} </ul>";
                }

                rowNum++;
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                throw new PowerDesignProException("", "", errorMessage);
            }
            else
            {
                _generatorRepository.Commit();
            }

            return true;
        }

        public bool ImportAlternatorData(IEnumerable<ImportAlternatorDataRequestDto> alternatorData, string userName)
        {
            var errorMessage = string.Empty;
            int rowNum = 1;
            var alternatorFamilyList = _alternatorFamilyRepository.GetAll().ToList();
            var frequencyList = _frequencyRepository.GetAll().ToList();
            var voltagePhaseList = _voltagePhaseRepository.GetAll().ToList();
            var voltageNominalList = _voltageNominalRepository.GetAll().ToList();
            foreach (var item in alternatorData)
            {
                var error = string.Empty;
                var alternator = _addImportAlternatorDataRequestDtoToAlternatorEntityMapper.AddMap(item);

                var alternatorFamily = alternatorFamilyList.FirstOrDefault(a => a.Description.Contains(item.AlternatorFamily.Trim()));
                if (alternatorFamily == null)
                {
                    var errStr = $"<li> No AlternatorFamily detail exist for 'AlternatorFamily' :  {item.AlternatorFamily} </li>";
                    error = GetError(rowNum, errStr, error);
                }
                else
                {
                    alternator.AlternatorFamilyID = alternatorFamily.ID;
                }

                var existingAlternator = _alternatorRepository.GetSingle(x => x.AlternatorFamilyID == alternator.AlternatorFamilyID
                                                                        && x.ModelDescription.Equals(alternator.ModelDescription.Trim(), StringComparison.InvariantCultureIgnoreCase)
                                                                        && x.KWRating == alternator.KWRating);

                if (existingAlternator != null)
                {
                    _addImportAlternatorDataRequestDtoToAlternatorEntityMapper.UpdateMap(item, existingAlternator);
                    alternator = existingAlternator;
                }

                var frequency = frequencyList.FirstOrDefault(f => f.Value == item.Frequency.ToString());
                if (frequency == null)
                {
                    var errStr = $"<li> No Frequency detail exist for 'Frequency' :  {item.Frequency} </li>";
                    error = GetError(rowNum, errStr, error);
                }
                else
                {
                    alternator.FrequencyID = frequency.ID;
                }

                var voltagePhase = voltagePhaseList.FirstOrDefault(f => f.Description.Equals(item.VoltagePhase.Trim(), StringComparison.InvariantCultureIgnoreCase));
                if (voltagePhase == null)
                {
                    var errStr = $"<li> No VoltagePhase detail exist for 'VoltagePhase' :  {item.VoltagePhase} </li>";
                    error = GetError(rowNum, errStr, error);
                }
                else
                {
                    alternator.VoltagePhaseID = voltagePhase.ID;
                }

                var voltageNominal = voltageNominalList.FirstOrDefault(f => f.Value == item.VoltageNominal.ToString());
                if (voltageNominal == null)
                {
                    var errStr = $"<li> No VoltageNominal detail exist for 'VoltageNominal' :  {item.VoltageNominal} </li>";
                    error = GetError(rowNum, errStr, error);
                }
                else
                {
                    alternator.VoltageNominalID = voltageNominal.ID;
                }

                if (string.IsNullOrEmpty(error))
                {
                    alternator.CreatedDateTime = DateTime.UtcNow;
                    alternator.ModifiedDateTime = DateTime.UtcNow;
                    alternator.Active = true;

                    if (existingAlternator == null)
                    {
                        _alternatorRepository.Add(alternator);
                    }
                    else
                    {
                        _alternatorRepository.Update(alternator);
                    }
                }
                else
                {
                    errorMessage += $"{error} </ul>";
                }

                rowNum++;
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                throw new PowerDesignProException("", "", errorMessage);
            }
            else
            {
                _alternatorRepository.Commit();
            }

            return true;
        }

        private static string GetError(int rowNum, string currentError, string error)
        {
            if (string.IsNullOrEmpty(error))
            {
                error = $"<p> <b>Server Errors in Excel Data Row {rowNum} : </b> </p> <ul>";
            }

            error += currentError;
            return error;
        }

        private GeneratorAvailableVoltage ToAvailableVoltageEntity(int input, string userID)
        {
            return new GeneratorAvailableVoltage
            {
                VoltageNominalID = input,
                CreatedDateTime = DateTime.UtcNow,
                ModifiedDateTime = DateTime.UtcNow,
                ModifiedBy = userID,
                CreatedBy = userID
            };
        }

        private GeneratorRegulatoryFilter ToAvailableGeneratorRegulatoryEntity(int input, string userID)
        {
            return new GeneratorRegulatoryFilter
            {
                RegulatoryFilterID = input,
                CreatedDateTime = DateTime.UtcNow,
                ModifiedDateTime = DateTime.UtcNow,
                ModifiedBy = userID,
                CreatedBy = userID
            };
        }

        private GeneratorAvailableAlternator ToAvailableGeneratorAlternatorEntity(int input, string userID)
        {
            return new GeneratorAvailableAlternator
            {
                AlternatorID = input,
                CreatedDateTime = DateTime.UtcNow,
                ModifiedDateTime = DateTime.UtcNow,
                ModifiedBy = userID,
                CreatedBy = userID
            };
        }

        private IEnumerable<AlternatorFamilyTreeDto> GetAlternatorFamilyTreeList()
        {
            var alternatorFamilyTreeList = new List<AlternatorFamilyTreeDto>();

            var alternatorFamilyList = _alternatorRepository.GetAll(a => a.Active).Select(x => x.AlternatorFamily).Distinct().ToList();
            foreach (var altFamily in alternatorFamilyList.Where(x=>x.Active))
            {
                var availableFrequencyList = altFamily.Alternators.Where(x => x.Active).Select(x => x.Frequency).Distinct().ToList();
                foreach (var frequency in availableFrequencyList)
                {
                    var voltageNominalList = altFamily.Alternators.Where(x => x.FrequencyID == frequency.ID && x.Active).Select(x => x.VoltageNominal).Distinct().ToList();
                    var alternatorFamilyTree = new AlternatorFamilyTreeDto
                    {
                        ID = altFamily.ID,
                        Description = altFamily.Description,
                        Frequency = frequency.ID,
                        VoltageNominalList = voltageNominalList.Select(v => v.ID).ToList()
                    };

                    var frequencyTree = new FrequencyTreeDto
                    {
                        ID = frequency.ID,
                        Description = frequency.Description
                    };

                    foreach (var voltageNominal in voltageNominalList)
                    {
                        var voltageNominalTree = new VoltageNominalTreeDto
                        {
                            ID = voltageNominal.ID,
                            Description = voltageNominal.Description,
                            VoltagePhaseTree = new List<VoltagePhaseTreeDto>
                            {
                                new VoltagePhaseTreeDto {
                                    ID = voltageNominal.VoltagePhaseID,
                                    Description = voltageNominal.VoltagePhase.Description,
                                    AlternatorTree = voltageNominal.Alternators.Where(a => a.AlternatorFamilyID == altFamily.ID && a.Active).OrderBy(x=>x.KWRating).Select(x =>
                                    new AlternatorTreeDto
                                        {
                                            ID = x.ID,
                                            Description = x.ModelDescription
                                        }).ToList()
                                }
                            }

                        };

                        frequencyTree.VoltageNominalTree.Add(voltageNominalTree);
                    }

                    alternatorFamilyTree.FrequencyTree.Add(frequencyTree);

                    alternatorFamilyTreeList.Add(alternatorFamilyTree);
                }
            }

            return alternatorFamilyTreeList;
        }

        public IEnumerable<ProductFamilyDto> GetProductFamilies()
        {
            return _productFamilyRepository.GetAll(x=>x.Active).ToList().OrderBy(pf => pf.Ordinal).
                Select(pf => new ProductFamilyDto
                {
                    Id = pf.ID,
                    Description = pf.Description,
                    IsDomestic = pf.IsDomestic,
                    IsGemini = pf.IsGemini,
                    Priority = pf.Priority,
                    BrandID = pf.BrandID,
                    LanguageKey=pf.LanguageKey,
                    BrandList = _brandRepository.GetAll().ToList().OrderBy(b => b.Ordinal).
                        Select(b => new PickListDto
                        {
                            ID = b.ID,
                            Description = b.Description,
                            Value = b.Value
                        })
                });
        }

        public ProductFamilyDto GetProductFamily(SearchAdminRequestDto searchDto)
        {
            var familyDto = new ProductFamilyDto();
            if (searchDto.ID == 0)
            {
                familyDto = new ProductFamilyDto
                {
                    Id = 0,
                    Description = string.Empty,
                    IsDomestic = false,
                    Priority = 0,
                    BrandID = 0,
                    IsGemini = false,
                    BrandList = _brandRepository.GetAll().ToList().OrderBy(b => b.Ordinal).
                                                      Select(b => new PickListDto
                                                      {
                                                          ID = b.ID,
                                                          Description = b.Description,
                                                          Value = b.Value
                                                      })
                };
            }
            else
            {
                var familyDetail = _productFamilyRepository.GetSingle(p => p.ID == searchDto.ID);
                if (familyDetail == null)
                {
                    throw new PowerDesignProException("ProductFamilyNotFound", Message.Admin);
                }
                else
                {
                    familyDto = new ProductFamilyDto
                    {
                        Id = familyDetail.ID,
                        Description = familyDetail.Description,
                        IsDomestic = familyDetail.IsDomestic,
                        IsGemini = familyDetail.IsGemini,
                        Priority = familyDetail.Priority,
                        BrandID = familyDetail.BrandID,
                        BrandList = _brandRepository.GetAll().ToList().OrderBy(b => b.Ordinal).
                                              Select(b => new PickListDto
                                              {
                                                  ID = b.ID,
                                                  Description = b.Description,
                                                  Value = b.Value
                                              })
                    };
                }                
            }
            return familyDto;
        }

        public AdminResponseDto SaveProductFamily(ProductFamilyDto productFamilyResponseDto)
        {
            if (productFamilyResponseDto.Id == 0)
            {
                return AddProductFamilyDetail(productFamilyResponseDto);
            }
            else
            {
                return UpdateProductFamilyDetail(productFamilyResponseDto);
            }
        }

        private AdminResponseDto AddProductFamilyDetail(ProductFamilyDto productFamilyResponseDto)
        {
            var maxOrdinal = _productFamilyRepository.GetAll().ToList().OrderByDescending(af => af.Priority).
                FirstOrDefault().Priority;

            var productFamily = new ProductFamily
            {            
                Description= productFamilyResponseDto.Description,
                IsDomestic = productFamilyResponseDto.IsDomestic,
                IsGemini = productFamilyResponseDto.IsGemini,
                Priority = maxOrdinal + 1,
                BrandID = productFamilyResponseDto.BrandID,
                Ordinal = maxOrdinal + 1,
                Value = productFamilyResponseDto.Description
            };

            productFamily.Active = true;
            productFamily.ModifiedDateTime = DateTime.UtcNow;
            productFamily.ModifiedBy = productFamilyResponseDto.Username; 
            productFamily.CreatedBy = productFamilyResponseDto.Username; 
            productFamily.CreatedDateTime = DateTime.UtcNow;

            var productFamilyDetail = _productFamilyRepository.Add(productFamily);
            _productFamilyRepository.Commit();

            return new AdminResponseDto
            {
                ID = productFamilyDetail.ID
            };
        }

        private AdminResponseDto UpdateProductFamilyDetail(ProductFamilyDto productFamilyResponseDto)
        {
            var productFamily = _productFamilyRepository.Find(productFamilyResponseDto.Id);
            if (productFamily == null)
            {
                throw new PowerDesignProException("ProductFamilyNotFound", Message.Admin);
            }

            if (productFamily.Priority != productFamilyResponseDto.Priority)
            {
                // check if priotiry is changed
                int direction = productFamilyResponseDto.Priority - productFamily.Priority;
                if(direction < 0)
                {
                    // down
                    var downProductFmaily = _productFamilyRepository.GetAll().Where(p => p.Priority < productFamily.Priority).OrderByDescending(p => p.Priority).FirstOrDefault();
                    int downProductFmailyPriority = downProductFmaily.Priority;
                    downProductFmaily.Priority = productFamily.Priority;
                    downProductFmaily.Ordinal = productFamily.Priority;
                    productFamily.Priority = downProductFmailyPriority;

                    downProductFmaily.Active = true;
                    downProductFmaily.ModifiedDateTime = DateTime.UtcNow;
                    downProductFmaily.ModifiedBy = productFamilyResponseDto.Username;
                   

                    _productFamilyRepository.Update(downProductFmaily);
                }
                else
                {
                    // up
                    var upProductFmaily = _productFamilyRepository.GetAll().Where(p => p.Priority > productFamily.Priority).OrderBy(p => p.Priority).FirstOrDefault();
                    int upProductFmailyPriority = upProductFmaily.Priority;
                    upProductFmaily.Priority = productFamily.Priority;
                    upProductFmaily.Ordinal = productFamily.Priority;
                    productFamily.Priority = upProductFmailyPriority;

                    upProductFmaily.Active = true;
                    upProductFmaily.ModifiedDateTime = DateTime.UtcNow;
                    upProductFmaily.ModifiedBy = productFamilyResponseDto.Username; 

                    _productFamilyRepository.Update(upProductFmaily);
                }
            }
            else
            {
                productFamily.Priority = productFamilyResponseDto.Priority;
            }

            productFamily.Description = productFamilyResponseDto.Description;
            productFamily.Value = productFamilyResponseDto.Description;
            productFamily.IsDomestic = productFamilyResponseDto.IsDomestic;
            productFamily.IsGemini = productFamilyResponseDto.IsGemini;
            productFamily.BrandID = productFamilyResponseDto.BrandID;
            productFamily.Ordinal = productFamily.Priority;

            productFamily.Active = true;
            productFamily.ModifiedDateTime = DateTime.UtcNow;
            productFamily.ModifiedBy = productFamilyResponseDto.Username; 

            var productFamilyDetail = _productFamilyRepository.Update(productFamily);

            _productFamilyRepository.Commit();

            return new AdminResponseDto
            {
                ID = productFamilyDetail.ID
            };
        }

        public bool DeleteProductFamily(int productFamilyID, string userName)
        {
            var productFamily = _productFamilyRepository.GetSingle(x => x.ID == productFamilyID);
            if (productFamily == null)
            {
                throw new PowerDesignProException("ProductFamilyNotFound", Message.Admin);
            }

            productFamily.Active = false;
            productFamily.ModifiedDateTime = DateTime.UtcNow;
            productFamily.ModifiedBy = userName;

            _productFamilyRepository.Update(productFamily);
            _productFamilyRepository.Commit();
            return true;
        }

        public IEnumerable<DocumentationDto> GetDocumentationsByGenerator(int generatorID)
        {
            return _documentationRepository.GetAll(d => d.GeneratorID == generatorID).ToList().OrderBy(d => d.Ordinal).
                Select(d => new DocumentationDto
                {
                    Id = d.ID,
                    Description = d.Description,
                    DocumentURL = d.DocumentURL,
                    Ordinal = d.Ordinal,
                    GeneratorID = d.GeneratorID
                });
        }

        public DocumentationDto GetDocumentation(SearchAdminRequestDto searchDto)
        {
            var documentationDto = new DocumentationDto();
            if (searchDto.ID == 0)
            {
                documentationDto = new DocumentationDto
                {
                    Id = 0,
                    Description = string.Empty,
                    DocumentURL = string.Empty,
                    Ordinal = 0,
                    GeneratorID = 0
                };
            }
            else
            {
                var documentation = _documentationRepository.GetSingle(d => d.ID == searchDto.ID);
                if (documentationDto == null)
                {
                    throw new PowerDesignProException("DocumentationNotFound", Message.Admin);
                }
                else
                {
                    documentationDto = new DocumentationDto
                    {
                        Id = documentation.ID,
                        Description = documentation.Description,
                        DocumentURL = documentation.DocumentURL,
                        Ordinal = documentation.Ordinal,
                        GeneratorID = documentation.GeneratorID
                    };
                }
            }
            return documentationDto;
        }

        public bool DeleteDocumentation(int documentationID, int generatorID, string userName)
        {
            var documentation = _documentationRepository.GetSingle(d => d.ID == documentationID);
            if (documentation == null)
            {
                throw new PowerDesignProException("DocumentationNotFound", Message.Admin);
            }

            _documentationRepository.Delete(documentation);
            _documentationRepository.Commit();

            var docDto = new DocumentationDto()
            {
                Id = documentationID,
                GeneratorID = generatorID,
                UserName = userName
            };

            UpdateGeneratorModifyBy(docDto);
            return true;
        }

        public AdminResponseDto SaveDocumentation(DocumentationDto documentationDto)
        {
            UpdateGeneratorModifyBy(documentationDto);
            if (documentationDto.Id == 0)
            {
                return AddDocumentationDetail(documentationDto);
            }
            else
            {
                return UpdateDocumentationDetail(documentationDto);
            }
        }

        private void UpdateGeneratorModifyBy(DocumentationDto docDto)
        {
            var generator = _generatorRepository.GetSingle(g => g.ID == docDto.GeneratorID);
            if(generator != null)
            {
                generator.ModifiedBy = docDto.UserName;
                generator.ModifiedDateTime = DateTime.UtcNow;
            }
            _generatorRepository.Update(generator);
            _generatorRepository.Commit();
        }
        
        private AdminResponseDto AddDocumentationDetail(DocumentationDto documentationResponseDto)
        {
            var tempDocumentation = _documentationRepository.GetAll(d => d.GeneratorID == documentationResponseDto.GeneratorID).ToList().OrderByDescending(d => d.Ordinal).
                FirstOrDefault();
            int maxOrdinal = 0;
            if (tempDocumentation != null)
            {
                maxOrdinal = tempDocumentation.Ordinal;
            }

            var documentation = new Documentation
            {
                Description = documentationResponseDto.Description,
                Value = documentationResponseDto.Description,
                DocumentURL = documentationResponseDto.DocumentURL,
                GeneratorID = documentationResponseDto.GeneratorID,
                Ordinal = maxOrdinal + 1,
                CreatedBy = documentationResponseDto.UserName,
                CreatedDateTime = DateTime.UtcNow,
                ModifiedBy = documentationResponseDto.UserName,
                ModifiedDateTime = DateTime.UtcNow
            };

            var documentationDetail = _documentationRepository.Add(documentation);
            _documentationRepository.Commit();

            return new AdminResponseDto
            {
                ID = documentationDetail.ID
            };
        }

        private AdminResponseDto UpdateDocumentationDetail(DocumentationDto documentationResponseDto)
        {
            var documentation = _documentationRepository.Find(documentationResponseDto.Id);
            if (documentation == null)
            {
                throw new PowerDesignProException("DocumentationNotFound", Message.Admin);
            }

            if (documentation.Ordinal != documentationResponseDto.Ordinal)
            {
                // check if priotiry is changed
                int direction = documentationResponseDto.Ordinal - documentation.Ordinal;
                if (direction < 0)
                {
                    // down
                    var downDocumentation = _documentationRepository.GetAll(d => d.GeneratorID == documentation.GeneratorID).Where(p => p.Ordinal < documentation.Ordinal).OrderByDescending(p => p.Ordinal).FirstOrDefault();
                    int downOrdinal = downDocumentation.Ordinal;
                    downDocumentation.Ordinal = documentation.Ordinal;
                    documentation.Ordinal = downOrdinal;
                    _documentationRepository.Update(downDocumentation);
                }
                else
                {
                    // up
                    var upDocumentation = _documentationRepository.GetAll(d => d.GeneratorID == documentation.GeneratorID).Where(p => p.Ordinal > documentation.Ordinal).OrderBy(p => p.Ordinal).FirstOrDefault();
                    int upOrdinal = upDocumentation.Ordinal;
                    upDocumentation.Ordinal = documentation.Ordinal;
                    documentation.Ordinal = upOrdinal;
                    _documentationRepository.Update(upDocumentation);
                }
            }
            else
            {
                documentation.Ordinal = documentationResponseDto.Ordinal;
            }

            documentation.Description = documentationResponseDto.Description;
            documentation.Value = documentationResponseDto.Description;
            documentation.DocumentURL = documentationResponseDto.DocumentURL;
            documentation.GeneratorID = documentationResponseDto.GeneratorID;
            documentation.ModifiedBy = documentationResponseDto.UserName;
            documentation.ModifiedDateTime = DateTime.UtcNow;

            var documentationDetail = _documentationRepository.Update(documentation);
            _documentationRepository.Commit();

            return new AdminResponseDto
            {
                ID = documentationDetail.ID
            };
        }
        #endregion

        #region Alternator
        public IEnumerable<AlternatorDto> GetAlternatorDetails()
        {
            return _alternatorRepository.GetAll(x=> x.Active).ToList().
                Select(x => new AlternatorDto
                {
                    ID = x.ID,
                    ModelDescription = x.ModelDescription,
                    AlternatorFamilyID = x.AlternatorFamilyID,
                    KWRating=x.KWRating,
                    FrequencyDescription =x.Frequency.Description,
                    VoltageNominalDescription=x.VoltageNominal.Description,
                    VoltagePhaseDescription=x.VoltagePhase.Description,
                });
        }

        public AlternatorDto GetAlternator(SearchAdminRequestDto searchDto)
        {
            if (searchDto.ID > 0)
            {
                var alternatorDetail = _alternatorRepository.GetSingle(p => p.ID == searchDto.ID && p.Active);

                if (alternatorDetail == null)
                {
                    throw new PowerDesignProException("AlternatorNotFound", Message.Admin);
                }

                return _alternatorEntityToAlternatorDtoMapper.AddMap(alternatorDetail);
            }

            throw new PowerDesignProException("AlternatorNotFound", Message.Admin);
        }

        public AdminResponseDto SaveAlternatorDetail(AlternatorDto alternatorResponseDto)
        {
            if (alternatorResponseDto.ID == 0)
            {
                return AddAlternatorDetail(alternatorResponseDto);
            }
            else
            {
                return UpdateAlternatorDetail(alternatorResponseDto);
            }
        }

        private AdminResponseDto AddAlternatorDetail(AlternatorDto alternatorRequestDto)
        {
            var alternator = _addAlternatorDtoToEntityMapper.AddMap(alternatorRequestDto);

            alternator.Active = true;
            alternator.CreatedDateTime = DateTime.UtcNow;
            alternator.CreatedBy = alternatorRequestDto.UserName;
            alternator.ModifiedDateTime = DateTime.UtcNow;
            alternator.ModifiedBy = alternatorRequestDto.UserName;

            var alternatorDetail = _alternatorRepository.Add(alternator);
            _alternatorRepository.Commit();

            return new AdminResponseDto
            {
                ID = alternatorDetail.ID
            };
        }

        private AdminResponseDto UpdateAlternatorDetail(AlternatorDto alternatorRequestDto)
        {
            var alternator = _alternatorRepository.Find(alternatorRequestDto.ID);
            if (alternator == null)
            {
                throw new PowerDesignProException("AlternatorNotFound", Message.Admin);
            }
            _addAlternatorDtoToEntityMapper.UpdateMap(alternatorRequestDto, alternator);

            alternator.Active = true;
            alternator.ModifiedDateTime = DateTime.UtcNow;
            alternator.ModifiedBy = alternatorRequestDto.UserName;

            var alternatorDetail = _alternatorRepository.Update(alternator);
            _alternatorRepository.Commit();

            return new AdminResponseDto
            {
                ID = alternatorDetail.ID
            };
        }

        public bool DeleteAlternator(int alternatorID,string userName)
        {
            var alternator = _alternatorRepository.GetSingle(x => x.ID == alternatorID);
            if (alternator == null)
            {
                throw new PowerDesignProException("AlternatorNotFound", Message.Admin);
            }

            alternator.Active = false;
            alternator.ModifiedDateTime = DateTime.UtcNow;
            alternator.ModifiedBy = userName;

            _alternatorRepository.Update(alternator);
            _alternatorRepository.Commit();
            return true;
        }

        public IEnumerable<AlternatorFamilyDto> GetAlternatorFamilies()
        {
            return _alternatorFamilyRepository.GetAll(x => x.Active).ToList().OrderBy(af => af.Ordinal).
                Select(af => new AlternatorFamilyDto
                {
                    ID = af.ID,
                    Description = af.Description,
                    Value = af.Value
                });
        }

        public AdminResponseDto SaveAlternatorFamily(AlternatorFamilyDto alternatorFamilyResponseDto)
        {
            if (alternatorFamilyResponseDto.ID == 0)
            {
                return AddAlternatorFamilyDetail(alternatorFamilyResponseDto);
            }
            else
            {
                return UpdateAlternatorFamilyDetail(alternatorFamilyResponseDto);
            }
        }

        private AdminResponseDto AddAlternatorFamilyDetail(AlternatorFamilyDto alternatorFamilyResponseDto)
        {
            var maxOrdinal = _alternatorFamilyRepository.GetAll().ToList().OrderByDescending(af => af.Ordinal).
                FirstOrDefault().Ordinal;

            var alternatorFamily = new AlternatorFamily
            {
                Description = alternatorFamilyResponseDto.Description,
                Value = alternatorFamilyResponseDto.Value,
                Ordinal = maxOrdinal + 1,
                Active=true,
                CreatedBy= alternatorFamilyResponseDto.UserName,
                CreatedDateTime =DateTime.UtcNow,
                ModifiedBy= alternatorFamilyResponseDto.UserName,
                ModifiedDateTime=DateTime.UtcNow
            };

            var alternatorFamilyDetail = _alternatorFamilyRepository.Add(alternatorFamily);
            _alternatorFamilyRepository.Commit();

            return new AdminResponseDto
            {
                ID = alternatorFamilyDetail.ID
            };
        }

        private AdminResponseDto UpdateAlternatorFamilyDetail(AlternatorFamilyDto alternatorFamilyResponseDto)
        {
            var alternatorFamily = _alternatorFamilyRepository.Find(alternatorFamilyResponseDto.ID);
            if (alternatorFamily == null)
            {
                throw new PowerDesignProException("AlternatorFamilyNotFound", Message.Admin);
            }

            alternatorFamily.Description = alternatorFamilyResponseDto.Description;
            alternatorFamily.Value = alternatorFamilyResponseDto.Value;
            alternatorFamily.Active = true;
            alternatorFamily.ModifiedBy = alternatorFamilyResponseDto.UserName;
            alternatorFamily.ModifiedDateTime = DateTime.UtcNow;

            var alternatorFamilyDetail = _alternatorFamilyRepository.Update(alternatorFamily);
            _alternatorFamilyRepository.Commit();

            return new AdminResponseDto
            {
                ID = alternatorFamilyDetail.ID
            };
        }

        public bool DeleteAlternatorFamily(int alternatorFamilyID,string userName)
        {
            var alternatorFamily = _alternatorFamilyRepository.GetSingle(x => x.ID == alternatorFamilyID);
            if (alternatorFamily == null)
            {
                throw new PowerDesignProException("AlternatorFamilyNotFound", Message.Admin);
            }

            alternatorFamily.Active = false;
            alternatorFamily.ModifiedBy = userName;
            alternatorFamily.ModifiedDateTime = DateTime.UtcNow;

            _alternatorFamilyRepository.Update(alternatorFamily);
            _alternatorFamilyRepository.Commit();
            return true;
        }

        #endregion

        #region Maintain Market Vertical
        public IEnumerable<SolutionApplicationDto> GetMaintainMarketVertical()
        {
            return _solutionApplicationRepository.GetAll(x => x.Active).ToList().OrderBy(af => af.Ordinal).
                Select(af => new SolutionApplicationDto
                {
                    ID = af.ID,
                    Description = af.Description,
                    Value = af.Value,
                    Active=af.Active
                });
        }

        public AdminResponseDto SaveMaintainMarketVertical(SolutionApplicationDto solutionApplicationDto)
        {
            if (solutionApplicationDto.ID == 0)
            {
                return AddMaintainMarketVertical(solutionApplicationDto);
            }
            else
            {
                return UpdateMaintainMarketVertical(solutionApplicationDto);
            }
        }

        private AdminResponseDto AddMaintainMarketVertical(SolutionApplicationDto solutionApplicationDto)
        {
            var maxOrdinal = 0;
            var count = _solutionApplicationRepository.GetAll().Count();
            if (count > 0)
            {
                maxOrdinal = _solutionApplicationRepository.GetAll().ToList().OrderByDescending(af => af.Ordinal).
                FirstOrDefault().Ordinal;
            }

            var solutionApplication = new SolutionApplication
            {
                Description = solutionApplicationDto.Description,
                Value = solutionApplicationDto.Value,
                Ordinal = maxOrdinal + 1,
                Active = true,
                CreatedBy = solutionApplicationDto.UserName,
                CreatedDateTime = DateTime.UtcNow,
                ModifiedBy = solutionApplicationDto.UserName,
                ModifiedDateTime = DateTime.UtcNow
            };

            var solutionApplicationDeatails = _solutionApplicationRepository.Add(solutionApplication);
            _solutionApplicationRepository.Commit();

            return new AdminResponseDto
            {
                ID = solutionApplicationDeatails.ID
            };
        }

        private AdminResponseDto UpdateMaintainMarketVertical(SolutionApplicationDto solutionApplicationDto)
        {
            var solutionApplication = _solutionApplicationRepository.Find(solutionApplicationDto.ID);
            if (solutionApplication == null)
            {
                throw new PowerDesignProException("MaintainMarketVerticalNotFound", Message.Admin);
            }

            solutionApplication.Description = solutionApplicationDto.Description;
            solutionApplication.Value = solutionApplicationDto.Value;
            solutionApplication.Active = true;
            solutionApplication.ModifiedBy = solutionApplicationDto.UserName;
            solutionApplication.ModifiedDateTime = DateTime.UtcNow;

            var maintainMarketVerticalsDetail = _solutionApplicationRepository.Update(solutionApplication);
            _solutionApplicationRepository.Commit();

            return new AdminResponseDto
            {
                ID = maintainMarketVerticalsDetail.ID
            };
        }

        public bool DeleteMaintainMarketVerticals(int maintainMarketVerticalID, string userName)
        {
            var solutionApplication = _solutionApplicationRepository.GetSingle(x => x.ID == maintainMarketVerticalID);
            if (solutionApplication == null)
            {
                throw new PowerDesignProException("MaintainMarketVerticalNotFound", Message.Admin);
            }

            solutionApplication.Active = false;
            solutionApplication.ModifiedBy = userName;
            solutionApplication.ModifiedDateTime = DateTime.UtcNow;

            _solutionApplicationRepository.Update(solutionApplication);
            _solutionApplicationRepository.Commit();
            return true;
        }
        #endregion

        #region Harmonic Device

        public IEnumerable<HarmonicDeviceTypeDto> GetHarmonicDeviceTypeDetail()
        {
            return _harmonicDeviceTypeRepository.GetAll(p=> p.Active).OrderByDescending(p=>p.ModifiedDateTime).ToList().
                Select(x => new HarmonicDeviceTypeDto
                {
                    ID = x.ID,
                    Description = x.Description,
                    StartingMethodDesc = x.StartingMethod.Description,
                    HarmonicContentDesc = x.HarmonicContent.Description,
                    StartingMethodID=x.StartingMethod.ID,
                });
        }

        public HarmonicDeviceTypeDto GetHarmonicDeviceType(SearchAdminRequestDto searchDto)
        {
            if (searchDto.ID > 0)
            {
                var harmonicDeviceTypeDetail = _harmonicDeviceTypeRepository.GetSingle(p => p.ID == searchDto.ID && p.Active);

                if (harmonicDeviceTypeDetail == null)
                {
                    throw new PowerDesignProException("HarmonicDeviceNotFound", Message.Admin);
                }

                return _harmonicDeviceTypeEntityToHarmonicDeviceTypeDtoMapper.AddMap(harmonicDeviceTypeDetail);
            }

            throw new PowerDesignProException("HarmonicDeviceNotFound", Message.Admin);
        }

        public AdminResponseDto SaveHarmonicDeviceTypeDetail(HarmonicDeviceTypeDto harmonicDeviceRequestDto)
        {
            if (harmonicDeviceRequestDto.ID == 0)
            {
                return AddHarmonicDeviceDetail(harmonicDeviceRequestDto);
            }
            else
            {
                return UpdateHarmonicDeviceDetail(harmonicDeviceRequestDto);
            }
        }

        private AdminResponseDto AddHarmonicDeviceDetail(HarmonicDeviceTypeDto harmonicDeviceRequestDto)
        {
            var maxOrdinal = _harmonicDeviceTypeRepository.GetAll().ToList().OrderByDescending(af => af.Ordinal).
               FirstOrDefault().Ordinal;

            var harmonicDeviceType = _addHarmonicDeviceDtoToEntityMapper.AddMap(harmonicDeviceRequestDto);

            harmonicDeviceType.Active = true;
            harmonicDeviceType.Value = harmonicDeviceRequestDto.Description;
            harmonicDeviceType.CreatedDateTime = DateTime.UtcNow;
            harmonicDeviceType.CreatedBy = harmonicDeviceRequestDto.UserName;
            harmonicDeviceType.ModifiedDateTime = DateTime.UtcNow;
            harmonicDeviceType.ModifiedBy = harmonicDeviceRequestDto.UserName;
            harmonicDeviceType.Ordinal = maxOrdinal + 1;
            

            var harmonicDeviceDetail = _harmonicDeviceTypeRepository.Add(harmonicDeviceType);
            _harmonicDeviceTypeRepository.Commit();

            return new AdminResponseDto
            {
                ID = harmonicDeviceDetail.ID
            };
        }

        private AdminResponseDto UpdateHarmonicDeviceDetail(HarmonicDeviceTypeDto harmonicDeviceRequestDto)
        {
            var harmonicDeviceType = _harmonicDeviceTypeRepository.Find(harmonicDeviceRequestDto.ID);
            if (harmonicDeviceType == null)
            {
                throw new PowerDesignProException("HarmonicDeviceNotFound", Message.Admin);
            }
            _addHarmonicDeviceDtoToEntityMapper.UpdateMap(harmonicDeviceRequestDto, harmonicDeviceType);

            harmonicDeviceType.Value = harmonicDeviceRequestDto.Description;
            harmonicDeviceType.Active = true;
            harmonicDeviceType.ModifiedDateTime = DateTime.UtcNow;
            harmonicDeviceType.ModifiedBy = harmonicDeviceRequestDto.UserName;
            

            var harmonicDeviceDetail = _harmonicDeviceTypeRepository.Update(harmonicDeviceType);
            _harmonicDeviceTypeRepository.Commit();

            return new AdminResponseDto
            {
                ID = harmonicDeviceDetail.ID
            };
        }

        public bool DeleteHarmonicDeviceType(int ID, string userName)
        {
            var harmonicDeviceType = _harmonicDeviceTypeRepository.GetSingle(x => x.ID == ID);
            if (harmonicDeviceType == null)
            {
                throw new PowerDesignProException("HarmonicDeviceNotFound", Message.Admin);
            }

            harmonicDeviceType.Active = false;
            harmonicDeviceType.ModifiedDateTime = DateTime.UtcNow;
            harmonicDeviceType.ModifiedBy = userName;

            _harmonicDeviceTypeRepository.Update(harmonicDeviceType);
            _harmonicDeviceTypeRepository.Commit();
            return true;
        }
        #endregion
    }
}
