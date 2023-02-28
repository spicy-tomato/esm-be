using System.Data;
using ESM.Data.Enums;
using ESM.Data.Models;

namespace ESM.Common.Core.Helpers;

public static class ExaminationHelper
{
    public static int CalculateExamsNumber(ExaminationData examination)
    {
        if (examination.Method != ExamMethod.Write)
            return 0;
        if (examination.CandidatesCount == null)
            throw new NoNullAllowedException();
        
        var shouldRoundUp = examination.CandidatesCount.Value % 5 != 0;
        return (examination.CandidatesCount.Value / 5 + (shouldRoundUp ? 1 : 0)) * 5;
    }
    
    public static int CalculateInvigilatorNumber(ExaminationData examination)
    {
        return examination.CandidatesCount < 80 ? 2 : 3;
    }
}