using ESM.Data.Enums;

namespace ESM.Common.Core.Helpers;

public static class ExaminationHelper
{
    public static int CalculateExamsNumber(ExamMethod? method, int candidatesNumberInShift)
    {
        if (method != ExamMethod.Write)
            return 0;
        
        var shouldRoundUp = candidatesNumberInShift % 5 != 0;
        return (candidatesNumberInShift / 5 + (shouldRoundUp ? 1 : 0)) * 5;
    }
    
    public static int CalculateInvigilatorNumber(int candidatesCount)
    {
        return candidatesCount < 80 ? 2 : 3;
    }
}