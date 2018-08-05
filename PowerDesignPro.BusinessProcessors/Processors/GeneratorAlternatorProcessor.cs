using PowerDesignPro.BusinessProcessors.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerDesignPro.BusinessProcessors.Dtos;
using PowerDesignPro.Data.Framework.Interface;
using PowerDesignPro.Data.Models;
using PowerDesignPro.Common;
using System.Diagnostics;

namespace PowerDesignPro.BusinessProcessors.Processors
{
    public class GeneratorAlternatorProcessor : IGeneratorAlternator
    {
        private readonly IEntityBaseRepository<Generator> _generatorRepository;
        private readonly IEntityBaseRepository<Alternator> _alternatorRepository;
        private readonly IEntityBaseRepository<SolutionSetup> _solutionSetupRepository;        

        private Generator _generator = new Generator();
        private List<Alternator> _alternators;
        private SolutionSetup _solutionSetup;
        private GeneratorDetail _generatorDetail;
        private AlternatorDetail _alternatorDetail;

        private int _quantity;
        private int _KwStandby, _KWPrime, _KWPeak;
        private int _KWNominalPrime;
        private int _KWNominalPeak;
        private int _KWNominalRated;
        private int _KWRated50;
        private decimal _Derate;
        private int _KWApplicationPeak;
        private int _KWApplicationRunning;
        private decimal _SKWFdip;

        private double _skvaMultiplier;

        public GeneratorAlternatorProcessor(
            IEntityBaseRepository<Generator> generatorRepository,
            IEntityBaseRepository<Alternator> alternatorRepository)
        {
            _generatorRepository = generatorRepository;
            _alternatorRepository = alternatorRepository;            
            // _generatorDetail = new GeneratorDetail();
            //_alternatorDetail = new AlternatorDetail();
        }

        public GeneratorDetail GetGeneratorAlternatorDetailByID(int generatorID, SolutionSetup solutionSetup, int quantity)
        {
            _solutionSetup = solutionSetup;
            _generator = _generatorRepository.GetAll(g => g.ID == generatorID).FirstOrDefault();
            _quantity = quantity;
            _KwStandby = _generator.KwStandby * quantity;
            _KWPrime = _generator.KWPrime * quantity;
            _KWPeak = _generator.KWPeak * quantity;

            _KWNominalPrime = GetKWNominalPrime();
            _KWNominalPeak = GetKWNominalPeak();
            _KWNominalRated = GetKWNominalRated();
            _KWRated50 = GetKWRated50();
            _Derate = GetDerate();
            _KWApplicationPeak = GetKWApplicationPeak();
            _KWApplicationRunning = GetKWApplicationRunning();
            _SKWFdip = GetSKWFdip();

            _generatorDetail = new GeneratorDetail
            {
                Generator = _generator,
                GeneratorID = _generator.ID,
                Quantity = _generator.IsGemini ? quantity * 2 : quantity,
                Description = $"{_generator.ModelDescription}, {_generator.Liters.ToString()} L Module",
                KWPeak = _KWPeak,
                KWPrime = _KWPrime,
                KwStandby = _KwStandby,
                KWNominalPrime = _KWNominalPrime,
                KWNominalPeak = _KWNominalPeak,
                KWNominalRated = _KWNominalRated,
                KWRated50 = _KWRated50,
                Derate = _Derate,
                KWApplicationPeak = _KWApplicationPeak,
                KWApplicationRunning = _KWApplicationRunning,
                SKWFdip = _SKWFdip,
                TransientKWFDIP_1 = TransientKWFDIP(1),
                TransientKWFDIP_2 = TransientKWFDIP(2),
                TransientKWFDIP_3 = TransientKWFDIP(3),
                TransientKWFDIP_4 = TransientKWFDIP(4),
                TransientKWFDIP_5 = TransientKWFDIP(5),
                TransientKWFDIP_6 = TransientKWFDIP(6),
                TransientKWFDIP_7 = TransientKWFDIP(7),
                TransientKWFDIP_8 = TransientKWFDIP(8),
                TransientKWFDIP_9 = TransientKWFDIP(9),
                TransientKWFDIP_10 = TransientKWFDIP(10),
                TransientKWFDIP_11 = TransientKWFDIP(11),
                TransientKWFDIP_12 = TransientKWFDIP(12),
                TransientKWFDIP_13 = TransientKWFDIP(13),
                TransientKWFDIP_14 = TransientKWFDIP(14),
                TransientKWFDIP_15 = TransientKWFDIP(15),
                AlternatorDetail = GetAlternator()
            };

            return _generatorDetail;
            //catch (Exception ex)
            //{
            //    _traceMessageProcessor.WriteTrace(TraceMessaging.EventSource, TraceLevel.Error, $"SolutionID: {_solutionSetup.SolutionID}", "GetGeneratorAlternatorDetailByID", "", ex.Message);
            //    throw ex;
            //}            
        }

