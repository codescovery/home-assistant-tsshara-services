using Codescovery.Library.Commons.Extensions;
using TsShara.Services.Domain.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TsShara.Services.Domain.Entities;

internal class TsSharaInformationResult:ITsSharaInformationResult
{
    public TsSharaInformationResult(ITsSharaInformation? data)
    {
        Data = data;
    }

    public ITsSharaInformation? Data { get; }

    public ITsSharaInformationError? AsError()
    {
        try
        {
            return Data as ITsSharaInformationError;
        }
        catch
        {
            return null;
        }
    }

    public ITsSharaInformationDataRaw? AsDataRaw()
    {
        try
        {
            return Data as ITsSharaInformationDataRaw;
        }
        catch
        {
            return null;
        }
    }

    public ITsSharaInformationData? AsData()
    {
        try
        {
            return Data as ITsSharaInformationData;
        }
        catch
        {
            return null;
        }
    }

    public bool IsType<T>()
    {
        try
        {
          var result = Data is T;
          return result;

        }
        catch
        {
            return false;
        }
    }


}