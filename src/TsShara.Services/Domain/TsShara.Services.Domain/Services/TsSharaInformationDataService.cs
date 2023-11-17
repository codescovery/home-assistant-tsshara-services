using System.Globalization;
using System.IO.Ports;
using Microsoft.Extensions.Logging;
using TsShara.Services.Domain.Entities;
using TsShara.Services.Domain.Enum;
using TsShara.Services.Domain.Interfaces;
using static System.Decimal;

namespace TsShara.Services.Domain.Services;

internal class TsSharaInformationDataService: ITsSharaInformationDataService
{
    private readonly ILogger<TsSharaInformationDataService> _logger;

    public TsSharaInformationDataService(ILogger<TsSharaInformationDataService> logger)
    {
        _logger = logger;
    }

    public ITsSharaInformation? Create(SerialPort serialPort, ITsSharaInformationDataRaw dataRaw)
    {
        if (string.IsNullOrWhiteSpace(dataRaw.RawData)) return null;
        var spitedData = dataRaw.RawData.Replace("(", string.Empty).Split(" ");
        if (spitedData.Length <= 7)
            return null;
        var provider = CultureInfo.InvariantCulture;
        var data = new TsSharaInformationData(dataRaw)
        {
            In = GetIn(spitedData, provider),
            Out = GetOut(spitedData, provider),
            Frequency = GetFrequency(spitedData, provider),
            Temperature = GetTemperature(spitedData, provider),
            PowerSource = GetPowerSource(spitedData),
            Battery = GetBatteryStatus(spitedData, provider),
            UpdateTime = DateTime.UtcNow
        };
        return data;
    }


    public ITsSharaInformation? Create(SerialPort serialPort, Exception ex)
    {
        return new TsSharaInformationError(serialPort, ex);
    }

    public ITsSharaInformationDataRaw Create(string deviceInfo, SerialPort serialPort,string rawData)
    {
        return new TsSharaInformationDataRaw(serialPort.PortName, deviceInfo, rawData);
    }

    public ITsSharaInformationDataRaw Create(SerialPort serialPort, string rawData)
    {
        return Create(serialPort.PortName, serialPort, rawData);
    }


    private decimal? GetBatteryStatus(string[] data, CultureInfo provider)
    {
        try
        {
            if (data.Length <= 5) return null;
            var voltageRaw = CalculateVoltageRaw(data);
            var batteryPercentage = CalculateBatteryPercentage(voltageRaw);
            var result = batteryPercentage switch
            {
                >= 100M => 100,
                >= 0M => Math.Round(Convert.ToDecimal(batteryPercentage, provider)),
                _ => 0
            };
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error while getting battery status");
            return null;
        }
    }

    private  decimal CalculateVoltageRaw(string[] data)
    {
        try
        {
            var voltageRaw = Math.Round(Parse(data[5]), 2);
            if (voltageRaw > 17M)
                voltageRaw -= 10M;
            return voltageRaw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error while getting voltage raw");
            return 0;
        }
    }

    private  decimal CalculateBatteryPercentage(decimal voltageRaw)
    {
        return (voltageRaw - 10M) * 100M / 3M;
    }



    private PowerSource? GetPowerSource(string[] data)
    {
        try
        {
            if (data.Length <= 7) return null;
            var powerSourceString = data[7][..1];
            var powerSource = Enum.PowerSource.None;
            powerSource = powerSourceString switch
            {
                "0" => Enum.PowerSource.Network,
                "1" => Enum.PowerSource.Battery,
                _ => powerSource
            };
            return powerSource;
        }
        catch (Exception ex)
        {

            _logger.LogError(ex, "Error while getting power Soucer");
            return null;
        }
    }


    private decimal? GetFrequency(string[] data, CultureInfo provider)
    {
        try
        {
            var @frequency = Math.Round(Convert.ToDecimal(data[4], provider));
            var result = @frequency is > 0 and < 5 ? 0 : @frequency;
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting frequency");
            return null;
        }
    }

    private decimal? GetOut(string[] data, CultureInfo provider)
    {

        try
        {
            var @out = Math.Round(Convert.ToDecimal(data[2], provider));
            var result = @out is > 0 and < 5 ? 0 : @out;
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting out");
            return null;
        }
    }

    private decimal GetIn(string[] data, CultureInfo provider)
    {
        try
        {
            var @in = Math.Round(Convert.ToDecimal(data[0], provider));
            var result = @in is > 0 and < 5 ? 0 : @in;
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting in");
            return 0;
        }
    }

    private decimal? GetTemperature(string[] data, CultureInfo provider)
    {
        try
        {
            if (data.Length <= 6) return null;
            var result = !string.IsNullOrWhiteSpace(data[6])
                ? Math.Round(Convert.ToDecimal(data[6], provider))
                : (decimal?)null;
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting temperature");
            return null;
        }

    }
}