        #region Private Helper Methods

        #region Generator Private Methods

        private decimal TransientKWFDIP(int hertz)
        {
            decimal[] firstSeries = { _generator.FDip50, _generator.FDip100 };
            double[] secondSeries = { _KWRated50, _KWNominalRated };
            int rounded;

            if (hertz < _generator.FDip50)
            {
                rounded = (int)((_KWRated50 / _generator.FDip50) * hertz) * 10;
                return rounded / 10;
            }
            else
            {
                rounded = (int)Statistics.lRegression(firstSeries, secondSeries, hertz) * 10;
                return rounded / 10;
            }
        }

        private int GetKWNominalPrime()
        {
            if (!_generator.PrimePowerAvailable)
                return 0;

            if (int.Parse(_solutionSetup.Frequency.Value) == 50 && int.Parse(_generator.Frequency.Value) != 50)
                return 0;
            else if (int.Parse(_solutionSetup.Frequency.Value) == 60 && int.Parse(_generator.Frequency.Value) != 60)
                return 0;
            else
                return _KWPrime;
        }

        private int GetKWNominalPeak()
        {
            if (int.Parse(_solutionSetup.Frequency.Value) == 50 && int.Parse(_generator.Frequency.Value) != 50)
                return 0;
            else if (int.Parse(_solutionSetup.Frequency.Value) == 60 && int.Parse(_generator.Frequency.Value) != 60)
                return 0;
            else
                return _KWPeak;
        }

        private int GetKWNominalRated()
        {
            if (int.Parse(_solutionSetup.Frequency.Value) == 50 && int.Parse(_generator.Frequency.Value) != 50)
                return 0;
            else if (int.Parse(_solutionSetup.Frequency.Value) == 60 && int.Parse(_generator.Frequency.Value) != 60)
                return 0;
            else
                return _KwStandby;
        }

        private int GetKWRated50()
        {
            return (int)Math.Round(_KWNominalRated * 0.5, 0, MidpointRounding.AwayFromZero);
        }

        private decimal GetDerate()
        {
            decimal elevationModifier = 0, ambientModifier = 0, projectAmbientF = 0, projectElevationFt = 0;

            projectElevationFt = decimal.Parse(_solutionSetup.Elevation.Value);
            projectAmbientF = decimal.Parse(_solutionSetup.AmbientTemperature.Value);

            if (projectElevationFt > _generator.AltitudeDeratePoint)
                elevationModifier = (decimal)(projectElevationFt - _generator.AltitudeDeratePoint) * _generator.AltitudeDeratePercent / 1000;

            if (projectAmbientF > _generator.AmbientDeratePoint)
                ambientModifier = (decimal)(projectAmbientF - _generator.AmbientDeratePoint) * _generator.AmbientDeratePercent / 10;

            return _KWNominalRated * (elevationModifier + ambientModifier);
        }

        private int GetKWApplicationPeak()
        {
            return (int)((decimal)_KWNominalPeak - _Derate);
        }

        private int GetKWApplicationRunning()
        {
            if (String.Equals(_solutionSetup.EngineDuty.Description.ToLower(), "standby"))
                return (int)((decimal)_KWNominalRated - _Derate);
            else
            {
                if (_Derate < (_KWNominalRated - _KWNominalPrime))
                    return _KWNominalPrime;
                else
                    return _KWNominalRated = (int)_Derate;
            }
        }

        private decimal GetSKWFdip()
        {
            return _generator.KwStandby / _generator.FDip50 * 0.5m;
        }

        #endregion

        #region Alternator Private Methods

        private AlternatorDetail GetAlternator()
        {
            var availableAlternators = _generator.GeneratorAvailableAlternators.Select(g => g.Alternator).ToList();
            _alternators = availableAlternators.Where(a => a.Active
                                           && a.VoltagePhaseID == _solutionSetup.VoltagePhaseID
                                           && a.FrequencyID == _solutionSetup.FrequencyID
                                           && a.VoltageNominalID == _solutionSetup.VoltageNominalID
                                           && a.KWRating >= _KwStandby / _quantity).OrderByDescending(a => a.KWRating).ToList();
            if (_alternators.Count == 0)
                return null;

            return GetAlternatorDetail(_alternators.FirstOrDefault());
            //catch (Exception ex)
            //{
            //    _traceMessageProcessor.WriteTrace(TraceMessaging.EventSource, TraceLevel.Error, $"SolutionID: {_solutionSetup.SolutionID}", "GetGeneratorAlternatorDetailByID", "", ex.Message);
            //    throw ex;
            //}

        }

