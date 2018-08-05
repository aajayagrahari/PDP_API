using Aspose.Words;
using Aspose.Words.Saving;
using PowerDesignPro.BusinessProcessors.Dtos;
using System;
using System.Reflection;
using System.Linq;
using System.Web;
using PowerDesignPro.BusinessProcessors.Interface;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using static PowerDesignPro.BusinessProcessors.Dtos.SummaryReportDto;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Net;
using Newtonsoft.Json.Linq;
using PowerDesignPro.Common.Constant;
using System.Text.RegularExpressions;
using PowerDesignPro.Common;
using System.Diagnostics;
using System.Web.Hosting;

namespace PowerDesignPro.BusinessProcessors.Processors
{
    public class PDFProcessor : IPDF
    {
        private  string _imageName = "";
        private IProject _project;
        private ISolutionSummary _solutionSummary;
        private IProjectSolution _projectSolution;
        private ITraceMessage _traceMessageProcessor;
        JObject translatedData;
        public PDFProcessor(IProject project, ISolutionSummary solutionSummary, IProjectSolution projectSolution, ITraceMessage traceMessage)
        {
            _project = project;
            _solutionSummary = solutionSummary;
            _projectSolution = projectSolution;
            _traceMessageProcessor = traceMessage;
        } 
        public bool WritePDF(HarmonicAnalysisDto harmonicAnalysisDto)
        {
            try
            {
                string LicenseFile = @"C:\Program Files\Aspose\Aspose.Total.lic";
                if (System.IO.File.Exists(LicenseFile))
                {
                    License wordsLicense = new License();
                    wordsLicense.SetLicense(LicenseFile);
                }
                var targetFileName = "HarmonicAnalysis.pdf";
                Document document = GetDocument(harmonicAnalysisDto);
                PdfSaveOptions saveOptions = new PdfSaveOptions();
                PdfEncryptionDetails encryptionDetails = new PdfEncryptionDetails(string.Empty, Guid.NewGuid().ToString(), PdfEncryptionAlgorithm.RC4_40);
                encryptionDetails.Permissions = (PdfPermissions.DisallowAll | PdfPermissions.Printing);
                saveOptions.EncryptionDetails = encryptionDetails;
                //document.Save(HttpContext.Current.Response, targetFileName, ContentDisposition.Attachment, saveOptions);
                document.Save(targetFileName, SaveFormat.Pdf);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private Document GetDocument(HarmonicAnalysisDto harmonicAnalysisDto)
        {            
            Document template;
            //string sResourceFile = HostingEnvironment.MapPath("~/App_Data/HarmonicAnalysis.docx"); ;
            string sResourceFile = "PowerDesignPro.BusinessProcessors.ReportTemplates.HarmonicAnalysis_New.docx";

            Assembly assembly = Assembly.GetExecutingAssembly();
            using (System.IO.Stream stream = assembly.GetManifestResourceStream(sResourceFile))
            {
                template = new Document(stream);
            }

            Document outDocument = template.Clone();

            outDocument.MailMerge.Execute(
                new string[] { "HarmonicProfile", "Sequence", "KVANonLinearLoad", "THID", "THVD", "KVABase", "AlternatorLoading" },
                new object[] { harmonicAnalysisDto.HarmonicProfile, harmonicAnalysisDto.Sequence, harmonicAnalysisDto.KVANonLinearLoad, harmonicAnalysisDto.THID,
                harmonicAnalysisDto.THVD, harmonicAnalysisDto.KVABase, harmonicAnalysisDto.AlternatorLoading});

            outDocument.MailMerge.Execute(
                new string[] { "C_HD3", "C_HD5", "C_HD7", "C_HD9", "C_HD11", "C_HD13", "C_HD15", "C_HD17", "C_HD19" },
                new object[] {
                    harmonicAnalysisDto.CurrentHarmonicDistortion.HarmonicDistortion3,
                    harmonicAnalysisDto.CurrentHarmonicDistortion.HarmonicDistortion5,
                    harmonicAnalysisDto.CurrentHarmonicDistortion.HarmonicDistortion7,
                    harmonicAnalysisDto.CurrentHarmonicDistortion.HarmonicDistortion9,
                    harmonicAnalysisDto.CurrentHarmonicDistortion.HarmonicDistortion11,
                    harmonicAnalysisDto.CurrentHarmonicDistortion.HarmonicDistortion13,
                    harmonicAnalysisDto.CurrentHarmonicDistortion.HarmonicDistortion15,
                    harmonicAnalysisDto.CurrentHarmonicDistortion.HarmonicDistortion17,
                    harmonicAnalysisDto.CurrentHarmonicDistortion.HarmonicDistortion19
                });

            outDocument.MailMerge.Execute(
                new string[] { "V_HD3", "V_HD5", "V_HD7", "V_HD9", "V_HD11", "V_HD13", "V_HD15", "V_HD17", "V_HD19" },
                new object[] {
                    harmonicAnalysisDto.VoltageHarmonicDistortion.HarmonicDistortion3,
                    harmonicAnalysisDto.VoltageHarmonicDistortion.HarmonicDistortion5,
                    harmonicAnalysisDto.VoltageHarmonicDistortion.HarmonicDistortion7,
                    harmonicAnalysisDto.VoltageHarmonicDistortion.HarmonicDistortion9,
                    harmonicAnalysisDto.VoltageHarmonicDistortion.HarmonicDistortion11,
                    harmonicAnalysisDto.VoltageHarmonicDistortion.HarmonicDistortion13,
                    harmonicAnalysisDto.VoltageHarmonicDistortion.HarmonicDistortion15,
                    harmonicAnalysisDto.VoltageHarmonicDistortion.HarmonicDistortion17,
                    harmonicAnalysisDto.VoltageHarmonicDistortion.HarmonicDistortion19
                });

            outDocument.MailMerge.RemoveEmptyRegions = true;
            outDocument.MailMerge.RemoveEmptyParagraphs = true;
            outDocument.MailMerge.DeleteFields();
            outDocument.UpdateFields();

            return outDocument;
        }

        private XmlDocument TransformDataset(PDPView qQview, string xslTemplateFile)
        {

            XmlSerializer serializer = new XmlSerializer(typeof(PDPView));
            string xmlData;
            using (StringWriter sWriter = new StringWriter())
            {
                serializer.Serialize(sWriter, qQview);
                xmlData = sWriter.ToString();
            }
            XmlDocument xmlResults = new XmlDocument();
            System.Xml.XPath.XPathDocument xpathdoc;
            System.IO.StringWriter writer = new System.IO.StringWriter();
            System.Xml.Xsl.XsltSettings settings = new System.Xml.Xsl.XsltSettings(false, true);
            System.Xml.Xsl.XsltArgumentList args = new System.Xml.Xsl.XsltArgumentList();
            xpathdoc = new System.Xml.XPath.XPathDocument(new StringReader(xmlData));
            System.Xml.Xsl.XslTransform XSLT = new System.Xml.Xsl.XslTransform();
            XSLT.Load(xslTemplateFile);

            //  XSLT.Load(xslTemplateFile, settings, null); 
            XSLT.Transform(xpathdoc, args, writer);
            writer.Flush();
            xmlResults.LoadXml(writer.ToString());

            return xmlResults;
        }

        public PDPView GetSolutionDetails(ReportModel reportModel, PDPView pview,Data.Models.Solution solutionDto)
        {
             if (solutionDto != null)
            { 
              pview.Project=  new Project()
                {
                    ProjectSummaryLabel = GetTranslation("report","Projectsummary"),
                    ContactInformationLabel = GetTranslation("report", "ContactInformation"),
                    ProjectName = solutionDto.Project.ProjectName,
                    ProjectNameLabel = GetTranslation("report", "Project"),
                    Contact = solutionDto.Project.ContactName,
                    ContactLabel = GetTranslation("report", "Contact"),
                    SpecRefLabel = GetTranslation("solutionSection", "SpecReference"),
                    SpecRef = "",
                    EmailLabel = GetTranslation("pdpheader", "Email"),
                    Email = solutionDto.Project.ContactEmail,
                    PreparedByLabel = GetTranslation("pdpheader", "PreparedBy"),
                    PreparedByNameLabel = GetTranslation("report", "Name"),
                    PreparedByCompanyLabel = GetTranslation("pdpheader", "Company"),
                    PreparedByEmailLabel = GetTranslation("pdpheader", "Email"),
                    PreparedByPhoneLabel = GetTranslation("register", "Phone"),
                    PreparedByName = reportModel.UserName,
                    PreparedByCompany = reportModel.Company,
                    PreparedByEmail = reportModel.Email,
                    PreparedByPhone = reportModel.Phone
                };

                pview.Solution=  new Solution
                {
                    SetupLabel = GetTranslation("pageTitle", "SolutionSummary"),
                    NameLabel = GetTranslation("solutionSection", "SolutionName"),
                    Name = solutionDto.SolutionName,
                    SpecRefLabel = GetTranslation("solutionSection", "SpecReference") ,
                    SpecRef = solutionDto.SpecRefNumber,
                    DescriptionLabel = GetTranslation("acLoad", "Description"),
                    Description = solutionDto.Description,
                };
                pview.Solution.Environment= new SummaryReportDto.Environment
                {
                    TitleLabel = GetTranslation("report", "Environment"),
                    AmbientTempLabel = GetTranslation("solutionSetupDetail", "AmbientTemperature") ,
                    AmbientTemp = solutionDto.SolutionSetup.FirstOrDefault().AmbientTemperature.Description,
                    ElevationLabel = GetTranslation("solutionSetupDetail", "Elevation") ,
                    Elevation = solutionDto.SolutionSetup.FirstOrDefault().Elevation.Description
                };

                pview.Solution.LoadSeqConf = new LoadSeqConf
                {
                    TitleLabel = GetTranslation("solutionSetupDetail", "LoadSequenceConfiguration"),
                    Cyclic1Label = GetTranslation("solutionSetupDetail", "Cyclic") +" #1",
                    Cyclic1 = solutionDto.SolutionSetup.FirstOrDefault().LoadSequenceCyclic1.Description,
                    Cyclic2Label = GetTranslation("solutionSetupDetail", "Cyclic") + " #2",
                    Cyclic2 = solutionDto.SolutionSetup.FirstOrDefault().LoadSequenceCyclic2.Description
                };


                pview.Solution.ElectricalConf = new ElectricalConf
                {
                    TitleLabel = GetTranslation("report", "ElectricalConfiguration") ,
                    PhaseLabel = GetTranslation("solutionSetupDetail", "Phase"),
                    Phase = solutionDto.SolutionSetup.FirstOrDefault().VoltagePhase.Description,
                    FrequencyLabel = GetTranslation("solutionSetupDetail", "Frequency"),
                    Frequency = solutionDto.SolutionSetup.FirstOrDefault().Frequency.Description,
                    VoltageNorminal = solutionDto.SolutionSetup.FirstOrDefault().VoltageNominal.Description,
                    VoltageNorminalLabel = GetTranslation("solutionSetupDetail", "VoltageN"),
                    VoltageSpecificLabel = GetTranslation("solutionSetupDetail", "VoltageS"),
                    VoltageSpecific = solutionDto.SolutionSetup.FirstOrDefault().VoltageSpecific.Description,
                };

                pview.Solution.Units = new Units
                {
                    TitleLabel = GetTranslation("solutionSetupDetail", "Units"),
                    UnitsLabel = GetTranslation("solutionSetupDetail", "Units"),
                    UnitsValue = solutionDto.SolutionSetup.FirstOrDefault().Units.Description
                };

                pview.Solution.MaxAllowableTransients = new MaxAllowableTransients
                {
                    TitleLabel = GetTranslation("acLoad", "MaximumAllowableTransients"),
                    MaximumRunningLoadLabel = GetTranslation("solutionSetupDetail", "MaximumRunningLoad"),
                    MaximumRunningLoad = solutionDto.SolutionSetup.FirstOrDefault().MaxRunningLoad.Description,
                    VoltageDipLabel = GetTranslation("solutionSetupDetail", "VoltageDip"),
                    VoltageDip = solutionDto.SolutionSetup.FirstOrDefault().VoltageDip.Description,
                    FrequencyDipLabel = GetTranslation("solutionSetupDetail", "FrequencyDip"),
                    FrequencyDip = solutionDto.SolutionSetup.FirstOrDefault().FrequencyDip.Description
                };

                pview.Solution.MaxAllowableVoltageDistortion = new MaxAllowableVoltageDistortion
                {
                    TitleLabel = GetTranslation("solutionSetupDetail", "MaxAllowableVoltageToolTip"),
                    ContinuousLabel = GetTranslation("solutionSetupDetail", "Continuous"),
                    Continuous = solutionDto.SolutionSetup.FirstOrDefault().ContinuousAllowableVoltageDistortion.Description,
                    MomentaryLabel = GetTranslation("solutionSetupDetail", "Momentary"),
                    Momentary = solutionDto.SolutionSetup.FirstOrDefault().MomentaryAllowableVoltageDistortion.Description
                };


                pview.Solution.Engine = new Engine
                {
                    TitleLabel = GetTranslation("solutionSetupDetail", "Engine"),
                    DutyLabel = GetTranslation("solutionSetupDetail", "Duty"),
                    Duty = solutionDto.SolutionSetup.FirstOrDefault().EngineDuty.Description,
                    FuelLabel = GetTranslation("solutionSetupDetail", "Fuel"),
                    Fuel = solutionDto.SolutionSetup.FirstOrDefault().FuelTank.Description
                };

                pview.Solution.RegulatoryInformation = new RegulatoryInformation
                {
                    TitleLabel = GetTranslation("solutionSetupDetail", "RegulatoryInformation"),
                    RegulatoryFiltersLabel = GetTranslation("solutionSetupDetail", "RegulatoryFilters"),
                    RegulatoryFilters = GetTranslation(solutionDto.SolutionSetup.FirstOrDefault().RegulatoryFilter.Split(':').LastOrDefault().Split('.')[0], solutionDto.SolutionSetup.FirstOrDefault().RegulatoryFilter.Split(':').LastOrDefault().Split('.').Length>1? solutionDto.SolutionSetup.FirstOrDefault().RegulatoryFilter.Split(':').LastOrDefault().Split('.')[1]:""),
                    ApplicationLabel = (string)translatedData["solutionSetupDetail"]["Application"],
                    Application = solutionDto.SolutionSetup.FirstOrDefault().SolutionApplication.Description
                };

                pview.Solution.GeneratorConfiguration = new GeneratorConfiguration
                {
                    TitleLabel = GetTranslation("report", "GeneratorConfiguration"),
                    SoundLabel = GetTranslation("solutionSetupDetail", "soundDesired"),
                    Sound = solutionDto.SolutionSetup.FirstOrDefault().DesiredSound.Description,
                    FuelTankLabel = GetTranslation("solutionSetupDetail", "FuelTank"),
                    FuelTank = solutionDto.SolutionSetup.FirstOrDefault().FuelTank.Description,
                    RunTimeLabel = GetTranslation("solutionSetupDetail", "RuntimeDesired"),
                    RunTime = solutionDto.SolutionSetup.FirstOrDefault().DesiredRunTime.Description
                };

               
            }
            return pview;
        } 

        /// <summary>
        /// Below method is used to set the Load related data in PDF reports
        /// </summary>
        /// <param name="reportModel"></param>
        /// <param name="pDPView"></param>
        /// <param name="solutionDto"></param>
        /// <returns></returns>
        private PDPView GetGensetLoadSummary(ReportModel reportModel ,PDPView pDPView, Data.Models.Solution solutionDto)
        {
            var loadSummary= _solutionSummary.GetLoadSummaryLoads(reportModel.ProjectId, reportModel.SolutionId, reportModel.UserID, reportModel.UserName);
            var solutionSummary = _solutionSummary.GetSolutionSummary(reportModel.ProjectId, reportModel.SolutionId, reportModel.UserID, reportModel.brand, reportModel.UserName);
            List<LoadItem> loaditems = new List<LoadItem>(); 
            foreach (var loadItem in loadSummary.ListOfLoadSummaryLoadList)
            {
                foreach (var item in loadItem.LoadSequenceList)
                {
                    loaditems.Add(new LoadItem
                    {
                        ItemType = LoadItemType.Group.ToString(),
                        GroupStepLabel = "",
                        Sequence = GetTranslation(loadItem.SequenceDescription.Split('.')[0], loadItem.SequenceDescription.Split('.').Length > 1 ? loadItem.SequenceDescription.Split('.')[1] : ""),
                        ItemID = item.LoadID,
                        ItemDesc = GetTranslation(item.LanguageKey.Split('.')[0],item.LanguageKey.Split('.').Length>1? item.LanguageKey.Split('.')[1]:""),
                        ItemKVA = item.KVAPFDescription,
                        ItemPF = "",
                        ItemHarmonics = item.HarmonicsDescription,
                        StartingkW = Convert.ToDouble(item.StartingKW),
                        StartingkVA = Convert.ToDouble(item.StartingKVA),
                        RunningkW = Convert.ToDouble(item.RunningKW),
                        RunningkVA = Convert.ToDouble(item.RunningKVA),
                        HoarmonicCurrentDistortionPeak = Convert.ToDouble(item.THIDMomentary),
                        HoarmonicCurrentDistortionCont = Convert.ToDouble(item.THIDContinuous),
                        HoarmonicCurrentDistortionkVA = Convert.ToDouble(item.THIDKva),
                        LimitsVdip = item.LimitsVdip,
                        LimitsFdip = item.LimitsFdip,
                        isSummaryRow =  0,
                        // SequencePeak = loadItem.LoadSequenceSummary.SequencePeakText,
                        ApplicationPeak = Convert.ToDouble(loadItem.LoadSequenceSummary.ApplicationPeak),
                        SequencePeakLimitVdip = Convert.ToDouble(loadItem.LoadSequenceSummary.VDipPerc),
                        SequencePeakLimitFdip = Convert.ToDouble(loadItem.LoadSequenceSummary.FDipPerc),
                        ApplicationPeakLimitVdip = Convert.ToDouble(loadItem.LoadSequenceSummary.VDipVolts),
                        ApplicationPeakLimitFdip = Convert.ToDouble(loadItem.LoadSequenceSummary.FDipHertz)

                    });
                     
                }

                loaditems.Add(new LoadItem
                {
                    ItemType = LoadItemType.Group.ToString(),
                    GroupStepLabel = "",
                    Sequence =GetTranslation(loadItem.SequenceDescription.Split('.')[0], loadItem.SequenceDescription.Split('.').Length>1 ? loadItem.SequenceDescription.Split('.')[1]:""),
                    ItemID = 1,
                    ItemDesc = GetTranslation(loadItem.LoadSequenceSummary.LoadFactorDescription,""),
                    ItemKVA = loadItem.LoadSequenceSummary.KVABaseErrorChecked.ToString(),
                    ItemPF = "",
                    ItemHarmonics ="",
                    StartingkW = Convert.ToDouble(loadItem.LoadSequenceSummary.StartingKW),
                    StartingkVA = Convert.ToDouble(loadItem.LoadSequenceSummary.StartingKVA),
                    RunningkW = Convert.ToDouble(loadItem.LoadSequenceSummary.RunningKW),
                    RunningkVA = Convert.ToDouble(loadItem.LoadSequenceSummary.RunningKVA),
                    HoarmonicCurrentDistortionPeak = Convert.ToDouble(loadItem.LoadSequenceSummary.THIDMomentary),
                    HoarmonicCurrentDistortionCont = Convert.ToDouble(loadItem.LoadSequenceSummary.THIDContinuous),
                    HoarmonicCurrentDistortionkVA = Convert.ToDouble(loadItem.LoadSequenceSummary.THIDKva),
                    LimitsVdip = loadItem.LoadSequenceSummary.VDipVolts.ToString(),
                    LimitsFdip = loadItem.LoadSequenceSummary.FDipHertz.ToString(),
                    isSummaryRow = 1,
                     SequencePeak = loadItem.LoadSequenceSummary.SequencePeakText,
                    ApplicationPeak = Convert.ToDouble(loadItem.LoadSequenceSummary.ApplicationPeak),
                    SequencePeakLimitVdip = Convert.ToDouble(loadItem.LoadSequenceSummary.VDipPerc),
                    SequencePeakLimitFdip = Convert.ToDouble(loadItem.LoadSequenceSummary.FDipPerc)

                });
            }
            pDPView.GensetLoadSummary = new GensetLoadSummary
            {
                 TitleLabel = GetTranslation("report","GeneratorAndLoadSummary"),
                 LoadSummary = new LoadSummary
                {
                      HarmonicskVA =Convert.ToDouble(loadSummary.SolutionSummaryLoadSummary.StepKVA),
                      HarmonicsTHIDCont = Convert.ToDouble(loadSummary.SolutionSummaryLoadSummary.THIDContinuous),
                      HarmonicsTHIDPeak = Convert.ToDouble(loadSummary.SolutionSummaryLoadSummary.THIDPeak),
                      RunningkVA= Convert.ToDouble(loadSummary.SolutionSummaryLoadSummary.RunningKVA),
                      RunningKW= Convert.ToDouble(loadSummary.SolutionSummaryLoadSummary.PeakKW),
                      RunningPF = Convert.ToDouble(loadSummary.SolutionSummaryLoadSummary.RunningPF),
                      TransientsLkVAStep = Convert.ToDouble(loadSummary.SolutionSummaryLoadSummary.StepKVA),
                      TransientsLkWPeak= Convert.ToDouble(loadSummary.SolutionSummaryLoadSummary.PeakKW),
                      TransientsLkWStep= Convert.ToDouble(loadSummary.SolutionSummaryLoadSummary.StepKW),
                        TitleLabel = GetTranslation("report", "LoadSummaryConnected"),
                        RunningLabel = GetTranslation("solutionSummary", "Running"),
                        TransientsLabel = GetTranslation("solutionSummary", "Transients"),
                        HarmonicsLabel = GetTranslation("solutionSummary", "Harmonics"),
                        RunningKWLabel = "kW",
                        RunningkVALabel = "kVA",
                        RunningPFLabel = "PF",
                        TransientsLkWStepLabel = "kW ("+GetTranslation("report", "Step")+")",
                        TransientsLkWPeakLabel = "kW ("+ GetTranslation("report", "Peak")+")",
                        TransientsLkVAStepLabel = "kVA (" + GetTranslation("report", "Step") + ")",
                        HarmonicskVALabel = "kVA",
                        HarmonicsTHIDContLabel = "THID Cont",
                        HarmonicsTHIDPeakLabel = "THID "+ GetTranslation("report", "Peak")
                 },
                 GensetAlternator= new GensetAlternator
                 {
                     TitleLabel1 =string.Join("",solutionSummary.SolutionSummaryRecommendedProductDetails.Description.Split('|').ToList().Select(x=>GetTranslation(x,""))),
                     TitleLabel2 = string.Join("", solutionSummary.SolutionSummaryRecommendedProductDetails.DescriptionPartwo.Split('|').ToList().Select(x => GetTranslation(x, ""))),
                     LoadLevelLabel =GetTranslation("loadUPS","LoadLevel"),
                     TransientsLabel =GetTranslation("solutionSummary","Transients"),
                     HarmonicsLabel = GetTranslation("solutionSummary", "Harmonics"),
                     LoadLevelRunningLabel = GetTranslation("solutionSummary", "Running"),
                     LoadLevelRunning =Convert.ToDouble(solutionSummary.SolutionSummaryRecommendedProductDetails.RunningKW),
                     LoadLevelPeakLabel = GetTranslation("report","Peak"),
                     LoadLevelPeak = Convert.ToDouble(solutionSummary.SolutionSummaryRecommendedProductDetails.PeakKW),
                     TransientsFdipLabel = "Fdip (Hz)",
                     TransientsFdip = Convert.ToDouble(solutionSummary.SolutionSummaryRecommendedProductDetails.FDip),
                     TransientsVdipLabel = "Vdip (%)",
                     TransientsVdip = Convert.ToDouble(solutionSummary.SolutionSummaryRecommendedProductDetails.VDip),
                     HarmonicsTHVDContLabel = "THVD Cont",
                     HarmonicsTHVDCont = (solutionSummary.SolutionSummaryRecommendedProductDetails.THVDContinuous),
                     HarmonicsTHVDPeakLabel = "THVD "+ GetTranslation("report", "Peak"),
                     HarmonicsTHVDPeak = solutionSummary.SolutionSummaryRecommendedProductDetails.THVDPeak,
                     LimitFdip= Convert.ToDouble(solutionSummary.SolutionLimits.FDip),
                     LimitFdipLabel= "Fdip (Hz)",
                     LimitMaxLoading= Convert.ToDouble(solutionSummary.SolutionLimits.MaxLoading),
                     LimitMaxLoadingLabel=GetTranslation("report", "MaxLoading"),
                     LimitProjectLabel =GetTranslation("report", "ProjectLimits"),
                     LimitTHVDCont = (solutionSummary.SolutionLimits.THVDContinuous),
                     LimitTHVDContLabel ="THVD Cont",
                     LimitTHVDPeak= solutionSummary.SolutionLimits.THVDPeak,
                     LimitTHVDPeakLabel= "THVD "+ GetTranslation("report", "Peak"),
                     LimitVdip= Convert.ToDouble(solutionSummary.SolutionLimits.VDip),
                     LimitVdipLabel = "Vdip (%)"
                 }
                ,SelectedGeneratorAlternator = new SelectedGeneratorAlternator
                {
                    TitleLabel = GetTranslation("report", "SelectedGenerator"),
                    ProductFamilyMethodLabel = GetTranslation("report", "ProductFamilyMethod"),
                    ProductFamilyMethod =solutionDto.RecommendedProduct.FirstOrDefault().FamilySelectionMethod == null? "":solutionDto.RecommendedProduct.FirstOrDefault().FamilySelectionMethod.Description,
                    ProductFamilyLabel = GetTranslation("solutionSummary", "ProductFamily"),
                    ProductFamily =solutionDto.RecommendedProduct.FirstOrDefault().ProductFamily==null?GetTranslation("report", "NoSolutionAvailableChange"):solutionDto.RecommendedProduct.FirstOrDefault().ProductFamily.Description,
                    ModuleSizeLabel = GetTranslation("report", "ModuleSize"),
                    ModuleSize = "NA",
                    SizingMethodLabel = GetTranslation("solutionSummary", "SizingMethod"),
                    SizingMethod =solutionDto.RecommendedProduct.FirstOrDefault().SizingMethod==null?"":solutionDto.RecommendedProduct.FirstOrDefault().SizingMethod.Description,
                    GeneratorLabel = GetTranslation("solutionSummary", "Generator"),
                    Generator =solutionDto.RecommendedProduct.FirstOrDefault().Generator==null?"":solutionDto.RecommendedProduct.FirstOrDefault().Generator.ModelDescription,
                    AlternatorLabel = GetTranslation("solutionSummary", "Alternator"),
                    Alternator =solutionDto.RecommendedProduct.FirstOrDefault().Alternator==null?"":solutionDto.RecommendedProduct.FirstOrDefault().Alternator.ModelDescription
                    
                }
                ,LoadList = new LoadList
                {   LoadItemList = loaditems,
                    TitleLabel = GetTranslation("report", "LoadList"),
                    SequenceHeaderColumnLabel = GetTranslation("transientAnalysis", "Sequence"),
                    DescriptionHeaderColumnLabel = GetTranslation("acLoad", "Description"),
                    StartingHeaderColumnLabel = GetTranslation("transientAnalysis", "Starting"),
                    StartingkWHeaderColumnLabel = "kW",
                    StartingkVAHeaderColumnLabel = "kVA",
                    RunningHeaderColumnLabel = GetTranslation("loadCharacteristics", "Running"),
                    RunningHeaderkWColumnLabel = "kW",
                    RunningHeaderkVAColumnLabel = "kVA",
                    HarmonicCurrentDistortionHeaderColumnLabel = GetTranslation("loadCharacteristics", "HarmonicCurrentDistortion"),
                    HarmonicCurrentDistortionPeakHeaderColumnLabel = GetTranslation("report", "Peak"),
                    HarmonicCurrentDistortionContHeaderColumnLabel = "Cont.",
                    HarmonicCurrentDistortionkVAHeaderColumnLabel = "kVA",
                    LimitsHeaderColumnLabel = GetTranslation("loadCharacteristics","Limits"),
                    LimitsVdipHeaderColumnLabel = "Vdip",
                    LimitsFdipHeaderColumnLabel = "Fdip",
                    SummaryLabel = GetTranslation("report", "Summary"),
                    SummaryAllLoadsLabel = GetTranslation("report", "AllLoadsOnSequence"),
                    SequencePeakLabel = "kW "+ GetTranslation("loadCharacteristics", "SequencePeak"),
                    ApplicationPeakLabel = "kW " + GetTranslation("loadCharacteristics", "ApplicationPeak"),
                }
          }; 
            
            return pDPView;
        }

        /// <summary>
        /// Below method is used to set the data for Transient analysis for
        /// PDF report
        /// </summary>
        /// <param name="reportModel"></param>
        /// <param name="pDPView"></param>
        /// <returns></returns>
        private PDPView GetTransientAnalysis(ReportModel reportModel ,PDPView pDPView, Data.Models.Solution solutionDto)
        {
            var transientAnalysis = _solutionSummary.GetTransientAnalysis(reportModel.ProjectId, reportModel.SolutionId, reportModel.UserID, reportModel.brand, reportModel.UserName);
            if(transientAnalysis!=null)
            {
                #region Transient Analysis
                pDPView.TransientAnalysis = new TransientAnalysis
                {
                    TitleLabel = GetTranslation("tab", "TransientAnalysis"),
                    Note = GetTranslation("transientAnalysis", "Note")

                };

                #region Alternator Transient Analysis List
                List<AnalysisItem> analysisList = new List<AnalysisItem>();
                foreach(var item in transientAnalysis.AlternatorTransientAnalysisList)
                {
                    analysisList.Add( new  AnalysisItem
                    {
                        AllowableVdip =item.AllowableVdip,
                        ExpectedVdip  =  item.VdipExpected,
                        SequenceVdip   =  Convert.ToDouble(item.SequenceStartingkVA),
                        LargestTransientLoad=item.LargestTransientLoad,
                        Sequence =  GetTranslation(item.Sequence.Split('.')[0], item.Sequence.Split('.').Length > 1 ? item.Sequence.Split('.')[1] : ""),
                        ItemType = LoadItemType.Group.ToString(),
                        GroupStepLabel =GetTranslation("transientAnalysis", "Group")
                    }
                    );

                }
                #endregion
                #region Engine Anaylysis
                List<AnalysisItem> engineAnalysisList = new List<AnalysisItem>();
                foreach (var item in transientAnalysis.EngineTransientAnalysisList)
                {
                    engineAnalysisList.Add(new AnalysisItem
                    {
                        AllowableVdip = item.AllowableFdip.ToString()+" %",
                        ExpectedVdip =  item.FdipExpected.ToString() + " %",
                        SequenceVdip = Convert.ToDouble(item.SequenceStartingkW),
                        LargestTransientLoad = item.LargestTransientLoad,
                        Sequence = GetTranslation(item.Sequence.Split('.')[0], item.Sequence.Split('.').Length > 1 ? item.Sequence.Split('.')[1] : ""),
                        ItemType = LoadItemType.Group.ToString(),
                        GroupStepLabel = GetTranslation("transientAnalysis", "Sequence")
                    }
                    );

                }
                #endregion

                //populating Alternator Transient Requirement
               pDPView.TransientAnalysis.AlternatorTransientReq = new AlternatorTransientReq
               {
                   TitleLabel = GetTranslation("transientAnalysis", "MostDifficultAlternator"),
                   SequenceLabel = GetTranslation("transientAnalysis", "Sequence"),
                   Sequence = GetTranslation(transientAnalysis.AlternatorTransientRequirement.Sequence.Split('.')[0],transientAnalysis.AlternatorTransientRequirement.Sequence.Split('.').Length>1? transientAnalysis.AlternatorTransientRequirement.Sequence.Split('.')[1]:""),
                   LoadLabel = GetTranslation("transientAnalysis", "Load") ,
                   Load = transientAnalysis.AlternatorTransientRequirement.Load,
                   StartingKVALabel = GetTranslation("transientAnalysis", "Starting") +" kVA",
                   StartingKVA =Convert.ToDouble(transientAnalysis.AlternatorTransientRequirement.StartingkVA),
                   VdipToleranceLabel = GetTranslation("transientAnalysis", "VdipTolerance"),
                   VdipTolerance = transientAnalysis.AlternatorTransientRequirement.VdipTolerance,
                   VdipExpectedLabel = GetTranslation("transientAnalysis", "VdipExpected"),
                   VdipExpected = transientAnalysis.AlternatorTransientRequirement.VdipExpected
               };

                //populating Engine Transient Requirement
                pDPView.TransientAnalysis.EngineTransientReq= new EngineTransientReq
                {
                    TitleLabel = GetTranslation("transientAnalysis", "MostDifficultAlternator"),
                    SequenceLabel = GetTranslation("transientAnalysis", "Sequence"),
                    Sequence = GetTranslation(transientAnalysis.EngineTransientRequirement.Sequence.Split('.')[0], transientAnalysis.EngineTransientRequirement.Sequence.Split('.').Length>1? transientAnalysis.EngineTransientRequirement.Sequence.Split('.')[1]:""),
                    LoadLabel = GetTranslation("transientAnalysis", "Load"),
                    Load = transientAnalysis.EngineTransientRequirement.Load,
                    StartingKVALabel = GetTranslation("transientAnalysis", "Starting") + " kVA",
                    StartingKVA = Convert.ToDouble(transientAnalysis.EngineTransientRequirement.StartingkW),
                    VdipToleranceLabel = GetTranslation("transientAnalysis", "VdipTolerance"),
                    VdipTolerance = Convert.ToDouble(transientAnalysis.EngineTransientRequirement.FdipTolerance),
                    VdipExpectedLabel = GetTranslation("transientAnalysis", "VdipExpected"),
                    VdipExpected = Convert.ToDouble(transientAnalysis.EngineTransientRequirement.FdipExpected)
                };

                //populating Alternator Transient Analysis
                pDPView.TransientAnalysis.AlternatorTransientAnalysis= new AnalysisList
                {
                    TitleLabel =GetTranslation("transientAnalysis", "AlternatorTransientAnalysis")+ "(Vdip)",
                    SequenceColumnLabel = GetTranslation("transientAnalysis", "Sequence"),
                    AllowableVdipColumnLabel = GetTranslation("transientAnalysis", "AllowableVdip"),
                    ExpectedVdipColumnLabel = GetTranslation("transientAnalysis", "ExpectedVdip"),
                    SequenceStartingKVAColumnLabel = GetTranslation("transientAnalysis", "SequenceStarting") +" kVA",
                    LargestTransientLoadColumnLabel = GetTranslation("transientAnalysis", "LargestTransientLoad"),
                     AnalysisItemList=  analysisList
                      
                };

                //populating Engine Transient Analysis
                pDPView.TransientAnalysis.EnginTransientAnalysis= new AnalysisList
                {
                    TitleLabel = GetTranslation("transientAnalysis", "EngineTransientAnalysis") +" (Fdip)",
                    SequenceColumnLabel = GetTranslation("transientAnalysis", "Sequence"),
                    AllowableVdipColumnLabel = GetTranslation("transientAnalysis", "AllowableVdip"),
                    ExpectedVdipColumnLabel = GetTranslation("transientAnalysis", "ExpectedVdip"),
                    SequenceStartingKVAColumnLabel = GetTranslation("transientAnalysis", "SequenceStarting") + " kVA",
                    LargestTransientLoadColumnLabel = GetTranslation("transientAnalysis", "LargestTransientLoad"),
                     AnalysisItemList =analysisList
                };
            #endregion

            }else
            {
                pDPView.TransientAnalysis = new TransientAnalysis
                {
                    TitleLabel =GetTranslation("tab","TransientAnalysis"),
                    Note = GetTranslation("warning", "NosolutionForAnalysis")

                };
            }

            return pDPView;
        }
        /// <summary>
        /// Below method  is used to set the data for Gas Piping for PDF report
        /// </summary>
        /// <param name="reportModel"></param>
        /// <param name="pDPView"></param>
        /// <returns></returns>
        private PDPView GetGasPipingDataForReport(ReportModel reportModel,PDPView pDPView, Data.Models.Solution solutionDto)
        {
            var gaspipingData = _solutionSummary.GetGasPipingDetails(reportModel.ProjectId, reportModel.SolutionId);
            if (gaspipingData != null)
            {
                if (solutionDto.SolutionSetup.FirstOrDefault().FuelTypeID != (int)FuelTypeEnum.NaturalGas

                && solutionDto.SolutionSetup.FirstOrDefault().FuelTypeID != (int)FuelTypeEnum.LPVapor

                && solutionDto.SolutionSetup.FirstOrDefault().FuelTypeID != (int)FuelTypeEnum.DualFuelVapor

                && solutionDto.SolutionSetup.FirstOrDefault().FuelTypeID != (int)FuelTypeEnum.BiFuel)
                {
                 GasPiping gasPiping = new GasPiping
                    {
                        TitleLabel =GetTranslation("tab","GasPiping"),
                        Note = GetTranslation("gasPiping", "GasfuelFuelCondition")
                    };
                    pDPView.GasPiping = gasPiping;
                }else
                {
                    #region ## Gas Piping ##
                    GasPiping gasPiping = new GasPiping
                    {
                        TitleLabel = GetTranslation("tab", "GasPiping"),
                        Note = GetTranslation("gasPipingReport", "Pipingpressuredropcalculation")
                    };

                    GeneratorSummary generatorSummary = new GeneratorSummary
                    {
                        TitleLabel = GetTranslation("gasPipingReport", "GeneratorSummary"),
                        SigingMethodLabel = GetTranslation("gasPipingReport", "SizingMethod"),
                        SigingMethod = gaspipingData.SizingMethodList.Where(x => x.ID == gaspipingData.SizingMethodID).FirstOrDefault().Description,
                        PipeSizeLabel = GetTranslation("gasPipingReport", "PipeSize"),
                        PipeSize = Convert.ToDouble(gaspipingData.PipeSizeList.Where(x => x.ID == gaspipingData.PipeSizeID).FirstOrDefault().PipeSize),
                        ProductFamilyLabel = GetTranslation("gasPipingReport", "ProductFamily"),
                        ProductFamily = gaspipingData.GeneratorSummary.ProductFamily,
                        GeneratorLabel = GetTranslation("Generator", ""),
                        Generator = gaspipingData.GeneratorSummary.Generator,
                        FuelTypeLabel = GetTranslation("gasPipingReport", "FuelType"),
                        FuelType = gaspipingData.GeneratorSummary.FuelType,
                        FuelConsumptionLabel = GetTranslation("gasPipingReport", "FuelConsumption") + " " + GetTranslation("gasPipingReport", "ThemeHour"),
                        FuelConsumption = Convert.ToDouble(gaspipingData.GeneratorSummary.FuelConsumption),
                        MinimumPressureLabel = GetTranslation("gasPipingReport", "MinimumPressure") + " " + GetTranslation("gasPipingReport", "inchesOfWater"),
                        MinimumPressure = gaspipingData.GeneratorSummary.Quantity,// need confirmation with pradeep
                        SummaryNote = gaspipingData.SingleUnit ? GetTranslation("gasPipingReport", "GasFlowPipeInfo") : "",
                    };

                    GasPipingSolution gasPipingSolution = new GasPipingSolution
                    {
                        TitleLabel = GetTranslation("gasPipingReport", "Solution"),
                        PressureDropLabel = GetTranslation("gasPipingReport", "PressureDrop") + " " + GetTranslation("gasPipingReport", "inchesOfWater"),
                        PressureDrop = Convert.ToDouble(gaspipingData.GasPipingSolution.PressureDrop),
                        PercentageAllowableLabel = "% " + GetTranslation("gasPipingReport", "ofAllowable"),
                        PercentageAllowable = Convert.ToDouble(gaspipingData.GasPipingSolution.AllowablePercentage),
                        AvailablePressureLabel = GetTranslation("gasPipingReport", "AvailablePressure") + " " + GetTranslation("gasPipingReport", "inchesOfWater"),
                        AvailablePressure = Convert.ToDouble(gaspipingData.GasPipingSolution.AvailablePressure)
                    };

                    GasPipingInput gasPipingInput = new GasPipingInput
                    {
                        TitleLabel = GetTranslation("gasPipingReport", "Inputs"),
                        SupplyGasPressureLabel = GetTranslation("gasPipingReport", "SupplyGasPressure") + " " + GetTranslation("gasPipingReport", "inchesOfWater"),
                        SupplyGasPressure = Convert.ToDouble(gaspipingData.GasPipingInput.SupplyGasPressure),
                        LenghOfRunLabel = GetTranslation("gasPipingReport", "Lengthofrun") + " (ft)",
                        LenghOfRun = Convert.ToDouble(gaspipingData.GasPipingInput.LengthOfRun),
                        NumberOf90ElbowsLabel = GetTranslation("gasPipingReport", "Numberof90elbows"),
                        NumberOf90Elbows = Convert.ToDouble(gaspipingData.GasPipingInput.NumberOf90Elbows),
                        Numberof45ElblowsLabel = GetTranslation("gasPipingReport", "Numberof45elbows"),
                        Numberof45Elblows = Convert.ToDouble(gaspipingData.GasPipingInput.NumberOf45Elbows),
                        NumberOfTeesLabel = GetTranslation("gasPipingReport", "NumberofTees"),
                        NumberOfTees = Convert.ToDouble(gaspipingData.GasPipingInput.NumberOfTees)
                    };

                    gasPiping.GeneratorSummary = generatorSummary;
                    gasPiping.GasPipingSolution = gasPipingSolution;
                    gasPiping.GasPipingInput = gasPipingInput;
                    pDPView.GasPiping = gasPiping;
                    #endregion
                }
            }
            else
            {
                GasPiping gasPiping = new GasPiping
                {
                    TitleLabel = GetTranslation("tab", "GasPiping"),
                    Note = GetTranslation("warning", "NosolutionForAnalysis")
                };
                pDPView.GasPiping = gasPiping;
            }
            return pDPView;
        }

        /// <summary>
        /// Below method is used to set the data for ExhaustPiping for PDF report
        /// </summary>
        /// <param name="reportModel"></param>
        /// <param name="pView"></param>
        /// <returns></returns>
        private PDPView GetExhaustPipingReport(ReportModel reportModel, PDPView pView)
        {
            var exhaustDetails= _solutionSummary.GetExhaustPipingDetails(reportModel.ProjectId,reportModel.SolutionId);
            if (exhaustDetails != null)
            {
                #region ## Exhautst Piping ##
                ExhaustPiping exhaustPiping = new ExhaustPiping
                {
                    TitleLabel = GetTranslation("tab", "ExhaustPiping"),
                    Note = GetTranslation("gasPipingReport", "PipingpressuredropcalculationReport")
                };

                ExhaustGeneratorSummary exhaustGeneratorSummary = new ExhaustGeneratorSummary
                {
                    TitleLabel = GetTranslation("gasPipingReport", "GeneratorSummary"),
                    SigingMethodLabel = GetTranslation("gasPipingReport", "SizingMethod"),
                    SigingMethod = exhaustDetails.SizingMethodList == null ? "" : exhaustDetails.SizingMethodList.Where(x => x.ID == exhaustDetails.SizingMethodID).FirstOrDefault().Description,
                    PipeSizeLabel = GetTranslation("gasPipingReport", "PipeSize"),
                    PipeSize = exhaustDetails.PipeSizeList == null ? "" : exhaustDetails.PipeSizeList.Where(x => x.ID == exhaustDetails.PipeSizeID).FirstOrDefault().Description,
                    ProductFamilyLabel = GetTranslation("gasPipingReport", "ProductFamily"),
                    ProductFamily = GetTranslation(exhaustDetails.ExhaustPipingGeneratorSummary.ProductFamily.Split('.')[0],exhaustDetails.ExhaustPipingGeneratorSummary.ProductFamily.Split('.').Length>1? exhaustDetails.ExhaustPipingGeneratorSummary.ProductFamily.Split('.')[1]:""),
                    GeneratorLabel = GetTranslation("gasPipingReport", "Generator"),
                    Generator = exhaustDetails.ExhaustPipingGeneratorSummary.Generator,
                    TotalExahustFlowLabel = GetTranslation("gasPipingReport", "TotalExahustFlow") +" (ft3 / Min)",
                    TotalExahustFlow = Convert.ToDouble(exhaustDetails.ExhaustPipingGeneratorSummary.TotalExhaustFlow),
                    MaxBackPressureLabel = GetTranslation("exhaustPiping", "MaximumBackPressure") + GetTranslation("gasPipingReport", "inchesOfWater"),
                    MaxBackPressure = Convert.ToDouble(exhaustDetails.ExhaustPipingGeneratorSummary.MaximumBackPressure)
                };

                ExhaustSolution exhaustSolution = new ExhaustSolution
                {
                    TitleLabel = GetTranslation("gasPipingReport", "Solution"),
                    PressureDropLabel = GetTranslation("gasPipingReport", "PressureDrop") + " " + GetTranslation("gasPipingReport", "inchesOfWater"),
                    PressureDrop = Convert.ToDouble(exhaustDetails.ExhaustPipingSolution.PressureDrop)
                };

                ExhausPipingInput exhausPipingInput = new ExhausPipingInput
                {
                    TitleLabel = GetTranslation("exhaustPiping", "Inputs"),
                    LenghOfRunLabel = GetTranslation("gasPipingReport", "Lengthofrun") + " (ft)",
                    LenghOfRun = Convert.ToDouble(exhaustDetails.ExhaustPipingInput.LengthOfRun),
                    NumberOfStandardElbowsLabel = GetTranslation("exhaustPiping", "NumberofStandardElbows"),
                    NumberOfStandardElbows = Convert.ToDouble(exhaustDetails.ExhaustPipingInput.NumberOfStandardElbows),
                    NumberOfLongLabel = GetTranslation("exhaustPiping", "NumberofLongElbows")+" (radius > 1.5 dia)",
                    NumberOfLong = Convert.ToDouble(exhaustDetails.ExhaustPipingInput.NumberOfLongElbows),
                    Numberof45ElblowsLabel = GetTranslation("gasPipingReport", "Numberof45elbows"),
                    Numberof45Elblows = Convert.ToDouble(exhaustDetails.ExhaustPipingInput.NumberOf45Elbows)
                };

                exhaustPiping.ExhaustGeneratorSummary = exhaustGeneratorSummary;
                exhaustPiping.ExhaustSolution = exhaustSolution;
                exhaustPiping.ExhausPipingInput = exhausPipingInput;
                pView.ExhaustPiping = exhaustPiping;
                #endregion
            }else
            {
                ExhaustPiping exhaustPiping = new ExhaustPiping
                {
                    TitleLabel = GetTranslation("tab", "ExhaustPiping"),
                    Note = GetTranslation("warning", "NosolutionForAnalysis")
                };
            }
            return pView;
        }
        private PDPView GetHarmonicAnalysisDataForReports(ReportModel reportModel, PDPView pDPView)
        {
            List<HarmonicAnalysis> harmonicAnalysisList = new List<HarmonicAnalysis>();
            if (reportModel.HarmonicAnalysis != null)
            {
                HarmonicAnalysis ha1 = new HarmonicAnalysis
                {
                    TitleLabel =GetTranslation("harmonicAnalysis", "HarmonicAnalysis"),
                    HarmonicProfileLabel = GetTranslation("harmonicAnalysis", "HarmonicProfile"),
                    HarmonicProfile = GetTranslation("tHarmonicProfile", "ApplicationTotalRunning"),
                    kVANonlinearLoadLabel = GetTranslation("harmonicAnalysis", "NonlinearLoad"),
                    kVANonlinearLoad = reportModel.HarmonicAnalysis.KVANonLinearLoad,
                    kVABaseLabel = GetTranslation("harmonicAnalysis", "allnonlinear"),
                    kVANBase = reportModel.HarmonicAnalysis.KVABase,
                    SequenceLabel = GetTranslation("harmonicAnalysis", "Sequence"),
                    Sequence = GetTranslation("harmonicAnalysis", "Total"),
                    THIDLabel = "THID",
                    THID = reportModel.HarmonicAnalysis.THID,
                    THVDLabel = "THVD",
                    THVD = reportModel.HarmonicAnalysis.THVD,
                    SelectedSeqHarmonicAlternatorLoadingLabel = GetTranslation("harmonicAnalysis", "SelectSequenceHarmonic"),
                    SelectedSeqHarmonicAlternatorLoading = reportModel.HarmonicAnalysis.AlternatorLoading
                };

                HarmonicCurrentAdnVoltageProfiles hcvp = new HarmonicCurrentAdnVoltageProfiles
                {
                    TitleLabel =GetTranslation("harmonicAnalysis", "SelectHarmonicCurrent"),
                    ProfileLabel = GetTranslation("harmonicAnalysis", "Profile"),
                    CurrentLabel = GetTranslation("harmonicAnalysis", "Current"),
                    VoltageLabel = GetTranslation("harmonicAnalysis", "Voltage"),

                    Current3 = Convert.ToDouble(reportModel.HarmonicAnalysis.CurrentHarmonicDistortion.HarmonicDistortion3),
                    Current5 = Convert.ToDouble(reportModel.HarmonicAnalysis.CurrentHarmonicDistortion.HarmonicDistortion5),
                    Current7 = Convert.ToDouble(reportModel.HarmonicAnalysis.CurrentHarmonicDistortion.HarmonicDistortion7),
                    Current9 = Convert.ToDouble(reportModel.HarmonicAnalysis.CurrentHarmonicDistortion.HarmonicDistortion9),
                    Current11 = Convert.ToDouble(reportModel.HarmonicAnalysis.CurrentHarmonicDistortion.HarmonicDistortion11),
                    Current13 = Convert.ToDouble(reportModel.HarmonicAnalysis.CurrentHarmonicDistortion.HarmonicDistortion13),
                    Current15 = Convert.ToDouble(reportModel.HarmonicAnalysis.CurrentHarmonicDistortion.HarmonicDistortion15),
                    Current17 = Convert.ToDouble(reportModel.HarmonicAnalysis.CurrentHarmonicDistortion.HarmonicDistortion17),
                    Current19 = Convert.ToDouble(reportModel.HarmonicAnalysis.CurrentHarmonicDistortion.HarmonicDistortion19),

                    Voltage3 = Convert.ToDouble(reportModel.HarmonicAnalysis.VoltageHarmonicDistortion.HarmonicDistortion3),
                    Voltage5 = Convert.ToDouble(reportModel.HarmonicAnalysis.VoltageHarmonicDistortion.HarmonicDistortion5),
                    Voltage7 = Convert.ToDouble(reportModel.HarmonicAnalysis.VoltageHarmonicDistortion.HarmonicDistortion7),
                    Voltage9 = Convert.ToDouble(reportModel.HarmonicAnalysis.VoltageHarmonicDistortion.HarmonicDistortion9),
                    Voltage11 = Convert.ToDouble(reportModel.HarmonicAnalysis.VoltageHarmonicDistortion.HarmonicDistortion11),
                    Voltage13 = Convert.ToDouble(reportModel.HarmonicAnalysis.VoltageHarmonicDistortion.HarmonicDistortion13),
                    Voltage15 = Convert.ToDouble(reportModel.HarmonicAnalysis.VoltageHarmonicDistortion.HarmonicDistortion15),
                    Voltage17 = Convert.ToDouble(reportModel.HarmonicAnalysis.VoltageHarmonicDistortion.HarmonicDistortion17),
                    Voltage19 = Convert.ToDouble(reportModel.HarmonicAnalysis.VoltageHarmonicDistortion.HarmonicDistortion19),
                    GraphImagePath = Base64ToImage(reportModel.HarmicAnalysisImageUrl)
                };

                ha1.HarmonicCurrentAdnVoltageProfiles = hcvp;

                harmonicAnalysisList.Add(ha1);
            }
            pDPView.HarmonicAnalysisList = harmonicAnalysisList;
            return pDPView;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="base64Url"></param>
        /// <returns></returns>
        public string Base64ToImage(string base64Url)
        {
            try
            {
                var base64Data = Regex.Match(base64Url, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;

                byte[] imageBytes = Convert.FromBase64String(base64Data);
                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                ms.Write(imageBytes, 0, imageBytes.Length);
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                _imageName = Guid.NewGuid().ToString() + ".png";
                var imageURL = System.Web.Hosting.HostingEnvironment.MapPath("~/_temp") + "/" + _imageName;
                image.Save(imageURL);
                return System.Configuration.ConfigurationManager.AppSettings["websiteURl"].ToString() + "_temp/" + _imageName;
            }catch
            {
                return "";
            }
        }


        public void DeleteImage()
        {
            try
            {
                var imageURL = System.Web.Hosting.HostingEnvironment.MapPath("~/_temp") + "/" + _imageName;
                FileInfo fileInfo = new FileInfo(imageURL);
                fileInfo.Delete();
            }catch
            {

            }
        }
        private PDPView GetData(ReportModel reportModel)
        {
            PDPView pView = new PDPView();
            var solutionDto = _projectSolution.GetSolution(reportModel.UserID, reportModel.ProjectId, reportModel.SolutionId, reportModel.UserName);
           string translationFile= new WebClient().DownloadString(System.Configuration.ConfigurationManager.AppSettings[reportModel.LanguageCode].ToString());
            translatedData = JObject.Parse(translationFile);
            #region ## Project and Solution ##
            pView = GetSolutionDetails(reportModel,pView, solutionDto);
            #endregion

            #region ## Generator and load summary ##
            pView = GetGensetLoadSummary(reportModel, pView, solutionDto);
            #endregion

            #region ## Transient Analysis ##   
            pView = GetTransientAnalysis(reportModel, pView, solutionDto);
            #endregion

            #region ## Harmonic Analysis ##
            pView = GetHarmonicAnalysisDataForReports(reportModel, pView);

            #endregion

            #region ## Gas Piping ## 
            pView = GetGasPipingDataForReport(reportModel, pView,solutionDto);

            #endregion
            #region ## Exhaust Piping ## 
            pView = GetExhaustPipingReport(reportModel, pView);
            #endregion 

            return pView;
        }  
        public MemoryStream GeneratePDF(ReportModel reportModel)
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                var templateFile = System.Web.Hosting.HostingEnvironment.MapPath("~/ReportTemplates/ProjectView.xsl");

                //string LicenseFile = @"C:\Program Files\Aspose\Aspose.Total.lic";
                string LicenseFile = HostingEnvironment.MapPath("~/App_Data/Aspose.Total.lic");
                if (System.IO.File.Exists(LicenseFile))
                {
                    License wordsLicense = new License();
                    wordsLicense.SetLicense(LicenseFile);
                }
                // get quote data
                PDPView pView = GetData(reportModel);
                XmlDocument xmlInput = TransformDataset(pView, templateFile);

                Aspose.Pdf.Generator.Pdf pdf = new Aspose.Pdf.Generator.Pdf();


                pdf.BindXML(xmlInput, null);

                pdf.Save(ms);
            }
            catch (Exception ex)
            {
                _traceMessageProcessor.WriteTrace(TraceMessaging.EventSource, TraceLevel.Error, "SolutionID: " + reportModel.SolutionId.ToString(), "GeneratePDF", ex.Message, ex.StackTrace);
                return null;
            }
            return ms;
        }
        

        public string GetTranslation(string key1,string key2)
        {
            string Firstkey = key1;
            try
            {
                int numCheck;
               
                if (string.IsNullOrEmpty(key2) && key1.Split('.').Length > 1 && !int.TryParse(key1,out numCheck)) {
                    key1 = Firstkey.Split('.')[0];
                    key2 = Firstkey.Split('.')[1];
                   }
                var translation = string.IsNullOrEmpty(key2) ? (string)translatedData[key1] : (string)translatedData[key1][key2];
                return string.IsNullOrEmpty(translation) ? key1 : translation;
            }
            catch(Exception ex)
            {

            }
            return key1 +key2;
        }

    }
}