        public AlternatorDetail GetAlternatorDetail(Alternator alternator)
        {
            _skvaMultiplier = GetSKVAMultiplier();

            return new AlternatorDetail
            {
                Alternator = alternator,
                AlternatorID = alternator.ID,
                SubtransientReactance1000 = GetSubtransientReactance1000(alternator),
                SubtransientReactanceCorrected = GetSubtransientReactanceCorrected(alternator),
                KWDerated = GetKWDerated(alternator),
                TransientKWVDip_10 = TransientKWVdip(alternator, 0.1m),
                TransientKWVDip_125 = TransientKWVdip(alternator, 0.125m),
                TransientKWVDip_15 = TransientKWVdip(alternator, 0.15m),
                TransientKWVDip_175 = TransientKWVdip(alternator, 0.175m),
                TransientKWVDip_20 = TransientKWVdip(alternator, 0.2m),
                TransientKWVDip_225 = TransientKWVdip(alternator, 0.225m),
                TransientKWVDip_25 = TransientKWVdip(alternator, 0.25m),
                TransientKWVDip_275 = TransientKWVdip(alternator, 0.275m),
                TransientKWVDip_30 = TransientKWVdip(alternator, 0.3m),
                TransientKWVDip_325 = TransientKWVdip(alternator, 0.325m),
                TransientKWVDip_35 = TransientKWVdip(alternator, 0.35m),
                SKVAMultiplier = _skvaMultiplier
            };
        }

        private decimal GetKWDerated(Alternator alternator)
        {
            decimal frequencyMultiplier = 1;

            if (int.Parse(_solutionSetup.Frequency.Value) == 50)
                frequencyMultiplier = 5 / 6;

            return alternator.KWRating;
        }

        private decimal TransientKWVdip(Alternator alternator, decimal vDip)
        {
            var multiplier = _skvaMultiplier;
            int rounded;

            if (vDip <= 0.15m)
            {
                rounded = Convert.ToInt32((vDip / 0.15m * alternator.Percent15 * (decimal)multiplier) * 10);
                return rounded / 10;
            }
            else if (vDip <= 0.25m)
            {
                decimal[] firstSeries = { 0.25m, 0.15m };
                double[] secondSeries = { alternator.Percent25, alternator.Percent15 };
                rounded = Convert.ToInt32(Statistics.lRegression(firstSeries, secondSeries, (double)vDip) * multiplier * 10);
                return rounded / 10;
            }
            else
            {
                decimal[] firstSeries = { 0.35m, 0.25m };
                double[] secondSeries = { alternator.Percent35, alternator.Percent25 };
                rounded = Convert.ToInt32(Statistics.lRegression(firstSeries, secondSeries, (double)vDip) * multiplier * 10);
                return rounded / 10;
            }
        }

        private double GetSKVAMultiplier()
        {
            decimal sKVA_50 = 1;
            decimal sKVA_50_OverExcited = 1;
            decimal sKVA_50_UnderExcited = 1;
            decimal sKVA_50_WayUnderExcited = 1;

            decimal b52 = 1, b53 = 0.95m, b54 = 1, b55 = 0.95m;
            decimal generalMultiplier;
            double voltageRatio = Convert.ToDouble(decimal.Parse(_solutionSetup.VoltageSpecific.Value) / decimal.Parse(_solutionSetup.VoltageNominal.Value));

            if (int.Parse(_solutionSetup.Frequency.Value) != 60)
                sKVA_50 = decimal.Parse(_solutionSetup.Frequency.Value) / 60 * b52;

            if (voltageRatio > 1.05)
                sKVA_50_OverExcited = (decimal)(Math.Pow(voltageRatio, 2)) * b53;

            if (voltageRatio > 0.85 && voltageRatio < 0.95)
                sKVA_50_UnderExcited = b54;

            if (voltageRatio <= 0.85)
                sKVA_50_WayUnderExcited = b55;

            generalMultiplier = (decimal)Math.Pow(voltageRatio, 2);

            return Convert.ToDouble(sKVA_50 * sKVA_50_OverExcited * sKVA_50_UnderExcited * sKVA_50_WayUnderExcited * generalMultiplier);
        }

        private decimal GetSubtransientReactanceCorrected(Alternator alternator)
        {
            return alternator.SubTransientReactance / (decimal)_skvaMultiplier;
        }

        private decimal GetSubtransientReactance1000(Alternator alternator)
        {
            return 1000 * alternator.SubTransientReactance / (decimal)(alternator.KVABase * _skvaMultiplier);
        }

        #endregion


        #endregion
    }
}
